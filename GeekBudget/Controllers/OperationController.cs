using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Entities;
using GeekBudget.Helpers;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;
using GeekBudget.Services;
using GeekBudget.Validators;
using Microsoft.AspNetCore.Mvc;

namespace GeekBudget.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class OperationController : ControllerBase
    {
        private readonly IOperationService _operationService;
        private readonly IOperationValidators _operationValidators;
        private readonly IMappingService _mappingService;

        public OperationController(IOperationService operationService, IOperationValidators operationValidators, IMappingService mappingService)
        {
            _operationService = operationService;
            _operationValidators = operationValidators;
            _mappingService = mappingService;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(Error[]), 400)]
        public async Task<ActionResult<OperationViewModel[]>> GetAll()
        {
            var result  = await _operationService.GetAll();

            if (!result.Failed)
                return _mappingService
                    .Map(result.Data)
                    .ToArray();
            else
                return BadRequest(result.Errors);
        }

        [HttpPost("Get")]
        [ProducesResponseType(typeof(Error[]), 400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OperationViewModel[]>> Get([FromBody] OperationFilter filter)
        {
            var result = await _operationService.Get(filter);

            if (!result.Failed)
            {
                if (!result.Data.Any())
                    return NotFound();
                else
                    return _mappingService
                        .Map(result.Data)
                        .ToArray();
            }
            else
                return BadRequest(result.Errors);
        }

        [HttpPost("Add")]
        [ProducesResponseType(typeof(Error[]), 400)]
        public async Task<ActionResult<int>> Add([FromBody] OperationViewModel vm)
        {
            var errors = await vm.Validate(
                _operationValidators.NotNull,
                _operationValidators.FromNotNull,
                _operationValidators.ToNotNull,
                _operationValidators.FromAndToAreNotEqual,
                _operationValidators.FromTabExists,
                _operationValidators.ToTabExists
            );

            if (errors.Any())
                return BadRequest(errors);

            var operation = _mappingService.Map(vm);

            var result = await _operationService.Add(operation, vm.From ?? -1, vm.To ?? -1); // not null should be validated before
            
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

        [HttpPost("Update")]
        [ProducesResponseType(typeof(Error[]), 400)]
        public async Task<ActionResult> Update([FromBody] OperationViewModel vm)
        {
            var errors = await vm.Validate(
                _operationValidators.NotNull,
                _operationValidators.IdExists,
                _operationValidators.FromAndToAreNotEqual
            );

            if (errors.Any())
                return BadRequest(errors);

            var operation = _mappingService.Map(vm);

            var result = await _operationService.Update(vm.Id, operation, vm);
            
            if (!result.Failed)
                return Ok();
            else
                return BadRequest(result.Errors);
        }
    }
}