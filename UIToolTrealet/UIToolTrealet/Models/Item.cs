using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIToolTrealet.Models
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }
        
        public string Title { get; set; }
        public string Desc { get; set; }

        public string TitleImage { get; set; }

        public ICollection<Info> Infoes { get; set; }
        public ICollection<Image> Images { get; set; }
        public ICollection<Video> Videos { get; set; }
        public ICollection<Interaction> Interactions { get; set; }

        public int PageId { get; set; }

        public Page Page { get; set; }
    }
}
