using System.Collections.Generic;
using System.Threading.Tasks;
using GeekBudget.Application.Operations.Requests;
using GeekBudget.Core;
using GeekBudget.Domain.Operations;

namespace GeekBudget.Application.Operations
{
    public interface IOperationService
    {
        Task<ServiceResult<IEnumerable<Operation>>> GetAll();
        Task<ServiceResult<IEnumerable<Operation>>> Get(OperationFilter filter);
        Task<ServiceResult> Add(AddOperationRequest request);
        Task<ServiceResult> Remove(int id);
        Task<ServiceResult> Update(UpdateOperationRequest request);
     }
 }