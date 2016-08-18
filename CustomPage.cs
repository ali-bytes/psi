using System;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using Db;
using NewIspNL.Services;

namespace NewIspNL
{
    public class CustomPage : Page{
        readonly ISPDataContext _ispDataContext;
        public CustomPage(){
            _ispDataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        }
        public ISPDataContext IspDataContext{
            get { return _ispDataContext; }
        }
        protected override void InitializeCulture(){
            var cultureService = new CultureService();
            var userCulture = cultureService.GetUserCulture(Convert.ToInt32(Session["User_ID"]));
            ApplyUiCulture(userCulture ?? "ar-EG");
            Session["redirected"] = false;
        }
        void ApplyUiCulture(string uiCulture){
            UICulture = uiCulture;
            Culture = uiCulture;
            Thread.CurrentThread.CurrentCulture =
                CultureInfo.CreateSpecificCulture(uiCulture);
            Thread.CurrentThread.CurrentUICulture = new
                CultureInfo(uiCulture);
            base.InitializeCulture();
        }
        protected override void OnInit(EventArgs e){
        }
        public override void VerifyRenderingInServerForm(Control control) {}
    }
}
