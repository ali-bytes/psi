using System;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using NewIspNL.Services;

namespace NewIspNL
{
    public class CustomDefaultPage : Page{
        protected override void InitializeCulture(){
            //const string controlName = "ctl00$ContentPlaceHolder1$DdlCultures";
            var cultureService = new CultureService();
            var userCulture = cultureService.GetUserCulture(Convert.ToInt32(Session["User_ID"]));
            ApplyUiCulture(userCulture ?? "ar-EG");
            /*string culture;
        if(Request.Form[controlName] != null){
            var selectedLanguage = Request.Form[controlName];
            var appliedCulture = cultureService.GetCulture(Convert.ToInt32(selectedLanguage));
            if(appliedCulture != null){
                culture = appliedCulture.Culture1;
                Session["cultureChoice"] = selectedLanguage;
            } else{
                culture = "ar-EG";
                Session["cultureChoice"] = "1";
            }
        } else{
            culture = "ar-EG";
            Session["cultureChoice"] = "1";
        }

        ApplyUiCulture(culture);*/
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
            /*var style = new HtmlLink();
        style.Attributes["rel"] = "stylesheet";
        style.Attributes["type"] = "text/css";
        style.Href = "~/Content/LoginStyles/" + Thread.CurrentThread.CurrentCulture + ".css";
        Header.Controls.Add(style);
        base.OnInit(e);*/
        }
    }
}
