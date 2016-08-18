using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class PhonesDataentry : CustomPage
    {
      
    readonly IEmployeeRepository _employeeRepository = new LEmployeeRepository();

    readonly IPhonesRepository _phonesRepository = new LPhonesRepository();

    string _fileName = "";


    protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack) return;
        PopulateEmployees();
        l_message.Text = string.Empty;
    }


    void PopulateEmployees(){
        var employees = _employeeRepository.Employees.ToList();
        ddl_eployees.DataSource = employees;
        ddl_eployees.DataTextField = "UserName";
        ddl_eployees.DataValueField = "ID";
        ddl_eployees.DataBind();
        Helper.AddDefaultItem(ddl_eployees);
    }


    protected void b_save_Click(object sender, EventArgs e){
        SavePhones();
    }


    void SavePhones(){
        // Save File
        var extention = Path.GetExtension(fu_sheet.FileName);
        var extensions = new List<string>{
                                             ".xls",
                                             ".xlsx"
                                         };
        // saving file
        if(extensions.Any(currentExtention => currentExtention == extention)){
            fu_sheet.SaveAs(Server.MapPath("~/Sheets/" + fu_sheet.PostedFile.FileName));
            _fileName = fu_sheet.PostedFile.FileName;
            var connectection = new OleDbConnection();
            switch(extention){
                case ".xlsx" :
                    connectection.ConnectionString =
                        string.Format(
                                      @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("~\\Sheets") +
                                          "\\{0};Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1'", _fileName);
                    break;
                case ".xls" :
                    connectection.ConnectionString =
                        string.Format(
                                      @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Server.MapPath("~\\Sheets") +
                                          "\\{0};Extended Properties='Excel 8.0;HDR=YES;IMEX=1'", _fileName);
                    break;
            }
            var command = new OleDbCommand("SELECT * FROM  [Sheet1$]", connectection);

            try{
                var phones = new List<Phone>();
                connectection.Open();
                var reader = command.ExecuteReader();
                while(reader != null && reader.Read()){
                    if (string.IsNullOrEmpty(reader["Phone"].ToString()) ||
                        string.IsNullOrEmpty(reader["Central"].ToString()) ||
                        string.IsNullOrEmpty(reader["GovernateCode"].ToString()))
                    {
                        l_message.Text = @"البيانات غير كاملة";
                        return;
                    }
                    var phone = new Phone{
                                             Phone1 = reader["Phone"].ToString(),
                                             Governate = reader["Governate"].ToString(),
                                             Name = reader["Name"].ToString(),
                                             CallStateId = 1,
                                             EmployeeId = Convert.ToInt32(ddl_eployees.SelectedItem.Value),
                                             Offer1 = reader["Offer1"].ToString(),
                                             Offer2 = reader["Offer2"].ToString(),
                                             Central = reader["Central"].ToString(),
                                             GovernerateCode = reader["GovernateCode"].ToString(),
                                             OldCreationDate = reader["OldCreationDate"].ToString(),
                                             OldPackage = reader["OldPackage"].ToString(),
                                             CancelDate = reader["CancelDate"].ToString(),
                                             Mobile = reader["Mobile"].ToString(),
                                             Notes = reader["Notes"].ToString()
                                         };
                    phones.Add(phone);
                }
                _phonesRepository.SaveMany(phones);
                l_message.Text = string.Format(Tokens.SavedPhonesCountMsg, phones.Count);
                ddl_eployees.SelectedIndex = 0;
                gv_items.DataSource = phones.Select(p => new{
                                                                p.Name,
                                                                p.Phone1,
                                                                p.Governate,
                                                                p.CallState.State,
                                                                Employee = p.User.UserName,
                                                                p.Offer1,
                                                                p.Offer2,
                                                                p.Central,
                                                                p.Mobile,
                                                                p.Notes

                                                            });
                gv_items.DataBind();
                if (File.Exists(Server.MapPath("~/Sheets/" + fu_sheet.PostedFile.FileName)))
                {
                    File.Delete(Server.MapPath("~/Sheets/" + fu_sheet.PostedFile.FileName));
                }
            }
            catch(Exception exception){

                l_message.Text = exception.Message;
                if (File.Exists(Server.MapPath("~/Sheets/" + fu_sheet.PostedFile.FileName)))
                {
                    File.Delete(Server.MapPath("~/Sheets/" + fu_sheet.PostedFile.FileName));
                }
            }
        } else{
            l_message.Text = Tokens.PostedFileIsInvalid;
        }
    }


    protected void gv_items_DataBound(object sender, EventArgs e){
        Helper.GridViewNumbering(gv_items, "l_Number");
    }
}
}