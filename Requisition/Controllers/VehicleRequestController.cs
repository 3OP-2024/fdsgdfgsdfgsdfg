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
                var departments = new[] { "ฝ่ายผลิต", "ฝ่ายบุคคล", "ฝ่ายบัญชี", "วิศวกรรม", "โลจิสติกส์" };

                for (int i = 1; i <= 10; i++) {
                    var status = i <= 3 ? "P01" : (i <= 6 ? "P02" : (i <= 8 ? "P03" : "P04"));

                    var approveStatus1 = status switch {
                        "P03" => 1,
                        "P04" => 1,
                        _ => 0
                    };

                    var approveStatus2 = status switch {
                        "P04" => 1,
                        _ => 0
                    };

                    _mockData.Add(new VehicleRequestViewModel {
                        Id = i,
                        RequestNo = $"VR-{i:0000}",
                        RequestStatus = status,
                        StartDate = DateTime.Now.AddDays(i),
                        EndDate = DateTime.Now.AddDays(i).AddHours(4),
                        Location = locations[i % locations.Length],
                        Province = provinces[i % provinces.Length],
                        Objective = $"เดินทางไปตรวจงานโปรเจกต์ที่ {i}",
                        LuggageDetails = i % 2 == 0 ? "เครื่องมือช่าง, กล่องเอกสาร" : "ไม่มี",
                        PassengerNames = $"สมชาย ใจดี, พนักงานคนที่ {i}",
                        PickupAtCompany = true,
                        PickupAtOther = i % 3 == 0, // บางคนรับจุดอื่น
                        PickupOtherDetail = i % 3 == 0 ? "หน้าปั๊ม ปตท." : "",
                        VehicleType = i % 2 == 0 ? "van" : "sedan",
                        ApproveStatus1 = approveStatus1,
                        ApproveStatus2 = approveStatus2,
                        ApproveStatus3 = 0,
                        Approve1By = status == "P03" || status == "P04" ? "หัวหน้าแผนก" : string.Empty,
                        Approve1Date = status == "P03" || status == "P04" ? DateTime.Now.AddDays(-2) : null,
                        Approve1Remark = status == "P03" || status == "P04" ? "อนุมัติให้ดำเนินการ" : string.Empty,
                        Approve2By = status == "P04" ? "HR" : string.Empty,
                        Approve2Date = status == "P04" ? DateTime.Now.AddDays(-1) : null,
                        Approve2Remark = status == "P04" ? "ตรวจสอบข้อมูลครบ" : string.Empty,
                        Approve3By = string.Empty,
                        Approve3Date = null,
                        Approve3Remark = string.Empty,
                        EditName = "สมชาย ใจดี",
                        DepartmentName = departments[i % departments.Length],
                        DepartmentID = $"D-{i % departments.Length + 1}"
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
                model.RequestNo = $"VR-{model.Id:0000}";
                model.ApproveStatus1 = 0; // เริ่มต้นรออนุมัติ
                model.RequestStatus = "P01";
                model.EditName = string.IsNullOrEmpty(model.EditName) ? "ผู้ขอ" : model.EditName;
                model.DepartmentName = ResolveDepartmentName(model.DepartmentID);
                _mockData.Add(model);
            } else // กรณี Update
              {
                var existing = _mockData.FirstOrDefault(x => x.Id == model.Id);
                if (existing != null) {
                    if (existing.RequestStatus != "P01") return BadRequest("ไม่สามารถแก้ไขได้ในสถานะปัจจุบัน");

                    existing.StartDate = model.StartDate;
                    existing.EndDate = model.EndDate;
                    existing.Location = model.Location;
                    existing.Province = model.Province;
                    existing.Objective = model.Objective;
                    existing.LuggageDetails = model.LuggageDetails;
                    existing.PassengerNames = model.PassengerNames;
                    existing.PickupAtCompany = model.PickupAtCompany;
                    existing.PickupAtOther = model.PickupAtOther;
                    existing.PickupOtherDetail = model.PickupOtherDetail;
                    existing.VehicleType = model.VehicleType;
                    existing.DepartmentID = model.DepartmentID;
                    existing.DepartmentName = ResolveDepartmentName(model.DepartmentID);
                }
            }
            return Ok(new { success = true, message = "บันทึกข้อมูลเรียบร้อย" });
        }

        // POST: อนุมัติ (Approve)
        [HttpPost]
        public IActionResult Approve(int id, int level, int status, string remark) {
            var item = _mockData.FirstOrDefault(x => x.Id == id);
            if (item == null) return NotFound();

            var now = DateTime.Now;
            switch (level) {
                case 1:
                    item.ApproveStatus1 = status;
                    item.Approve1Remark = remark;
                    item.Approve1Date = now;
                    item.Approve1By = "หัวหน้าแผนก";
                    item.RequestStatus = status == 1 ? "P03" : "P01";
                    break;
                case 2:
                    item.ApproveStatus2 = status;
                    item.Approve2Remark = remark;
                    item.Approve2Date = now;
                    item.Approve2By = "HR";
                    item.RequestStatus = status == 1 ? "P04" : "P01";
                    break;
                case 3:
                    item.ApproveStatus3 = status;
                    item.Approve3Remark = remark;
                    item.Approve3Date = now;
                    item.Approve3By = "ผู้บริหาร";
                    item.RequestStatus = status == 1 ? "APPROVED" : "P01";
                    break;
                default:
                    return BadRequest("Level ไม่ถูกต้อง");
            }

            return Ok(new { success = true, status = item.RequestStatus });
        }

        private string ResolveDepartmentName(string departmentId) {
            return departmentId switch {
                "PRD" => "ฝ่ายผลิต",
                "ENG" => "ฝ่ายวิศวกรรม",
                "HR" => "ฝ่ายบุคคล",
                "ACC" => "ฝ่ายบัญชี",
                "LOG" => "ฝ่ายโลจิสติกส์",
                _ => departmentId
            };
        }
    }

}