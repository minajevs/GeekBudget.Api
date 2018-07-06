using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBudget.Entities
{
    public enum TargetTabType
    {
        From = 1,
        To
    }
    
    public enum TabType
    {
        Income = 1,
        Account,
        Expense
    }
    
    public enum ServiceResultStatus
    {
        Success,
        Warning,
        Failure
    }
}
