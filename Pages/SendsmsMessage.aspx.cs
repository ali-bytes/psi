using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain.Concrete;

namespace NewIspNL.Pages
{
    public partial class SendsmsMessage : CustomPage
    {

    //readonly SiteDateRepository _siteDateRepository;

    readonly SMSData _smsData;

    public SendsmsMessage()
    {
            var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
            //_siteDateRepository = new SiteDateRepository(context);
            _smsData = new SMSData(context);

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if(IsPostBack)return;
        txtMeaageText.MaxLength = UICulture == "en-GB" ? 160 : 70;
    }

    protected void btnSendSms_Click(object sender, EventArgs e){
        var data = _smsData.GetActiveCnfg();
        string mobile = txtSendTo.Text;
        var message = SendSms.Send(data.UserName, data.Password, mobile, txtMeaageText.Text, data.Sender,data.UrlAPI);
        string myscript = "window.open('" + message + "');";
        ClientScript.RegisterClientScriptBlock(typeof(Page), "myscript", myscript, true);
        Msg.Visible = true;
        Clear();
    }


    void Clear()
    {
        txtSendTo.Text = txtMeaageText.Text = string.Empty;
    }
}
}