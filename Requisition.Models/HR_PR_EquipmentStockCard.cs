using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Requisition.Models
{
    
   [Table("WH_MT_StockCard")]
    public partial class HR_PR_EquipmentStockCard
    {
        
        [StringLength(7)]
        public string CodeID { get; set; }
        
        public string RunningID { get; set; }


        public string CodeIDName
        {
            get
            {
                if (!string.IsNullOrEmpty(CodeID))
                {
                    return CodeID + "-" + CodeName;
                }
                return "";
            }

        }

        [StringLength(200)]
        public string CodeName { get; set; }
        public string SerialNo { get; set; }
        public virtual HR_PR_EquipmentInventory Inventory { get; set; }


        public int? QtyReceived { get; set; }

        public int? QtyPay { get; set; }

       
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
                return EditDate.Value.ToString("dd/MM/yyyy");
            }
        }
    }
}
