using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIToolTrealet.Models
{
    public class Page
    {
        [Key]
        public int PageId { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string OpeningHours { get; set; }
        public string Exhibition { get; set; }
        public string UpcomingEvents { get; set; }
        public string RegistrationDesc { get; set; }
        public string BuyOnlineDesc { get; set; }
        public string BannerURL { get; set; }
        public ICollection<Item> Items { get; set; }


    }
}
