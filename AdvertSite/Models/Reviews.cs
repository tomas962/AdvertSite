using System;
using System.Collections.Generic;

namespace AdvertSite.Models
{
    public partial class Reviews
    {
        public int Id { get; set; }
        public int Sellerid { get; set; }
        public int Buyerid { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public short? Evaluation { get; set; }

        public Users Buyer { get; set; }
        public Users Seller { get; set; }
    }
}
