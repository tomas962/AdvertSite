using System.ComponentModel.DataAnnotations.Schema;

namespace AdvertSite.Models
{
    [NotMapped]
    public partial class ListingAndComment
    {
        public Listings Listing { get; set; }
        public Comments Comment { get; set; }
    }
}
