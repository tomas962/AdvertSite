using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertSite.Models
{
    public class UserLogin
    {
        [Display(Name = "User ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Būtina įvesto vardą")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Būtina įvesti slaptažodį")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

       // [Display(Name = "Remember Me")]
        //public bool RememberMe { get; set; }
    }
}
