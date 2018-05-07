using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CgData.Models
{
    public class Store
    {
        public string Account { get; set; }
        public string SubAccount { get; set; }
        [Key]
        public string Name { get; set; }
        public int Level { get;set; }
        public string Job { get; set; }
        public string Bag { get; set; }
        public string Bank { get; set; }
    }
}
