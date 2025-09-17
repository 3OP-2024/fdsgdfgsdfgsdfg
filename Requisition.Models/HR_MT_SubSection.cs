using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Requisition.Models
{
    public partial class HR_MT_SubSection
    {
        [StringLength(3)]
        public string SubSectionID { get; set; }
        [StringLength(6)]
        public string DepartmentID { get; set; }
        [StringLength(10)]
        public string EditID { get; set; }
        [StringLength(50)]
        public string EditName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EditDate { get; set; }
        public bool? UsageStatus { get; set; }

        [ForeignKey("DepartmentID")]
        [InverseProperty("HR_MT_SubSection")]
        public virtual HR_MT_Department Department { get; set; }
    }
}
