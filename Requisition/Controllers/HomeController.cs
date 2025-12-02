using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GFPT.Extension.Utility.Std;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Requisition;
using Requisition.Data;
using Requisition.Models;

namespace Template_Tabler.Controllers
{
    public class HomeController : Controller
    {
        private IWrapper _repo;
        private string host;
        private readonly IHttpContextAccessor _httpContext;
        private readonly V_SYS_USER user;
        private readonly IEnumerable<V_SYS_USER_ROLE_PRIVILEGE> userPrivilege;
        public HomeController(IWrapper repo, IHttpContextAccessor httpContext)
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
            ViewData["Home"] = Url.Action("Index", "Home", Request.Scheme);
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
                        controller.ViewBag.userName = user.UserName;
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
            ViewBag.userPrivilege = userPrivilege;
            ViewBag.numberP01 = _repo.RequisitionHeader.FindByCondition(l => l.SYS_Status == "P01").Count() ;
            ViewBag.numberP02 = _repo.RequisitionHeader.FindByCondition(l => l.SYS_Status == "P02").Count() ;
            ViewBag.numberP03 = _repo.RequisitionHeader.FindByCondition(l => l.SYS_Status == "P03").Count() ;
            ViewBag.numberP04 = _repo.RequisitionHeader.FindByCondition(l => l.SYS_Status == "P04").Count() ;
            ViewBag.numberF01 = _repo.RequisitionHeader.FindByCondition(l => l.SYS_Status == "F01").Count() ;
            ViewBag.numberF02 = _repo.RequisitionHeader.FindByCondition(l => l.SYS_Status == "F01" && l.IsUpAs400 == false).Count() ;

            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IEnumerable<HR_MT_Department> GetDepartmentByPermissionGetList(List<string> status)
        {
            var GroupId = userPrivilege.Where(l => status.Contains(l.SYS_Status)).Select(l => l.GroupDepartment).Distinct().ToList();
            var DepartmentID = _repo.GroupDepartmentDetail.FindByCondition(e => GroupId.Contains(e.GroupDepID)).Select(l => l.DepartmentID).ToList();
            var MTDepartment = _repo.MT_Department.FindByCondition(l => l.UsageStatus == true && DepartmentID.Any(s => l.DepartmentID.StartsWith(s)));
            return MTDepartment;
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
