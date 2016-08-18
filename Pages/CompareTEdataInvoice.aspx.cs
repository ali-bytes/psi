using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class CompareTEdataInvoice : CustomPage
    {
        
            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                l_message.Text = string.Empty;
                //PrepareInputs();
            }
           
            protected void btnArrangeInvoice_Click(object sender, EventArgs e)
            {
               
                using (var db2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var extention = Path.GetExtension(fu_sheet.FileName);
                    var extensions = new List<string>{
                ".xls",
                ".xlsx"
            };
                    // saving file
                    if (extensions.Any(currentExtention => currentExtention == extention))
                    {
                        fu_sheet.SaveAs(Server.MapPath("~/Sheets/" + fu_sheet.PostedFile.FileName));
                        string fileName = fu_sheet.PostedFile.FileName;
                        var connectection = new OleDbConnection();
                        switch (extention)
                        {
                            case ".xlsx":
                                connectection.ConnectionString =
                                    string.Format(
                                                  @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("~\\Sheets") +
                                                  "\\{0};Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1'", fileName);
                                break;
                            case ".xls":
                                connectection.ConnectionString =
                                    string.Format(
                                                  @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Server.MapPath("~\\Sheets") +
                                                  "\\{0};Extended Properties='Excel 8.0;HDR=YES;IMEX=1'", fileName);
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
                                // ip package 
                                /*how can i get code of governmaent*/
                                var phone = row["Phone Number"].ToString().Trim();
                                var startAt = row["Start Date"].ToString().Trim();
                                //var GVcode = row["Governorate Code"].ToString().Trim();
                                //var phonenum = GVcode != string.Empty ? string.Format("{0}{1}", GVcode, phone) : phone;
                                //var order = new List<WorkOrder>();
                                if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(startAt)) continue;
                                var order = db2.WorkOrders.Where(p => p.CustomerPhone == phone).ToList();
                                var ai = new CompareTEData
                                {
                                    CustomerNumber = row["Phone Number"].ToString().Trim(),
                                    CustomerName = row["Customer Name"].ToString().Trim(),
                                    PackageName = row["Package Name"].ToString().Trim(),
                                    NetAmout = row["Netamount"].ToString().Trim(),
                                    StartAt = startAt,
                                    EndAt = row["End Date"].ToString().Trim(),
                                    Description = row["Description"].ToString().Trim(),
                                    PhoneNumber = phone,
                                    Exchange = row["Exchange"].ToString().Trim()
                                };
                                if (order.Count > 0)
                                {
                                    var currentorder = order.FirstOrDefault();
                                    var demand = db2.Demands.Where(a => a.WorkOrderId == currentorder.ID).OrderByDescending(a => a.Id);
                                    //var lastdemand = demand.FirstOrDefault(a => a.StartAt.Month == Convert.ToDateTime(startAt).Month);

                                    // new addition 8/9/2015
                                    //ai.PackageName.Trim() == "ADSL Option Pack 1 - 1 Month"
                                    var lastdemand = new Demand();
                                    if (ai.PackageName.Trim() == "Extra ADSL Usage")
                                    {
                                        lastdemand = demand.FirstOrDefault(a => a.StartAt.Month == Convert.ToDateTime(startAt).Month && a.StartAt.Year == Convert.ToDateTime(startAt).Year && a.Notes.Trim().Contains("جيجات اضافية"));
                                    }
                                    else if (ai.PackageName.Contains("ADSL Option Pack"))
                                    {
                                        lastdemand = demand.FirstOrDefault(a => a.StartAt.Month == Convert.ToDateTime(startAt).Month && a.StartAt.Year == Convert.ToDateTime(startAt).Year && a.Notes.Trim() == "IP Package");
                                    }
                                    else
                                    {
                                        lastdemand = demand.FirstOrDefault(a => a.StartAt.Month == Convert.ToDateTime(startAt).Month && a.StartAt.Year == Convert.ToDateTime(startAt).Year && a.Notes.Trim() != "IP Package" && a.Notes.Trim().Contains("جيجات اضافية") == false);
                                    }

                                   //-------------------------
                                    
                                    if (ai.NetAmout==null)
                                    {
                                        ai.NetAmout = "0";
                                    }
                                    //new table Comapre TEdata
                                    if (lastdemand == null) continue;
                                    //if(lastdemand.Amount==Convert.ToDecimal(ai.OurAmount))
                                    ai.OurAmount = lastdemand.Amount.ToString(CultureInfo.InvariantCulture);
                                    ai.OurStartAt = lastdemand.StartAt.ToString(CultureInfo.InvariantCulture);
                                    ai.OurEndAt = lastdemand.EndAt.ToString(CultureInfo.InvariantCulture);
                                    ai.IsDiffrent = lastdemand.Amount != Convert.ToDecimal(ai.NetAmout);
                                    db2.CompareTEDatas.InsertOnSubmit(ai);
                                    db2.SubmitChanges();
                                }
                                else
                                {
                                    db2.CompareTEDatas.InsertOnSubmit(ai);
                                    db2.SubmitChanges();
                                }
                                l_message.Text = string.Format(Tokens.Saved, order.Count);
                            }
                            if (File.Exists(Server.MapPath("~/Sheets/" + fu_sheet.PostedFile.FileName)))
                            {
                                File.Delete(Server.MapPath("~/Sheets/" + fu_sheet.PostedFile.FileName));
                            }
                        }
                        catch (Exception exception)
                        {
                            l_message.Text = exception.Message;
                            if (File.Exists(Server.MapPath("~/Sheets/" + fu_sheet.PostedFile.FileName)))
                            {
                                File.Delete(Server.MapPath("~/Sheets/" + fu_sheet.PostedFile.FileName));
                            }
                            return;
                        }
                        var allcustomerinarranged = db2.CompareTEDatas.ToList();
                        /*{.Select(a => new
                            a.Id,
                            a.CustomerNumber,
                            a.CustomerName,
                            a.PackageName,
                            a.NetAmout,
                            a.StartAt,
                            a.EndAt,
                            a.Description,
                            a.PhoneNumber,
                            a.Exchange,
                            a.OurAmount,
                            a.OurEndAt,
                            a.OurStartAt,
                            a.IsDiffrent
                        }).ToList();*/
                        GridView1.DataSource = allcustomerinarranged;
                        GridView1.DataBind();

                        l_message.Text = string.Format(Tokens.Saved);
                        // delete from table 
                        // var deletecustomerinarranged = db2.CompareTEDatas.ToList();
                        db2.CompareTEDatas.DeleteAllOnSubmit(allcustomerinarranged);
                        db2.SubmitChanges();

                        // export

                        //var branchPrintExcel = true;
                        string attachment = "attachment; filename=ArrangedInvoice.xls";
                        Response.ClearContent();
                        Response.AddHeader("content-disposition", attachment);
                        Response.ContentType = "application/ms-excel";
                        Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");

                        var sw = new StringWriter();
                        var htw = new HtmlTextWriter(sw);
                        GridView1.RenderControl(htw);
                        Response.Write(sw.ToString());
                        Response.End();
                    }
                    else
                    {
                        l_message.Text = Tokens.PostedFileIsInvalid;
                    }
                }
            }
        }
    }
 