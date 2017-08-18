using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBudget.Models
{
    public class Tab
    {
        int Id { get; set; }
        string Name { get; set; }
        decimal Amount { get; set; }
        string Currency { get; set; }
        IEnumerable<Operation> Operations { get; set; }
    }
}
