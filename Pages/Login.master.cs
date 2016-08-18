using System;
using System.Collections.Generic;
using System.Configuration;

using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.Web.UI;
using Db;
using NewIspNL.Domain;
using System.IO;

namespace NewIspNL.Pages
{
    public partial class Login : MasterPage
    {


        readonly SiteDateRepository _siteDateRepository;
        public Login()
        {
            var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
            _siteDateRepository = new SiteDateRepository(context);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var data = _siteDateRepository.SiteData();
            if (data == null)
            {
              ImgLogo.ImageUrl =
                    lblCompany.Text = string.Empty;
                return;
            }
           ImgLogo.ImageUrl = "../SiteLogo/" + data.LogoUrl;
            lblCompany.Text = data.SiteName;
        }

    }

}
