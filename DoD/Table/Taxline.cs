using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Table
{
    public class Taxline
    {
        [Key]
        public int id { get; set; }
        public int food { get; set; }
        public int parts { get; set; }
        public int electric { get; set; }
        public int gas { get; set; }
        public int cash { get; set; }
        public int shadow { get; set; }
        public int time { get; set; }
        public string startdate { get; set; }
        public string? enddate { get; set; }
    }
}