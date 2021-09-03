using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public enum Types
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
        public Types Type { get; set; }
        [Required]
        public String Description { get; set; }
        public double Goalvalue { get; set; }
        public double SinglePaymentvalue { get; set; }
        [Required]
        public DateTime StartDate { get; set; }

        [Required] 
        public DateTime EndDate { get; set; }

        public DateTime lastpayment { get; set; }

        public DateTime nextpayment { get; set; }

        [Required]
        public Frequency Frequency { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }


    }
}
