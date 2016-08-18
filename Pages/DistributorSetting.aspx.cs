using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using NewIspNL.Services;
using Resources;


namespace NewIspNL.Pages
{
    public partial class DistributorSetting : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateProvider();
                PopulateBoxes();
            }
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (IsPostBack) return;

                var distributorProvider = context.DistributorProviders.ToList();
                if (distributorProvider.Count > 0)
                {
                    CbProvidersForDistributor.Checked = true;
                    ProvidersForDistributorDiv.Attributes.Add("style", "display:block");
                    foreach (var op in distributorProvider)
                    {
                        foreach (ListItem provi in CbProvidersForDistributorList.Items)
                        {
                            if (op.ServiceProvider.ID == int.Parse(provi.Value))
                            {
                                provi.Selected = true;
                            }
                        }
                    }
                }
                var option = OptionsService.GetDistributorOptions(context, false);
                if (option == null) return;
                txtCollectionCommission.Text = option.CollectionCommission.ToString();
                CbSubtractResellerCommission.Checked = Convert.ToBoolean(option.SubtractResellerCommission);
                CbclientActivationSubtract.Checked = Convert.ToBoolean(option.ClientActivationSubtract);
                ddlbox.SelectedValue = option.BoxId.ToString();
            }
        }
        protected void Save(object sender, EventArgs e)
        {
            Msg.Visible = false;
            Msg.InnerHtml = "";
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (CbProvidersForDistributorList.Items.Cast<ListItem>().Any(item => item.Selected))
                {
                    var p = context.DistributorProviders.ToList();
                    context.DistributorProviders.DeleteAllOnSubmit(p);
                    context.SubmitChanges();
                foreach (ListItem item2 in CbProvidersForDistributorList.Items)
                {
                    if (item2.Selected)
                    {
                        var providers = new DistributorProvider()
                        {
                            ProviderForDistributorID = int.Parse(item2.Value)
                        };
                        context.DistributorProviders.InsertOnSubmit(providers);
                        context.SubmitChanges();
                    }
                }
               
                    // at least one selected
                }
                else
                {
                    var p = context.DistributorProviders.ToList(); 
                    context.DistributorProviders.DeleteAllOnSubmit(p);
                    context.SubmitChanges();
                }
                decimal commission = 0;
                if (!string.IsNullOrEmpty(txtCollectionCommission.Text))
                {
                    commission = Convert.ToDecimal(txtCollectionCommission.Text);
                }
                int boxId = 0;
                if (ddlbox.SelectedIndex > 0)
                {
                    boxId = Convert.ToInt32(ddlbox.SelectedItem.Value);
                }
                OptionsService.SaveDistributorOptions(context, commission, boxId,CbclientActivationSubtract.Checked,CbSubtractResellerCommission.Checked);

                if (CbProvidersForDistributor.Checked)
                {
                    ProvidersForDistributorDiv.Attributes.Add("style", "display:block");
                }
                else
                {
                    ProvidersForDistributorDiv.Attributes.Add("style", "display:none");
                }
                Msg.Visible = true;
                Msg.InnerHtml = Tokens.Saved;
                Msg.Attributes.Add("class", "alert alert-success");
               
            }
        }
        private void PopulateProvider()
        {
            using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var providers = dataContext.ServiceProviders.ToList();
                CbProvidersForDistributorList.DataSource = providers;
                CbProvidersForDistributorList.DataTextField = "SPName";
                CbProvidersForDistributorList.DataValueField = "ID";
                CbProvidersForDistributorList.DataBind();
            }
        }

        private void PopulateBoxes()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                ddlbox.DataSource = context.Boxes.Where(a => a.ShowInCustomerDemands == true);
                ddlbox.DataTextField = "BoxName";
                ddlbox.DataValueField = "ID";
                ddlbox.DataBind();
                Helper.AddDefaultItem(ddlbox);
            }
        }
    }
}