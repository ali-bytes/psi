using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class ResellerVoiceBS :CustomPage
    {
      

    readonly IspDomian _domian;


    public ResellerVoiceBS(){
      
        _domian = new IspDomian(IspDataContext);
    }


    protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack) return;
        _domian.PopulateResellers(ddl_Reseller);
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var userId = Convert.ToInt32(Session["User_ID"]);
            var user = context.Users.FirstOrDefault(a => a.ID == userId);
            if (user != null && user.GroupID != 6) Helper.AddDropDownItem(ddl_Reseller,1, Tokens.All);
        }
    }


    protected void btn_search_Click(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var user=new User();
            var resellerId = Helper.GetDropValue(ddl_Reseller);
            if (resellerId > 0 ) user = context.Users.FirstOrDefault(us => us.ID == resellerId);
            else if(ddl_Reseller.SelectedIndex==1) user = null;
           
            tb_SearchResult.Visible = true;
           
            var data = new Myobject();
            if(user != null) data.Reseller = user.ID;
            if(!string.IsNullOrEmpty(TbStartAt.Text)) data.StartAt = Convert.ToDateTime(TbStartAt.Text);
            if(!string.IsNullOrEmpty(TbTo.Text)) data.EndAt = Convert.ToDateTime(TbTo.Text);
            Bind_grd_Transactions(data);
        }
    }


    void Bind_grd_Transactions(Myobject conditions)
    {
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var query = context.RechargeClientRequests.OrderByDescending(x => x.Time)
                .Select(x => new{
                    x.ResellerId,
                    x.User.UserName,
                    x.ClientName,
                    x.ClientTelephone,
                    x.VoiceCompany.CompanyName,
                    x.Amount,
                    x.Time,
                    x.Notes,
                    x.ID,
                    x.RejectReason,
                    x.IsApproved
                }).ToList();
            if(conditions.Reseller != null){
                query = query.Where(s => s.ResellerId == conditions.Reseller).ToList();
            }
            if(conditions.StartAt != null){
                query = query.Where(s => s.Time.Value.Date >= conditions.StartAt).ToList();
            }
            if(conditions.EndAt != null){
                query = query.Where(s => s.Time.Value.Date <= conditions.EndAt).ToList();
            }
           
            grd_Transactions.DataSource = query;
            grd_Transactions.DataBind();
            if(query.Count != 0)
                BtnExport.Visible = true;
        }
    }


    protected void BtnExport_Click(object sender, EventArgs e){
       
        const string attachment = "attachment; filename=ResellerRequests.xls";
        Response.ClearContent();
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/ms-excel";

        var sw = new StringWriter();
        var htw = new HtmlTextWriter(sw);
        grd_Transactions.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
    }


    protected void grd_DataBound(object sender, EventArgs e){
        
        foreach(GridViewRow row in grd_Transactions.Rows){
            var label = row.FindControl("lblStatus") as Label;
            if(label != null){
                if(label.Text == "True"){
                    label.Text = Tokens.Approved;
                    label.CssClass = "label label-success arrowed";
                   
                } else if(label.Text == "False"){
                    label.Text = Tokens.Rejected;
                    label.CssClass = "label label-danger arrowed-in";
                    
                } else{
                    label.Text = Tokens.PendingRequest;
                    label.CssClass = "label label-warning";
                   
                }
            }
        }
    }


    public class Myobject
    {
        public DateTime? StartAt { get; set; }

        public DateTime? EndAt { get; set; }

        public int? Reseller { get; set; }
    }
}
}