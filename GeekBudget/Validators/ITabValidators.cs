using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;

namespace GeekBudget.Validators
{
    public interface ITabValidators
    {
        Task<IEnumerable<Error>> IdExists(TabViewModel tab);
        Task<IEnumerable<Error>> TabTypeRequired(TabViewModel tab);
        Task<IEnumerable<Error>> NotNull(TabViewModel tab);
    }
}