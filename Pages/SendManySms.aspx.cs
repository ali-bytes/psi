using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;

using NewIspNL.Domain;
using NewIspNL.Domain.Concrete;
using NewIspNL.Domain.SearchService;
using NewIspNL.Helpers;
using NewIspNL.Models;
using NewIspNL.Services.DemandServices;

namespace NewIspNL.Pages
{
    public partial class SendManySms : CustomPage
    {
     
    readonly IspDomian _domian;
    private readonly IspEntries _ispEntries;
    readonly DemandsSearchService _searchService;
    private readonly SearchEngine _searchEngine;
    private readonly SMSData _smsData;
    public SendManySms()
    {
        _domian=new IspDomian(IspDataContext);
        _searchService=new DemandsSearchService(IspDataContext);
        _ispEntries=new IspEntries(IspDataContext);
        _smsData=new SMSData(IspDataContext);
        _searchEngine=new SearchEngine(IspDataContext);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if(IsPostBack)return;
        PrepareInputs();
        txtMeaageText.MaxLength = UICulture == "en-GB" ? 160 : 70;

    }
    void PrepareInputs()
    {
        _domian.PopulateResellerswithDirectUser(DdlReseller, true);
        _domian.PopulateBranches(DdlBranchs, true);
        _domian.PopulateProviders(DdlProvider);
        _domian.PopulateStatuses(DdlStatus);
        _domian.PopulatePaymentTypes(DdlPaymentType);

    }
    protected void SearchCustomers(object sender, EventArgs e)
    {
        SearchCus();
    }


    void SearchCus()
    {
        var searchDemands = _searchService.AdvancedSearchDemandToPreview(new AdvancedBasicSearchModel
        {
            BranchId = Helper.GetDropValue(DdlBranchs),
            ResellerId = Helper.GetDropValue(DdlReseller),
            StatusId = Helper.GetDropValue(DdlStatus),
            ProviderId = Helper.GetDropValue(DdlProvider),
            PaymentTypeId=Helper.GetDropValue(DdlPaymentType)
        });
        
        var orders=new List<DemandPreviewModel>();
        foreach (var demandPreviewModel in searchDemands)
        {
            var phone = demandPreviewModel.Phone;
            var l = orders.Where(a => a.Phone == phone);
            if(l.Any())continue;
            orders.Add(demandPreviewModel);
        }
      
        GvResults.DataSource = orders.OrderBy(a => a.Status);
        GvResults.DataBind();
    }
    protected void NumberGrid(object sender, EventArgs e)
    {
        Helper.GridViewNumbering(GvResults, "LNo");
    }

    private string SendSms(global::Db.SMSCnfg data, string mobile, string messageText)
    {
       
        if (!string.IsNullOrWhiteSpace(mobile) && !string.IsNullOrWhiteSpace(messageText))
        {
            var message = global::SendSms.Send(data.UserName, data.Password, mobile, messageText, data.Sender, data.UrlAPI);
            
            string myscript = "window.open('" + message + "');";
            return myscript;
            
        }
        return string.Empty;
    }

    protected void btnSendSms_Click(object sender, EventArgs e)
    {
        var messages = new StringBuilder();
       global::Db.SMSCnfg data = _smsData.GetActiveCnfg();
        foreach (GridViewRow row in GvResults.Rows)
        {
            //var cb = row.FindControl("CbPay") as CheckBox;
            //if (cb == null || !cb.Checked) continue;
            var dataKey = GvResults.DataKeys[row.RowIndex];
            if (dataKey != null)
            {
                var demandid =Convert.ToInt32(dataKey.Value);
                var demand = _ispEntries.GetDemand(demandid);
                //var woId = demand.WorkOrderId;
                var order = demand.WorkOrder;
                messages.Append(SendSms(data, order.CustomerMobile, txtMessageText.Text));
            }
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "myscript", messages.ToString(), true);
        Msg.Visible = true;
    }
    protected void btnSendSmsDay_Click(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var smsData = new SMSData(context);
            var data = smsData.GetActiveCnfg();
            var days = int.Parse(txtDays.Text);
            var now = DateTime.Now.AddHours();
            //var orders = context.WorkOrders.Where(a => (now - a.RequestDate.Value.Date).Days <= days).ToList();
            var orders = context.WorkOrders.Where(a => a.RequestDate.Value != null).ToList();
            var targetOrders = new List<WorkOrder>();
            foreach (var order in orders)
            {
                if (order.RequestDate != null)
                {
                    var d = (order.RequestDate.Value.Date - now.Date).Days;
                    if (d > 0)
                    {
                        if (days >= d)
                        {
                            targetOrders.Add(order);
                        }
                    }
                }
            }
            if (targetOrders.Any())
            {
                string myscript = string.Empty;
                foreach (var workOrder in targetOrders)
                {
                    var mobile = workOrder.CustomerMobile;
                    if (!string.IsNullOrWhiteSpace(mobile))
                    {
                        var message = global::SendSms.Send(data.UserName, data.Password, mobile, txtMeaageText.Text, data.Sender,
                            data.UrlAPI);
                        myscript += "window.open('" + message + "');";
                    }
                    
                }
                ClientScript.RegisterClientScriptBlock(typeof(Page), "myscript", myscript, true);
                Msgsuc.Visible = true;
            }
            else
            {
                errorMsg.Visible = true;
            }

            Clear();
        }
    }
    void Clear()
    {
        txtDays.Text = txtMeaageText.Text = string.Empty;
    }
}
}