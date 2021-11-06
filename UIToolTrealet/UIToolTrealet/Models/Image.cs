using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIToolTrealet.Models
{
    public class Image
    {
        [Key]
        public int ImageId { get; set; }
        
        public string ImageCodeTrealet { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
