using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertSite.Models
{
    [NotMapped]
    public class CreateMessageModel
    {
        public Messages Message { get; set; }
        public string RecipientId { get; set; }
        public ApplicationUser Recipient { get; set; }
    }
}
