using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class ResellerDownloadInvoice : CustomPage
    {
        readonly IspDomian _domian;
   
    public ResellerDownloadInvoice()
    {
        _domian = new IspDomian(IspDataContext);
    }


    protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack) return;
        PrepareInputs();
        Search();
    }



    void PrepareInputs(){
        _domian.PopulateResellers(DdlReseller, true);
        var currentYear = DateTime.Now.Year;
        Helper.PopulateDrop(Helper.FillYears(currentYear - 5, currentYear+2).OrderBy(x => x), DdlYear);
        Helper.PopulateMonths(DdlMonth);
    }


    protected void SearchDemands(object sender, EventArgs e){
        Msg.InnerHtml = string.Empty;
        Search();
        HfSerched.Value="1";
    }


    void Search(){
        using(var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            if(!string.IsNullOrEmpty(DdlReseller.SelectedValue)){
                var attachment = _context.ResellerAttachments.Where(x => x.ResellerId == int.Parse(DdlReseller.SelectedItem.Value) && x.Date.Value.Year == int.Parse(DdlYear.SelectedItem.Value) && x.Date.Value.Month == int.Parse(DdlMonth.SelectedItem.Value)).Select(x => new{
                    x.ID,
                    x.User.UserName,
                    FileNameAfterUrl = string.Format("../Attachments/{0}", x.FileNameAfter),
                    FileNameBeforUrl = string.Format("../Attachments/{0}", x.FileNameBefor)
                    // Url = string.Format("../Attachments/{0}", x.FileName)
                }).ToList();
                GvResults.DataSource = attachment;
                GvResults.DataBind();
            }
        }
    }

    void ResetPage(){
        GvResults.DataSource = null;
        GvResults.DataBind();
        Helper.Reset(Forsearch);
        HfSerched.Value = string.Empty;
    }

    }
}