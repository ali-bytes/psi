using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
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
    public partial class AddCustomersManual : CustomPage
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

                //var user = Convert.ToInt32(Session["User_ID"]);

                #endregion

                if (extensions.Any(currentExtention => currentExtention == currentExtension))
                {
                    //  var file=Helper.LoadExcelFile(fu_sheet, extensions, path, this, "PaymentInvoices");

                    var file = fu_sheet.PostedFile;
                    var extention = Path.GetExtension(file.FileName);
                    if (string.IsNullOrEmpty(extention))
                    {
                        return;
                    }
                    if (extensions.Any(currentExtention => currentExtention == extention))
                    {
                        file.SaveAs(Server.MapPath("~/Sheets/" + file.FileName));

                    }

                    #region reject file


                    string invalidMessage = Tokens.WrongExcelFormat;

                    if (file == null)
                    {
                        l_message.Text = string.Format("{2}: {0} {3} {1}", ".xls", ".xsls", invalidMessage, Tokens.Colon);
                        return;
                    }

                    #endregion

                    #region Validate sheet

                    //var provider = ExcelProvider.Create(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName)),
                    //"Sheet1");
                    var con = new OleDbConnection();
                    switch (currentExtension)
                    {
                        case ".xlsx":
                            con.ConnectionString =
                                              string.Format(
                                                  @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("~\\Sheets") +
                                                  "\\{0};Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1'", file.FileName);
                            break;

                        case ".xls":
                            con.ConnectionString =
                                string.Format(
                                              @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                                              Server.MapPath("~\\Sheets\\") +
                                              "\\{0};Extended Properties='Excel 8.0;HDR=YES;IMEX=1'", file.FileName);
                            break;
                    }

                    //var sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Server.MapPath("~/Sheets/" + file.FileName) + ";" + "Extended Properties=Excel 8.0;";
                   
                    var cmd = new OleDbCommand("Select * from  [Sheet1$]", con);
                    con.Open();
                    var dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    con.Close();
                    var neWorkOrders = dt.Rows;
                    var notCompleteOrders = new List<WorkOrder>();
                    try
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (!string.IsNullOrEmpty(row["CustomerName"].ToString()) &&
                                !string.IsNullOrEmpty(row["ServiceProvider"].ToString())
                                && !string.IsNullOrEmpty(row["ServicePackage"].ToString()) &&
                                !string.IsNullOrEmpty(row["IpPackage"].ToString()) &&
                                !string.IsNullOrEmpty(row["PaymentType"].ToString())
                                && !string.IsNullOrEmpty(row["Branch"].ToString()) &&
                                !string.IsNullOrEmpty(row["WorkOrderStatus"].ToString()) &&
                                !string.IsNullOrEmpty(row["Central"].ToString()) &&
                                !string.IsNullOrEmpty(row["CreationDate"].ToString()) &&
                                !string.IsNullOrEmpty(row["request date"].ToString()))
                            {
                                var phone = row["CustomerPhone"].ToString().Trim();
                                var central = row["Central"].ToString().Trim();
                                var name = row["CustomerName"].ToString();
                                var centralDb = dataContext.Centrals.FirstOrDefault(a => a.Name == central);
                                if (string.IsNullOrEmpty(phone) || centralDb == null)
                                {
                                    var order = new WorkOrder { CustomerName = name, CustomerPhone = phone };
                                    notCompleteOrders.Add(order);
                                    continue;
                                }

                                var checkifCustomerExist =
                                    dataContext.WorkOrders.Where(
                                        a => a.CustomerPhone == phone && a.CustomerGovernorateID == centralDb.GovernateId);
                                if (checkifCustomerExist.Any())
                                {
                                    var order = new WorkOrder { CustomerName = name, CustomerPhone = phone };
                                    notCompleteOrders.Add(order);
                                    continue;
                                }
                                var wo = new WorkOrder { CustomerName = name };

                                if (centralDb != null) wo.CentralId = centralDb.Id; else continue;
                                wo.CustomerGovernorateID = centralDb.GovernateId;
                                //var gover = row["CustomerGovernorate"].ToString().Trim();
                                //var governmentfromDb =
                                // dataContext.Governorates.FirstOrDefault(a => a.GovernorateName==gover);
                                //if (governmentfromDb != null) wo.CustomerGovernorateID = governmentfromDb.ID;else continue;
                                wo.CustomerPhone = phone;
                                wo.CustomerAddress = row["CustomerAddress"].ToString().Trim();
                                wo.CustomerMobile = row["CustomerMobile"].ToString().Trim();
                                wo.CustomerEmail = row["CustomerEmail"].ToString().Trim();
                                var provider = row["ServiceProvider"].ToString().Trim();
                                var serviceProviderDb =
                                    dataContext.ServiceProviders.FirstOrDefault(a => a.SPName == provider);
                                if (serviceProviderDb != null) wo.ServiceProviderID = serviceProviderDb.ID; else continue;
                                var package = row["ServicePackage"].ToString().Trim();
                                var packageDb =
                                    dataContext.ServicePackages.FirstOrDefault(a => a.ServicePackageName == package);
                                if (packageDb != null) wo.ServicePackageID = packageDb.ID; else continue;
                                var ipPackage = row["IpPackage"].ToString().Trim();
                                var ipPackageDb =
                                    dataContext.IpPackages.FirstOrDefault(a => a.IpPackageName == ipPackage);
                                if (ipPackageDb != null) wo.IpPackageID = ipPackageDb.ID; else continue;
                                var paymentType = row["PaymentType"].ToString().Trim();
                                var paymentTypeDb =
                                    dataContext.PaymentTypes.FirstOrDefault(a => a.PaymentTypeName == paymentType);
                                if (paymentTypeDb != null) wo.PaymentTypeID = paymentTypeDb.ID; else continue;
                                if (row["Reseller"] != DBNull.Value)
                                {
                                    var reseller = row["Reseller"].ToString().Trim();
                                    var resellerDb = dataContext.Users.FirstOrDefault(a => a.UserName == reseller.Trim());
                                    if (resellerDb != null) wo.ResellerID = resellerDb.ID;
                                    else wo.ResellerID = null;
                                }
                                else
                                {
                                    wo.ResellerID = null;

                                }
                                var branch = row["Branch"].ToString().Trim();
                                var branchDb = dataContext.Branches.FirstOrDefault(a => a.BranchName == branch);
                                if (branchDb != null) wo.BranchID = branchDb.ID; else continue;
                                wo.VPI = row["VPI"].ToString().Trim();
                                wo.VCI = row["VCI"].ToString().Trim();
                                wo.UserName = row["UserName"].ToString().Trim();
                                wo.Password = row["Password"].ToString();
                                wo.Notes = row["Notes"].ToString().Trim();
                                var status = row["WorkOrderStatus"].ToString().Trim();
                                var statusDb = dataContext.Status.FirstOrDefault(a => a.StatusName == status);
                                if (statusDb != null) wo.WorkOrderStatusID = statusDb.ID; else continue;
                                wo.WorkorderDate = Convert.ToDateTime(row["WorkOrderDate"]);
                                wo.CreationDate = Convert.ToDateTime(row["CreationDate"]);
                                //wo.OrderNumebr = row["OrderNumber"].ToString();
                                if (row["Offer"] != DBNull.Value)
                                {
                                    var offer = row["Offer"].ToString().Trim();
                                    var offerDb = dataContext.Offers.FirstOrDefault(a => a.Title == offer);
                                    if (offerDb != null) wo.OfferId = offerDb.Id; else wo.OfferId = null;

                                }
                                else
                                {
                                    wo.OfferId = null;
                                }

                                //wo.RepresentativeId = Convert.ToInt32(row["RepresentativeId"]);
                                wo.PersonalId = row["PersonalId"].ToString();
                                //wo.ProductId = Convert.ToInt32(row["ProductId"]);
                                //wo.CustomerTypeId = Convert.ToInt32(row["CustomerTypeId"]);
                                // wo.ResellerCustomerTypeId = 1;
                                wo.RequestNumber = row["request number"].ToString();
                                wo.RouterSerial = row["router serial"].ToString();
                                wo.LineOwner = row["line owner"].ToString();
                                //wo.Prepaid = Convert.ToDecimal(row["Prepaid"].ToString());
                                wo.RequestDate = Convert.ToDateTime(row["request date"]);
                                wo.OfferStart = Convert.ToDateTime(row["offerstart"]);
                                dataContext.WorkOrders.InsertOnSubmit(wo);
                                dataContext.SubmitChanges();

                                var ws = new global::Db.WorkOrderStatus
                                {
                                    WorkOrderID = wo.ID,
                                    StatusID = statusDb.ID,
                                    UserID = 1,
                                    UpdateDate = Convert.ToDateTime(row["Activation Date"])
                                };
                                dataContext.WorkOrderStatus.InsertOnSubmit(ws);
                                dataContext.SubmitChanges();

                            }
                            else
                            {
                                var name = row["CustomerName"].ToString();
                                var phone = row["CustomerPhone"].ToString();
                                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(phone))
                                {
                                    var order = new WorkOrder { CustomerName = name, CustomerPhone = phone };
                                    notCompleteOrders.Add(order);
                                }
                            }
                        }
                        l_message.Text = Tokens.Saved;
                        GvUnComplete.DataSource = notCompleteOrders;
                        GvUnComplete.DataBind();

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

                    if (neWorkOrders.Count < 1)
                    {
                        l_message.Text = Tokens.NoWorkorders;
                        //return;
                    }

                    #endregion

                }

                #region Old Method

                /*var dataContext = new ISPDataContext(ConfigurationManager.ConnectionStrings["roboticsegConnectionString"].ToString());
        String sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Server.MapPath("WorkordersRB.xls") + ";" + "Extended Properties=Excel 8.0;";
        OleDbConnection con = new OleDbConnection(sConnectionString);
        OleDbCommand cmd = new OleDbCommand("Select * from  [Sheet1$]", con);
        con.Open();
        DataTable dt = new DataTable();
        dt.Load(cmd.ExecuteReader());
        con.Close();
        int i = 0;
        foreach (DataRow row in dt.Rows)
        {

            WorkOrder wo = new WorkOrder();
            wo.CustomerName = row["CustomerName"].ToString().Trim();
            wo.CustomerGovernorateID = Convert.ToInt32(row["CustomerGovernorateID"]);
            wo.CustomerPhone = row["CustomerPhone"].ToString().Trim();
            wo.CustomerAddress = row["CustomerAddress"].ToString().Trim();
            wo.CustomerMobile = row["CustomerMobile"].ToString().Trim();
            wo.CustomerEmail = row["CustomerEmail"].ToString().Trim();
            wo.ServiceProviderID = Convert.ToInt32(row["ServiceProviderID"]);
            wo.ServicePackageID = Convert.ToInt32(row["ServicePackageID"]);
            wo.IpPackageID = Convert.ToInt32(row["IpPackageID"]);
            wo.PaymentTypeID = Convert.ToInt32(row["PaymentTypeID"]);

            if (row["ResellerID"] != DBNull.Value)
            {
                wo.ResellerID = Convert.ToInt32(row["ResellerID"].ToString());
              
            }
            else
            {
                wo.ResellerID = null;
               
            }
            wo.BranchID = Convert.ToInt32(row["BranchID"]);
            wo.VPI = row["VPI"].ToString().Trim();
            wo.VCI = row["VCI"].ToString().Trim();
            wo.UserName = row["UserName"].ToString().Trim();
            wo.Password = row["Password"].ToString();
            wo.Notes = row["Notes"].ToString().Trim();
            wo.WorkOrderStatusID = Convert.ToInt32(row["WorkOrderStatusID"]);
            wo.WorkorderDate = Convert.ToDateTime(row["WorkOrderDate"]);
            wo.CreationDate = Convert.ToDateTime(row["CreationDate"]);
            //wo.OrderNumebr = row["OrderNumber"].ToString();
            if (row["Offer"] != DBNull.Value)
            {
                wo.OfferId = Convert.ToInt32(row["Offer"]);

            }
            else
            {
                wo.OfferId = null;
            }
           
            wo.CentralId = Convert.ToInt32(row["CentralID"]);
            //wo.RepresentativeId = Convert.ToInt32(row["RepresentativeId"]);
            wo.PersonalId = row["PersonalId"].ToString();
            //wo.ProductId = Convert.ToInt32(row["ProductId"]);
            //wo.CustomerTypeId = Convert.ToInt32(row["CustomerTypeId"]);
           // wo.ResellerCustomerTypeId = 1;
            wo.RequestNumber = row["request number"].ToString();
            wo.RouterSerial = row["router serial"].ToString();
            wo.LineOwner = row["line owner"].ToString();
            //wo.Prepaid = Convert.ToDecimal(row["Prepaid"].ToString());
            wo.RequestDate = Convert.ToDateTime(row["request date"]);
            wo.OfferStart = Convert.ToDateTime(row["offerstart"]);
          dataContext.WorkOrders.InsertOnSubmit(wo);
            dataContext.SubmitChanges();

            WorkOrderStatus ws = new WorkOrderStatus();
            ws.WorkOrderID = wo.ID;
            ws.StatusID = Convert.ToInt32(row["WorkOrderStatusID"]);
            ws.UserID = 1;
            ws.UpdateDate = Convert.ToDateTime(row["CreationDate"]);
            dataContext.WorkOrderStatus.InsertOnSubmit(ws);
            dataContext.SubmitChanges();*/

                #endregion
            }
        }
        protected void GvUnp_OnDataBound(object sender, EventArgs e)
        {
            Helper.GridViewNumbering(GvUnComplete, "LNo");
        }
        protected void GvUpdate_OnDataBound(object sender, EventArgs e)
        {
            Helper.GridViewNumbering(GvUpdate, "LNo");
        }

        protected void UpdateCustomer(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                #region Load file

                //var path = Server.MapPath("~/Sheets/");
                var currentExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                var extensions = new List<string>
            {
                ".xls",
                ".xlsx"
            };

                //var user = Convert.ToInt32(Session["User_ID"]);

                #endregion

                if (extensions.Any(currentExtention => currentExtention == currentExtension))
                {
                    //  var file=Helper.LoadExcelFile(fu_sheet, extensions, path, this, "PaymentInvoices");

                    var file = FileUpload1.PostedFile;
                    var extention = Path.GetExtension(file.FileName);
                    if (string.IsNullOrEmpty(extention))
                    {
                        return;
                    }
                    if (extensions.Any(currentExtention => currentExtention == extention))
                    {
                        file.SaveAs(Server.MapPath("~/Sheets/" + file.FileName));

                    }

                    #region reject file


                    var invalidMessage = Tokens.WrongExcelFormat;

                    if (file == null)
                    {
                        lblMsg.Text = string.Format("{2}: {0} {3} {1}", ".xls", ".xsls", invalidMessage, Tokens.Colon);
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
                    var reader = cmd.ExecuteReader();
                    if (reader != null) dt.Load(reader);
                    con.Close();
                    var neWorkOrders = dt.Rows;
                    var notCompleteOrders = new List<WorkOrder>();
                    try
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (!string.IsNullOrEmpty(row["CustomerPhone"].ToString()))
                            {
                                //!string.IsNullOrEmpty(row["UserName"].ToString())
                                //&& !string.IsNullOrEmpty(row["Password"].ToString()) &&
                                //!string.IsNullOrEmpty(row["VPI"].ToString()) &&
                                //!string.IsNullOrEmpty(row["VCI"].ToString())
                                var phone = row["CustomerPhone"].ToString().Trim();
                                var userName = row["UserName"].ToString().Trim();
                                var passWord = row["Password"].ToString().Trim();
                                var vpi = row["VPI"].ToString().Trim();
                                var vci = row["VCI"].ToString().Trim();

                                var routerSerial = row["RouterSerial"].ToString().Trim();
                                var requestNumber = row["RequestNumber"].ToString().Trim();
                                var email = row["Email"].ToString().Trim();
                                var mobile = row["Mobile"].ToString().Trim();
                                //var centralDb = dataContext.Centrals.FirstOrDefault(a => a.Name == central);
                                var orders =
                                    context.WorkOrders.Where(a => a.CustomerPhone == phone);
                                if (!orders.Any())
                                {
                                    var order = new WorkOrder { CustomerName = userName, CustomerPhone = phone };
                                    notCompleteOrders.Add(order);
                                    continue;
                                }
                                //var wo = new WorkOrder();
                                var thisOrder = orders.FirstOrDefault();
                                if (thisOrder == null) continue;
                                if (row["UserName"] != DBNull.Value)
                                {
                                    thisOrder.UserName = userName;
                                }
                                if (row["Password"] != DBNull.Value)
                                {
                                    thisOrder.Password = passWord;
                                }
                                if (row["VPI"] != DBNull.Value)
                                {
                                    thisOrder.VPI = vpi;
                                }
                                if (row["VCI"] != DBNull.Value)
                                {
                                    thisOrder.VCI = vci;
                                }
                                if (row["RouterSerial"] != DBNull.Value)
                                {
                                    thisOrder.RouterSerial = routerSerial;
                                }
                                if (row["RequestNumber"] != DBNull.Value)
                                {
                                    thisOrder.RequestNumber = requestNumber;
                                }
                                if (row["Email"] != DBNull.Value)
                                {
                                    thisOrder.CustomerEmail = email;
                                }
                                if (row["Mobile"] != DBNull.Value)
                                {
                                    thisOrder.CustomerMobile = mobile;
                                }

                                context.SubmitChanges();

                            }
                            else
                            {
                                //var name = row["CustomerName"].ToString();
                                var phone = row["CustomerPhone"].ToString();
                                if (!string.IsNullOrWhiteSpace(phone))
                                {
                                    var order = new WorkOrder {CustomerPhone = phone };
                                    notCompleteOrders.Add(order);
                                }
                            }
                        }
                        l_message.Text = Tokens.Saved;

                        GvUpdate.DataSource = notCompleteOrders;
                        GvUpdate.DataBind();
                        if (File.Exists(Server.MapPath("~/Sheets/" + file.FileName)))
                        {
                            File.Delete(Server.MapPath("~/Sheets/" + file.FileName));
                        }
                    }
                    catch (Exception)
                    {
                        lblMsg.Text = Tokens.WrongColoumnsCount;
                        if (File.Exists(Server.MapPath("~/Sheets/" + file.FileName)))
                        {
                            File.Delete(Server.MapPath("~/Sheets/" + file.FileName));
                        }
                        
                        return;
                    }

                    if (neWorkOrders.Count < 1)
                    {
                        lblMsg.Text = Tokens.NoWorkorders;
                    }

                    #endregion

                }
            }
        }
    }
}
