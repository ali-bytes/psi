using System;
using System.Collections.Generic;
using System.Configuration;
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
    public partial class NumberAttchments : CustomPage
    {
      

    protected void Page_Load(object sender, EventArgs e){
        Activate();
        
    }


    void Search(){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var attachment = context.NumberAttachments.Where(x => x.Number.Equals(TbPhone.Text));
            GvResults.DataSource = attachment.Select(x => new{
                x.Id,
                x.Number,
                x.AttachmentName,
                Url = string.Format("../Attachments/{0}", x.AttachmentName)
            });
            GvResults.DataBind();
        }
    }


    void Activate(){
        BSearch.ServerClick += (o, e) => Search();
        BAdd.ServerClick += (o, e) => Add();
        GvResults.DataBound += (o, e) => Helpers.Helper.GridViewNumbering(GvResults, "LNo");
    }


        private void Add()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (string.IsNullOrWhiteSpace(TbUpload.FileName))
                {
                    Msg.InnerHtml = Tokens.AttachFile;
                    return;
                }
                string ex = Path.GetExtension(TbUpload.FileName);
               
                var extensions = new List<string> { ".JPG", ".GIF", ".JPEG", ".PNG", ".PDF", ".DOC", ".DOCX", ".XLS", ".XLSX"};

                if (!string.IsNullOrEmpty(ex) && extensions.Any(currentExtention => currentExtention == ex.ToUpper()))
                {
                    string virtualName = DateTime.Now.AddHours().ToFileTime().ToString(CultureInfo.InvariantCulture);
                    TbUpload.SaveAs(Server.MapPath(string.Format("~/Attachments/{0}", virtualName + ex)));
                    context.NumberAttachments.InsertOnSubmit(new NumberAttachment()
                    {
                        Number = TbPhone.Text,
                        AttachmentName = virtualName + ex
                    });
                    context.SubmitChanges();
                    Search();
                }
            }
        }

        protected void Delete(object sender, EventArgs e)
    {
        var btn = sender as LinkButton;
        if(btn==null)return;
        var id = Convert.ToInt32(btn.CommandArgument);
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var selectedrow = context.NumberAttachments.First(a => a.Id == id);
            if(selectedrow==null)return;
            context.NumberAttachments.DeleteOnSubmit(selectedrow);
            context.SubmitChanges();
        }
        Search();
    }
}
}