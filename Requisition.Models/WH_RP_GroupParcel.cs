using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Requisition.Models
{
    public partial class WH_RP_GroupParcel
    {
        [Key]
        [StringLength(1)]
        public string GroupParcelID { get; set; }

        [StringLength(500)]
        public string GroupParcelName { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(5)]
        public string WarehouseTypeID { get; set; }
    }
}
