using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class BranchPaymentCreditReport : CustomPage
    {
        readonly BranchCreditRepository _creditRepository = new BranchCreditRepository();
        readonly IUserSaveRepository _userSave = new UserSaveRepository();
        void PopulateBranches()
        {
            var branches = DataLevelClass.GetUserBranches();
            Ddlbranch.DataSource = branches;
            Ddlbranch.DataTextField = "BranchName";
            Ddlbranch.DataValueField = "ID";
            Ddlbranch.DataBind();
            Helper.AddAllDefaultItem(Ddlbranch);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateBranches();
        }
        void UpdateHistor(int branchId)
        {
            var history = _creditRepository.BranchCredits(branchId);
            var hist2 = history.ToList().OrderByDescending(x => x.Id).Select(x => new
            {
                Amount = Helper.FixNumberFormat(x.Amount),
                Net = Helper.FixNumberFormat(x.Net),
                Branch = x.Branch.BranchName,
                User = x.User.UserName,
                Date = x.Time,//string.Format("{0:d}", x.Time),
                x.Notes,
                Type = x.Amount < 0 ? Tokens.Subtract : Tokens.Add,
                RecieptUrl = string.Format("BranchPaymentReciept.aspx?id={0}", x.Id)
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
                GvHistory.DataSource = hist2.Where(x => x.Date.Date >= from && x.Date.Date <= to).ToList();
                GvHistory.DataBind();
            }

           
            var firstOrDefault = history.OrderByDescending(z => z.Id).FirstOrDefault();
            if (firstOrDefault != null) lblLastCredit.Text = Helper.FixNumberFormat(firstOrDefault.Net);
        }
        //protected void Ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var id = string.IsNullOrEmpty(Ddlbranch.SelectedItem.Value) ? 0 : Convert.ToInt32(Ddlbranch.SelectedItem.Value);
        //    if (id == 0) return;
        //    UpdateHistor(id);
        //}
        protected void GvHistory_DataBound(object sender, EventArgs e)
        {
            Helper.GridViewNumbering(GvHistory, "no");
        }
        protected void Search(object sender, EventArgs e)
        {
            int branchId = Convert.ToInt32(Ddlbranch.SelectedItem.Value);
            int userId = Convert.ToInt32(Session["User_ID"]);
            UpdateHistor(branchId);

        }
    }
}