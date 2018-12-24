using System;
using System.Collections.Generic;

namespace AdvertSite.Models
{
    public partial class UsersHasMessages
    {
        public string RecipientId { get; set; }
        public int MessagesId { get; set; }
        public string MessagesSenderId { get; set; }

        public Messages Messages { get; set; }
        public ApplicationUser Recipient { get; set; }
    }
}
