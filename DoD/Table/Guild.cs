using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DoD
{
    public class Guild
    {
        [Key]
        public long guild { get; set; }
        public long discordid { get; set; }
        public string startdate { get; set; }
        public int exemption { get; set; }
    }
}
