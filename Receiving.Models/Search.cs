using System;
using System.Collections.Generic;
using System.Text;

namespace Receiving.Models
{
    public partial class Search
    {

        public string ID { get; set; } 
        public string TypeID { get; set; } 
        public string BranchID { get; set; } 
        public string _typestatus { get; set; } 


        public string receiveDateStart { get; set; }
        public string receiveDateEnd { get; set; } 
        public string critiriaVendor { get; set; }
        public string RunningID { get; set; }
        public string RequestNo { get; set; }

        public string SerialNo { get; set; }
        public string Location { get; set; }


         
        public string DepartmentID { get; set; }
        public string critiria { get; set; }
        public string status { get; set; }
 
        public string statusPage { get; set; }
        public string sorting { get; set; }
        public string currentSort { get; set; }
     
        public bool usestatus { get; set; }
        public bool PageOn { get; set; } = true;
        public int page { get; set; }
        public string strDate { get; set; }
        public string endStrDate { get; set; }
        public string ResignDateOutStr { get; set; }
        public decimal? NumberOfHour { get; set; } = 0;
        public DateTime? DateFromstr { get; set; }

        public DateTime? DateTostr { get; set; }

        public DateTime DateStartFrom { get; set; } = default(DateTime);
        public DateTime DateStartTo { get; set; } = default(DateTime);

        public List<string> DepartmentPermistions { get; set; }

        public int PageSetNumber
        {
            get
            {
                return 25;
            }
        }
        public string TypeIDName
        {
            get
            {
                //text-info text-green text-red text-blue text-warning
                switch (TypeID)
                {
                    case "001":
                        return "รับอะไหล่";

                    case "002":
                        return "รับบรรจุภัณฑ์"; 
                    default:
                        return "";
                }
            }

        }
    }

}
 
