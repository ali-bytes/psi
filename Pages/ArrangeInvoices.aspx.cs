using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class ArrangeInvoices : CustomPage
    {
       
            string _fileName = "";
            readonly ISPDataContext _context;

            public ArrangeInvoices()
            {
                _context = IspDataContext;
            }

         
            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                l_message.Text = string.Empty;
                clear_arrengedinvoice_db();
            }

            protected void clear_arrengedinvoice_db()
        {
                using (var db = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var deletecustomerinarranged = db.ArrangedInvoices.Where(a => a.isfounded == false).ToList();
                    db.ArrangedInvoices.DeleteAllOnSubmit(deletecustomerinarranged);
                    db.SubmitChanges();
                }
        }

            protected void btnArrangeInvoice_Click(object sender, EventArgs e)
            {


                /*SearchByPhones();*/

                /* String sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Server.MapPath("EgyLineWorkordersPre.xls") + ";" + "Extended Properties=Excel 8.0;";
                 OleDbConnection con = new OleDbConnection(sConnectionString);
                 OleDbCommand cmd = new OleDbCommand("Select * from  [Sheet1$]", con);
                 con.Open();*/
                //
                using (var db2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var o2 = db2.ArrangedInvoices.Where(x => x.isfounded == false).ToList();
                    if (o2.Count>0)
                    {
                        db2.ArrangedInvoices.DeleteAllOnSubmit(o2);
                        db2.SubmitChanges();
                    }
                    var extention = Path.GetExtension(fu_sheet.FileName);
                    var extensions = new List<string>{
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
                                var phone = row["Phone Number"].ToString().Trim();
                                //var GVcode = row["Governorate Code"].ToString().Trim();
                                if (string.IsNullOrWhiteSpace(phone))
                                {
                                 continue;   
                                }
                                //var phonenum = GVcode != string.Empty ? string.Format("{0}{1}", GVcode, phone) : phone;
                                var order = new List<WorkOrder>();
                                order = db2.WorkOrders.Where(p => p.CustomerPhone == phone).ToList();
                                if (order.Count > 0)
                                {
                                    var currentorder = order.FirstOrDefault();
                                    var ai = new ArrangedInvoice();
                                    ai.Customernumber = row["Customer Number"].ToString().Trim();
                                    ai.Customername = row["Customer Name"].ToString().Trim();
                                    ai.packagename = row["Package Name"].ToString().Trim();
                                    ai.Netamout = row["Netamount"].ToString().Trim();
                                    ai.Startdate = row["Start Date"].ToString().Trim();
                                    ai.Enddate = row["End Date"].ToString().Trim();
                                    ai.Description = row["Description"].ToString().Trim();
                                    ai.Phonenumber = row["Phone Number"].ToString().Trim();
                                    ai.Exchange = row["Exchange"].ToString().Trim();
                                    ai.CumstomerBranch = currentorder.Branch.BranchName;
                                    ai.CustomerStatus = currentorder.Status.StatusName;
                                    ai.isfounded = true;
                                    if (currentorder.OfferId != null)
                                    {
                                        ai.CustomerOffer = currentorder.Offer.Title;
                                    }
                                    else
                                    {
                                        ai.CustomerOffer = string.Empty;
                                    }
                                    if (currentorder.ResellerID != null)
                                    {
                                        ai.CustomerReseller = currentorder.User.UserName;
                                    }
                                    else
                                    {
                                        ai.CustomerReseller = string.Empty;
                                    }

                                    db2.ArrangedInvoices.InsertOnSubmit(ai);
                                    db2.SubmitChanges();
                                }
                                else
                                {
                                    var ai = new ArrangedInvoice();
                                    ai.Customernumber = row["Customer Number"].ToString().Trim();
                                    ai.Customername = row["Customer Name"].ToString().Trim();
                                    ai.packagename = row["Package Name"].ToString().Trim();
                                    ai.Netamout = row["Netamount"].ToString().Trim();
                                    ai.Startdate = row["Start Date"].ToString().Trim();
                                    ai.Enddate = row["End Date"].ToString().Trim();
                                    ai.Description = row["Description"].ToString().Trim();
                                    ai.Phonenumber = row["Phone Number"].ToString().Trim();
                                    ai.Exchange = row["Exchange"].ToString().Trim();
                                    ai.CumstomerBranch = string.Empty;
                                    ai.CustomerStatus = string.Empty;
                                    ai.CustomerOffer = string.Empty;
                                    ai.CustomerReseller = string.Empty;
                                    ai.isfounded = false;
                                    db2.ArrangedInvoices.InsertOnSubmit(ai);
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
                        var allcustomerinarranged = db2.ArrangedInvoices.Select(a => new
                        {
                            a.ID,
                            a.Customernumber,
                            a.Customername,
                            a.packagename,
                            a.Netamout,
                            a.Startdate,
                            a.Enddate,
                            a.Description,
                            a.Phonenumber,
                            a.Exchange,
                            a.CumstomerBranch,
                            a.CustomerReseller,
                            a.CustomerOffer,
                            a.CustomerStatus,
                            a.isfounded
                        }).ToList();


                        var newArr = new List<ArrInvoices>();


                        foreach (var cust in allcustomerinarranged)
                        {

                            var order = db2.WorkOrders.FirstOrDefault(p => p.CustomerPhone == cust.Phonenumber);
                            if (order != null)
                            {
                                var pack = db2.ServicePackages.FirstOrDefault(a => a.ID == order.ServicePackageID);

                                if (pack != null)
                                {
                                    var systemPackgeName = pack.ServicePackageName.ToString();

                                    newArr.Add(new ArrInvoices()
                                    {
                                        ID = cust.ID,
                                        Customernumber = Convert.ToInt32(string.IsNullOrEmpty(cust.Customernumber) ? "0" : cust.Customernumber),
                                        Customername = cust.Customername,
                                        packagename = cust.packagename,
                                        Netamout = cust.Netamout,
                                        Startdate = cust.Startdate,
                                        Enddate = cust.Enddate,
                                        Description = cust.Description,
                                        Phonenumber = cust.Phonenumber,
                                        Exchange = cust.Exchange,
                                        CumstomerBranch = cust.CumstomerBranch,
                                        CustomerReseller = cust.CustomerReseller,
                                        CustomerOffer = cust.CustomerOffer,
                                        CustomerStatus = cust.CustomerStatus,
                                        isfounded = cust.isfounded==null?false:true,
                                        systemPackgeName = systemPackgeName
                                    });
                                }

                            }

                        }

                       
                        GridView1.DataSource = newArr.Where(x => x.isfounded).ToList();
                        GridView1.DataBind();
                      
                        l_message.Text = string.Format(Tokens.Saved);
                        // delete from table 
                        var deletecustomerinarranged = db2.ArrangedInvoices.Where(a => a.isfounded).ToList();
                        db2.ArrangedInvoices.DeleteAllOnSubmit(deletecustomerinarranged);
                        db2.SubmitChanges();
           
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

                        /* //export NotFounded
                    BranchPrintExcel = true;
                    string attachment2 = "attachment; filename=NotFoundedInvoices.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", attachment2);
                    Response.ContentType = "application/ms-excel";

                    var sw2 = new StringWriter();
                    var htw2 = new HtmlTextWriter(sw2);
                    GridView2.RenderControl(htw2);
                    Response.Write(sw2.ToString());
                    Response.End();*/


                    }
                    else
                    {
                        l_message.Text = Tokens.PostedFileIsInvalid;
                    }
                }
            }


            protected void ExportNotFounded_click(object sender, EventArgs e)
            {
                using (var db = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var allcustomerinarranged = db.ArrangedInvoices.Select(a => new
                    {
                        a.ID,
                        a.Customernumber,
                        a.Customername,
                        a.packagename,
                        a.Netamout,
                        a.Startdate,
                        a.Enddate,
                        a.Description,
                        a.Phonenumber,
                        a.Exchange,
                        a.CumstomerBranch,
                        a.CustomerReseller,
                        a.CustomerOffer,
                        a.CustomerStatus,
                        a.isfounded
                    }).ToList();
                    var o = allcustomerinarranged.Where(x => x.isfounded == false).Where(a=>a.Customernumber!=null || a.Phonenumber!=null || a.CumstomerBranch !=null 
                        || a.CustomerOffer!=null||a.CustomerReseller!=null||a.CustomerStatus!=null||
                        a.Customername!=null||a.Description!=null||a.Enddate!=null||a.Exchange!=null||a.packagename!=null||a.Startdate!=null||a.Netamout!=null).ToList();
                  
                    GridView3.DataSource = o;
                    GridView3.DataBind();

                    var deletecustomerinarranged = db.ArrangedInvoices.Where(a => a.isfounded == false).ToList();
                    db.ArrangedInvoices.DeleteAllOnSubmit(deletecustomerinarranged);
                    db.SubmitChanges();


                }
               
               
                var gl = new GridView[] { GridView3 };
                GridHelper.Export("NotFoundedInvoices.xls", gl);
            }
          

          
        
    }
    public class ArrInvoices
    {

        public int ID { get; set; }

        public int? Customernumber { get; set; }

        public string Customername { get; set; }

        public string packagename { get; set; }

        public string Netamout { get; set; }

        public string Startdate { get; set; }

        public string Enddate { get; set; }
        public string Description { get; set; }

        public string Phonenumber { get; set; }
        public string Exchange { get; set; }
        public string CumstomerBranch { get; set; }
        public string CustomerReseller { get; set; }
        public string CustomerOffer { get; set; }
        public string CustomerStatus { get; set; }
        public bool isfounded { get; set; }
        public string systemPackgeName { get; set; }

    }

}