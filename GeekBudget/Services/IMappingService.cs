using System.Collections.Generic;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;

namespace GeekBudget.Services
{
    public interface IMappingService
    {
        Tab Map(TabViewModel tab);
        TabViewModel Map(Tab tab);
        IEnumerable<TabViewModel> Map(IEnumerable<Tab> tabs);
    }
}