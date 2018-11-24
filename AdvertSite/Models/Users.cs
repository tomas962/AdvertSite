using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdvertSite.Models
{
    public partial class Users
    {
        public Users()
        {
            Listings = new HashSet<Listings>();
            ReviewsBuyer = new HashSet<Reviews>();
            ReviewsSeller = new HashSet<Reviews>();
        }

        public int Id { get; set; }
        public int Userlevel { get; set; }

        [Required(ErrorMessage = "Ši lauką butina užpildyti"), DisplayName("Vartotojo Vardas")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Ši lauką butina užpildyti")]
        [DataType(DataType.Password)]
        [DisplayName("Slaptažodis")]
        public string Password { get; set; }
        /*
        [Required(ErrorMessage = "Ši lauką butina užpildyti")]
        [DataType(DataType.Password)]
        [DisplayName("Patvirtinti slaptažodį")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        */
        [Required(ErrorMessage = "Ši lauką butina užpildyti"), DisplayName("Elektroninis paštas")]
        public string Email { get; set; }

        [DisplayName("Telefono numeris")]
        public int? PhoneNumber { get; set; }

        [DisplayName("Gyvenamasis adresas")]
        public string HomeAdress { get; set; }

        [DisplayName("Miestas")]
        public string City { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public ICollection<Listings> Listings { get; set; }
        public ICollection<Reviews> ReviewsBuyer { get; set; }
        public ICollection<Reviews> ReviewsSeller { get; set; }
    }
}
