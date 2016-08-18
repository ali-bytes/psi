using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class TestOnlineUsers : CustomPage
    {
    
    protected void Page_Load(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {

           GetActive(context);

        }
        
    }

    protected void GvUserdataBounded(object sender, EventArgs e)
    {
        Helper.GridViewNumbering(GvUser, "LNo");
    }

    void GetActive(ISPDataContext context)
    {
        //get all Active User 
        var userList = new List<User>();
        object obj = typeof(HttpRuntime).GetProperty("CacheInternal", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, null);
        var obj2 = (object[])obj.GetType().GetField("_caches", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj);
        for (int i = 0; i < obj2.Length; i++)
        {
            var c2 = (Hashtable)obj2[i].GetType().GetField("_entries", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj2[i]);
            foreach (DictionaryEntry entry in c2)
            {
                object o1 = entry.Value.GetType().GetProperty("Value", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(entry.Value, null);
                if (o1.GetType().ToString() == "System.Web.SessionState.InProcSessionState")
                {

                    var sess = (SessionStateItemCollection)o1.GetType().GetField("_sessionItems", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(o1);
                    if (sess != null)
                    {
                        if (sess["User_ID"] != null)
                        {
                            var userId = Convert.ToInt32(sess["User_ID"]);
                            //lbl.Text +=  + @" is Active.<br>";
                            var usersactivit = context.Users.FirstOrDefault(a => a.ID == userId);
                            userList.Add(usersactivit);

                        }

                    }

                }

            }

        }
        GvUser.DataSource = userList;
        GvUser.DataBind();
        lblusersCount.Text=userList.Count>0?userList.Count.ToString():"0";

    }

    
}
}