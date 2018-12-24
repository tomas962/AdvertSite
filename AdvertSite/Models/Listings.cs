using System;
using System.Collections.Generic;

namespace AdvertSite.Models
{
    public partial class Listings
    {
        public Listings()
        {
            Comments = new HashSet<Comments>();
            ListingPictures = new HashSet<ListingPictures>();
        }

        public int Id { get; set; }
        public string Userid { get; set; }
        public int Subcategoryid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public short? Verified { get; set; }
        public short? Display { get; set; }

        public Subcategory Subcategory { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Comments> Comments { get; set; }
        public ICollection<ListingPictures> ListingPictures { get; set; }
    }
}
