using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvertSite.Models
{
    public partial class ListingPictures
    {
        
        [Key]
        public int PictureId { get; set; }


        public int ListingId { get; set; }

        public Listings Listing { get; set; }
    }
}
