using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Entities;
using GeekBudget.Helpers;
using GeekBudget.Models.ViewModels;
using GeekBudget.Validators;

namespace GeekBudget.Models.Requests
{
    public class AddTabRequest : IServiceRequest<TabVm>
    {
        public int Type { get; private set; }
        public decimal Amount { get; private set; } = 0;
        public string Name { get; private set; }
        public string Currency { get; private set; } = "EUR";

        public List<Error> ValidateAndMap(TabVm vm)
        {
            var errors = new List<Error>();

            if(vm.Name == null)
                errors.Add(Errors.TabNameIsRequired);

            if (vm.Type == null)
                errors.Add(Errors.TabTypeIsRequired);

            this.MapNewValues(vm,
                (x => x.Type, y => y.Type),
                (x => x.Name, y => y.Name),
                (x => x.Currency, y => y.Currency),
                (x => x.Amount, y => y.Amount)
                );

            return errors;
        }
    }
}
