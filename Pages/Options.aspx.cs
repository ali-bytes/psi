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
using NewIspNL.Services;
using Resources;
using NewIspNL.Helpers;
using ISPDataContext = Db.ISPDataContext;
using OptionInvoiceProvider = Db.OptionInvoiceProvider;
using OptionProvider = Db.OptionProvider;
using OptionSuspendProvider = Db.OptionSuspendProvider;
using WorkOrder = Db.WorkOrder;
using WorkOrderRequest = Db.WorkOrderRequest;

namespace NewIspNL.Pages
{
    public partial class Options : CustomPage
    {
       

        protected void Page_Load(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                Activate();
                if (IsPostBack) return;
                var context2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
                var type = context2.Options.FirstOrDefault();
                foreach (ListItem fawrytype in checkfawrytype.Items)
                {
                    if (fawrytype.Value == type.FawryType)
                    {
                        fawrytype.Selected = true;
                    }
                }



                PopulateTypes();
                PopulateProvider();

                PopulateUser();
                var option = OptionsService.GetOptions(context, false);
                if (option == null) return;
                TbSuspendDaysCount.Text = string.Format("{0}", option.SuspendDaysCount);
                TbTimeDifference.Text = option.TimeDifference.ToString();
                CbIncludeGov.Checked = option.IncludeGovernorateInSearch;
                CbMergeGovPhone.Checked = option.MergeGovernorateWithPhoneInCreateCustomer;
                CheckDiscound.Checked = option.DiscoundFromBranchCredit;
                CheckDiscoundfrombranchandReseller.Checked = option.DiscoundfromResellerandBranch;
                Checkminuscredit.Checked = option.Allowminuscredit;
                checkUpload.Checked = option.UploadFielsToNewCustomer;
                CheckStatistic.Checked = option.ShowStatistic;
                checkInstallationDiscound.Checked = option.IntallationDiscound;
                CheckSendMessage.Checked = Convert.ToBoolean(option.SendMessageAfterOperations);
                CheckShowRequestInSearch.Checked = Convert.ToBoolean(option.ShowRequestsInSearch);
                CheckCounter.Checked = Convert.ToBoolean(option.ShowCounters);
                CheckActivatephone.Checked = Convert.ToBoolean(option.ValidationOnCustomerPhone);
                CheckAllDemand.Checked = Convert.ToBoolean(option.ShowAllDemandOfPR);
                CheckFawry.Checked = Convert.ToBoolean(option.FawryService);
                AutoSusCustomerUnderReseller.Checked = Convert.ToBoolean(option.AutoSuspendCustomersUnderReseller);
                var alert = option.AlertWayOfUnpaidDemand;
                checkUnpaidDemand.SelectedValue = alert.ToString();
                DdTypesFrom.SelectedValue = option.ConvertFromPackageType.ToString();
                DdTypesTo.SelectedValue = option.ConvertToPackageType.ToString();
                txtPrice.Text = option.ConversionDebt.ToString();
                chkbxShowDedWithFix.Checked = Convert.ToBoolean(option.ShowDeductionWithFixedRequestDateInCD);
                chkbxPreventUnsus.Checked = Convert.ToBoolean(option.PreventUnsusForCustomerHasIndebtedness);
                txtPortalRelayDays.Text = option.PortalRelayDays==null?"": option.PortalRelayDays.ToString();
                // Reseller Payments 
                txtday1.Text = option.Day1.ToString();
                txtday2.Text = option.Day2.ToString();
                txtday3.Text = option.Day3.ToString();
                txtday4.Text = option.Day4.ToString();
                txtday5.Text = option.Day5.ToString();
                txtday6.Text = option.Day6.ToString();
                txtpercentage1.Text = option.Percentage1.ToString();
                txtpercentage2.Text = option.Percentage2.ToString();
                txtpercentage3.Text = option.Percentage3.ToString();
                txtpercentage4.Text = option.Percentage4.ToString();
                txtpercentage5.Text = option.Percentage5.ToString();
                txtpercentage6.Text = option.Percentage6.ToString();

                //auto payment
                autoreseller.Checked =option.AutoResellerPayment != null && Convert.ToBoolean(option.AutoResellerPayment);
                autobranch.Checked = option.AutoBranchPayment != null && Convert.ToBoolean(option.AutoBranchPayment);
                //Prevent suspend before month from reactive
                cbSusAfterMonth.Checked = option.PreventSuspendBeforeMonthFromReActive != null && Convert.ToBoolean(option.PreventSuspendBeforeMonthFromReActive);



                //-- check box Reseller Payment Installments
                bool chkPActive = option.IsResellerPaymentActive ?? false;
                if (chkPActive)
                {
                    ChkboxResellerdebt.Checked = true;
                    Resellerdebt.Attributes.Add("style", "display:block");

                }
                else
                {
                    ChkboxResellerdebt.Checked = false;
                    Resellerdebt.Attributes.Add("style", "display:none");
                }

                //-- populate Reseller Payments Installment checkbox

                var branchesList = context.BranchesForResellerPayments.ToList();
                if (branchesList.Count > 0)
                {
                    foreach (var lst in branchesList)
                    {
                        foreach (ListItem pr in ChkListBranchesForInstallments.Items)
                        {
                            if (lst.BranchId == int.Parse(pr.Value))
                            {
                                pr.Selected = true;
                            }
                        }
                    }
                }
                //----------------
                //-- populate pending requests checkbox

                var pList = context.ShowPendingRequestsOptions.ToList();
                if (pList.Count > 0)
                {
                   
                    ckPendingRequestsAll.Checked = true;
                    pendingRequestsDiv.Attributes.Add("style", "display:block");

                    foreach (var lst in pList)
                    {
                        foreach (ListItem pr in CkUnSuspend.Items)
                        {
                            if (lst.RequestType == "unsus" && lst.Name == pr.Value)
                            {
                                pr.Selected = true;
                            }
                        }
                        foreach (ListItem pr in CkSuspend.Items)
                        {
                            if (lst.RequestType == "sus" && lst.Name == pr.Value)
                            {
                                pr.Selected = true;
                            }
                        }
                        foreach (ListItem pr in CkUpDwonOptions.Items)
                        {
                            if (lst.RequestType == "updown" && lst.Name == pr.Value)
                            {
                                pr.Selected = true;
                            }
                        }
                    }
                }
                else
                {
                    ckPendingRequestsAll.Checked = false;
                    pendingRequestsDiv.Attributes.Add("style", "display:none");
                }
                //----------------

                if (alert == 2)
                {

                    txtRemindertext.Text = option.ReminderMessage;
                    var user = option.ReminderToUserId;
                    ddlUser.SelectedValue = (user != null && user != 0) ? user.ToString() : "-1";
                }
                else
                {

                }
                txtDaysCount.Text = option.DaysOfUnpaidDemandsLimit.ToString();
                if (option.FromDay != 0 && option.ToDay != 0)
                {
                    checkDuration.Checked = true;
                    txtFrom.Text = option.FromDay.ToString();
                    txtTo.Text = option.ToDay.ToString();
                }
                if (option.WidthOfReciept == false) CheckHalfReciepit.Checked = true;
                else
                {
                    CheckReciepit.Checked = true;
                }


                var foundedPortalproviders = context.OptionPortalProviders.ToList();
                foreach (var op in foundedPortalproviders)
                {
                    foreach (ListItem provi in portalTeData.Items)
                    {
                        if (op.ServiceProvider.ID == int.Parse(provi.Value))
                        {
                            provi.Selected = true;
                        }
                    }
                }





                var foundedproviders = context.OptionProviders.ToList();
                foreach (var op in foundedproviders)
                {
                    foreach (ListItem provi in providerlist.Items)
                    {
                        if (op.ServiceProvider.ID == int.Parse(provi.Value))
                        {
                            provi.Selected = true;
                        }
                    }
                }

                var foundedprovidersforreslleraccount = context2.SPoptionReselleraccounts.ToList();
                foreach (var op in foundedprovidersforreslleraccount)
                {
                    foreach (ListItem provi in CheckBoxList1.Items)
                    {
                        if (op.ServiceProvider.ID == int.Parse(provi.Value))
                        {
                            provi.Selected = true;
                        }
                    }
                }
                var foundedinvoiceprovider = context.OptionInvoiceProviders.ToList();
                foreach (var item in foundedinvoiceprovider)
                {
                    foreach (ListItem pr in InvoiceProviderList.Items)
                    {
                        if (item.ServiceProvider.ID == int.Parse(pr.Value))
                        {
                            pr.Selected = true;
                        }
                    }
                }
                var foundedsuspendprovider = context.OptionSuspendProviders.ToList();
                foreach (var it in foundedsuspendprovider)
                {
                    foreach (ListItem pr in CheckProviderSuspend.Items)
                    {
                        if (it.ServiceProvider.ID == int.Parse(pr.Value))
                        {
                            pr.Selected = true;
                        }
                    }
                }
            }
        }

        void PopulateTypes()
        {
            using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                PackagesRepository _packagesRepository = new PackagesRepository(dataContext);
                var packageTypes = _packagesRepository.PackageTypes();

                DdTypesFrom.DataSource = packageTypes;
                DdTypesFrom.DataTextField = "SPTName";
                DdTypesFrom.DataValueField = "ID";
                DdTypesFrom.DataBind();
                Helper.AddDefaultItem(DdTypesFrom, Tokens.Chose);

                DdTypesTo.DataSource = packageTypes;
                DdTypesTo.DataTextField = "SPTName";
                DdTypesTo.DataValueField = "ID";
                DdTypesTo.DataBind();
                Helper.AddDefaultItem(DdTypesTo, Tokens.Chose);
            }
        }
        void PopulateProvider()
        {
            using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var providers = dataContext.ServiceProviders.ToList();
                providerlist.DataSource = providers;
                providerlist.DataTextField = "SPName";
                providerlist.DataValueField = "ID";
                providerlist.DataBind();
                InvoiceProviderList.DataSource = providers;
                InvoiceProviderList.DataTextField = "SPName";
                InvoiceProviderList.DataValueField = "ID";
                InvoiceProviderList.DataBind();


                portalTeData.DataSource = providers;
                portalTeData.DataTextField = "SPName";
                portalTeData.DataValueField = "ID";
                portalTeData.DataBind();

                CheckProviderSuspend.DataSource = providers;
                CheckProviderSuspend.DataTextField = "SPName";
                CheckProviderSuspend.DataValueField = "ID";
                CheckProviderSuspend.DataBind();

                //Checkbox List Branches For Installments
                var branches = dataContext.Branches.ToList();
                ChkListBranchesForInstallments.DataSource = branches;
                ChkListBranchesForInstallments.DataTextField = "BranchName";
                ChkListBranchesForInstallments.DataValueField = "ID";
                ChkListBranchesForInstallments.DataBind();
                //-------------

                //todo:new task 28-4-2015
                CheckBoxList1.DataSource = providers;
                CheckBoxList1.DataTextField = "SPName";
                CheckBoxList1.DataValueField = "ID";
                CheckBoxList1.DataBind();
            }
        }

        private void PopulateUser()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                ddlUser.DataSource = context.Users.Where(a => a.GroupID != 6).ToList();
                ddlUser.DataTextField = "UserName";
                ddlUser.DataValueField = "ID";
                ddlUser.DataBind();

            }
        }


        void Activate()
        {
            BSave.ServerClick += (o, a) => Save();
        }


        void Save()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var context2 = new Db.ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
                var allprovider = context.OptionProviders.ToList();
                var allinvoiceprovider = context.OptionInvoiceProviders.ToList();
                var allsuspendprovider = context.OptionSuspendProviders.ToList();
                var allprospreseller = context2.SPoptionReselleraccounts.ToList();
                var allPortalProvider = context2.OptionPortalProviders.ToList();
                var allPendingRequests = context2.ShowPendingRequestsOptions.ToList();

                context2.OptionPortalProviders.DeleteAllOnSubmit(allPortalProvider);
                context.OptionProviders.DeleteAllOnSubmit(allprovider);
                context.OptionInvoiceProviders.DeleteAllOnSubmit(allinvoiceprovider);
                context.OptionSuspendProviders.DeleteAllOnSubmit(allsuspendprovider);
                context2.SPoptionReselleraccounts.DeleteAllOnSubmit(allprospreseller);
                context2.ShowPendingRequestsOptions.DeleteAllOnSubmit(allPendingRequests);
                context.SubmitChanges();


                //auto payment
                var option = OptionsService.GetOptions(context, false);
                option.AutoResellerPayment = autoreseller.Checked;
                option.AutoBranchPayment = autobranch.Checked;
                context.SubmitChanges();

                //pending request options
                foreach (ListItem item in CkUnSuspend.Items)
                {
                    if (item.Selected)
                    {
                        var rq = new ShowPendingRequestsOption
                        {
                            Name = item.Value,
                            RequestType = "unsus",
                            Show = true
                        };
                        context.ShowPendingRequestsOptions.InsertOnSubmit(rq);
                        context.SubmitChanges();
                    }
                }
                foreach (ListItem item in CkSuspend.Items)
                {
                    if (item.Selected)
                    {
                        var rq = new ShowPendingRequestsOption
                        {
                            Name = item.Value,
                            RequestType = "sus",
                            Show = true
                        };
                        context.ShowPendingRequestsOptions.InsertOnSubmit(rq);
                        context.SubmitChanges();
                    }
                }
                foreach (ListItem item in CkUpDwonOptions.Items)
                {
                    if (item.Selected)
                    {
                        var rq = new ShowPendingRequestsOption
                        {
                            Name = item.Value,
                            RequestType = "updown",
                            Show = true
                        };
                        context.ShowPendingRequestsOptions.InsertOnSubmit(rq);
                        context.SubmitChanges();
                    }
                }
                //------
                foreach (ListItem item in providerlist.Items)
                {
                    if (item.Selected)
                    {
                        var providers = new OptionProvider
                        {
                            ProviderId = int.Parse(item.Value)
                        };
                        context.OptionProviders.InsertOnSubmit(providers);
                        context.SubmitChanges();
                    }
                }
                foreach (ListItem item in CheckBoxList1.Items)
                {
                    if (item.Selected)
                    {
                        var providers = new SPoptionReselleraccount
                        {
                            ProviderId = int.Parse(item.Value)
                        };
                        context2.SPoptionReselleraccounts.InsertOnSubmit(providers);
                        context2.SubmitChanges();
                    }
                }
                foreach (ListItem item2 in InvoiceProviderList.Items)
                {
                    if (item2.Selected)
                    {
                        var providers = new OptionInvoiceProvider()
                        {
                            ProviderId = int.Parse(item2.Value)
                        };
                        context.OptionInvoiceProviders.InsertOnSubmit(providers);
                        context.SubmitChanges();
                    }
                }

                foreach (ListItem item2 in portalTeData.Items)
                {
                    if (item2.Selected)
                    {
                        var providers = new OptionPortalProvider()
                        {
                            PortalProvidersId = int.Parse(item2.Value)
                        };
                        context.OptionPortalProviders.InsertOnSubmit(providers);
                        context.SubmitChanges();
                    }
                }
                // insert branches id in BranchesForResellerPayment table
                int ck = 1;
                if (ChkboxResellerdebt.Checked)
                {

                    foreach (ListItem itemB in ChkListBranchesForInstallments.Items)
                    {
                        if (itemB.Selected)
                        {
                            var bsp = context.BranchesForResellerPayments.ToList();
                            if (bsp.Count > 0 && ck == 1)
                            {
                                context.BranchesForResellerPayments.DeleteAllOnSubmit(bsp);
                                context.SubmitChanges();
                                ck++;
                            }
                            var branch = new BranchesForResellerPayment()
                            {
                                BranchId = int.Parse(itemB.Value)
                            };
                            context.BranchesForResellerPayments.InsertOnSubmit(branch);
                            context.SubmitChanges();
                        }
                    }
                }
               
                //---------------
                foreach (ListItem item3 in CheckProviderSuspend.Items)
                {
                    if (item3.Selected)
                    {
                        var providers = new OptionSuspendProvider
                        {
                            ProviderId = int.Parse(item3.Value)
                        };
                        context.OptionSuspendProviders.InsertOnSubmit(providers);
                        context.SubmitChanges();
                    }
                }
                bool reciept;
                if (CheckReciepit.Checked) reciept = true;
                else
                {
                    reciept = false;
                }
                if (checkDuration.Checked != true)
                {
                    txtFrom.Text = @"0";
                    txtTo.Text = @"0";
                }
                var alertway = Convert.ToInt32(checkUnpaidDemand.SelectedItem.Value);
                var reminderMessage = string.Empty;
                int userId = Convert.ToInt32(ddlUser.SelectedItem.Value);
                if (alertway == 2)
                {
                    reminderMessage = txtRemindertext.Text;
                    //userId = 
                }
                OptionsService.SaveOptions(context, Convert.ToInt32(TbSuspendDaysCount.Text), CbMergeGovPhone.Checked, CbIncludeGov.Checked, reciept,
                    CheckDiscound.Checked, int.Parse(txtFrom.Text), int.Parse(txtTo.Text),
                    int.Parse(TbTimeDifference.Text), CheckDiscoundfrombranchandReseller.Checked,
                    checkUpload.Checked, Checkminuscredit.Checked, CheckStatistic.Checked, checkInstallationDiscound.Checked,
                    Convert.ToInt32(txtDaysCount.Text), alertway, reminderMessage, userId, CheckSendMessage.Checked, CheckCounter.Checked,
                    CheckShowRequestInSearch.Checked, CheckActivatephone.Checked, CheckAllDemand.Checked, CheckFawry.Checked, AutoSusCustomerUnderReseller.Checked,
                    string.IsNullOrEmpty(DdTypesFrom.SelectedValue) ? 0 : Convert.ToInt32(DdTypesFrom.SelectedValue), string.IsNullOrEmpty(DdTypesTo.SelectedValue) ? 0 : Convert.ToInt32(DdTypesTo.SelectedValue), string.IsNullOrEmpty(txtPrice.Text) ? 0 : Convert.ToDecimal(txtPrice.Text),
                    Convert.ToInt32(txtday1.Text), Convert.ToInt32(txtday2.Text),
                    Convert.ToInt32(txtday3.Text), Convert.ToInt32(txtday4.Text), Convert.ToInt32(txtday5.Text), Convert.ToInt32(txtday6.Text),
                    Convert.ToInt32(txtpercentage1.Text), Convert.ToInt32(txtpercentage2.Text), Convert.ToInt32(txtpercentage3.Text),
                    Convert.ToInt32(txtpercentage4.Text), Convert.ToInt32(txtpercentage5.Text), Convert.ToInt32(txtpercentage6.Text), ChkboxResellerdebt.Checked, chkbxShowDedWithFix.Checked, chkbxPreventUnsus.Checked, string.IsNullOrEmpty(txtPortalRelayDays.Text) ? 0 : Convert.ToInt32(txtPortalRelayDays.Text),
                    cbSusAfterMonth.Checked
                    );

                if (ChkboxResellerdebt.Checked)
                {
                    Resellerdebt.Attributes.Add("style", "display:block");
                }
                else
                {
                    Resellerdebt.Attributes.Add("style", "display:none");
                }
                if (ckPendingRequestsAll.Checked)
                {
                    pendingRequestsDiv.Attributes.Add("style", "display:block");
                }
                else
                {
                    pendingRequestsDiv.Attributes.Add("style", "display:none");
                }
                Msg.InnerHtml = Tokens.Saved;
            }
        }
        protected void checkUnpaidDemand_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = Convert.ToInt32(checkUnpaidDemand.SelectedItem.Value);
            if (index == 2)
            {
                ddlUser.Enabled = txtRemindertext.Enabled = true;
            }
            else
            {
                ddlUser.Enabled = txtRemindertext.Enabled = false;
            }
        }
        protected void AddRequest(object sender, EventArgs w)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(txtDaysCount.Text)) return;
                    var ispEntries = new IspEntries(context);
                    var demandService = new DemandService();
                    //var providers = ispEntries.OptionInvoice;

                    //var orders = new List<WorkOrder>();
                    var orders =
                        context.WorkOrders.Where(x => x.ResellerID == null && x.WorkOrderStatusID == 6).ToList();
                    /*foreach (var item in providers)
                    {
                        var workorder =
                            ispEntries.ClintsOfProvider(Convert.ToInt32(item.ProviderId));
                        if (workorder.Count > 0)
                        {
                            orders.AddRange(workorder);
                        }
                    }*/
                    var now = DateTime.Now.AddHours();
                    var suspendproviders = context.OptionSuspendProviders.ToList();
                    foreach (var order in orders)
                    {
                        var orderprovider = order.ServiceProviderID;
                        var isfound = suspendproviders.Where(a => a.ProviderId == orderprovider);
                        if (!isfound.Any()) continue;

                        var orderId = order.ID;
                        var lastdemand = demandService.GetLastDemand(orderId);
                        if (lastdemand == null || lastdemand.Paid || lastdemand.Amount <= 0 || order.ResellerID != null)
                            continue;
                        var newdate = lastdemand.StartAt.AddDays(Convert.ToInt32(txtDaysCount.Text));
                        if (now.Date < newdate.Date) continue;


                        if (order.WorkOrderStatusID != 6) continue;
                        var orderRequests =
                            context.WorkOrderRequests.Where(
                                woreq => woreq.WorkOrderID == orderId && woreq.RSID == 3);
                        if (orderRequests.Any()) continue;

                       
                        var worequest = new WorkOrderRequest
                        {
                            WorkOrderID = orderId,
                            CurrentPackageID = order.ServicePackageID,
                            NewPackageID = order.ServicePackageID,
                            RequestDate = now,
                            RequestID = 2,
                            RSID = 3,
                            NewIpPackageID = order.IpPackageID,
                            SenderID = 1,
                        };
                        ispEntries.SaveRequest(worequest);
                        ispEntries.Commit();
                    }
                    Msg.InnerHtml = Tokens.ProcessDone;
                }
                catch
                {
                    Msg.InnerHtml = Tokens.Error;
                }

            }
        }
    }
}