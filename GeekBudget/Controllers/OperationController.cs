using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Entities;
using GeekBudget.Helpers;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using GeekBudget.Models;
using GeekBudget.Models.Requests;
using GeekBudget.Models.ViewModels;
using GeekBudget.Services;
using GeekBudget.Services.Implementations;
using GeekBudget.Validators;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace GeekBudget.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [SwaggerDefaultResponse]
    public class OperationController : ControllerBase
    {
        private readonly IOperationService _operationService;

        public OperationController(IOperationService operationService)
        {
            _operationService = operationService;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(Error[]), 400)]
        public async Task<ActionResult<OperationVm[]>> GetAll()
        {
            var result  = await _operationService.GetAll();

            if (!result.Failed)
                return MappingFactory
                    .Map(result.Data)
                    .ToArray();
            else
                return BadRequest(result.Errors);
        }

        [HttpPost("Get")]
        [ProducesResponseType(typeof(Error[]), 400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OperationVm[]>> Get([FromBody] OperationFilter filter)
        {
            var result = await _operationService.Get(filter);

            if (!result.Failed)
            {
                if (!result.Data.Any())
                    return NotFound();
                else
                    return MappingFactory.Map(result.Data);
            }
            else
                return BadRequest(result.Errors);
        }

        [HttpPost("Add")]
        [ProducesResponseType(typeof(Error[]), 400)]
        public async Task<ActionResult<int>> Add([FromBody] OperationVm vm)
        {
            var request = new AddOperationRequest();

            var errors = request.ValidateAndMap(vm);

            if (errors.Any())
                return BadRequest(errors);

            var result = await _operationService.Add(request);
            
            if (!result.Failed)
                return result.Data;
            else
                return BadRequest(result.Errors);
        }

        // DELETE api/values/5
        [HttpPost("Remove/{id}")]
        [ProducesResponseType(typeof(Error[]), 400)]
        public async Task<ActionResult> Remove(int id)
        {
            var result = await _operationService.Remove(id);

            if (!result.Failed)
                return Ok();
            else
                return BadRequest(result.Errors);
        }

        [HttpPost("Update/{id}")]
        [ProducesResponseType(typeof(Error[]), 400)]
        public async Task<ActionResult> Update(int id, [FromBody] OperationVm vm)
        {
            var request = new UpdateOperationRequest(id);

            var errors = request.ValidateAndMap(vm);

            if (errors.Any())
                return BadRequest(errors);

            var result = await _operationService.Update(request);
            
            if (!result.Failed)
                return Ok();
            else
                return BadRequest(result.Errors);
        }
    }
}