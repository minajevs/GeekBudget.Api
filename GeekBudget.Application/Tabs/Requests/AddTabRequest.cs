using System.Collections.Generic;
using GeekBudget.Core;
using GeekBudget.Core.Helpers;
using GeekBudget.Domain.Tabs;

namespace GeekBudget.Application.Tabs.Requests
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
                errors.Add(TabErrors.TabNameIsRequired);

            if (vm.Type == null)
                errors.Add(TabErrors.TabTypeIsRequired);

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
