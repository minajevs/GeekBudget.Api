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
    }
}
