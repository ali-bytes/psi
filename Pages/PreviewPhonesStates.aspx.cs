using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class PreviewPhonesStates : CustomPage
    {
      
    readonly IPhonesServices _phonesServices = new PhonesServices();

    readonly IPhoneStatesRepository _statesRepository = new LPhoneStatesRepository();


    protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack) return;
        PopulatePhoneStates();
    }


    void PopulatePhoneStates(){
        var states = _statesRepository.RejectedDataRejectedContractStates();
        ddl_states.DataSource = states;
        ddl_states.DataTextField = "State";
        ddl_states.DataValueField = "Id";
        ddl_states.DataBind();
    }


    void PopulatePhones(int stateId){
        var phones = _phonesServices.GetPhonesByStateId(stateId);

        gv_items.DataSource = phones
            .Select(p =>
                        new{
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


    protected void b_preview_Click(object sender, EventArgs e){
        var selectedStateId = Convert.ToInt32(ddl_states.SelectedItem.Value);
        PopulatePhones(selectedStateId);
    }


    protected void gv_items_DataBound(object sender, EventArgs e){
        Helper.GridViewNumbering(gv_items, "l_Number");
    }
}
}