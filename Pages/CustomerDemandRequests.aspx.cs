using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class CustomerDemandRequests : CustomPage
    {
         
            private readonly IRequestNotifiy _requestNotifiy;
            public  CustomerDemandRequests()
            {
                _requestNotifiy = new RequestNotifiy();
            }
            protected void Page_Load(object sender, EventArgs e)
            {
                if (!IsPostBack)
                {
                    using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                    {
                        PopulateRequest(context);
                    }
                }
            }

            void PopulateRequest(ISPDataContext context)
            {
                var groupIdQuery = context.Users.FirstOrDefault(usr => usr.ID == Convert.ToInt32(Session["User_ID"]));
                if (groupIdQuery == null)
                {
                    Response.Redirect("UnAuthorized.aspx");
                    return;
                }
                var dataLevelId = groupIdQuery.Group.DataLevel.ID;



                if (dataLevelId != 1)
                {

                    grd_Requests.Columns[19].Visible = false;
                    grd_Requests.Columns[20].Visible = false;
                    btnSelectAll.Visible = false;

                }



                var data = _requestNotifiy.GetNotificationRequestses(false, context);
                grd_Requests.DataSource = data.OrderBy(a => a.RequestNotifiId);
                grd_Requests.DataBind();
                PopulateProccess(data, context);
            }

            void PopulateProccess(IEnumerable<NotificationRequests> requestses, ISPDataContext context)
            {
                var req = new Dictionary<int, string>();
                foreach (var notificationRequestse in requestses)
                {
                    var process = notificationRequestse.ProccessName;
                    if (!req.ContainsValue(process)) req.Add(notificationRequestse.ProccessId, process);
                }
                ddlProccessType.DataSource = req;
                ddlProccessType.DataTextField = "Value";
                ddlProccessType.DataValueField = "Key";
                ddlProccessType.DataBind();
                Helper.AddDefaultItem(ddlProccessType);
            }

            protected void ddlProccess_SelectedIndexChanged(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    if (ddlProccessType.SelectedIndex <= 0) return;
                    var proccessId = Convert.ToInt32(ddlProccessType.SelectedItem.Value);
                    var data = _requestNotifiy.GetNotificationRequestses(false, proccessId, context);
                    grd_Requests.DataSource = data;
                    grd_Requests.DataBind();
                }
            }

            protected
                    void grd_Requests_RowDataBound(object sender, GridViewRowEventArgs e)
            {
                Helper.GridViewNumbering(grd_Requests, "lbl_No");
            }

            protected void ApproveRequest(object sender, EventArgs e)
            {
                var buttonId = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                if (buttonId != 0)
                {
                    using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                    {
                        var data = _requestNotifiy.UpdateNotification(buttonId, true, context);
                        hdfMsg.Value = data ? "1" : "0";
                        PopulateRequest(context);
                    }
                }
            }

            protected void BtnApproveSelectAll(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    foreach (GridViewRow row in grd_Requests.Rows)
                    {
                        var control = row.FindControl("SelectItem") as CheckBox;
                        if (control == null || !control.Checked) continue;
                        var dataKey = grd_Requests.DataKeys[row.RowIndex];
                        if (dataKey == null) continue;
                        var id = Convert.ToInt32(dataKey["RequestNotifiId"]);
                        if (id == 0) continue;
                        var data = _requestNotifiy.UpdateNotification(id, true, context);
                        hdfMsg.Value = data ? "1" : "0";
                    }

                    PopulateRequest(context);
                }
            }
        }
    }
 