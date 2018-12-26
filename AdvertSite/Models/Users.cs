using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace AdvertSite.Models
{
    public partial class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Comments = new HashSet<Comments>();
            Listings = new HashSet<Listings>();
            Messages = new HashSet<Messages>();
            ReviewsBuyer = new HashSet<Reviews>();
            ReviewsSeller = new HashSet<Reviews>();
            UsersHasMessages = new HashSet<UsersHasMessages>();
        }

        [PersonalData]
        public string HomeAdress { get; set; }
        public string UserLevel { get; set; }
        [PersonalData]
        public string City { get; set; }
        public DateTime RegistrationDate { get; set; }

        public ICollection<Comments> Comments { get; set; }
        public ICollection<Listings> Listings { get; set; }
        public ICollection<Messages> Messages { get; set; }
        public ICollection<Reviews> ReviewsBuyer { get; set; }
        public ICollection<Reviews> ReviewsSeller { get; set; }
        public ICollection<UsersHasMessages> UsersHasMessages { get; set; }
    }
}
