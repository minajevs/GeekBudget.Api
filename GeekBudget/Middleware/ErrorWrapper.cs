using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GeekBudget.Middleware
{
    public class ErrorWrapper
    {
        private readonly RequestDelegate _next;

        public ErrorWrapper(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var errorMsg = "";
            try
            {
                await _next.Invoke(context);
            }
            catch (InvalidOperationException ex) //TODO: convert all expected errors to known type
            {
                context.Response.StatusCode = 400;
                errorMsg = ex.Message;
            }
            catch (ArgumentException ex)
            {
                context.Response.StatusCode = 400;
                errorMsg = ex.Message;
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                //errorMsg = "internal server error";
            }

            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(errorMsg);
            }
        }
    }
}
