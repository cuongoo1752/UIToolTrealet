using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIToolTrealet.Models
{
    public class Info
    {
        [Key]
        public int InfoId { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
