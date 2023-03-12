using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Table
{
    public class Config
    {
        [Key]
        public long guild { get; set; }
        public string guildname { get; set; }
        public int taxactive { get; set; }
    }
}