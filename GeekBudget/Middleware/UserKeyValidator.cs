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

        public UserKeyValidator(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method != "OPTIONS") //Do not check if it is preflight cors request
            {
                var userRepo = context.RequestServices.GetService(typeof(IUserRepository)) as IUserRepository;
                if (userRepo.AreContactsEmpty()) //Do not check login if there are no users!)
                {
                    await _next.Invoke(context);
                    return;;
                } 

                if (!context.Request.Headers.Keys.Contains("user-key"))
                {
                    context.Response.StatusCode = 400; //Bad Request                
                    await context.Response.WriteAsync("User Key is missing");
                    return;
                }
                else
                {
                    if (!userRepo.CheckValidUserKey(context.Request.Headers["user-key"]))
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
