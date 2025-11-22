using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GFPT.Extension.Utility.Std;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Requisition.Data;
using Requisition.Models;

namespace Requisition.Controllers
{
    public class RequisitionController : Controller
    {
        private IWrapper _repo  ;
        private string host;
        private readonly IHttpContextAccessor _httpContext;
        private readonly V_SYS_USER user;
        private readonly IEnumerable<V_SYS_USER_ROLE_PRIVILEGE> userPrivilege;
        public RequisitionController(IWrapper repo, IHttpContextAccessor httpContext)
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
        
        public IEnumerable<HR_MT_Department> GetDepartmentByPermissionGetList(List<string> status)
        {
            var GroupId = userPrivilege.Where(l => status.Contains(l.SYS_Status)).Select(l => l.GroupDepartment).Distinct().ToList();
            var DepartmentID = _repo.GroupDepartmentDetail.FindByCondition(e => GroupId.Contains(e.GroupDepID)).Select(l => l.DepartmentID).ToList();
            var MTDepartment = _repo.MT_Department.FindByCondition(l => l.UsageStatus == true && DepartmentID.Any(s => l.DepartmentID.StartsWith(s)));
            return MTDepartment;
        }
        public bool CheckPer(string SYS_Status = null, string RightCode = null)
        {

            return userPrivilege.Any(l => l.SYS_Status == SYS_Status && l.RightCode == RightCode);
        }
        [HttpGet]
        public IActionResult SearchStock(string keyword, string RequisitionType)
        {
            string typeId = UtilityHelper.GetTypeRecei(RequisitionType);
            
            var data = _repo.VItemName
                .FindByCondition(x => (string.IsNullOrEmpty(keyword)
                         || x.ItemID.Contains(keyword)
                         || x.ItemNameTH.Contains(keyword)) 
                        && ( x.WarehouseTypeID.Contains(typeId))   
                        && ( x.Quantity > 0)   

                         )
                .Select(x => new
                {
                     CodeID = x.ItemID,
                     CodeName = x.ItemNameTH,
                    x.LocationID,
                     TotalQuantity = x.Quantity,
                    x.UnitNameTH
                })
                .Take(50) // จำกัดแค่ 50 รายการ
                .ToList();

            data = data.Where(d =>
            {
                var Quantitytall = _repo.RequisitionHeader
                                         .FindByCondition(
                                             l => l.WH_PR_RequisitionDetail.Any(f => f.ItemID == d.CodeID)
                                                  && l.SYS_Status != "C01",
                                             l => l.Include(v => v.WH_PR_RequisitionDetail))
                                         .SelectMany(g => g.WH_PR_RequisitionDetail
                                             .Where(f => f.ItemID == d.CodeID))
                                         .Sum(f => f.QtyRequisition) ?? 0;

                return d.TotalQuantity > Quantitytall; 
            }).ToList();

            return Json(data);
        }
        public IActionResult Detail(Search item)
        {
            ViewBag.Edit = CheckPer("P01", "RC01");
            ViewBag.approve1 = CheckPer("P02", "RCA1"); 
            var DepPartment = new List<HR_MT_Department>();

            var result = _repo.RequisitionHeader.FindSingle(l => l.RunningID == item.ID,
                l => l.Include(x => x.WH_PR_RequisitionDetail)
                         .ThenInclude(c => c.WH_MT_ItemName)
                        .Include(x => x.WH_PR_RequisitionDetail)
                            .ThenInclude(c => c.V_WH_JobDepartment)
                      .Include(x => x.V_HR_MT_Department)) ?? null; 

            ViewBag.Search = item; 
            if(result?.SYS_Status == "P01" || string.IsNullOrEmpty(result?.SYS_Status))
            {
                DepPartment = GetDepartmentByPermissionGetList(new List<string> { "P01" }).Where(l => l.DepartmentID.Length < 6).ToList();
            }
            else
            {
                DepPartment = _repo.MT_Department.FindByCondition(l => l.UsageStatus == true).ToList(); 
            }
            ViewBag.DepartmentID = new SelectList(DepPartment.Where(l=>  l.DepartmentID.Length < 6).ToList(), "DepartmentID", "DepartmentIDAndName",result?.DepartmentID);
            return View(result);
        }
        [HttpGet]
        public IActionResult SearchRequisitionFromPO(string DepartmentID, string RequisitionNo, string ProductCode, string DateStart, string DateEnd , string ReceivingType)
        {
            string typeId = UtilityHelper.GetTypeRecei(ReceivingType);

          
            //switch (ReceivingType)
            //{
            //    case "001":
            //        typeId = "RM";
            //        break;
            //    case "002":
            //        typeId = "WH";
            //        break;

            //    default:
            //        break;
            //}

            var item = _repo.VRequisitionReceiving.FindByCondition(l =>
                                                                (l.DepartmentID.StartsWith(DepartmentID) || string.IsNullOrEmpty(DepartmentID)) && 
                                                                (l.RequisitionNo.Contains(RequisitionNo) || string.IsNullOrEmpty(RequisitionNo))  
                                                                &&(l.WarehouseTypeID.Contains(typeId)) && 
                                                                (l.ProductCode.Contains(ProductCode) || string.IsNullOrEmpty(ProductCode))  

            ).Select(x => new
            {
                DepartmentID = x.DepartmentID,
                DepartmentName = x.DepartmentNameTH,
                RequisitionNo = x.RequisitionNo,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
                Quantity =  x.QuantityPR ,
                ReceivedQty =  x.ReceivedQty,
                UnitName = x.UnitName,
                Remark = x.Remark,
                ReceivingDate = x.ReceivingDate
            }).ToList();
            var search = new Search();
            if (!string.IsNullOrEmpty(DateStart)) { search.DateStartFrom = DateStart.stringToDateTime(); }
            if (!string.IsNullOrEmpty(DateEnd)) { search.DateStartTo = DateEnd.stringToDateTime(); }

            if (item != null)
            {
             var   DepPartment = GetDepartmentByPermissionGetList(new List<string> { "P01" }).Select(d=>d.DepartmentID).ToList();
                item = item.Where(p => DepPartment.Any(s =>
                   (p.DepartmentID.Length <= 4 && p.DepartmentID == s)
                   || (p.DepartmentID.Length > 4 && p.DepartmentID.Substring(0, 4) == s))).ToList();
            }

            if (search.DateStartFrom != default(DateTime) && search.DateStartTo != default(DateTime)) { item = item.Where(e => search.DateStartFrom.Date <= e.ReceivingDate.Value.Date && e.ReceivingDate.Value.Date <= search.DateStartTo.Date).ToList(); }

            
            return Json(item);
        }
        public IActionResult SaveRequisition(WH_PR_RequisitionHeader item, bool send)
        {
            if (item == null) return BadRequest();
            var DataSendMail = new WH_PR_RequisitionHeader();
            try
            {
                if (!string.IsNullOrEmpty(item.RequisitionDateStr)) {
                    item.RequisitionDate = item.RequisitionDateStr.stringToDateTime();
                }
                if (string.IsNullOrEmpty(item.RunningID)) // CREATE NEW
                {
                    int index = 0;
                    item.Create(user.UserID, user.UserName, _repo.RequisitionHeader.GetRunning(), send);

                    foreach (var d in item.WH_PR_RequisitionDetail)
                    {
                        var resultall = _repo.RequisitionHeader
                                         .FindByCondition(l => l.WH_PR_RequisitionDetail.Any(f => f.ItemID == d.ItemID)
                                                               && l.SYS_Status != "C01"
                                                               && l.RunningID != item.RunningID,
                                                            l => l.Include(v => v.WH_PR_RequisitionDetail))
                                          .SelectMany(g => g.WH_PR_RequisitionDetail
                                             .Where(f => f.ItemID == d.ItemID)) // ✅ กรองเฉพาะ ItemID ที่ตรง
                                         .ToList();
                        var MtQty = _repo.ItemName.FindByCondition(g => g.ItemID == d.ItemID).FirstOrDefault()?.Quantity ?? 0;
                        var TotalPayQty = (resultall.Sum(h => h.QtyRequisition) ?? 0) + (item.WH_PR_RequisitionDetail.Where(a => a.ItemID == d.ItemID).Sum(v => v.QtyRequisition) ?? 0);
                        if (MtQty < TotalPayQty)
                        {
                            return Json(new
                            {
                                success = false,
                                ex = $"รหัส {d.ItemID} มีจำนวนเบิก เกิน จากที่ขอเบิก  ที่มี {MtQty} / ขอเบิก {TotalPayQty}"
                            });
                        }
                        d.create(item.RunningID, item.RunningID + index.ToString("00"));
                        index++;
                    } 

                    _repo.RequisitionHeader.Create(item);
                    DataSendMail = item;
                }
                else // UPDATE EXISTING
                {
                    var old = _repo.RequisitionHeader.FindSingle(
                        l => l.RunningID == item.RunningID,
                        l => l.Include(f => f.WH_PR_RequisitionDetail)
                    );

                    if (old == null)
                        return Json(new { success = false, error = "ไม่พบข้อมูลที่ต้องการแก้ไข" });

                    // ---- Step D: Clear Detail เก่า ----
                    _repo.RequisitionDetail.DeleteByCondition(l => l.RunningID == item.RunningID);
                    if (!string.IsNullOrEmpty(item.RequisitionDateStr))
                    {
                        item.RequisitionDate = item.RequisitionDateStr.stringToDateTime();
                    }
                    // ---- Step H: Map Header ----
                    old.Edit(user.UserID, user.UserName, item, send);

                 
                    // ---- Step H: Map Detail ใหม่ ----
                    if(item?.WH_PR_RequisitionDetail != null)
                    {
                        int index = 0;
                        foreach (var d in item.WH_PR_RequisitionDetail)
                        {
                            var resultall = _repo.RequisitionHeader
                                                .FindByCondition(l => l.WH_PR_RequisitionDetail.Any(f => f.ItemID == d.ItemID)
                                                                      && l.SYS_Status != "C01"
                                                                      && l.RunningID != item.RunningID,
                                                                   l => l.Include(v => v.WH_PR_RequisitionDetail))
                                                 .SelectMany(g => g.WH_PR_RequisitionDetail
                                                    .Where(f => f.ItemID == d.ItemID)) // ✅ กรองเฉพาะ ItemID ที่ตรง
                                                .ToList();
                            var MtQty = _repo.ItemName.FindByCondition(g => g.ItemID == d.ItemID).FirstOrDefault()?.Quantity ?? 0;
                            var TotalPayQty = (resultall.Sum(h => h.QtyRequisition) ?? 0) + (item.WH_PR_RequisitionDetail.Where(a => a.ItemID == d.ItemID).Sum(v => v.QtyRequisition) ?? 0);
                            if (MtQty < TotalPayQty)
                            {
                                return Json(new
                                {
                                    success = false,
                                    ex = $"รหัส {d.ItemID} มีจำนวนเบิก เกิน จากที่ขอเบิก  ที่มี {MtQty} / ขอเบิก {TotalPayQty}"
                                });
                            }
                            d.create(old.RunningID, old.RunningID + index.ToString("00"));
                            old.WH_PR_RequisitionDetail.Add(d);
                            index++;

                        }

                        // ---- Step W: Update ----
                        _repo.RequisitionHeader.Update(old);

                    }
                   
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
                        var link = Url.Action(controllername, "Requisition", new { RunningID = item.RunningID }, Request.Scheme);
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
        [HttpPost]
        public IActionResult ApproveEq(string RunningID, string Approve2Status, string Approve2Remark, string sys)
        {
            try
            {
                if (sys != "P04")
                {
                    return Json(new { success = false, error = "ไม่สามารถทำรายการได้ เนื่องจากสถานะ ของ ใบนี้ เปลี่ยนไป แล้ว" });
                }
                var header = _repo.RequisitionHeader.FindSingle(x => x.RunningID == RunningID, l => l.Include(f => f.WH_PR_RequisitionDetail).ThenInclude(c => c.WH_MT_ItemName));
                if (header == null)
                    return Json(new { success = false, error = "ไม่พบข้อมูล" });
                header.Approve2Status = Approve2Status;
                header.Approve2Remark = Approve2Remark;
                header.Approve2Name = user.UserName;
                header.Approve2Date = DateTime.Now;
                if (Approve2Status == "Y")
                {
                    header.SYS_Status = "F01";
                    foreach (var item in header.WH_PR_RequisitionDetail)
                    {
                        var oldStockCard = new HR_PR_EquipmentStockCard()
                        {
                            RunningID = header.RunningID,
                            SerialNo = item.SerialNo,
                            CodeID = item.ItemID,
                            CodeName = item?.WH_MT_ItemName?.ItemNameTH,
                            QtyPay = item.QtyRequisition,
                            QtyReceived = 0,
                            EditID = user.UserID,
                            EditName = user.UserName,
                            EditDate = DateTime.Now,
                        };
                        var oldMt = _repo.ItemName.FindSingle(f => f.ItemID == item.ItemID) ?? null;
                        if (oldMt != null)
                        {
                            oldMt.EditID = user.UserID;
                            oldMt.EditName = user.UserName;
                            oldMt.EditDate = DateTime.Now;
                            oldMt.Quantity = oldMt.Quantity - item.QtyRequisition;
                            _repo.ItemName.Update(oldMt);
                        }
                        _repo.StockCard.Create(oldStockCard);
                    }
                   

                }
                else
                {
                    header.SYS_Status = "P03";
                    string refidlog = $"{header.RunningID.ToString()}";
                    var Log = new SYS_RejectLogs(UtilityHelper.ProgramId, refidlog, header.statusPageName, Approve2Remark, user.UserName);
                    _repo.RejectLogs.Create(Log);
                }


                _repo.RequisitionHeader.Update(header);
                _repo.Save();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        } [HttpPost]
        public IActionResult Approve(string RunningID, string Approve1Status, string Approve1Remark, string sys)
        {
            try
            {
                if (sys != "P02")
                {
                    return Json(new { success = false, error = "ไม่สามารถทำรายการได้ เนื่องจากสถานะ ของ ใบนี้ เปลี่ยนไป แล้ว" });
                }
                var header = _repo.RequisitionHeader.FindSingle(x => x.RunningID == RunningID, l => l.Include(f => f.WH_PR_RequisitionDetail));
                if (header == null)
                    return Json(new { success = false, error = "ไม่พบข้อมูล" });
                header.Approve1Status = Approve1Status;
                header.Approve1Remark = Approve1Remark;
                header.Approve1Name = user.UserName;
                header.Approve1Date = DateTime.Now;
                if (Approve1Status == "Y")
                {
                    header.SYS_Status = "P03";
                
                }
                else
                {
                    header.SYS_Status = "P01";
                    string refidlog = $"{header.RunningID.ToString()}";
                    var Log = new SYS_RejectLogs(UtilityHelper.ProgramId, refidlog, header.statusPageName, Approve1Remark, user.UserName);
                    _repo.RejectLogs.Create(Log);
                }


                _repo.RequisitionHeader.Update(header);
                _repo.Save();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
        public IActionResult List(Search Search)
        {
            ViewData["Title"] = "ระบบ ใบเบิกของ";
            ViewBag.Search = Search;
            Search.page = ((Search.page == 0) ? 1 : Search.page);
            ViewBag.CurrentSort = Search.currentSort;
            ViewBag.userPrivilege = userPrivilege;



            if (!string.IsNullOrEmpty(Search.receiveDateStart)) { Search.DateStartFrom = Search.receiveDateStart.stringToDateTime(); }
            if (!string.IsNullOrEmpty(Search.receiveDateEnd)) { Search.DateStartTo = Search.receiveDateEnd.stringToDateTime(); }
            var ViewAll = false;
            var DepPartment = new List<HR_MT_Department>(); 
           
            switch (Search.statusPage)
            {
                case "P01":
                    DepPartment = GetDepartmentByPermissionGetList(new List<string> { "P01" }).ToList();

                    ViewAll = userPrivilege.Any(l => (l.SYS_Status == "P01" && l.RightCode == "RC00"));
                    break;
                case "P02":
                    DepPartment = GetDepartmentByPermissionGetList(new List<string> { "P02" }).ToList();

                    ViewAll = userPrivilege.Any(l => (l.SYS_Status == "P02") && l.RightCode == "RCA1");
                    break;
                case "P03":
                    DepPartment = GetDepartmentByPermissionGetList(new List<string> { "P03" }).ToList();

                    ViewAll = userPrivilege.Any(l => (l.SYS_Status == "P03") && l.RightCode == "RCA2");
                    break;
                case "P04":
                    DepPartment = GetDepartmentByPermissionGetList(new List<string> { "P04" }).ToList();

                    ViewAll = userPrivilege.Any(l => (l.SYS_Status == "P04") && l.RightCode == "RCA3");
                    break;
                case "F01":
                case "C01":
                    DepPartment = GetDepartmentByPermissionGetList(new List<string> { "F01" }).ToList();

                    ViewAll = userPrivilege.Any(l => (l.SYS_Status == "F01") && l.RightCode == "RC00");
                    break;
                default:
                    break;
            }

            if (!ViewAll) { TempData["Error"] = "ท่านไม่มีสิทธิ์ใช้งาน"; return RedirectToAction("Index", "Home"); }
            ViewBag.ViewEdit =  CheckPer("P01", "RC01");
            Search.DepartmentPermistions = DepPartment.Select(l => l.DepartmentID).ToList();

            var ListItemss = _repo.RequisitionHeader.GetDataList(Search);
            var _TypeID =
                    new List<SelectListItem>
                    {
                            new SelectListItem {Text = "เบิกอะไหล่", Value = "001"},
                            new SelectListItem {Text = "เบิกบรรจุภัณฑ์", Value = "002"},
                    };
            ViewBag.TypeID = new SelectList(_TypeID, "Value", "Text", Search?.TypeID);
            ViewBag.DepartmentID = new SelectList(DepPartment.Where(d=>d.UsageStatus == true && d.DepartmentID.Length < 6).ToList(), "DepartmentID", "DepartmentIDAndName", Search?.DepartmentID); 
            return View(ListItemss);
        }

        public IActionResult PayDetail(Search item)
        {
            ViewBag.Edit = CheckPer("P01", "RC01");
            ViewBag.approve1 = CheckPer("P02", "RCA1");
            ViewBag.approve2 = CheckPer("P03", "RCA2");
            var result = _repo.RequisitionHeader.FindSingle(
                     l => l.RunningID == item.ID,
                     l => l
                         .Include(x => x.WH_PR_RequisitionDetail)
                             .ThenInclude(c => c.WH_MT_ItemName)
                         .Include(x => x.WH_PR_RequisitionDetail)
                             .ThenInclude(c => c.V_WH_JobDepartment)
                         .Include(x => x.V_HR_MT_Department)
                 ) ?? null;


            ViewBag.Search = item;
            var MTDepartment = _repo.MT_Department.FindByCondition(l => l.UsageStatus == true).ToList();
            ViewBag.DepartmentID = new SelectList(MTDepartment, "DepartmentID", "DepartmentIDAndName", result?.DepartmentID);
            return View(result);
        }

        [HttpGet]
        public IActionResult JobSearch(string q)
        {

            var DepPartment = GetDepartmentByPermissionGetList(new List<string> { "P01" }).Select(d => d.AccountingCode).ToList();

            var jobs = _repo.JobDepartment
                .FindByCondition(x => ( x.JobID.Contains(q) || x.JobName.Contains(q) || string.IsNullOrEmpty(q)) && DepPartment.Any(s=> s == x.JobAccountingCode))
                .Select(x => new {
                    jobID = x.JobID,
                    jobName = x.JobName
                })
                .Take(50) // กันข้อมูลเยอะเกิน
                .ToList();

            return Json(jobs);
        }

        [HttpGet]
        public IActionResult SerialSearch(string q,string id)
        {
            var jobs = _repo.StockCard
                .FindByCondition(x =>(x.CodeID == id) && ( x.SerialNo.Contains(q) || x.CodeName.Contains(q) || String.IsNullOrEmpty(q) ), f=>f.Include(d=>d.Inventory))
                .Select(x => new {
                    SerialNo = x.SerialNo ?? "",
                    CodeID = x.CodeID ?? "",
                    CodeName = x.CodeName ?? "",
                    LotNo = x.Inventory?.LotNo ?? "",
                    QtyRemain = x.QtyReceived - x.QtyPay ,
                    ExpireDateTH = x.Inventory?.ExpireDateTH ?? "",
                    LocationID = x.Inventory?.LocationID ?? "",
                })
                .Take(50) // กันข้อมูลเยอะเกิน
                .ToList();

            return Json(jobs);
        }
        public IActionResult SavePay(WH_PR_RequisitionHeader item, bool send)
        {
            if (item == null) return BadRequest();
            var DataSendMail = new WH_PR_RequisitionHeader();
            try
            {
                    var old = _repo.RequisitionHeader.FindSingle(
                        l => l.RunningID == item.RunningID,
                        l => l.Include(f => f.WH_PR_RequisitionDetail)
                    );

                    if (old == null)
                        return Json(new { success = false, error = "ไม่พบข้อมูลที่ต้องการแก้ไข" });
                if (item.WH_PR_RequisitionDetail.Any(f => string.IsNullOrEmpty(f.SerialNo))) { return Json(new { success = false, error = "ระบุ SerialNo" }); }
                    // ---- Step H: Map Header ----
                    old.Pay(user.UserID, user.UserName, item, send); 
                    // ---- Step W: Update ----
                    _repo.RequisitionHeader.Update(old);
                    DataSendMail = old;
            

                // Save to Database
                _repo.Save();
                if (send)
                {
                    if (DataSendMail.SYS_Status == "P04")
                    {
                        var rgApprove = Helper.RightCode.Approve1;
                        var controllername = "Detail";
                        var link = Url.Action(controllername, "Requisition", new { RunningID = item.RunningID }, Request.Scheme);
                        string detail = "รายการ พัสดุตรวจสอบการจ่าย  " + DataSendMail.statusPageName + item.RunningID;
                        if (!string.IsNullOrEmpty(DataSendMail.DepartmentID))
                        {
                            String DepartmentSendMail = ((DataSendMail.DepartmentID.Length < 4 ? DataSendMail.DepartmentID : DataSendMail.DepartmentID.Substring(0, 4)));

                            Helper.SendEmail(Helper.Company.GFPT, UtilityHelper.ProgramId, "มีรายการรอ พัสดุตรวจสอบการจ่าย", detail, DataSendMail.SYS_Status, rgApprove, DepartmentSendMail, user.UserName, link);

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
        [HttpPost]
        public IActionResult UpdateAS400([FromBody] UpdateAS400Request request)
        {
            if (request == null || request.Ids == null || !request.Ids.Any())
            {
                return Json(new { success = false, message = "ไม่มีรายการที่ถูกเลือก" });
            }

            try
            {

                _repo.RequisitionHeader.UPAS400(user.UserID, request.Ids);

                return Json(new { success = true, message = "ส่งข้อมูลไปยัง AS400 สำเร็จ" });
            }
            catch (Exception ex)
            {
                // เก็บ log หรือ handle exception ตามต้องการ
                return Json(new { success = false, message = $"เกิดข้อผิดพลาด: {ex.Message}" });
            }
        }
        public class UpdateAS400Request
        {
            public List<string> Ids { get; set; }
        }
        public IActionResult Reject(string id, string remark)
        {
            try
            {
                var item = _repo.RequisitionHeader.FindByCondition(l => l.RunningID == id).SingleOrDefault();
                if (item == null) { return Json(new { success = false, ex = "ไม่พบข้อมูล" }); }
                string refidlog = $"{item.RunningID.ToString()}";
                var Log = new SYS_RejectLogs(UtilityHelper.ProgramId, refidlog, item.statusPageName, remark, user.UserName);
                item.Cancel(user.UserID, user.UserName);
                _repo.RejectLogs.Create(Log);
                _repo.Save();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, ex = ex.Message });
            }
        }
        public IActionResult ListAs400(Search Search)
        {
            ViewData["Title"] = "อัปเดทเข้าระบบ AS400";
       
            ViewBag.Search = Search;
            Search.page = ((Search.page == 0) ? 1 : Search.page);
            ViewBag.CurrentSort = Search.currentSort;
            ViewBag.userPrivilege = userPrivilege;



            if (!string.IsNullOrEmpty(Search.receiveDateStart)) { Search.DateStartFrom = Search.receiveDateStart.stringToDateTime(); }
            if (!string.IsNullOrEmpty(Search.receiveDateEnd)) { Search.DateStartTo = Search.receiveDateEnd.stringToDateTime(); }
            var ViewAll = CheckPer("P05", "RC00");
 
          
            if (!ViewAll) { TempData["Error"] = "ท่านไม่มีสิทธิ์ใช้งาน"; return RedirectToAction("Index", "Home"); }
            ViewBag.ViewEdit = CheckPer("P05", "RC00");
            Search.ListAs400 = false;

            var ListItemss = _repo.RequisitionHeader.GetDataList(Search);
            var _TypeID =
                    new List<SelectListItem>
                    {
                            new SelectListItem {Text = "เบิกอะไหล่", Value = "001"},
                            new SelectListItem {Text = "เบิกบรรจุภัณฑ์", Value = "002"},
                    };
            ViewBag.TypeID = new SelectList(_TypeID, "Value", "Text", Search?.TypeID);
            ViewBag.DepartmentID = new SelectList(_repo.MT_Department.FindByCondition(d => d.UsageStatus == true && d.DepartmentID.Length < 6).ToList(), "DepartmentID", "DepartmentIDAndName", Search?.DepartmentID);
            return View(ListItemss);
        }
    }
}