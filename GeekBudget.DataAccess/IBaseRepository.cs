using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeekBudget.DataAccess
{
    public interface IBaseRepository
    {
        Task<int> Save();
    }
}
