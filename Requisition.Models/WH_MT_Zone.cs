using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Requisition.Models
{
    public partial class WH_MT_Zone
    {
        [Key] 
        public string ZoneID { get; set; }

        [StringLength(100)]
        public string ZoneName { get; set; }
        public string ZoneIDAndName
        {
            get
            {
                if (ZoneID != null)
                {
                    return ZoneID + "-" + ZoneName;
                }
                return "";
            }
        }
        [StringLength(10)]
        public string EditID { get; set; }

        [StringLength(50)]
        public string EditName { get; set; }

        public DateTime? EditDate { get; set; }

        public bool? UsageStatus { get; set; }
        public void Create(string userId, string userName)
        {
            EditDate = DateTime.Now;
            EditName = userName;
            EditID = userId;
        }
        [ForeignKey("ZoneID")]
        public virtual ICollection<WH_MT_Location> WH_MT_Location { get; set; }
        public void Edit(string userId, string userName, WH_MT_Zone newItem)
        {
            EditDate = DateTime.Now;
            EditName = userName;
            EditID = userId;
            ZoneName = newItem.ZoneName;
            UsageStatus = newItem.UsageStatus;

            // ของเก่า (จาก DB)
            var oldLocations = WH_MT_Location.ToList();

            // ของใหม่ (จาก UI)
            var newLocations = newItem.WH_MT_Location.ToList();

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
                WH_MT_Location.Add(new WH_MT_Location
                {
                    ZoneID = ZoneID,
                    Capacity = add.Capacity,
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
                WH_MT_Location.Remove(remove);
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
