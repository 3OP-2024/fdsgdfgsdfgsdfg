using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Requisition.Models
{
    public partial class WH_PR_RequisitionHeader
    {
        [Key]
        [StringLength(11)]
        public string RunningID { get; set; }
        public string RequisitionType { get; set; }
        public string TypeIDName
        {
            get
            {
                 switch (RequisitionType)
                {
                    case "001":
                        return "รับอะไหล่";

                    case "002":
                        return "รับบรรจุภัณฑ์";
                    default:
                        return "";
                }
            }

        }

        [StringLength(10)]
        public string EditID { get; set; }

        [StringLength(50)]
        public string EditName { get; set; }

        public DateTime? EditDate { get; set; }

        [StringLength(6)]
        public string DepartmentID { get; set; }

        [StringLength(3)]
        public string SYS_Status { get; set; }

        [StringLength(10)]
        public string Approve1ID { get; set; }

        [StringLength(50)]
        public string Approve1Name { get; set; }

        public DateTime? Approve1Date { get; set; }

        [StringLength(1)]
        public string Approve1Status { get; set; }

        [StringLength(100)]
        public string Approve1Remark { get; set; }



         [StringLength(10)]
        public string Approve2ID { get; set; }

        [StringLength(50)]
        public string Approve2Name { get; set; }

        public DateTime? Approve2Date { get; set; }

        [StringLength(1)]
        public string Approve2Status { get; set; }

        [StringLength(100)]
        public string Approve2Remark { get; set; }

        [StringLength(10)]
        public string PayID { get; set; }

        [StringLength(50)]
        public string PayName { get; set; }

        public DateTime? PayDate { get; set; }

        [StringLength(1)]
        public string PayStatus { get; set; }
        public void Cancel(string userId, string userName)
        {
            if (SYS_Status != "P01") throw new Exception("ใบรายการนี้ไม่อยู่ในสถานะ ยกเลิกได้");
            //CreateDate = DateTime.Now;
            //CreateName = userName;
            //CreateID = userId;
            SYS_Status = "C01";
        }

        [StringLength(100)]
        public string PayRemark { get; set; }
        public bool IsReject
        {
            get
            {
                bool Check = false;
                if (Approve1Status == "N") { Check = true; } 
                return Check;
            }

        }
        
        [ForeignKey("RunningID")]
        public virtual ICollection<WH_PR_RequisitionDetail> WH_PR_RequisitionDetail { get; set; }

        [ForeignKey("DepartmentID")]
        public virtual V_HR_MT_Department V_HR_MT_Department { get; set; }
      
        public string PayDateTH
        {
            get
            {
                if (PayDate == null)
                {
                    return "";
                }
                return PayDate.Value.ToString("dd/MM/yyyy  HH:mm");
            }
        }public string EditDateTH
        {
            get
            {
                if (EditDate == null)
                {
                    return "";
                }
                return EditDate.Value.ToString("dd/MM/yyyy  HH:mm");
            }
        }
        public string Color_Status
        {
            get
            {
                //text-info text-green text-red text-blue text-warning
                switch (SYS_Status)
                {
                    case "P01":
                        return "text-green";

                    case "P02":
                        return "text-blue";

                    case "P03":
                        return "text-brow";

                    case "P04":
                        return "text-brow";

                    case "C01":
                        return "text-red";

                    default:
                        return "";
                }
            }
        }
        public string statusPageName
        {
            get
            {
                string name = "";
                switch (SYS_Status)
                {
                    case "":
                    case "P01":
                        name = "บันทึกใบเบิกของ";
                        break;
                    case "P02":
                        name = "อนุมัติใบเบิกของ";
                        break;
                    case "P03":
                        name = "พัสดุจ่ายของ";
                        break;
                    case "P04":
                        name = "พัสดุตรวจสอบการจ่าย";
                        break;
                    case "F01":
                        name = "ใบรับของเสร็จสิ้น";
                        break;
                    case "C01":
                        name = "รายการยกเลิก";
                        break;
                    default:
                        name = "บันทึกใบรับของ";
                        break;
                }
                return name;
            }
        }
        public DateTime? RequisitionDate { get; set; }
 
        [StringLength(11)]
        public string RequisitionNo { get; set; }
        [NotMapped]
        public string RequisitionDateStr { get; set; }
        public string Approve1DateTH
        {
            get
            {
                if (Approve1Date == null)
                {
                    return "";
                }
                return Approve1Date.Value.ToString("dd/MM/yyyy  HH:mm");
            }
        }
        public string RequisitionDateTH
        {
            get
            {
                if (RequisitionDate == null)
                {
                    return "";
                }
                return RequisitionDate.Value.ToString("dd/MM/yyyy");
            }
        }
        public string DepartmentID4Name
        {
            get
            {
                if (V_HR_MT_Department == null) { return ""; }
                return ((V_HR_MT_Department.DepartmentID.Length < 4 ? V_HR_MT_Department?.DepartmentID + "-" + V_HR_MT_Department?.Division : V_HR_MT_Department.DepartmentID.Substring(0, 4) + "-" + V_HR_MT_Department.Department));
            }
        }
        public void Create(string userId, string userName, string GetRunning, bool send)
        {
            EditDate = DateTime.Now;
            EditName = userName;
            EditID = userId;
            RunningID = GetRunning;
            SYS_Status = (send == true ? "P02" : "P01");
        }
        public void Edit(string userId, string userName, WH_PR_RequisitionHeader item, bool send)
        {
            EditDate = DateTime.Now;
            EditName = userName;
            EditID = userId;

            DepartmentID = item.DepartmentID;
            RequisitionDate = item.RequisitionDate;
            RequisitionNo = item.RequisitionNo; 
            if (send)
            {
                SYS_Status = "P02";
            }
            else
            {
                SYS_Status = "P01";
            }

        }
        public void Pay(string userId, string userName, WH_PR_RequisitionHeader item, bool send)
        {
            PayDate = DateTime.Now;
            PayName = userName;
            PayID = userId; 
            if (send)
            {
                SYS_Status = "P04";
            }
            else
            {
                SYS_Status = "P03";
            }
            WH_PR_RequisitionDetail.ToList().ForEach(l =>
            {

                l.SerialNo = item.WH_PR_RequisitionDetail.FirstOrDefault(d => d.ItemID == l.ItemID)?.SerialNo ?? "";
                l.JobID = item.WH_PR_RequisitionDetail.FirstOrDefault(d => d.ItemID == l.ItemID)?.JobID ?? "";
                l.QtyRequisition = item.WH_PR_RequisitionDetail.FirstOrDefault(d => d.ItemID == l.ItemID)?.QtyRequisition ?? 0;

            });

        }
    }
}
