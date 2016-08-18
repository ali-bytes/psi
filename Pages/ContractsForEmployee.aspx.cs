using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class ContractsForEmployee : CustomPage
    {
          readonly IEmployeeServices _employeeServices = new EmployeeServices();


            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                var contractCountByEmployee = _employeeServices.ContractsCount();
                gv_counts.DataSource = contractCountByEmployee;
                gv_counts.DataBind();
            }


            protected void gv_counts_DataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(gv_counts, "Label1");
            }
        }
    }
 