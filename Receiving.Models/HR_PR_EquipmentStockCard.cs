using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Receiving.Models
{
    public partial class HR_PR_EquipmentStockCard
    {
        [Key]
        [StringLength(7)]
        public string CodeID { get; set; }

        [StringLength(200)]
        public string CodeName { get; set; }

        public int? QtyTotal { get; set; }

        public int? QtyReceived { get; set; }

        public int? QtyPay { get; set; }

        public int? QtyRemaining { get; set; }

        [StringLength(10)]
        public string EditID { get; set; }

        [StringLength(50)]
        public string EditName { get; set; }

        public DateTime? EditDate { get; set; }
    }
}
