using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DoD
{
    public class Data_bank
    {

        public long id { get; set; }
        [Key]
        public int lineid { get; set; }
        public string name { get; set; }
        public string date { get; set; }
        public string guild { get; set; }
        public string lotteryweek { get; set; }
        public string type { get; set; }
        public int food { get; set; }
        public int parts { get; set; }
        public int electric { get; set;}
        public int gas { get; set; }
        public int cash { get; set; }
        public int shadow { get; set; }
    }
}
