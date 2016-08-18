using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NewIspNL.Domain;
using NewIspNL.Domain.SearchService;
using NewIspNL.Helpers;
using NewIspNL.Models;

namespace NewIspNL.Pages
{
    public partial class CustomersPayments : CustomPage
    {
       
            readonly IspDomian _domian;


            public  CustomersPayments()
            {
                _domian = new IspDomian(IspDataContext);
                _demandSearch = new DemandSearch();
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                PrepareInputs();
                Bind_ddl_PaymentType();
            }

        private void Bind_ddl_PaymentType()
        {
            _domian.PopulatePaymentTypes(DdlPaymentTypes);
        }

        public void PopulateResellerswithDirectUser()
            {
                if (Session["User_ID"] == null) Response.Redirect("default.aspx");
                var userId = Convert.ToInt32(Session["User_ID"]);
                var user = _domian.User(userId);

                DdlReseller.AppendDataBoundItems = true;
                if (user.Group.DataLevelID != 3) Helper.AddDropDownItem(DdlReseller, DdlReseller.Items.Count, "Direct User");//
                DdlReseller.DataSource = DataLevelClass.GetUserReseller();
                DdlReseller.DataTextField = "UserName";
                DdlReseller.DataValueField = "ID";

                DdlReseller.DataBind();
                if (user.Group.DataLevelID != 3) Helper.AddDefaultItem(DdlReseller);
            }
            public virtual void PopulateBranches()
            {
                if (Session["User_ID"] == null) Response.Redirect("default.aspx");
                var userId = Convert.ToInt32(Session["User_ID"]);
                var user = _domian.User(userId);
                DdlBranch.DataSource = DataLevelClass.GetUserBranches();
                DdlBranch.DataTextField = "BranchName";
                DdlBranch.DataValueField = "ID";
                DdlBranch.DataBind();
                if (user.Group.DataLevelID != 2) Helper.AddDefaultItem(DdlBranch);
            }

            void PrepareInputs()
            {
                //_domian.PopulateBranches(DdlBranch,true);
                //_domian.PopulateResellerswithDirectUser(DdlReseller,true);
                PopulateResellerswithDirectUser();
                PopulateBranches();
                _domian.PopulateProviders(DdlServiceProvider);
                _domian.PopulateGovernorates(DdlGov);
                Helper.AddAllDefaultItem(DdlCentral);
                var today = DateTime.Now.AddHours();
                TbTo.Text = today.ToShortDateString();
                TbFrom.Text = today.AddDays((today.Day * -1) + 1).ToShortDateString();
            }


            readonly DemandSearch _demandSearch;

            protected void UpdateCentrals(object sender, EventArgs e)
            {
                if (DdlGov.SelectedIndex <= 0)
                {
                    DdlCentral.Items.Clear();
                    Helper.PopulateDrop(null, DdlCentral);
                    return;
                }
                var governorateId = Convert.ToInt32(DdlGov.SelectedItem.Value);
                _domian.PopulateCentrals(DdlCentral, governorateId);
            }


            protected void Search(object sender, EventArgs e)
            {
                var model = new BasicSearchModel() { };
                if (DdlBranch.SelectedIndex > 0 || DdlBranch.SelectedItem.Value.ToString(CultureInfo.InvariantCulture) != "")
                {
                    model.BranchId = Convert.ToInt32(DdlBranch.SelectedItem.Value);
                }
                if (DdlReseller.SelectedIndex > 0 || DdlReseller.SelectedItem.Value.ToString(CultureInfo.InvariantCulture) != "")
                {
                    if (DdlReseller.SelectedValue != "0")
                    {
                        model.ResellerId = Convert.ToInt32(DdlReseller.SelectedItem.Value);
                    }
                    else
                    {
                        model.ResellerId = 0;
                    }
                }
                if (DdlPaymentTypes.SelectedIndex > 0)
                {
                    model.PaymentTypeId = Convert.ToInt32(DdlPaymentTypes.SelectedItem.Value);
                }
                if (DdlGov.SelectedIndex > 0)
                {
                    model.GovernorateId = Convert.ToInt32(DdlGov.SelectedItem.Value);
                }
                if (DdlCentral.SelectedIndex > 0)
                {
                    model.CentralId = Convert.ToInt32(DdlCentral.SelectedItem.Value);
                }
                if (DdlServiceProvider.SelectedIndex > 0)
                {
                    model.ProviderId = Convert.ToInt32(DdlServiceProvider.SelectedItem.Value);
                }
                if (!string.IsNullOrEmpty(TbFrom.Text))
                {
                    model.From = Convert.ToDateTime(TbFrom.Text);
                }
                if (!string.IsNullOrEmpty(TbTo.Text))
                {
                    model.To = Convert.ToDateTime(TbTo.Text);
                }
                if (RblEffect.SelectedIndex==1)
                {
                    model.DateSearchType = 1;
                }
                model.Isrequested = checkIsrequested.Checked;
                List<DemandResultModel> demands = _demandSearch.SearchDemands(model, IspDataContext);
                GvResults.DataSource = demands;
                GvResults.DataBind();
                var dtotaly = demands.Sum(x => Convert.ToDecimal(x.Amount));
                totaly.InnerHtml = Helper.FixNumberFormat(dtotaly);
                var sum = demands.Sum(x => x.Gains);
                //lgains.InnerHtml = Helper.FixNumberFormat(sum);
            }

            protected void NumberResults(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(GvResults, "No");
            }
            //static bool _branchPrintExcel;
            protected void ToExcel(object sender, EventArgs e)
            {
                //_branchPrintExcel = true;
                const string attachment = "attachment; filename=CustomersPayments.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/ms-excel";
                Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");

                var sw = new StringWriter();
                var htw = new HtmlTextWriter(sw);
                GvResults.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
            public override void VerifyRenderingInServerForm(Control control)
            {

            }
        }
    }
 