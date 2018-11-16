using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeekBudget.DataAccess
{
    public class BaseRepository : IBaseRepository
    {
        protected readonly IGeekBudgetContext _context;

        public BaseRepository(IGeekBudgetContext context)
        {
            _context = context;
        }

        public Task<int> Save() => _context.SaveChangesAsync();
    }
}
