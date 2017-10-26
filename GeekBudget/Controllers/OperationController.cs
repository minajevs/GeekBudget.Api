using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;

namespace GeekBudget.Controllers
{
    [Route("api/[controller]")]
    public class OperationController : BaseController
    {
        public OperationController(GeekBudgetContext context) : base(context) { }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_context
                .Operations
                .Include(o => o.From)
                .Include(o => o.To)
                .Select(o => OperationViewModel.FromEntity(o))
                .ToList()
            );
        }

        [HttpPost("Get")]
        public IActionResult Get([FromBody]OperationFilter filter)
        {
            var data = filter.CreateQuery(_context.Operations);

            if (!data.Any())
                return NotFound();
            else
                return Ok(data
                    .Include(o => o.From)
                    .Include(o => o.To)
                    .Select(o => OperationViewModel.FromEntity(o))
                    .ToList()
                    );
        }

        // GET: api/values
        [HttpPost("Add")]
        public IActionResult Add([FromBody]OperationViewModel value)
        {
            TryValidateModel(value);

            if (value.From == null)
                ModelState.AddModelError("From", "'From' id can't be null!");

            if (value.To == null)
                ModelState.AddModelError("To", "'To' id can't be null!");

            if (value.From == value.To) //no need to check for null first.
                ModelState.AddModelError("Tabs", "'From' tab and 'To' tab are same!");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tabFrom = _context.Tabs.SingleOrDefault(x => x.Id == value.From);
            var tabTo = _context.Tabs.SingleOrDefault(x => x.Id == value.To);

            if (tabFrom == null || tabTo == null)
                return BadRequest("Can't find tab with provided Id!");


            var operation = value
                .MapToEntity()
                .UpdateTab(Enums.TabType.From, tabFrom)
                .UpdateTab(Enums.TabType.To, tabTo);
            var newOperation = _context.Operations.Add(operation).Entity;
            _context.SaveChanges();

            return Ok(newOperation.Id);
        }

        // DELETE api/values/5
        [HttpPost("Remove/{id}")]
        public IActionResult Remove(int id)
        {
            if (!_context.Operations.Any(o=> o.Id == id)) //If entry by id exist
                return BadRequest(String.Format("No Operation with id '{0}' was found!", id));

            var removeOperation = new Operation() { Id = id };
            _context.Operations.Attach(removeOperation);
            _context.Operations.Remove(removeOperation);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("Update")]
        public IActionResult Update([FromBody]OperationViewModel value)
        {
            if (value == null)
                return BadRequest("Can't update with null value!");
            TryValidateModel(value);
            if (!ModelState.IsValid) //If model is wrong
                return BadRequest(ModelState);

            Operation updateOperation = null;
            Tab newTabFrom = null;
            Tab newTabTo = null;

            //Check if operation exsits (to save db transaction costs)
            if (!_context.Operations.Any(o => o.Id == value.Id))
                return BadRequest("No Operation with this id was found!");

            //Get operation to update
            updateOperation = _context
                .Operations
                    .Include(o => o.From)
                    .Include(o => o.To)
                .SingleOrDefault(t => t.Id == value.Id);
            

            //Get new tab From
            if (value.From != null && value.From != updateOperation.From.Id)
            {
                newTabFrom = _context.Tabs.SingleOrDefault(t => t.Id == value.From);
                if (newTabFrom == null) //If Tab by id does not exist
                    return BadRequest("No Tab with this id was found!");
            }

            //Get new tab To
            if (value.To != null && value.To != updateOperation.To.Id)
            {
                newTabTo = _context.Tabs.SingleOrDefault(t => t.Id == value.From);
                if (newTabTo == null) //If Tab by id does not exist
                    return BadRequest("No Tab with this id was found!");
            }

            //Update operation
            updateOperation
                .MapNewValues(value)
                .UpdateTab(Enums.TabType.From, newTabFrom)
                .UpdateTab(Enums.TabType.To, newTabTo);  //TODO: change to Attach update to not query db before update
            _context.SaveChanges();

            //Can't happen. Argument exception raises only from Add/Remove op, and all conditions are checked before!
            //catch (ArgumentException ex)
            //{
            //    return BadRequest(ex.Message);
            //}
            return Ok();
        }

    }
}
