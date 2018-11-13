using System;
using System.Collections.Generic;
using System.Text;
using GeekBudget.Core;

namespace GeekBudget.Application.Tabs
{
    public static class TabErrors
    {
        public static Error TabIdIsRequired => new Error { Id = 100, Description = $"Tab Id is required!" };
        public static Error TabNameIsRequired => new Error { Id = 101, Description = $"Tab Name is required!" };
        public static Error TabTypeIsRequired => new Error { Id = 102, Description = $"Tab Type is required!" };
        public static Error TabWithIdDoesNotExist(int id) => new Error { Id = 103, Description = $"No Tab with id '{id}' was found!" };
    }
}
