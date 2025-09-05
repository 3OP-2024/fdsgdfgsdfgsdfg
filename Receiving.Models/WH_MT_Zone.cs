using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Receiving.Models
{
    public partial class WH_MT_Zone
    {
        [Key] 
        public string ZoneID { get; set; }

        [StringLength(100)]
        public string ZoneName { get; set; }

        [StringLength(10)]
        public string EditID { get; set; }

        [StringLength(50)]
        public string EditName { get; set; }

        public DateTime? EditDate { get; set; }

        public bool? UsageStatus { get; set; }
    }
}
