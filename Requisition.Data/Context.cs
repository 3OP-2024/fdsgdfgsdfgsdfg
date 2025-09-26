using Microsoft.EntityFrameworkCore;
using Requisition.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Requisition.Data
{
    public partial class Context : DbContext
    {
        public Context()
        {
        }
        public Context(DbContextOptions<Context> options)
              : base(options)
        {
        }
        public virtual DbSet<V_WH_MT_ItemName> V_WH_MT_ItemName { get; set; }
        public virtual DbSet<V_WH_JobDepartment> V_WH_JobDepartment { get; set; }
        public virtual DbSet<WH_RP_GroupParcel> WH_RP_GroupParcel { get; set; }

        public virtual DbSet<WH_PR_RequisitionDetail> WH_PR_RequisitionDetail { get; set; }
        public virtual DbSet<WH_PR_RequisitionHeader> WH_PR_RequisitionHeader { get; set; }

        public virtual DbSet<V_WH_StockCardReport> V_WH_StockCardReport { get; set; }
        public virtual DbSet<V_WH_RequisitionReceiving> V_WH_RequisitionReceiving { get; set; }

        public virtual DbSet<WH_MT_ItemName> WH_MT_ItemName { get; set; }
        public virtual DbSet<WH_MT_Location> WH_MT_Location { get; set; }
        public virtual DbSet<WH_MT_Zone> WH_MT_Zone { get; set; }
        public virtual DbSet<WH_PR_Supplier> WH_PR_Supplier { get; set; }
        public virtual DbSet<V_HR_PR_EquipmentInventory> V_HR_PR_EquipmentInventory { get; set; }
        public virtual DbSet<HR_PR_EquipmentBranch> HR_PR_EquipmentBranch { get; set; }
        public virtual DbSet<HR_PR_EquipmentClaimRate> HR_PR_EquipmentClaimRate { get; set; }
        public virtual DbSet<HR_PR_EquipmentInventory> HR_PR_EquipmentInventory { get; set; }
        public virtual DbSet<HR_PR_EquipmentStockCard> HR_PR_EquipmentStockCard { get; set; }

        public virtual DbSet<V_WH_RE_PR> V_WH_RE_PR { get; set; }
        public virtual DbSet<V_HR_MT_Department> V_HR_MT_Department { get; set; }
        public virtual DbSet<V_Sys_Program_Document> V_Sys_Program_Document { get; set; }
        public virtual DbSet<Sys_Program> Sys_Program { get; set; }
        //public virtual DbSet<HR_PR_EquipmentLocation> HR_PR_EquipmentLocation { get; set; }
        //public virtual DbSet<HR_PR_EquipmentZone> HR_PR_EquipmentZone { get; set; }
        public virtual DbSet<SYS_RejectLogs> SYS_RejectLogs { get; set; }
        public virtual DbSet<V_SYS_USER> V_SYS_USER { get; set; }
        public virtual DbSet<V_SYS_USER_ROLE_PRIVILEGE> V_SYS_USER_ROLE_PRIVILEGE { get; set; }
        public virtual DbSet<HR_MT_SubSection> HR_MT_SubSection { get; set; }

        public virtual DbSet<V_HR_MT_Employee> V_HR_MT_Employee { get; set; }
        public virtual DbSet<SYS_GroupDepartmentDetail> SYS_GroupDepartmentDetail { get; set; }
        public virtual DbSet<Sys_ProgramStatus> Sys_ProgramStatus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<V_WH_RequisitionReceiving>(entity =>
            {
                entity.HasKey(e => new { e.RunningID, e.ProductCode, e.RequisitionNo });
            });
            modelBuilder.Entity<V_WH_StockCardReport>(entity =>
            {
                entity.HasKey(e => new { e.CodeID, e.LocationID });
            });
            modelBuilder.Entity<V_WH_RE_PR>(entity =>
            {
                entity.HasKey(e => new { e.ShopName, e.ShopID, e.PODate, e.PONO, e.ItemOrder, e.DepartmentID, e.RequestDate, e.RequestNo });
            });
            modelBuilder.Entity<HR_PR_EquipmentStockCard>(entity =>
            {
                entity.HasKey(e => new { e.CodeID, e.RunningID });
            });

          
           

            modelBuilder.Entity<Sys_ProgramStatus>(entity =>
            {
                entity.HasKey(e => new { e.ProgramID, e.StatusID })
                    .HasName("PK_Sys_ProgramStatus_1");
            });
            modelBuilder.Entity<HR_MT_Department>(entity =>
            {
                entity.Property(e => e.DepartmentID).ValueGeneratedNever();



                entity.Property(e => e.UsageStatus).HasDefaultValueSql("((1))");
            });
            modelBuilder.Entity<HR_MT_SubSection>(entity =>
            {

                entity.HasKey(e => new { e.SubSectionID, e.DepartmentID })
                    .HasName("PK_HR_MT_SubSection_1");

                entity.Property(e => e.UsageStatus).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.HR_MT_SubSection)
                    .HasForeignKey(d => d.DepartmentID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HR_MT_SubSection_HR_MT_Department");
            });
            modelBuilder.Entity<SYS_GroupDepartmentDetail>(entity =>
            {
                entity.HasKey(e => new { e.GroupDepID, e.DepartmentID });
            }); modelBuilder.Entity<V_SYS_USER_ROLE_PRIVILEGE>(entity => {
                entity.HasKey(e => new { e.UserID, e.RoleID, e.RightCode });
                //entity.HasQueryFilter(x => x.ProgramID == 2001);
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
