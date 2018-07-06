using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBudget.Models.ViewModels
{
    public class OperationVm
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; } = "EUR";
        public int? From { get; set; }
        public int? To { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
