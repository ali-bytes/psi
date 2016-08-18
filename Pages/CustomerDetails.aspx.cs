using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class CustomerDetails : CustomPage
    {
        
            readonly IspClientService _clientService = new IspClientService();

            /*public Pages_CustomerDetails(){
                _context = ISPDataContext;
            }*/


            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                if (!string.IsNullOrEmpty(Request.QueryString["WOID"]) && Request.QueryString["WOID"]!=null)
                {
                    var que = Request.QueryString["WOID"]; ;
                    var id2 = QueryStringSecurity.Decrypt(que);
                    var id = Convert.ToInt32(id2);

                    UserFile1.Woid = id;
                    Activate();
                    Routers(id);
                    PreviewCustomerDetails(id);
                    CheckCanEdit();
                    HlEdit.NavigateUrl = string.Format("~/Pages/EditCustomer.aspx?WOID={0}", QueryStringSecurity.Encrypt(id.ToString()));
                }
                else if(!string.IsNullOrEmpty(Request.QueryString["NID"]))
                {
                    var myid = Request.QueryString["NID"];


                    var id = Convert.ToInt32(myid);

                    UserFile1.Woid = id;
                    Activate();
                    Routers(id);
                    PreviewCustomerDetails(id);
                    CheckCanEdit();
                    HlEdit.NavigateUrl = string.Format("~/Pages/EditCustomer.aspx?WOID={0}", QueryStringSecurity.Encrypt(id.ToString()));

                }
                 else
                {
                    return;
                }
                
                
                //var querystring = QueryStringSecurity.Decrypt(Request.QueryString["c"]);
                //var id = Convert.ToInt32(querystring);


            
            }


        public void Routers(int id)
        {
            using (var dataContext = new Db.ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {

                var routerData = dataContext.RecieveRouters.Where(a => a.WorkOrderIdRecive == id).ToList().Select(a => new
                {
                    a.Id,
                    a.RouterSerial,
                    a.Store.StoreName,
                    CompanyUserName = a.User != null ? a.User.UserName : "-",
                    CustomerUserName = a.User1 != null ? a.User1.UserName : "-",
                    CompanyDate = Convert.ToDateTime(a.CompanyProcessDate).ToShortDateString(),
                    CustomerDate =
                        a.CustomerProcessDate != null
                            ? Convert.ToDateTime(a.CustomerProcessDate).ToShortDateString()
                            : "",
                    Attach = a.Attachments != null ? string.Format("../Attachments/{0}", a.Attachments) : "",
                    Attach2 = a.Attachments2 != null ? string.Format("../Attachments/{0}", a.Attachments2) : "",
                    a.IsRecieved
                });
                GVRouter.DataSource = routerData;

                GVRouter.DataBind();
            }
        }




            void CheckCanEdit()
            {
                using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    bool flag = false;
                    HlEdit.Visible = false;
                    if (Session["User_ID"] != null) return;
                    var userId = Convert.ToInt32(Session["User_ID"]);
                    var groupIdQuery = dataContext.Users.FirstOrDefault(us => us.ID == userId);
                    if (groupIdQuery == null || groupIdQuery.GroupID == null) return;
                    int groupId = groupIdQuery.GroupID.Value;
                    var groupPrivilegeQuery = dataContext.GroupPrivileges.Where(gp => gp.Group.ID == groupId);
                    /*from gp in dataContext.GroupPrivileges
                                          where gp.Group.ID == groupId
                                          select gp;*/
                    foreach (GroupPrivilege tmpgp in groupPrivilegeQuery)
                    {
                        if (tmpgp.privilege.Name == "EditCustomer.aspx")
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                        HlEdit.Visible = true;
                }
            }


            void PreviewCustomerDetails(int id)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var lastActivationdate = _clientService.GetLastActivationDate(id);
                    if (lastActivationdate != null)
                    {
                        l_activationdate.Text = lastActivationdate.Value.ToShortDateString();
                    }
                    var orders = context.WorkOrders.Where(wo => wo.ID == id);
                    var query = orders.Select(wo => new
                    {
                        wo.CustomerName,
                        wo.Governorate.GovernorateName,
                        wo.CustomerAddress,
                        wo.CustomerPhone,
                        wo.IpPackage.IpPackageName,
                        wo.CustomerMobile,
                        wo.CustomerEmail,
                        wo.ServiceProvider.SPName,
                        wo.ServicePackage.ServicePackageName,
                        reseller = wo.User.UserName,
                        wo.Branch.BranchName,
                        wo.VPI,
                        wo.VCI,
                        wo.UserName,
                        wo.Status.StatusName,
                        wo.Password,
                        ExtraGigas = wo.ExtraGiga.Name,
                        wo.PaymentType.PaymentTypeName,
                        wo.Notes,
                        Central = wo.Central.Name,
                        wo.Offer.Title,
                        wo.ID,
                        wo.PersonalId,
                        wo.RequestNumber,
                        wo.RouterSerial,
                        Installer = wo.User1.UserName,
                        InstallationTime = wo.InstallationTime == null ? "-" : wo.InstallationTime.Value.ToString(CultureInfo.InvariantCulture),
                        wo.WorkorderNumbers,
                        wo.WorkorderDate,
                        wo.PortNumber,
                        wo.BlockNumber,
                        wo.DslamNumbers,
                        wo.LineOwner,
                        wo.Prepaid,
                        wo.InstallationCost,
                        wo.ContractingCost,
                        wo.CustomerMobile2,
                        wo.OfferStart,
                        wo.CreationDate,
                        LifeTime = wo.Offer == null ? 0 : wo.Offer.LifeTime,
                    });

                    var status = context.WorkOrderStatus
                        .Where(wos => wos.WorkOrderID == id)
                        .Select(wos =>
                            new
                            {
                                wos.Status.StatusName,
                                wos.User.UserName,
                                wos.UpdateDate
                            }).ToList();

                    GridView1.DataSource = status;
                    GridView1.DataBind();

                    var requests = context.WorkOrderRequests.Where(wor => wor.WorkOrderID == id).Select(wor => new
                    {
                        wor.RequestDate,
                        wor.RequestStatus.RSName,
                        wor.Request.RequestName,
                        wor.User.UserName,
                        UserName2 = wor.User1.UserName,
                        wor.ServicePackage.ServicePackageName,
                        ServicePackageName2 = wor.ServicePackage1.ServicePackageName,
                        wor.RejectReason,
                        wor.Total
                    });


                    grd_Requests.DataSource = requests;
                    grd_Requests.DataBind();

                    var connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                    var command = new SqlCommand("PROC_GET_TICKETS", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.Add(new SqlParameter("@TICKETSTATUSID", SqlDbType.Int)).Value = DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@USERID", SqlDbType.Int)).Value = DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.DateTime)).Value = DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.DateTime)).Value = DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@WorkOrderID", SqlDbType.Int)).Value = id;
                    command.Parameters.Add(new SqlParameter("@BRANCHID", SqlDbType.Int)).Value = DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@BRANCHADMINID", SqlDbType.Int)).Value = DBNull.Value;
                    connection.Open();
                    var table = new DataTable();
                    table.Load(command.ExecuteReader());
                    connection.Close();

                    grd_Tickets.DataSource = table;
                    grd_Tickets.DataBind();

                    var woinfoQuery = context.WorkOrderHistories.Where(woh => woh.WOID == id).Select(woh => new
                    {
                        woh.Governorate.GovernorateName,
                        woh.ServiceProvider.SPName,
                        woh.UpdateDate,
                        Reseller = woh.User.UserName,
                        woh.User1.UserName,
                        woh.Branch.BranchName
                    });
                    grd_Info.DataSource = woinfoQuery;
                    grd_Info.DataBind();

                    var order = query.First();


                    if (order != null)
                    {
                        lbl_BranchName.Text = order.BranchName;
                        lbl_Client_UserName.Text = order.UserName;
                        lbl_CustomerAddress.Text = order.CustomerAddress;
                        lbl_CustomerEmail.Text = order.CustomerEmail;
                        lbl_CustomerMobile.Text = order.CustomerMobile;
                        lbl_CustomerName.Text = order.CustomerName;
                        lbl_CustomerPhone.Text = order.CustomerPhone;
                        lbl_ExtraGigas.Text = order.ExtraGigas;
                        lbl_GovernorateName.Text = order.GovernorateName;
                        lbl_IpPackageName.Text = order.IpPackageName;
                        lbl_Password.Text = order.Password;
                        lbl_ResellerName.Text = order.reseller;
                        lbl_ServicePackageName.Text = order.ServicePackageName;
                        lbl_SPName.Text = order.SPName;
                        lbl_StatusName.Text = order.StatusName;
                        lbl_VCI.Text = order.VCI;
                        lbl_VPI.Text = order.VPI;
                        lbl_PaymentType.Text = order.PaymentTypeName;
                        lbl_Notes.Text = order.Notes;
                        lbl_Offer.Text = order.Title;
                        lblWorkorderDate.Text = order.WorkorderDate.ToString();
                        lblWorkorderNumber.Text = order.WorkorderNumbers;
                        lblPortNumber.Text = order.PortNumber;
                        lblBlock.Text = order.BlockNumber;
                        lblDslam.Text = order.DslamNumbers;
                        lbl_Mobile2.Text = order.CustomerMobile2;
                        lbl_lineowner.Text = order.LineOwner;
                        lbl_NationaId.Text = order.PersonalId;
                        lblcentral.Text = order.Central;
                        lblprepaid.Text = order.Prepaid.ToString();
                        lblconstractingsoct.Text = order.ContractingCost.ToString();
                        lblinstalationcost.Text = order.InstallationCost.ToString();
                        lblRequestnumber.Text = order.RequestNumber;
                        lblrouterserial.Text = order.RouterSerial;
                        lblCreationDate.Text = order.CreationDate.ToString();
                        if (lastActivationdate != null)
                        {
                            lblOfferStart.InnerHtml = order.OfferStart == null ? "" : order.OfferStart.Value.ToShortDateString();
                            if (order.Title != null && order.OfferStart != null)
                            {
                                var offerLastDate = order.OfferStart.Value.AddMonths(Convert.ToInt32(order.LifeTime));
                                lblOfferEnd.InnerHtml = offerLastDate.ToShortDateString();
                            }
                        }
                        else
                        {
                            lblOfferStart.InnerHtml = lblOfferEnd.InnerHtml = string.Empty;
                        }
                        GVRequestDateHistory.DataSource = context.RequestDateHistories.Where(a => a.WorkOrderId == id).Select(a => new
                        {
                            a.WorkOrder.CustomerName,
                            a.User.UserName,
                            a.newRequestDate,
                            a.oldRequestDate,
                            a.ChangeDate
                        });
                        GVRequestDateHistory.DataBind();
                    }
                }
            }


            void Activate()
            {
                //GvNotes.DataBound += (o, e) => Helper.GridViewNumbering(GvNotes, "LNo");
                grd_Tickets.DataBound += (o, e) => PopulateTicketDays();
                GVRequestDateHistory.DataBound += (o, e) => Helper.GridViewNumbering(GVRequestDateHistory, "LNo");
            }


            void PopulateTicketDays()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    foreach (GridViewRow row in grd_Tickets.Rows)
                    {
                        var literal = row.FindControl("DaysCount") as Literal;
                        if (literal == null) return;
                        var tickectId = Convert.ToInt32(literal.Text);
                        var ticket = context.Tickets.FirstOrDefault(t => t.ID == tickectId);
                        if (ticket == null)
                        {
                            literal.Text = "";
                            return;
                        }
                        var comment = context.TicketComments.Where(x => x.TicketID == tickectId).OrderByDescending(x => x.CommentDate.Value).FirstOrDefault();
                        if (ticket.TicketDate != null && comment != null && comment.CommentDate != null)
                        {
                            literal.Text = string.Format("{0}", (comment.CommentDate.Value.Date - ticket.TicketDate.Value.Date).Days);
                        }
                        if (ticket.TicketDate != null && (comment == null || comment.CommentDate == null))
                        {
                            literal.Text = string.Format("{0}", (DateTime.Now.Date - ticket.TicketDate.Value.Date).Days);
                        }
                    }
                }
            }


            protected void GVRouter_DataBound(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {

                    foreach (GridViewRow row in GVRouter.Rows)
                    {
                        var id = GVRouter.DataKeys[row.RowIndex];
                        if (id != null)
                        {
                            var routerid = Convert.ToInt32(id.Value);
                            var rowdata = context.RecieveRouters.FirstOrDefault(a => a.Id == routerid);
                            var btn1 = row.FindControl("link") as HtmlAnchor;
                            var btn2 = row.FindControl("link2") as HtmlAnchor;
                            if (rowdata != null && rowdata.Attachments != null && btn1 != null)
                            {
                                btn1.Visible = true;
                            }
                            if (rowdata != null && rowdata.Attachments2 != null && btn2 != null)
                            {
                                btn2.Visible = true;
                            }
                        }
                    }
                }
            }
    
    
    }
    }
 