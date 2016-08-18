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
    public partial class HrSetting : CustomPage
    {
        readonly ISPDataContext pio = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        
        const string EntityName = "أعدادات الموظف";

 

        protected void Page_Load(object sender, EventArgs e)
        {

                if (IsPostBack)
                    return;
                //  Authenticate.Authenticated();
                MultiView1.SetActiveView(v_index);
                PopulateEmployess();
                Populatesettings();
                l_message.Text = "";


                
           


        }

        private void Populatesettings()
        {
            try
            {
                var setting = pio.EmployeeSettings.ToList();
                GvItems.DataSource = setting.ToList().Select(r => new
                {
                    r.Id,
                   pio.Employes.FirstOrDefault(x=>x.Id==r.EmplyeeId).Name,
                    EmployeeDayAbsValue = r.EmployeeDayAbs!= 0 ? r.EmployeeDayAbs.ToString() + "ساعة" : r.EmployeeDayAbsValue.ToString() + " جنية ",
                    EmployeeHalfAbdsValue = r.EmployeeHalfAbds != 0 ? r.EmployeeHalfAbds.ToString() + "ساعة" : r.EmployeeHalfAbdsValue.ToString() + " جنية ",

                    EmployeeAddValue = r.EmployeeAdd != 0 ? r.EmployeeAdd.ToString() + " ساعة " : r.EmployeeAddValue.ToString() + " جنية "
                });

                GvItems.DataBind();
            }
            catch (Exception)
            {


                txtError.Visible = true;

                txtError.Text = "عفوا يوجد خطأ في البيانات";
            }


        }

        private void PopulateEmployess()
        {
            try
            {
                var employee = pio.Employes.ToList();
                TbEmployeeSelect.DataSource = employee;
                TbEmployeeSelect.DataTextField = "Name";
                TbEmployeeSelect.DataValueField = "Id";
                TbEmployeeSelect.DataBind();
              Addons.  Helpers.AddDefaultItem(TbEmployeeSelect);
            }
            catch (Exception)
            {


                txtError.Visible = true;

                txtError.Text = "عفوا يوجد خطأ في البيانات";
            }

        }

      
        protected void BSave_OnClick(object sender, EventArgs e)
        {
            try
            {
               
                     string daynum = "0";
                     string dayval = "0";

                     string halfDaynum = "0";
                     string halfDay = "0";

                     string addvalnum = "0";
                     string addValue = "0";


                if (RadioButtonList1.SelectedValue == "1")
                {
                    daynum = AbsDayPound.Text;
                }
                else
                {
                    dayval = AbsDayPound.Text;
                }

                if (RadioButtonList2.SelectedValue == "1")
                {
                    halfDaynum = TbHalfDaySelect.Text;
                }
                else
                {
                    halfDay = TbHalfDaySelect.Text;
                }

                if (RadioButtonList3.SelectedValue == "1")
                {
                    addvalnum = TbAddHourText.Text;
                }
                else
                {
                    addValue = TbAddHourText.Text;
                }
                EmployeeSetting employee;

                if (hf_id.Value == string.Empty)
                {
                    var emp =
                   pio.EmployeeSettings.FirstOrDefault(
                       z => z.EmplyeeId == Convert.ToInt32((string)TbEmployeeSelect.SelectedValue));
                    if (emp != null)
                    {
                        l_message.Text = "هذا الموظف له اعدادات ";
                        return;
                    }
                    employee = new EmployeeSetting()
                    {
                        
                        EmplyeeId = Convert.ToInt32((string)TbEmployeeSelect.SelectedValue),
                        EmployeeDayAbs = Convert.ToInt32((string)daynum),
                        EmployeeDayAbsValue = dayval,
                        EmployeeHalfAbds = Convert.ToInt32((string)halfDaynum),
                        EmployeeHalfAbdsValue = halfDay,
                        EmployeeAdd = Convert.ToInt32((string)addvalnum),
                        EmployeeAddValue = addValue

                    };
                    pio.EmployeeSettings.InsertOnSubmit(employee);
                }
                else
                {
                    employee = pio.EmployeeSettings.FirstOrDefault(x=>x.Id==Convert.ToInt32((string)hf_id.Value));

                    if (employee != null)
                    {
                        employee.EmplyeeId = Convert.ToInt32((string)TbEmployeeSelect.SelectedValue);
                        employee.EmployeeDayAbs = Convert.ToInt32((string)daynum);
                        employee.EmployeeDayAbsValue = dayval;
                        employee.EmployeeHalfAbds = Convert.ToInt32((string)halfDaynum);
                        employee.EmployeeHalfAbdsValue = halfDay;
                        employee.EmployeeAdd = Convert.ToInt32((string)addvalnum);
                        employee.EmployeeAddValue = addValue;
                
                    }
                }
                pio.SubmitChanges();
                TbEmployeeSelect.Enabled = true;
                Clear();
                Populatesettings();
                if (employee != null) l_message.Text = string.Format(" تم حفظ {0} ({1})", EntityName, pio.Employes.FirstOrDefault(z=>z.Id==employee.EmplyeeId).Name);
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
            AbsDayvalue.Value = HalfDayValue.Value = TbHalfDaySelect.Text = TbAddHourText.Text = AbsDayPound.Text = string.Empty;
        }
        protected void b_new_OnClick(object sender, EventArgs e)
        {
           
            MultiView1.SetActiveView(v_AddEdit);


        }

        protected void gvb_edit_OnClick(object sender, EventArgs e)
        {
            try
            {
                TbEmployeeSelect.Enabled = false;
                MultiView1.SetActiveView(v_AddEdit);
                var buttonSender = sender as Button;
                if (buttonSender == null)
                    return;
                var id = Convert.ToInt32(buttonSender.CommandArgument);
                hf_id.Value = id.ToString(CultureInfo.InvariantCulture);
                var emp = pio.EmployeeSettings.FirstOrDefault(z=>z.Id==id);
                if (emp == null) return;
                TbEmployeeSelect.SelectedValue = emp.EmplyeeId.ToString();
                AbsDayvalue.Value = emp.EmployeeDayAbs.ToString();
                HalfDayValue.Value = emp.EmployeeHalfAbds.ToString();
                AddValue.Value = emp.EmployeeAdd.ToString();

                if (AbsDayvalue.Value == "0")
                {
                    AbsDayPound.Text = emp.EmployeeDayAbsValue;
                    RadioButtonList1.Items.FindByValue("2").Selected = true;
                }
                else
                {
                    AbsDayPound.Text = emp.EmployeeDayAbs.ToString();
                    RadioButtonList1.Items.FindByValue("1").Selected = true;
                }

                if (HalfDayValue.Value == "0")
                {
                    TbHalfDaySelect.Text = emp.EmployeeHalfAbdsValue;
                    RadioButtonList2.Items.FindByValue("2").Selected = true;
                }
                else
                {
                    TbHalfDaySelect.Text = emp.EmployeeDayAbs.ToString();
                    RadioButtonList2.Items.FindByValue("1").Selected = true;
                }

                if (AddValue.Value == "0")
                {
                    TbAddHourText.Text = emp.EmployeeAddValue;
                    RadioButtonList3.Items.FindByValue("2").Selected = true;
                }
                else
                {
                    TbAddHourText.Text = emp.EmployeeDayAbs.ToString();
                    RadioButtonList3.Items.FindByValue("1").Selected = true;
                }

            }
            catch (Exception)
            {


                txtError.Visible = true;

                txtError.Text = "عفوا يوجد خطأ في البيانات";
            }

        }

        protected void GvItems_OnDataBound(object sender, EventArgs e)
        {
            Helper.GridViewNumbering(GvItems, "l_number");
        }

        protected void BBack_OnClick(object sender, EventArgs e)
        {
            Clear();
            MultiView1.SetActiveView(v_index);
        }
    }
}