using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Table
{
    public class Color
    {
        [Key]
        public long guild { get; set; }
        public long food { get; set; }
        public long parts { get; set; }
        public long electric { get; set; }
        public long gas { get; set; }
        public long cash { get; set; }
        public long shadow { get; set; }
    }
}
