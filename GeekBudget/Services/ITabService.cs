using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;

namespace GeekBudget.Services
{
    public interface ITabService
    {
        Task<IEnumerable<TabViewModel>> GetAll();
        Task<TabViewModel> Get(int id);
        Task<int> Add(TabViewModel vm);
        Task Remove(int id);
        Task Update(TabViewModel vm);
    }
}