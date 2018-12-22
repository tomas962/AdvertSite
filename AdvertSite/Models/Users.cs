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
            Comments = new HashSet<Comments>();
            Messages = new HashSet<Messages>();
            UsersHasMessages = new HashSet<UsersHasMessages>();
        }

        public int Id { get; set; }
        public bool Userlevel { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int? PhoneNumber { get; set; }
        public string HomeAdress { get; set; }

        public string City { get; set; }
        public DateTime RegistrationDate { get; set; }

        public ICollection<Listings> Listings { get; set; }
        public ICollection<Reviews> ReviewsBuyer { get; set; }
        public ICollection<Reviews> ReviewsSeller { get; set; }
        public ICollection<Comments> Comments { get; set; }
        public ICollection<Messages> Messages { get; set; }
        public ICollection<UsersHasMessages> UsersHasMessages { get; set; }

    }
}
