using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Receiving.Models
{
    public partial class V_HR_MT_Department
    {
        [Key]
        [StringLength(6)]
        public string DepartmentID { get; set; }

        [StringLength(50)]
        public string DepartmentNameTH { get; set; }

        [StringLength(50)]
        public string Division { get; set; }
        [StringLength(50)]
        public string DivisionID { get; set; }

        [StringLength(50)]
        public string Department { get; set; }

        [StringLength(50)]
        public string Institution { get; set; }

        public bool? UsageStatus { get; set; }
        [NotMapped]
        public string DepartmentID4Name
        {
            get
            {
                return ((DepartmentID.Length < 4 ? DepartmentID + "-" + Division : DepartmentID.Substring(0, 4) + "-" + Department));
            }
        }
        [NotMapped]
        public string DepartmentIDAndName
        {
            get
            {
                return DepartmentID + "-" + DepartmentNameTH;
            }
        }

    }
}
