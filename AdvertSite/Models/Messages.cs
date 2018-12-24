using System;
using System.Collections.Generic;

namespace AdvertSite.Models
{
    public partial class Messages
    {
        public Messages()
        {
            UsersHasMessages = new HashSet<UsersHasMessages>();
        }

        public int Id { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public string SenderId { get; set; }

        public ApplicationUser Sender { get; set; }
        public ICollection<UsersHasMessages> UsersHasMessages { get; set; }
    }
}
