using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Requisition.Models
{
    public partial class WH_MT_ItemName
    {
        [Key]
        [StringLength(10)]
        public string ItemID { get; set; }

        [StringLength(100)]
        public string ItemNameTH { get; set; }

        public double? Quantity { get; set; }

        [StringLength(20)]
        public string UnitNameTH { get; set; }

        public double? PricePerUnit { get; set; }

        [StringLength(2)]
        public string ZoneID { get; set; }

        [StringLength(10)]
        public string LocationID { get; set; }

        [StringLength(10)]
        public string EditID { get; set; }

        [StringLength(50)]
        public string EditName { get; set; }

        public DateTime? EditDate { get; set; }
    }
}
