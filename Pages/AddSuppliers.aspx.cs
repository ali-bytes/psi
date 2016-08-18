using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class AddSuppliers : CustomPage
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
                var list = context.Suppliers.Select(a => new
                {
                    a.Id,
                    a.SupplierName,
                    a.SupplierPhone,
                    a.SupplierEmail,
                    a.SupplierMobile1,
                    a.Governorate.GovernorateName,
                    a.SupplierPersonalId,
                    a.SupplierAddress
                }).ToList();
                GvSuppliers.DataSource = list;
                GvSuppliers.DataBind();
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
                        var mobile2 = txtMobile2.Text;
                        var personalId = txtPersonalId.Text;

                        if (chk == 0)
                        {
                            //new
                            if (string.IsNullOrWhiteSpace(selected.Value) || chk == 0)
                            {

                                var newsupplier = new Supplier
                                {
                                    SupplierName = name,
                                    SupplierPhone = phone,
                                    SupplierAddress = address,
                                    SupplierEmail = email,
                                    SupplierGovernerateId = governerateId,
                                    SupplierMobile1 = mobile,
                                    SupplierMobile2 = mobile2,
                                    SupplierPersonalId = personalId
                                };
                                context.Suppliers.InsertOnSubmit(newsupplier);
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
                            var detail = context.Suppliers.FirstOrDefault(a => a.Id == id);
                            if (detail != null)
                            {
                                detail.SupplierName = name;
                                detail.SupplierPhone = phone;
                                detail.SupplierEmail = email;
                                detail.SupplierMobile1 = mobile;
                                detail.SupplierMobile2 = mobile2;
                                detail.SupplierAddress = address;
                                detail.SupplierGovernerateId = governerateId;
                                detail.SupplierPersonalId = personalId;
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
                GvSuppliers.DataBound += (o, e) => Helper.GridViewNumbering(GvSuppliers, "LNo");
                BAdd.ServerClick += (o, e) => AddItem();
                BSave.ServerClick += (o, e) => Save();
                bCancel.ServerClick += (o, e) =>
                {
                    flag.Value = "0";
                };

            }
            void AddItem()
            {
                flag.Value = "1";
                txtName.Text = txtMobile.Text = txtMobile2.Text = txtAddress.Text = txtPhone.Text = txtPersonalId.Text = txtEmail.Text = string.Empty;
                ddl_Governorates.SelectedIndex = -1;
            }
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
                    var detail = contexte.Suppliers.FirstOrDefault(a => a.Id == id);
                    if (detail == null) return;
                    txtName.Text = detail.SupplierName;
                    txtMobile.Text = detail.SupplierMobile1;
                    txtMobile2.Text = detail.SupplierMobile2;
                    txtPhone.Text = detail.SupplierPhone;
                    txtEmail.Text = detail.SupplierEmail;
                    txtAddress.Text = detail.SupplierAddress;
                    txtPersonalId.Text = detail.SupplierPersonalId;
                    ddl_Governorates.SelectedItem.Value = detail.SupplierGovernerateId.ToString();
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
                        var detail = contextd.Suppliers.FirstOrDefault(a => a.Id == id);
                        if (detail != null)
                        {
                            contextd.Suppliers.DeleteOnSubmit(detail);
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
 