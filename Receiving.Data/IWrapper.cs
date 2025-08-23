using System;
using System.Collections.Generic;
using System.Text;

namespace Receiving.Data
{
    public interface IWrapper
    {


        IEquipmentZoneRepository EquipmentZone { get; }
        IStockCardRepository StockCard { get; }
        IEquipmentInventoryRepository Inventory { get; }
        IReceivingDetailRepository ReceivingDetail { get; }
        IRejectLogsRepository RejectLogs { get; }
        IVDepartmentRepository VDepartment { get; }
        IVWHRepository VWH { get; }

        IReceivingHeaderRepository ReceivingHeader { get; }



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
