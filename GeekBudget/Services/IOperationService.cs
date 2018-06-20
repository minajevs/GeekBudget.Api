using System.Collections.Generic;
using System.Threading.Tasks;
using GeekBudget.Entities;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;

namespace GeekBudget.Services
{
    public interface IOperationService
    {
        Task<ServiceResult<IEnumerable<Operation>>> GetAll();
        Task<ServiceResult<IEnumerable<Operation>>> Get(OperationFilter filter);
        Task<ServiceResult<int>> Add(OperationViewModel vm);
        Task<ServiceResult> Remove(int id);
        Task<ServiceResult> Update(OperationViewModel vm);
     }
 }