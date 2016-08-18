using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using NewIspNL.Service.Hr;

namespace NewIspNL.Pages
{
    public partial class HrDayes : CustomPage
    {
        readonly ISPDataContext pio = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        
        protected void Page_Load(object sender, EventArgs e)
        {
            PopulateEmployess();
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
                Addons.Helpers.AddDefaultItem(tb_name);
                dbStates.DataSource=  pio.EmployeeStates.ToList();
                dbStates.DataTextField = "Name";
                dbStates.DataValueField = "Id";
                dbStates.DataBind();
                Addons.Helpers.AddDefaultItem(dbStates);
            }
            catch (Exception)
            {
                txtError.Visible = true;
                txtError.Text = "عفوا يوجد خطأ في البيانات";
            }

        }
        protected void BSave_Click(object sender, EventArgs e)
        {
            var extensions = new List<string> { ".XLSX", ".XLS" };
            string filePath = "";
            if (FileUploadControl.HasFile)
            {
                filePath = Server.MapPath(string.Format("~/Attachments/Hr/{0}", FileUploadControl.PostedFile.FileName));
            }
            try
            {
                if (FileUploadControl.HasFile)
                {
                    string extension = Path.GetExtension(FileUploadControl.PostedFile.FileName);
                    if (!string.IsNullOrEmpty(extension) &&
                        extensions.Any(currentExtention => currentExtention == extension.ToUpper()))
                    {
                        string fileName = Path.GetFileName(FileUploadControl.PostedFile.FileName);
                        //string extension = Path.GetExtension(FileUploadControl.PostedFile.FileName);
                        FileUploadControl.SaveAs(filePath);
                        GetData(filePath, extension, "true", fileName);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                    }
                }
            }
            catch (Exception)
            {


                txtError.Visible = true;

                txtError.Text = "عفوا يوجد خطأ في البيانات";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

        }

        private void GetData(string filePath, string extension, string isHdr, string fileName)
        {
            try
            {
                var tt = new DateTime();
                var dif = Convert.ToDouble(tt.AddHours());
                string conStr = "";
                switch (extension)
                {
                    case ".xls": //Excel 97-03
                        //conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        conStr = string.Format(
                                                  @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Server.MapPath("~\\Attachments\\Hr") +
                                                  "\\{0};Extended Properties='Excel 8.0;HDR=YES;IMEX=1'", fileName);
                        break;
                    case ".xlsx": //Excel 07
                        //conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        conStr = string.Format(
                             @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("~\\Attachments\\Hr") +
                                                  "\\{0};Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1'", fileName);
                                                 
                        break;
                }
                //conStr = String.Format(conStr, filePath, isHdr);
                var connExcel = new OleDbConnection(conStr);
                var cmdExcel = new OleDbCommand();
                var oda = new OleDbDataAdapter();
                var dt = new DataTable();
                cmdExcel.Connection = connExcel;

                //Get the name of First Sheet
                connExcel.Open();
                DataTable dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dtExcelSchema != null)
                {
                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                    connExcel.Close();

                    //Read Data from First Sheet
                    connExcel.Open();
                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";

                    oda.SelectCommand = cmdExcel;
                    oda.Fill(dt);
                    connExcel.Close();


                    var rec = dt.Select();


                    foreach (var dataRow in rec)
                    {

                        var d = dataRow.ItemArray.GetValue(3).ToString().Split('/');
                        var test = string.Format("{0}/{1}/{2}", d[1], d[0], d[2]);

                        DateTime date;
                        if (DateTime.TryParse(test, out date))
                        {
                            int daynum = Convert.ToInt32(d[1]);
                            int month = Convert.ToInt32(d[0]);



                            if (date.Day == daynum && date.Month == month)
                            {
                                DataRow row = dataRow;
                                int code = Convert.ToInt32(row.ItemArray.GetValue(0).ToString());
                                var day =
                                    pio.HrDayes.FirstOrDefault(e =>
                                                e.Time.Day == daynum && e.Time.Month == month &&
                                                e.Time.Year == date.Year && e.Code == code);

                                var employee = pio.Employes.FirstOrDefault(c => c.Code == code);
                                if (employee != null)
                                {
                                    string mint = "0";
                                    var ss = date.Minute;
                                    if (ss < 10)
                                    {
                                        mint = "0" + ss;
                                    }
                                    else
                                    {
                                        mint = ss.ToString();
                                    }

                                    var time = Convert.ToDouble(string.Format("{0}.{1}", date.Hour, mint));


                                    if (day == null)
                                    {
                                        day = new HrDaye { Time = date.AddHours(dif), EmployeeId = employee.Id, Code = code };
                                    }
                                    if (row.ItemArray.GetValue(4).ToString().Trim().ToLower() == "C/In".ToLower().Trim())
                                    {
                                        if (day.Attendance == null || day.Attendance > time)
                                        {
                                            day.Attendance = time;
                                            day.StateId = 1;
                                        }

                                    }
                                    else if (row.ItemArray.GetValue(4).ToString().Trim().ToLower() == "C/Out".ToLower().Trim())
                                    {
                                        if (day.Leave == null || time > day.Leave)
                                        {
                                            day.Leave = time;
                                            day.StateId = 2;
                                        }

                                    }


                                    pio.HrDayes.InsertOnSubmit(day);

                                    string message = "تم الرفع بنجاح";
                                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                                    sb.Append("<script type = 'text/javascript'>");
                                    sb.Append("window.onload=function(){");
                                    sb.Append("alert('");
                                    sb.Append(message);
                                    sb.Append("')};");
                                    sb.Append("</script>");
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                                }
                                else
                                {
                                    string message = "تاكد من البيانات هل صحيحة ام لا ؟ ";
                                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                                    sb.Append("<script type = 'text/javascript'>");
                                    sb.Append("window.onload=function(){");
                                    sb.Append("alert('");
                                    sb.Append(message);
                                    sb.Append("')};");
                                    sb.Append("</script>");
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                                }
                            }
                        }
                    }
                }


            }
            catch (Exception)
            {
                
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                txtError.Visible = true;

                txtError.Text = "عفوا يوجد خطأ في البيانات";
            }



        }


    }
}