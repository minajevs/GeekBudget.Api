using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBudget.Entities
{
    public enum TargetTabType
    {
        From,
        To
    }
    
    public enum TabType
    {
        Income,
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
