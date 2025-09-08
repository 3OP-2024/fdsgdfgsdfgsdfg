using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Receiving.Models
{
    public partial class WH_MT_Location
    {
        [Key]
        [StringLength(10)]
        public string LocationID { get; set; }

        [StringLength(100)]
        public string LocationName { get; set; }
        public string LocationIDAndName
        {
            get
            {
                if (LocationID != null)
                {
                    return LocationID + "-" + LocationName;
                }
                return "";
            }
        }

        [StringLength(2)]
        public string ZoneID { get; set; }

        public bool? UsageStatus { get; set; }

        public int? Capacity { get; set; }

        [StringLength(10)]
        public string EditID { get; set; }

        [StringLength(50)]
        public string EditName { get; set; }

        public DateTime? EditDate { get; set; }
    }
}
