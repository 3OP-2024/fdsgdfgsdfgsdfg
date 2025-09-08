using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Receiving.Models
{
    public partial class V_WH_StockCardReport
    {
       
        public string CodeID { get; set; }

        [StringLength(200)]
        public string CodeName { get; set; }

        public int? TotalQtyReceived { get; set; }

        public int? TotalQtyPay { get; set; }

        public double? TotalQuantity { get; set; }

       
        public string LocationID { get; set; }

        [StringLength(2)]
        public string ZoneID { get; set; }
    }
}
