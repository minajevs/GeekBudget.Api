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

namespace GeekBudget.Controllers
{
    [Route("api/[controller]")]
    public class OperationController : Controller
    {
        private readonly IOperationService _operationService;
        private readonly IOperationValidators _operationValidators;
        
        public OperationController(GeekBudgetContext context, IOperationService operationService,
            IOperationValidators operationValidators)
        {
            _operationService = operationService;
            _operationValidators = operationValidators;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result  = await _operationService.GetAll();
            
            if (result.Failed)
                return Ok(result.Data);
            else
                return BadRequest(result.Errors);
        }

        [HttpPost("Get")]
        public async Task<IActionResult> Get([FromBody] OperationFilter filter)
        {
            var result = await _operationService.Get(filter);

            if (result.Failed)
            {
                if (!result.Data.Any())
                    return NotFound();
                else
                    return Ok(result.Data);
            }
            else
                return BadRequest(result.Errors);
        }

        // GET: api/values
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] OperationViewModel vm)
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
            
            var result = await _operationService.Add(vm);
            
            if (result.Failed)
                return Ok(result.Data);
            else
                return BadRequest(result.Errors);
        }

        // DELETE api/values/5
        [HttpPost("Remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var result = await _operationService.Remove(id);

            if (result.Failed)
                return Ok(result.Data);
            else
                return BadRequest(result.Errors);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] OperationViewModel vm)
        {
            var errors = await vm.Validate(
                _operationValidators.NotNull,
                _operationValidators.FromAndToAreNotEqual
            );

            var result = await _operationService.Update(vm);
            
            if (result.Failed)
                return Ok(result.Data);
            else
                return BadRequest(result.Errors);
        }
    }
}