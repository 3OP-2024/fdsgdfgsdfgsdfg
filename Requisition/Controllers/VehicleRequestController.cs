using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Requisition.Models;

namespace Requisition.Controllers {

    public class VehicleRequestController : Controller {
        // จำลอง Database (Static List)
        private static List<VehicleRequestViewModel> _mockData = new List<VehicleRequestViewModel>();

        public VehicleRequestController() {
            // สร้าง Mock Data 10 แถว ถ้ายังไม่มีข้อมูล
            if (_mockData.Count == 0) {
                var locations = new[] { "โรงงานบางนา", "นิคมอมตะ", "ลูกค้าสมุทรสาคร", "สำนักงานใหญ่", "โรงงานแปรรูปไก่" };
                var provinces = new[] { "สมุทรปราการ", "ชลบุรี", "สมุทรสาคร", "กรุงเทพฯ", "สระบุรี" };

                for (int i = 1; i <= 10; i++) {
                    _mockData.Add(new VehicleRequestViewModel {
                        Id = i,
                        StartDate = DateTime.Now.AddDays(i),
                        EndDate = DateTime.Now.AddDays(i).AddHours(4),
                        Location = locations[i % 5],
                        Province = provinces[i % 5],
                        Objective = $"เดินทางไปตรวจงานโปรเจกต์ที่ {i}",
                        LuggageDetails = i % 2 == 0 ? "เครื่องมือช่าง, กล่องเอกสาร" : "ไม่มี",
                        PassengerNames = $"สมชาย ใจดี, พนักงานคนที่ {i}",
                        PickupAtCompany = true,
                        PickupAtOther = i % 3 == 0, // บางคนรับจุดอื่น
                        PickupOtherDetail = i % 3 == 0 ? "หน้าปั๊ม ปตท." : "",
                        VehicleType = i % 2 == 0 ? "van" : "sedan",
                        // Mock Status: 0=รอ, 1=อนุมัติ, 2=ไม่อนุมัติ
                        ApproveStatus1 = 1, // หัวหน้าอนุมัติแล้วทุกคน (สมมติ)
                        ApproveStatus2 = i % 2,
                        ApproveStatus3 = 0,
                        EditName = "สมชาย ใจดี",
                        DepartmentName = "บุคคล"
                    });
                }
            }
        }

        public IActionResult Index() {
            return View(_mockData); // ส่งข้อมูลไปแสดงในตาราง
        }

        // --- API Functions ---

        // GET: ดึงข้อมูล 1 รายการเพื่อแสดงใน Modal
        [HttpGet]
        public IActionResult GetDetail(int id) {
            var item = _mockData.FirstOrDefault(x => x.Id == id);
            if (item == null) return NotFound();
            return Json(item);
        }

        // POST: เพิ่ม (Add) หรือ แก้ไข (Update)
        [HttpPost]
        public IActionResult Save(VehicleRequestViewModel model) {
            if (model.Id == 0) // กรณี Add ใหม่
            {
                model.Id = _mockData.Max(x => x.Id) + 1;
                model.ApproveStatus1 = 0; // เริ่มต้นรออนุมัติ
                _mockData.Add(model);
            } else // กรณี Update
              {
                var existing = _mockData.FirstOrDefault(x => x.Id == model.Id);
                if (existing != null) {
                    existing.StartDate = model.StartDate;
                    existing.EndDate = model.EndDate;
                    existing.Location = model.Location;
                    existing.Objective = model.Objective;
                    existing.VehicleType = model.VehicleType;
                    // ... อัปเดตฟิลด์อื่นๆ ตามต้องการ ...
                }
            }
            return Ok(new { success = true, message = "บันทึกข้อมูลเรียบร้อย" });
        }

        // POST: อนุมัติ (Approve)
        [HttpPost]
        public IActionResult Approve(int id, int level, int status) {
            var item = _mockData.FirstOrDefault(x => x.Id == id);
            if (item != null) {
                if (level == 1) item.ApproveStatus1 = status;
                else if (level == 2) item.ApproveStatus2 = status;
                else if (level == 3) item.ApproveStatus3 = status;
            }
            return Ok(new { success = true });
        }
    }

}