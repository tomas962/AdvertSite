using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AdvertSite.Models
{
    public partial class Category
    {
        public Category()
        {
            Subcategory = new HashSet<Subcategory>();
        }

        public int Id { get; set; }
        [DisplayName("Kategorija")]
        public string Name { get; set; }

        public ICollection<Subcategory> Subcategory { get; set; }
    }
}
