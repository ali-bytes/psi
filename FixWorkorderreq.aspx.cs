using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using Db;
using System.Web.UI;
using System.Web.UI.WebControls;
using NewIspNL.Helpers;

namespace NewIspNL
{
    public partial class FixWorkorderreq : System.Web.UI.Page
    {
        private string _fileName = "";

        protected void Fix_OnClick(object sender, EventArgs e)
        {
            using (var db2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var extention = Path.GetExtension(fu_sheet.FileName);
                var extensions = new List<string>
            {
                ".xls",
                ".xlsx"
            };
                // saving file
                if (extensions.Any(currentExtention => currentExtention == extention))
                {
                    fu_sheet.SaveAs(Server.MapPath("~/Sheets/" + fu_sheet.PostedFile.FileName));
                    _fileName = fu_sheet.PostedFile.FileName;
                    var connectection = new OleDbConnection();
                    switch (extention)
                    {
                        case ".xlsx":
                            connectection.ConnectionString =
                                string.Format(
                                    @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("~\\Sheets") +
                                    "\\{0};Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1'", _fileName);
                            break;
                        case ".xls":
                            connectection.ConnectionString =
                                string.Format(
                                    @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Server.MapPath("~\\Sheets") +
                                    "\\{0};Extended Properties='Excel 8.0;HDR=YES;IMEX=1'", _fileName);
                            break;
                    }
                    try
                    {
                        var command = new OleDbCommand("SELECT * FROM  [Sheet1$]", connectection);
                        connectection.Open();
                        var dt = new DataTable();
                        dt.Load(command.ExecuteReader());
                        connectection.Close();
                        foreach (DataRow row in dt.Rows)
                        {
                            /*how can i get code of governmaent*/
                            var phone = row["phone"].ToString().Trim();
                            //var gVcode = Convert.ToDateTime(row["sus in"]);

                            //var phonenum = GVcode != string.Empty ? string.Format("{0}{1}", GVcode, phone) : phone;

                            var order = db2.WorkOrders.Where(p => p.CustomerPhone == phone).ToList();
                            if (order.Count > 0)
                            {
                                var currentorder = order.FirstOrDefault();
                                if (currentorder != null) currentorder.WorkOrderStatusID = 6;
                                db2.SubmitChanges();

                                if (currentorder != null)
                                {
                                    var orderstatus = new WorkOrderStatus
                                    {
                                        WorkOrderID = currentorder.ID,
                                        StatusID = 6,
                                        UpdateDate = DateTime.Now.AddHours(),
                                        UserID = 1
                                    };
                                    db2.WorkOrderStatus.InsertOnSubmit(orderstatus);
                                }
                                db2.SubmitChanges();
                            }
                        }

                        if (File.Exists(Server.MapPath("~/Sheets/" + fu_sheet.PostedFile.FileName)))
                        {
                            File.Delete(Server.MapPath("~/Sheets/" + fu_sheet.PostedFile.FileName));
                        }
                    }
                    catch
                    {
                        if (File.Exists(Server.MapPath("~/Sheets/" + fu_sheet.PostedFile.FileName)))
                        {
                            File.Delete(Server.MapPath("~/Sheets/" + fu_sheet.PostedFile.FileName));
                        }
                        
                    }
                }
            }
        }
    }
}