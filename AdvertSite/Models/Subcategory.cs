using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AdvertSite.Models
{
    public partial class Subcategory
    {
        public Subcategory()
        {
            Listings = new HashSet<Listings>();
        }

        public int Id { get; set; }
        [DisplayName("Subkategorija")]
        public string Name { get; set; }
        public int Categoryid { get; set; }
        [DisplayName("Kategorija")]
        public Category Category { get; set; }
        public ICollection<Listings> Listings { get; set; }
    }
}
