using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeekBudget.Models;

namespace GeekBudget.Helpers
{
    public static class ValidationHelper
    {
        public static async Task<IEnumerable<Error>> Validate<T>(this T value, params Func<T, Task<IEnumerable<Error>>>[] validators)
        {
            var errors = new List<Error>();

            foreach (var validator in validators)
            {
                errors.AddRange(await validator(value));
            }

            return errors;
        }
    }
}