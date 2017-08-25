using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeekBudget.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeekBudget.Controllers
{
    [Route("api/[controller]")]
    public class TabController : BaseController
    {
        public TabController(GeekBudgetContext context) : base(context) { }

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

        // GET api/values
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Tabs.ToList());
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
    }
}
