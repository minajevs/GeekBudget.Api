using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBudget.Models.ViewModels
{
    public class TabViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; } = "EUR";

        public Tab MapToEntity()
        {
            return new Tab()
            {
                Name = this.Name,
                Amount = this.Amount ?? 0,
                Currency = this.Currency
            };
        }

        public TabViewModel MapFromEntity(Tab entity)
        {
            this.Id = entity.Id;
            this.Name = entity.Name;
            this.Amount = entity.Amount;
            this.Currency = entity.Currency;
            return this;
        }

        public static TabViewModel FromEntity(Tab entity)
        {
            return new TabViewModel().MapFromEntity(entity);
        }
    }
}
