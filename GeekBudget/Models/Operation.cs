using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBudget.Models
{
    public class Operation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }

        public bool ItIsBar()
        {
            return this.Name == "";
        }
    }
}
