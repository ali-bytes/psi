using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;
using NewIspNL.Helpers;
using NewIspNL.Services;
using NewIspNL.Services.DemandServices;
using Resources;

namespace NewIspNL.Pages
{
    public partial class ViewTickets : CustomPage
    {


        //readonly ISPDataContext _context;

        readonly DemandFactory _demandFactory;

        readonly DemandService _demandService;


        public ViewTickets()
        {
            var context = IspDataContext;
            var entries = new IspEntries(context);
            _demandFactory = new DemandFactory(entries);
            _demandService = new DemandService(context);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_ID"] == null) return;
            Bind_TicketReasons();
            if (!IsPostBack) ProcessQuery();

        }

        void Bind_TicketReasons()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                //ddl_TicketReasons.SelectedValue = null;
                //ddl_TicketReasons.Items.Clear();
                //ddl_TicketReasons.AppendDataBoundItems = true;
                var query = context.TicketReasons.ToList();
                ddl_TicketReasons.DataSource = query;
                ddl_TicketReasons.DataBind();
                Helper.AddDefaultItem(ddl_TicketReasons);
            }
        }
        void ProcessQuery()
        {
            var ts = QueryStringSecurity.Decrypt(Request.QueryString["ts"]);
            if (string.IsNullOrEmpty(ts))
            {
                lbl_Link.Text = Tokens.Linkerror;
                lbl_Link.ForeColor = Color.Red;
            }
            else
            {
                int ticketStatusId;
                if (int.TryParse(ts, out ticketStatusId))
                {
                    if (ticketStatusId > 0 && ticketStatusId < 4)
                    {
                        switch (ticketStatusId)
                        {
                            case 1:
                                lblTitle.Text = Tokens.MenuPendingTicketing;
                                break;
                            case 2:
                                lblTitle.Text = Tokens.MenuPendingSolving;
                                break;
                            case 3:
                                lblTitle.Text = Tokens.MenuSolved;
                                break;
                        }
                        Bind_grd_Tickets(Request.QueryString["ts"]);
                    }
                    else
                    {
                        lbl_Link.Text = Tokens.Linkerror;
                        lbl_Link.ForeColor = Color.Red;
                    }
                }
                else
                {
                    lbl_Link.Text = Tokens.Linkerror;
                    lbl_Link.ForeColor = Color.Red;
                }
            }
        }


        void Bind_grd_Tickets(string ticketStatus, int filter = 0)
        {

            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var que = QueryStringSecurity.Decrypt(ticketStatus);
                var ticketStatusId = Convert.ToInt32(que);
                int dataLevel = (context.Users.Where(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).Select(usr => usr.Group.DataLevelID)).First().Value;
                int userBranchId = (context.Users.Where(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).Select(usr => usr.BranchID)).First().Value;
                if (dataLevel == 1) //sys Admin
                {
                    var connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                    var cmd = new SqlCommand("PROC_GET_TICKETS", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@TICKETSTATUSID", SqlDbType.Int)).Value = ticketStatusId;
                    cmd.Parameters.Add(new SqlParameter("@USERID", SqlDbType.Int)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.DateTime)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.DateTime)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@WorkOrderID", SqlDbType.Int)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@BRANCHID", SqlDbType.Int)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@BRANCHADMINID", SqlDbType.Int)).Value = DBNull.Value;

                    connection.Open();
                    var table = new DataTable();
                    table.Load(cmd.ExecuteReader());
                    connection.Close();

                    if (filter == 0)
                    {
                        grd_Tickets.DataSource = table;
                        grd_Tickets.DataBind();
                    }
                    else
                    {
                        grd_Tickets.DataSource = table.Select("ReasonID = " + ddl_TicketReasons.SelectedValue + ""); ;
                        grd_Tickets.DataBind();
                    }

                    if (ticketStatusId == 1)
                    {
                        grd_Tickets.Columns[10].Visible = true;
                        grd_Tickets.Columns[11].Visible = true;
                        grd_Tickets.Columns[12].Visible = false;
                        grd_Tickets.Columns[13].Visible = true;
                        grd_Tickets.Columns[14].Visible = true;
                        grd_Tickets.Columns[15].Visible = true;
                        grd_Tickets.Columns[16].Visible = false;
                        grd_Tickets.Columns[17].Visible = false;
                    }
                    else if (ticketStatusId == 2)
                    {
                        grd_Tickets.Columns[10].Visible = true;
                        grd_Tickets.Columns[11].Visible = true;
                        grd_Tickets.Columns[12].Visible = false;
                        grd_Tickets.Columns[13].Visible = false;
                        grd_Tickets.Columns[14].Visible = false;
                        grd_Tickets.Columns[15].Visible = true;
                        grd_Tickets.Columns[16].Visible = true;
                        grd_Tickets.Columns[17].Visible = true;
                    }
                    else
                    {
                        grd_Tickets.Columns[10].Visible = true;
                        grd_Tickets.Columns[11].Visible = true;
                        grd_Tickets.Columns[12].Visible = true;
                        grd_Tickets.Columns[13].Visible = false;
                        grd_Tickets.Columns[14].Visible = false;
                        grd_Tickets.Columns[15].Visible = false;
                        grd_Tickets.Columns[16].Visible = false;
                        grd_Tickets.Columns[17].Visible = false;
                    }
                }
                else if (dataLevel == 2) //Branch Admin
                {
                    var connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                    var cmd = new SqlCommand("PROC_GET_TICKETS", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add(new SqlParameter("@TICKETSTATUSID", SqlDbType.Int)).Value = ticketStatusId;
                    cmd.Parameters.Add(new SqlParameter("@USERID", SqlDbType.Int)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.DateTime)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.DateTime)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@WorkOrderID", SqlDbType.Int)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@BRANCHID", SqlDbType.Int)).Value = userBranchId;
                    cmd.Parameters.Add(new SqlParameter("@BRANCHADMINID", SqlDbType.Int)).Value = Convert.ToInt32(HttpContext.Current.Session["User_ID"]);

                    connection.Open();
                    var table = new DataTable();
                    table.Load(cmd.ExecuteReader());
                    connection.Close();
                    grd_Tickets.DataSource = table;
                    grd_Tickets.DataBind();
                    if (ticketStatusId == 1)
                    {
                        grd_Tickets.Columns[10].Visible = true;
                        grd_Tickets.Columns[11].Visible = true;
                        grd_Tickets.Columns[12].Visible = false;
                        grd_Tickets.Columns[13].Visible = true;
                        grd_Tickets.Columns[14].Visible = true;
                        grd_Tickets.Columns[15].Visible = false;
                        grd_Tickets.Columns[16].Visible = false;
                    }
                    else if (ticketStatusId == 2)
                    {
                        grd_Tickets.Columns[10].Visible = true;
                        grd_Tickets.Columns[11].Visible = true;
                        grd_Tickets.Columns[12].Visible = false;
                        grd_Tickets.Columns[13].Visible = false;
                        grd_Tickets.Columns[14].Visible = false;
                        grd_Tickets.Columns[15].Visible = true;
                        grd_Tickets.Columns[16].Visible = true;
                    }
                    else
                    {
                        grd_Tickets.Columns[10].Visible = true;
                        grd_Tickets.Columns[11].Visible = true;
                        grd_Tickets.Columns[12].Visible = true;
                        grd_Tickets.Columns[13].Visible = false;
                        grd_Tickets.Columns[14].Visible = false;
                        grd_Tickets.Columns[15].Visible = false;
                        grd_Tickets.Columns[16].Visible = false;
                    }


                    grd_Tickets.Columns[13].Visible = false;
                    grd_Tickets.Columns[14].Visible = false;
                    grd_Tickets.Columns[15].Visible = false;
                    grd_Tickets.Columns[16].Visible = false;
                    //--------------------------------------
                }
                else if (dataLevel == 3) //reseller
                {
                    var connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                    var cmd = new SqlCommand("PROC_GET_TICKETS", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add(new SqlParameter("@TICKETSTATUSID", SqlDbType.Int)).Value = ticketStatusId;
                    cmd.Parameters.Add(new SqlParameter("@USERID", SqlDbType.Int)).Value = Convert.ToInt32(Session["User_ID"]);
                    cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.DateTime)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.DateTime)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@WorkOrderID", SqlDbType.Int)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@BRANCHID", SqlDbType.Int)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@BRANCHADMINID", SqlDbType.Int)).Value = DBNull.Value;
                    connection.Open();
                    var table = new DataTable();
                    table.Load(cmd.ExecuteReader());
                    connection.Close();
                    grd_Tickets.DataSource = table;
                    grd_Tickets.DataBind();

                    grd_Tickets.Columns[11].Visible = true;
                    grd_Tickets.Columns[14].Visible = false;
                    grd_Tickets.Columns[15].Visible = false;
                    grd_Tickets.Columns[16].Visible = false;
                    grd_Tickets.Columns[17].Visible = false;

                    if (ticketStatusId > 1)
                        grd_Tickets.Columns[8].Visible = true;
                }
            }
        }



        protected void lnb_Reject_Click(object sender, EventArgs e)
        {
            mpe_Reject.Show();
            var ticketId = Convert.ToInt32(grd_Tickets.DataKeys[((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex]["ID"]);
            ViewState.Add("TicketID", ticketId);
        }


        void PendSolvingTicket(int ticketId)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var tickets = context.Tickets.Where(ticket => ticket.ID == ticketId);
                var item = tickets.First();
                item.ProviderTicketNo = txt_ProviderTicketNo.Text.Trim();
                item.StatusID = 2;
                context.SubmitChanges();
                var itemcomment = new TicketComment
                {
                    Comment = txt_Comment1.Text.Trim(),
                    CommentDate = DateTime.Now.AddHours(),
                    TicketID = ticketId,
                    TicketStatusID = 2,
                    UserID = Convert.ToInt32(Session["User_ID"])
                };
                context.TicketComments.InsertOnSubmit(itemcomment);
                context.SubmitChanges();

                lbl_Link.Text = Tokens.ItemAdded;
                lbl_Link.ForeColor = Color.Green;
                Bind_grd_Tickets(Request.QueryString["ts"]);
            }
        }


        protected void btn_Reject_Click(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var query = context.Tickets.Where(ticket => ticket.ID == Convert.ToInt32(ViewState["TicketID"]));
                var item = query.First();
                item.ProviderTicketNo = "---";
                item.StatusID = 3;
                context.SubmitChanges();
                var userid = Convert.ToInt32(Session["User_ID"]);
                var itemcomment = new TicketComment
                {
                    Comment = txt_Comment2.Text.Trim(),
                    CommentDate = DateTime.Now.AddHours(),
                    TicketID = Convert.ToInt32(ViewState["TicketID"]),
                    TicketStatusID = 2,
                    UserID = userid
                };
                context.TicketComments.InsertOnSubmit(itemcomment);
                context.SubmitChanges();
                lbl_Link.Text = Tokens.Rejected;
                lbl_Link.ForeColor = Color.Green;
                var option = OptionsService.GetOptions(context, true);
                if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                {
                    CenterMessage.SendRequestReject(item.WorkOrder, txt_Comment2.Text, Tokens.Ticket, userid);
                }
                Bind_grd_Tickets(Request.QueryString["ts"]);
            }
        }


        protected void btn_Solved_Click(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var ticketId = Convert.ToInt32(HfTicketIdToSolve.Value);/* Convert.ToInt32(ViewState["TicketID"])*/
                var query = context.Tickets.Where(ticket => ticket.ID == ticketId);
                var item = query.First();
                item.StatusID = 3;
                // item.TicketComments = txt_Comment3.Text;
                context.SubmitChanges();
                var userid = Convert.ToInt32(Session["User_ID"]);
                var itemcomment = new TicketComment
                {
                    Comment = txt_Comment3.Text.Trim(),
                    CommentDate = DateTime.Now.AddHours(),
                    TicketID = ticketId,// Convert.ToInt32(ViewState["TicketID"]),
                    TicketStatusID = 3,
                    UserID = userid
                };
                context.TicketComments.InsertOnSubmit(itemcomment);
                context.SubmitChanges();

                if (RbWith.Checked)
                {
                    var demand =
                        _demandService.CustomerDemands(item.WorkOrder.ID, context).OrderByDescending(x => x.Id).FirstOrDefault();
                    var amount = Convert.ToDecimal(TbDiscount.Text);
                    if (demand != null && !demand.Paid)
                    {
                        demand.Amount -= amount;
                        demand.Notes += " - خصم تذكرة";
                        
                    }
                    else
                    {
                        if (demand != null)
                        {
                            var newDemand = _demandFactory.CreateDemand(item.WorkOrder, demand.StartAt, demand.EndAt, amount * -1,
                                Convert.ToInt32(Session["User_ID"]), paid: false, notes: "تذكرة"
                                /*, applyResellerDiscount : false*/);
                            context.Demands.InsertOnSubmit(newDemand);
                        }
                    }
                    context.SubmitChanges();
                }
                if (RbDeport.Checked)
                {
                    var days = Convert.ToInt32(txtdeportDays.Text);
                    var wr = context.WorkOrders.FirstOrDefault(x => x.ID == item.WorkOrderID);
                    if (wr != null)
                    {
                        if (wr.RequestDate != null)
                        {
                            var v = wr.RequestDate.Value.Date.AddDays(days);
                            wr.RequestDate = v;
                        }
                        context.SubmitChanges();
                    }
                }
                var option = OptionsService.GetOptions(context, true);
                if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                {
                    CenterMessage.SendRequestApproval(item.WorkOrder, Tokens.Ticket, userid);
                }
                lbl_Link.Text = Tokens.Solved;
                lbl_Link.ForeColor = Color.Green;

                Bind_grd_Tickets(Request.QueryString["ts"]);
            }
        }


        /*protected void lnb_Solved_Click(object sender, EventArgs e){
            mpe_Solved.Show();
            int TicketID = Convert.ToInt32(grd_Tickets.DataKeys[((GridViewRow) ((LinkButton) sender).Parent.Parent).RowIndex]["ID"]);
            ViewState.Add("TicketID", TicketID);
        }*/


        protected void grd_Tickets_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grd_Tickets.EditIndex = e.NewEditIndex;
            Bind_grd_Tickets(Request.QueryString["ts"]);
        }


        protected void grd_Tickets_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grd_Tickets.EditIndex = -1;
            Bind_grd_Tickets(Request.QueryString["ts"]);
        }


        protected void grd_Tickets_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var datakey = grd_Tickets.DataKeys[e.RowIndex];
                if (datakey == null) return;
                var commentId = Convert.ToInt32(datakey["LastCommentID"]);
                var query = context.TicketComments.Where(tc => tc.ID == commentId);
                var updatedtc = query.First();
                updatedtc.Comment = ((TextBox)(grd_Tickets.Rows[e.RowIndex].FindControl("TextBox1"))).Text;
                context.SubmitChanges();
                var currentTicket = context.Tickets.First(t => t.ID == Convert.ToInt32(grd_Tickets.DataKeys[e.RowIndex]["ID"]));
                currentTicket.ProviderTicketNo = ((TextBox)(grd_Tickets.Rows[e.RowIndex].FindControl("TextBox2"))).Text;
                context.SubmitChanges();
                grd_Tickets.EditIndex = -1;
                Bind_grd_Tickets(Request.QueryString["ts"]);
            }
        }


        protected void grd_Tickets_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                ViewState["No"] = null;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ViewState["No"] == null)
                {
                    if (grd_Tickets.PageIndex != -1 && grd_Tickets.PageIndex != 0)
                    {
                        ViewState["No"] = grd_Tickets.PageIndex * 20;
                        ViewState["No"] = Convert.ToInt32(ViewState["No"]) + 1;
                        ((Label)e.Row.FindControl("lbl_No")).Text = ViewState["No"].ToString();
                    }
                    else
                    {
                        ((Label)e.Row.FindControl("lbl_No")).Text = @"1";
                        ViewState["No"] = 1;
                    }
                }
                else
                {
                    ViewState["No"] = Convert.ToInt32(ViewState["No"]) + 1;
                    ((Label)e.Row.FindControl("lbl_No")).Text = ViewState["No"].ToString();
                }
            }
        }


        protected void AddTickect(object sender, EventArgs e)
        {
            var ticketId = Convert.ToInt32(HfTicketId.Value);
            PendSolvingTicket(ticketId);
        }

    }

}