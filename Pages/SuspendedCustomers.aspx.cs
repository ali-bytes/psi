using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using NewIspNL.Services;

namespace NewIspNL.Pages
{
    public partial class SuspendedCustomers : CustomPage
    {



    readonly WorkOrderRepository _orderRepository = new WorkOrderRepository();
    private readonly IspEntries _ispEntries;

    public SuspendedCustomers()
    {
        
        _ispEntries = new IspEntries();
    }


    protected void Page_Load(object sender, EventArgs e){
        Div1.Visible = false;
        DivSuccess.Visible = false;
        DivCount.Visible = false;
        if (!IsPostBack)
        {
            PopulateGrd();
         
        }
    }

    void PopulateGrd()
    {
       
       
        if(Session["User_Id"]==null)return;
        var userWorkOrders = _orderRepository.GetUserWorkOrders(Convert.ToInt32(Session["User_Id"]));
        var suspendedCustomers = userWorkOrders
            .Where(o => o.Status.ID == 11 && o.WorkOrderStatus.Any())
            .Select(
                    w =>
                    {
                        var user = w.User;
                        var dateTime = w.WorkOrderStatus.Max(s => s.UpdateDate);
                        if (dateTime != null)
                            return new
                            {
                                w.ID,
                                w.CustomerName,
                                w.CustomerPhone,
                                w.Governorate.GovernorateName,
                                Provider = w.ServiceProvider.SPName,
                                Branch = w.Branch.BranchName,
                                Resseller = user == null ? "-" : user.UserName,
                                Requestdate =
                                    dateTime.Value.Date.ToShortDateString(),
                            
                                Days=_ispEntries.DaysForCustomerAtStatus(w.ID,11),
                                req=w.WorkOrderRequests.Any()
                                //susDate = _ispEntries.LastWorkOrderStatus(w.ID,11,null,null).UpdateDate
                            };
                        return null;
                    })
            .ToList();
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var option = OptionsService.GetOptions(context, true);
            if (option == null) return;
            var overRangeCustomers = suspendedCustomers.Where(o => o.Days > option.SuspendDaysCount).ToList();
            gv_customers.DataSource = suspendedCustomers;
            gv_customers.DataBind();
            if (overRangeCustomers.Any(s => s.Days > option.SuspendDaysCount))
            {
                count.Value = "true";
                GridView1.DataSource = overRangeCustomers;
                GridView1.DataBind();
            }
        }
        
    }
  

    //protected void btn_cancelAll_Click(object sender, EventArgs e)
    //{
    //    Div1.InnerHtml = "";
    //    DivSuccess.InnerHtml = "";
    //    CookieContainer cookiecon = null;
    //    int PortalSentCount = 0;
    //    int PortalnotSentCount = 0;
    //    bool isPortalEnable = false;
    //    foreach (GridViewRow row in GridView1.Rows)
    //    {
    //        var control = row.FindControl("selecttocancel") as CheckBox;
    //        if (control == null || !control.Checked) continue;
    //        var dataKey = GridView1.DataKeys[row.RowIndex];
    //        if (dataKey == null) continue;
    //        var id = Convert.ToInt32(dataKey["ID"]);
    //        // حط هنا شرك انه مفعل الابوشن والا لأ .. والا حاطة هناك فى كلاس الربط ؟؟
    //        //get from WorkOrder
    //        var note = "";
    //        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
    //        {
    //            var portalList = context.OptionPortalProviders.Select(z => z.PortalProvidersId).ToList();
    //            var wor = context.WorkOrders.FirstOrDefault(z => z.ID == id);
    //            if (wor != null && portalList.Contains(wor.ServiceProviderID))
    //            {
    //                isPortalEnable = true;
    //                var username = wor.UserName;
    //                if (username.Length>0)
    //                {
    //                if (cookiecon==null)
    //                {
    //                    cookiecon = new CookieContainer();
    //                    cookiecon = Tedata.Login();
    //                }
                   

    //                if (cookiecon != null)
    //                {
    //                    var pagetext = Tedata.GetSearchPage(username, cookiecon);
    //                    if (pagetext != null)
    //                    {
    //                        var searchPage = Tedata.CheckSearchPage(pagetext);
    //                        if (searchPage)
    //                        {
    //                            var custStatus = Tedata.CheckIfCancelPage(pagetext);
    //                            if (custStatus)
    //                            {
    //                                Div1.Visible = true;
    //                                Div1.InnerHtml += "هذا العميل : تليفون : " + wor.CustomerPhone+ "حالتة كانسل بالفعل على البورتال <br />";
    //                                Div1.Attributes.Add("class", "alert alert-danger");
    //                                continue;
    //                            }
    //                            else
    //                            {
    //                                var worNote = Tedata.SendTedataCancelRequest(username, cookiecon, pagetext);
    //                                if (worNote == 2)
    //                                {
    //                                    //فى حالة البورتال واقع
    //                                    Div1.Visible = true;
    //                                    Div1.InnerHtml += "لم يتم ارسال طلب كانسل هذا العميل :" + wor.CustomerPhone + " بسبب تعذر الوصول الى البورتال <br />";
    //                                    Div1.Attributes.Add("class", "alert alert-danger");
    //                                   note =
    //                                        "لم يتم ارسال الطلب إلى البورتال بسبب عدم إستجابة البورتال";
    //                                    //ينزل الطلب معلق فى اى اس بى
    //                                }
    //                                else
    //                                {
    //                                    //فى حالة نجاح الارسال الى البورتال ننزل الطلب متوافق علية فى اى اس بى
    //                                    WorkOrderRequest req=new WorkOrderRequest();
    //                                    req.WorkOrderID = wor.ID;
    //                                    req.RequestID = 6;

    //                                    req.ConfirmerID = 1;
    //                                    req.ProcessDate = DateTime.Now.AddHours();
    //                                    req.RSID = 1;

    //                                    req.SenderID = Convert.ToInt32(Session["User_Id"]);
    //                                    req.CurrentPackageID = wor.ServicePackageID;
    //                                    req.NewPackageID = wor.ServicePackageID;
    //                                    req.NewIpPackageID = wor.IpPackageID;
    //                                    req.Notes = "تم الغاء العميل ";
    //                                    context.WorkOrderRequests.InsertOnSubmit(req);
    //                                    context.SubmitChanges();

    //                                    //تغيير الحالة الى(WorkOrders,WorkOrderStatus) suspend
    //                                    var current = context.WorkOrders.FirstOrDefault(x => x.ID == id);

    //                                    if (current != null)
    //                                    {
    //                                        current.WorkOrderStatusID = 8;

    //                                        global::Db.WorkOrderStatus wos = new global::Db.WorkOrderStatus
    //                                        {
    //                                            WorkOrderID = current.ID,
    //                                            StatusID = 8,
    //                                            UserID = Convert.ToInt32(Session["User_Id"]),
    //                                            UpdateDate = DateTime.Now.AddHours(),
    //                                        };
    //                                        context.WorkOrderStatus.InsertOnSubmit(wos);
    //                                    }

    //                                    context.SubmitChanges();

    //                                    DivSuccess.Visible = true;
    //                                    DivSuccess.InnerHtml += "تم إرسال طلب العميل : " + wor.CustomerPhone + " الى البورتال بنجاح <br />";
    //                                    DivSuccess.Attributes.Add("class", "alert alert-success");
    //                                    PortalSentCount += 1;
    //                                    continue;
    //                                }

    //                            }

    //                        }
    //                        else
    //                        {
    //                            //فى حالة البورتال واقع
    //                            Div1.Visible = true;
    //                            Div1.InnerHtml += "لم يتم ارسال هذا الطلب " + wor.CustomerPhone + " بسبب تعذر الوصول الى البورتال الرجاء تأكد من حقل (User Name) <br />";
    //                            Div1.Attributes.Add("class", "alert alert-danger");
    //                            note = "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
    //                        }

    //                    }
    //                    else
    //                    {
    //                        Div1.Visible = true;
    //                        Div1.InnerHtml += "لم يتم ارسال طلب كانسل هذا العميل :" + wor.CustomerPhone + " بسبب تعذر الوصول الى البورتال <br />";
    //                        Div1.Attributes.Add("class", "alert alert-danger");
    //                        note = "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
    //                    }
    //                }
    //                else
    //                {
    //                     Div1.Visible = true;
    //                     Div1.InnerHtml += "فشل ارسال هذا الطلب " + wor.CustomerPhone + " بسبب فشل الأتصال بالسيرفر رجاءً تأكد من Portal User Name or Portal Password <br />";
    //                    Div1.Attributes.Add("class", "alert alert-danger");
    //                    note = "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
    //               }
    //                }
    //                else
    //                {
    //                    //user name null
    //                    Div1.Visible = true;
    //                    Div1.InnerHtml += "لم يتم ارسال هذا الطلب " + wor.CustomerPhone + " بسبب تعذر الوصول الى البورتال الرجاء تأكد من حقل (User Name) <br />";
    //                    Div1.Attributes.Add("class", "alert alert-danger");
    //                    note = "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
    //                }
    //            }
    //            var orderRequests = context.WorkOrderRequests
    //                   .Where(woreq => woreq.WorkOrderID == id && woreq.RSID == 3);
    //            if (orderRequests.Any())
    //            {
    //                Div1.Visible = true;
    //                Div1.InnerHtml += "فشل إتمام هذا الطلب العميل : " + wor.CustomerPhone + " لديه طلبات معلقة لم يتم الموافقة عليها <br />";
    //                Div1.Attributes.Add("class", "alert alert-danger");
    //                PortalnotSentCount += 1;
    //                continue;
    //            }
    //            var worc = context.WorkOrders.FirstOrDefault(z => z.ID == id);
    //            WorkOrderRequest cReq = new WorkOrderRequest();
    //            if (worc!=null)
    //            {
    //                cReq.WorkOrderID = worc.ID;
    //                cReq.RequestID = 6;
    //                cReq.RSID = 3;
    //                cReq.SenderID = Convert.ToInt32(Session["User_Id"]);
    //                cReq.CurrentPackageID = worc.ServicePackageID;
    //                cReq.NewPackageID = worc.ServicePackageID;
    //                cReq.NewIpPackageID = worc.IpPackageID;
    //                cReq.Notes = note;
    //                context.WorkOrderRequests.InsertOnSubmit(cReq);
    //                context.SubmitChanges();
    //                PortalnotSentCount += 1;
    //            }
               
    //        }
            

    //    }
    //    if (isPortalEnable)
    //    {
    //        DivCount.Visible = true;
    //        DivCount.InnerHtml = "تم إرسال " + PortalSentCount.ToString() + " طلب بنجاح الى البورتال || لم يتم ارسال " + PortalnotSentCount.ToString() + " طلب الى البورتال";
    //        DivCount.Attributes.Add("class", "alert alert-info");
    //    }
    //    else
    //    {
    //        DivCount.Visible = true;
    //        DivCount.InnerHtml = "تم إضافة " + PortalnotSentCount.ToString() + " طلب بنجاح ";
    //        DivCount.Attributes.Add("class", "alert alert-info");
    //    }
       
    //    string funcCall = "<script language='javascript'>$('#pop-me-up').modal('show');</script>";
    //    if (!ClientScript.IsStartupScriptRegistered("JSScript"))
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "JSScript", funcCall);
    //    } 
    //}
        protected void btn_cancelAll_Click(object sender, EventArgs e)
        {
            DivSuccess.InnerHtml = "";
            Div1.InnerHtml = "";

            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                foreach (GridViewRow row in GridView1.Rows)
                {
                    var control = row.FindControl("selecttocancel") as CheckBox;
                    if (control == null || !control.Checked) continue;
                    var dataKey = GridView1.DataKeys[row.RowIndex];
                    if (dataKey == null) continue;
                    var id = Convert.ToInt32(dataKey["ID"]);
                    var wor = context.WorkOrders.FirstOrDefault(z => z.ID == id);
                    var orderRequests = context.WorkOrderRequests
                        .Where(woreq => woreq.WorkOrderID == id && woreq.RSID == 3);
                    if (orderRequests.Any())
                    {
                        Div1.Visible = true;
                        Div1.InnerHtml += "فشل إتمام هذا الطلب العميل : " + wor.CustomerPhone +
                                          " لديه طلبات معلقة لم يتم الموافقة عليها <br />";
                        Div1.Attributes.Add("class", "alert alert-danger");
                        continue;
                    }
                    var worc = context.WorkOrders.FirstOrDefault(z => z.ID == id);
                    WorkOrderRequest cReq = new WorkOrderRequest();
                    if (worc != null)
                    {
                        cReq.WorkOrderID = worc.ID;
                        cReq.RequestID = 6;
                        cReq.RSID = 3;
                        cReq.SenderID = Convert.ToInt32(Session["User_Id"]);
                        cReq.CurrentPackageID = worc.ServicePackageID;
                        cReq.NewPackageID = worc.ServicePackageID;
                        cReq.NewIpPackageID = worc.IpPackageID;
                        cReq.Notes = "طلب الغاء من صفحة عملاء موقوفين";
                        context.WorkOrderRequests.InsertOnSubmit(cReq);
                        context.SubmitChanges();
                        DivSuccess.Visible = true;
                        DivSuccess.InnerHtml += "تم إضافة طلب بنجاح  للعميل  " + wor.CustomerPhone + "  <br />";
                        DivSuccess.Attributes.Add("class", "alert alert-success");
                    }
                }
            }
        }

        protected void gv_customers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Helper.GridViewNumbering(gv_customers, "gv_l_number");
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Helper.GridViewNumbering(GridView1, "gv_l_number");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[9].Text.Trim() == "True")
            {
                e.Row.Cells[9].Text ="Yes" ;
            }
            if (e.Row.Cells[9].Text.Trim() == "False")
            {
                e.Row.Cells[9].Text ="No" ;
            }
        }
    }
}

}