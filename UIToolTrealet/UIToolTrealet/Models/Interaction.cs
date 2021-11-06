using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIToolTrealet.Models
{
    public class Interaction
    {
        [Key]
        public int InteractionId { get; set; }
        public string  Question { get; set; }
        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string AnswerC { get; set; }
        public string AnswerD { get; set; }
        public string TrueAnswer { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
