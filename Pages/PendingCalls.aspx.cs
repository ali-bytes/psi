using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class PendingCalls : CustomPage
    {
    
    const int StateId = 3;

    readonly IspEntries _lIspEntries = new IspEntries();

    readonly IPhonesServices _phonesServices = new PhonesServices();


    protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack) return;
        PopulatePhones(StateId);
        PopulateRejectReasons();
    }


    void PopulatePhones(int stateId){
        var phones = _phonesServices.GetPhonesByStateId(stateId);
       
        gv_items.DataSource = phones.Select(p => new
        {
                                                        p.Id,
                                                        p.Phone1,
                                                        p.Name,
                                                        p.Governate,
                                                        p.CallState.State,
                                                        p.Offer1,
                                                        p.Offer2,
                                                        Employee = p.User.UserName,
                                                        p.Comment
                                                    });
        gv_items.DataBind();
    }


    protected void gv_items_DataBound(object sender, EventArgs e){
        Helper.GridViewNumbering(gv_items, "l_Number");
    }


    protected void b_approve_Click(object sender, EventArgs e){
        l_message.Text = string.Empty;
        var approveButton = sender as LinkButton;
        if(approveButton != null){
            var id = Convert.ToInt32(approveButton.CommandArgument);
            _phonesServices.UpdateState(id, 6);
            Response.Redirect("~/Pages/AddNewCustomer.aspx");
        }
    }


    void PopulateRejectReasons(){
        var reasons = _lIspEntries.RejectionReasons();
        ddl_reject2.DataSource = reasons;
        ddl_reject2.DataTextField = "Reason";
        ddl_reject2.DataValueField = "Id";
        ddl_reject2.DataBind();
        Helper.AddDefaultItem(ddl_reject2);
    }


    protected void b_saveReject_Click(object sender, EventArgs e){
        l_message.Text = Tokens.Rejected;
        _phonesServices.UpdateState(Convert.ToInt32(hf_rejectionId.Value), 4);
        PopulatePhones(StateId);
        ddl_reject2.SelectedIndex = -1;
    }
}
}