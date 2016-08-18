using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class HrRegister : CustomPage
    {

        readonly ISPDataContext pio = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        

        const string EntityName = "الموظف";
        List<string> extensions = new List<string> { ".JPG", ".GIF", ".JPEG", ".PNG" };

        protected void Page_Load(object sender, EventArgs e)
        {

          
                        
                if (IsPostBack)
                    return;
                MultiView1.SetActiveView(v_index);
                PopulateSuppliers();

                l_message.Text = "";


             
               
           

        }

        public string attach1()
        {
            try
            {
                 
                  string ex = Path.GetExtension(FileUpload1.FileName);

                if (!string.IsNullOrEmpty(ex) && extensions.Any(currentExtention => currentExtention == ex.ToUpper()))
                {
                    string filepath = "../Attachments/Hr/" + FileUpload1.FileName;
                    FileUpload1.SaveAs(Server.MapPath(filepath));
                    return FileUpload1.FileName;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {

                return "";
            }
        }
        public string attach2()
        {
            try
            {
                 string ex = Path.GetExtension(FileUpload2.FileName);

                if (!string.IsNullOrEmpty(ex) && extensions.Any(currentExtention => currentExtention == ex.ToUpper()))
                {
                    string filepath = "../Attachments/Hr/" + FileUpload2.FileName;
                    FileUpload2.SaveAs(Server.MapPath(filepath));
                    return FileUpload2.FileName;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {

                return "";
            }
        }
        public string attach3()
        {
            try
            {
                string ex = Path.GetExtension(FileUpload3.FileName);

                if (!string.IsNullOrEmpty(ex) && extensions.Any(currentExtention => currentExtention == ex.ToUpper()))
                {
                    string filepath = "../Attachments/Hr/" + FileUpload3.FileName;
                    FileUpload3.SaveAs(Server.MapPath(filepath));
                    return FileUpload3.FileName;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {

                return "";
            }
        }
        public string attach4()
        {
            try
            {
                string ex = Path.GetExtension(FileUpload4.FileName);

                if (!string.IsNullOrEmpty(ex) && extensions.Any(currentExtention => currentExtention == ex.ToUpper()))
                {
                    string filepath = "../Attachments/Hr/" + FileUpload4.FileName;
                    FileUpload4.SaveAs(Server.MapPath(filepath));
                    return FileUpload4.FileName;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {

                return "";
            }
        }

        void PopulateSuppliers()
        {
            try
            {
                var roles = pio.Employes.ToList().OrderBy(g => g.Name);
                GvItems.DataSource = roles.Select(r => new
                {
                    r.Id,
                    r.Name,
                    r.Mobile,
                    r.Code,
                    r.Phone,
                    r.Email,
                    r.Grade,
                    r.GradeYear,
                    r.Spacific,
                    r.rent,
                    r.Attendance,
                    r.Leave,
                    r.LastDiscount,
                    r.HiringDate,
                    r.Attach1,
                    r.Attach2,
                    r.Attach3,
                    r.Attach4
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
            hf_id.Value = string.Empty;
        }
        protected void b_new_Click(object sender, EventArgs e)
        {
            MultiView1.SetActiveView(v_AddEdit);
        }


        protected void b_save_Click(object sender, EventArgs e)
        {
            try
            {
              
                Employe item;
                if (hf_id.Value == string.Empty)
                {
                      var code = Convert.ToInt32(tb_code.Text);
                var check = pio.Employes.FirstOrDefault(x => x.Code == code);
                if (check != null)
                {
                    l_message.Text = string.Format(" عفوا كود البصمة مستخدم لموظف اخر");
                return;
                }
                    item = new Employe
                    {
                        Name = tb_name.Text,
                        Address = TbAddress.Text,
                        Code = Convert.ToInt32((string)tb_code.Text),
                        Phone = TbPhone.Text,
                        Mobile = TbMobile.Text,
                        Email = TbEmail.Text,
                        Grade = TbGrade.Text,
                        Spacific = TbSpacific.Text,
                        GradeYear = GradeYear.Text,
                        rent = Convert.ToDecimal((string)Tbrent.Text),
                        Leave = Convert.ToInt32((string)TbLeave.Text),
                        Attendance = Convert.ToInt32((string)TbAttendance.Text),
                        LastDiscount = Convert.ToInt32((string)TbDiscount.Text),
                        HiringDate =Convert.ToDateTime(TextBox5.Text),
                        Attach1 = attach1(),
                        Attach2 = attach2(),
                        Attach3 = attach3(),
                        Attach4 = attach4(),
                        Days = Convert.ToInt32(days.Text),
                        insurance = Convert.ToDecimal(ins.Text)


                    };

                   

                    pio.Employes.InsertOnSubmit(item);

                }
                else
                {
                    var code = Convert.ToInt32(tb_code.Text);
                    var check = pio.Employes.FirstOrDefault(x => x.Code == code && x.Id != Convert.ToInt32((string)hf_id.Value));
                    if (check != null)
                    {
                        l_message.Text = string.Format(" عفوا كود البصمة مستخدم لموظف اخر");
                        return;
                    }
                    item = pio.Employes.FirstOrDefault(z=>z.Id==Convert.ToInt32((string)hf_id.Value));
                    if (item != null)
                    {
                        item.Name = tb_name.Text;
                        item.Code = Convert.ToInt32((string)tb_code.Text);
                        item.Address = TbAddress.Text;
                        item.Phone = TbPhone.Text;
                        item.Mobile = TbMobile.Text;
                        item.Email = TbEmail.Text;
                        item.Grade = TbGrade.Text;
                        item.Spacific = TbSpacific.Text;
                        item.GradeYear = GradeYear.Text;
                        item.rent = Convert.ToDecimal((string)Tbrent.Text);
                        item.Leave = Convert.ToDouble((string)TbLeave.Text);
                        item.Attendance = Convert.ToDouble((string)TbAttendance.Text);
                        item.LastDiscount = Convert.ToDouble((string)TbDiscount.Text);
                        item.HiringDate =  Convert.ToDateTime(TextBox5.Text);
                      
                        item.Days = Convert.ToInt32(days.Text);
                        item.insurance = Convert.ToDecimal(ins.Text);

                        if (FileUpload1.HasFile)
                        {
                            item.Attach1 = attach1();

                        }
                   

                        if (FileUpload2.HasFile)
                        {
                            item.Attach2 = attach2();

                        }
                      

                        if (FileUpload3.HasFile)
                        {
                            item.Attach3 = attach3();

                        }
                        if (FileUpload4.HasFile)
                        {
                            item.Attach4 = attach4();

                        }
                      
                     
                    }
                }
                pio.SubmitChanges();
                Clear();
                PopulateSuppliers();
                if (item != null) l_message.Text = string.Format(" تم حفظ {0} ({1})", EntityName, item.Name);
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
            tb_name.Text = TbAddress.Text = GradeYear.Text = GradeYear.Text = TbPhone.Text = TbGrade.Text = TbEmail.Text = TbGrade.Text = TbMobile.Text = string.Empty;

            Tbrent.Text = "0";
            TbDiscount.Text = "0";
            TbAttendance.Text = "0";
            TbLeave.Text = "0";
        }


        protected void gv_index_DataBound(object sender, EventArgs e)
        {
         Helper.GridViewNumbering(GvItems, "l_number");
        }


        protected void gvb_edit_Click(object sender, EventArgs e)
        {
            try
            {
                MultiView1.SetActiveView(v_AddEdit);
                var buttonSender = sender as Button;
                if (buttonSender == null)
                    return;
                var id = Convert.ToInt32(buttonSender.CommandArgument);
                hf_id.Value = id.ToString(CultureInfo.InvariantCulture);
                var item = pio.Employes.FirstOrDefault(z=>z.Id==id);
                if (item == null) return;
                tb_name.Text = item.Name;
                tb_code.Text = item.Code.ToString();
                TbAddress.Text = item.Address;
                TbPhone.Text = item.Phone;
                TbMobile.Text = item.Mobile;
                TbEmail.Text = item.Email;
                TbSpacific.Text = item.Spacific;
                TbGrade.Text = item.Grade;
                GradeYear.Text = item.GradeYear;
                Tbrent.Text =Addons. Helpers.FixNumberFormat(item.rent);
                TbAttendance.Text = item.Attendance.ToString();
                TbLeave.Text = item.Leave.ToString();
                TbDiscount.Text = item.LastDiscount.ToString();
                TextBox5.Text = item.HiringDate.ToString("yyyy-MM-dd");
                LinkButton4.Text = item.Attach1;
                LinkButton5.Text = item.Attach2;
                LinkButton6.Text = item.Attach3;
                LinkButton7.Text = item.Attach4;
                days.Text = item.Days.ToString();
                ins.Text = item.insurance.ToString();
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
                var supplier =
                       pio.Employes.FirstOrDefault(z=>z.Id==Convert.ToInt32(button.CommandArgument));

                int id = Convert.ToInt32(supplier.Id);

                var check =
                           (from g in pio.HrDayes where g.EmployeeId == id select g)
                               .FirstOrDefault();
                var check1 =
                          (from g in pio.EmployeeSettings where g.EmplyeeId == id select g)
                              .FirstOrDefault();
                var check2 =
                        (from g in pio.EmployeeAssemplies where g.EmployeeId == id select g)
                            .FirstOrDefault();
                var check3 =
                       (from g in pio.EmployeeDebits where g.employeeId == id select g)
                           .FirstOrDefault();
                var check4 =
                   (from g in pio.EmployeeSalaries where g.employeeId == id select g)
                       .FirstOrDefault();


                if (check != null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('هذا الموظف له عمليات من قبل')", true);
                }
                else if (check1 != null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('هذا الموظف له عمليات من قبل')", true);

                }
                else if (check2 != null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('هذا الموظف له عمليات من قبل')", true);
                }
                else if (check3 != null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('هذا الموظف له عمليات من قبل')", true);
                }
                else if (check4 != null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('هذا الموظف له عمليات من قبل')", true);
                }
                else
                {

                    if (button != null)
                    {
                        if (supplier == null)
                            return;
                        l_message.Text = string.Format(" تم حذف {0} ({1})", EntityName, supplier.Name);
                        pio.Employes.DeleteOnSubmit(supplier);
                        pio.SubmitChanges();
                    }

                    PopulateSuppliers();
                }
            }
            catch (Exception)
            {


                txtError.Visible = true;

                txtError.Text = "عفوا يوجد خطأ في البيانات";
            }

        }







        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = "../Attachments/Hr/" + LinkButton4.Text;
                Response.ContentType = "image/jpg";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + LinkButton4.Text + "\"");
                Response.TransmitFile(Server.MapPath(filePath));
                Response.End();
            }
            catch (Exception)
            {

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('عفوا هذا الملف غير موجود')", true);

            }

        }
        protected void LinkButton5_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = "../Attachments/Hr/" + LinkButton5.Text;
                Response.ContentType = "image/jpg";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + LinkButton5.Text + "\"");
                Response.TransmitFile(Server.MapPath(filePath));
                Response.End();
            }
            catch (Exception)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('عفوا هذا الملف غير موجود')", true);


            }

        }
        protected void LinkButton6_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = "../Attachments/Hr/" + LinkButton6.Text;
                Response.ContentType = "image/jpg";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + LinkButton6.Text + "\"");
                Response.TransmitFile(Server.MapPath(filePath));
                Response.End();
            }
            catch (Exception)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('عفوا هذا الملف غير موجود')", true);


            }

        }
        protected void LinkButton7_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = "../Attachments/Hr/" + LinkButton7.Text;
                Response.ContentType = "image/jpg";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + LinkButton7.Text + "\"");
                Response.TransmitFile(Server.MapPath(filePath));
                Response.End();
            }
            catch (Exception)
            {

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('عفوا هذا الملف غير موجود')", true);

            }

        }



        protected void ins_TextChanged(object sender, EventArgs e)
        {

        }
    }
}