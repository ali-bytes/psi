using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using System.Web.UI.HtmlControls;


namespace NewIspNL.Pages
{
    public partial class AddItems : CustomPage
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
                    //PopulateGovernorates(context);
                }
            }
            private void PopulateDetails(ISPDataContext context)
            {
                var list = context.Items.ToList();
                GvSuppliers.DataSource = list;
                GvSuppliers.DataBind();
            }
            /*void PopulateGovernorates(ISPDataContext db3)
            {
                var query = db3.Governorates.Select(gov => gov);
                ddl_Governorates.DataSource = query;
                ddl_Governorates.DataBind();
                Helper.AddDefaultItem(ddl_Governorates);
            }*/

            private void Save()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    try
                    {
                        var chk = Convert.ToInt32(selected.Value);
                        var name = txtName.Text;
                        var purchusPrice = Convert.ToDecimal(txtPurchasePrice.Text);
                        var sellPrice = Convert.ToDecimal(txtSellPrice.Text);
                        var firstBalance = Convert.ToDecimal(txtFirstBalance.Text);
                        if (chk == 0)
                        {
                            //new
                            var checkcode = context.Items.FirstOrDefault(x => x.Code == Txtcode.Text);
                            if (checkcode != null)
                            {
                                SuccDiv.Visible = false;
                                ErrorDiv.Visible = true;
                                return;
                            }


                            if (string.IsNullOrWhiteSpace(selected.Value) || chk == 0)
                            {

                                var newitem = new Item
                                {
                                    ItemName = name,
                                    PurchasPrice = purchusPrice,
                                    SellPrice = sellPrice,
                                    FirstBalance = firstBalance,
                                    Code = Txtcode.Text
                                };
                                context.Items.InsertOnSubmit(newitem);
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
                            var checkcode = context.Items.FirstOrDefault(x => x.Code == Txtcode.Text&&x.Id!=id);
                            if (checkcode != null)
                            {
                                SuccDiv.Visible = false;
                                ErrorDiv.Visible = true;
                                return;
                            }


                            var detail = context.Items.FirstOrDefault(a => a.Id == id);
                            if (detail != null)
                            {
                                detail.ItemName = name;
                                detail.PurchasPrice = purchusPrice;
                                detail.SellPrice = sellPrice;
                                detail.FirstBalance = firstBalance;
                                detail.Code = Txtcode.Text;
                                context.SubmitChanges();
                                SuccDiv.Visible = true;
                                ErrorDiv.Visible = false;
                                flag.Value = "0";
                                PopulateDetails(context);
                            }
                        }
                        Txtcode.Text = "";
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
                txtName.Text = txtSellPrice.Text = txtPurchasePrice.Text = txtFirstBalance.Text = string.Empty;
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
                    var detail = contexte.Items.FirstOrDefault(a => a.Id == id);
                    if (detail == null) return;
                    txtName.Text = detail.ItemName;
                    txtSellPrice.Text = detail.SellPrice.ToString(CultureInfo.InvariantCulture);
                    txtPurchasePrice.Text = detail.PurchasPrice.ToString(CultureInfo.InvariantCulture);
                    txtFirstBalance.Text = detail.FirstBalance.ToString(CultureInfo.InvariantCulture);
                    Txtcode.Text = detail.Code;
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
                        var detail = contextd.Items.FirstOrDefault(a => a.Id == id);
                        if (detail != null)
                        {
                            contextd.Items.DeleteOnSubmit(detail);
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
 