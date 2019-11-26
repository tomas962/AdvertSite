using System.ComponentModel.DataAnnotations.Schema;

namespace AdvertSite.Models
{
    [NotMapped]
    public class CreateMessageModel
    {
        public Messages Message { get; set; }
        public string RecipientId { get; set; }
        public ApplicationUser Recipient { get; set; }
        public UsersHasMessages UsersHasMessages { get; set; }
    }
}
