using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Receiving.Models
{
    
   [Table("WH_MT_Branch")]

    public partial class HR_PR_EquipmentBranch
    {
        [Key]
        [StringLength(10)]
        public string BranchID { get; set; }

        [StringLength(100)]
        public string BranchName { get; set; }
 
        public bool? UsageStatus { get; set; }

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
        public void Edit(string userId, string userName, HR_PR_EquipmentBranch item)
        {
            EditDate = DateTime.Now;
            EditName = userName;
            EditID = userId;
            UsageStatus = item.UsageStatus;
            BranchName = item.BranchName;  
        }

        public string BranchIDAndName
        {
            get
            {
                if (BranchID != null)
                {
                    return BranchID +"-" + BranchName;
                }
                return "";  
            }
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
