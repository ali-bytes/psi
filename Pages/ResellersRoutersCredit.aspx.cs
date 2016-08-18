using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class ResellersRoutersCredit : CustomPage
    {
      
    readonly IRouterRepository _routerRepository = new RouterRepository();


    protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack) return;
        var resellers = DataLevelClass.GetUserReseller();
        CalculateResellersStocks(resellers);
    }


    void CalculateResellersStocks(List<User> resellers){
        var stocks = _routerRepository.CalculateResellersStocks(resellers);
        GResellersStocks.DataSource = stocks;
        GResellersStocks.DataBind();
    }


    protected void GResellersStocks_OnDataBound(object sender, EventArgs e){
        Helper.GridViewNumbering(GResellersStocks, "LNo");
    }
}
}