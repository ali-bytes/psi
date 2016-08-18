using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewIspNL.Pages
{
    public partial class Makeerror : CustomPage
    {

    protected void Page_Load(object sender, EventArgs e)
    {
        const string errorMsg = "Error made by system to test error handeling system.";
        throw new Exception(errorMsg);
    }
}
}