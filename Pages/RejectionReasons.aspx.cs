using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class RejectionReasons : CustomPage
    {
     
    readonly IRejectionReasonsRepository _reasonsRepository = new LRejectionReasonsRepository();


    protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack)
            return;
        MultiView1.SetActiveView(v_index);
        PopulateReasons();
        l_message.Text = string.Empty;
    }


    void PopulateReasons(){
        var reasons = _reasonsRepository.Reasons;
        gv_index.DataSource = reasons;
        gv_index.DataBind();
    }


    protected void b_new_Click(object sender, EventArgs e){
        MultiView1.SetActiveView(v_AddEdit);
    }


    protected void b_save_Click(object sender, EventArgs e){
        RejectionReason reason;
        if(hf_id.Value == string.Empty){
            reason = new RejectionReason{
                                            Reason = tb_reason.Text
                                        };
            _reasonsRepository.Save(reason);
        } else{
            reason = _reasonsRepository.Reasons.FirstOrDefault(o => o.Id == Convert.ToInt32(hf_id.Value));
            if(reason != null){
                reason.Reason = tb_reason.Text;
                _reasonsRepository.Save(reason);
            }
        }
        Clear();
        PopulateReasons();
        if(reason != null) l_message.Text = Tokens.Saved;
        hf_id.Value = string.Empty;
        MultiView1.SetActiveView(v_index);
    }


    void Clear(){
        tb_reason.Text = string.Empty;
    }


    protected void gv_index_DataBound(object sender, EventArgs e){
        Helper.GridViewNumbering(gv_index, "l_number");
        foreach(GridViewRow row in gv_index.Rows){
            var deleteButton = row.FindControl("gvb_delete") as LinkButton;
            var reason =
                _reasonsRepository.Reasons.FirstOrDefault(o => o.Id == Convert.ToInt32(deleteButton.CommandArgument));
            //todo:check for relations before delete
            if(deleteButton != null && reason != null && reason.Phones.Any()){
                deleteButton.Visible = false;
            }
        }
    }


    protected void gvb_edit_Click(object sender, EventArgs e){
        MultiView1.SetActiveView(v_AddEdit);
        var buttonSender = sender as LinkButton;
        if(buttonSender == null)
            return;
        var id = Convert.ToInt32(buttonSender.CommandArgument);
        var reason = _reasonsRepository.Reasons.FirstOrDefault(o => o.Id == id);
        if(reason == null) return;
        tb_reason.Text = reason.Reason;
        hf_id.Value = reason.Id.ToString(CultureInfo.InvariantCulture);
    }


    protected void gvb_delete_Click(object sender, EventArgs e){
        var reason =
            _reasonsRepository.Reasons.FirstOrDefault(o => o.Id == Convert.ToInt32((sender as LinkButton).CommandArgument));
        if(reason == null)
            return;

        l_message.Text = Tokens.Deleted;
        _reasonsRepository.Delete(reason);
        PopulateReasons();
    }
}
}