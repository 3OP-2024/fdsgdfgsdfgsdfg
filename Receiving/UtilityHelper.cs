using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Receiving
{
    public static class UtilityHelper
    {
        public static int ProgramId { get { return 6003; } }
        public static DateTime stringToDateTime(this string stringDateTime)
        {

            try
            {
                var s = stringDateTime.Split('/');

                if (s.Length != 3)
                    return default(DateTime);

                return new DateTime(Convert.ToInt32(s[2]), Convert.ToInt32(s[1]), Convert.ToInt32(s[0]));


            }
            catch
            {
                return default(DateTime);
            }

        }
        public static string ToThaiMonthYear(this DateTime d)
        {
            System.Globalization.CultureInfo _cultureTHInfo = new System.Globalization.CultureInfo("th-TH");

            DateTime dateThai = Convert.ToDateTime(d, _cultureTHInfo);

            return dateThai.ToString("MMMM yyyy", _cultureTHInfo);
        }
    }
}
