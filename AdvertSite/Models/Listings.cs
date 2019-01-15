using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        [DisplayName("Kategorija")]
        [Required]
        public int Subcategoryid { get; set; }
        [DisplayName("Pavadinimas")]
        [Required]
        public string Name { get; set; }
        [DisplayName("Aprašymas")]
        [Required]
        public string Description { get; set; }
        [DisplayName("Kaina")]
        public double Price { get; set; }
        public int Quantity { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
        public DateTime Date { get; set; }
        public short? Verified { get; set; }
        public short? Display { get; set; }
        public double? GoogleLatitude { get; set; }
        public double? GoogleLongitude { get; set; }

        [DisplayName("Radius (km)")]
        [DataType("Double")]
        public double? GoogleRadius { get; set; }

        [DisplayName("Subkategorija")]
        public Subcategory Subcategory { get; set; }
        [DisplayName("Vartotojas")]
        public ApplicationUser User { get; set; }
        public ICollection<Comments> Comments { get; set; }
        public ICollection<ListingPictures> ListingPictures { get; set; }
    }
}
