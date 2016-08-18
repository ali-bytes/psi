using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;
using NewIspNL.Services;
using Resources;

namespace NewIspNL.Pages
{
    public partial class AddTicket : CustomPage
    {
         
            readonly IWOrkOrderConverter _converter = new WOrkOrderConverter();
            readonly ISPDataContext _context;
            readonly IspDomian _domian;

            public AddTicket()
            {
                _context = IspDataContext;
                _domian = new IspDomian(_context);
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                using (var db = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    if (Session["User_ID"] == null) return;
                    if (IsPostBack) return;
                    Bind_TicketReasons();
                    ProcessQuery();
                    var option = OptionsService.GetOptions(db, true);
                    if (option == null || option.IncludeGovernorateInSearch == false) GovBox.Visible = false;
                    else
                    {
                        _domian.PopulateGovernorates(DdlGovernorate);
                        GovBox.Visible = true;
                    }
                }
            }


            protected void btn_search_Click(object sender, EventArgs e)
            {
                using (var db1 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var orders = DataLevelClass.GetUserWorkOrder();
                    var option = OptionsService.GetOptions(db1, true);
                    WorkOrder order;
                    if (option != null && option.IncludeGovernorateInSearch)
                        order = DdlGovernorate.SelectedIndex > 0 ? orders.FirstOrDefault(wo => wo.CustomerPhone == txt_CustomerPhone0.Text.Trim() && wo.CustomerGovernorateID == Convert.ToInt32(DdlGovernorate.SelectedItem.Value)) : null;
                    else order = orders.FirstOrDefault(wo => wo.CustomerPhone == txt_CustomerPhone0.Text.Trim());
                    if (order != null)
                    {
                        FetchInfoForWorkOrder(order);
                        //check if the TargetWO exsists in user workorders
                        IEnumerable<bool> MatchedList = orders.Select(tmpwo => tmpwo.ID == order.ID);
                        if (MatchedList.Contains(true))
                        {
                            var Query = from wo in db1.WorkOrders
                                        where wo.CustomerPhone == txt_CustomerPhone0.Text.Trim()
                                        select new
                                        {
                                            wo.ID,
                                            wo.WorkOrderStatusID
                                        };

                            if (Query.First().WorkOrderStatusID == 6 || Query.First().WorkOrderStatusID == 5) //Check if the client is active
                            {
                                //check if the client already has un Solved ticket
                                var TicketQuery = from tickt in db1.Tickets
                                                  where tickt.WorkOrderID == Query.First().ID
                                                        && tickt.StatusID != 3 //Not Solved
                                                  select tickt;
                                if (TicketQuery.Count() > 0)
                                {
                                    lbl_SearchResult.Text = Tokens.PendingTicket;
                                    tr_AddTicket.Visible = false;
                                }
                                else
                                {
                                    ViewState.Add("WOID", Query.First().ID);
                                    tr_AddTicket.Visible = true;
                                    lbl_AddResult.Visible = false;
                                }
                            }
                            else
                            {
                                lbl_SearchResult.Text = Tokens.TicketNotActive;
                                tr_AddTicket.Visible = false;
                            }
                        }
                        else
                        {
                            lbl_SearchResult.Text = Tokens.NoResults;
                            tr_AddTicket.Visible = false;
                        }
                    }
                    else
                    {
                        lbl_SearchResult.Text = Tokens.NoResults;
                        tr_AddTicket.Visible = false;
                    }
                }
            }


            void FetchInfoForWorkOrder(WorkOrder targetWo)
            {
                var info = _converter.ToPreviewTemplate(targetWo);
                lActivation.Text = info.Activation.ToShortDateString();
                lBranch.Text = info.Branch;
                lCentral.Text = info.Central;
                lGovernate.Text = info.Governate;
                lName.Text = info.Name;
                lOffer.Text = info.Offer;
                lPackage.Text = info.Package;
                lPhone.Text = info.Phone;
                lProvider.Text = info.Provider;
                lReseller.Text = info.Reseller;
                lState.Text = info.State;
            }




            void Bind_TicketReasons()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    ddl_TicketReasons.SelectedValue = null;
                    ddl_TicketReasons.Items.Clear();
                    ddl_TicketReasons.AppendDataBoundItems = true;
                    var query = context.TicketReasons.Select(tr => tr);
                    ddl_TicketReasons.DataSource = query;
                    ddl_TicketReasons.DataBind();
                    Helper.AddDefaultItem(ddl_TicketReasons);
                }
            }


            protected void btn_AddTicket_Click(object sender, EventArgs e)
            {
                using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var newTicket = new Ticket
                    {
                        Details = txt_Details.Text,
                        ReasonID = Convert.ToInt32(ddl_TicketReasons.SelectedItem.Value),
                        StatusID = 1,
                        TicketDate = DateTime.Now.AddHours(),
                        UserID = Convert.ToInt32(Session["User_ID"]),
                        WorkOrderID = Convert.ToInt32(ViewState["WOID"])
                    };
                    dataContext.Tickets.InsertOnSubmit(newTicket);
                    dataContext.SubmitChanges();
                    lbl_SearchResult.Text = Tokens.TikectAdded;
                    lbl_AddResult.ForeColor = Color.Green;
                    tr_AddTicket.Visible = false;
                    ClearControls(this);
                }
            }


            void ClearControls(Control ctr)
            {
                if (ctr.HasControls())
                {
                    foreach (Control tmpctr in ctr.Controls)
                    {
                        if (ctr.HasControls())
                            ClearControls(tmpctr);
                        else if (ctr is TextBox)
                            ((TextBox)ctr).Text = "";
                        else if (ctr is DropDownList)
                        {
                            ((DropDownList)ctr).SelectedValue = "-1";
                            //((DropDownList)ctr).Items.Clear();
                        }
                    }
                }
                else if (ctr is TextBox)
                    ((TextBox)ctr).Text = "";
                else if (ctr is DropDownList)
                {
                    ((DropDownList)ctr).SelectedIndex = -1;
                    //((DropDownList)ctr).Items.Clear();
                }
            }


            void ProcessQuery()
            {
                using (var db3 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    if (string.IsNullOrEmpty(Request.QueryString["woid"])) return;
                    var workOrderId = QueryStringSecurity.Decrypt(Request.QueryString["woid"]);
                    if (string.IsNullOrEmpty(workOrderId))
                        return;
                    else
                    {
                        int woid;
                        if (int.TryParse(workOrderId, out woid))
                        {
                            var query = from wo in db3.WorkOrders
                                        where wo.ID == woid
                                        select
                                            new
                                            {
                                                wo.CustomerPhone,
                                                wo.CustomerGovernorateID
                                            };
                            if (query.Any())
                            {

                                txt_CustomerPhone0.Text = query.First().CustomerPhone;
                                btn_search_Click(null, null);
                            }
                        }
                    }
                }
            }
        }
    }
 