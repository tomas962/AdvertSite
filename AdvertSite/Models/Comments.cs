﻿using System;

namespace AdvertSite.Models
{
    public partial class Comments
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Userid { get; set; }
        public int Listingid { get; set; }

        public DateTime Date { get; set; }

        public Listings Listing { get; set; }
        public ApplicationUser User { get; set; }
    }
}
