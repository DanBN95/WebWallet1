using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public enum Type
    {
        Saving,
        ProductGoal
    }
    public enum Frequency
    {
        Day,
        Week,
        Month,
        Year
    }
    public class FuturePayment
    {
        public int Id { get; set; }
        public Type Type { get; set; }
        public String Description { get; set; }
        public double GoalCost { get; set; }
        public double PaymentCost { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Frequency Frequency { get; set; }
        public List<Account> AccountsList{ get; set; }
    }
}
