using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Table
{
    public class Data_bank
    {
        public long? id { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int lineid { get; set; }
        public string name { get; set; }
        public string date { get; set; }
        public string type { get; set; }
        public double food { get; set; }
        public double parts { get; set; }
        public double electric { get; set;}
        public double gas { get; set; }
        public double cash { get; set; }
        public double shadow { get; set; }
    }
}
