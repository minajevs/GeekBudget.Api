using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using GeekBudget.Helpers;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GeekBudget.Services.Implementations
{
    public class TabService : ITabService
    {
        private readonly IGeekBudgetContext _context;
        private readonly IMappingService _mappingService;

        public TabService(IGeekBudgetContext context, IMappingService mappingService)
        {
            _context = context;
            _mappingService = mappingService;
        }

        public Task<IEnumerable<TabViewModel>> GetAll()
        {
            return Task.FromResult(_context.Tabs.Select(_mappingService.Map));
        }

        public async Task<TabViewModel> Get(int id)
        {
            var tab = await _context.Tabs
                .AsNoTracking()
                .SingleOrDefaultAsync(x => Equals(x.Id, id));
            
            return tab != null
                ? _mappingService.Map(tab)
                : null;
        }

        public async Task<int> Add(TabViewModel vm)
        {
            var tab = _mappingService.Map(vm);
            var addedTab = _context.Tabs
                .Add(tab);
            await _context.SaveChangesAsync();
            return addedTab.Entity.Id;
        }

        public async Task Remove(int id)
        {
            var tab = await _context.Tabs
                .AsNoTracking()
                .FirstOrDefaultAsync(t => Equals(t.Id, id));
            
            if (tab == null) //If entry by id exist should not do anything
                return;

            _context.Tabs.Remove(tab);
            await _context.SaveChangesAsync();
        }

        public async Task Update(TabViewModel vm)
        {
            var tab = await _context.Tabs
                .AsNoTracking()
                .FirstOrDefaultAsync(t => Equals(t.Id, vm.Id));

            var updateTab = _mappingService.Map(vm);
            
            tab.MapNewValues(updateTab,
                x => x.Name,
                x => x.Amount,
                x => x.Currency,
                x => x.Type);
            
            await _context.SaveChangesAsync();
        }
    }
}