using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;


namespace CgData.Models
{
    public class Stall
    {
        public Stall()
        {
            UpdateTime=DateTime.Now;
        }
        [Required]
        public string PlayerName { get; set; }
        public string StallName { get; set; }
        public string Description { get; set; }
        [Key, Column(Order=0)]
        public int X { get; set; }
        [Key, Column(Order=1)]
        public int Y { get; set; }
        [Required]
        public string Line { get; set; }
        public DateTime UpdateTime { get; set; }
        public List<StallItem> Items { get; set; }
    }
}
