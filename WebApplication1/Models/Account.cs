﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Account
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public String Name { get; set; }
        public double Balance { get; set; } = 0;
        public double SavingBalance { get; set; } = 0;
        public List<Expenses> ExpensesList { get; set; } = null;
        public List<Incomes> IncomesList { get; set; } = null;
        public List<FuturePayment> FuturePaymentesList { get; set; } = null;

    }
}
