using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertSite.Models
{
    [NotMapped]
    public partial class ListingAndComment
    {
        public Listings Listing { get; set; }
        public Comments Comment { get; set; }
    }
}
