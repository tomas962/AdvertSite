﻿using System.ComponentModel.DataAnnotations.Schema;

namespace AdvertSite.Models
{
    public partial class ListingPictures
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PictureId { get; set; }
        public int ListingId { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public Listings Listing { get; set; }
    }
}
