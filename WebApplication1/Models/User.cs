using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public enum UserType
    {
        Guest,
        Client,
        Admin
    }
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20,ErrorMessage ="Name out of range!")]
        public string User_first_name { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Name out of range!")]
        public string User_last_name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public UserType Type { get; set; } = UserType.Client;

        
    }
}
