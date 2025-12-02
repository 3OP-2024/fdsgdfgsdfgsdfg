using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Requisition.Models {
    public class VehicleRequestViewModel {
        public int Id { get; set; }
        public string RequestNo { get; set; }
        public string RequestStatus { get; set; } // P01, P02, P03, P04

        // 3.1 วันที่ใช้งาน
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // 3.2 - 3.4 ข้อมูลทั่วไป
        public string Location { get; set; } // สถานที่
        public string Province { get; set; } // จังหวัด
        public string Objective { get; set; } // วัตถุประสงค์

        // 3.5 สิ่งของสัมภาระ
        public string LuggageDetails { get; set; }
        public IFormFile LuggageImage { get; set; } // รูปภาพสัมภาระ

        // 3.6 ผู้โดยสาร (ตัวอย่างรับเป็น string list หรือ comma separated)
        public string PassengerNames { get; set; }
        public IFormFile PassengerListImage { get; set; } // ภาพรายชื่อหรือภาพผู้โดยสาร

        // 3.7 จุดขึ้นรถ (เลือกได้มากกว่า 1)
        public bool PickupAtCompany { get; set; }
        public bool PickupAtOther { get; set; }
        public string PickupOtherDetail { get; set; }
        public IFormFile PickupMapImage { get; set; } // ภาพแผนที่

        // 3.8 ประเภทรถ (SelectGroup)
        public string VehicleType { get; set; } // Sedan, Van, Truck

        // 3.9 การอนุมัติ (Status: 0=Pending, 1=Approved, 2=Rejected)
        public int ApproveStatus1 { get; set; }
        public int ApproveStatus2 { get; set; }
        public int ApproveStatus3 { get; set; }
        public string Approve1By { get; set; }
        public DateTime? Approve1Date { get; set; }
        public string Approve1Remark { get; set; }
        public string Approve2By { get; set; }
        public DateTime? Approve2Date { get; set; }
        public string Approve2Remark { get; set; }
        public string Approve3By { get; set; }
        public DateTime? Approve3Date { get; set; }
        public string Approve3Remark { get; set; }

        public string EditName { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentID { get; set; }

    }
}
