using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class ResellerAttachment : CustomPage
    {
         readonly IspDomian _domian;
    public ResellerAttachment(){
        _domian = new IspDomian(IspDataContext);
    }
  
    protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack) return;
        PrepareInputs();
        Search();
    }

    void PrepareInputs(){
        _domian.PopulateResellers(DdlReseller, true);
        var currentYear = DateTime.Now.Year;
        Helper.PopulateDrop(Helper.FillYears(currentYear - 5, currentYear+2).OrderBy(x => x), DdlYear);
        Helper.PopulateMonths(DdlMonth);
    }


    protected void SearchDemands(object sender, EventArgs e){
        Msg.InnerHtml = string.Empty;
        Search();
        HfSerched.Value="1";
    }


    void Search(){
        using(var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            if(!string.IsNullOrEmpty(DdlReseller.SelectedValue)){
                var attachment = _context.ResellerAttachments.Where(x => x.ResellerId == int.Parse(DdlReseller.SelectedItem.Value) && x.Date.Value.Year == int.Parse(DdlYear.SelectedItem.Value) && x.Date.Value.Month == int.Parse(DdlMonth.SelectedItem.Value)).Select(x => new{
                    x.ID,
                    x.User.UserName,
                    x.FileNameAfter,
                    x.FileNameBefor
                    //Url = string.Format("../Attachments/{0}", x.FileName)
                }).ToList();
                GvResults.DataSource = attachment;
                GvResults.DataBind();

                var firstOrDefault = attachment.FirstOrDefault();
                if(firstOrDefault != null){
                    btnupdate.Visible = true;
                    btnAdd.Visible = false;

                    lnkfileafterReview.Visible = true;
                    lnkfilebeforreview.Visible = true;
                    lnkfileafterReview.HRef = string.Format("../Attachments/{0}", firstOrDefault.FileNameAfter);
                    lnkfilebeforreview.HRef = string.Format("../Attachments/{0}", firstOrDefault.FileNameBefor);
                    ViewState.Add("FileId", firstOrDefault.ID);
                }

            }
        }
    }


    void Add(){
        using (var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var Exists = _context.ResellerAttachments.FirstOrDefault(x => x.Date.Value.Year == Convert.ToInt32(DdlYear.SelectedItem.Value) && x.Date.Value.Month == Convert.ToInt32(DdlMonth
                .SelectedItem.Value) && x.ResellerId == Convert.ToInt32(DdlReseller.SelectedValue));
            if(Exists == null){
                if(string.IsNullOrWhiteSpace(TbUploadBeforRview.FileName)){
                    Msg.InnerHtml = Tokens.AttachFile;
                    return;
                }
                string filenameBefor = DateTime.Now.Millisecond + TbUploadBeforRview.FileName;
                TbUploadBeforRview.SaveAs(Server.MapPath(string.Format("../Attachments/{0}", filenameBefor)));
                /*if(string.IsNullOrWhiteSpace(TbUploadAfterRview.FileName)){
                Msg.InnerHtml = Tokens.AttachFile;
                return;
            }*/
                string filenameAfter = string.Empty;
                if(!string.IsNullOrEmpty(TbUploadAfterRview.FileName)){
                    filenameAfter = DateTime.Now.Millisecond + TbUploadAfterRview.FileName;
                    TbUploadAfterRview.SaveAs(Server.MapPath(string.Format("../Attachments/{0}", filenameAfter)));
                }

                _context.ResellerAttachments.InsertOnSubmit(new Db.ResellerAttachment{
                    ResellerId = Helper.GetDropValue(DdlReseller),
                    FileNameBefor = filenameBefor,
                    FileNameAfter = filenameAfter,
                    //var nextMonth = new DateTime(time.Year, time.Month, Math.Min(activationDate.Value.Day, monthDays));
                    Date = new DateTime(Convert.ToInt32(DdlYear.SelectedItem.Value), Convert.ToInt32(DdlMonth.SelectedItem.Value), 1),
                });
                _context.SubmitChanges();
                Search();
            } else{
                Msg.InnerHtml = Tokens.UserNameAlreadyExist;
            }

        }

    }
    void ResetPage(){
        GvResults.DataSource = null;
        GvResults.DataBind();
        Helper.Reset(Forsearch);
        HfSerched.Value = string.Empty;
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Add();
    }


    protected void btnUpdate_Click(object sender, EventArgs e){
        using(var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var button = int.Parse(ViewState["FileId"].ToString());

            var attachment = _context.ResellerAttachments.FirstOrDefault(x => x.ID == button);
            if(attachment != null){
                update(button);
            }
            Search();
        }
    }


    void update(int fileId){
        using(var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var attachment = _context.ResellerAttachments.FirstOrDefault(x => x.ID == fileId);
            var extensions = new List<string> {".XLS", ".XLSX" };
           
            if(attachment != null){
                if(!string.IsNullOrWhiteSpace(TbUploadAfterRview.FileName)){
                    string ex1 = Path.GetExtension(TbUploadAfterRview.FileName).ToUpper();
                    if (!string.IsNullOrEmpty(ex1) && extensions.Any(currentExtention => currentExtention == ex1))
                    {
                        string filenameafter = DateTime.Now.Millisecond + TbUploadAfterRview.FileName;
                        TbUploadAfterRview.SaveAs(Server.MapPath(string.Format("../Attachments/{0}", filenameafter)));
                        attachment.ResellerId = Helper.GetDropValue(DdlReseller);
                        attachment.FileNameAfter = filenameafter;
                        //attachment.Date = DateTime.Now;
                    }
                }
                if(!string.IsNullOrWhiteSpace(TbUploadBeforRview.FileName)){
                    string ex2 = Path.GetExtension(TbUploadBeforRview.FileName).ToUpper();
                    if (!string.IsNullOrEmpty(ex2) && extensions.Any(currentExtention => currentExtention == ex2))
                    {
                        string filenamebefor = DateTime.Now.Millisecond + TbUploadBeforRview.FileName;
                        TbUploadBeforRview.SaveAs(Server.MapPath(string.Format("../Attachments/{0}", filenamebefor)));
                        attachment.FileNameBefor = filenamebefor;
                        //attachment.Date = DateTime.Now;
                    }
                } else{
                    attachment.ResellerId = Helper.GetDropValue(DdlReseller);
                }

                _context.SubmitChanges();
                Msg.InnerText = Tokens.Done;
            }
        }
    }
    }
}