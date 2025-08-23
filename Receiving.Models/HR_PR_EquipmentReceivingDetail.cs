using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Receiving.Models
{
    public partial class HR_PR_EquipmentReceivingDetail
    {
        [Key]
         
        public string RunningDetailID { get; set; }

        [Required]
        [StringLength(10)]
        public string RunningID { get; set; }

        [StringLength(6)]
        public string CodeID { get; set; }

        [StringLength(200)]
        public string CodeName { get; set; }

        [StringLength(10)]
        public string ItemOrder { get; set; }

        public decimal? Unitprice { get; set; }

        public decimal? Amount { get; set; }

        public decimal? VatAmount { get; set; }

        [StringLength(20)]
        public string Unit { get; set; }

        [StringLength(20)]
        public string ShopID { get; set; }

        [StringLength(100)]
        public string ShopName { get; set; }

        public int? Quantity { get; set; }

        public int? QtyReceived { get; set; }

        [StringLength(6)]
        public string ClaimRateID { get; set; }

        [StringLength(10)]
        public string LotNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpireDate { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }
        
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
        public void create(string ID ,  string DetailID)
        {
            RunningID = ID;
            RunningDetailID = DetailID;
        }

    }
}
