using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class ResellerPaymenyCreditReport : CustomPage
    {
        readonly IResellerCreditRepository _creditRepository = new ResellerCreditRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateReseller();
        }

        protected void Search(object sender, EventArgs e)
        {
            int resellerId = Convert.ToInt32(DdlReseller.SelectedItem.Value);
            int userId = Convert.ToInt32(Session["User_ID"]);
            UpdateHistor(resellerId);

        }

        protected void DdlReseller_SelectedIndexChanged(object sender, EventArgs e)
        {
            var id = string.IsNullOrEmpty(DdlReseller.SelectedItem.Value) ? 0 : Convert.ToInt32(DdlReseller.SelectedItem.Value);
            if (id == 0)
            {
                return;
            }
            UpdateHistor(id);
        }

        void PopulateReseller()
        {
            if (Session["User_ID"] == null)
            {
                Response.Redirect("default.aspx");
                return;
            }
            var resellers = _creditRepository.GetResellers(Convert.ToInt32(Session["User_ID"]));//GetResellersUponUserGroupWithCredit(Convert.ToInt32(Session["User_ID"]));
            DdlReseller.DataSource = resellers;
            DdlReseller.DataTextField = "UserName";
            DdlReseller.DataValueField = "Id";
            DdlReseller.DataBind();
            Helper.AddAllDefaultItem(DdlReseller);
        }
        void UpdateHistor(int resellerId)
        {
            lblLastCredit.Text = "";
            var history = _creditRepository.ResellerCredits(resellerId);
            var hist2 = history.ToList().OrderByDescending(x => x.Id).Select(x => new
            {
                Amount = Helper.FixNumberFormat(x.Amount),
                Net = Helper.FixNumberFormat(x.Net),
                Reseller = x.User.UserName,
                User = x.User1.UserName,
                Date = x.Time,
                x.Notes,
                //todo: link beetween reseller credit and recharge request
                // todo: reseller name in bote in bs
                link = x.RechargeRequestId != null ? string.Format("../Attachments/{0}", x.RechargeRequest.RecieptImage) : Tokens.NoFiles,
                Type = x.Amount < 0 ? Tokens.Subtract : Tokens.Add,
                RecieptUrl = string.Format("ResellerPaymentReciept.aspx?id={0}", x.Id),
                ifAttchment = x.RechargeRequestId != null
            });

            if (string.IsNullOrEmpty(TbFrom.Text) || string.IsNullOrEmpty(TbTo.Text))
            {
                GvHistory.DataSource = hist2;
                GvHistory.DataBind();
            }
            else
            {
                var from = Convert.ToDateTime(TbFrom.Text);
                var to = Convert.ToDateTime(TbTo.Text);
                GvHistory.DataSource = hist2.Where(x=>x.Date.Date >= from && x.Date.Date <= to).ToList();
                GvHistory.DataBind();
            }
          
            var firstOrDefault = history.OrderByDescending(z => z.Id).FirstOrDefault();
            if (firstOrDefault != null) lblLastCredit.Text = Helper.FixNumberFormat(firstOrDefault.Net);
        }
        protected void GvHistory_DataBound(object sender, EventArgs e)
        {
            Helper.GridViewNumbering(GvHistory, "no");
        }
        protected void Export_OnClick(object sender, EventArgs e)
        {

            string attachment = string.Format("attachment; filename=ResellerPaymentCredit.xls");
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";

            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);
            GvHistory.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();

        }
    }
}