using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvertSite.Models
{
    [NotMapped]
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

        //public bool Userlevel { get; set; }
        //public string Password { get; set; }
        //public string HomeAdress { get; set; }

        //public string City { get; set; }
        //public DateTime RegistrationDate { get; set; }

        public ICollection<Listings> Listings { get; set; }
        public ICollection<Reviews> ReviewsBuyer { get; set; }
        public ICollection<Reviews> ReviewsSeller { get; set; }
        public ICollection<Comments> Comments { get; set; }
        public ICollection<Messages> Messages { get; set; }
        public ICollection<UsersHasMessages> UsersHasMessages { get; set; }

    }
}
