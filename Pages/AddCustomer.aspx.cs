using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using System.Web.UI.HtmlControls;
namespace NewIspNL.Pages
{
    public partial class AddCustomer : CustomPage
    {
        
            protected void Page_Load(object sender, EventArgs e)
            {
                if (Session["User_ID"] == null)
                {
                    Response.Redirect("~/Pages/default.aspx");
                    return;
                }
                Activate();
                if (IsPostBack) return;
                flag.Value = "0";
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    PopulateDetails(context);
                    PopulateGovernorates(context);
                }
            }
            private void PopulateDetails(ISPDataContext context)
            {
                var list = context.Customers.Select(a => new
                {
                    a.Id,
                    a.CustomerName,
                    a.CustomerPhone,
                    a.CustomerEmail,
                    a.CustomerMobile,
                    a.Governorate.GovernorateName,
                    a.CustomerPersonalId,
                    a.CustomerAddress
                }).ToList();
                GvCustomer.DataSource = list;
                GvCustomer.DataBind();
            }
            void PopulateGovernorates(ISPDataContext db3)
            {
                var query = db3.Governorates.Select(gov => gov);
                ddl_Governorates.DataSource = query;
                ddl_Governorates.DataBind();
                Helper.AddDefaultItem(ddl_Governorates);
            }

            private void Save()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    try
                    {
                        if (ddl_Governorates.SelectedIndex < 0)
                        {
                            ErrorDiv.Visible = true;
                            return;
                        }
                        var chk = Convert.ToInt32(selected.Value);
                        var name = txtName.Text;
                        var phone = txtPhone.Text;
                        var address = txtAddress.Text;
                        var email = txtEmail.Text;
                        var governerateId = Convert.ToInt32(ddl_Governorates.SelectedItem.Value);
                        var mobile = txtMobile.Text;
                        var personalId = txtPersonalId.Text;

                        if (chk == 0)
                        {
                            //new
                            if (string.IsNullOrWhiteSpace(selected.Value) || chk == 0)
                            {

                                var newcustomer = new Customer
                                {
                                    CustomerName = name,
                                    CustomerPhone = phone,
                                    CustomerAddress = address,
                                    CustomerEmail = email,
                                    CustomerGovernerateId = governerateId,
                                    CustomerMobile = mobile,
                                    CustomerPersonalId = personalId
                                };
                                context.Customers.InsertOnSubmit(newcustomer);
                                context.SubmitChanges();
                                SuccDiv.Visible = true;
                                ErrorDiv.Visible = false;

                                flag.Value = "0";
                                PopulateDetails(context);
                            }
                        }
                        else
                        {
                            // edit
                            var id = chk;
                            var detail = context.Customers.FirstOrDefault(a => a.Id == id);
                            if (detail != null)
                            {
                                detail.CustomerName = name;
                                detail.CustomerPhone = phone;
                                detail.CustomerEmail = email;
                                detail.CustomerMobile = mobile;
                                detail.CustomerAddress = address;
                                detail.CustomerGovernerateId = governerateId;
                                detail.CustomerPersonalId = personalId;
                                context.SubmitChanges();
                                SuccDiv.Visible = true;
                                ErrorDiv.Visible = false;
                                flag.Value = "0";
                                PopulateDetails(context);
                            }
                        }

                    }
                    catch (Exception)
                    {
                        ErrorDiv.Visible = true;
                        SuccDiv.Visible = false;
                    }
                }
            }

            void Activate()
            {
                GvCustomer.DataBound += (o, e) => Helper.GridViewNumbering(GvCustomer, "LNo");
                BAdd.ServerClick += (o, e) => AddItem();
                BSave.ServerClick += (o, e) => Save();
                bCancel.ServerClick += (o, e) =>
                {
                    flag.Value = "0";
                    //Clear();
                };

            }
            void AddItem()
            {
                //Clear();
                flag.Value = "1";
                txtName.Text = txtMobile.Text = txtAddress.Text = txtPhone.Text = txtPersonalId.Text = txtEmail.Text = string.Empty;
                ddl_Governorates.SelectedIndex = -1;
            }
            /*void Clear()
            {
                MsgSuccess.Visible = MsgError.Visible = false;
                MsgSuccess.InnerHtml = MsgError.InnerHtml = string.Empty;
            }*/
            protected void Newdata(object sender, EventArgs e)
            {
                selected.Value = "0";
            }
            protected void EditEvent(object sender, EventArgs e)
            {
                using (var contexte = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    flag.Value = "1";
                    var btn = sender as HtmlButton;
                    if (btn == null) return;
                    var id = Convert.ToInt32(btn.ValidationGroup);
                    selected.Value = btn.ValidationGroup;
                    var detail = contexte.Customers.FirstOrDefault(a => a.Id == id);
                    if (detail == null) return;
                    txtName.Text = detail.CustomerName;
                    txtMobile.Text = detail.CustomerMobile;
                    txtPhone.Text = detail.CustomerPhone;
                    txtEmail.Text = detail.CustomerEmail;
                    txtAddress.Text = detail.CustomerAddress;
                    txtPersonalId.Text = detail.CustomerPersonalId;
                    ddl_Governorates.SelectedItem.Value = detail.CustomerGovernerateId.ToString();
                }
            }
            protected void DeleteEvent(object sender, EventArgs e)
            {
                try
                {
                    using (var contextd = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                    {
                        var btn = sender as LinkButton;
                        if (btn == null) return;
                        var id = Convert.ToInt32(btn.ValidationGroup);
                        var detail = contextd.Customers.FirstOrDefault(a => a.Id == id);
                        if (detail != null)
                        {
                            contextd.Customers.DeleteOnSubmit(detail);
                            contextd.SubmitChanges();
                            flag.Value = "0";
                            PopulateDetails(contextd);
                        }
                    }
                }
                catch (Exception)
                {
                    ErrorDiv.Visible = true;
                    SuccDiv.Visible = false;
                }
            }

        }
    }
 