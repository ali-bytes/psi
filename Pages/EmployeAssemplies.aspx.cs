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

namespace NewIspNL.Pages
{
    public partial class EmployeAssemplies : CustomPage
    {
        readonly ISPDataContext context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
                
        const string EntityName = "";


        protected void Page_Load(object sender, EventArgs e)
        {
           
           

                if (IsPostBack)
                    return;
                MultiView1.SetActiveView(v_index);
                PopulateSuppliers();
                PopulateEmployess();
                l_message.Text = "";

               

           


        }

        void PopulateEmployess()
        {
            try
            {
                var emp = context.Employes.ToList();
                tb_name.DataSource = emp;
                tb_name.DataTextField = "Name";
                tb_name.DataValueField = "Id";
                tb_name.DataBind();
               Addons. Helpers.AddDefaultItem(tb_name);
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
                var roles = context.EmployeeAssemplies.ToList().OrderBy(g => g.EmployeeId);
                GvItems.DataSource = roles.Select(r => new
                {
                    r.Id,
                    context.Employes.FirstOrDefault(x=>x.Id==r.EmployeeId).Name,
                    Type = r.Type == 1 ? "سفريات" : "تركيبات",
                    Increase = Convert.ToDecimal(r.Increase),
                    Time= r.Time.ToShortDateString()
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

                tb_name.Enabled = true;
                EmployeeAssemply item;
                if (hf_id.Value == string.Empty)
                {
                    item = new EmployeeAssemply
                    {
                        EmployeeId = Convert.ToInt32(tb_name.SelectedValue),
                        Increase = Convert.ToDecimal(TbIncrease.Text),
                        Time = DateTime.Now.AddHours(),
                        Type = Convert.ToInt32(TbType.SelectedValue)

                    };
                    context.EmployeeAssemplies.InsertOnSubmit(item);
                }
                else
                {
                    item = context.EmployeeAssemplies.FirstOrDefault(x=>x.Id==Convert.ToInt32(hf_id.Value));
                    if (item != null)
                    {
                        item.EmployeeId = Convert.ToInt32(tb_name.SelectedValue);
                        item.Increase = Convert.ToDecimal(TbIncrease.Text);
                        item.Time = DateTime.Now.AddHours();
                        item.Type = Convert.ToInt32(TbType.SelectedValue);
                      
                    }
                }
                context.SubmitChanges();
                Clear();
                PopulateSuppliers();
                if (item != null) l_message.Text = string.Format(" تم حفظ {0} ({1})", EntityName, TbType.SelectedItem);
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


        protected void gvb_edit_Click(object sender, EventArgs e)
        {
            try
            {
                tb_name.Enabled = false;
                MultiView1.SetActiveView(v_AddEdit);
                var buttonSender = sender as Button;
                if (buttonSender == null) return;
                var id = Convert.ToInt32(buttonSender.CommandArgument);
                hf_id.Value = id.ToString(CultureInfo.InvariantCulture);
                var item = context.EmployeeAssemplies.FirstOrDefault(x=>x.Id==id);
                if (item == null) return;
                tb_name.SelectedValue = item.EmployeeId.ToString();
                TbType.SelectedValue = item.Type.ToString();
                TbIncrease.Text =Addons. Helpers.FixNumberFormat(item.Increase);

            }
            catch (Exception)
            {


                txtError.Visible = true;

                txtError.Text = "عفوا يوجد خطأ في البيانات";
            }

        }



        protected void gvb_delete_Click(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Button;
                if (button != null)
                {
                    var supplier = context.EmployeeAssemplies.FirstOrDefault(x=>x.Id==Convert.ToInt32(button.CommandArgument));
                    if (supplier == null)
                        return;
                    l_message.Text = string.Format(" تم حذف {0} ({1})", EntityName, supplier.Type);
                    context.EmployeeAssemplies.DeleteOnSubmit(supplier);

                }
                context.SubmitChanges();
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