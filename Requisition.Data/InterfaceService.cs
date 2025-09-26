

using GFPT.Extension.Paging.Std20;
using Microsoft.EntityFrameworkCore;
using Requisition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Requisition.Data
{
    


    public interface IVItemNameRepository : IRepository<V_WH_MT_ItemName>
    {
    }
    public class VItemNameRepository : Repository<V_WH_MT_ItemName>, IVItemNameRepository
    {
        public VItemNameRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }
    }
    public interface IJobDepartmentRepository : IRepository<V_WH_JobDepartment>
    {
    }
    public class  JobDepartmentRepository : Repository<V_WH_JobDepartment>, IJobDepartmentRepository
    {
        public JobDepartmentRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }
    }

    public interface IStockCardReportRepository : IRepository<V_WH_StockCardReport>
    {
    }
    public class StockCardReportRepository : Repository<V_WH_StockCardReport>, IStockCardReportRepository
    {
        public StockCardReportRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }
    }

    public interface IV_WH_RequisitionReceivingRepository : IRepository<V_WH_RequisitionReceiving>
    {
    }
    public class  V_WH_RequisitionReceivingRepository : Repository<V_WH_RequisitionReceiving>, IV_WH_RequisitionReceivingRepository
    {
        public V_WH_RequisitionReceivingRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }
    }

    public interface IItemNameRepository : IRepository<WH_MT_ItemName>
    {
    }
    public class ItemNameRepository : Repository<WH_MT_ItemName>, IItemNameRepository
    {
        public ItemNameRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }
    }










    public interface IClaimRateRepository : IRepository<HR_PR_EquipmentClaimRate>
    {
        int GetRunning();
        HelperPaging<HR_PR_EquipmentClaimRate> GetDataList(Search search);
    }
    public class  ClaimRateRepository : Repository<HR_PR_EquipmentClaimRate>, IClaimRateRepository
    {
        public ClaimRateRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }
        public int GetRunning()
        {
            var runningId = 0;
            try
            { 
                var result = RepositoryContext.HR_PR_EquipmentClaimRate.Max(e => e.ClaimRateID.Substring(1));
                runningId = Convert.ToInt32(result) + 1;
            }
            catch { runningId = -1; }
            return runningId;
        }
        public HelperPaging<HR_PR_EquipmentClaimRate> GetDataList(Search search)
        {
            try
            { 

                var items = RepositoryContext.HR_PR_EquipmentClaimRate.Where(l =>
                                                                         (l.ClaimRateID.Contains(search.RunningID)   ||  string.IsNullOrEmpty(search.RunningID))   
                                                                    );
                bool usestatus = false;
                if (search.TypeID == "1") { usestatus = true; }
                if (!string.IsNullOrEmpty(search?.TypeID))
                {
                    items = items.Where(l => l.UsageStatus == usestatus);
                }

                search.sorting = String.IsNullOrEmpty(search.sorting) ? "ClaimRateID" : search.sorting;
                search.currentSort = String.IsNullOrEmpty(search.currentSort) ? "" : search.currentSort; 
                var result = new HelperPaging<HR_PR_EquipmentClaimRate>(items.OrderByDescending(l => l.ClaimRateID), (search.page != 0 ? search.page : 1), search.PageSetNumber, search.PageOn); 
                result.Items = result.ToList();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        } 
        
    }

    public interface IBranchRepository : IRepository<HR_PR_EquipmentBranch>
    {
        int GetRunning();
        HelperPaging<HR_PR_EquipmentBranch> GetDataList(Search search);
    }
    public class BranchRepository : Repository<HR_PR_EquipmentBranch>, IBranchRepository
    {
        public BranchRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }
        public int GetRunning()
        {
            var runningId = 0;
            try
            { 
                var result = RepositoryContext.HR_PR_EquipmentBranch.Max(e => e.BranchID.Substring(1));
                runningId = Convert.ToInt32(result) + 1;
            }
            catch { runningId = -1; }
            return runningId;
        }
        public HelperPaging<HR_PR_EquipmentBranch> GetDataList(Search search)
        {
            try
            { 

                var items = RepositoryContext.HR_PR_EquipmentBranch.Where(l =>
                                                                         (l.BranchName.Contains(search.RunningID) || l.BranchID.Contains(search.RunningID) ||  string.IsNullOrEmpty(search.RunningID))   
                                                                    );
                bool usestatus = false;
                if (search.TypeID == "1") { usestatus = true; }
                if (!string.IsNullOrEmpty(search?.TypeID))
                {
                    items = items.Where(l => l.UsageStatus == usestatus);
                }

                search.sorting = String.IsNullOrEmpty(search.sorting) ? "BranchID" : search.sorting;
                search.currentSort = String.IsNullOrEmpty(search.currentSort) ? "" : search.currentSort; 
                var result = new HelperPaging<HR_PR_EquipmentBranch>(items.OrderByDescending(l => l.BranchID), (search.page != 0 ? search.page : 1), search.PageSetNumber, search.PageOn); 
                result.Items = result.ToList();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        } 
        
    }



    public interface IEquipmentZoneRepository : IRepository<WH_MT_Zone>
    { 
        HelperPaging<WH_MT_Zone> GetDataList(Search search);
    }
    public class  EquipmentZoneRepository : Repository<WH_MT_Zone>, IEquipmentZoneRepository
    {
        public EquipmentZoneRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }
        public HelperPaging<WH_MT_Zone> GetDataList(Search search)
        {
            try
            {
                
         

                var items = RepositoryContext.WH_MT_Zone.Include(s => s.WH_MT_Location) 
                                                                    .Where(l =>
                                                                         (l.ZoneName.Contains(search.RunningID) || l.ZoneID.Contains(search.RunningID) ||  string.IsNullOrEmpty(search.RunningID))   
                                                                    );
                bool usestatus = false;
                if (search.TypeID == "1") { usestatus = true; }
                if (!string.IsNullOrEmpty(search?.TypeID))
                {
                    items = items.Where(l => l.UsageStatus == usestatus);
                }

                search.sorting = String.IsNullOrEmpty(search.sorting) ? "ZoneID" : search.sorting;
                search.currentSort = String.IsNullOrEmpty(search.currentSort) ? "" : search.currentSort; 
                var result = new HelperPaging<WH_MT_Zone>(items.OrderByDescending(l => l.ZoneID), (search.page != 0 ? search.page : 1), search.PageSetNumber, search.PageOn); 
                result.Items = result.ToList();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        
    }

   public interface IVInventoryRepository : IRepository<V_HR_PR_EquipmentInventory>
    {
        HelperPaging<V_HR_PR_EquipmentInventory> GetDataList(Search search);
    }
    public class VInventoryRepository : Repository<V_HR_PR_EquipmentInventory>, IVInventoryRepository
    {
        public VInventoryRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }
        public HelperPaging<V_HR_PR_EquipmentInventory> GetDataList(Search search)
        {
            try
            { 

                var items = RepositoryContext.V_HR_PR_EquipmentInventory
                                                                    .Where(l =>
                                                                         (l.CodeID.Contains(search.RunningID) || l.CodeName.Contains(search.RunningID) || string.IsNullOrEmpty(search.RunningID))
                                                                        && (l.ReceivingType.Contains(search.TypeID) || string.IsNullOrEmpty(search.TypeID))
                                                                    ); 
                search.sorting = String.IsNullOrEmpty(search.sorting) ? "CodeID" : search.sorting;
                search.currentSort = String.IsNullOrEmpty(search.currentSort) ? "" : search.currentSort;
                var result = new HelperPaging<V_HR_PR_EquipmentInventory>(items.OrderByDescending(l => l.CodeID), (search.page != 0 ? search.page : 1), search.PageSetNumber, search.PageOn);
                result.Items = result.ToList();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



    }

    public interface ILocationRepository : IRepository<WH_MT_Location>
    { 
    }
    public class  LocationRepository : Repository<WH_MT_Location>, ILocationRepository
    {
        public LocationRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }
 

        
    }









    public interface IRequisitionHeaderRepository : IRepository<WH_PR_RequisitionHeader>
    {
        string GetRunning();
        HelperPaging<WH_PR_RequisitionHeader> GetDataList(Search search);
    }
    public class RequisitionHeaderRepository : Repository<WH_PR_RequisitionHeader>, IRequisitionHeaderRepository
    {
        public RequisitionHeaderRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }
        public HelperPaging<WH_PR_RequisitionHeader> GetDataList(Search search)
        {
            try
            {
                var items = RepositoryContext.WH_PR_RequisitionHeader
                                                                    .Include(s => s.WH_PR_RequisitionDetail)
                                                                    .Include(s => s.V_HR_MT_Department)
                                                                    .Where(l =>
                                                                         (l.RunningID.Contains(search.RunningID) ||  string.IsNullOrEmpty(search.RunningID))
                                                                        && (l.RequisitionNo.Contains(search.RequestNo) ||  string.IsNullOrEmpty(search.RequestNo))
                                                                        && (l.DepartmentID.StartsWith(search.DepartmentID) || l.DepartmentID.Contains(search.DepartmentID) ||  string.IsNullOrEmpty(search.DepartmentID))
                                                                        && (l.RequisitionType.StartsWith(search.TypeID) || string.IsNullOrEmpty(search.TypeID)) 

                                                                    );



                if (search.DateStartFrom != default(DateTime) && search.DateStartTo != default(DateTime)) { items = items.Where(e => search.DateStartFrom.Date <= e.RequisitionDate.Value.Date && e.RequisitionDate.Value.Date <= search.DateStartTo.Date); }

               

                search.sorting = String.IsNullOrEmpty(search.sorting) ? "RunningID" : search.sorting;
                search.currentSort = String.IsNullOrEmpty(search.currentSort) ? "" : search.currentSort;


                switch (search.statusPage)
                {
                    case "F01":
                        if (search.PageOn == false)
                        {
                            items = items.Where(l => new List<string>() { "C01", "F01", "F02" }.Contains(l.SYS_Status));
                        }
                        else
                        {
                            items = items.Where(l => new List<string>() { "C01", "F01" }.Contains(l.SYS_Status));
                        }

                        break;
                    case "F02":
                        items = items.Where(l => new List<string>() { "F01" }.Contains(l.SYS_Status));
                        break;
                    case "C01":
                        items = items.Where(l => new List<string>() { "C01" }.Contains(l.SYS_Status));
                        break;
                    case "P02":
                        items = items.Where(l => new List<string>() { "P02" }.Contains(l.SYS_Status));
                        break;
                    case "P03":
                        items = items.Where(l => new List<string>() { "P03" }.Contains(l.SYS_Status));
                        break;
                    case "P04":
                        items = items.Where(l => new List<string>() { "P04" }.Contains(l.SYS_Status));
                        break; 
                    default:
                        items = items.Where(l =>
                            !new List<string>() { "C01", "F01", "P00" }.Contains(l.SYS_Status)
                            
                        );
                        break;
                }


                var result = new HelperPaging<WH_PR_RequisitionHeader>(items, (search.page != 0 ? search.page : 1), search.PageSetNumber, search.PageOn);
               
                result.Items = result.ToList();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public string GetRunning()
        {
            var runningId = 0;

            // Use day in credential to ensure uniqueness for the same day
            var credential = $"W{DateTime.Now.Year.ToString("0000")}{DateTime.Now.Month.ToString("00")}{DateTime.Now.Day.ToString("00")}";

            try
            {
                // Filter by year, month, and day
                var result = RepositoryContext.WH_PR_RequisitionHeader
                              .Where(e => e.RunningID.StartsWith(credential))
                              .Max(e => e.RunningID);

                if (result != null)
                {
                    runningId = Convert.ToInt32(result.Substring(9)); // Adjust index to include day
                }
            }
            catch
            {
                runningId = -1;
            }

            // Increment the runningId
            runningId++;

            // Create the new RunningHeaderID with day included
            credential = $"W{DateTime.Now.Year.ToString("0000")}{DateTime.Now.Month.ToString("00")}{DateTime.Now.Day.ToString("00")}{runningId.ToString("00")}";

            return credential;
        }
    }




    public interface IRequisitionDetailRepository : IRepository<WH_PR_RequisitionDetail>
    {
    }
    public class  RequisitionDetailRepository : Repository<WH_PR_RequisitionDetail>, IRequisitionDetailRepository
    {
        public RequisitionDetailRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }
    }

  


     public interface IStockCardRepository : IRepository<HR_PR_EquipmentStockCard>
    {
    }
    public class  StockCardRepository : Repository<HR_PR_EquipmentStockCard>, IStockCardRepository
    {
        public StockCardRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }
    }

    public interface IEquipmentInventoryRepository : IRepository<HR_PR_EquipmentInventory>
    {
        int GetRunning();
        HelperPaging<HR_PR_EquipmentInventory> GetDataList(Search search);
    }
    public class  EquipmentInventoryRepository : Repository<HR_PR_EquipmentInventory>, IEquipmentInventoryRepository
    {
        public EquipmentInventoryRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }
        public HelperPaging<HR_PR_EquipmentInventory> GetDataList(Search search)
        {
            try
            {
                var items = RepositoryContext.HR_PR_EquipmentInventory.Include(d=>d.WH_MT_Location)  
                                                                    .Where(l =>
                                                                         (l.RunningID.Contains(search.RunningID) || string.IsNullOrEmpty(search.RunningID))
                                                                        && (l.SerialNo.Contains(search.SerialNo) || string.IsNullOrEmpty(search.SerialNo))
                                                                        && (l.ReceivingType.Contains(search.TypeID) || string.IsNullOrEmpty(search.TypeID))
                                                                        && (l.CodeID.Contains(search.critiriaVendor) || l.CodeName.Contains(search.critiriaVendor) || string.IsNullOrEmpty(search.critiriaVendor))
 
                                                                    );



                if (search.DateStartFrom != default(DateTime) && search.DateStartTo != default(DateTime)) { items = items.Where(e => search.DateStartFrom.Date <= e.CreateDate.Value.Date && e.CreateDate.Value.Date <= search.DateStartTo.Date); }
 
                search.sorting = String.IsNullOrEmpty(search.sorting) ? "SerialNo" : search.sorting;
                search.currentSort = String.IsNullOrEmpty(search.currentSort) ? "" : search.currentSort;


                switch (search.statusPage)
                {
                    case "F01":
                        if (search.PageOn == false)
                        {
                            items = items.Where(l => new List<string>() { "C01", "F01", "F02" }.Contains(l.SYS_Status));
                        }
                        else
                        {
                            items = items.Where(l => new List<string>() { "C01", "F01" }.Contains(l.SYS_Status));
                        }

                        break;
                    case "P01":
                        items = items.Where(l => new List<string>() { "P01", "P02" }.Contains(l.SYS_Status));
                        break;
                 
                    default:
                        items = items.Where(l =>
                            !new List<string>() { "C01", "F01", "P00" }.Contains(l.SYS_Status)
                            && (l.SYS_Status.Contains(search._typestatus) || string.IsNullOrEmpty(search._typestatus))
                        );
                        break;
                }


                var result = new HelperPaging<HR_PR_EquipmentInventory>(items.OrderByDescending(l => l.RunningID), (search.page != 0 ? search.page : 1), search.PageSetNumber, search.PageOn);

                result.Items = result.ToList();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public int GetRunning()
        {
            var runningId = 0;

            // Use day in credential to ensure uniqueness for the same day
            var credential = $"{DateTime.Now.Year.ToString("0000")}{DateTime.Now.Month.ToString("00")}{DateTime.Now.Day.ToString("00")}"; 
            try
            {
                // Filter by year, month, and day
                var result = RepositoryContext.HR_PR_EquipmentInventory
                              .Where(e => e.SerialNo.StartsWith(credential))
                              .Max(e => e.SerialNo);

                if (result != null)
                {
                    runningId = Convert.ToInt32(result.Substring(8)); // Adjust index to include day
                }
            }
            catch
            {
                runningId = -1;
            }

            // Increment the runningId
            runningId++; 
            // Create the new RunningHeaderID with day included
 
            return runningId;
        }
    }

    public interface IVWHRepository : IRepository<V_WH_RE_PR>
    {
    }
    public class VWHRepository : Repository<V_WH_RE_PR>, IVWHRepository
    {
        public VWHRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }
    }

    public interface IRejectLogsRepository : IRepository<SYS_RejectLogs>
    {
    }
    public class RejectLogsRepository : Repository<SYS_RejectLogs>, IRejectLogsRepository
    {
        public RejectLogsRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }
    } 


    public interface IMT_DepartmentRepository : IRepository<HR_MT_Department>
    {
    }
    public class MT_DepartmentRepository : Repository<HR_MT_Department>, IMT_DepartmentRepository
    {
        public MT_DepartmentRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }
    }

    public interface IGroupDepartmentDetailRepository : IRepository<SYS_GroupDepartmentDetail>
    {
    }
    public class GroupDepartmentDetailRepository : Repository<SYS_GroupDepartmentDetail>, IGroupDepartmentDetailRepository
    {
        public GroupDepartmentDetailRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }
    }
    public interface ISysUserRepository : IRepository<V_SYS_USER>
    {
    }
    public class SysUserRepository : Repository<V_SYS_USER>, ISysUserRepository
    {
        public SysUserRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }
    }


    public interface IProgramRepository : IRepository<Sys_Program>
    {
    }
    public class ProgramRepository : Repository<Sys_Program>, IProgramRepository
    {
        public ProgramRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }

    }


    public interface IProgramDocumentRepository : IRepository<V_Sys_Program_Document>
    {
    }
    public class ProgramDocumentRepository : Repository<V_Sys_Program_Document>, IProgramDocumentRepository
    {
        public ProgramDocumentRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }
    }




    public interface IVMTEmployeeRepository : IRepository<V_HR_MT_Employee>
    {
        IEnumerable<V_HR_MT_Employee> GetEmployeeByCritiriaByPErmisstions(List<string> departmentPermission, string critiria, string department, string subSection, DateTime workingDateFrom = default(DateTime), DateTime workingDateTo = default(DateTime));
    }
    public class VMTEmployeeRepository : Repository<V_HR_MT_Employee>, IVMTEmployeeRepository
    {
        public VMTEmployeeRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }

        public IEnumerable<V_HR_MT_Employee> GetEmployeeByCritiriaByPErmisstions(List<string> departmentPermission, string critiria, string department, string subSection, DateTime workingDateFrom = default(DateTime), DateTime workingDateTo = default(DateTime))
        {
            var item = RepositoryContext.V_HR_MT_Employee.Where(e => departmentPermission.Any(s =>
                                          (e.DepartmentID.Length <= 4 && e.DepartmentID == s)
                                       || (e.DepartmentID.Length > 4 && e.DepartmentID.Substring(0, 4) == s)
                                            ));
            if (!string.IsNullOrEmpty(critiria)) { critiria = critiria.Trim().ToLower().ToUpper(); }

            var result = item.Include(l => l.V_HR_MT_Department)
                .Where(e =>
                                              (e.EmployeeNameTH.Contains(critiria) || e.EmployeeID.Contains(critiria) || String.IsNullOrEmpty(critiria))
                                           && (e.ResignTypeID == "")
                                           && (e.DepartmentID.StartsWith(department) || String.IsNullOrEmpty(department))
                                           && (e.SubSectionID == subSection || String.IsNullOrEmpty(subSection))
                                  );
            if (workingDateFrom != default(DateTime) && workingDateTo != default(DateTime)) { result = result.Where(e => workingDateFrom <= e.StartingDate && e.StartingDate <= workingDateTo); }

            return result.ToList();
        }
    }

    public interface IUserPrivilegeRepository : IRepository<V_SYS_USER_ROLE_PRIVILEGE>
    {
    }
    public class UserPrivilegeRepository : Repository<V_SYS_USER_ROLE_PRIVILEGE>, IUserPrivilegeRepository
    {
        public UserPrivilegeRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }

    }
  

     
    public interface IVDepartmentRepository : IRepository<V_HR_MT_Department>
    {
    }
    public class VDepartmentRepository : Repository<V_HR_MT_Department>, IVDepartmentRepository
    {
        public VDepartmentRepository(Context repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
