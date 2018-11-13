using System;
using System.ComponentModel.DataAnnotations;
using GeekBudget.Domain.Tabs;

namespace GeekBudget.Domain.Operations
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
    }
}
