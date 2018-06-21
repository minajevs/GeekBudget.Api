using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Entities;

namespace GeekBudget.Models.ViewModels
{
    public class TabViewModel
    {
        public int Id { get; set; }
        public TabType? Type { get; set; }
        public string Name { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; } = "EUR";

        public Tab MapToEntity()
        {
            return new Tab()
            {
                Name = this.Name,
                Amount = this.Amount ?? 0,
                Currency = this.Currency,
                Type = this.Type ?? 0
            };
        }

        public TabViewModel MapFromEntity(Tab entity)
        {
            this.Id = entity.Id;
            this.Name = entity.Name;
            this.Amount = entity.Amount;
            this.Currency = entity.Currency;
            this.Type = entity.Type;
            return this;
        }

        public static TabViewModel FromEntity(Tab entity)
        {
            return new TabViewModel().MapFromEntity(entity);
        }
    }
}
