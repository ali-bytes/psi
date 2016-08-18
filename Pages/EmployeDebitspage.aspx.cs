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
using NewIspNL.Service;
using NewIspNL.Service.Enums;

namespace NewIspNL.Pages
{
    public partial class EmployeDebitspage : CustomPage
    {
        const string EntityName = "مسحوبات";
       
        readonly ISPDataContext pio = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        

        protected void Page_Load(object sender, EventArgs e)
        {
        
              
 

                if (IsPostBack)
                    return;
                MultiView1.SetActiveView(v_index);
                PopulateSuppliers();
                PopulateEmployess();
                Populatesaves();
                l_message.Text = "";
           
               

        }

        void PopulateEmployess()
        {
            try
            {
            
                var emp = pio.Employes.ToList();
                tb_name.DataSource = emp;
                tb_name.DataTextField = "Name";
                tb_name.DataValueField = "Id";
                tb_name.DataBind();
              Addons.  Helpers.AddDefaultItem(tb_name);
            }
            catch (Exception)
            {

                txtError.Visible = true;

                txtError.Text = "عفوا يوجد خطأ في البيانات";
            }

        }
        void Populatesaves()
        {
            try
            {
                    int userId = Convert.ToInt32(Session["User_ID"]);
                var emp = (from d in pio.UserSaves
                           where d.UserId == userId
                           select new
                           {
                               d.Save.SaveName,
                               d.SaveId

                           }).ToList();
                DropDownList1.DataSource = emp;
                DropDownList1.DataTextField = "SaveName";
                DropDownList1.DataValueField = "SaveId";
                DropDownList1.DataBind();
                Addons.Helpers.AddDefaultItem(DropDownList1);
            }
            catch (Exception)
            {

                txtError.Visible = true;

                txtError.Text = "عفوا يوجد خطأ في البيانات";
            }

        }
        void PopulateSuppliers()
        {
            try
            {
                var roles = pio.EmployeeDebits.ToList().OrderBy(g => g.employeeId);
                GvItems.DataSource = roles.Select(r => new
                {
                    r.Id,
                    pio.Employes.FirstOrDefault(x=>x.Id==r.employeeId).Name,
                    Increase = Convert.ToDecimal(r.Debit),
                    Time = r.Time.ToShortDateString(),
                    note = r.note

                });
                GvItems.DataBind();
            }
            catch (Exception)
            {

                txtError.Visible = true;

                txtError.Text = "عفوا يوجد خطأ في البيانات";
            }

        }

        protected void BBack_OnClick(object sender, EventArgs e)
        {
            Clear();
            MultiView1.SetActiveView(v_index);
        }
        protected void b_new_Click(object sender, EventArgs e)
        {
            MultiView1.SetActiveView(v_AddEdit);
        }


        protected void b_save_Click(object sender, EventArgs e)
        {
            try
            {
                
                EmployeeDebit item;
                if (hf_id.Value == string.Empty)
                {
                    var amount = Convert.ToDecimal((string)TbIncrease.Text);
                    int userId = Convert.ToInt32(Session["User_ID"]);
                    var username = pio.Users.FirstOrDefault(x => x.ID == userId);
                    ICreditBusiness business = new CreditBusiness();
                    int saveid = Convert.ToInt32(DropDownList1.SelectedValue);
                    var result = business.Pay(amount, saveid);
                    switch (result)
                    {
                        case CreditOperationResult.Success:
                            item = new EmployeeDebit
                            {
                                employeeId = Convert.ToInt32((string)tb_name.SelectedValue),
                                Debit = Convert.ToDecimal((string)TbIncrease.Text),
                                Time = Convert.ToDateTime(TextBox5.Text).AddHours(),
                                saveid = saveid,
                                note = TextBox1.Text

                            };
                            pio.EmployeeDebits.InsertOnSubmit(item);

                            business.AddTreasuryMovement(DateTime.Now, amount, saveid, userId, " مسحوبات موظف", 1, username.UserName);

                            if (item != null) l_message.Text = string.Format(" تم حفظ {0} ({1})", EntityName, item.Debit);
                            break;
                        case CreditOperationResult.LessCredit:
                            l_message.Text = "لايوجد رصيد كافى";
                            break;
                        default:
                            l_message.Text = result.ToString();
                            break;
                    }
                }
                else
                {
                    int saveid = Convert.ToInt32(DropDownList1.SelectedValue);
                    item = pio.EmployeeDebits.FirstOrDefault(z=>z.Id==Convert.ToInt32((string)hf_id.Value));
                    if (item != null)
                    {
                        item.employeeId = Convert.ToInt32((string)tb_name.SelectedValue);
                        item.Debit = Convert.ToDecimal((string)TbIncrease.Text);
                        item.Time = Convert.ToDateTime(TextBox5.Text).AddHours();
                        item.saveid = saveid;
                        item.note = TextBox1.Text;
                        pio.EmployeeDebits.InsertOnSubmit(item);
                    }
                }
                pio.SubmitChanges();
                Clear();
                PopulateSuppliers();

                hf_id.Value = string.Empty;
                MultiView1.SetActiveView(v_index);
            }
            catch (Exception)
            {

                txtError.Visible = true;

                txtError.Text = "عفوا يوجد خطأ في البيانات";
            }

        }


        void Clear()
        {
            TbIncrease.Text = string.Empty;
        }


        protected void gv_index_DataBound(object sender, EventArgs e)
        {
            Helper.GridViewNumbering(GvItems, "l_number");
        }

        protected void gvb_delete_Click(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Button;
                if (button != null)
                {
                    int id = Convert.ToInt32(button.CommandArgument);
                    var supplier = (from d in pio.EmployeeDebits
                                    where d.Id == id
                                    select d).FirstOrDefault();
                    if (supplier == null)
                        return;

                    var amount = Convert.ToDecimal(supplier.Debit);
                    ICreditBusiness business = new CreditBusiness();

                    int userId = Convert.ToInt32(Session["User_ID"]);
                 
                    var username = pio.Users.FirstOrDefault(x => x.ID == userId);
                    int saveid = (from d in pio.EmployeeDebits
                                  where d.Id == id
                                  select d.saveid).FirstOrDefault();
                    var result = business.Receive(amount, saveid);
                    switch (result)
                    {
                        case CreditOperationResult.Success:


                            pio.EmployeeDebits.DeleteOnSubmit(supplier);
                            business.AddTreasuryMovement(DateTime.Now, amount, saveid, userId, " رد مسحوب موظف", 1, username.UserName);
                            l_message.Text = string.Format(" تم حذف {0} ({1})", EntityName, supplier.Debit);
                            break;
                        case CreditOperationResult.LessCredit:
                            l_message.Text = result.ToString();
                            break;
                        default:
                            l_message.Text = result.ToString();
                            break;
                    }


                }
                pio.SubmitChanges();
                PopulateSuppliers();

            }
            catch (Exception)
            {

                txtError.Visible = true;

                txtError.Text = "عفوا يوجد خطأ في البيانات";
            }
        }
    }
}