using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using NewIspNL.Services;
using Resources;

namespace NewIspNL.Pages
{
    public partial class ManageOfferRequest : CustomPage
    {
      
    protected void Page_Load(object sender, EventArgs e)
    {
        if(IsPostBack)return;
        PopulateRequests();
    }
    protected void grd_Requests_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Helper.GridViewNumbering(grd_Requests, "lbl_No");
    }
   

    void PopulateRequests()
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var requests = DataLevelClass.GetUserNonCofirmedWoRequests(12);
            grd_Requests.DataSource = requests;
            grd_Requests.DataBind();


            var columns = grd_Requests.Columns;
            GridHelper.HideAllColumns(columns);
            var columnNames = new List<string>
                    {
                        "#",
                        Tokens.Name,
                        Tokens.Phone,
                        Tokens.Governrate,
                        Tokens.Central,
                        Tokens.CurrentSpeed,
                        Tokens.Status,
                        Tokens.Provider,
                        Tokens.Reseller,
                        Tokens.Branch,
                        Tokens.SenderName,
                        Tokens.Date,
                        Tokens.CurrentOffer,
                        Tokens.NewOffer,
                         Tokens.Activation_Date,

                        Tokens.Request_Date,
                       
                    };
            var datalevel = context.Users.First(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"]));
            if (datalevel.Group.DataLevelID == 1)
            {
                columnNames.AddRange(new List<string>
                            {
                                Tokens.Approve,
                                Tokens.Reject
                            });

                GridHelper.ShowExactColumns(columns, columnNames);
            }
            else
            {
                GridHelper.ShowExactColumns(columns, columnNames);
            }
        }

    }
    void RejectRequest(int orderRequestId)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var workOrderRequest = context.WorkOrderRequests.FirstOrDefault(wor => wor.ID == orderRequestId);
            if (workOrderRequest == null) return;
            workOrderRequest.RSID = 2;
            workOrderRequest.RejectReason = TbRejectReason.Text;
            workOrderRequest.ConfirmerID = Convert.ToInt32(Session["User_ID"]);
            workOrderRequest.ProcessDate = DateTime.Now.AddHours();
            context.SubmitChanges();
            var currentWorkOrder = context.WorkOrders.FirstOrDefault(wo => wo.ID == workOrderRequest.WorkOrderID);
            if (currentWorkOrder != null)
            {
                var option = OptionsService.GetOptions(context, true);
                if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                {
                    CenterMessage.SendRequestReject(currentWorkOrder, TbRejectReason.Text,
                        workOrderRequest.Request.RequestName, Convert.ToInt32(Session["User_ID"]));
                }
            }

            context.SubmitChanges();
            lbl_ProcessResult.Text = Tokens.RequestRejected;
            lbl_ProcessResult.ForeColor = Color.Green;
            PopulateRequests();

        }
    }

    protected void Rejected_Click(object sender, EventArgs e)
    {
        var id = RejectedRequestId.Value;
        if(id==null)return;
        var requestId = Convert.ToInt32(id);
        RejectRequest(requestId);
    }

    protected void Approved_Click(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var id = ApproveId.Value;
            if(id==null)return;
            var requId = Convert.ToInt32(id);
            var request = context.WorkOrderRequests.FirstOrDefault(wor => wor.ID == requId);
            if (request == null) return;
            request.RSID = 1;
            int confirmerId = Convert.ToInt32(Session["User_ID"]);
            request.ConfirmerID = confirmerId;
            request.ProcessDate = Convert.ToDateTime(TbofferDate.Text);
            var order = request.WorkOrder;
            order.OfferId = request.NewOfferId;

            var option = OptionsService.GetOptions(context, true);
            if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
            {
                CenterMessage.SendRequestApproval(order, Tokens.ChangeOfferRequest, confirmerId);
            }
            context.SubmitChanges();
            lbl_ProcessResult.Text = Tokens.Approved;
            lbl_ProcessResult.ForeColor = Color.Green;
            PopulateRequests();
        }
    }
}
}