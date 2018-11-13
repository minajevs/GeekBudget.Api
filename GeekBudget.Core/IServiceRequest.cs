using System.Collections.Generic;

namespace GeekBudget.Core
{
    public interface IServiceRequest<in T>
    {
        List<Error> ValidateAndMap(T vm);
    }
}
