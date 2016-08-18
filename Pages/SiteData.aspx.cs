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
using NewIspNL.Domain;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class SiteData : CustomPage
    {
     
    const string contentSitelogo = "~/SiteLogo/";

    readonly SiteDateRepository _siteDateRepository;


    public SiteData(){
        var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        _siteDateRepository = new SiteDateRepository(context);
    }


    protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack) return;
        PopulateSiteData();
    }


    void PopulateSiteData(){
        var data = SiteData2();
        if(data == null){
            LMsg.Text = Tokens.NoSiteDataAvailable;
            BSave.Visible = false;
            return;
        }
        BSave.Visible = true;
        TbCompanyName.Text = data.SiteName;
        TbCompanyLink.Text = data.SiteUrl;
        ImgLogo.ImageUrl = contentSitelogo + data.LogoUrl;
    }


        public global::Db.SiteData SiteData2(){
        return _siteDateRepository.SiteData();
    }



    protected void BSave_OnServerClick(object sender, EventArgs e){
        global::Db.SiteData data = SiteData2();
        if(data == null) return;
        data.SiteName = TbCompanyName.Text;
        data.SiteUrl = TbCompanyLink.Text;
        if(FLogo.HasFile){
            var mapPath = Server.MapPath(contentSitelogo + data.LogoUrl);
            File.Exists(mapPath);
            try{
                File.Delete(mapPath);
            }
            catch(Exception){}

            var extensions = new List<string> { ".JPG", ".GIF",".JPEG",".PNG" };
            string ex = Path.GetExtension(FLogo.FileName);

            if (!string.IsNullOrEmpty(ex) &&
                extensions.Any(currentExtention => currentExtention == ex.ToUpper()))
            {
                string virtualName = DateTime.Now.AddHours().ToFileTime().ToString(CultureInfo.InvariantCulture);
                var fileName = virtualName + ".jpeg";

                data.LogoUrl = fileName;
                FLogo.SaveAs(Server.MapPath(contentSitelogo + fileName));
            }
            else
            {
                return;
            }
        }
        _siteDateRepository.Save(data);
        PopulateSiteData();
        LMsg.Text = Tokens.Saved;
    }
}
}