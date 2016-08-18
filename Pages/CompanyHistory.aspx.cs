using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NewIspNL.Domain.Abstract;

namespace NewIspNL.Pages
{
    public partial class CompanyHistory : CustomPage
    {
        
            readonly IRouterRepository _routerRepository = new RouterRepository();



            protected void BSearch_OnClick(object sender, EventArgs e)
            {
                var start = Convert.ToDateTime(TbFrom.Text);
                var end = Convert.ToDateTime(TbTo.Text);
                var reportItems = _routerRepository.CalculateMainHistory(start, end);
                GItems.DataSource = reportItems;
                GItems.DataBind();
            }
        }
    }
 