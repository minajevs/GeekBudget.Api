using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Helpers;
using GeekBudget.Models.ViewModels;
using GeekBudget.Validators;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GeekBudget.Models.Requests
{
    public class UpdateOperationRequest : IServiceRequest<OperationVm>
    {
        public UpdateOperationRequest(int id)
        {
            Id = id;
        }
        public int Id { get; private set; }
        public string Comment { get; private set; }
        public decimal? Amount { get; private set; }
        public string Currency { get; private set; } = "EUR";
        public int? From { get; private set; }
        public int? To { get; private set; }
        public DateTime Date { get; private set; } = DateTime.Now;

        public List<Error> ValidateAndMap(OperationVm vm)
        {
            var errors = new List<Error>();

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
