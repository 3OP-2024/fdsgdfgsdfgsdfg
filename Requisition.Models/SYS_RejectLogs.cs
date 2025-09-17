using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Requisition.Models
{
    public partial class SYS_RejectLogs
    {
        public SYS_RejectLogs()
        {

        }
        public int ID { get; set; }
        public int? ProgramID { get; set; }
        [StringLength(20)]
        public string RefID { get; set; }
        [StringLength(2000)]
        public string Details { get; set; }
        [StringLength(100)]
        public string ByName { get; set; }

        public DateTime? ByDate { get; set; }
        public string ByDateTH
        {
            get
            {
                if (ByDate == null)
                {
                    return "";
                }
                return ByDate.Value.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }
        public SYS_RejectLogs(int programid, string refid, string approvestauts, string remark, string name)
        {
            ProgramID = programid;
            RefID = refid;
            Details = $"{approvestauts} : {remark}";
            ByName = name;
            ByDate = DateTime.Now;

        }
    }
}
