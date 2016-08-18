using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class ResellerCollectionCommission : CustomPage
    {
        private readonly IspDomian _domian;

        public ResellerCollectionCommission()
        {
            _domian = new IspDomian(IspDataContext);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulatePage();
            SearchRes();
        }

        protected void Search(object sender, EventArgs e)
        {
            SearchRes();
        }

        protected void Add(object sender, EventArgs e)
        {
            if (DdlReseller.SelectedIndex > 0  &&
                !string.IsNullOrEmpty(TbExternalCustomersCommission.Text) &&
                !string.IsNullOrEmpty(TbHisClientsCommission.Text))
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    //var providerId = Convert.ToInt32(DdlProvider.SelectedValue);
                    var resellerId = Convert.ToInt32(DdlReseller.SelectedValue);
                    var oldComm =
                        context.ResellerDistributorCommisions.FirstOrDefault(
                            x => x.ResellerID == resellerId);
                    if (oldComm != null)
                    {
                        msg.Visible = true;
                        msg.InnerHtml = "هذا السجل موجود مسبقاً";
                        msg.Attributes.Add("class", "alert alert-danger");
                        return;
                    }
                    var newComm = new ResellerDistributorCommision
                    {
                        //ProviderID = providerId,
                        ResellerID = resellerId,
                        HisClientCommission = Convert.ToDecimal(TbHisClientsCommission.Text),
                        OtherClientCommission = Convert.ToDecimal(TbExternalCustomersCommission.Text)
                    };

                    context.ResellerDistributorCommisions.InsertOnSubmit(newComm);
                    context.SubmitChanges();
                    SearchRes();
                    msg.Visible = true;
                    msg.InnerHtml = Tokens.Saved;
                    msg.Attributes.Add("class", "alert alert-success");
                }
            }
            else
            {
                msg.Visible = true;
                msg.InnerHtml = "Not Saved";
                msg.Attributes.Add("class", "alert alert-danger");
            }
        }

        protected void EditCommission(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TbDialogHisClientsCommission.Text) &&
                !string.IsNullOrEmpty(TbDialogExternalCustomersCommission.Text))
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                   
                    if (hdResellerId.Value == null)
                    {
                        msg.Visible = true;
                        msg.InnerHtml = "Not Saved";
                        msg.Attributes.Add("class", "alert alert-danger");
                        return;
                    }
                    //var providerId = Convert.ToInt32(hdProviderId.Value);
                    var resellerId = Convert.ToInt32(hdResellerId.Value);
                    if (resellerId <=0)
                    {
                        msg.Visible = true;
                        msg.InnerHtml = "Not Saved";
                        msg.Attributes.Add("class", "alert alert-danger");
                        return;
                    }
                    var oldComm =
                        context.ResellerDistributorCommisions.FirstOrDefault(
                            x => x.ResellerID == resellerId);
                    if (oldComm != null)
                    {
                        //oldComm.ProviderID = providerId;
                        oldComm.ResellerID = resellerId;
                        oldComm.HisClientCommission = Convert.ToDecimal(TbDialogHisClientsCommission.Text);
                        oldComm.OtherClientCommission = Convert.ToDecimal(TbDialogExternalCustomersCommission.Text);
                        context.SubmitChanges();
                        SearchRes();
                        msg.Visible = true;
                        msg.InnerHtml = Tokens.Saved;
                        msg.Attributes.Add("class", "alert alert-success");
                    }
                }
            }
            else
            {
                msg.Visible = true;
                msg.InnerHtml = "Not Saved";
                msg.Attributes.Add("class", "alert alert-danger");
            }
        }

        private void PopulatePage()
        {
            //_domian.PopulateProviders(DdlProvider);
           //_domian.PopulateProviders(DdlProviderEdit);
            _domian.PopulateResellers(DdlReseller);
           _domian.PopulateResellers(DdlResellerEdit);
        }
        private void SearchRes()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var resCommission =
                    context.ResellerDistributorCommisions.ToList()
                        .Select(a => new ResellerCollectionCommissionModal
                        {
                            ResellerId = a.ResellerID,
                            //ProviderId = a.ProviderID,
                            Reseller = a.User.UserName,
                            //Provider = a.ServiceProvider.SPName,
                            ExternalCustomersCommission = a.OtherClientCommission ?? 0,
                            HisClientsCommission = a.HisClientCommission ?? 0
                        });

                gv_index.DataSource = resCommission;
                gv_index.DataBind();
            }
        }

        protected void gv_index_DataBound(object sender, EventArgs e)
        {
            Helper.GridViewNumbering(gv_index, "l_number");
        }
    }

    public class ResellerCollectionCommissionModal
    {
        public int ResellerId { get; set; }
        public int ProviderId { get; set; }
        public string Reseller { get; set; }
        public string Provider { get; set; }
        public decimal HisClientsCommission { get; set; }
        public decimal ExternalCustomersCommission { get; set; }
    }
}