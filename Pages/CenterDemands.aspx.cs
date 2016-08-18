using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.SearchService;
using NewIspNL.Helpers;
using NewIspNL.Services;
using Resources;

namespace NewIspNL.Pages
{
    public partial class CenterDemands : CustomPage
    {
        
          
            readonly DemandService _demandService;

            readonly ICenterCreditRepository _centerRepository;
            readonly IspEntries _ispEntries;
            readonly SearchEngine _searchEngine;
            readonly IspDomian _domian;
            public Option WidthOption { get; set; }
            public  CenterDemands()
            {
                var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
                _demandService = new DemandService(context);
                _ispEntries = new IspEntries(context);
                _searchEngine = new SearchEngine(context);
                _domian = new IspDomian(context);
                _centerRepository = new CenterCreditRepository();
                DetailsUrl = "CustomerDetails.aspx?c=";
                WidthOption = context.Options.Any() ? context.Options.FirstOrDefault() : new Option();
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                using (var context1 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    HandlePrivildges();
                    if (IsPostBack) return;
                    var option = OptionsService.GetOptions(context1, true);
                    if (option == null || option.IncludeGovernorateInSearch == false) GovBox.Visible = false;
                    else
                    {
                        _domian.PopulateGovernorates(DdGovernorates);
                        GovBox.Visible = true;
                    }
                    suspendMsg.Visible = false;
                    
                    if (!string.IsNullOrWhiteSpace(Request.QueryString["c"]) && !string.IsNullOrWhiteSpace(Request.QueryString["g"]))
                    {
                        var phone = Request.QueryString["c"];
                        var gov = Request.QueryString["g"];
                        if (option != null && option.IncludeGovernorateInSearch)
                        {
                            DdGovernorates.SelectedValue = gov;
                        }
                        TPhone.Text = phone;
                        SearchDemands();
                    }

                }
            }


            void HandlePrivildges()
            {
                int userId = Convert.ToInt32(Session["User_ID"]);
                CanPay = _ispEntries.UserHasPrivlidge(userId, "PayDemand");
                
            }
            protected void BSearch_OnServerClick(object sender, EventArgs e)
            {
                ResetSearch();
            }
            void ResetSearch()
            {
                Reset();
                SearchDemands();
            }
            void Reset()
            {
                foreach (var control in Mother.Controls)
                {
                    var grd = control as GridView;
                    if (grd != null)
                    {
                        grd.DataSource = null;
                        grd.DataBind();
                    }
                    Msg.InnerHtml = string.Empty;
                }
            }


            void SearchDemands()
            {
                using (var context2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var order = Getworkorder(context2);
                    if (order == null)
                    {
                        Reset();
                        Msg.InnerHtml = Tokens.CustomerNotFound;
                        Msg.Attributes.Add("class", "alert alert-warning");
                        HfCustomerId.Value = "0";
                        GvCustomerData.DataSource = null;
                        GvCustomerData.DataBind();
                        return;
                    }

                    if (order.WorkOrderStatusID != null && order.WorkOrderStatusID.Value == 11)
                    {
                        int daysCount = _ispEntries.DaysForCustomerAtStatus(order.ID, 11);
                        suspendMsg.Visible = true;
                        suspendMsg.InnerHtml = string.Format("{0} : ( {1} ) , {2}", Tokens.SuspendDaysCount, daysCount, Tokens.CustomerShouldConvertToActive);
                      
                    }
                    else
                    {
                        suspendMsg.Visible = false;
                        suspendMsg.InnerHtml = string.Empty;
                       
                    }

                    ExtractOrderData(order);
                    GetDemands(order);
                    var fakeOrderList = new List<WorkOrder>{
                order
            };
                    GvCustomerData.DataSource = fakeOrderList.Select(x => _searchEngine.ToCustomerResult(x));
                    GvCustomerData.DataBind();
                }
            }


            WorkOrder Getworkorder(ISPDataContext _context2)
            {
                var userOrders = _context2.WorkOrders;
                var option = OptionsService.GetOptions(_context2, true);
                WorkOrder order;
                if (option != null && option.IncludeGovernorateInSearch)
                    order = DdGovernorates.SelectedIndex > 0 ?
                        userOrders.FirstOrDefault(wo => wo.CustomerPhone == TPhone.Text.Trim() && wo.CustomerGovernorateID == Convert.ToInt32(DdGovernorates.SelectedItem.Value)) :
                        null;
                else order = userOrders.FirstOrDefault(wo => wo.CustomerPhone == TPhone.Text.Trim());
                return order;

            }


            void GetDemands(WorkOrder order)
            {
                var demands = _demandService.CustomerDemands(order.ID);
                var unpaid = demands.Where(x => !x.Paid).ToList().Select(Transformer.DemandToGridPreview);
                var paid = demands.Where(x => x.Paid).ToList().Select(Transformer.DemandToGridPreview);

                GvUnpaid.DataSource = unpaid;
                GvUnpaid.DataBind();
                lblTotalUnpaid.Text = Helper.FixNumberFormat(demands.Where(a => !a.Paid).Sum(a => a.Amount));
                GvPaid.DataSource = paid;
                GvPaid.DataBind();
            }
            void ExtractOrderData(WorkOrder order)
            {
                HfCustomerId.Value = string.Format("{0}", QueryStringSecurity.Encrypt(order.ID.ToString()));
            }
            protected void GvUnpaid_OnDataBound(object sender, EventArgs e)
            {

               
                Helper.GridViewNumbering(GvUnpaid, "LNo");
                if (GvUnpaid.Rows.Count <= 0) return;
                var button = GvUnpaid.Rows[0].FindControl("BPay") as Button;
                if (button == null) return;
                button.Visible = CanPay;
            }


            protected void GvPaid_OnDataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(GvPaid, "LNo");
                Helper.GridViewNumbering(GvUnpaid, "LNo");
               
            }
            protected void DeleteCurrentDemand(object sender, EventArgs e)
            {
                var button = sender as Button;
                if (button != null)
                {
                    var demandId = Convert.ToInt32(button.CommandArgument);

                    var deleted = _ispEntries.DeleteDemand(demandId, true);
                    if (deleted)
                    {
                        _ispEntries.Commit();
                        Msg.InnerHtml = Tokens.Deleted;
                        Msg.Attributes.Add("class", "alert alert-success");
                        SearchDemands();
                    }
                    else
                    {
                        Msg.InnerHtml = Tokens.DemandCantBeDeleted;
                        Msg.Attributes.Add("class", "alert alert-warning");
                    }
                }
            }
            protected void CancelPayment(object sender, EventArgs e)
            {
                var btn = sender as Button;
                if (btn == null)
                {
                    return;
                }

                int demandId = Convert.ToInt32(btn.CommandArgument);
                _demandService.CancelPayment(demandId);
                Msg.InnerHtml = Tokens.Saved;
                Msg.Attributes.Add("class", "alert alert-success");
                SearchDemands();
            }
          

            public bool CanPay { get; set; }
            public string DetailsUrl { get; set; }

            protected void PayDemand(object sender, EventArgs e)
            {
                if (string.IsNullOrEmpty(HfDemandId.Value)) return;
                var demandId = Convert.ToInt32(HfDemandId.Value);
                int userId = Convert.ToInt32(Session["User_ID"]);
              
                var demand = _ispEntries.GetDemand(demandId);
                var lastCredit = _centerRepository.GetNetCredit(userId);

                if (demand.Amount <= lastCredit)
                {
                    var amount = demand.Amount * -1;
                    _centerRepository.Save(userId, userId, amount, txtComment.Text, DateTime.Now.AddHours());
                    payDemand(demandId);
                    Msg.InnerHtml = Tokens.Saved;
                    Msg.Attributes.Add("class", "alert alert-success");
                }
                else
                {
                    Msg.InnerHtml = Tokens.NotEnoughtCrefit;
                    Msg.Attributes.Add("class", "alert alert-danger");
                    
                }


            }


            void payDemand(int demandId)
            {
                using (var context3 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    //var demand = _ispEntries.GetDemand(demandId);
                    _demandService.Pay(demandId, Convert.ToInt32(Session["User_ID"]), txtComment.Text);
                    Msg.InnerHtml = Tokens.Saved;
                    Msg.Attributes.Add("class", "alert alert-success");
                    context3.Receipts.InsertOnSubmit(new Receipt()
                    {
                        DemandId = demandId,
                        PrcessDate = DateTime.Now.AddHours(),
                        Notes = txtComment.Text
                    });
                    context3.SubmitChanges();
                    SearchDemands();
                }
            }
        }
    }
 