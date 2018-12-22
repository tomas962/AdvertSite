using System;
using System.Collections.Generic;

namespace AdvertSite.Models
{
    public partial class ListingPictures
    {
        public int PictureId { get; set; }
        public int ListingId { get; set; }

        public Listings Listing { get; set; }
    }
}
