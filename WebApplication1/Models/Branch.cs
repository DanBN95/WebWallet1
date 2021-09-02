using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Branch
    {
        public int Id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string Addrees { get; set; }
        public List<Account> Accounts { get; set; }
    }
}
