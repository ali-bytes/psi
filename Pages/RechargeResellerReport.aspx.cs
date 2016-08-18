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
using Resources;

namespace NewIspNL.Pages
{
    public partial class RechargeResellerReport : CustomPage
    {
   
    private readonly IspDomian _ispDomian;

    public RechargeResellerReport()
    {
        var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        _ispDomian=new IspDomian(context);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
            CheckOnReseller();
        }
    }

    private void CheckOnReseller()
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var userId = Convert.ToInt32(HttpContext.Current.Session["User_ID"]);
            
            var first = context.Users.Where(usr => usr.ID == userId).
                Select(usr => usr.Group.DataLevelID).First();
            var resellers = DataLevelClass.GetUserReseller();
            var resellerHasRequest = new List<User>();
            foreach (var item in resellers)
            {
                var check = context.RechargeRequests.FirstOrDefault(ch => ch.ResellerId == item.ID);
                if (check != null)
                {
                    resellerHasRequest.Add(item);
                }
            }
            ddlReseller.DataSource = resellerHasRequest;
            ddlReseller.DataTextField = "UserName";
            ddlReseller.DataValueField = "ID";
            ddlReseller.DataBind();
            if(first!=null && first.Value==1) Helper.AddDefaultItem(ddlReseller, "All");
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            if (ddlReseller.SelectedIndex == 0)
            {
                Getdata(null, context);
            }
            if (ddlReseller.SelectedIndex != 0)
            {
                var resellertId = Convert.ToInt32(ddlReseller.SelectedValue);
                Getdata(resellertId, context);
            }
        }
    }

    private void Getdata(int? id, ISPDataContext context)
    {
        if (id != null)
        {
            var requests = context.RechargeRequests.Where(x => x.ResellerId == id && x.Time.Value.Date <= Convert.ToDateTime(tb_to.Text)
                             && x.Time.Value.Date >= Convert.ToDateTime(tb_from.Text)).Select(x => new
                             {
                                 x.ID,
                                 x.BoxId,
                                 x.Box.BoxName,
                                 x.ResellerId,
                                 x.User.UserName,
                                 x.DepositorName,
                                 x.Amount,
                                 x.Time,
                                 Url = string.Format("../Attachments/{0}", x.RecieptImage),
                                 x.CreditORVoice,
                                 x.IsApproved,
                                 x.RejectReason
                             }).ToList();
            grd.DataSource = requests;
            grd.DataBind();
            
        }
        else
        {
            var requests = context.RechargeRequests.Where(x => x.Time.Value.Date <= Convert.ToDateTime(tb_to.Text)
                             && x.Time.Value.Date >= Convert.ToDateTime(tb_from.Text)).Select(x => new
                             {
                                 x.ID,
                                 x.BoxId,
                                 x.Box.BoxName,
                                 x.ResellerId,
                                 x.User.UserName,
                                 x.DepositorName,
                                 x.Amount,
                                 x.Time,
                                 Url = string.Format("../Attachments/{0}", x.RecieptImage),
                                 x.CreditORVoice,
                                 x.IsApproved,
                                 x.RejectReason
                             }).ToList(); ;
            grd.DataSource = requests;
            grd.DataBind();
           
        }

    }
    protected void grd_DataBound(object sender, EventArgs e)
    {
        Helper.GridViewNumbering(grd, "l_Number");
        var rows = grd.Rows;
        foreach (GridViewRow item in rows)
        {
            var lbl = item.FindControl("lbldirection") as Label;
            if (lbl != null)
            {
                switch (lbl.Text)
                {
                    case "0":
                        lbl.Text = Tokens.ResellerVoiceCredit;
                        break;
                    case "1":
                        lbl.Text = Tokens.ResellerPaymentCredit;
                        break;
                    case "2":
                        lbl.Text = Tokens.AddToResellerBalanceSheet;
                        break;
                    default:
                        lbl.Text = @"-";
                        break;
                }
            }
            var lblStatus = item.FindControl("lblApproved") as Label;
            if (lblStatus != null)
            {
                switch (lblStatus.Text)
                {
                    case "True":
                        lblStatus.Text = Tokens.Approve;
                        lblStatus.CssClass = "label label-success arrowed";
                        break;
                    case "False":
                        lblStatus.Text = Tokens.RequestRejected;
                        lblStatus.CssClass = "label label-danger arrowed-in";
                        break;
                    default:
                        lblStatus.Text = Tokens.PendingRequest;
                        lblStatus.CssClass = "label label-warning";
                        break;
                }
            }
        }
    }
}
}