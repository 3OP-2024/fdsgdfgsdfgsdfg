using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Requisition.Models
{
    public partial class Sys_ProgramStatus
    {
        public int ProgramID { get; set; }
        [StringLength(3)]
        public string StatusID { get; set; }
        [StringLength(50)]
        public string StatusName { get; set; }
        public int? ApproveStatusID { get; set; }
        //[StringLength(1)]
        //public string GroupStatus { get; set; }
        [StringLength(10)]
        public string EditID { get; set; }
        [StringLength(50)]
        public string EditName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EditDate { get; set; }
    }
}
