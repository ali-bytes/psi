using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class StoreInventory : CustomPage
    {
       

    protected void Page_Load(object sender, EventArgs e)
    {
        if(IsPostBack)return;
        Button1.Visible = false;
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            PopulateStore(context);
        }
    }
    void PopulateStore(ISPDataContext db)
    {
        var stores = db.Stores.ToList();
        ddlStore.DataSource = stores;
        ddlStore.DataBind();
        Helper.AddDefaultItem(ddlStore);
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        const string attachment = "attachment; filename=StorInventory.xls";
        Response.ClearContent();
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/ms-excel";
        Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");

        var sw = new StringWriter();
        var htw = new HtmlTextWriter(sw);
        GvSearch.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
    }
    protected void Search(object sender, EventArgs e)
    {
        var storeId = Convert.ToInt32(ddlStore.SelectedItem.Value);
        if(storeId==0)return;
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var storeData = context.StoreTransactions.Where(a => a.StoreId == storeId).Select(a=>new
            {
                a.Item.ItemName,
                a.ItemId,
                a.Store.StoreName,
                a.StoreId,
            });
            GvSearch.DataSource = storeData.Distinct().ToList();
            GvSearch.DataBind();
            Button1.Visible = true;
        }

    }
    protected void GvSearch_DataBound(object sender, EventArgs e)
    {
        Helper.GridViewNumbering(GvSearch, "LNo");
        foreach (GridViewRow item in GvSearch.Rows)
        {
            var lblstoreid = item.FindControl("lblStore") as Label;
            var lblitemId = item.FindControl("lblItem") as Label;
            var lblQuantity = item.FindControl("lblQuantity") as Label;
            if (lblstoreid != null && lblitemId != null && lblQuantity!=null)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var storeId = Convert.ToInt32(lblstoreid.ToolTip);
                    var itemId = Convert.ToInt32(lblitemId.ToolTip);
                    var itemdata =
                        context.StoreTransactions.Where(a => a.StoreId == storeId && a.ItemId == itemId).ToList();
                    lblQuantity.Text =Helper.FixNumberFormat(itemdata.Sum(a => a.Quantity)); //.ToString();
                }
            }
        }
    }
}
}