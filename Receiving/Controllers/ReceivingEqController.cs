using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GFPT.Extension.Utility;
using GFPT.Extension.Utility.Std;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Receiving.Data;
using Receiving.Models;
using Template_Tabler;

namespace Receiving.Controllers
{
    public class ReceivingEqController : Controller
    {
        private IWrapper _repo;
        private string host;
        private readonly IHttpContextAccessor _httpContext;
        private readonly V_SYS_USER user;
        private readonly IEnumerable<V_SYS_USER_ROLE_PRIVILEGE> userPrivilege;
        public ReceivingEqController(IWrapper repo, IHttpContextAccessor httpContext)
        {
            _repo = repo;
            _httpContext = httpContext;
            var currentUser = _httpContext.HttpContext.User.Identity.Name.NoDomainName();
            user = _repo.SysUser.FindByCondition(x => x.UserLogin == currentUser).SingleOrDefault();
            userPrivilege = _repo.UserPrivilege.FindByCondition(x => x.UserLogin == currentUser && x.ProgramID == UtilityHelper.ProgramId);
            host = $"{httpContext.HttpContext.Request.Scheme}://{httpContext.HttpContext.Request.Host.ToUriComponent()}{httpContext.HttpContext.Request.PathBase.ToString()}";
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var result = _repo.SysDocProgram.FindByCondition(l => l.ProgramID == UtilityHelper.ProgramId);
            ViewData["Permiss"] = result.SingleOrDefault(l => l.DocType == 0)?.DocLink + "&ProgramID=" + UtilityHelper.ProgramId;
            ViewData["Doc"] = result.SingleOrDefault(l => l.DocType == 1)?.DocLink + "&ProgramID=" + UtilityHelper.ProgramId;
            ViewData["Manual"] = result.SingleOrDefault(l => l.DocType == 2)?.DocLink + "&ProgramID=" + UtilityHelper.ProgramId;
            if (context.Controller is Controller controller)
            {
                controller.ViewBag.userLogin = "ไม่พบชื่อผู้ใช้งาน";

                if (user != null)
                {
                    if (!string.IsNullOrEmpty(user.UserLogin))
                        controller.ViewBag.userLogin = user.UserLogin;
                }
                controller.ViewBag.Host = host.GetHost();
                controller.ViewBag.userPrivilege = userPrivilege;
                //controller.ViewBag.Guide = linkDocument.SingleOrDefault(x => x.DocType == 2)?.FullLink;
                //controller.ViewBag.Permissions = linkDocument.SingleOrDefault(x => x.DocType == 0)?.FullLink;
                //controller.ViewBag.Presentation = linkDocument.SingleOrDefault(x => x.DocType == 1)?.FullLink;
            }
            ViewBag.URL = host;
            ViewBag.HostReport = $"{host}Report";
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FindePrItem(string critiria, string DepartmentID, string  StartFrom , string  StartTo)
        {

            var result = _repo.VWH
                        .FindByCondition(e =>
                          (  string.IsNullOrEmpty(critiria) ||
                            e.ShopName.StartsWith(critiria) ||
                            e.ShopID.StartsWith(critiria) ||
                            e.CodeName.StartsWith(critiria))
                            &&(string.IsNullOrEmpty(DepartmentID) || e.DepartmentID.Contains(DepartmentID))
                        )
                        .GroupBy(e => new
                        {
                            e.RequestDate,
                            e.RequestNo,
                            e.DepartmentID,
                            e.ShopID,
                            e.ShopName,
                            e.DepartmentName
                        })
                        .Select(g => new
                        {
                            RequestDate = g.Key.RequestDate,
                            RequestNo = g.Key.RequestNo,
                            DepartmentID = g.Key.DepartmentID,
                            ShopID = g.Key.ShopID,
                            ShopName = g.Key.ShopName,
                            DepartmentName = g.Key.DepartmentName,
                            // ถ้าต้องการรวมข้อมูลในกลุ่ม เช่น นับจำนวน
                            Count = g.Count()
                            // หรือรวมค่าอื่น ๆ เช่น TotalAmount = g.Sum(x => x.Amount)
                        })
                        .OrderBy(d => d.DepartmentID)
                        .ToList();

             var search = new Search();
            if (!string.IsNullOrEmpty(StartFrom)) { search.DateStartFrom = StartFrom.stringToDateTime(); }
            if (!string.IsNullOrEmpty(StartTo)) { search.DateStartTo = StartTo.stringToDateTime(); }

            if (search.DateStartFrom != default(DateTime) && search.DateStartTo != default(DateTime)) { result = result.Where(e => search.DateStartFrom.Date <= e.RequestDate.Date && e.RequestDate.Date <= search.DateStartTo.Date).ToList(); }


             

            return Json(result);

        }

          public IActionResult GetDetail(string requestNo)
        {
 
            var result = _repo.VWH.FindByCondition(e => 
                                                           (e.RequestNo.Equals(requestNo))  

                                                         ).ToList();
            result.ToList().ForEach(l => {
                    l.ClaimRateNumber = _repo.ClaimRate.FindSingle(g => g.DepartmentID == l.DepartmentID)?.ClaimRateNumber;
                    l.ClaimRateID = _repo.ClaimRate.FindSingle(g => g.DepartmentID == l.DepartmentID)?.ClaimRateID;

            });

            return Json(result);

        }

        public IActionResult ListAs400(Search Search)
        {
            ViewData["Title"] = "อัปเดทเข้าระบบ AS400";
            ViewBag.Search = Search;
            Search.page = ((Search.page == 0) ? 1 : Search.page);
            ViewBag.CurrentSort = Search.currentSort;
            if(!string.IsNullOrEmpty(Search.receiveDateStart)) { Search.DateStartFrom = Search.receiveDateStart.stringToDateTime(); }
            if (!string.IsNullOrEmpty(Search.receiveDateEnd)) { Search.DateStartTo = Search.receiveDateEnd.stringToDateTime(); }
            var ListItemss = _repo.ReceivingHeader.GetDataList(Search);
            var _TypeID =
                    new List<SelectListItem>
                    {
                            new SelectListItem {Text = "รับอะไหล่", Value = "001"},
                            new SelectListItem {Text = "รับบรรจุภัณฑ์", Value = "002"}, 
                    };
            ViewBag.TypeID = new SelectList(_TypeID, "Value", "Text", Search?.TypeID); 
            return View(ListItemss);
        }
        public IActionResult ListInventory(Search Search)
        {
            ViewData["Title"] = "จัดเก็บสินค้า";
            ViewBag.Search = Search;
            Search.page = ((Search.page == 0) ? 1 : Search.page);
            ViewBag.CurrentSort = Search.currentSort;
            if(!string.IsNullOrEmpty(Search.receiveDateStart)) { Search.DateStartFrom = Search.receiveDateStart.stringToDateTime(); }
            if (!string.IsNullOrEmpty(Search.receiveDateEnd)) { Search.DateStartTo = Search.receiveDateEnd.stringToDateTime(); }
            var ListItemss = _repo.Inventory.GetDataList(Search);
            var _TypeID =
                    new List<SelectListItem>
                    {
                            new SelectListItem {Text = "รับอะไหล่", Value = "001"},
                            new SelectListItem {Text = "รับบรรจุภัณฑ์", Value = "002"}, 
                    };
            ViewBag.TypeID = new SelectList(_TypeID, "Value", "Text", Search?.TypeID);

            return View(ListItemss);
        }
        public IActionResult List(Search Search)
        {
            ViewData["Title"] = "ระบบ ใบรับของ";
            ViewBag.Search = Search;
            Search.page = ((Search.page == 0) ? 1 : Search.page);
            ViewBag.CurrentSort = Search.currentSort;
            if(!string.IsNullOrEmpty(Search.receiveDateStart)) { Search.DateStartFrom = Search.receiveDateStart.stringToDateTime(); }
            if (!string.IsNullOrEmpty(Search.receiveDateEnd)) { Search.DateStartTo = Search.receiveDateEnd.stringToDateTime(); }
            var ListItemss = _repo.ReceivingHeader.GetDataList(Search);
            var _TypeID =
                    new List<SelectListItem>
                    {
                            new SelectListItem {Text = "รับอะไหล่", Value = "001"},
                            new SelectListItem {Text = "รับบรรจุภัณฑ์", Value = "002"}, 
                    };
            ViewBag.TypeID = new SelectList(_TypeID, "Value", "Text", Search?.TypeID);

            return View(ListItemss);
        }
        public IActionResult Detail(Search item)
        {
            ViewBag.Edit = true;
            ViewBag.approve1 = true;
            var result = _repo.ReceivingHeader.FindSingle(l => l.RunningID == item.ID, 
                l => l.Include(x => x.HR_PR_EquipmentReceivingDetail).ThenInclude(c => (c as HR_PR_EquipmentReceivingDetail).HR_PR_EquipmentClaimRate)
                      .Include(x => x.V_HR_MT_Department)) ?? null;
            ViewBag.Search = item; 
             ViewBag.Branch = new SelectList(_repo.Branch.FindByCondition(l=>l.UsageStatus == true), "BranchID", "BranchIDAndName", result?.BranchID);
             ViewBag.ClaimRate = new SelectList(_repo.ClaimRate.FindByCondition(l=>l.UsageStatus == true), "ClaimRateID", "ClaimRateNumber");
            var MTDepartment = _repo.MT_Department.FindByCondition(l => l.UsageStatus == true).ToList();

            ViewBag.DepartmentID = new SelectList(MTDepartment, "DepartmentID", "DepartmentIDAndName" );
            return View(result);
        }
        [HttpPost]
        public IActionResult Approve(string RunningID, string Approve1Status, string Approve1Remark, string sys)
        {
            try
            {
                if(sys != "P02")
                {
                    return Json(new { success = false, error = "ไม่สามารถทำรายการได้ เนื่องจากสถานะ ของ ใบนี้ เปลี่ยนไป แล้ว" });
                }
                var header = _repo.ReceivingHeader.FindSingle(x => x.RunningID == RunningID,l=> l.Include(f=>f.HR_PR_EquipmentReceivingDetail));
                if (header == null)
                    return Json(new { success = false, error = "ไม่พบข้อมูล" });
                    header.Approve1Status = Approve1Status;
                    header.Approve1Remark = Approve1Remark;
                    header.Approve1Name = user.UserName;
                    header.Approve1Date = DateTime.Now;
                    if (Approve1Status == "Y")
                    {
                            header.SYS_Status = "F01";
                            var id = _repo.Inventory.GetRunning();
                            var invetoryList = new List<HR_PR_EquipmentInventory>();
                            foreach (var d in header.HR_PR_EquipmentReceivingDetail)
                            {
                                string credential = $"{DateTime.Now.Year.ToString("0000")}{DateTime.Now.Month.ToString("00")}{DateTime.Now.Day.ToString("00")}{id.ToString("00")}";

                                    invetoryList.Add(new HR_PR_EquipmentInventory {
                                        SerialNo = credential,
                                        ReceivingType = header.ReceivingType,
                                        CodeID = d.CodeID,
                                        CodeName = d.CodeName,
                                        LotNo = d.LotNo,
                                        QtyReceived = d.QtyReceived,
                                        ExpireDate = d.ExpireDate,
                                        LocationID = "",
                                        RunningID = d.RunningID,
                                        Unit = d.Unit,
                                        SYS_Status = "P01",
                                        EditID = user.UserID,
                                        EditName = user.UserName,
                                        EditDate = DateTime.Now,

                                        CreateID = user.UserID,
                                        CreateName = user.UserName,
                                        CreateDate = DateTime.Now, 
                               
                                    });
                                         id++;
                            }
                           _repo.Inventory.Create(invetoryList);
                      }
                    else
                    {
                        header.SYS_Status = "P01";
                        string refidlog = $"{header.RunningID.ToString()}";
                        var Log = new SYS_RejectLogs(UtilityHelper.ProgramId, refidlog, header.statusPageName, Approve1Remark, user.UserName); 
                        _repo.RejectLogs.Create(Log);
                }
               

                _repo.ReceivingHeader.Update(header);
                _repo.Save();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }


        [HttpPost]
        public IActionResult SendInventory([FromBody] List<string> Ids)
        {
            if ( Ids == null || !Ids.Any())
            {
                return Json(new { success = false, message = "ไม่มีข้อมูลที่เลือก" });
            }

            try
            {
                foreach (var id in Ids)
                {
                    var item = _repo.Inventory.FindSingle(l => l.SerialNo == id) ?? null;
                    if(item != null)
                    {
                         item.SYS_Status = "F01";
                         item.EditID = user.UserID;
                         item.EditName = user.UserName;
                         item.EditDate = DateTime.Now;
                        _repo.Inventory.Update(item);

                        var oldStockCard = new HR_PR_EquipmentStockCard()
                        {
                            RunningID = item.RunningID,
                            CodeID = item.CodeID,
                            CodeName = item.CodeName,
                            QtyPay = 0,
                            QtyReceived = item.QtyReceived,
                            EditID = user.UserID,
                            EditName = user.UserName,
                            EditDate = DateTime.Now,
                        };

                        _repo.StockCard.Create(oldStockCard);

                    }
                }
                _repo.Save();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult SearchLocation(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return Json(new List<object>()); // ส่ง array ว่าง

            var locations = _repo.Location.FindByCondition(f=>f.LocationID.Contains(keyword) || f.LocationName.Contains(keyword) || string.IsNullOrEmpty(keyword)  ) 
                .Select(l => new
                {
                    LocationID = l.LocationID,
                    Name = l.LocationName ?? ""
                })
                .Take(10) // จำกัดผลลัพธ์
                .ToList();

            return Json(locations);
        }

        [HttpPost]
        public IActionResult SaveLocation( string locationId, string serialNo)
        { 

            if (string.IsNullOrEmpty(locationId) && string.IsNullOrEmpty(serialNo))
            {
                return Json(new { success = false, message = "ข้อมูลไม่ถูกต้อง" });
            }

            try
            {
                var old = _repo.Inventory.FindSingle(l => l.SerialNo == serialNo) ?? null;
                if(old != null)
                {
                    old.SYS_Status = "P02";
                    old.LocationID = locationId;
                    old.EditDate = DateTime.Now;
                    old.EditID = user.UserID;
                    old.EditName = user.UserName;
                }
              

                _repo.Inventory.Update(old);
                _repo.Save();
                return Json(new { success = true, message = "บันทึกเรียบร้อย" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult SaveReceive(HR_PR_EquipmentReceivingHeader item, bool send)
        {
            if (item == null) return BadRequest();
              var DataSendMail = new HR_PR_EquipmentReceivingHeader();
            try
            {
                if (string.IsNullOrEmpty(item.RunningID)) // CREATE NEW
                {
                    int index = 0;
                    item.Create(user.UserID, user.UserName, _repo.ReceivingHeader.GetRunning(), send);

                    foreach (var d in item.HR_PR_EquipmentReceivingDetail)
                    {
                        d.create(item.RunningID, item.RunningID + index.ToString("00"));
                        index++;
                    }

                    _repo.ReceivingHeader.Create(item);
                    DataSendMail = item;
                }
                else // UPDATE EXISTING
                {
                    var old = _repo.ReceivingHeader.FindSingle(
                        l => l.RunningID == item.RunningID,
                        l => l.Include(f => f.HR_PR_EquipmentReceivingDetail)
                    );

                    if (old == null)
                        return Json(new { success = false, error = "ไม่พบข้อมูลที่ต้องการแก้ไข" });

                    // ---- Step D: Clear Detail เก่า ----
                    _repo.ReceivingDetail.DeleteByCondition(l => l.RunningID == item.RunningID);

                    // ---- Step H: Map Header ----
                    old.Edit(user.UserID, user.UserName, item, send);

                    // ---- Step H: Map Detail ใหม่ ----
                    int index = 0;
                    foreach (var d in item.HR_PR_EquipmentReceivingDetail)
                    {
                        d.create(old.RunningID, old.RunningID + index.ToString("00"));
                        old.HR_PR_EquipmentReceivingDetail.Add(d);
                        index++;
                    }

                    // ---- Step W: Update ----
                    _repo.ReceivingHeader.Update(old);
                    DataSendMail = old;
                }

                // Save to Database
                _repo.Save();
                if (send)
                { 
                    if (DataSendMail.SYS_Status == "P02")
                    {
                        var rgApprove = Helper.RightCode.Approve1;
                        var controllername = "Detail";
                        var link = Url.Action(controllername, "ReceivingEq", new { RunningID = item.RunningID }, Request.Scheme);
                        string detail = "รายการ ตรวจสอบใบรับของ  " + DataSendMail.statusPageName + item.RunningID;
                        if (!string.IsNullOrEmpty(DataSendMail.DepartmentID))
                        {
                            String DepartmentSendMail = ((DataSendMail.DepartmentID.Length < 4 ? DataSendMail.DepartmentID : DataSendMail.DepartmentID.Substring(0, 4)));

                            Helper.SendEmail(Helper.Company.GFPT, UtilityHelper.ProgramId, "มีรายการรอ ตรวจสอบใบรับของ", detail, DataSendMail.SYS_Status, rgApprove, DepartmentSendMail, user.UserName, link);

                        }
                    } 
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, ex = ex.Message });
            }

            return Json(new { success = true, id = item.RunningID });
        }

        public IActionResult Cancel(string id, string remark)
        {
            try
            {
                var item = _repo.ReceivingHeader.FindByCondition(l => l.RunningID == id).SingleOrDefault() ?? null;
                if (item == null) { return Json(new { success = false, ex = "ไม่พบข้อมูล" }); }
                item.Cancel(user.UserID, user.UserName);
                _repo.ReceivingHeader.Update(item);
                _repo.Save();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, ex = ex.Message });
            }
        }


        #region Master 
        public IActionResult ListMasterZone(Search Search)
        {
            ViewData["Title"] = " Zone & Location";
            ViewBag.Search = Search;
            Search.page = ((Search.page == 0) ? 1 : Search.page);
            ViewBag.CurrentSort = Search.currentSort; 
            var ListItemss = _repo.EquipmentZone.GetDataList(Search);
            
            var _typestatus =
                   new List<SelectListItem>
                   {
                            new SelectListItem {Text = "ใช้งาน", Value = "1"},
                            new SelectListItem {Text = "ไม่ใช้งาน", Value = "0"},
                   };
            ViewBag._typedaystatus = new SelectList(_typestatus, "Value", "Text", Search.TypeID);
            return View(ListItemss);
        }
        public IActionResult CreateZone(Search Search)
        {
            var _typestatus =
                      new List<SelectListItem>
                      {
                            new SelectListItem {Text = "ใช้งาน", Value = "1"},
                            new SelectListItem {Text = "ไม่ใช้งาน", Value = "0"},
                      };
            ViewData["Title"] = "Zone";
            ViewBag.Search = Search; 
            var ListItemss = _repo.EquipmentZone.FindSingle(l => l.ZoneID == Search.ID , g=>g.Include(f=>f.HR_PR_EquipmentLocation)) ?? null ;
            ViewBag._typedaystatus = new SelectList(_typestatus, "Value", "Text", ListItemss?.UsageStatus == true ? "1" : "0");

            return View(ListItemss);
        }
        public IActionResult SaveZone(HR_PR_EquipmentZone item)
        {
            if (item == null) return BadRequest();
            var DataSendMail = new HR_PR_EquipmentReceivingHeader();
            try
            {
                var old = _repo.EquipmentZone.FindSingle(
                        l => l.ZoneID == item.ZoneID,
                        l => l.Include(f => f.HR_PR_EquipmentLocation)
                    );
                if (old == null) // CREATE NEW
                { 
                    item.Create(user.UserID, user.UserName);  
                    _repo.EquipmentZone.Create(item); 
                }
                else // UPDATE EXISTING
                { 

                    if (old == null)
                        return Json(new { success = false, error = "ไม่พบข้อมูลที่ต้องการแก้ไข" }); 
                    // ---- Step H: Map Header ----
                    old.Edit(user.UserID, user.UserName, item);
 
                    // ---- Step W: Update ----
                    _repo.EquipmentZone.Update(old); 
                } 
                _repo.Save();
                 
            }
            catch (Exception ex)
            {
                return Json(new { success = false, ex = ex.Message });
            }

            return Json(new { success = true, id = item.ZoneID });
        }

        public IActionResult ListMasterBranch(Search Search)
        {
            ViewData["Title"] = "ข้อมูล Branch";
            ViewBag.Search = Search;
            Search.page = ((Search.page == 0) ? 1 : Search.page);
            ViewBag.CurrentSort = Search.currentSort;
            var ListItemss = _repo.Branch.GetDataList(Search);

            var _typestatus =
                   new List<SelectListItem>
                   {
                            new SelectListItem {Text = "ใช้งาน", Value = "1"},
                            new SelectListItem {Text = "ไม่ใช้งาน", Value = "0"},
                   };
            ViewBag._typedaystatus = new SelectList(_typestatus, "Value", "Text", Search.TypeID);
            return View(ListItemss);
        }

        [HttpPost]  
        public IActionResult SaveBranch(HR_PR_EquipmentBranch item, string status)
        {
            try
            {
                //if (string.IsNullOrEmpty(item?.BranchID)) { throw new Exception("ไม่พบข้อมูลที่จะแก้ไข"); }

                if (status == "A")
                {
                    var RunningID = _repo.Branch.GetRunning();
                    var RunningIDStr = "B" + RunningID.ToString("0000");
                    item.BranchID = RunningIDStr; 
                    item.Add(user.UserID, user.UserName); 
                    _repo.Branch.Create(item);

                }
                else if (status == "E")
                {
                    var old = _repo.Branch.FindSingle(l => l.BranchID == item.BranchID ) ?? null;
                    if (old == null) { throw new Exception("ไม่พบข้อมูลที่จะแก้ไข"); } 

                    old.Edit(user.UserID, user.UserName, item); 
                    _repo.Branch.Update(old);
                }
                else
                {
                    throw new Exception("ไม่สามารถบันทึกได้");
                }
                _repo.Save();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, ex = ex.Message });
            }
        }
        [HttpPost]  
        public IActionResult SaveClaimRate(HR_PR_EquipmentClaimRate item, string status)
        {
            try
            {
 
                if (status == "A")
                {
                    var RunningID = _repo.ClaimRate.GetRunning();
                    var RunningIDStr = "C" + RunningID.ToString("0000");
                    item.ClaimRateID = RunningIDStr; 
                    item.Add(user.UserID, user.UserName); 
                    _repo.ClaimRate.Create(item);

                }
                else if (status == "E")
                {
                    var old = _repo.ClaimRate.FindSingle(l => l.ClaimRateID == item.ClaimRateID ) ?? null;
                    if (old == null) { throw new Exception("ไม่พบข้อมูลที่จะแก้ไข"); } 

                    old.Edit(user.UserID, user.UserName, item); 
                    _repo.ClaimRate.Update(old);
                }
                else
                {
                    throw new Exception("ไม่สามารถบันทึกได้");
                }
                _repo.Save();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, ex = ex.Message });
            }
        }
        public IEnumerable<HR_MT_Department> GetDepartmentByPermissionGetList(List<string> status)
        {
            var GroupId = userPrivilege.Where(l => status.Contains(l.SYS_Status)).Select(l => l.GroupDepartment).Distinct().ToList();
            var DepartmentID = _repo.GroupDepartmentDetail.FindByCondition(e => GroupId.Contains(e.GroupDepID)).Select(l => l.DepartmentID).ToList();
            var MTDepartment = _repo.MT_Department.FindByCondition(l => l.UsageStatus == true && DepartmentID.Any(s => l.DepartmentID.StartsWith(s)));
            return MTDepartment;
        }


        public IActionResult ListMasterClaimRate(Search Search)
        {
            ViewData["Title"] = "ข้อมูล ClaimRate";
            ViewBag.Search = Search;
            Search.page = ((Search.page == 0) ? 1 : Search.page);
            ViewBag.CurrentSort = Search.currentSort;
            var ListItemss = _repo.ClaimRate.GetDataList(Search);
 
 
            var MTDepartment = _repo.MT_Department.FindByCondition(l => l.UsageStatus == true ).ToList();

            ViewBag.DepartmentID = new SelectList(MTDepartment, "DepartmentID", "DepartmentIDAndName", Search?.DepartmentID);

            var _typestatus =
                   new List<SelectListItem>
                   {
                            new SelectListItem {Text = "ใช้งาน", Value = "1"},
                            new SelectListItem {Text = "ไม่ใช้งาน", Value = "0"},
                   };
            ViewBag._typedaystatus = new SelectList(_typestatus, "Value", "Text", Search.TypeID);
            return View(ListItemss);
        }

        #endregion

        public IActionResult ReportCard(Search Search)
        {
            ViewData["Title"] = "รายงานสินค้าคงคลัง";
            ViewBag.Search = Search;
            Search.page = ((Search.page == 0) ? 1 : Search.page);
             ViewBag.CurrentSort = Search.currentSort;
            //var ListItemss = _repo.ClaimRate.GetDataList(Search);


            //var MTDepartment = _repo.MT_Department.FindByCondition(l => l.UsageStatus == true).ToList();

            //ViewBag.DepartmentID = new SelectList(MTDepartment, "DepartmentID", "DepartmentIDAndName", Search?.DepartmentID);

            //var _typestatus =
            //       new List<SelectListItem>
            //       {
            //                new SelectListItem {Text = "ใช้งาน", Value = "1"},
            //                new SelectListItem {Text = "ไม่ใช้งาน", Value = "0"},
            //       };
            //ViewBag._typedaystatus = new SelectList(_typestatus, "Value", "Text", Search.TypeID);
            return View();
        }
        public IActionResult ReportCardList(Search Search)
        {
            ViewData["Title"] = "รายงานสินค้าคงคลัง";
            ViewBag.Search = Search;
            Search.page = ((Search.page == 0) ? 1 : Search.page);
            ViewBag.CurrentSort = Search.currentSort;
            var ListItemss = _repo.VInventory.GetDataList(Search); 

            var _TypeID =
                  new List<SelectListItem>
                  {
                            new SelectListItem {Text = "รับอะไหล่", Value = "001"},
                            new SelectListItem {Text = "รับบรรจุภัณฑ์", Value = "002"},
                  };
            ViewBag.TypeID = new SelectList(_TypeID, "Value", "Text", Search?.TypeID);

            return View(ListItemss);
        }
        // ================= Inventory =================
        [HttpGet]
        public IActionResult GetInventory(string codeID)
        {
            try
            {
                // ตัวอย่าง Mock Data (คุณสามารถ query จาก DB จริงแทนได้)

                var items = _repo.Inventory.FindByCondition(l => l.CodeID == codeID).ToList().Select(l=> new  {

                    CodeIDName = l.CodeName,
                    SerialNo = l.SerialNo,
                    LocationID = l.LocationID,
                    QtyReceived = l.QtyReceived,
                    LotNo =l.LotNo,
                    ExpireDate = l.ExpireDateTH

                });
                 
                var result = new
                {
                    success = true,
                    data = new
                    {
                        CodeName = items.FirstOrDefault()?.CodeIDName,
                        TotalQty = items.Sum(x => x.QtyReceived),
                        Items = items
                    }
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        // ================= Stock Card =================
        [HttpGet]
        public IActionResult GetStockCard(string codeID)
        {
            try
            {
                var rawItems = _repo.StockCard
                    .FindByCondition(l => l.CodeID == codeID)
                    .OrderBy(l => l.EditDate)   // เรียงตามวันที่ (สำคัญมาก)
                    .ToList();

                int balance = 0;
                var items = rawItems.Select(l =>
                {
                    balance += (l.QtyReceived ?? 0) - (l.QtyPay ?? 0);

                    return new
                    {
                        CodeIDName = l.CodeName,
                        TransDate = l.EditDateTH,
                        RefNo = l.RunningID,
                        QtyIn = l.QtyReceived ?? 0,
                        QtyOut = l.QtyPay ?? 0,
                        Balance = balance
                    };
                }).ToList();

                var result = new
                {
                    success = true,
                    data = new
                    {
                        CodeName = items.FirstOrDefault()?.CodeIDName ?? "",
                        TotalQty = items.LastOrDefault()?.Balance ?? 0,
                        Items = items
                    }
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



    }
}