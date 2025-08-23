using Itenso.TimePeriod;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Receiving.Models
{
    public partial class V_HR_MT_Employee
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string EmployeeID { get; set; }

        [StringLength(50)]
        public string EmployeeTypeName { get; set; }
        public string EmployeeTypeID { get; set; }
        public string NationalityID { get; set; }
        public string ResignDateTH
        {
            get
            {
                if (ResignDate == null)
                {
                    return "";
                }
                return ResignDate.Value.ToString("dd/MM/yyyy");
            }
        }
        public int? NameTitle { get; set; }

        [StringLength(50)]
        public string NameTitleEN { get; set; }

        [StringLength(50)]
        public string NameTitleTH { get; set; }
        public string IdentificationNumber { get; set; }

        [StringLength(50)]
        public string EmployeeNameTH { get; set; }

        [StringLength(50)]
        public string EmployeeLastNameTH { get; set; }

        [StringLength(153)]
        public string EmployeeFullNameTH { get; set; }

        [StringLength(50)]
        public string EmployeeNameEN { get; set; }

        [StringLength(50)]
        public string EmployeeLastNameEN { get; set; }

        [StringLength(153)]
        public string EmployeeFullNameEN { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartingDate { get; set; }

        [StringLength(3)]
        public string PositionID { get; set; }

        [StringLength(10)]
        public string LevelID { get; set; }

        [StringLength(50)]
        public string PositionNameEN { get; set; }

        [StringLength(50)]
        public string PositionNameTH { get; set; }

        [StringLength(6)]
        public string DepartmentID { get; set; }

        [StringLength(50)]
        public string DepartmentNameEN { get; set; }

        [StringLength(50)]
        public string DepartmentNameTH { get; set; }
        public string SubSectionID { get; set; }
        [StringLength(2)]
        public string ResignTypeID { get; set; }

        [StringLength(50)]
        public string ResignTypeName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ResignDate { get; set; }
        public string EditID { get; set; }
        public string Tel { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string EditName { get; set; }

        public DateTime? EditDate { get; set; }
        [StringLength(1)]
        public string GenderCode { get; set; }
        public int IsForeigner { get; set; }


        //[ForeignKey("EmployeeID")]
        // public virtual V_SYS_USER USER { get; set; }

        [ForeignKey("DepartmentID")]
        public virtual V_HR_MT_Department V_HR_MT_Department { get; set; }
        public string DepartmentID4Name
        {
            get
            {
                if (V_HR_MT_Department == null) { return ""; }
                return ((V_HR_MT_Department.DepartmentID.Length < 4 ? V_HR_MT_Department?.DepartmentID + "-" + V_HR_MT_Department?.Division : V_HR_MT_Department.DepartmentID.Substring(0, 4) + "-" + V_HR_MT_Department.Department));
            }
        }
        [NotMapped]
        public string DepartmentIDAndName
        {
            get
            {
                return DepartmentID + "-" + DepartmentNameTH;
            }
        }
        [NotMapped]
        public string age
        {
            get
            {

                var item = ToAge(DateOfBirth, DateTime.Now, "ym");
                return item;

            }

        }

        public string DateOfBirthTH
        {
            get
            {
                if (DateOfBirth == null)
                {
                    return "";
                }
                return DateOfBirth.Value.ToString("dd/MM/yyyy");
            }
        }

        [NotMapped]
        public string workdulation
        {
            get
            {
                if (ResignTypeID != "" && ResignDate != null) { return ToAge(StartingDate, ResignDate.Value, "ymd"); ; }
                var item = ToAge(StartingDate, DateTime.Now, "ym");
                return item;

            }

        }
        [NotMapped]
        public int workdulationNumber
        {
            get
            {
                if (StartingDate == null)
                {
                    return 0;
                }

                var year = new DateDiff(StartingDate.Value, DateTime.Now).ElapsedYears;
                if (year == 0) return 0;
                return (year * 12);
            }

        }

        private int ToAgeYear(DateTime? bday, DateTime defaultDay)
        {
            return new DateDiff(bday.Value, defaultDay).ElapsedYears;
        }

        private string ToAge(DateTime? bday, DateTime defaultDay, string format)
        {

            if (bday == null)
            {
                return "";
            }

            var result = "";

            if (format.Contains("y"))
            {
                result += $"{new DateDiff(bday.Value, defaultDay).ElapsedYears} ปี ";
            }

            if (format.Contains("m"))
            {
                result += $"{new DateDiff(bday.Value, defaultDay).ElapsedMonths} เดือน ";
            }

            if (format.Contains("d"))
            {
                result += $"{new DateDiff(bday.Value, defaultDay).ElapsedDays} วัน ";
            }

            return result;
        }
        public string StartingDateTH
        {
            get
            {
                if (StartingDate == null)
                {
                    return "";
                }
                return StartingDate.Value.ToString("dd/MM/yyyy");
            }
        }

        [NotMapped]
        public string DocumentTypeID { get; set; }
        [NotMapped]
        public string PayTypeIDandNameTH { get; set; }
        [NotMapped]
        public string DocumentNumber { get; set; }
        [NotMapped]
        public string StartDateOld { get; set; }
        [NotMapped]
        public string ExpireDateOld { get; set; }
        [NotMapped]
        public string ReQuestDate { get; set; }
        [NotMapped]
        public string PayDate { get; set; }
        [NotMapped]
        public string StartDateNew { get; set; }
        [NotMapped]
        public string ExpireDateNew { get; set; }
        [NotMapped]
        public string Amount { get; set; }
        [NotMapped]
        public decimal AmountInt { get; set; }
        [NotMapped]
        public string PayTypeID { get; set; }
        [NotMapped]
        public string Checked { get; set; }

        [NotMapped]
        public string StartingDateStr { get; set; }
        [NotMapped]
        public string ResignDateStr { get; set; }

        [NotMapped]

        public string AgeVisa { get; set; }
        [NotMapped]
        public string EmployeeName { get; set; }
        [NotMapped]
        public string Department { get; set; }
        [NotMapped]
        public string SubSection { get; set; }
        [NotMapped]
        public string departmentID4 { get; set; }
        [NotMapped]
        public string EndDocDateOld { get; set; }
        [NotMapped]
        public string DocumentTypeIDTH { get; set; }
        [NotMapped]
        public string StartingDateOld { get; set; }
    }
}
