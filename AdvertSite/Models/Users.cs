using System;
using System.Collections.Generic;

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
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int? PhoneNumber { get; set; }
        public string HomeAdress { get; set; }
        public string City { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public ICollection<Listings> Listings { get; set; }
        public ICollection<Reviews> ReviewsBuyer { get; set; }
        public ICollection<Reviews> ReviewsSeller { get; set; }
    }
}
