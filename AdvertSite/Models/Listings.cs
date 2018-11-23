using System;
using System.Collections.Generic;

namespace AdvertSite.Models
{
    public partial class Listings
    {
        public int Id { get; set; }
        public int Userid { get; set; }
        public int Subcategoryid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public short Verified { get; set; }
        public short Display { get; set; }

        public Subcategory Subcategory { get; set; }
        public Users User { get; set; }
    }
}
