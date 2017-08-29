using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeekBudget.Models;

namespace GeekBudget.Controllers
{
    [Route("api/[controller]")]
    public class OperationController : BaseController
    {
        public OperationController(GeekBudgetContext context) : base(context) { }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Operations.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var tab = _context.Tabs.FirstOrDefault(t => t.Id == id);
            if (tab == null)
                return NotFound(null);
            else
                return Ok(tab);
        }

        // GET: api/values
        [HttpGet]
        public IActionResult Add([FromBody]Tab value)
        {
            TryValidateModel(value);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Tabs.Add(value);
            _context.SaveChanges();

            return Ok(value.Id);
        }

        // DELETE api/values/5
        [HttpPost("{id}")]
        public IActionResult Remove(int id)
        {
            if (!_context.Tabs.Any(t => t.Id == id)) //If entry by id exist
                return BadRequest(String.Format("No Tab with id '{0}' was found!", id));

            var removeTab = new Tab() { Id = id };
            _context.Tabs.Attach(removeTab);
            _context.Tabs.Remove(removeTab);
            _context.SaveChanges();
            return Ok(true);
        }

        [HttpPost("{id}")]
        public IActionResult Update(int id, [FromBody]Tab value)
        {
            if (value == null)
                return BadRequest("Can't update with null value!");
            TryValidateModel(value);
            if (!ModelState.IsValid) //If model is wrong
                return BadRequest(ModelState);

            var updateTab = _context.Tabs.FirstOrDefault(t => t.Id == id);

            if (updateTab == null) //If entry by id exist
                return BadRequest("No Tab with this id was found!");

            updateTab.MapNewValues(value); //TODO: change to Attach update to not query db before update

            _context.SaveChanges();
            return Ok(true);
        }

    }
}
