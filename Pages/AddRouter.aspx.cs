using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;
using NewIspNL.Helpers;
using Resources;
using System.Data;

namespace NewIspNL.Pages
{
    public partial class AddRouter : CustomPage
    {
         ISPDataContext _context;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                PopulateGrid(context);
                PopulateStore(context);
            }
            
        }
        void PopulateStore(ISPDataContext context)
        {
            // using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var allstores = context.Stores.ToList();
                ddlStores.DataSource = allstores;
                ddlStores.DataTextField = "StoreName";
                ddlStores.DataValueField = "Id";
                ddlStores.DataBind();
                Helper.AddDefaultItem(ddlStores);
            }
        }

        private void PopulateGrid(ISPDataContext context)
        {
            var routers = context.RecieveRouters.Where(a => a.IsRecieved == false).Select(a => new
            {
                a.Id,
                a.RouterSerial,
                a.RouterType,
                a.Store.StoreName,
                a.IsRecieved
            }).ToList();
            grdRouters.DataSource = routers;
            grdRouters.DataBind();
        }

        protected void grdRouters_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Helper.GridViewNumbering(grdRouters, "lbl_No");
        }

        protected void BtnAddClick(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var check = context.RecieveRouters.Where(z => z.RouterSerial == txtRouterSerial.Text).Select(z => z).ToList();
                if (check.Count > 0)
                {
                    message.InnerHtml = Tokens.SavingError + " سيريال الراوتر موجود من قبل";
                    message.Attributes.Add("class", "alert alert-success");
                }
                else
                {
                    if (string.IsNullOrEmpty(txtRouterSerial.Text) || string.IsNullOrEmpty(txtRouterType.Text)) return;
                    var userId = Convert.ToInt32(Session["User_ID"]);
                    var storeId = Convert.ToInt32(ddlStores.SelectedItem.Value);
                    var receivefromcompany = new RecieveRouter
                    {
                        RouterSerial = txtRouterSerial.Text.Trim(),
                        RouterType = txtRouterType.Text.Trim(),
                        CompanyConfirmerUserId = userId,
                        CompanyProcessDate = DateTime.Now.AddHours(),
                        StoreId = storeId,
                        IsRecieved = false,
                    };
                    context.RecieveRouters.InsertOnSubmit(receivefromcompany);
                    context.SubmitChanges();
                    message.InnerHtml = Tokens.Saved;
                    message.Attributes.Add("class", "alert alert-success");
                    PopulateGrid(context);
                    Clear();
                }
            }
        }

        void Clear()
        {
            txtRouterSerial.Text = txtRouterType.Text = string.Empty;
            ddlStores.SelectedIndex = 0;
        }
        protected void grd_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdRouters.EditIndex = -1;
            Clear();
            btnAddRouter.Visible = true;
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                PopulateGrid(context);
            }
        }
        protected void grd_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var dataKey = grdRouters.DataKeys[e.RowIndex];
            if (dataKey == null) return;
            var id = Convert.ToInt32(dataKey["Id"]);
            DeleteRouter(id);

        }

        private void DeleteRouter(int id)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var selectedRouter = context.RecieveRouters.FirstOrDefault(a => a.Id == id && a.IsRecieved == false);
                if (selectedRouter == null) return;
                context.RecieveRouters.DeleteOnSubmit(selectedRouter);
                context.SubmitChanges();
                PopulateGrid(context);
                message.InnerHtml = Tokens.Deleted;
                message.Attributes.Add("class", "alert alert-success");
            }
        }


        protected void grd_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdRouters.EditIndex = e.NewEditIndex;
            var dataKey = grdRouters.DataKeys[e.NewEditIndex];
            if (dataKey != null)
            {
                var id = dataKey.Value;
                using (var context2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var firstOrDefault = context2.RecieveRouters.FirstOrDefault(x => x.Id == int.Parse(id.ToString()));
                    if (firstOrDefault != null)
                    {
                        txtRouterSerial.Text = firstOrDefault.RouterSerial;
                        txtRouterType.Text = firstOrDefault.RouterType;
                        ddlStores.SelectedValue = firstOrDefault.StoreId.ToString();
                    }
                    PopulateGrid(context2);
                }
            }
            btnAddRouter.Visible = false;
        }
        protected void grd_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var dataKey = grdRouters.DataKeys[e.RowIndex];
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (dataKey != null)
                {
                    var id = Convert.ToInt32(dataKey["Id"]);
                    var userId = Convert.ToInt32(Session["User_ID"]);
                    var query = context.RecieveRouters.FirstOrDefault(item => item.Id == id);
                    if (query == null) return;
                    query.RouterSerial = txtRouterSerial.Text.Trim();
                    query.RouterType = txtRouterType.Text.Trim();
                    query.StoreId = Convert.ToInt32(ddlStores.SelectedItem.Value);
                    query.CompanyConfirmerUserId = userId;
                    //query.CompanyProcessDate = DateTime.Now.AddHours();
                    context.SubmitChanges();
                }


                grdRouters.EditIndex = -1;
                Clear();
                btnAddRouter.Visible = true;
                PopulateGrid(context);
            }
        }


        protected void BDel_OnClick(object sender, EventArgs e)
        {
            var btn = sender as LinkButton;
            if (btn == null) return;

            var id = Convert.ToInt32(btn.CommandArgument);
            DeleteRouter(id);
        }

        protected void BtnAddFromExcel(object sender, EventArgs e)
        {
            var currentExtension = Path.GetExtension(f_sheet.PostedFile.FileName);
            var extensions = new List<string>{
            ".xls"
        };
            var file = f_sheet.PostedFile;
            string extention = Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(extention))
            {
                l_message.Text = @"File Must be .xls";
                return;
            }
            if (extensions.Any(currentExtention => currentExtention == extention))
            {
                file.SaveAs(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName)));
            }

            const string invalidMessage = "Posted file is Invalid, only Files with extentions";

            if (string.IsNullOrEmpty(file.FileName))
            {
                l_message.Text = string.Format(invalidMessage + ": {0} or {1}", ".xls", ".xsls");
                return;
            }


            var cn = new OleDbConnection();
            switch (currentExtension)
            {
                case ".xlsx":
                    l_message.Text = @"File Must be .xls";
                    return;

                case ".xls":
                    cn.ConnectionString =
                        string.Format(
                                      @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                                      Server.MapPath("~\\Sheets\\") +
                                      "\\{0};Extended Properties='Excel 8.0;HDR=YES;IMEX=1'", file.FileName);
                    break;
            }

            //string query = string.Format("Select * from [{0}]", file.FileName);

            var cmd = new OleDbCommand("SELECT * FROM  [Sheet1$]", cn);
            //var cmd = new OleDbCommand(query, cn);
            var da = new OleDbDataAdapter(cmd);
            var invs = new DataTable();
            da.Fill(invs);
            if (invs.Columns.Count != 3)
            {
                l_message.Text = @"Bad File";
                return;
            }

            var invoices = new List<Invoice>();
            try
            {

                var userId = Convert.ToInt32(Session["User_ID"]);
                var routersList = new List<RecieveRouter>();

                for (var i = 0; i < invs.Rows.Count; i++)
                {
                    routersList.Add(new RecieveRouter
                    {
                        RouterSerial = invs.Rows[i][0].ToString(),
                        RouterType = invs.Rows[i][1].ToString(),
                        StoreId =  GetStoreID((invs.Rows[i][2]).ToString()),
                        CompanyConfirmerUserId = userId,
                        CompanyProcessDate = (DateTime)DateTime.Now.AddHours(),
                        IsRecieved = false
                    });
                }

                if (routersList.Count <=0)
                {
                    if (File.Exists(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName))))
                    {
                        File.Delete(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName)));
                    }
                    return;
                }
                var errors = new List<string>();
                var storeErrors = new List<string>();
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    foreach (var r in routersList)
                    {
                        var check =
                            context.RecieveRouters.Where(z => z.RouterSerial == r.RouterSerial)
                                .Select(z => z)
                                .FirstOrDefault();

                        

                        if (check!= null)
                        {
                            errors.Add(check.RouterSerial);
                            continue; 
                        }else if (r.StoreId == 0)
                        {
                            storeErrors.Add(r.RouterSerial);
                            continue; 
                        }
                        else
                        {
                            context.RecieveRouters.InsertOnSubmit(r);
                            context.SubmitChanges();
                        }
                    }
                }
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    PopulateGrid(context);
                }
                message.InnerHtml = Tokens.Saved;
                message.Attributes.Add("class", "alert alert-success");
               

                var errorModel = errors.Select(x => new
                {
                    error = x
                });
                gv_errors.DataSource = errorModel;
                gv_errors.DataBind();

                var sterrorModel = storeErrors.Select(x => new
                {
                    error = x
                });
                gv_storeError.DataSource = sterrorModel;
                gv_storeError.DataBind();
                if (File.Exists(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName))))
                {
                    File.Delete(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName)));
                }
               

            }
            catch (Exception v)
            {
                l_message.Text = v.Message /*"Posted File Columns are not correct"*/;
                if (File.Exists(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName))))
                {
                    File.Delete(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName)));
                }
                return;
            }

            //if (routersList.Count < 1)
            //{
            //    l_message.Text = @"There are no invoices in posted File";
            //    return;
            //}


        }

        private int GetStoreID(string storeName)
        {
            _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
           
           var st= _context.Stores.Where(a => a.StoreName.Equals(storeName)).Select(s => s.Id).FirstOrDefault();
           if (st!=null)
            {
                return st;
            }
           else
           {
               return 0;
           }
            
        }

        protected void gv_errors_DataBound(object sender, EventArgs e)
        {
            Helper.GridViewNumbering(gv_errors, "gv_l_Number");
        }

        protected void gv_storeError_DataBound(object sender, EventArgs e)
        {
            Helper.GridViewNumbering(gv_storeError, "gv_2_Number");
        }

       

        

    }
}
