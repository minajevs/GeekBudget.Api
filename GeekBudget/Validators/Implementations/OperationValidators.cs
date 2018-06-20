using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Entities;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GeekBudget.Validators.Implementations
{
    public class OperationValidators : IOperationValidators
    {
        private static Error OperationViewModelIsNull() =>  new Error{Id = 200, Description = $"OperationViewModel is null!"};
        private static Error FromIsNull() =>  new Error{Id = 201, Description = $"'From' id can't be null!"};
        private static Error ToIsNull() =>  new Error{Id = 202, Description = $"'To' id can't be null!"};
        private static Error FromAndToAreEqual() =>  new Error{Id = 203, Description = $"'From' tab and 'To' tab should not be same!"};
        private static Error NoOperationWithId(int id) =>  new Error{Id = 203, Description = $"No Operation with id '{id}' was found!"};
        private static Error TabTypeIsRequired() =>  new Error{Id = 102, Description = $"Tab Type is required!"};
        
        private readonly IGeekBudgetContext _context;
        private readonly ITabValidators _tabValidators;
        
        public OperationValidators(IGeekBudgetContext context, ITabValidators tabValidators)
        {
            _context = context;
            _tabValidators = tabValidators;
        }
        
        public async Task<IEnumerable<Error>> FromNotNull(OperationViewModel operation)
        {
            var errors = new List<Error>();

            if(operation.From == null)
                errors.Add(FromIsNull());

            return await Task.FromResult(errors);
        }   
        
        public async Task<IEnumerable<Error>> ToNotNull(OperationViewModel operation)
        {
            var errors = new List<Error>();

            if(operation.To == null)
                errors.Add(ToIsNull());

            return await Task.FromResult(errors);
        }
        
        public async Task<IEnumerable<Error>> FromAndToAreNotEqual(OperationViewModel operation)
        {
            var errors = new List<Error>();

            if(operation.From == null || operation.To == null)
                return await Task.FromResult(errors);
            
            if(operation.From == operation.To)
                errors.Add(FromAndToAreEqual());

            return await Task.FromResult(errors);
        }
        
        public async Task<IEnumerable<Error>> FromTabExists(OperationViewModel operation)
        {
            var errors = new List<Error>();

            // 'From Id' should be validated to be not null before!
            // ReSharper disable once PossibleInvalidOperationException 
            errors.AddRange(await _tabValidators.IdExists(new TabViewModel{Id = operation.From.Value}));

            return errors;
        }
        
        public async Task<IEnumerable<Error>> ToTabExists(OperationViewModel operation)
        {
            var errors = new List<Error>();

            // 'To Id' should be validated to be not null before!
            // ReSharper disable once PossibleInvalidOperationException 
            errors.AddRange(await _tabValidators.IdExists(new TabViewModel{Id = operation.To.Value}));

            return errors;
        }
        
        public async Task<IEnumerable<Error>> NotNull(OperationViewModel operation)
        {
            var errors = new List<Error>();

            if (operation == null)
                errors.Add(OperationViewModelIsNull());
            
            return await Task.FromResult(errors);
        }  
    }
}