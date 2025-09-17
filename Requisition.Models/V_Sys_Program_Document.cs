using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Requisition.Models
{
    public partial class V_Sys_Program_Document
    {
        [Key]
        [Column(Order = 0)]
         public int DocType { get; set; }

        [StringLength(161)]
        public string DocLink { get; set; }


        public int ProgramID { get; set; }
    }
}
