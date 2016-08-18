using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewIspNL.Pages
{
    public partial class PriceNotFound : CustomPage
    {
     
    protected void Page_Load(object sender, EventArgs e) {
        if (IsPostBack) return;

        if (Request.QueryString["Prov"] != null && Request.QueryString["Pac"] != null) {
            li_message.Text = 
                string.Format("Package {0} for provider {1} Has no price (add price)",
                                            Request.QueryString["Pac"], Request.QueryString["Prov"]);
        }
    }
}
}