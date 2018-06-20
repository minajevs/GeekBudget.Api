using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Entities;
using GeekBudget.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;
using GeekBudget.Services;
using GeekBudget.Validators;
using Microsoft.AspNetCore.Cors;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GeekBudget.Controllers
{
    [Route("api/[controller]")]
    public class TabController : Controller
    {
        private readonly ITabService _tabService;
        private readonly ITabValidators _tabValidators;
        private readonly IMappingService _mappingService;

        public TabController(ITabService tabService, ITabValidators tabValidators, IMappingService mappingService)
        {
            _tabService = tabService;
            _tabValidators = tabValidators;
            _mappingService = mappingService;
        }

        // GET api/values
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _tabService.GetAll();

            if (result.Status != Enums.ServiceResultStatus.Failure)
                return Ok(_mappingService.Map(result.Data));
            else
                return BadRequest(result.Errors);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _tabService.Get(id);

            if (result.Status != Enums.ServiceResultStatus.Failure)
            {
                if (result.Data != null)
                    return Ok(_mappingService.Map(result.Data));
                else
                    return NotFound();
            }
            else
                return BadRequest(result.Errors);
            
            
        }

        // POST: api/values
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody]TabViewModel vm)
        {
            var errors = await vm.Validate(
                _tabValidators.NotNull,
                _tabValidators.TabTypeRequired
            );

            if (errors.Any())
                return BadRequest(errors);

            var tab = _mappingService.Map(vm);
            
            var result = await _tabService.Add(tab);
            
            if (result.Status != Enums.ServiceResultStatus.Failure)
                return Ok(result.Data);
            else
                return BadRequest(result.Errors);
        }

        // DELETE api/values
        [HttpPost("Remove")]
        public async Task<IActionResult> Remove([FromBody]TabViewModel vm)
        {
            var errors = await vm.Validate(
                _tabValidators.NotNull,
                _tabValidators.IdExists
            );

            if (errors.Any())
                return BadRequest(errors);
            
            var result = await _tabService.Remove(vm.Id);
            
            if (result.Status != Enums.ServiceResultStatus.Failure)
                return Ok();
            else
                return BadRequest(result.Errors);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody]TabViewModel vm)
        {
            var errors = await vm.Validate(
                _tabValidators.NotNull,
                _tabValidators.IdExists
            );

            if (errors.Any())
                return BadRequest(errors);
            
            var tab = _mappingService.Map(vm);
            
            var result = await _tabService.Update(vm.Id, tab);

            if (result.Status != Enums.ServiceResultStatus.Failure)
                return Ok();
            else
                return BadRequest(result.Errors);
        }
    }
}
