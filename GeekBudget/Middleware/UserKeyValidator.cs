using GeekBudget.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBudget.Middleware
{
    //http://www.mithunvp.com/write-custom-asp-net-core-middleware-web-api/
    public class UserKeyValidator
    {
        private readonly RequestDelegate _next;
        private IUserRepository UsersRepo { get; set; }

        public UserKeyValidator(RequestDelegate next, IUserRepository _repo)
        {
            _next = next;
            UsersRepo = _repo;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!UsersRepo.AreContactsEmpty()) //Do not check login if there are no users!
            {
                if (!context.Request.Headers.Keys.Contains("user-key"))
                {
                    context.Response.StatusCode = 400; //Bad Request                
                    await context.Response.WriteAsync("User Key is missing");
                    return;
                }
                else
                {
                    if (!UsersRepo.CheckValidUserKey(context.Request.Headers["user-key"]))
                    {
                        context.Response.StatusCode = 401; //UnAuthorized
                        await context.Response.WriteAsync("Invalid User Key");
                        return;
                    }
                }
            }

            await _next.Invoke(context);
        }
    }
}
