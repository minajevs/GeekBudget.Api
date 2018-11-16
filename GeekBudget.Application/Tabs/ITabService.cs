using System.Collections.Generic;
using System.Threading.Tasks;
using GeekBudget.Application.Tabs.Requests;
using GeekBudget.Core;
using GeekBudget.Domain.Operations;
using GeekBudget.Domain.Tabs;

namespace GeekBudget.Application.Tabs
{
    public interface ITabService
    {
        Task<ServiceResult<IEnumerable<Tab>>> GetAll();
        Task<ServiceResult<Tab>> Get(int id);
        Task<ServiceResult> Add(AddTabRequest request);
        Task<ServiceResult> Remove(int id);
        Task<ServiceResult> Update(UpdateTabRequest request);
        Task<ServiceResult<bool>> IsTabOperationAllowed(Tab tabFrom, Tab tabTo);
    }
}