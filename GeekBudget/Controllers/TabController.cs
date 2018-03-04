using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;
using GeekBudget.Services;
using GeekBudget.Validators;
using Microsoft.AspNetCore.Cors;

namespace GeekBudget.Controllers
{
    [Route("api/[controller]")]
    public class TabController : Controller
    {
        private readonly ITabService _tabService;
        private readonly ITabValidators _tabValidators;

        public TabController(ITabService tabService, ITabValidators tabValidators)
        {
            _tabService = tabService;
            _tabValidators = tabValidators;
        }

        // GET api/values
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var tabs = await _tabService.GetAll();
            return Ok(tabs);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var tab = await _tabService.Get(id);
            
            if (tab == null)
                return NotFound();
            else
                return Ok(tab);
        }

        // POST: api/values
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody]TabViewModel tab)
        {
            var errors = await tab.Validate(_tabValidators.TabTypeRequired);

            if (errors.Any())
                return BadRequest(errors);

            var id = await _tabService.Add(tab);
            
            return Ok(id);
        }

        // DELETE api/values/5
        [HttpPost("Remove")]
        public async Task<IActionResult> Remove([FromBody]TabViewModel tab)
        {
            var errors = await tab.Validate(_tabValidators.IdExists);

            if (errors.Any())
                return BadRequest(errors);
            
            await _tabService.Remove(tab.Id);
            
            return Ok();
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody]TabViewModel tab)
        {
            var errors = await tab.Validate(_tabValidators.IdExists);

            if (errors.Any())
                return BadRequest(errors);
            
            await _tabService.Update(tab);

            return Ok();
        }
    }
}
