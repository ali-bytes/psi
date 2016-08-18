using System;
using System.Configuration;
using System.Linq;
using Db;

namespace NewIspNL.Helpers{
    public static class TimeExtension{
        
        public static DateTime Add9Hours(this DateTime dateTime){
            var context =new  ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
            var firstOrDefault = context.Options.FirstOrDefault();
            if (firstOrDefault == null) return dateTime;
            var hourDifference =Convert.ToDouble(firstOrDefault.TimeDifference);
            return dateTime.AddHours(hourDifference);
            //return dateTime.AddHours(hourDifference);
        }


        
    }
}
