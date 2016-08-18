using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;
using NewIspNL.Helpers;
using NewIspNL.Models;
using NewIspNL.Services.DemandServices;
using Resources;

namespace NewIspNL.Pages
{
    public partial class CreatInvoices : CustomPage
    {
       
            readonly DemandsSearchService _searchService;
            private readonly IspEntries _ispEntries;

            public  CreatInvoices()
            {
                var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
                _searchService = new DemandsSearchService(context);
                _ispEntries = new IspEntries(context);
            }
            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                PopulateProvider();
                PrepareInputs();
            }
            void PopulateProvider()
            {
                var providers = _ispEntries.ServiceProviders();
                providerlist.DataSource = providers;
                providerlist.DataTextField = "SPName";
                providerlist.DataValueField = "ID";
                providerlist.DataBind();
            }
            void PrepareInputs()
            {
               
                var currentYear = DateTime.Now.Year;
                Helper.PopulateDrop(Helper.FillYears(currentYear - 5, currentYear + 2).OrderBy(x => x), DdlYear);
                Helper.PopulateMonths(DdlMonth);
            }

            protected void CreateInvoice(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var resList = new List<User>();
                    var resellers = context.Users.Where(a => a.GroupID == 6).ToList();
                    foreach (var res in resellers)
                    {
                        var searchDemands = _searchService.SearchDemandsToPreview(new BasicSearchModel
                        {
                            Paid = false,
                            ResellerId = res.ID,//Helper.GetDropValue(DdlReseller),
                            Month = Helper.GetDropValue(DdlMonth),
                            Year = Helper.GetDropValue(DdlYear),
                            WithResellerDiscount = true,
                        });
                        var newlist = new List<DemandPreviewModel>();
                        if (providerlist.Items.Count > 0)
                        {
                            var totalcount = providerlist.Items.Cast<ListItem>().Count(item => item.Selected);
                            if (totalcount == 0) { Msg.InnerText = Tokens.SelectServiceProvider; continue; }
                            foreach (ListItem item in providerlist.Items)
                            {

                                if (item.Selected)
                                {
                                    var item1 = item;
                                    var data = searchDemands.Where(a => a.Provider == item1.ToString()).ToList();
                                    newlist.AddRange(data);
                                }
                            }
                        }
                        else
                        {
                            newlist = searchDemands;
                        }

                        if (!newlist.Any())
                        {
                            resList.Add(res);

                            continue;
                        }

                        IspDataContext.SubmitChanges();
                        var total = newlist.Sum(x => x.ResellerNet); //Amount);
                        var resellerId = res.ID;//Convert.ToInt32(DdlReseller.SelectedItem.Value);
                        var reseller = res;//context.Users.FirstOrDefault(x => x.ID == resellerId);
                        //if (reseller == null) return;
                        string completePath = HttpContext.Current.Server.MapPath("~/ExcelTemplates/ResellerPaidDemands.xls");
                        var time = DateTime.Now.AddHours();
                        var timeName = time.Day + "_" + time.Month + "_" + time.Year + "_" + time.Hour + "_" + time.Minute + "_" +
                                       time.Millisecond;

                        if (File.Exists(completePath))
                            File.Copy(HttpContext.Current.Server.MapPath("~/ExcelTemplates/ResellerPaidDemands.xls"),
                                HttpContext.Current.Server.MapPath(
                                    string.Format("~/ExcelTemplates/ResselerPaidDemands/{0}_{1}.xls", resellerId, timeName)),
                                true);

                        var currentExtension = Path.GetExtension("~/ExcelTemplates/temp/AccountStatment.xls");
                        var connection = new OleDbConnection();
                        switch (currentExtension)
                        {
                            case ".xls":
                                connection.ConnectionString =
                                    string.Format(
                                        @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                                        HttpContext.Current.Server.MapPath("~\\ExcelTemplates\\ResselerPaidDemands\\") +
                                        "\\{0};Extended Properties='Excel 8.0;HDR=Yes;'",
                                        string.Format("{0}_{1}.xls", resellerId, timeName));
                                break;
                        }
                        try
                        {
                            for (int i = 0; i < newlist.Count; i++)
                            {
                                var myCommand = new OleDbCommand
                                {
                                    Connection = connection
                                };
                                const string sql = "Insert into [Sheet1$] values(@a,@b,@j,@c,@d,@e,@f,@g,@h,@i,@k,@l)";
                                myCommand.CommandText = sql;
                                connection.Open();
                                var demand = newlist[i];
                                myCommand.Parameters.AddWithValue("@a", demand.Customer);
                                myCommand.Parameters.AddWithValue("@b", demand.Phone);
                                myCommand.Parameters.AddWithValue("@j", demand.servicepack);
                                myCommand.Parameters.AddWithValue("@c", demand.Provider);
                                myCommand.Parameters.AddWithValue("@d", reseller.UserName);
                                myCommand.Parameters.AddWithValue("@e", demand.Central);
                                myCommand.Parameters.AddWithValue("@f", demand.Governorate);
                                myCommand.Parameters.AddWithValue("@g", demand.Offer);
                                myCommand.Parameters.AddWithValue("@h", Convert.ToDecimal(demand.TResellerNet).ToString(CultureInfo.InvariantCulture));
                                myCommand.Parameters.AddWithValue("@i", demand.TStartAt);
                                myCommand.Parameters.AddWithValue("@k", demand.TEndAt);
                                myCommand.Parameters.AddWithValue("@l", demand.Notes);
                                myCommand.ExecuteNonQuery();
                                connection.Close();
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        //check Commission of Reseller
                        var userId = Convert.ToInt32(Session["User_ID"]);
                        string note = "اصدار فاتورة موزع";
                        if (DdlMonth.SelectedIndex > 0) note += string.Format("عن شهر {0}", DdlMonth.SelectedItem.Text);
                        if (DdlYear.SelectedIndex > 0) note += string.Format("عن سنة {0}", DdlYear.SelectedItem.Text);
                        var transaction = new UsersTransaction
                        {
                            ResellerID = resellerId,
                            DepitAmmount = Convert.ToDouble(total),
                            Total =
                                Billing.GetLastBalance(resellerId, "Reseller") +
                                Convert.ToDouble(newlist.Sum(x => x.ResellerNet)),
                            CreditAmmount = 0,
                            CreationDate = time,
                            IsInvoice = true,
                            IsPaid = false,
                            Description = "Invoice",
                            Notes = "اصدار فاتورة بتاريخ" + time.ToShortDateString() + note,
                            UserId = userId,
                            FileUrl = string.Format("{0}_{1}.xls", resellerId, timeName)
                        };
                        context.UsersTransactions.InsertOnSubmit(transaction);
                        //newlist.ForEach(x => x.Demand.Paid = true);

                        foreach (var list in newlist)
                        {
                            var demand = context.Demands.FirstOrDefault(a => a.Id == list.Id);
                            if (demand != null && demand.StartAt.Date == Convert.ToDateTime(list.TStartAt).Date && demand.EndAt.Date == Convert.ToDateTime(list.EndAt).Date)
                            {
                                demand.Paid = true;
                                demand.PaymentDate = DateTime.Now.AddHours();
                                demand.UserId = userId;
                                demand.PaymentComment = note;
                                context.SubmitChanges();
                            }
                        }
                        IspDataContext.SubmitChanges();
                        context.SubmitChanges();
                        Msg.InnerHtml = Tokens.Saved;
                        //ResetPage();
                        var smsdata = context.SMSCnfgs.FirstOrDefault();
                        var messagetext = context.SMSCaseNotifications.FirstOrDefault(a => a.Id == 3);
                        var mobile = reseller.UserMobile;
                        if (smsdata != null && messagetext != null)
                        {
                            var message = SendSms.Send(smsdata.UserName, smsdata.Password, mobile, messagetext.Message,
                                smsdata.Sender, smsdata.UrlAPI);
                            var myscript = "window.open('" + message + "')";
                            ClientScript.RegisterClientScriptBlock(typeof(Page), "myscript", myscript, true);
                            //var message = SendSMS.Send(userId,password, mobile, txtMeaageText.Text, senderS, data.UrlAPI);
                            /*Response.Write("<html><head></head><script type='text/javascript'>mywin= window.open('" + message +
                                           "','_blank');</script><body onload='window.close();'></body></html> ");*/
                        }
                    }
                    GVResellers.DataSource = resList;
                    GVResellers.DataBind();
                }
            }
            protected void NumberGrid(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(GVResellers, "LNo");
            }
            protected void SearchDemands(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    resellerDiv.Visible = true;
                    //var resList = new List<User>();
                    var resellers = context.Users.Where(a => a.GroupID == 6).ToList();
                    var newlist = new List<DemandPreviewModel>();
                    var resellerList = new List<QulaifiedData>();
                    foreach (var res in resellers)
                    {
                        var searchDemands = _searchService.SearchDemandsToPreview(new BasicSearchModel
                        {
                            Paid = false,
                            ResellerId = res.ID, //Helper.GetDropValue(DdlReseller),
                            Month = Helper.GetDropValue(DdlMonth),
                            Year = Helper.GetDropValue(DdlYear),
                            WithResellerDiscount = true,
                        });

                        if (providerlist.Items.Count > 0)
                        {
                            var totalcount = providerlist.Items.Cast<ListItem>().Count(item => item.Selected);
                            if (totalcount == 0)
                            {
                                Msg.InnerText = Tokens.SelectServiceProvider;
                                continue;
                            }
                            foreach (ListItem item in providerlist.Items)
                            {

                                if (item.Selected)
                                {
                                    var item1 = item;
                                    var data = searchDemands.Where(a => a.Provider == item1.ToString()).ToList();
                                    newlist.AddRange(data);
                                }
                            }
                        }
                        else
                        {
                            newlist = searchDemands;
                        }

                        /* if (!newlist.Any())
                         {
                             resList.Add(res);

                             continue;
                         }*/

                        //IspDataContext.SubmitChanges();
                        var total = newlist.Sum(x => x.ResellerNet); //Amount);

                        byte[] bytes = Encoding.Unicode.GetBytes(res.UserName);
                        byte[] utf8Bytes = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, bytes);
                        var myString = Encoding.UTF8.GetString(utf8Bytes);

                        byte[] bytes1 = Encoding.Unicode.GetBytes(res.Branch.BranchName);
                        byte[] utf8Bytes1 = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, bytes1);
                        var myString1 = Encoding.UTF8.GetString(utf8Bytes1);


                        var ld = new QulaifiedData
                        {

                            Reseller = myString,
                            Branch = myString1,
                            Total = total
                        };
                        resellerList.Add(ld);
                        newlist.Clear();
                        //var resellerId = res.ID; //Convert.ToInt32(DdlReseller.SelectedItem.Value);
                        //var reseller = res;

                    }
                    GridView1.DataSource = resellerList;
                    GridView1.DataBind();

                    GridHelper.ExportOneGrid("ResellerInvoices", GridView1);
                    resellerDiv.Visible = false;
                }
            }

        }
    }
 