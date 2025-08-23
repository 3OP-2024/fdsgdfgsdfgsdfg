using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Receiving.Models
{
    public partial class HR_PR_EquipmentZone
    {
        [Key]
        [StringLength(2)]
        public string ZoneID { get; set; }

        [StringLength(100)]
        public string ZoneName { get; set; }

        [StringLength(10)]
        public string EditID { get; set; }

        [StringLength(50)]
        public string EditName { get; set; }
        public bool UsageStatus { get; set; }
     

        public DateTime? EditDate { get; set; }

     
        public void Create(string userId, string userName)
        {
            EditDate = DateTime.Now;
            EditName = userName;
            EditID = userId; 
        }
        [ForeignKey("ZoneID")]
        public virtual ICollection<HR_PR_EquipmentLocation> HR_PR_EquipmentLocation { get; set; }
        public void Edit(string userId, string userName, HR_PR_EquipmentZone newItem)
        {
            EditDate = DateTime.Now;
            EditName = userName;
            EditID = userId;
            ZoneName = newItem.ZoneName;
            UsageStatus = newItem.UsageStatus;

            // ของเก่า (จาก DB)
            var oldLocations = HR_PR_EquipmentLocation.ToList();

            // ของใหม่ (จาก UI)
            var newLocations = newItem.HR_PR_EquipmentLocation.ToList();

            // 1) หาที่ถูกลบ
            var removed = oldLocations
                .Where(o => !newLocations.Any(n => n.LocationID == o.LocationID))
                .ToList();

            // 2) หาที่ถูกเพิ่ม
            var added = newLocations
                .Where(n => !oldLocations.Any(o => o.LocationID == n.LocationID))
                .ToList();

            // 3) หาที่ถูกแก้ไข
            var updated = oldLocations
                .Where(o => newLocations.Any(n => n.LocationID == o.LocationID && (n.LocationName != o.LocationName || n.Capacity != o.Capacity || n.UsageStatus != o.UsageStatus)))
                .ToList();

            // ✅ อัปเดตข้อมูลจริง
            foreach (var add in added)
            {
                HR_PR_EquipmentLocation.Add(new HR_PR_EquipmentLocation
                {
                    ZoneID = ZoneID,
                    LocationID = add.LocationID,
                    LocationName = add.LocationName,
                    UsageStatus = add.UsageStatus,
                    EditDate = DateTime.Now,
                    EditID = userId,
                    EditName = userName
                });
            }

            foreach (var remove in removed)
            {
                HR_PR_EquipmentLocation.Remove(remove);
            }

            foreach (var update in updated)
            {
                var newValue = newLocations.First(n => n.LocationID == update.LocationID);
                update.LocationName = newValue.LocationName;
                update.Capacity = newValue.Capacity;
                update.UsageStatus = newValue.UsageStatus;
                update.EditDate = DateTime.Now;
                update.EditID = userId;
                update.EditName = userName;
            }
        }

    }
}
