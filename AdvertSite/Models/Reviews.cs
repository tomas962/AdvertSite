using System;
using System.Collections.Generic;

namespace AdvertSite.Models
{
    public partial class Reviews
    {
        public int Id { get; set; }
        public string Sellerid { get; set; }
        public string Buyerid { get; set; }
        public DateTime? Date { get; set; }
        public string Comment { get; set; }
        public short? Evaluation { get; set; }

        public ApplicationUser Buyer { get; set; }
        public ApplicationUser Seller { get; set; }
    }
}
