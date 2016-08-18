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
    public partial class ChangeRequestDate : CustomPage
    {
        
            //readonly ISPDataContext _context = new ISPDataContext();//(ConfigurationManager.AppSettings["ConnectionString"]);


            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var option = OptionsService.GetOptions(context, true);
                    if (option != null && option.IncludeGovernorateInSearch) Bind_ddl_Governorates();
                    else GovDiv.Visible = option != null && option.IncludeGovernorateInSearch;
                    //Bind_ddl_Governorates();
                    if (!string.IsNullOrWhiteSpace(Request.QueryString["c"]) && !string.IsNullOrWhiteSpace(Request.QueryString["g"]))
                    {

                        var phone = QueryStringSecurity.Decrypt(Request.QueryString["c"]);
                        var gov = QueryStringSecurity.Decrypt(Request.QueryString["g"]);
                        if (option != null && option.IncludeGovernorateInSearch)
                        {
                            ddl_Governorates.SelectedValue = gov;
                        }
                        txt_CustomerPhone.Text = phone;
                        PopulateRequestDate();
                    }
                }
            }


            void PopulateRequestDate()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var option = OptionsService.GetOptions(context, true);
                    List<WorkOrder> targetWo;
                    var phone = txt_CustomerPhone.Text;
                    if (option.IncludeGovernorateInSearch)
                    {
                        var governorate = context.Governorates.FirstOrDefault(x => x.ID == Convert.ToInt32(ddl_Governorates.SelectedItem.Value));

                        // var govId = DdlGovernorate.SelectedIndex > 0 ? Convert.ToInt32(DdlGovernorate.SelectedItem.Value) : 0;

                        targetWo = context.WorkOrders.Where
                            (x => x.CustomerPhone == phone && x.CustomerGovernorateID == governorate.ID).ToList();

                    }
                    else
                    {

                        targetWo = context.WorkOrders.Where
                            (wo => wo.CustomerPhone == phone).ToList();

                    }

                    /* var targetWO =
                    _context.WorkOrders.FirstOrDefault(wo =>
                                                           wo.CustomerGovernorateID ==governorate.ID
                                                               && wo.CustomerPhone == phone);*/
                    var targetWo1 = targetWo.FirstOrDefault();
                    if (targetWo.Count < 0)
                    {
                        gv_requests.DataBind();
                        return;
                    }
                    if (targetWo1 == null) return;
                    if (targetWo1.RequestDate == null)
                    {
                        var dateStrong = Tokens.SetDate;
                        var activationRequests =
                            context.WorkOrders.Where(s => targetWo1 != null && ((s.WorkOrderStatusID == 11 ||s.WorkOrderStatusID == 6 )&& s.ID == targetWo1.ID)).
                                Select(x => new
                                {
                                    x.ID,
                                    x.CustomerName,
                                    x.CustomerPhone,
                                    x.Branch.BranchName,
                                    x.ServicePackage.ServicePackageName,
                                    x.Governorate.GovernorateName,
                                    UpdateDate = dateStrong
                                }).ToList();


                        gv_requests.DataSource = activationRequests;
                        gv_requests.DataBind();
                    }
                    else
                    {
                        var activationRequests =
                            context.WorkOrders.Where(s => targetWo1 != null && ((s.WorkOrderStatusID == 11 || s.WorkOrderStatusID == 6) && s.ID == targetWo1.ID)).
                                Select(x => new
                                {
                                    x.ID,
                                    x.CustomerName,
                                    x.CustomerPhone,
                                    x.Branch.BranchName,
                                    x.ServicePackage.ServicePackageName,
                                    x.Governorate.GovernorateName,
                                    UpdateDate = string.Format("{0:d}", x.RequestDate.Value.Date)
                                }).ToList();


                        gv_requests.DataSource = activationRequests;
                        gv_requests.DataBind();
                    }
                }
            }


            protected void Button1_Click(object sender, EventArgs e)
            {
                using (var context1 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var button = sender as LinkButton;
                    if (button == null) return;
                    var id = Convert.ToInt32(button.CommandArgument);
                    var historyItem = context1.WorkOrders.FirstOrDefault(s => s.ID == id);
                    foreach (GridViewRow row in gv_requests.Rows)
                    {
                        var hiddenField = row.FindControl("hf_id") as HiddenField;
                        if (hiddenField == null) continue;
                        if (id == Convert.ToInt32(hiddenField.Value))
                        {
                            var dateTb = row.FindControl("tb_date") as TextBox;
                            if (dateTb != null)
                            {
                                if (historyItem != null)
                                {
                                    if (Session["User_ID"] == null) continue;
                                    var userId = Convert.ToInt32(Session["User_ID"]);
                                    var changeRequestDate = new RequestDateHistory
                                    {
                                        WorkOrderId = historyItem.ID,
                                        UserId = userId,
                                        oldRequestDate = historyItem.RequestDate,
                                        newRequestDate = Convert.ToDateTime(dateTb.Text),
                                        ChangeDate = DateTime.Now.AddHours()
                                    };
                                    context1.RequestDateHistories.InsertOnSubmit(changeRequestDate);
                                    historyItem.RequestDate = Convert.ToDateTime(dateTb.Text);
                                }
                                context1.SubmitChanges();
                                break;
                            }
                        }
                    }
                    l_message.Text = Tokens.Saved;
                    PopulateRequestDate();
                }
            }


            protected void btn_search_Click(object sender, EventArgs e)
            {
                PopulateRequestDate();
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
 