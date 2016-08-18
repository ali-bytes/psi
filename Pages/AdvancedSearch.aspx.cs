using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.SearchService;
using NewIspNL.Helpers;
using NewIspNL.Models;
using Resources;
using System.Diagnostics;
using Microsoft.Ajax.Utilities;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Text;
using BL.Concrete;
using NewIspNL.Domain.Concrete;

namespace NewIspNL.Pages
{
    public partial class AdvancedSearch : CustomPage
    {

        // readonly ISPDataContext _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        //static bool BranchPrintExcel;
        readonly IspDomian _domian;

        readonly ISearchEngine _searchEngine;
        private readonly IspEntries _ispEntries;

        public AdvancedSearch()
        {
            _searchEngine = new SearchEngine(IspDataContext);
            _domian = new IspDomian(IspDataContext);
            //ResultsData = Results != null ? Results : null;
            _ispEntries = new IspEntries(IspDataContext);
        }


        public bool CanEdit { get; set; }
        public int GroupId { get; set; }


        public List<CustomerResult> Results { get; set; }
        //public List<CustomerResult> ResultsData { get; set; }



        protected void Page_Load(object sender, EventArgs e)
        {
            var userId = Convert.ToInt32(Session["User_ID"]);
            var canEditModel = _searchEngine.EditCustomer(userId);
            CanEdit = canEditModel.CanEdit;
            GroupId = canEditModel.GroupId;

            if (IsPostBack) return;
            lblFromRequestDate.Text = Tokens.RequestDate + @" : " + Tokens.From;
            PrepareInputs(userId);
            CheckCanchange();

        }
        protected void SendsmsSelected(object sender, EventArgs e)
        {
            string idList = Request.Form["ck"];
            string[] ids = idList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var messages = new StringBuilder();
            foreach (string id in ids)
            {
                var wId = Convert.ToInt32(id);
                var wo = _ispEntries.GetWorkOrder(wId);
                var mobile = wo.CustomerMobile;
                messages.Append(SendSms(mobile));
            }




            //var messages = new StringBuilder();
            //foreach (GridViewRow row in ListView1.Rows)
            //{
            //    var cb = row.FindControl("CbPay") as CheckBox;
            //    if (cb == null || !cb.Checked) continue;
            //    var demandId = Convert.ToInt32(cb.CssClass);
            //    var demand = _ispEntries.GetDemand(demandId);
            //    var mobile = demand.WorkOrder.CustomerMobile;
            //    messages.Append(SendSms(mobile));
            //}
            ClientScript.RegisterClientScriptBlock(typeof(Page), "myscript", messages.ToString(), true);
            Msg.InnerHtml = Tokens.Saved;
        }
        private string SendSms(string mobile)
        {
            using (var context1 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var smsData = new SMSData(context1);
                var data = smsData.GetActiveCnfg();
                if (!string.IsNullOrWhiteSpace(mobile) && !string.IsNullOrWhiteSpace(txtMessageText.Text) && data != null) // && Convert.ToBoolean(data.sendsms))
                {

                    var message = global::SendSms.Send(data.UserName, data.Password, mobile, txtMessageText.Text, data.Sender,
                        data.UrlAPI);
                    string myscript = "window.open('" + message + "');";
                    return myscript;
                }
                return string.Empty;
            }
        }
        void CheckCanchange()
        {
            using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {

                var flag = false;
                Changeset.Visible = false;
                var groupIdQuery = from usr in dataContext.Users
                                   where usr.ID == Convert.ToInt32(Session["User_ID"])
                                   select usr;

                var id = groupIdQuery.First().GroupID;
                if (id != null)
                {
                    var groupId = id.Value;

                    if (groupId == 1)
                    {
                        flag = true;

                    }
                }

                if (flag)
                    Changeset.Visible = true;
            }
        }


        void PrepareInputs(int userId)
        {
            if (GroupId == 6)
            {
                _domian.PopulateResellers(DdlReseller, true);
            }
            else
            {
                _domian.PopulateResellerswithDirectUser(DdlReseller, true);
            }
            _domian.PopulateGovernorates(DdlGovernorate);
            _domian.PopulateIpPackages(DdlIpPackages);
            PopulateProviders();
            //_domian.PopulateProviders(DdlProvider);
            _domian.PopulatePackages(DdlPackage);
            _domian.PopulateBranches(DdlBranchs, true);
            _domian.PopulateStatuses(DdlStatus);
            _domian.PopulateOffers(DdlOffer, userId);
            //_domian.PopulateCentrals(DdlCentral);
            Helper.AddDefaultItem(DdlCentral);
            _domian.PopulateResellers(DdlReseller2, true);
            // _domian.PopulateResellerswithDirectUser(DdlReseller2,true);
            _domian.PopulateGovernorates(DdlGovernorate2);
            _domian.PopulateProviders(DdlProvider2);
            _domian.PopulatePackages(DdlPackage2);
            _domian.PopulateBranches(DdlBranchs2, true);
            _domian.PopulateCentrals(DdlCentral2);
            _domian.PopulatePaymentTypes(DdlPaymentTypes);
            _domian.PopulatePaymentTypes(DdlPaymentType);

            _domian.PopulateOffers(DdlOffersets);
        }

        /*void Bind_ddl_PaymentType()
        {
            using (var db4 = new ISPDataContext())
            {
                var query = db4.PaymentTypes.Select(paymenttp => paymenttp);
                DdlPaymentType.DataSource = query;
                ddl_PaymentType.DataBind();
                Helper.AddDefaultItem(ddl_PaymentType);

                var first = db4.Users.Where(usr => usr.ID == Convert.ToInt32(Session["User_ID"])).Select(usr => usr.Group.DataLevelID).First();
                if (first == null)
                    return;
                int dataLevel = first.Value;
                if (dataLevel != 3)
                    return;
                ddl_PaymentType.SelectedValue = "1";
                ddl_PaymentType.Enabled = false;
            }
        }*/
        void PopulateProviders()
        {
            using (var db = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {

                var userId = Convert.ToInt32(Session["User_ID"]);
                if (GroupId == 6)
                {
                    var providers = db.UserProviders.Where(a => a.UserId == userId).Select(x => new
                    {
                        x.ServiceProvider.SPName,
                        x.ServiceProvider.ID,
                    }).ToList();
                    DdlProvider.DataSource = providers;
                    DdlProvider.DataTextField = "SPName";
                    DdlProvider.DataValueField = "ID";
                    DdlProvider.DataBind();
                    Helper.AddDefaultItem(DdlProvider);
                }
                else
                {
                    _domian.PopulateProviders(DdlProvider);
                }
            }
        }


        protected void BSearch_OnClick(object sender, EventArgs e)
        {
            Stopwatch sWatch = new Stopwatch();
            sWatch.Start();
            Search();
            sWatch.Stop();
            sWatch.ElapsedMilliseconds.ToString();
            sWatch.Reset();

        }


        void Search()
        {
            
             int isMonthChecked = 0;
            int CheckPoint = 0;
            int isPrePaidChecked = 0;
            int isTextEmpty = 1;
            var model = new AdvancedBasicSearchModel
            {
                Email = TbEmail.Text,
                Mobile = TbMobile.Text,
                Name = TbName.Text,
                Phone = TbPhone.Text,
                Vci = TbVCI.Text,
                Vpi = TbVPI.Text,
                UserName = TbUserName.Text,
                IsSystemAdmin = _domian.GetUserGroup(Convert.ToInt32(Session["User_ID"])) == 1
            };
            if (DdlGovernorate.SelectedIndex != 0)
            {
                model.GovernorateId = Convert.ToInt32(DdlGovernorate.SelectedItem.Value);
                CheckPoint = 1;
            }
            if (DdlIpPackages.SelectedIndex != 0)
            {
                model.IpPackageId = Convert.ToInt32(DdlIpPackages.SelectedItem.Value);
                CheckPoint = 1;
            }
            if (DdlProvider.SelectedIndex != 0)
            {
                model.ProviderId = Convert.ToInt32(DdlProvider.SelectedItem.Value);
                CheckPoint = 1;
            }
            if (DdlPackage.SelectedIndex != 0)
            {
                model.PackageId = Convert.ToInt32(DdlPackage.SelectedItem.Value);
                CheckPoint = 1;
            }
            if (DdlReseller.SelectedIndex != 0)
            {
                model.ResellerId = Convert.ToInt32(DdlReseller.SelectedItem.Value);
                CheckPoint = 1;
            }
            else
            {
                
                model.Resellers = new List<int>();
                foreach (ListItem item in DdlReseller.Items.Cast<ListItem>().Where(item => DdlReseller.Items.IndexOf(item) != 0))
                {
                    model.Resellers.Add(Convert.ToInt32(item.Value));
                }

            }

            if (DdlBranchs.SelectedIndex != 0)
            {
                model.BranchId = Convert.ToInt32(DdlBranchs.SelectedItem.Value);
                CheckPoint = 1;
            }
            else
            {
                model.Branches = new List<int>();
                foreach (ListItem item in DdlBranchs.Items.Cast<ListItem>().Where(item => DdlBranchs.Items.IndexOf(item) != 0))
                {
                    model.Branches.Add(Convert.ToInt32(item.Value));
                }
            }
            if (DdlStatus.SelectedIndex != 0)
            {
                model.StatusId = Convert.ToInt32(DdlStatus.SelectedItem.Value);
                CheckPoint = 1;
            }

            if (DdlOffer.SelectedIndex != 0)
            {
                model.OfferId = Convert.ToInt32(DdlOffer.SelectedItem.Value);
                CheckPoint = 1;
            }

            if (DdlCentral.SelectedIndex != 0)
            {
                model.CentralId = Convert.ToInt32(DdlCentral.SelectedItem.Value);
                CheckPoint = 1;
            }
            if (DdlPaymentType.SelectedIndex != 0)
            {
                model.PaymentTypeId = Convert.ToInt32(DdlPaymentType.SelectedItem.Value);
                CheckPoint = 1;
            }
            if (Check24Month.Checked)
            {
                model.Path24Month = true;
                isMonthChecked = 1;
            }
            if(CheckPrePaid.Checked)
            {
            model.PrePaid = true;
            isPrePaidChecked = 1;
            }

            if (!string.IsNullOrWhiteSpace(txtFromRequestDate.Text))
            {
                model.From = Convert.ToDateTime(txtFromRequestDate.Text);
            }

            if (!string.IsNullOrWhiteSpace(txtToRequestDate.Text))
            {
                model.To = Convert.ToDateTime(txtToRequestDate.Text);
            }

            if (string.IsNullOrWhiteSpace(TbEmail.Text) && string.IsNullOrWhiteSpace(txtToRequestDate.Text) && string.IsNullOrWhiteSpace(txtFromRequestDate.Text) && string.IsNullOrWhiteSpace(TbMobile.Text) && string.IsNullOrWhiteSpace(TbName.Text) && string.IsNullOrWhiteSpace(TbPhone.Text) && string.IsNullOrWhiteSpace(TbVCI.Text) && string.IsNullOrWhiteSpace(TbVPI.Text) && string.IsNullOrWhiteSpace(TbUserName.Text))
            {
                isTextEmpty = 0;
            }else
            {
                if (!string.IsNullOrWhiteSpace(TbName.Text))
                {
                    if (!IsNumOrLetter(TbName.Text))
                    {
                        return;
                    }
                }

                if (!string.IsNullOrWhiteSpace(TbMobile.Text))
                {
                    if (!IsNum(TbMobile.Text))
                    {
                        return;
                    }
                }

                if (!string.IsNullOrWhiteSpace(TbPhone.Text))
                {
                    if (!IsNum(TbPhone.Text))
                    {
                        return;
                    }
                }

                if (!string.IsNullOrWhiteSpace(TbVPI.Text))
                {
                    if (!IsNumOrLetter(TbVPI.Text))
                    {
                        return;
                    }
                }
                if (!string.IsNullOrWhiteSpace(TbUserName.Text))
                {
                    if (!IsNumOrLetter(TbUserName.Text))
                    {
                        return;
                    }
                }
                if (!string.IsNullOrWhiteSpace(TbEmail.Text))
                {
                    if (!IsValidEmail(TbEmail.Text))
                    {
                        return;
                    }
                }
                if (!string.IsNullOrWhiteSpace(TbVCI.Text))
                {
                    if (!IsNumOrLetter(TbVCI.Text))
                    {
                        return;
                    }
                }



            }

            var userid = 0;
            using (var db = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {

                userid = Convert.ToInt32(Session["User_ID"]);

            }
            Stopwatch sWatch = new Stopwatch();
            sWatch.Start();
            Results = _searchEngine.Search(model, CheckPoint, isMonthChecked, isPrePaidChecked, isTextEmpty, userid);
            sWatch.Stop();
            sWatch.ElapsedMilliseconds.ToString();
            sWatch.Reset();
    
            //if (!string.IsNullOrWhiteSpace(txtFromRequestDate.Text))
            //    Results = Results.Where(a => a.RequestDate.Date >= Convert.ToDateTime(txtFromRequestDate.Text).Date).ToList();
            //if (!string.IsNullOrWhiteSpace(txtToRequestDate.Text))
            //    Results = Results.Where(a => a.RequestDate.Date <= Convert.ToDateTime(txtToRequestDate.Text).Date).ToList();

            //ResultsData = Results;
            if (!ChcekFullData.Checked && Results!=null)
            {
                //SGVResult.DataSource = Results;
                //SGVResult.DataBind();
                ListView1.DataSource = Results.DistinctBy(a=>a.Id);
                ListView1.DataBind();
                //Export.Visible = true;
            }
        }

        bool IsNumOrLetter(string str)
        {
            
                //[^a-z0-9]
            Match match = Regex.Match(str, "^[a-zA-Z0-9 ]|[\u0600-\u06ff]|[\u0750-\u077f]|[\ufb50-\ufc3f]|[\ufe70-\ufefc]+$",
          RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
           
        }
        bool IsNum(string str)
        {

          Match match = Regex.Match(str, "^[0-9]+$",
          RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        protected void SaveNewSettings(object sender, EventArgs e)
        {
            using (var db1 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var selectedIds = HfIds.Value;
                var ids = selectedIds.Split(',');
                var list = ids.Select(x => new
                {
                    Id = Convert.ToInt32(x)
                }).ToList();

                foreach (var item in list)
                {
                    var order = db1.WorkOrders.FirstOrDefault(w => w.ID == item.Id);
                    if (order == null)
                    {
                        continue;
                    }
                    if (DdlReseller2.SelectedIndex > 0)
                    {
                        order.ResellerID = Convert.ToInt32(DdlReseller2.SelectedItem.Value);
                    }
                    if (DdlGovernorate2.SelectedIndex > 0)
                    {
                        order.CustomerGovernorateID = Convert.ToInt32(DdlGovernorate2.SelectedItem.Value);
                    }
                    if (DdlProvider2.SelectedIndex > 0)
                    {
                        order.ServiceProviderID = Convert.ToInt32(DdlProvider2.SelectedItem.Value);
                    }
                    if (DdlPackage2.SelectedIndex > 0)
                    {
                        order.ServicePackageID = Convert.ToInt32(DdlPackage2.SelectedItem.Value);
                    }
                    if (DdlBranchs2.SelectedIndex > 0)
                    {
                        order.BranchID = Convert.ToInt32(DdlBranchs2.SelectedItem.Value);
                    }
                    if (DdlCentral2.SelectedIndex > 0)
                    {
                        order.CentralId = Convert.ToInt32(DdlCentral2.SelectedItem.Value);
                    }
                    if (DdlPaymentTypes.SelectedIndex > 0)
                    {
                        order.PaymentTypeID = Convert.ToInt32(DdlPaymentTypes.SelectedItem.Value);
                    }
                    if (DdlOffersets.SelectedIndex > 0)
                    {
                        order.OfferId = Convert.ToInt32(DdlOffersets.SelectedItem.Value);
                    }
                    if (ToDirctCustomer.Checked)
                    {
                        order.ResellerID = null;
                    }
                    db1.SubmitChanges();
                }

                Search();
                Reset();
            }
        }


        void Reset()
        {
            HfIds.Value = string.Empty;
            DdlReseller2.SelectedIndex = DdlGovernorate2.SelectedIndex =
                DdlProvider2.SelectedIndex = DdlPackage2.SelectedIndex = DdlBranchs2.SelectedIndex
                    = DdlCentral2.SelectedIndex = DdlPaymentTypes.SelectedIndex = DdlOffersets.SelectedIndex = -1;
            ToDirctCustomer.Checked = false;
        }
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
        protected void Export_OnClick(object sender, EventArgs e)
        {
            //SGVResult.Visible = true;
            //BranchPrintExcel = true;
            string attachment = string.Format("attachment; filename=AdvancedSearch.xls");
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);
            //SGVResult.RenderControl(htw);
            ListView1.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();

            //SGVResult.Visible = false;

            /*string completePath = HttpContext.Current.Server.MapPath("~/ExcelTemplates/AdvancedSearch.xls");
            var time = DateTime.Now.AddHours();
            var timeName = time.Day + "_" + time.Month + "_" + time.Year + "_" + time.Hour + "_" + time.Minute + "_" + time.Millisecond;

            if (File.Exists(completePath))
                File.Copy(HttpContext.Current.Server.MapPath("~/ExcelTemplates/AdvancedSearch.xls"),
                    HttpContext.Current.Server.MapPath(string.Format("~/ExcelTemplates/AdvancedSearch/{0}.xls", timeName)),
                    true);

            var currentExtension = Path.GetExtension("~/ExcelTemplates/temp/AdvancedSearch.xls");
            var connection = new OleDbConnection();
            switch (currentExtension)
            {
                case ".xls":
                    connection.ConnectionString =
                        string.Format(
                                      @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                                      HttpContext.Current.Server.MapPath("~\\ExcelTemplates\\AdvancedSearch\\") +
                                      "\\{0};Extended Properties='Excel 8.0;HDR=Yes;'", string.Format("{0}.xls", timeName));
                    break;
            }
            try
            {
                for (int i = 0; i < Results.Count; i++)
                {
                    var myCommand = new OleDbCommand
                    {
                        Connection = connection
                    };
                    const string sql = "Insert into [Sheet1$] values(@a,@b,@j,@c,@d,@e,@f,@g,@h,@i)";
                    myCommand.CommandText = sql;
                    connection.Open();
                    var demand = Results[i];
                    myCommand.Parameters.AddWithValue("@a", demand.Customer);
                    myCommand.Parameters.AddWithValue("@b", demand.Phone);
                    myCommand.Parameters.AddWithValue("@j", demand.Offer);
                    myCommand.Parameters.AddWithValue("@c", demand.State);
                    myCommand.Parameters.AddWithValue("@d", demand.Branch);
                    myCommand.Parameters.AddWithValue("@e", demand.Reseller);
                    myCommand.Parameters.AddWithValue("@f", demand.Central);
                    myCommand.Parameters.AddWithValue("@g", demand.Package);
                    myCommand.Parameters.AddWithValue("@h", demand.ActivationDate);
                    myCommand.Parameters.AddWithValue("@i", demand.TCreationDate);
                
                    myCommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception)
            {

            }
    */

        }

        protected void LinkBtnEdit_Command(object sender, CommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            Response.Redirect(string.Format("EditCustomer.aspx?WOID={0}", QueryStringSecurity.Encrypt(id.ToString())));
        }
        protected void LinkBtnDetails_Command(object sender, CommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            Response.Redirect(string.Format("CustomerDetails.aspx?WOID={0}", QueryStringSecurity.Encrypt(id.ToString())));
        }
    }
}
