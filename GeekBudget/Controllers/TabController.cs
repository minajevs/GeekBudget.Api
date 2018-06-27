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
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TabController : ControllerBase
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

        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(Error[]), 400)]
        public async Task<ActionResult<TabViewModel[]>> GetAll()
        {
            var result = await _tabService.GetAll();

            if (!result.Failed)
                return _mappingService
                    .Map(result.Data)
                    .ToArray();
            else
                return BadRequest(result.Errors);
        }

        [HttpGet("Get/{id}")]
        [ProducesResponseType(typeof(Error[]), 400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TabViewModel>> Get(int id)
        {
            var result = await _tabService.Get(id);

            if (!result.Failed)
            {
                if (result.Data != null)
                    return _mappingService.Map(result.Data);
                else
                    return NotFound();
            }
            else
                return BadRequest(result.Errors);
            
            
        }

        // POST: api/values
        [HttpPost("Add")]
        [ProducesResponseType(typeof(Error[]), 400)]
        public async Task<ActionResult<int>> Add([FromBody]TabViewModel vm)
        {
            var errors = await vm.Validate(
                _tabValidators.NotNull,
                _tabValidators.TabTypeRequired
            );

            if (errors.Any())
                return BadRequest(errors);

            var tab = _mappingService.Map(vm);
            
            var result = await _tabService.Add(tab);
            
            if (!result.Failed)
                return result.Data;
            else
                return BadRequest(result.Errors);
        }

        // DELETE api/values
        [HttpPost("Remove/{id}")]
        [ProducesResponseType(typeof(Error[]), 400)]
        public async Task<ActionResult> Remove(int id)
        {
            var result = await _tabService.Remove(id);
            
            if (!result.Failed)
                return Ok();
            else
                return BadRequest(result.Errors);
        }

        [HttpPost("Update")]
        [ProducesResponseType(typeof(Error[]), 400)]
        public async Task<ActionResult> Update([FromBody]TabViewModel vm)
        {
            var errors = await vm.Validate(
                _tabValidators.NotNull,
                _tabValidators.IdExists
            );

            if (errors.Any())
                return BadRequest(errors);
            
            var tab = _mappingService.Map(vm);
            
            var result = await _tabService.Update(vm.Id, tab);

            if (!result.Failed)
                return Ok();
            else
                return BadRequest(result.Errors);
        }
    }
}
