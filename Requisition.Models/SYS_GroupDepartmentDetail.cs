using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Requisition.Models
{
    public partial class SYS_GroupDepartmentDetail
    {
        [StringLength(6)]
        public string GroupDepID { get; set; }
        [StringLength(6)]
        public string DepartmentID { get; set; }
        [StringLength(10)]
        public string EditID { get; set; }
        [StringLength(50)]
        public string EditName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EditDate { get; set; }
    }
}
