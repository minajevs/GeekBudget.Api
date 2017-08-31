using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBudget.Models
{
    public class OperationFilter
    {
        public int? Id { get; set; }
        public string Comment { get; set; }
        public MinMaxFilter<decimal> Amount { get; set; }
        public string Currency { get; set; }
        public int? From { get; set; }
        public int? To { get; set; }
        public MinMaxFilter<DateTime> Date { get; set; }

        public IQueryable<Operation> CreateQuery(IQueryable<Operation> operationsSet)
        {
            var query = operationsSet;

            if (this.Id != null)
                query = query.Where(x => x.Id == this.Id);

            if (this.Comment != null)
                query = query.Where(x => x.Comment.IndexOf(this.Comment, StringComparison.OrdinalIgnoreCase) >= 0); //Comment contains TODO: make more simple search with allowed errors

            if (this.Amount != null)
            { //TODO: Add possibility to add nullable Min/Max values
                decimal min = this.Amount.Min;
                decimal max = this.Amount.Max;
                query = query.Where(x => x.Amount >= min);
                query = query.Where(x => x.Amount <= max);
            }

            if (this.Currency != null)
                query = query.Where(x => x.Currency == this.Currency); //TODO: must work with any case?

            if (this.From != null)
                query = query.Where(x => x.From == this.From);

            if (this.To != null)
                query = query.Where(x => x.To == this.To);

            if (this.Date != null) //TODO: Add possibility to add nullable Min/Max values
                query = query.Where(x =>
                    x.Date >= this.Date.Min &&
                    x.Date <= this.Date.Max);

            return query;
        }
    }
}
