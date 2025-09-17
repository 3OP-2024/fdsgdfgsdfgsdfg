using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Requisition.Models
{
    public partial class V_SYS_USER_ROLE_PRIVILEGE
    {

        [StringLength(10)]
        public string UserID { get; set; }

        [StringLength(70)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string UserLogin { get; set; }

        [StringLength(6)]
        public string GroupDepartment { get; set; }

        [StringLength(3)]
        public string SYS_Status { get; set; }



        public int RoleID { get; set; }

        public int ProgramID { get; set; }

        [StringLength(4)]
        public string RightCode { get; set; }
    }
}
