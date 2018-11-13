using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GeekBudget.Core;
using GeekBudget.Domain.Operations;

namespace GeekBudget.Domain.Tabs
{
    public class Tab
    {
        [Key]
        public int Id { get; set; }
        public TabType Type { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EUR";
        public List<Operation> Operations => OperationsFrom.Concat(OperationsTo).Distinct().ToList();
        public List<Operation> OperationsFrom { get; set; } = new List<Operation>();
        public List<Operation> OperationsTo { get; set; } = new List<Operation>();
    }
}
