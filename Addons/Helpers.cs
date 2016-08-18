using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewIspNL.Addons
{
    /// <summary>
    ///   Summary description for Helper
    /// </summary>
    public static class Helpers{
        /// <summary>
        ///   Provides Gridview with numbering
        /// </summary>
        /// <param name="gridViewId"> </param>
        /// <param name="numberPlaceHolderLabelId"> </param>

        public static DateTime TransformDate(string date){
            var all = date.Split('/');
            var now = DateTime.Now;
            var time = new DateTime(Convert.ToInt32(all[2]), Convert.ToInt32(all[1]), Convert.ToInt32(all[0]), now.Hour, now.Minute,now.Second);
            return time;
        }


        public static void AddDefaultItem(DropDownList list, string text = "--اختر--"){
            var item = new ListItem(text, "");
            list.Items.Insert(0, item);
        }

        public static string FixNumberFormat(decimal amount){
            return string.Format("{0:####.##}", amount) == string.Empty ? "0" : string.Format("{0:####.##}", amount);
        }

    }
}
