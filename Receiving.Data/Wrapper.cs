using System;
using System.Collections.Generic;
using System.Text;

namespace Receiving.Data
{
    public class Wrapper : IWrapper
    {
        private Context _repoContext;

        public Wrapper(Context repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        

        public IVInventoryRepository VInventory => new VInventoryRepository(_repoContext);
        public ILocationRepository Location => new LocationRepository(_repoContext);
        public IStockCardRepository StockCard => new  StockCardRepository(_repoContext);
        public IEquipmentInventoryRepository Inventory => new EquipmentInventoryRepository(_repoContext);
        public IEquipmentZoneRepository EquipmentZone => new  EquipmentZoneRepository(_repoContext);
        public IBranchRepository Branch => new  BranchRepository(_repoContext);
        public IClaimRateRepository ClaimRate   => new ClaimRateRepository(_repoContext);

          

        public IReceivingDetailRepository ReceivingDetail => new ReceivingDetailRepository(_repoContext);
        public IReceivingHeaderRepository ReceivingHeader => new ReceivingHeaderRepository(_repoContext);
        public IVDepartmentRepository VDepartment => new VDepartmentRepository(_repoContext);
      
        public IRejectLogsRepository RejectLogs => new RejectLogsRepository(_repoContext);
        public IVWHRepository VWH => new VWHRepository(_repoContext); 


        public IGroupDepartmentDetailRepository GroupDepartmentDetail => new GroupDepartmentDetailRepository(_repoContext);
        public IMT_DepartmentRepository MT_Department => new MT_DepartmentRepository(_repoContext);

        public IProgramRepository Program => new ProgramRepository(_repoContext);
        public IVMTEmployeeRepository Employee => new VMTEmployeeRepository(_repoContext);
        public ISysUserRepository SysUser => new SysUserRepository(_repoContext);
        public IProgramDocumentRepository SysDocProgram => new ProgramDocumentRepository(_repoContext);
        public IUserPrivilegeRepository UserPrivilege => new UserPrivilegeRepository(_repoContext);
        public int Save()
        {
            return _repoContext.SaveChanges();
        }
    }
}
