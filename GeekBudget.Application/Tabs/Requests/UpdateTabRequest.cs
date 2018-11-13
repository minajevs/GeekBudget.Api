using System.Collections.Generic;
using GeekBudget.Core;
using GeekBudget.Core.Helpers;
using GeekBudget.Domain.Tabs;

namespace GeekBudget.Application.Tabs.Requests
{
    public class UpdateTabRequest : IServiceRequest<TabVm>
    {
        public UpdateTabRequest(int id)
        {
            Id = id;
        }
        public int Id { get; private set; }
        public int Type { get; private set; }
        public string Name { get; private set; }
        public string Currency { get; private set; }
        public decimal? Amount { get; private set; }

        public List<Error> ValidateAndMap(TabVm vm)
        {
            var errors = new List<Error>();

            this.MapNewValues(vm,
                (x => x.Name, y => y.Name),
                (x => x.Type, y => y.Type),
                (x => x.Currency, y => y.Currency),
                (x => x.Amount, y => y.Amount)
                );

            return errors;
        }
    }
}
