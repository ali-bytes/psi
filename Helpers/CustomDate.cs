using System;
using System.Configuration;
using System.Linq;
using Db;

namespace NewIspNL.Helpers{
    public static class CustomDate{
        public static DateTime AddHours(this DateTime time){
            //return time.AddHours(9);
            var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
   
            var firstOrDefault = context.Options.FirstOrDefault();
            if (firstOrDefault != null)
            {
                var hourDifference = Convert.ToDouble(firstOrDefault.TimeDifference);
                return time.AddHours(hourDifference);
            }
            return time;
        }
        
        public static string ToDateTime(this DateTime d){
            return string.Format("{0} - {1}", d.ToShortDateString(), d.ToShortTimeString());
        }
    }
}
