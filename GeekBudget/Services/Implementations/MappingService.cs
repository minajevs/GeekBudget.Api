using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;

namespace GeekBudget.Services.Implementations
{
    public class MappingService : IMappingService
    {
        public Tab Map(TabViewModel tab) => new Tab()
        {
            Name = tab.Name,
            Amount = tab.Amount ?? 0,
            Currency = tab.Currency,
            Type = tab.Type ?? 0
        };
        
        public TabViewModel Map(Tab tab) => new TabViewModel
        {
            Id = tab.Id,
            Name = tab.Name,
            Amount = tab.Amount,
            Currency = tab.Currency,
            Type = tab.Type
        };

        public IEnumerable<TabViewModel> Map(IEnumerable<Tab> tabs) => tabs.Select(Map);
    }
}