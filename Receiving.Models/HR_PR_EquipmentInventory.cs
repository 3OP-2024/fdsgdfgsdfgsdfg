using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Receiving.Models
{
    
    [Table("WH_PR_Inventory")]

    public partial class HR_PR_EquipmentInventory
    {
        [Key]
        [StringLength(10)]
        public string SerialNo { get; set; }

        [StringLength(5)]
        public string ReceivingType { get; set; }
        public string CheckSheetNo { get; set; }

        [StringLength(7)]
        public string CodeID { get; set; } 
        public string CodeName { get; set; }

        public string CodeIDName
        {
            get
            {
                if (string.IsNullOrEmpty(CodeID))
                {
                    return CodeID + "-" + CodeName;
                }
                return "";
            }

        }

        [StringLength(10)]
        public string LotNo { get; set; }
        public string Unit { get; set; }
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
        public string statusPageName
        {
            get
            {
                string name = "";
                switch (SYS_Status)
                {
                    case "P01":
                        name = "ระบุสถานที่จัดเก็บ";
                        break;
                    case "P02":
                        name = "รอจัดเก็บ";
                        break;
                   
                    case "F01":
                        name = "เสร็จสิ้น";
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
        public int? QtyReceived { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpireDate { get; set; }
        public string ExpireDateTH
        {
            get
            {
                if (ExpireDate == null)
                {
                    return "";
                }
                return ExpireDate.Value.ToString("dd/MM/yyyy");
            }
        }

        [StringLength(10)]
        public string LocationID { get; set; }

        [ForeignKey("LocationID")]
        public virtual WH_MT_Location WH_MT_Location { get; set; }

        [StringLength(10)]
        public string RunningID { get; set; }

        [StringLength(3)]
        public string SYS_Status { get; set; }

        [StringLength(10)]
        public string EditID { get; set; }

        [StringLength(50)]
        public string EditName { get; set; }

        public DateTime? EditDate { get; set; }


        [StringLength(10)]
        public string CreateID { get; set; }

        [StringLength(50)]
        public string CreateName { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
