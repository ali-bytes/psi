using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;
using Resources;
using ScipBe.Common.Office;
namespace NewIspNL.Pages
{
    public partial class ManualPayment : CustomPage
    {
      
    readonly InvoiceService _invoiceService=new InvoiceService();
    private readonly IUserSaveRepository _userSave =new UserSaveRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateSaves();
            }
        }


    protected void bSave_Click(object sender, EventArgs e){
        #region Load file
        var path=Server.MapPath("~/Sheets/");
        var currentExtension=Path.GetExtension(fu_sheet.PostedFile.FileName);
        var extensions=new List<string>{
            ".xls",
            ".xlsx"
        };

        var user=Convert.ToInt32(Session["User_ID"]);
      #endregion
        if(extensions.Any(currentExtention => currentExtention == currentExtension)){
       

            var file = fu_sheet.PostedFile;
            var extention = Path.GetExtension(file.FileName);
            if (extention == null)
            {
                return;
            }
            if (extensions.Any(currentExtention => currentExtention == extention))
            {
                file.SaveAs(Server.MapPath("~/Sheets/"+ file.FileName));
                
            }

            #region reject file


            string invalidMessage = Tokens.WrongExcelFormat;

            if(file == null){
                l_message.Text= string.Format("{2}: {0} {3} {1}", ".xls", ".xsls", invalidMessage, Tokens.Colon);
                return;
            }
            #endregion

            #region Validate sheet
            var provider = ExcelProvider.Create(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName)), "Sheet1");

            List<Invoice> invoices;
            try{
                invoices=(from excelRow in provider.Rows
                          let what = excelRow.GetByName<string>("Name")
                          select new Invoice{
                              Phone=excelRow.GetByName<string>("CustomerPhone"),
                              Amount=Convert.ToDouble(excelRow.GetByName<string>("Amount")),
                              StartAt = Convert.ToDateTime(excelRow.GetByName<string>("StartAt")),
                              EndAt = Convert.ToDateTime(excelRow.GetByName<string>("EndAt"))
                          }).ToList();
            }
            catch(Exception){
                l_message.Text = Tokens.WrongColoumnsCount;
                if (File.Exists(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName))))
                {
                    File.Delete(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName)));
                }
                return;
            }

            if(invoices.Count < 1){
                l_message.Text=Tokens.NoInvoices;
                if (File.Exists(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName))))
                {
                    File.Delete(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName)));
                }
                return;
            }
            #endregion

            #region Map invoices
            invoices=(from excelRow in provider.Rows
                      let what = excelRow.GetByName<string>("Name")
                      select new Invoice{
                          Phone=excelRow.GetByName<string>("CustomerPhone"),
                          Amount=Convert.ToDouble(excelRow.GetByName<string>("Amount")),
                          StartAt = Convert.ToDateTime(excelRow.GetByName<string>("StartAt")),
                          EndAt = Convert.ToDateTime(excelRow.GetByName<string>("EndAt"))
                      }).ToList();
            #endregion

            #region Saving manual invoices
            var errors=_invoiceService.CreateManaulInvoices(invoices, user,Convert.ToInt32(ddlSaves.SelectedItem.Value));
            var errorModel=errors.Select(x => new{
                error=x,
            });
            gv_errors.DataSource=errorModel;
            gv_errors.DataBind();
            if (File.Exists(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName))))
            {
                File.Delete(Server.MapPath(string.Format("~/Sheets/{0}", file.FileName)));
            }
            #endregion
        }
    }
    void PopulateSaves()
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var userId = Convert.ToInt32(Session["User_ID"]);
            ddlSaves.DataSource = _userSave.SavesOfUser(userId, context).Select(a => new
            {
                a.Save.SaveName,
                a.Save.Id
            });
            ddlSaves.DataBind();
            Helper.AddDefaultItem(ddlSaves);
        }
    }

    protected void gv_errors_DataBound(object sender, EventArgs e){
        Helper.GridViewNumbering(gv_errors, "gv_l_Number");
    }
}
}