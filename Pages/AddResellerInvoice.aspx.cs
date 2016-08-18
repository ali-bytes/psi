using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using NewIspNL.Helpers;
using Db;
using Resources;

namespace NewIspNL.Pages
{
    public partial class AddResellerInvoice : CustomPage
    {
        
            protected void Page_Load(object sender, EventArgs e)
            {

            }
            protected void btnAdd_Click(object sender, EventArgs e)
            {
                using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    #region Load file

                    //var path = Server.MapPath("~/Sheets/");
                    var currentExtension = Path.GetExtension(fu_sheet.PostedFile.FileName);
                    var extensions = new List<string>
            {
                ".xls",
                ".xlsx"
            };

                    var user = Convert.ToInt32(Session["User_ID"]);

                    #endregion

                    if (extensions.Any(currentExtention => currentExtention == currentExtension))
                    {
                        //  var file=Helper.LoadExcelFile(fu_sheet, extensions, path, this, "PaymentInvoices");

                        var file = fu_sheet.PostedFile;
                        var extention = Path.GetExtension(file.FileName);
                        if (extensions.Any(currentExtention => currentExtention == extention))
                        {
                            file.SaveAs(Server.MapPath("~/Sheets/" + file.FileName));

                        }

                        #region reject file


                        string invalidMessage = Tokens.WrongExcelFormat;

                        if (file == null)
                        {
                            l_message.Text = string.Format("{2}: {0} {3} {1}", ".xls", ".xsls", invalidMessage, Tokens.Colon);
                            if (File.Exists(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName))))
                            {
                                File.Delete(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName)));
                            }
                            return;
                        }

                        #endregion

                        #region Validate sheet

                        //var provider = ExcelProvider.Create(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName)),
                        //"Sheet1");
                        var sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Server.MapPath("~/Sheets/" + file.FileName) + ";" + "Extended Properties=Excel 8.0;";
                        var con = new OleDbConnection(sConnectionString);
                        var cmd = new OleDbCommand("Select * from  [Sheet1$]", con);
                        con.Open();
                        var dt = new DataTable();
                        var read = cmd.ExecuteReader();
                        if (read != null) dt.Load(read);
                        con.Close();
                        var neWorkOrders = dt.Rows;
                        try
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                if (string.IsNullOrEmpty(row["Reseller"].ToString()) ||
                                    string.IsNullOrEmpty(row["Amount"].ToString())) continue;
                                var resellerName = row["Reseller"].ToString().Trim();
                                var amount = row["Amount"].ToString().Trim();
                                var resellerDb = dataContext.Users.FirstOrDefault(a => a.UserName == resellerName);
                                if (resellerDb == null) continue;

                                var userTransaction = new UsersTransaction
                                {
                                    CreationDate = DateTime.Now.AddHours(),
                                    DepitAmmount = Convert.ToDouble(amount),
                                    CreditAmmount = 0,
                                    IsInvoice = false,
                                    ResellerID = resellerDb.ID,
                                    Total =
                                        Billing.GetLastBalance(Convert.ToInt32(resellerDb.ID), "Reseller") +
                                        Convert.ToDouble(amount),
                                    UserId = user,
                                    Notes = "اضافة فاتورة لموزع يدوى",
                                    Description = "Add"
                                };
                                dataContext.UsersTransactions.InsertOnSubmit(userTransaction);
                                dataContext.SubmitChanges();
                            }
                            l_message.Text = Tokens.Saved;
                            if (File.Exists(Server.MapPath("~/Sheets/" + file.FileName)))
                            {
                                File.Delete(Server.MapPath("~/Sheets/" + file.FileName));
                            }
                        }
                        catch (Exception)
                        {
                            l_message.Text = Tokens.WrongColoumnsCount;
                            if (File.Exists(Server.MapPath("~/Sheets/" + file.FileName)))
                            {
                                File.Delete(Server.MapPath("~/Sheets/" + file.FileName));
                            }
                            return;
                        }

                        if (neWorkOrders.Count >= 1) return;
                        l_message.Text = Tokens.NoInvoices;

                        #endregion

                    }
                }
            }
        }
    }
  