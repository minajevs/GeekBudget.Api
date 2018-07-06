using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeekBudget.Entities;
using GeekBudget.Models;
using GeekBudget.Models.Requests;
using GeekBudget.Models.ViewModels;

namespace GeekBudget.Services
{
    public interface ITabService
    {
        Task<ServiceResult<IEnumerable<Tab>>> GetAll();
        Task<ServiceResult<Tab>> Get(int id);
        Task<ServiceResult<int>> Add(AddTabRequest request);
        Task<ServiceResult> Remove(int id);
        Task<ServiceResult> Update(UpdateTabRequest request);
        Task<ServiceResult<bool>> IsTabOperationAllowed(Tab tabFrom, Tab tabTo);
        Task<ServiceResult> AddOperation(int id, Operation operation, TargetTabType targetType);
    }
}