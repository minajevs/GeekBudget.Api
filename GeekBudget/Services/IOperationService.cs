using System.Collections.Generic;
using System.Threading.Tasks;
using GeekBudget.Entities;
using GeekBudget.Models;
using GeekBudget.Models.Requests;
using GeekBudget.Models.ViewModels;

namespace GeekBudget.Services
{
    public interface IOperationService
    {
        Task<ServiceResult<IEnumerable<Operation>>> GetAll();
        Task<ServiceResult<IEnumerable<Operation>>> Get(OperationFilter filter);
        Task<ServiceResult<int>> Add(AddOperationRequest request);
        Task<ServiceResult> Remove(int id);
        Task<ServiceResult> Update(UpdateOperationRequest request);
     }
 }