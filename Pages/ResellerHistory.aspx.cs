using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class ResellerHistory : CustomPage
    {
    
    readonly IRouterRepository _routerRepository = new RouterRepository();


    protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack) return;
        PopulateResellers();
    }


    void PopulateResellers(){
        var users = DataLevelClass.GetUserReseller();
        DResellers.DataSource = users;
        DResellers.DataValueField = "ID";
        DResellers.DataTextField = "UserName";
        DResellers.DataBind();
        Helper.AddDefaultItem(DResellers, "--Choose--");
    }


    protected void BSearch_OnClick(object sender, EventArgs e){
        var start = Convert.ToDateTime(TbFrom.Text);
        var end = Convert.ToDateTime(TbTo.Text);
        var reportItems = _routerRepository.CalculateResellerHistory(start, end, Convert.ToInt32(DResellers.SelectedItem.Value), DResellers.SelectedItem.Text, 1);

        GItems.DataSource = reportItems;
        GItems.DataBind();

        var customerReportItems = _routerRepository.CalculateResellerHistory(start, end, Convert.ToInt32(DResellers.SelectedItem.Value), DResellers.SelectedItem.Text, 3);
        GItemsCustomers.DataSource = customerReportItems;
        GItemsCustomers.DataBind();
    }
}
}