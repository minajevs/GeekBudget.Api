using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeekBudget.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeekBudget.Controllers
{
    public class BaseController : Controller
    {
        protected readonly GeekBudgetContext _context;
        
        public BaseController(GeekBudgetContext context)
        {
            _context = context;

            //Seed method here? 
        }
    }
}
