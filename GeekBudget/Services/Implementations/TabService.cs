using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using GeekBudget.Entities;
using GeekBudget.Helpers;
using GeekBudget.Models;
using GeekBudget.Models.Requests;
using GeekBudget.Models.ViewModels;
using GeekBudget.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeekBudget.Services.Implementations
{
    public class TabService : ITabService
    {
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

            return tabs;
        }

        public async Task<ServiceResult<Tab>> Get(int id)
        {
            var tab = await _context.Tabs
                .AsNoTracking()
                .SingleOrDefaultAsync(x => Equals(x.Id, id));

            if (tab == null)
                return Errors.TabWithIdDoesNotExist(id);

            return tab;
        }

        public async Task<ServiceResult<int>> Add(AddTabRequest request)
        {
            var tab = MappingFactory.Map(request);

            var addedTab = _context.Tabs
                .Add(tab);

            await _context.SaveChangesAsync();

            return addedTab.Entity.Id;
        }

        public async Task<ServiceResult> Remove(int id)
        {
            var tab = await _context.Tabs
                .AsNoTracking()
                .FirstOrDefaultAsync(t => Equals(t.Id, id));

            if (tab == null)
                return new ServiceResult(ServiceResultStatus.Warning, Errors.TabWithIdDoesNotExist(id));

            _context.Tabs.Remove(tab);

            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<ServiceResult> Update(UpdateTabRequest request)
        {
            var result = await Get(request.Id);
            if(result.Failed)
                return ServiceResult.From(result);

            var tab = result.Data;

            tab.MapNewValues(request,
                (x => x.Name, y => y.Name),
                (x => x.Amount, y => y.Amount),
                (x => x.Currency, y => y.Currency),
                (x => x.Type, y => y.Type)
                );

            await _context.SaveChangesAsync();

            return null;
        }

        public Task<ServiceResult<bool>> IsTabOperationAllowed(Tab tabFrom, Tab tabTo)
        {
            var tabOperationAllowed = Dictionaries.AllowedTabTypes[tabFrom.Type].Any(t => t == tabTo.Type);
            return Task.FromResult((ServiceResult<bool>)tabOperationAllowed);
        }
        
        public async Task<ServiceResult> AddOperation(int id, Operation operation, TargetTabType targetType)
        {
            var result = await Get(id);
            
            if(result.Failed)
                return ServiceResult.From(result);
            
            var tab = result.Data;

            var tabOperations = targetType == TargetTabType.From
                ? tab.OperationsFrom
                : tab.OperationsTo;
            
            if(tabOperations.Any(o => o.Id == operation.Id))
                return Errors.OperationAlreadyExist(operation.Id, tab.Id);
            
            // TODO: implement currency check and adjust!

            var amount = targetType == TargetTabType.From
                ? -operation.Amount
                : operation.Amount;

            tab.Amount += amount;

            await _context.SaveChangesAsync();
            
            return null;
        }
    }
}