using System.Collections.Generic;
using System.Threading.Tasks;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;

namespace GeekBudget.Validators
{
    public interface IOperationValidators
    {
        Task<IEnumerable<Error>> IdExists(OperationViewModel operation);
        Task<IEnumerable<Error>> FromNotNull(OperationViewModel operation);
        Task<IEnumerable<Error>> ToNotNull(OperationViewModel operation);
        Task<IEnumerable<Error>> FromAndToAreNotEqual(OperationViewModel operation);
        Task<IEnumerable<Error>> FromTabExists(OperationViewModel operation);
        Task<IEnumerable<Error>> ToTabExists(OperationViewModel operation);
        Task<IEnumerable<Error>> NotNull(OperationViewModel operation);
    }
}