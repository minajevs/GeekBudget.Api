using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Entities;
using GeekBudget.Models.ViewModels;

namespace GeekBudget.Models
{
    public class Operation
    {
        [Key]
        public int Id { get; set; }
        public string Comment { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EUR";
        public Tab From { get; set; }
        public Tab To { get; set; }
        public DateTime Date { get; set; }

        public Operation MapNewValues(OperationViewModel value)
        {
            this.Comment = value.Comment ?? this.Comment;
            this.Currency = value.Currency ?? this.Currency; //TODO: Add currency conversion
            this.Date = value.Date;

            this.UpdateAmount(value.Amount);

            return this;
        }

        private void UpdateAmount(decimal? amount)
        {
            if(amount == null) return;
            decimal newAmount = (decimal) amount;

            //difference with previous amount
            decimal delta = newAmount - this.Amount;

            //Update tab amounts
            this.From.Amount -= delta;
            this.To.Amount += delta;

            //Assing new value
            this.Amount = newAmount;
        }

        public Operation UpdateTab(Enums.TargetTabType type, Tab tab)
        {
            if (tab == null) return this;
            switch (type)
            {
                case Enums.TargetTabType.From:
                    this.From?.RemoveOperation(this);
                    this.From = tab;
                    this.From.AddNewOperation(type, this);
                    break;
                case Enums.TargetTabType.To:
                    this.To?.RemoveOperation(this);
                    this.To = tab;
                    this.To.AddNewOperation(type, this);
                    break;
            }

            return this;
        }
    }
}
