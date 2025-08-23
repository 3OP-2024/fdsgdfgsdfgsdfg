using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Receiving.Models
{
    public partial class Sys_Program
    {
        [Key]
         public int ProgramID { get; set; }

        [StringLength(50)]
        public string ProgramNameTH { get; set; }

        [StringLength(50)]
        public string ProgramNameEN { get; set; }

        [StringLength(10)]
        public string EditID { get; set; }

        [StringLength(50)]
        public string EditName { get; set; }

        public DateTime? EditDate { get; set; }

        [StringLength(50)]
        public string Logo { get; set; }

        [StringLength(150)]
        public string Link { get; set; }

        [StringLength(4)]
        public string LinkID { get; set; }

        public string ProgramIDAndName
        {
            get
            {
                return ProgramID + " : " + ProgramNameTH;
            }
        }
    }
}
