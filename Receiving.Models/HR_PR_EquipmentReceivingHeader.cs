using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Receiving.Models
{
    public partial class HR_PR_EquipmentReceivingHeader
    {
        [Key]
        [StringLength(10)]
        public string RunningID { get; set; }
        public bool IsUpAs400 { get; set; } = false;

        public bool IsReject
        {
            get
            {
                bool Check = false;
                if (Approve1Status == "N") { Check = true; }
                if (Approve2Status == "N") { Check = true; }
                return Check;
            }

        }
        public void Cancel(string userId, string userName)
        {
            if (SYS_Status != "P01") throw new Exception("ใบรายการนี้ไม่อยู่ในสถานะ ยกเลิกได้");
            //CreateDate = DateTime.Now;
            //CreateName = userName;
            //CreateID = userId;
            SYS_Status = "C01";
        }

        [StringLength(15)]
        public string InvoiceNo { get; set; }

        public DateTime? InvoiceDate { get; set; }
        public string InvoiceDateTH
        {
            get
            {
                if (InvoiceDate == null)
                {
                    return "";
                }
                return InvoiceDate.Value.ToString("dd/MM/yyyy");
            }
        }

        [StringLength(15)]
        public string TaxInvNo { get; set; }

        [StringLength(15)]
        public string TaxIDInv { get; set; }

       
        public int BranchID { get; set; }
 
        public DateTime? TaxInvDate { get; set; }
        public string TaxInvDateTH
        {
            get
            {
                if (TaxInvDate == null)
                {
                    return "";
                }
                return TaxInvDate.Value.ToString("dd/MM/yyyy");
            }
        }

        [StringLength(6)]
        public string DepartmentID { get; set; }

        [StringLength(3)]
        public string SYS_Status { get; set; }

        [StringLength(10)]
        public string EditID { get; set; }

        [StringLength(50)]
        public string EditName { get; set; }

        public DateTime? EditDate { get; set; }
        public string EditDateTH
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
        [StringLength(15)]
        public string VendorID { get; set; }
        public string VendorName { get; set; }

        [StringLength(10)]
        public string Approve1ID { get; set; }

        [StringLength(50)]
        public string Approve1Name { get; set; }
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
                    case "P01":
                        name = "บันทึกใบรับของ";
                        break;
                    case "P02":
                        name = "ตรวจสอบใบรับของ";
                        break;
                    case "P03":
                        name = "อัปเดทเข้าระบบ AS400";
                        break; 
                    case "F01":
                        name = "ใบรับของเสร็จสิ้น";
                        break;
                    case "C01":
                        name = "รายการยกเลิก";
                        break;
                    default:
                        break;
                }
                return name;
            }
        }
        public DateTime? Approve1Date { get; set; }
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

        [StringLength(1)]
        public string Approve1Status { get; set; }

        [StringLength(100)]
        public string Approve1Remark { get; set; }

        [StringLength(10)]
        public string Approve2ID { get; set; }

        [StringLength(50)]
        public string Approve2Name { get; set; }

        public DateTime? Approve2Date { get; set; }
        public string Approve2DateTH
        {
            get
            {
                if (Approve2Date == null)
                {
                    return "";
                }
                return Approve2Date.Value.ToString("dd/MM/yyyy  HH:mm");
            }
        }

        [StringLength(1)]
        public string Approve2Status { get; set; }

        [StringLength(1000)]
        public string Approve2Remark { get; set; }
        public string RequestNo { get; set; }

        public DateTime? ReceivingDate { get; set; }
        public string ReceivingDateTH
        {
            get
            {
                if (ReceivingDate == null)
                {
                    return "";
                }
                return ReceivingDate.Value.ToString("dd/MM/yyyy");
            }
        }
        [StringLength(5)]
        public string ReceivingType { get; set; }
        public void Create(string userId, string userName, string GetRunning ,bool send)
        {
            EditDate = DateTime.Now;
            EditName = userName;
            EditID = userId; 
            RunningID = GetRunning;
            SYS_Status = (send == true ? "P02" : "P01"); 
        }
        public void Edit(string userId, string userName, HR_PR_EquipmentReceivingHeader item , bool send)
        {
            EditDate = DateTime.Now;
            EditName = userName;
            EditID = userId; 

            VendorID = item.VendorID;
            ReceivingDate = item.ReceivingDate;

            InvoiceDate = item.InvoiceDate;
            InvoiceNo = item.InvoiceNo;

            TaxInvDate = item.TaxInvDate; 
            TaxInvNo = item.TaxInvNo;
            TaxIDInv = item.TaxIDInv;

            BranchID = item.BranchID;
            if (send)
            {
                SYS_Status = "P02";
            }
            else
            {
                SYS_Status = "P01";
            }
            
        }
        public string TypeIDName
        {
            get
            {
                //text-info text-green text-red text-blue text-warning
                switch (ReceivingType)
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

        [ForeignKey("RunningID")] 
        public virtual ICollection<HR_PR_EquipmentReceivingDetail> HR_PR_EquipmentReceivingDetail { get; set; }

        [ForeignKey("DepartmentID")]
        public virtual V_HR_MT_Department V_HR_MT_Department { get; set; }
        public string DepartmentID4Name
        {
            get
            {
                if (V_HR_MT_Department == null) { return ""; }
                return ((V_HR_MT_Department.DepartmentID.Length < 4 ? V_HR_MT_Department?.DepartmentID + "-" + V_HR_MT_Department?.Division : V_HR_MT_Department.DepartmentID.Substring(0, 4) + "-" + V_HR_MT_Department.Department));
            }
        }
         
    }
}
