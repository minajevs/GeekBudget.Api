using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Application.Operations.Requests;
using GeekBudget.Application.Tabs;
using GeekBudget.Core;
using GeekBudget.Core.Helpers;
using GeekBudget.DataAccess;
using GeekBudget.DataAccess.Operations;
using GeekBudget.DataAccess.Tabs;
using GeekBudget.Domain.Operations;
using GeekBudget.Domain.Tabs;
using Microsoft.EntityFrameworkCore;

namespace GeekBudget.Application.Operations
{
    public class OperationService : IOperationService
    {
        private readonly IGeekBudgetContext _context;
        private readonly ITabService _tabService;

        public OperationService(IGeekBudgetContext context, ITabService tabService)
        {
            _context = context;
            _tabService = tabService;
        }

        public async Task<ServiceResult<IEnumerable<Operation>>> GetAll()
        {
            var operations = await _context.Operations
                .AsNoTracking()
                .Include(x => x.From)
                .Include(x => x.To)
                .ToListAsync();

            return operations;
        }

        public async Task<ServiceResult<IEnumerable<Operation>>> Get(OperationFilter filter)
        {
            var operations = await _context.Operations
                .AsNoTracking()
                .ApplyFilter(filter)
                .Include(x => x.From)
                .Include(x => x.To)
                .ToListAsync();

            if (filter.Id != null && !operations.Any())
                return OperationErrors.OperationWithIdDoesNotExist(filter.Id ?? -1);

            return operations;
        }

        public async Task<ServiceResult> Add(AddOperationRequest request)
        {
            // Get 'From' tab
            var resultFrom = await _tabService.Get(request.From);
            if (resultFrom.Failed)
                return ServiceResult.From(resultFrom);

            var tabFrom = resultFrom.Data;
            
            // Get 'To' tab
            var resultTo = await _tabService.Get(request.To);
            if (resultTo.Failed)
                return ServiceResult.From(resultFrom);

            var tabTo = resultTo.Data;

            // Check if operation from tab to tab is allowed
            var resultOperationAllowed = await _tabService.IsTabOperationAllowed(tabFrom, tabTo);
            if (resultOperationAllowed.Failed)
                return ServiceResult.From(resultOperationAllowed);

            if (!resultOperationAllowed.Data)
                return OperationErrors.OperationNotAllowed;

            // Create domain model
            var operation = MappingFactory.Map(request);

            // Add operation to 'From' tab
            AddFromTab(tabFrom,  operation);

            // Add operation to 'To' tab
            AddToTab(tabTo, operation);

            // Add operation
            _context.Operations.Add(operation);

            // Save changes
            await _context.SaveChangesAsync();

            return ServiceResultStatus.Success;
        }

        public async Task<ServiceResult> Remove(int id)
        {
            var resultGet = await Get(new OperationFilter { Id = id });
            if (resultGet.Failed)
                return ServiceResult.From(resultGet);

            var operation = resultGet.Data.FirstOrDefault();
            if (operation == null) throw new InvalidOperationException("Must never happen because condition is checked in 'Get' service!");

            // Update from tab
            RemoveFromTab(operation);

            // Update to tab
            RemoveToTab(operation);

            _context.Operations.Remove(operation);

            await _context.SaveChangesAsync();

            return ServiceResultStatus.Success;
        }

        public async Task<ServiceResult> Update(UpdateOperationRequest request)
        {
            var resultGet = await Get(new OperationFilter { Id = request.Id });

            if (resultGet.Failed)
                return ServiceResult.From(resultGet);

            var operation = resultGet.Data.FirstOrDefault();
            if (operation == null) throw new InvalidOperationException("Must never happen because condition is checked in 'Get' service!");

            // --- Amount update ---

            if (request.Amount.HasValue)
            {
                var newAmount = (decimal)request.Amount;

                //difference with previous amount
                var delta = newAmount - operation.Amount;

                //Update tab amounts
                operation.From.Amount -= delta;
                operation.To.Amount += delta;

                // Update amount
                operation.Amount = newAmount;
            }

            // --- New values ---

            operation.MapNewValues(request,
                (x => x.Comment, y => y.Comment),
                (x => x.Currency, y => y.Currency),
                (x => x.Date, y => y.Date));

            // --- Tab updates ---

            var fromTabChange = request.From != null && request.From != operation.From.Id;
            var toTabChange = request.To != null && request.To != operation.To.Id;

            var tabFrom = operation.From;
            var tabTo = operation.From;

            if (fromTabChange)
            {
                var resultFrom = await _tabService.Get(request.From ?? -1);
                if (resultFrom.Failed)
                    return ServiceResult.From(resultFrom);

                tabFrom = resultFrom.Data;
            }

            if (toTabChange)
            {
                var resultTo = await _tabService.Get(request.To ?? -1);
                if (resultTo.Failed)
                    return ServiceResult.From(resultTo);

                tabTo = resultTo.Data;
            }

            // Check if operation from tab to tab is allowed
            var resultOperationAllowed = await _tabService.IsTabOperationAllowed(tabFrom, tabTo);
            if (resultOperationAllowed.Failed)
                return ServiceResult.From(resultOperationAllowed);

            if (!resultOperationAllowed.Data)
                return OperationErrors.OperationNotAllowed;


            if (fromTabChange)
            {
                RemoveFromTab(operation);
                AddFromTab(tabFrom, operation);
            }

            if (toTabChange)
            {
                RemoveToTab(operation);
                AddToTab(tabTo, operation);
                _context.SetModified(tabTo);
            }

            _context.SetModified(operation);

            await _context.SaveChangesAsync();

            return ServiceResultStatus.Success;
        }

        private void RemoveFromTab(Operation operation)
        {
            var tab = operation.From;
            tab.Amount += operation.Amount;
            tab.OperationsFrom.Remove(operation);
            _context.SetModified(tab);
        }

        private void RemoveToTab(Operation operation)
        {
            var tab = operation.To;
            tab.Amount -= operation.Amount;
            tab.OperationsTo.Remove(operation);
            _context.SetModified(tab);
        }

        private void AddFromTab(Tab tab, Operation operation)
        {
            tab.Amount -= operation.Amount;
            operation.From = tab;
            tab.OperationsFrom.Add(operation);
            _context.SetModified(tab);
        }

        private void AddToTab(Tab tab, Operation operation)
        {
            tab.Amount += operation.Amount;
            operation.To = tab;
            tab.OperationsTo.Add(operation);
            _context.SetModified(tab);
        }
    }
}