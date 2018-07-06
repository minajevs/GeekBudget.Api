using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Entities;

namespace GeekBudget.Models.ViewModels
{
    public class TabVm
    {
        public int Id { get; set; }
        public int? Type { get; set; }
        public string Name { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; } = "EUR";
    }
}
