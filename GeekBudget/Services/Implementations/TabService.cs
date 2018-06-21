using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using GeekBudget.Entities;
using GeekBudget.Helpers;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeekBudget.Services.Implementations
{
    public class TabService : ITabService
    {
        private static Error NoTabWithId(int id) =>
            new Error {Id = 1101, Description = $"No Tab with id '{id}' was found!"};
        
        private static Error OperationAlreadyExists(int operationId, int tabId) =>
            new Error {Id = 1102, Description = $"Operation with id '{operationId}' already exists on Tab with id '{tabId}'!"};

        private readonly IGeekBudgetContext _context;

        public TabService(IGeekBudgetContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<IEnumerable<Tab>>> GetAll()
        {
            var tabs = await _context.Tabs
                .AsNoTracking()
                .ToListAsync();

            return new ServiceResult<IEnumerable<Tab>>(tabs);
        }

        public async Task<ServiceResult<Tab>> Get(int id)
        {
            var tab = await _context.Tabs
                .AsNoTracking()
                .SingleOrDefaultAsync(x => Equals(x.Id, id));

            return new ServiceResult<Tab>(tab);
        }

        public async Task<ServiceResult<int>> Add(Tab tab)
        {
            var addedTab = _context.Tabs
                .Add(tab);

            await _context.SaveChangesAsync();

            return new ServiceResult<int>(addedTab.Entity.Id);
        }

        public async Task<ServiceResult> Remove(int id)
        {
            var tab = await _context.Tabs
                .AsNoTracking()
                .FirstOrDefaultAsync(t => Equals(t.Id, id));

            if (tab == null)
                return new ServiceResult(ServiceResultStatus.Warning, NoTabWithId(id));

            _context.Tabs.Remove(tab);

            await _context.SaveChangesAsync();

            return new ServiceResult(ServiceResultStatus.Success);
        }

        public async Task<ServiceResult> Update(int id, Tab source)
        {
            var result = await Get(id);
            
            if(result.Failed)
                return ServiceResult.From(result);
            
            var tab = result.Data;

            if (tab == null)
                return new ServiceResult(ServiceResultStatus.Failure, NoTabWithId(id));

            tab.MapNewValues(source,
                x => x.Name,
                x => x.Amount,
                x => x.Currency,
                x => x.Type);

            await _context.SaveChangesAsync();

            return new ServiceResult(ServiceResultStatus.Success);
        }

        public Task<ServiceResult<bool>> IsTabOperationAllowed(Tab tabFrom, Tab tabTo)
        {
            var tabOperationAllowed = Dictionaries.AllowedTabTypes[tabFrom.Type].Any(t => t == tabTo.Type);
            return Task.FromResult(new ServiceResult<bool>(tabOperationAllowed));
        }
        
        public async Task<ServiceResult> AddOperation(int id, Operation operation, TargetTabType targetType)
        {
            var result = await Get(id);
            
            if(result.Failed)
                return ServiceResult.From(result);
            
            var tab = result.Data;

            if (tab == null)
                return new ServiceResult(ServiceResultStatus.Failure, NoTabWithId(id));

            var tabOperations = targetType == TargetTabType.From
                ? tab.OperationsFrom
                : tab.OperationsTo;
            
            if(tabOperations.Any(o => o.Id == operation.Id))
                return new ServiceResult(ServiceResultStatus.Failure, OperationAlreadyExists(operation.Id, tab.Id));
            
            // TODO: implement currency check and adjust!

            var amount = targetType == TargetTabType.From
                ? -operation.Amount
                : operation.Amount;

            tab.Amount += amount;

            await _context.SaveChangesAsync();
            
            return new ServiceResult(ServiceResultStatus.Success);
        }
    }
}