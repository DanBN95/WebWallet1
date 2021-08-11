using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public enum ExCategory
    {
        Food,
        Pleasure,
        Insurance,
        Health,
        Sport,
        Investment,
        Other
    }
    public class Expenses
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public String Description { get; set; }
        public ExCategory Category { get; set; }
        public DateTime Date { get; set; }

    }
}
