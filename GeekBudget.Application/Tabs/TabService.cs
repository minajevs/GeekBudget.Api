using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Application.Operations;
using GeekBudget.Application.Tabs.Requests;
using GeekBudget.Core;
using GeekBudget.Core.Helpers;
using GeekBudget.DataAccess;
using GeekBudget.DataAccess.Tabs;
using GeekBudget.Domain.Operations;
using GeekBudget.Domain.Tabs;
using Microsoft.EntityFrameworkCore;

namespace GeekBudget.Application.Tabs
{
    public class TabService : ITabService
    {
        private readonly IGeekBudgetContext _context;

        public TabService(IGeekBudgetContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<IEnumerable<Tab>>> GetAll()
        {
            var tabs = await _context.Tabs
                .AsNoTracking()
                .ToListAsync();

            return tabs;
        }

        public async Task<ServiceResult<Tab>> Get(int id)
        {
            var tab = await _context.Tabs
                .AsNoTracking()
                .SingleOrDefaultAsync(x => Equals(x.Id, id));

            if (tab == null)
                return TabErrors.TabWithIdDoesNotExist(id);

            return tab;
        }

        public async Task<ServiceResult> Add(AddTabRequest request)
        {
            var tab = MappingFactory.Map(request);

            _context.Tabs.Add(tab);

            await _context.SaveChangesAsync();

            return ServiceResultStatus.Success;
        }

        public async Task<ServiceResult> Remove(int id)
        {
            var tabResult = await Get(id);

            if (tabResult.Failed)
                return ServiceResult.From(tabResult);

            _context.Tabs.Remove(tabResult.Data);

            await _context.SaveChangesAsync();

            return ServiceResultStatus.Success;
        }

        public async Task<ServiceResult> Update(UpdateTabRequest request)
        {
            var tabResult = await Get(request.Id);

            if(tabResult.Failed)
                return ServiceResult.From(tabResult);

            var tab = tabResult.Data;

            tab.MapNewValues(request,
                (x => x.Name, y => y.Name),
                (x => x.Amount, y => y.Amount),
                (x => x.Currency, y => y.Currency),
                (x => x.Type, y => y.Type)
                );

            _context.SetModified(tab);

            await _context.SaveChangesAsync();

            return ServiceResultStatus.Success;
        }

        public Task<ServiceResult<bool>> IsTabOperationAllowed(Tab tabFrom, Tab tabTo)
        {
            var tabOperationAllowed = Dictionaries.AllowedTabTypes[tabFrom.Type].Any(t => t == tabTo.Type);
            return Task.FromResult((ServiceResult<bool>)tabOperationAllowed);
        }
    }
}