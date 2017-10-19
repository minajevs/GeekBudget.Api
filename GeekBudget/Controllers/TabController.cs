using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeekBudget.Controllers
{
    [Route("api/[controller]")]
    public class TabController : BaseController
    {
        public TabController(GeekBudgetContext context) : base(context) { }

        // GET api/values
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_context
                .Tabs
                .Include(t => t.OperationsFrom)
                .Include(t => t.OperationsTo)
                .Select(t => TabViewModel.FromEntity(t))
                .ToList()
            );
        }

        [HttpGet("Get/{id}")]
        public IActionResult Get(int id)
        {
            var tab = _context.Tabs.FirstOrDefault(t => t.Id == id);
            if (tab == null)
                return NotFound();
            else
                return Ok(TabViewModel.FromEntity(tab));
        }

        // POST: api/values
        [HttpPost("Add")]
        public IActionResult Add([FromBody]TabViewModel value)
        {
            TryValidateModel(value);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tab = value.MapToEntity();
            _context.Tabs.Add(tab);
            _context.SaveChanges();

            return Ok(tab.Id);
        }

        // DELETE api/values/5
        [HttpPost("Remove/{id}")]
        public IActionResult Remove(int id)
        {
            if (!_context.Tabs.Any(t => t.Id == id)) //If entry by id exist
                return BadRequest(String.Format("No Tab with id '{0}' was found!", id));

            var removeTab = new Tab() { Id = id };
            _context.Tabs.Attach(removeTab);
            _context.Tabs.Remove(removeTab);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("Update/{id}")]
        public IActionResult Update([FromBody]TabViewModel value)
        {
            if(value == null)
                return BadRequest("Can't update with null value!");

            if (!_context.Tabs.Any(t => t.Id == value.Id)) //If entry by id exist
                return BadRequest(String.Format("No Tab with id '{0}' was found!", value.Id));

            TryValidateModel(value);
            if (!ModelState.IsValid) //If model is wrong
                return BadRequest(ModelState);

            var updateTab = _context.Tabs.SingleOrDefault(t => t.Id == value.Id);

            updateTab.MapNewValues(value); //TODO: change to Attach update to not query db before update

            _context.SaveChanges();
            return Ok();
        }
    }
}
