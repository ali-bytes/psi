using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class ManaulInvoices : CustomPage
    {
     
    readonly InvoiceService _invoiceService = new InvoiceService();


    protected void Page_Load(object sender, EventArgs e) {}


    protected void bSave_Click(object sender, EventArgs e)
    {
        l_message.Text = "";
        var currentExtension = Path.GetExtension(fu_sheet.PostedFile.FileName);
        var extensions = new List<string>{
            ".xls", ".xsls"
        };
        var file = fu_sheet.PostedFile;
        string extention = Path.GetExtension(file.FileName);
        if(string.IsNullOrWhiteSpace(extention)){
            l_message.Text = @"File Must be .xls";
            return;
        }
       

        const string invalidMessage = "Posted file is Invalid, only Files with extentions";

        if(string.IsNullOrEmpty(file.FileName)){
            l_message.Text = string.Format(invalidMessage + ": {0} or {1}", ".xls", ".xsls");
            return;
        }

        if (extensions.Any(currentExtention => currentExtention == extention))
        {
            file.SaveAs(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName)));
        }
        var cn = new OleDbConnection();
        switch(currentExtension){
            case ".xlsx" :
              cn.ConnectionString =
                                string.Format(
                                    @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("~\\Sheets") +
                                    "\\{0};Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1'", file.FileName);
                            break;

            case ".xls" :
                cn.ConnectionString =
                    string.Format(
                                  @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                                  Server.MapPath("~\\Sheets\\") +
                                  "\\{0};Extended Properties='Excel 8.0;HDR=YES;IMEX=1'", file.FileName);
                break;
        }

        var invoices = new List<Invoice>();
        try
        {
            var command = new OleDbCommand("SELECT * FROM  [Sheet1$]", cn);
            cn.Open();
            var dt = new DataTable();
            dt.Load(command.ExecuteReader());
            cn.Close();
            
            foreach (DataRow row in dt.Rows)
            {
                  invoices.Add(new Invoice
                {
                    Amount = Convert.ToDouble(row[1]),
                    Phone = row[0].ToString().Trim(),
                    StartAt = Convert.ToDateTime(row[2]),
                    EndAt = Convert.ToDateTime(row[3]),
                    ResellerCommission = Convert.ToBoolean(row[4]),
                    Note = row[5].ToString().Trim()
                });
            }
        

        //var cmd = new OleDbCommand("SELECT * FROM  [Sheet1$]", cn);
        //var da = new OleDbDataAdapter(cmd);
        //var invs = new DataTable();
        //da.Fill(invs);
        //if (invs.Columns.Count != 6)
        //{
        //    l_message.Text = @"Bad File";
        //    return;
        //}

        //var invoices = new List<Invoice>();
        //try
        //{
        //    for (var i = 0; i < invs.Rows.Count; i++)
        //    {
        //        invoices.Add(new Invoice
        //        {
        //            Amount = Convert.ToDouble(invs.Rows[i][1]),
        //            Phone = invs.Rows[i][0].ToString(),
        //            StartAt = Convert.ToDateTime(invs.Rows[i][2]),
        //            EndAt = Convert.ToDateTime(invs.Rows[i][3]),
        //            ResellerCommission = Convert.ToBoolean(invs.Rows[i][4]),
        //            Note = invs.Rows[i][5].ToString()
        //        });
        //    }
            if (File.Exists(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName))))
            {
                File.Delete(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName)));
            }
        }
        catch(Exception v){
            l_message.Text = v.Message /*"Posted File Columns are not correct"*/;
            if (File.Exists(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName))))
            {
                File.Delete(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName)));
            }
            return;
        }

        if(invoices.Count < 1){
            l_message.Text = @"There are no invoices in posted File";
            if (File.Exists(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName))))
            {
                File.Delete(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName)));
            }
            return;
        }


        #region Saving manual invoices


        var errors = _invoiceService.SaveDemands(invoices, Convert.ToInt32(Session["User_ID"]));
        var errorModel = errors.Select(x => new{
            error = x,
        });
        gv_errors.DataSource = errorModel;
        gv_errors.DataBind();

        Msgsuccess.Visible = true;

        #endregion
    }


    protected void gv_errors_DataBound(object sender, EventArgs e){
        Helper.GridViewNumbering(gv_errors, "gv_l_Number");
    }
}
}