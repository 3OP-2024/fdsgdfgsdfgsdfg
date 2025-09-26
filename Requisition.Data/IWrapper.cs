using System;
using System.Collections.Generic;
using System.Text;

namespace Requisition.Data
{
    public interface IWrapper
    {
        IVItemNameRepository VItemName { get; } 
        IJobDepartmentRepository JobDepartment { get; } 
        IRequisitionDetailRepository RequisitionDetail { get; } 
        IRequisitionHeaderRepository  RequisitionHeader  { get; } 
        IV_WH_RequisitionReceivingRepository VRequisitionReceiving { get; } 
        IStockCardReportRepository StockCardReport { get; } 
        IItemNameRepository ItemName { get; } 
         ILocationRepository Location { get; } 
        IClaimRateRepository ClaimRate { get; }
        IBranchRepository Branch { get; }

        IEquipmentZoneRepository EquipmentZone { get; }
        IStockCardRepository StockCard { get; }
        IEquipmentInventoryRepository Inventory { get; }
        IRejectLogsRepository RejectLogs { get; }
        IVDepartmentRepository VDepartment { get; }
        IVWHRepository VWH { get; }

        IVInventoryRepository VInventory { get; }



        IProgramRepository Program { get; }
        IProgramDocumentRepository SysDocProgram { get; }
        IVMTEmployeeRepository Employee { get; }
        ISysUserRepository SysUser { get; }
        IUserPrivilegeRepository UserPrivilege { get; }
        IGroupDepartmentDetailRepository GroupDepartmentDetail { get; }
        IMT_DepartmentRepository MT_Department { get; }
        int Save();
    }
}
