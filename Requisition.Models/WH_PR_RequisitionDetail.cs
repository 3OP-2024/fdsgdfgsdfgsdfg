using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Requisition.Models
{
    public partial class WH_PR_RequisitionDetail
    {
        [Key]
        [StringLength(13)]
        public string RunningDetailID { get; set; }
        public string ItemOrder { get; set; }
        public string SerialNo	  { get; set; }

        [StringLength(11)]
        public string RunningID { get; set; }

        [StringLength(7)]
        public string ItemID { get; set; }
        [ForeignKey("ItemID")]
        public virtual WH_MT_ItemName WH_MT_ItemName { get; set; }
        [ForeignKey("JobID")]
        public virtual V_WH_JobDepartment V_WH_JobDepartment { get; set; }
        public int? Quantity { get; set; }

        public int? QtyRequisition { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        [StringLength(7)]
        public string JobID { get; set; }
        public void create(string ID, string DetailID)
        {
            RunningID = ID;
            RunningDetailID = DetailID;
        }
    }
}
