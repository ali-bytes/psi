using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using NewIspNL.Models;
using NewIspNL.Services;
using NewIspNL.Services.DemandServices;
using Resources;

namespace NewIspNL.Pages
{
    public partial class UnPaidDemands : CustomPage
    {
        
   readonly DemandService _demandService;

    readonly IspDomian _domian;
    private readonly IspEntries _ispEntries;
    private readonly IUserSaveRepository _userSave;
    readonly DemandsSearchService _searchService;
    //public bool CanDelete { get; set; }

    public UnPaidDemands()
    {
        _demandService = new DemandService();
        _searchService = new DemandsSearchService(IspDataContext);
        _domian = new IspDomian(IspDataContext);
        _ispEntries=new IspEntries(IspDataContext);
        _userSave=new UserSaveRepository();
        //CanDelete = false;
    }


    protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack) return;
        PrepareInputs();
        HideBtns(false);
        PopulateSvaes();
        HandlePrivildges();
        lblFromRequestDate.Text = Tokens.RequestDate + @" : " + Tokens.From;
    }


    void HideBtns(bool hide){
        Pb1.Visible = hide;
        Pb2.Visible = hide;
    }

    private void HandlePrivildges()
    {
        if (Session["User_ID"] == null) return;
        var userId = Convert.ToInt32(Session["User_ID"]);
        divbtnDelete.Visible=divbtnDelete2.Visible = _ispEntries.UserHasPrivlidge(userId, "DeleteDemand");
    }

    void PopulateSvaes()
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var userId = Convert.ToInt32(Session["User_ID"]);
             ddlSavesPay.DataSource = _userSave.SavesOfUser(userId, context).Select(a => new
            {
                a.Save.SaveName,
                a.Save.Id
            });
            ddlSavesPay.DataBind();
            Helper.AddDefaultItem(ddlSavesPay);
        }
    }

    void PrepareInputs(){
        _domian.PopulateResellerswithDirectUser(DdlReseller, true);
        _domian.PopulateGovernorates(DdlGovernorate);
        _domian.PopulateBranches(DdlBranchs, true);
        _domian.PopulateProviders(DdlProvider);
        _domian.PopulateStatuses(ddlCustomerStatus);
        _domian.PopulatePackages(DdlPackage);
        _domian.PopulateIpPackages(DdlIpPackages);

        Helper.AddDefaultItem(DdlCentral);
        var currentYear = DateTime.Now.Year;
        Helper.PopulateDrop(Helper.FillYears(currentYear - 5, currentYear).OrderByDescending(x=>x), DdlYear);
        Helper.PopulateMonths(DdlMonth);
        _domian.PopulatePaymentTypes(ddlPaymentType);
    }


    protected void SearchDemands(object sender, EventArgs e){
        SearchDemands();
    }


    void SearchDemands(){
   
        var searchDemands = _searchService.AdvancedSearchDemandToPreview(new AdvancedBasicSearchModel
        {
            BranchId = Helper.GetDropValue(DdlBranchs),
            CentralId = Helper.GetDropValue(DdlCentral),
            GovernorateId = Helper.GetDropValue(DdlGovernorate),
            Paid = false,
            ResellerId = Helper.GetDropValue(DdlReseller),
            Month = Helper.GetDropValue(DdlMonth),
            Year = Helper.GetDropValue(DdlYear),
            ProviderId = Helper.GetDropValue(DdlProvider),
            StatusId = Helper.GetDropValue(ddlCustomerStatus),
            PackageId = Helper.GetDropValue(DdlPackage),
            IpPackageId = Helper.GetDropValue(DdlIpPackages),
            PaymentTypeId = Helper.GetDropValue(ddlPaymentType),
           
        });
        if(!string.IsNullOrEmpty(txtFrom.Text) && !string.IsNullOrEmpty(txtTo.Text)){
            
            searchDemands = searchDemands.Where(s => s.StartAt >= Convert.ToDateTime(txtFrom.Text) && s.StartAt<=Convert.ToDateTime(txtTo.Text)).ToList();
        }
        if (!string.IsNullOrWhiteSpace(txtFromRequestDate.Text))
            searchDemands =
                searchDemands.Where(a => a.RequestDate.Date >= Convert.ToDateTime(txtFromRequestDate.Text).Date)
                    .ToList();
        if (!string.IsNullOrWhiteSpace(txtToRequestDate.Text))
            searchDemands =
                searchDemands.Where(a => a.RequestDate.Date <= Convert.ToDateTime(txtToRequestDate.Text).Date).ToList();
        if (Session["User_ID"] == null)
            Response.Redirect("../default.aspx");
        var userId = Convert.ToInt32(Session["User_ID"]);
        var user = _searchService.GetUser(userId);
        var dataleve = user.Group.DataLevelID;
        switch (dataleve)
        {
            case 1:
                break;
            case 2:
                searchDemands =
                    searchDemands.Where(a => DataLevelClass.GetBranchAdminBranchIDs(user.ID).Contains(a.BranchId))
                        .ToList();
                break;
            case 3:
                searchDemands = searchDemands.Where(a => a.Reseller.Equals(user.UserName)).ToList();
                break;
        }
        GvResults.DataSource = searchDemands.OrderBy(a=>a.Status);
        GvResults.DataBind();

        //var suspendedOrders = searchDemands.Where(x => x.StatusId != 6);
        //GvSuspned.DataSource = suspendedOrders;
        //GvSuspned.DataBind();
        HideBtns(GvResults.Rows.Count > 0);
    }


    protected void NumberGrid(object sender, EventArgs e){
        Helper.GridViewNumbering(GvResults, "LNo");
    }


    protected void PayDemand(object sender, EventArgs e){
        var btn = sender as Button;
        if(btn == null) return;
        var demandId = Convert.ToInt32(btn.CommandArgument);
        ProceedPayDemand(demandId);
    }


    void ProceedPayDemand(int demandId)
    {
        var userId = Convert.ToInt32(Session["User_ID"]);
        _demandService.Pay(demandId,userId , TbComment.Text);
        var demand = _ispEntries.GetDemand(demandId);
        var notes2 = TbComment.Text + " - " + demand.WorkOrder.CustomerName + " - " +
            demand.WorkOrder.CustomerPhone;
        var amount = Convert.ToDouble(demand.Amount);
        Savestepsinsaves(amount, notes2);
        Msg.InnerHtml = Tokens.Saved;
        SearchDemands();
    }


    protected void PaySelectedDemands(object sender, EventArgs e){
        foreach(GridViewRow row in GvResults.Rows){
            var cb = row.FindControl("CbPay") as CheckBox;
            if(cb == null || !cb.Checked) continue;
            var demandId = Convert.ToInt32(cb.CssClass);
            ProceedPayDemand(demandId);
        }
    }
    protected void DeleteSelectedDemands(object sender, EventArgs e)
    {
        foreach (GridViewRow row in GvResults.Rows)
        {
            var cb = row.FindControl("CbPay") as CheckBox;
            if (cb == null || !cb.Checked) continue;
            var demandId = Convert.ToInt32(cb.CssClass);
           var done= _ispEntries.DeleteDemand(demandId);

            if (done)
            {
                _ispEntries.Commit();
                Msg.InnerHtml = Tokens.Deleted;
                
            }
            else
            {
                Msg.InnerHtml = Tokens.DemandCantBeDeleted;
            }
        }SearchDemands();
    }

    protected void SendsmsSelected(object sender, EventArgs e)
    {
        var messages = new StringBuilder();
        foreach (GridViewRow row in GvResults.Rows)
        {
            var cb = row.FindControl("CbPay") as CheckBox;
            if (cb == null || !cb.Checked) continue;
            var demandId = Convert.ToInt32(cb.CssClass);
            var demand = _ispEntries.GetDemand(demandId);
            var mobile = demand.WorkOrder.CustomerMobile;
             messages.Append(SendSms(mobile));
        }
        ClientScript.RegisterClientScriptBlock(typeof (Page), "myscript", messages.ToString(), true);
        Msg.InnerHtml = Tokens.Saved;
    }
    private string SendSms(string mobile)
    {
        using (var context1 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var smsData = new SMSData(context1);
            var data = smsData.GetActiveCnfg();
            if (!string.IsNullOrWhiteSpace(mobile) && !string.IsNullOrWhiteSpace(txtMessageText.Text) && data!=null) // && Convert.ToBoolean(data.sendsms))
            {

                var message = global::SendSms.Send(data.UserName, data.Password, mobile, txtMessageText.Text, data.Sender,
                    data.UrlAPI);
                string myscript = "window.open('" + message + "');";
                return myscript;
            }
            return string.Empty;
        }
    }

    void Savestepsinsaves(double amount, string note2)
    {
        try
        {
            using (var context5 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                int userId = Convert.ToInt32(Session["User_ID"]);
                var saveId = Convert.ToInt32(ddlSavesPay.SelectedItem.Value);
                _userSave.BranchAndUserSaves(saveId,userId,amount, "دفع مطالبة من صفحة مطالبات غير مدفوعة ", note2, context5);
            }
        }
        catch { }
    }

    //protected void SusNumberGrid(object sender, EventArgs e){
    //    Helper.GridViewNumbering(GvSuspned, "LNo");
    //}
    protected void DdlGovernorate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DdlGovernorate.SelectedIndex <= 0)
        {
            DdlCentral.Items.Clear();
            Helper.PopulateDrop(null, DdlCentral);
            return;
        }
        var governorateId = Convert.ToInt32(DdlGovernorate.SelectedItem.Value);
        _domian.PopulateCentrals(DdlCentral, governorateId);

    }
    protected void btnExport_click(object sender, EventArgs e)
    {
        //creating the array of GridViews and calling the Export function
        var gvList = new GridView[] {GvResults};//, GvSuspned};
        GridHelper.Export("UnpaidDemands.xls", gvList);
    }

    protected void SuspendOrder(object sender, EventArgs e)
    {
        Div1.InnerHtml = "";
        DivSuccess.InnerHtml = "";
        CookieContainer cookiecon = null;
        int PortalSentCount = 0;
        int PortalnotSentCount = 0;
        int SysSentCount = 0;
        bool isPortalEnable = false;
        List<int> ids = new List<int>();
        foreach (GridViewRow row in GvResults.Rows)
        {
            var control = row.FindControl("CbPay") as CheckBox;
            if (control == null || !control.Checked) continue;
            var dataKey = GvResults.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            var id = Convert.ToInt32(dataKey["WorkOrderId"]);
            ids.Add(id);
        }
        var newIds = ids.Distinct();
        foreach (var d in newIds)
        {
            var id = d;
            // حط هنا شرك انه مفعل الابوشن والا لأ .. والا حاطة هناك فى كلاس الربط ؟؟
            //get from WorkOrder
            var note = "";
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var portalList = context.OptionPortalProviders.Select(z => z.PortalProvidersId).ToList();
                var wor = context.WorkOrders.FirstOrDefault(z => z.ID == id);
                if (wor != null && portalList.Contains(wor.ServiceProviderID))
                {
                    isPortalEnable = true;
                    var username = wor.UserName;
                    if (username.Length > 0)
                    {
                        if (cookiecon == null)
                        {
                            cookiecon = new CookieContainer();
                            cookiecon = Tedata.Login();
                        }


                        if (cookiecon != null)
                        {
                            var pagetext = Tedata.GetSearchPage(username, cookiecon);
                            if (pagetext != null)
                            {
                                var searchPage = Tedata.CheckSearchPage(pagetext);
                                if (searchPage)
                                {
                                    var custStatus = Tedata.CheckCustomerStatus(pagetext);
                                    if (custStatus == "disable")
                                    {
                                        Div1.Visible = true;
                                        Div1.InnerHtml += "هذا العميل : تليفون : " + wor.CustomerPhone + " موقوف بالفعل على البورتال <br />";
                                        Div1.Attributes.Add("class", "alert alert-danger");
                                        continue;
                                    }
                                    else
                                    {
                                        var worNote = Tedata.SendTedataSuspendRequest(username, cookiecon, pagetext);
                                        if (worNote == 2)
                                        {
                                            //فى حالة البورتال واقع
                                            Div1.Visible = true;
                                            Div1.InnerHtml += "لم يتم ارسال طلب إيقاف هذا العميل :" + wor.CustomerPhone + " بسبب تعذر الوصول الى البورتال <br />";
                                            Div1.Attributes.Add("class", "alert alert-danger");
                                            note =
                                                 "لم يتم ارسال الطلب إلى البورتال بسبب عدم إستجابة البورتال";
                                            //ينزل الطلب معلق فى اى اس بى
                                        }
                                        else
                                        {
                                            //فى حالة نجاح الارسال الى البورتال ننزل الطلب متوافق علية فى اى اس بى
                                            WorkOrderRequest req = new WorkOrderRequest();
                                            req.WorkOrderID = wor.ID;
                                            req.RequestID = 2;

                                            req.ConfirmerID = 1;
                                            req.ProcessDate = DateTime.Now.AddHours();
                                            req.RSID = 1;
                                            req.RequestDate = DateTime.Now.AddHours();
                                            req.SenderID = Convert.ToInt32(Session["User_Id"]);
                                            req.CurrentPackageID = wor.ServicePackageID;
                                            req.NewPackageID = wor.ServicePackageID;
                                            req.NewIpPackageID = wor.IpPackageID;
                                            req.Notes = "تم إيقاف العميل ";
                                            context.WorkOrderRequests.InsertOnSubmit(req);
                                            context.SubmitChanges();

                                            //تغيير الحالة الى(WorkOrders,WorkOrderStatus) suspend
                                            var current = context.WorkOrders.FirstOrDefault(x => x.ID == id);

                                            if (current != null)
                                            {
                                                current.WorkOrderStatusID = 11;

                                                global::Db.WorkOrderStatus wos = new global::Db.WorkOrderStatus
                                                {
                                                    WorkOrderID = current.ID,
                                                    StatusID = 11,
                                                    UserID = Convert.ToInt32(Session["User_Id"]),
                                                    UpdateDate = DateTime.Now.AddHours(),
                                                };
                                                context.WorkOrderStatus.InsertOnSubmit(wos);
                                            }

                                            context.SubmitChanges();

                                            DivSuccess.Visible = true;
                                            DivSuccess.InnerHtml += "تم إرسال طلب العميل : " + wor.CustomerPhone + " الى البورتال بنجاح <br />";
                                            DivSuccess.Attributes.Add("class", "alert alert-success");
                                            PortalSentCount += 1;
                                            continue;
                                        }

                                    }

                                }
                                else
                                {
                                    //فى حالة البورتال واقع
                                    Div1.Visible = true;
                                    Div1.InnerHtml += "لم يتم ارسال هذا الطلب " + wor.CustomerPhone + " بسبب تعذر الوصول الى البورتال الرجاء تأكد من حقل (User Name) <br />";
                                    Div1.Attributes.Add("class", "alert alert-danger");
                                    note = "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
                                }

                            }
                            else
                            {
                                Div1.Visible = true;
                                Div1.InnerHtml += "لم يتم ارسال طلب إيقاف هذا العميل :" + wor.CustomerPhone + " بسبب تعذر الوصول الى البورتال <br />";
                                Div1.Attributes.Add("class", "alert alert-danger");
                                note = "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
                            }
                        }
                        else
                        {
                            Div1.Visible = true;
                            Div1.InnerHtml += "فشل ارسال هذا الطلب " + wor.CustomerPhone + " بسبب فشل الأتصال بالسيرفر رجاءً تأكد من Portal User Name or Portal Password <br />";
                            Div1.Attributes.Add("class", "alert alert-danger");
                            note = "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
                        }
                    }
                    else
                    {
                        //user name null
                        Div1.Visible = true;
                        Div1.InnerHtml += "لم يتم ارسال هذا الطلب " + wor.CustomerPhone + " بسبب تعذر الوصول الى البورتال الرجاء تأكد من حقل (User Name) <br />";
                        Div1.Attributes.Add("class", "alert alert-danger");
                        note = "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
                    }
                }
                var orderRequests = context.WorkOrderRequests
                       .Where(woreq => woreq.WorkOrderID == id && woreq.RSID == 3);
                if (orderRequests.Any())
                {
                    Div1.Visible = true;
                    Div1.InnerHtml += "فشل إتمام هذا الطلب العميل : " + wor.CustomerPhone + " لديه طلبات معلقة لم يتم الموافقة عليها <br />";
                    Div1.Attributes.Add("class", "alert alert-danger");
                    //PortalnotSentCount += 1;
                    continue;
                }
                var worc = context.WorkOrders.FirstOrDefault(z => z.ID == id);
                WorkOrderRequest cReq = new WorkOrderRequest();
                if (worc != null)
                {
                    cReq.WorkOrderID = worc.ID;
                    cReq.RequestID = 2;
                    cReq.RSID = 3;
                    cReq.RequestDate = DateTime.Now.AddHours();
                    cReq.SenderID = Convert.ToInt32(Session["User_Id"]);
                    cReq.CurrentPackageID = worc.ServicePackageID;
                    cReq.NewPackageID = worc.ServicePackageID;
                    cReq.NewIpPackageID = worc.IpPackageID;
                    cReq.Notes += note;
                    context.WorkOrderRequests.InsertOnSubmit(cReq);
                    context.SubmitChanges();
                    PortalnotSentCount += 1;
                    SysSentCount += 1;
                    DivCount.Visible = true;
                    DivCount.InnerHtml += " تم إضافة هذا الطلب " + worc.CustomerPhone + " على السيستم الداخلى بنجاح <br />";
                    DivCount.Attributes.Add("class", "alert alert-info");
                }

            }


        }
        if (isPortalEnable)
        {
            DivCount.Visible = true;
            DivCount.InnerHtml += "تم إرسال " + PortalSentCount.ToString() + " طلب بنجاح الى البورتال || لم يتم ارسال " + PortalnotSentCount.ToString() + " طلب الى البورتال <br/>";
            DivCount.InnerHtml += "تم إرسال " + SysSentCount.ToString() + " طلب بنجاح الى السيستم الداخلى || لم يتم ارسال " + SysSentCount.ToString() + " طلب الى السيستم الداخلى";
            DivCount.Attributes.Add("class", "alert alert-info");
        }
        else
        {
            DivCount.Visible = true;
            DivCount.InnerHtml += "تم إضافة " + PortalnotSentCount.ToString() + " طلب بنجاح ";
            DivCount.Attributes.Add("class", "alert alert-info");
        }

        //string funcCall = "<script language='javascript'>$('#pop-me-up').modal('show');</script>";
        //if (!ClientScript.IsStartupScriptRegistered("JSScript"))
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "JSScript", funcCall);
        //}
    }
}

}