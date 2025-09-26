using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Requisition.Models
{
    public partial class V_WH_RequisitionReceiving
    {
         
        [StringLength(11)]
        public string RunningID { get; set; }

        [StringLength(10)]
        public string RequisitionNo { get; set; }

        [StringLength(7)]
        public string ProductCode { get; set; }
        public string WarehouseTypeID { get; set; }

        [StringLength(200)]
        public string ProductName { get; set; }

        public int? Quantity { get; set; }

        public int? ReceivedQty { get; set; }

        [StringLength(20)]
        public string UnitName { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        [StringLength(6)]
        public string DepartmentID { get; set; }

        [StringLength(50)]
        public string DepartmentNameTH { get; set; }

        [StringLength(15)]
        public string VendorID { get; set; }

        [StringLength(200)]
        public string VendorName { get; set; }

        public DateTime? ReceivingDate { get; set; }
    }
}
