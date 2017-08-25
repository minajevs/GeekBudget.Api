using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBudget.Models
{
    public class Tab
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; } = "EUR";
        public IEnumerable<Operation> Operations { get; set; }

        public Tab MapNewValues(Tab values)
        {
            this.Name = values.Name ?? this.Name;
            this.Amount = values.Amount ?? this.Amount;
            this.Currency = values.Currency ?? this.Currency;

            return this;
        }
    }
}
