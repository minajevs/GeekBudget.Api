using System.Collections.Generic;

namespace GeekBudget.Entities
{
    public static class Dictionaries
    {
        public static Dictionary<Enums.TabType, IEnumerable<Enums.TabType>> AllowedTabTypes = new Dictionary<Enums.TabType, IEnumerable<Enums.TabType>>()
        {
            {Enums.TabType.Income, new [] { Enums.TabType.Account}},
            {Enums.TabType.Account, new [] { Enums.TabType.Account, Enums.TabType.Expense}},
            {Enums.TabType.Expense, new Enums.TabType [] {}}            
        };
    }
} 