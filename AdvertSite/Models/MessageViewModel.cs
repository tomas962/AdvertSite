using System.ComponentModel.DataAnnotations.Schema;

namespace AdvertSite.Models
{
    [NotMapped]
    public class MessageViewModel
    {
        public string CssClassName { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
