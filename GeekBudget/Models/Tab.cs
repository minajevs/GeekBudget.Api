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
        [Required]
        public string Name { get; set; }
        public decimal Amount { get; set; } = 0;
        public string Currency { get; set; } = "EUR";
        public IEnumerable<Operation> Operations { get; set; }
    }
}
