using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class RoutersnotReceived :CustomPage
    {
     
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var allstore = context.Stores;
                ddlStore.DataSource = allstore;
                ddlStore.DataValueField = "Id";
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataBind();
                Helper.AddDefaultItem(ddlStore);
            }
        }
    }

    protected void Search(object sender, EventArgs e)
    {
        using (var context=new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var storeId = Convert.ToInt32(ddlStore.SelectedItem.Value);
            var routersNotDeliverd = context.RecieveRouters.Where(a => a.StoreId == storeId && !Convert.ToBoolean(a.IsRecieved)).Select(s=>new
            {
                s.Id,
                s.Store.StoreName,
                s.User.UserName,
                s.CompanyProcessDate,
                s.RouterSerial,
                RouterType=s.RouterType ?? " ",
               
            });
            GVRouters.DataSource = routersNotDeliverd;
            GVRouters.DataBind();
        }
        
    }

    protected void GVRouters_DataBound(object sender, EventArgs e)
    {
        Helper.GridViewNumbering(GVRouters, "LNo");
    }

}
}