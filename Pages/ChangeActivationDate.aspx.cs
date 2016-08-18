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
using NewIspNL.Services;
using Resources;

namespace NewIspNL.Pages
{
    public partial class ChangeActivationDate : CustomPage
    {
        
            // readonly ISPDataContext _context;

            readonly IspDomian _domian;


            public  ChangeActivationDate()
            {
                var _context = IspDataContext;
                _domian = new IspDomian(_context);
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    if (IsPostBack) return;
                    Bind_ddl_Governorates();
                    var option = OptionsService.GetOptions(context, true);
                    if (option == null || option.IncludeGovernorateInSearch == false) GovBox.Visible = false;
                    else
                    {
                        _domian.PopulateGovernorates(ddl_Governorates);
                        GovBox.Visible = true;
                    }
                    if (!string.IsNullOrWhiteSpace(Request.QueryString["c"]) && !string.IsNullOrWhiteSpace(Request.QueryString["g"]))
                    {

                        var phone = QueryStringSecurity.Decrypt(Request.QueryString["c"]);
                        var gov = QueryStringSecurity.Decrypt(Request.QueryString["g"]);
                        if (option != null && option.IncludeGovernorateInSearch)
                        {
                            ddl_Governorates.SelectedValue = gov;
                        }
                        txt_CustomerPhone.Text = phone;
                        PopulateActivationRequests();
                    }
                }
            }


            void PopulateActivationRequests()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    WorkOrder order;
                    var option = OptionsService.GetOptions(context, true);
                    if (option != null && option.IncludeGovernorateInSearch)
                        order = ddl_Governorates.SelectedIndex > 0 ?
                            context.WorkOrders.FirstOrDefault(wo => wo.CustomerPhone == txt_CustomerPhone.Text.Trim() && wo.CustomerGovernorateID == Convert.ToInt32(ddl_Governorates.SelectedItem.Value)) : null;
                    else order = context.WorkOrders.FirstOrDefault(wo => wo.CustomerPhone == txt_CustomerPhone.Text.Trim());
                    if (order == null)
                    {
                        gv_requests.DataBind();
                        return;
                    }

                    var activationRequests =
                        context.WorkOrderStatus.Where(s => order != null && (s.StatusID == 6 && s.WorkOrderID == order.ID)).
                            Select(x => new
                            {
                                x.ID,
                                x.WorkOrder.
                                CustomerName,
                                x.WorkOrder.
                                CustomerPhone,
                                x.WorkOrder.Branch.BranchName,
                                x.WorkOrder.ServicePackage.ServicePackageName,
                                x.WorkOrder.Governorate.GovernorateName,
                                UpdateDate = string.Format("{0:d}", x.UpdateDate.Value.Date)
                            }).ToList();



                    gv_requests.DataSource = activationRequests;
                    gv_requests.DataBind();
                }
            }


            protected void Button1_Click(object sender, EventArgs e)
            {
                using (var context1 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var button = sender as LinkButton;
                    if (button == null) return;
                    var id = Convert.ToInt32(button.CommandArgument);
                    var historyItem = context1.WorkOrderStatus.FirstOrDefault(s => s.ID == id);
                    foreach (GridViewRow row in gv_requests.Rows)
                    {
                        var hiddenField = row.FindControl("hf_id") as HiddenField;
                        if (hiddenField == null) continue;
                        if (id == Convert.ToInt32(hiddenField.Value))
                        {
                            var dateTB = row.FindControl("tb_date") as TextBox;
                            if (dateTB != null)
                            {
                                if (historyItem != null) historyItem.UpdateDate = Convert.ToDateTime(dateTB.Text);
                                context1.SubmitChanges();
                                break;
                            }
                        }
                    }
                    l_message.Text = Tokens.Saved;
                    PopulateActivationRequests();
                }
            }


            protected void btn_search_Click(object sender, EventArgs e)
            {
                PopulateActivationRequests();
            }


            void Bind_ddl_Governorates()
            {
                using (var context2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    ddl_Governorates.SelectedValue = null;
                    ddl_Governorates.Items.Clear();

                    ddl_Governorates.AppendDataBoundItems = true;
                    var query = context2.Governorates.Select(gov => gov);
                    ddl_Governorates.DataSource = query;
                    ddl_Governorates.DataBind();
                    Helper.AddDefaultItem(ddl_Governorates);
                }
            }
        }

    }
 