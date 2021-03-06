using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public enum InCategory
    {
        Salary,
        RealEstate,
        Apps,
        SavingAccount,
        Other
    }
    public class Incomes
    {
        
        public int Id { get; set; }
        public double Amount { get; set; }
        public String Description { get; set; }
        public InCategory Category { get; set; } = InCategory.Other;
        public DateTime Date { get; set; }

        public int AccountId { get; set; }

        public Account Account { get; set; }




    }
}
