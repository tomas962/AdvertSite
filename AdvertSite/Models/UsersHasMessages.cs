using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvertSite.Models
{
    public partial class UsersHasMessages
    {
        public string RecipientId { get; set; }
        public int MessagesId { get; set; }
        public string SenderId { get; set; }

        public Messages Messages { get; set; }

        public ApplicationUser Recipient { get; set; }
        public ApplicationUser Sender { get; set; }
    }
}
