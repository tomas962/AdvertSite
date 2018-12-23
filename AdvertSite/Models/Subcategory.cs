using System;
using System.Collections.Generic;

namespace AdvertSite.Models
{
    public partial class Subcategory
    {
        public Subcategory()
        {
            Listings = new HashSet<Listings>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Categoryid { get; set; }

        public Category Category { get; set; }
        public ICollection<Listings> Listings { get; set; }
    }
}
