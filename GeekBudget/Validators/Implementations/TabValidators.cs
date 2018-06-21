using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Entities;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GeekBudget.Validators.Implementations
{
    public class TabValidators : ITabValidators
    {
        private static Error TabViewModelIsNull() =>  new Error{Id = 100, Description = $"TabViewModel is null!"};
        private static Error NoTabWithId(int id) =>  new Error{Id = 101, Description = $"No Tab with id '{id}' was found!"};
        private static Error TabTypeIsRequired() =>  new Error{Id = 102, Description = $"Tab Type is required!"};

        private readonly IGeekBudgetContext _context;
        
        public TabValidators(IGeekBudgetContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Error>> IdExists(TabViewModel tab)
        {
            var errors = new List<Error>();
           
            if (!await _context.Tabs.AnyAsync(t => Equals(t.Id, tab.Id)))
                errors.Add(NoTabWithId(tab.Id));

            return await Task.FromResult(errors);
        }   
        
        public async Task<IEnumerable<Error>> TabTypeRequired(TabViewModel tab)
        {
            var errors = new List<Error>();

            if (tab.Type == null)
                errors.Add(TabTypeIsRequired());
            
            return await Task.FromResult(errors);
        }
        
        public async Task<IEnumerable<Error>> NotNull(TabViewModel tab)
        {
            var errors = new List<Error>();

            if (tab == null)
                errors.Add(TabViewModelIsNull());
            
            return await Task.FromResult(errors);
        }
    }
}