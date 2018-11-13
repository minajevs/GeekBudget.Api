using System;
using GeekBudget.DataAccess.Users;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace GeekBudget.Controllers
{
    [Route("api/[controller]")]
    [SwaggerDefaultResponse]
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