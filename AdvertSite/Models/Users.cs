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
            ReviewsBuyer = new HashSet<Reviews>();
            ReviewsSeller = new HashSet<Reviews>();
            ReceivedMessages = new HashSet<UsersHasMessages>();
            SentMessages = new HashSet<UsersHasMessages>();
        }

        [PersonalData]
        public string HomeAdress { get; set; }
        public string UserLevel { get; set; }
        [PersonalData]
        public string City { get; set; }
        public DateTime RegistrationDate { get; set; }

        public ICollection<Comments> Comments { get; set; }
        public ICollection<Listings> Listings { get; set; }
        public ICollection<Reviews> ReviewsBuyer { get; set; }
        public ICollection<Reviews> ReviewsSeller { get; set; }
        public ICollection<UsersHasMessages> ReceivedMessages { get; set; }
        public ICollection<UsersHasMessages> SentMessages { get; set; }
    }
}
