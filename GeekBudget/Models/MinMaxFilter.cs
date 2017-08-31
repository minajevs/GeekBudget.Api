using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBudget.Models
{
    public class MinMaxFilter<TEntity>
    {
        public TEntity Min { get; set; }
        public TEntity Max { get; set; }
    }
}
