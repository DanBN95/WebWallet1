using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Account
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public double Balance { get; set; }
        public double SavingBalance { get; set; }
        public List<Expenses> ExpensesList { get; set; }
        public List<Incomes> IncomesList { get; set; }
        public List<FuturePayment> FuturePaymentesList { get; set; }

    }
}
