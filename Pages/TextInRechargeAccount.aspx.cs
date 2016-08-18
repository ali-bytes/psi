using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using Resources;

namespace NewIspNL.Pages
{
    public partial class TextInRechargeAccount : CustomPage
    {
    
    //readonly ISPDataContext _context;


        public TextInRechargeAccount()
        {
        //_context = IspDataContext;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if(IsPostBack)return;
        PopulateText();
    }


    protected void btnUpdateText_Click(object sender, EventArgs e){
        using(var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var update = _context.TextOfRechargeAccounts.FirstOrDefault();
            if(update != null) update.Text = Editor1.Value;
         
            _context.SubmitChanges();
            lblMsg.Text = Tokens.Done;
            PopulateText();
        }
    }


    void PopulateText(){
        using(var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var text = _context.TextOfRechargeAccounts.FirstOrDefault();
            if(text != null) Editor1.Value = text.Text;
        }
    }
}
}