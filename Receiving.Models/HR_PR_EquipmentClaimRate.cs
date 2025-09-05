using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Receiving.Models
{
    public partial class HR_PR_EquipmentClaimRate
    {
        [Key]
        [StringLength(10)]
        public string ClaimRateID { get; set; }

        public int? ClaimRateNumber { get; set; }

        public bool? UsageStatus { get; set; }
        public bool? DefaultItem { get; set; }

        [StringLength(10)]
        public string EditID { get; set; }

        [StringLength(50)]
        public string EditName { get; set; }

        public DateTime? EditDate { get; set; }

      

        public void Add(string userId, string userName)
        {
            EditDate = DateTime.Now;
            EditName = userName;
            EditID = userId;
        }
        public void Edit(string userId, string userName, HR_PR_EquipmentClaimRate item)
        {
            EditDate = DateTime.Now;
            EditName = userName;
            EditID = userId;
            UsageStatus = item.UsageStatus;
            ClaimRateNumber = item.ClaimRateNumber;
            DefaultItem = item.DefaultItem; 
        }

        public string EditDateTH
        {
            get
            {
                if (EditDate == null)
                {
                    return "";
                }
                return EditDate.Value.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }
    }
}
