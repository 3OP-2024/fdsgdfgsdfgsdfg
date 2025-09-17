using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Requisition.Models
{
    public partial class V_WH_JobDepartment
    {
        [Key]
        [StringLength(7)]
        public string JobID { get; set; }

        [StringLength(50)]
        public string JobName { get; set; }

        [StringLength(2)]
        public string Profit_Cost_Center { get; set; }

        [StringLength(2)]
        public string Location { get; set; }

        [StringLength(10)]
        public string JobAccountingCode { get; set; }

        [StringLength(6)]
        public string DepartmentID { get; set; }

        [StringLength(50)]
        public string DepartmentNameTH { get; set; }

        [StringLength(50)]
        public string DepartmentNameEN { get; set; }

        [StringLength(10)]
        public string DeptAccountingCode { get; set; }

        public bool? DiligenceStatus { get; set; }

        public bool? AllowanceType { get; set; }

        [StringLength(10)]
        public string EditID { get; set; }

        [StringLength(50)]
        public string EditName { get; set; }

        public DateTime? EditDate { get; set; }

        public bool? UsageStatus { get; set; }

        public int? DirectionTypeID { get; set; }

        [StringLength(50)]
        public string DepartmentNameKM { get; set; }

        [StringLength(50)]
        public string DepartmentNameMM { get; set; }
    }
}
