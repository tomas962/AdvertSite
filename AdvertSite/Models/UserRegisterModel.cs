using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertSite.Models
{
    public class UserRegisterModel
    {
        [Required]
        [Display(Name = "Vartotojo Vardas")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "El. Paštas")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Slaptažodžio ilgis {0} turi būti tarp {2} ir {1}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Slaptažodžiai nesutampa")]
        public string ConfirmPassword { get; set; }
    }
}
