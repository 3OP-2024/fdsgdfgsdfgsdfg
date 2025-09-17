using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Requisition.Models
{
    public partial class V_SYS_USER
    {
        [Key]
        [StringLength(10)]
        public string UserID { get; set; }

        [StringLength(70)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string UserLogin { get; set; }

        [StringLength(6)]
        public string DepartmentID { get; set; }

        [StringLength(50)]
        public string DepartmentNameTH { get; set; }

        [StringLength(70)]
        public string UserEmail { get; set; }

        [StringLength(1)]
        public string UserStatus { get; set; }
    }
}
