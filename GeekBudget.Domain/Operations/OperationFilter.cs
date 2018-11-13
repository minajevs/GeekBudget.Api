using System;
using GeekBudget.Core;

namespace GeekBudget.Domain.Operations
{
    public class OperationFilter
    {
        public int? Id { get; set; }
        public string Comment { get; set; }
        public MinMaxFilter<decimal> Amount { get; set; }
        public string Currency { get; set; }
        public int? From { get; set; }
        public int? To { get; set; }
        public MinMaxFilter<DateTime> Date { get; set; }
    }
}
