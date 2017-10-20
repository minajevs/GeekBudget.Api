using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeekBudget.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private UserRepository _userRepository { get;}

        public UserController(GeekBudgetContext context) : base(context) => _userRepository = new UserRepository(context);

        [HttpPost("Add")]
        public IActionResult Add(string username)
        {
            try
            {
                var userKey = _userRepository.Add(username);
                return Ok(userKey);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}