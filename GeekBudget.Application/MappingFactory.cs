using System.Collections.Generic;
using System.Linq;
using GeekBudget.Application.Operations.Requests;
using GeekBudget.Application.Tabs.Requests;
using GeekBudget.Core;
using GeekBudget.Domain.Operations;
using GeekBudget.Domain.Tabs;

namespace GeekBudget.Application
{
    public static class MappingFactory
    {
        public static TabVm Map(Tab tab) => new TabVm
        {
            Id = tab.Id,
            Name = tab.Name,
            Amount = tab.Amount,
            Currency = tab.Currency,
            Type = (int)tab.Type
        };

        public static TabVm[] Map(IEnumerable<Tab> tabs) => tabs.Select(Map).ToArray();

        public static Tab Map(AddTabRequest request) => new Tab
        {
            Name = request.Name,
            Type = (TabType) request.Type,
            Currency = request.Currency, 
            Amount = request.Amount
        };

        public static OperationVm Map(Operation operation) => new OperationVm
        {
            Id = operation.Id,
            Comment = operation.Comment,
            Amount = operation.Amount,
            Currency = operation.Currency,
            From = operation.From.Id,
            To = operation.To.Id,
            Date = operation.Date
        };

        public static OperationVm[] Map(IEnumerable<Operation> operations) => operations.Select(Map).ToArray();

        public static Operation Map(AddOperationRequest request) => new Operation
        {
            Comment = request.Comment,
            Amount = request.Amount,
            Currency = request.Currency,
            Date = request.Date
        };
    }
}