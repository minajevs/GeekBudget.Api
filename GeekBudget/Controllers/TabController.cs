﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Entities;
using GeekBudget.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeekBudget.Models;
using GeekBudget.Models.Requests;
using GeekBudget.Models.ViewModels;
using GeekBudget.Services;
using GeekBudget.Services.Implementations;
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

        public TabController(ITabService tabService)
        {
            _tabService = tabService;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(Error[]), 400)]
        public async Task<ActionResult<TabVm[]>> GetAll()
        {
            var result = await _tabService.GetAll();

            if (!result.Failed)
                return MappingFactory.Map(result.Data);
            else
                return BadRequest(result.Errors);
        }

        [HttpGet("Get/{id}")]
        [ProducesResponseType(typeof(Error[]), 400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TabVm>> Get(int id)
        {
            var result = await _tabService.Get(id);

            if (!result.Failed)
            {
                if (result.Data != null)
                    return MappingFactory.Map(result.Data);
                else
                    return NotFound();
            }
            else
                return BadRequest(result.Errors);
            
            
        }

        // POST: api/values
        [HttpPost("Add")]
        [ProducesResponseType(typeof(Error[]), 400)]
        public async Task<ActionResult<int>> Add([FromBody]TabVm vm)
        {
            var request = new AddTabRequest();

            var errors = request.ValidateAndMap(vm);

            if (errors.Any())
                return BadRequest(errors);

            var result = await _tabService.Add(request);
            
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

        [HttpPost("Update/{id}")]
        [ProducesResponseType(typeof(Error[]), 400)]
        public async Task<ActionResult> Update(int id, [FromBody]TabVm vm)
        {
            var request = new UpdateTabRequest(id);

            var errors = request.ValidateAndMap(vm);

            if (errors.Any())
                return BadRequest(errors);

            var result = await _tabService.Update(request);

            if (!result.Failed)
                return Ok();
            else
                return BadRequest(result.Errors);
        }
    }
}
