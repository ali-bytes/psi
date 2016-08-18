using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class ResellersStore : CustomPage
    {

    readonly IRouterRepository _routerRepository = new RouterRepository();


    protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack) return;
        UpdateQuantity();
        PopulateResellers();
    }


    void PopulateResellers(){
        var users = DataLevelClass.GetUserReseller();
        DResellers.DataSource = users;
        DResellers.DataValueField = "ID";
        DResellers.DataTextField = "UserName";
        DResellers.DataBind();
        Helper.AddDefaultItem(DResellers);
    }


    void UpdateQuantity(){
        LAvailableQuantity.Text = _routerRepository.Quantity().ToString(CultureInfo.InvariantCulture);
    }


    protected void BSave_OnClick(object sender, EventArgs e){
        var reseller = Convert.ToInt32(DResellers.SelectedItem.Value);
        var quantity = Convert.ToInt32(TbQuantity.Text);
        var result = _routerRepository.TransferToReseller(reseller, quantity);
        switch(result){
            case RouterSaveState.Saved :
                LMessage.Text = Tokens.Saved;
                ClearInputs();
                break;
            case RouterSaveState.Problem :
                LMessage.Text = Tokens.NotAvailableQTN;
                break;
            default :
                LMessage.Text = Tokens.StoreprobMsg;
                break;
        }
    }


    void ClearInputs(){
        TbQuantity.Text = string.Empty;
        DResellers.SelectedIndex = -1;
    }
}
}