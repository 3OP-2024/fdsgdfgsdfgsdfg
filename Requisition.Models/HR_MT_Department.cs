using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Requisition.Models
{
    public partial class HR_MT_Department
    {
        public HR_MT_Department()
        {
            HR_MT_SubSection = new HashSet<HR_MT_SubSection>();
        }

        [Key]
        [StringLength(6)]
        public string DepartmentID { get; set; }
        [StringLength(50)]
        public string DepartmentNameTH { get; set; }
        [StringLength(50)]
        public string DepartmentNameEN { get; set; }
        [StringLength(10)]
        public string AccountingCode { get; set; }
        public bool? DiligenceStatus { get; set; }
        public bool? AllowanceType { get; set; }
        [StringLength(10)]
        public string EditID { get; set; }
        [StringLength(50)]
        public string EditName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EditDate { get; set; }
        public bool? UsageStatus { get; set; }

        [InverseProperty("Department")]
        public virtual ICollection<HR_MT_SubSection> HR_MT_SubSection { get; set; }
        public string DepartmentIDAndName
        {
            get
            {
                return DepartmentID + " : " + DepartmentNameTH;
            }
        }
    }
}
