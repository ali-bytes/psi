using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class SearchAboutBranchInvoice : CustomPage
    {
    


    public SearchAboutBranchInvoice(){
      
    }


    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void GvHistory_DataBound(object sender, EventArgs e)
    {
        Helper.GridViewNumbering(GvHistory, "no");
        var col = GvHistory.Columns;
        foreach (GridViewRow row in GvHistory.Rows)
        {
            var label = row.FindControl("lblStatus") as Label;
            if (label != null)
            {
                if (label.Text == "True")
                {
                    label.Text = Tokens.Approved;
                    label.CssClass = "label label-success arrowed";
                    col[10].Visible = false;
                }
                else if (label.Text == "False")
                {
                    label.Text = Tokens.Rejected;
                    label.CssClass = "label label-danger arrowed-in";
                    col[10].Visible = true;
                }
                else
                {
                    label.Text = Tokens.PendingRequest;
                    label.CssClass = "label label-warning";
                    col[10].Visible = false;
                }
            }
        }
    }


    void SearchResulte(int requestid){
        using(var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var result = _context.RechargeBranchRequests.Where(s => s.Id == requestid).OrderByDescending(s => s.Time).Select(s => new{
                s.Id,
                s.ClientName,
                s.ClientTelephone,
                s.Amount,
                s.Notes,
                s.VoiceCompany.CompanyName,
                s.Branch.BranchName,
                s.IsApproved,
                s.Time,
                s.RejectReason,
                Type = s.Amount < 0 ? Tokens.Subtract : Tokens.Add,
            }).ToList();
            GvHistory.DataSource = result;
            GvHistory.DataBind();

        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        SearchResulte(int.Parse(txtInvoiceNumber.Text));
    }
}
}