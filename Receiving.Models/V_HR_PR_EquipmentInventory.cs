using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Receiving.Models
{
     
    [Table("V_WH_PR_Inventory")]

    public partial class V_HR_PR_EquipmentInventory
    {

        [Key]
        [StringLength(10)]
        public string CodeID { get; set; }
        public string CodeName { get; set; }
        public string Unit { get; set; }
        public int TotalQtyReceived { get; set; } = 0; 
        public string ReceivingType { get; set; }
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
    }
}
