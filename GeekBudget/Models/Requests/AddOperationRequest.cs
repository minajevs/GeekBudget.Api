using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Helpers;
using GeekBudget.Models.ViewModels;
using GeekBudget.Validators;

namespace GeekBudget.Models.Requests
{
    public class AddOperationRequest : IServiceRequest<OperationVm>
    {
        public string Comment { get; private set; }
        public decimal Amount { get; private set; }
        public string Currency { get; private set; } = "EUR";
        public int From { get; private set; }
        public int To { get; private set; }
        public DateTime Date { get; private set; } = DateTime.Now;

        public List<Error> ValidateAndMap(OperationVm vm)
        {
            var errors = new List<Error>();

            if (vm.From == null)
                errors.Add(Errors.OperationFromIsRequired);

            if (vm.To == null)
                errors.Add(Errors.OperationToIsRequired);

            if (vm.Amount == null)
                errors.Add(Errors.OperationAmountIsRequired);

            if (errors.Any())
                return errors;

            this.MapNewValues(vm,
                (x => x.Comment, y => y.Comment),
                (x => x.Amount, y => y.Amount),
                (x => x.Currency, y => y.Currency),
                (x => x.From, y => y.From),
                (x => x.To, y => y.To),
                (x => x.Date, y => y.Date)
            );

            return errors;
        }
    }
}
