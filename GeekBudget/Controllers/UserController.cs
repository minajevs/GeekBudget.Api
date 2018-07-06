using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeekBudget.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        } 

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