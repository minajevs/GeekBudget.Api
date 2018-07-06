using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBudget.Models
{
    public interface IServiceRequest<in T>
    {
        List<Error> ValidateAndMap(T vm);
    }
}
