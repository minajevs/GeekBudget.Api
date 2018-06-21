using System.Collections.Generic;

namespace GeekBudget.Entities
{
    public static class Dictionaries
    {
        public static Dictionary<TabType, IEnumerable<TabType>> AllowedTabTypes = new Dictionary<TabType, IEnumerable<TabType>>()
        {
            {TabType.Income, new [] { TabType.Account}},
            {TabType.Account, new [] { TabType.Account, TabType.Expense}},
            {TabType.Expense, new TabType [] {}}            
        };
    }
} 