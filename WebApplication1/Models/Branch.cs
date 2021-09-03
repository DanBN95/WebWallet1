using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string city { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,7)")]
        public decimal lat { set; get; }
        [Required]
        [Column(TypeName = "decimal(10,7)")]
        public decimal lng { set; get; }
        public List<Account> Accounts { get; set; }
    }
}
