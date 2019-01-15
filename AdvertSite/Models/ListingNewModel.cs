using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertSite.Models
{
    [NotMapped]
    public class ListingNewModel
    {
        [DisplayName("Kategorija")]
        [Required]
        public int Subcategoryid { get; set; }
        [DisplayName("Pavadinimas")]
        [Required]
        public string Name { get; set; }
        [DisplayName("Aprašymas")]
        [Required]
        public string Description { get; set; }
        [DisplayName("Kaina (€)")]
        public double Price { get; set; }

        public double? GoogleLatitude { get; set; }
        public double? GoogleLongitude { get; set; }
        [DisplayName("Skersmuo (km)")]
        [DataType("Double")]
        public double? GoogleRadius { get; set; }

        [DisplayName("Nuotrauka")]
        public IEnumerable<IFormFile> ListingPictures { get; set; }
    }
}
