using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertSite.Models
{
    [NotMapped]
    public class MessageViewModel
    {
        public string CssClassName { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
