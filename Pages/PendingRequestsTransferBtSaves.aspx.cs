using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;
using NewIspNL.Services;
using Resources;

namespace NewIspNL.Pages
{
    public partial class PendingRequestsTransferBtSaves : CustomPage
    {
        private readonly IUserSaveRepository _userSave;

        public PendingRequestsTransferBtSaves()
        {
            _userSave = new UserSaveRepository();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateRequests();
        }
        protected void grd_Requests_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Helper.GridViewNumbering(grd_Requests, "lbl_No");
        }

        private void PopulateRequests()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var savesIdsList = _userSave.SavesIdsOfUser(Convert.ToInt32(Session["User_ID"]), context);

                var requests = context.TransferBetweenSavesRequests.Where(x => x.Status == null && savesIdsList.Contains(x.ToSave)).Select(s => new
                {
                    id=s.Id,
                   FromSave= s.Save.SaveName,
                   ToSave= s.Save1.SaveName,
                    s.RequestDate,
                    RequestMaker = s.User1.UserName,
                    s.Amount,
                    s.RequestMakerNote
                }).ToList();
                grd_Requests.DataSource = requests;
                grd_Requests.DataBind();
            }
        }
        protected void Rejected_Click(object sender, EventArgs e)
        {
            var id = RejectedRequestId.Value;
            if (id == null) return;
            var requestId = Convert.ToInt32(id);
            RejectRequest(requestId);
        }
        void RejectRequest(int requestId)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var req = context.TransferBetweenSavesRequests.FirstOrDefault(x => x.Id == requestId);
                if (req == null) return;
                req.ConfirmerNote = TbRejectReason.Text;
                req.ConfirmerUserId = Convert.ToInt32(Session["User_ID"]);
                req.ApprovedDate = DateTime.Now.AddHours();
                req.Status = false;
                context.SubmitChanges();
                
                lbl_ProcessResult.Text = Tokens.RequestRejected;
                lbl_ProcessResult.ForeColor = Color.Green;
                PopulateRequests();

            }
        }
        protected void Approved_Click(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var id = ApproveId.Value;
                if (id == null) return;
                var requId = Convert.ToInt32(id);
                var request = context.TransferBetweenSavesRequests.FirstOrDefault(x => x.Id == requId);
                if (request == null) return;
                int fromid = request.FromSave;
                var toid = request.ToSave;
                var amount = Convert.ToDouble(request.Amount);
                var userId = Convert.ToInt32(Session["User_ID"]);
                var notesFrom = " تحويل رصيد الى خزنة " + request.Save1.SaveName;
                var notesTo = "تحويل رصيد من خزنة " + request.Save.SaveName;
                var fromtotal = Convert.ToDouble(context.Saves.Where(z => z.Id == fromid).Select(z => z.Total).FirstOrDefault());
                if (amount > fromtotal)
                {
                    lbl_ProcessResult.Text = Tokens.Error + " " + "رصيد الخزينة لا يكفي ";
                }
                else
                {
                    _userSave.UpdateSave(userId, request.FromSave, (amount * -1), notesFrom, tbApproveNote.Text, context);
                    _userSave.UpdateSave(userId, toid, amount, notesTo, tbApproveNote.Text, context);

                    request.ConfirmerUserId = userId;
                    request.ApprovedDate = DateTime.Now.AddHours();
                    request.ConfirmerNote = tbApproveNote.Text;
                    request.Status = true;
                    context.SubmitChanges();
                    lbl_ProcessResult.Text = Tokens.Approved;
                    lbl_ProcessResult.ForeColor = Color.Green;
                   
                }
                PopulateRequests();
            }
        }
    }
}