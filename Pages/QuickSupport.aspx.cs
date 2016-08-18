using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class QuickSupport : CustomPage
    {
      
    protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack) return;
        MultiView1.SetActiveView(v_index);
        PopulateGv();
        //l_message.Visible
    }


    void PopulateGv(){
        using (var context=new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var all = context.QuickSupports.ToList();
            gv_index.DataSource = all;
            gv_index.DataBind();
        }
    }


    protected void b_new_Click(object sender, EventArgs e){
        
        MultiView1.SetActiveView(v_AddEdit);
    }



    protected void b_save_Click(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            if (txtBody.Value == "")
            {
                Label4.Visible = true;


            }
            else
            {

                global::Db.QuickSupport support;
                if (hf_id.Value == string.Empty)
                {
                    support = new global::Db.QuickSupport
                    {
                        Url = txtUrl.Text,
                        Body = txtBody.Value
                    };
                    context.QuickSupports.InsertOnSubmit(support);
                    context.SubmitChanges();
                }
                else
                {
                    support = context.QuickSupports.FirstOrDefault(o => o.Id == Convert.ToInt32(hf_id.Value));
                    if (support != null)
                    {
                        support.Url = txtUrl.Text;
                        support.Body = txtBody.Value;
                    }
                    context.SubmitChanges();
                }
                l_message.Visible = true;
                Clear();
            }
        }
    }


    void Clear()
    {
        txtUrl.Text = txtBody.Value = string.Empty;
        PopulateGv();
        hf_id.Value = string.Empty;
        MultiView1.SetActiveView(v_index);
    }


    protected void gv_index_DataBound(object sender, EventArgs e){
        Helper.GridViewNumbering(gv_index, "l_number");
    }


    protected void gvb_edit_Click(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            MultiView1.SetActiveView(v_AddEdit);
            var buttonSender = sender as LinkButton;
            if (buttonSender == null) return;
            var id = Convert.ToInt32(buttonSender.CommandArgument);
            var suport = context.QuickSupports.FirstOrDefault(o => o.Id == id);
            if (suport == null) return;
            txtUrl.Text = suport.Url;
            txtBody.Value = suport.Body;
            hf_id.Value = suport.Id.ToString(CultureInfo.InvariantCulture);
        }
    }


    protected void gvb_delete_Click(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var button = sender as LinkButton;
            if(button != null){
                var quikId = Convert.ToInt32(button.CommandArgument);
                var support = context.QuickSupports.FirstOrDefault(o => o.Id == quikId);
                if(support == null) return;
                    context.QuickSupports.DeleteOnSubmit(support);
                    context.SubmitChanges();
            }
            PopulateGv();
        }
    }


    protected void ReturnToMainView(object sender, EventArgs e){
        Clear();
        l_message.Visible = false; 
    }
}
}