using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using GeekBudget.Entities;
using GeekBudget.Helpers;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;
using GeekBudget.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Microsoft.VisualStudio.Web.CodeGeneration.Templating;

namespace GeekBudget.Services.Implementations
{
    public class OperationService : IOperationService 
    {
        private static Error AddOperationNotAllowed() =>
            new Error {Id = 2201, Description = $"Can't add operation with that tab target types!"};
        private static Error NoOperationWithId(int id) =>  
            new Error{Id = 2202, Description = $"No Operation with id '{id}' was found!"};        
        private static Error ChangeOperationNotAllowed() =>  
            new Error{Id = 2203, Description = $"Can't add operation with that tab target types!"};        

        private readonly IGeekBudgetContext _context;
        private readonly ITabService _tabService;
        private readonly IMappingService _mappingService;
  
        public OperationService(IGeekBudgetContext context, ITabService tabService, IMappingService mappingService)
        {
            _context = context;
            _tabService = tabService;
            _mappingService = mappingService;
        }

        public async Task<ServiceResult<IEnumerable<Operation>>> GetAll()
        {
            var operations = await _context.Operations
                .AsNoTracking()
                .ToListAsync();
            
            return new ServiceResult<IEnumerable<Operation>>(operations);
        }

        public async Task<ServiceResult<IEnumerable<Operation>>> Get(OperationFilter filter)
        {
            var operations = await _context.Operations
                .AsNoTracking()
                .ApplyFilter(filter)
                .Include(x => x.From)
                .Include(x => x.To)
                .ToListAsync();
            
            return new ServiceResult<IEnumerable<Operation>>(operations);
        }

        public async Task<ServiceResult<int>> Add(OperationViewModel vm)
        {
            var operation = _mappingService.Map(vm);
            
            // Get 'From' tab
            var resultFrom = await _tabService.Get(vm.From ?? -1);
            if(resultFrom.Failed)
                return ServiceResult<int>.From(resultFrom);
            
            var tabFrom = resultFrom.Data;
            
            // Get 'To' tab
            var resultTo = await _tabService.Get(vm.To ?? -1);
            if(resultTo.Failed)
                return ServiceResult<int>.From(resultFrom);
            
            var tabTo = resultTo.Data;

            // Check if operation from tab to tab is allowed
            var resultOperationAllowed = await _tabService.IsTabOperationAllowed(tabFrom, tabTo);
            if(resultOperationAllowed.Failed)
                return ServiceResult<int>.From(resultOperationAllowed);
            
            if(!resultOperationAllowed.Data)
                return new ServiceResult<int>(Enums.ServiceResultStatus.Failure, AddOperationNotAllowed());

            using (var scope = new TransactionScope())
            {
                // Add operation to 'From' tab
                operation.From = tabFrom;

                var resultAddFrom = await _tabService.AddOperation(tabFrom.Id, operation, Enums.TargetTabType.From);
                if(resultAddFrom.Failed)
                    return ServiceResult<int>.From(resultAddFrom);
            
                // Add operation to 'To' tab
                operation.To = tabTo;
            
                var resultAddTo = await _tabService.AddOperation(tabTo.Id, operation, Enums.TargetTabType.To);
                if(resultAddTo.Failed)
                    return ServiceResult<int>.From(resultAddTo);
            
                // Add operation to context
                var newOperation = _context.Operations.Add(operation).Entity;
            
                // Save changes
                await _context.SaveChangesAsync();
               
                // Commit changes
                scope.Complete();

                return new ServiceResult<int>(newOperation.Id);
            }
        }
        
        public async Task<ServiceResult> Remove(int id)
        {
            var resultGet = await Get(new OperationFilter {Id = id});
            if(resultGet.Failed)
                return ServiceResult.From(resultGet);

            var operation = resultGet.Data.FirstOrDefault();
            if(operation == null)
                return new ServiceResult(Enums.ServiceResultStatus.Warning, NoOperationWithId(id));
     
            using (var scope = new TransactionScope())
            {
                // Update from tab
                RemoveFromTab(operation);
                
                // Update to tab
                RemoveToTab(operation);
                
                _context.Operations.Remove(operation);  
                
                await _context.SaveChangesAsync();
                
                // Commit changes
                scope.Complete();
            }

            return new ServiceResult(Enums.ServiceResultStatus.Success);
        }

        public async Task<ServiceResult> Update(OperationViewModel vm)
        {
            var result = await Get(new OperationFilter {Id = vm.Id});

            if (result.Failed)
                return ServiceResult.From(result);

            var operation = result.Data.FirstOrDefault();
            if (operation == null)
                return new ServiceResult(Enums.ServiceResultStatus.Failure, NoOperationWithId(vm.Id));

            var source = _mappingService.Map(vm);

            using (var scope = new TransactionScope())
            {
                // --- Amount update ---
                
                if (vm.Amount != null)
                {
                    var newAmount = (decimal) vm.Amount;

                    //difference with previous amount
                    var delta = newAmount - operation.Amount;

                    //Update tab amounts
                    operation.From.Amount -= delta;
                    operation.To.Amount += delta;

                    // Update amount
                    operation.Amount = newAmount;
                }

                // --- New values ---
                
                operation.MapNewValues(source,
                    x => x.Comment,
                    x => x.Currency,
                    x => x.Date);
                
                // --- Tab updates ---

                var fromTabChange = vm.From != null && vm.From != operation.From.Id;
                var toTabChange = vm.To != null && vm.To != operation.To.Id;

                var tabFrom = operation.From;
                var tabTo = operation.From;

                if (fromTabChange)
                {
                    var resultFrom = await _tabService.Get(vm.From ?? -1);
                    if(resultFrom.Failed)
                        return ServiceResult.From(resultFrom);
            
                    tabFrom = resultFrom.Data;
                }
                
                if (toTabChange)
                {
                    var resultTo = await _tabService.Get(vm.To ?? -1);
                    if(resultTo.Failed)
                        return ServiceResult.From(resultTo);
            
                    tabTo = resultTo.Data;
                }
                 
                // Check if operation from tab to tab is allowed
                var resultOperationAllowed = await _tabService.IsTabOperationAllowed(tabFrom, tabTo);
                if(resultOperationAllowed.Failed)
                    return ServiceResult.From(resultOperationAllowed);
            
                if(!resultOperationAllowed.Data)
                    return new ServiceResult(Enums.ServiceResultStatus.Failure, ChangeOperationNotAllowed());

                
                if (fromTabChange)
                {
                    RemoveFromTab(operation);
                    AddFromTab(tabFrom, operation);
                }
                
                if (toTabChange)
                {
                    RemoveFromTab(operation);
                    AddFromTab(tabFrom, operation);
                }

                await _context.SaveChangesAsync();
                
                scope.Complete();

                return new ServiceResult(Enums.ServiceResultStatus.Success);
            }
        }

        private static void RemoveFromTab(Operation operation)
        {
            var tab = operation.From;
            tab.Amount += operation.Amount;
            tab.OperationsFrom.Remove(operation);
        }
        
        private static void RemoveToTab(Operation operation)
        {
            var tab = operation.To;
            tab.Amount -= operation.Amount;
            tab.OperationsTo.Remove(operation);
        }

        private static void AddFromTab(Tab tab, Operation operation)
        {
            tab.Amount -= operation.Amount;
            operation.From = tab;
            tab.OperationsFrom.Add(operation);
        }
        
        private static void AddToTab(Tab tab, Operation operation)
        {
            tab.Amount += operation.Amount;
            operation.To = tab;
            tab.OperationsTo.Add(operation);
        }
    }
}