using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Models.ViewModels;
using Newtonsoft.Json;

namespace GeekBudget.Models
{
    public class Tab
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EUR";
        public List<Operation> Operations => OperationsFrom.Concat(OperationsTo).Distinct().ToList();
        public List<Operation> OperationsFrom { get; set; } = new List<Operation>();
        public List<Operation> OperationsTo { get; set; } = new List<Operation>();

        public Tab MapNewValues(TabViewModel values)
        {
            this.Name = values.Name ?? this.Name;
            this.Amount = values.Amount ?? this.Amount;
            this.Currency = values.Currency ?? this.Currency;

            return this;
        }

        public void AddNewOperation(Operation operation)
        {
            if (this.Id == operation.From.Id)
            {
                if (this.OperationsFrom.Any(o => o.Id == operation.Id))
                    throw new InvalidOperationException("Can't add same operation twice!");
                if (operation.Currency == this.Currency) //TODO: Add currency conversion
                    this.Amount -= operation.Amount;
                else 
                    throw new NotImplementedException("Currency conversion is not supported yet!");
                this.OperationsFrom.Add(operation);
            }
            else if (this.Id == operation.To.Id)
            {
                if (this.OperationsTo.Any(o => o.Id == operation.Id))
                    throw new InvalidOperationException("Can't add same operation twice!");
                if (operation.Currency == this.Currency) //TODO: Add currency conversion
                    this.Amount += operation.Amount;
                else
                    throw new NotImplementedException("Currency conversion is not supported yet!");
                this.OperationsTo.Add(operation);
            }
            else
            {
                throw new ArgumentException("Wrong ID of operation passed to 'AddNewOperation' method!");
            }
        }

        public void RemoveOperation(Operation operation)
        {
            var operationExist = this.Operations.Any(o => o.Id == operation.Id);
            if (!operationExist)
            {
                throw new ArgumentException("Can't remove non-existing operation!");
            }

            if (this.Id == operation.From.Id)
            {
                if (operation.Currency == this.Currency) //TODO: Add currency conversion
                    this.Amount += operation.Amount;
                else
                    throw new NotImplementedException("Currency conversion is not supported yet!");
                this.OperationsFrom.RemoveAll(o => o.Id == operation.Id);
            }
            else if (this.Id == operation.To.Id)
            {
                if (operation.Currency == this.Currency) //TODO: Add currency conversion
                    this.Amount -= operation.Amount;
                else
                    throw new NotImplementedException("Currency conversion is not supported yet!");
                this.OperationsTo.RemoveAll(o => o.Id == operation.Id);
            }
        }
    }
}
