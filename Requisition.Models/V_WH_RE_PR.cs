using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Requisition.Models
{
    public partial class V_WH_RE_PR
    {
       
        public string RequestNo { get; set; }
        public string ShopAddressNo { get; set; }

    
        public DateTime RequestDate { get; set; }

   
        public string DepartmentID { get; set; }
        [NotMapped]
        public string ClaimRateID { get; set; }
        [NotMapped]
        public int? ClaimRateNumber { get; set; }

        [StringLength(60)]
        public string DepartmentName { get; set; }
 
         public int ItemOrder { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(500)]
        public string CodeName { get; set; }

        public double? Quantity { get; set; }

        [StringLength(50)]
        public string Unit { get; set; }

        public DateTime? UseDate { get; set; }

        [StringLength(1000)]
        public string Remark { get; set; }

         
        public string PONO { get; set; }

      
        public string PODate { get; set; }

      
        public string ShopName { get; set; }
        public string ShopID { get; set; }

       
        public decimal UnitPrice { get; set; }

        public decimal Amount { get; set; }
        public decimal VatAmount { get; set; }
    }
}
