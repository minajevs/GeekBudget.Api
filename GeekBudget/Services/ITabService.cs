using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeekBudget.Entities;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;

namespace GeekBudget.Services
{
    public interface ITabService
    {
        Task<ServiceResult<IEnumerable<Tab>>> GetAll();
        Task<ServiceResult<Tab>> Get(int id);
        Task<ServiceResult<int>> Add(Tab tab);
        Task<ServiceResult> Remove(int id);
        Task<ServiceResult> Update(int id, Tab source);
        Task<ServiceResult<bool>> IsTabOperationAllowed(Tab tabFrom, Tab tabTo);
        Task<ServiceResult> AddOperation(int id, Operation operation, Enums.TargetTabType targetType);
    }
}