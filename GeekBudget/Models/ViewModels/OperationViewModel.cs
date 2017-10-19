using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBudget.Models.ViewModels
{
    public class OperationViewModel
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; } = "EUR";
        public int? From { get; set; }
        public int? To { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public Operation MapToEntity()
        {
            return new Operation()
            {
                Comment = this.Comment,
                Amount = this.Amount ?? 0,
                Currency = this.Currency,
                Date = this.Date
            };
        }

        public OperationViewModel MapFromEntity(Operation entity)
        {
            this.Id = entity.Id;
            this.Comment = entity.Comment;
            this.Amount = entity.Amount;
            this.Currency = entity.Currency;
            this.From = entity.From.Id; //Must not be null. If it is - there is a huge problem
            this.To = entity.To.Id;     //Must not be null. If it is - there is a huge problem
            this.Date = entity.Date;

            return this;
        }

        public static OperationViewModel FromEntity(Operation entity)
        {
            return new OperationViewModel().MapFromEntity(entity);
        }
    }
}
