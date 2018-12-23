using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace AdvertSite.Models
{
    public partial class UsersHasMessages
    {
        public int RecipientId { get; set; }
        public int MessagesId { get; set; }
        public int MessagesSenderId { get; set; }

        public Messages Messages { get; set; }
        public IdentityUser Recipient { get; set; }
    }
}
