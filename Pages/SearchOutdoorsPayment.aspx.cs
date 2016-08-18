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

namespace NewIspNL.Pages
{
    public partial class SearchOutdoorsPayment : CustomPage
    {
    readonly IspDomian _domian;
    public SearchOutdoorsPayment(){
      
        _domian = new IspDomian(IspDataContext);
    }
    protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack) return;
        PopulateCompanies();
    }
    void PopulateCompanies()
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var query = context.VoiceCompanies.Select(c => new
            {
              c.CompanyName,
              c.Id
            });
            ddl_Company.DataSource = query;
            ddl_Company.DataBind();
            Helper.AddDefaultItem(ddl_Company);
        }
    }

    protected void btn_search_Click(object sender, EventArgs e)
    {
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
          
            var payment = context.CustomerPayments.FirstOrDefault();

            tb_SearchResult.Visible = true;

            var data = new SearchParamter();
            if(payment != null) 
            if(!string.IsNullOrEmpty(TbStartAt.Text)) data.StartAt = Convert.ToDateTime(TbStartAt.Text);
            if(!string.IsNullOrEmpty(TbTo.Text)) data.EndAt = Convert.ToDateTime(TbTo.Text);
            if (!string.IsNullOrEmpty(tbCust.Text)) data.CustomerName = tbCust.Text;
            if (ddl_Company.SelectedIndex > 0) data.VoiceCompanyId = ddl_Company.SelectedValue;

            Bind_grd_Transactions(data);
        }
    }

    void Bind_grd_Transactions(SearchParamter conditions){
        using(var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var query = dataContext.CustomerPayments.OrderByDescending(x => x.Time)
                .Select(x => new{
                    x.User.UserName,
                    x.Notes,
                    x.Box.BoxName,
                    x.CustomerName,
                    x.CustomerTelephone,
                    x.InvoiceAmount,
                    x.BoxAmount,
                    x.ID,
                    x.VoiceCompany.CompanyName,
                    x.BoxId,
                    x.Time,
                    x.VoiceCompanyId
                }).ToList();
            if(conditions.StartAt != null){
                query = query.Where(s => s.Time >= conditions.StartAt).ToList();
            }
            if(conditions.EndAt != null){
                query = query.Where(s => s.Time.Date <= conditions.EndAt).ToList();
            }
            if (conditions.VoiceCompanyId != null)
            {
                query = query.Where(s => s.VoiceCompanyId == Convert.ToInt32(conditions.VoiceCompanyId)).ToList();
            }
            if (conditions.CustomerName != null)
            {
                query = query.Where(s => s.CustomerName.Contains(conditions.CustomerName)).ToList();
            }
            grd_Transactions.DataSource = query;
            grd_Transactions.DataBind();
          
            if(query.Count != 0)
                BtnExport.Visible = true;

        }
    }


    protected void BtnExport_Click(object sender, EventArgs e){
        //_branchPrintExcel = true;
        const string attachment = "attachment; filename=Payment.xls";
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
       
    }


    public class SearchParamter
    {
        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }
        public string VoiceCompanyId { get; set; }
        public string CustomerName { get; set; }

       
    }
}
}