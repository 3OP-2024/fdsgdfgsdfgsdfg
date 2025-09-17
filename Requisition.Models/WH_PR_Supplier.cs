using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Requisition.Models
{
    public partial class WH_PR_Supplier
    {
        [Key]
        [StringLength(50)]
        public string SupplierID { get; set; }

        [StringLength(10)]
        public string AccNo { get; set; }

        [StringLength(100)]
        public string SupplierName { get; set; }

        [StringLength(100)]
        public string Address1 { get; set; }

        [StringLength(100)]
        public string Address2 { get; set; }

        [StringLength(50)]
        public string Telephone { get; set; }

        public int? MCRTRM { get; set; }
    }
}
