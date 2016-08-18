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
    public partial class EmployeesReports : CustomPage
    {
           readonly IEmployeeRepository _employeeRepository = new LEmployeeRepository();

            readonly IEmployeeServices _employeeServices = new EmployeeServices();

            readonly IPhoneStatesRepository _statesRepository = new LPhoneStatesRepository();


            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                PopulateEmployees();
                PopulatePhoneStates();
            }


            void PopulateEmployees()
            {
                var employees = _employeeRepository.Employees.ToList();
                ddl_eployees.DataSource = employees;
                ddl_eployees.DataTextField = "UserName";
                ddl_eployees.DataValueField = "ID";
                ddl_eployees.DataBind();
                Helper.AddDefaultItem(ddl_eployees);
            }


            void PopulatePhoneStates()
            {
                var states = _statesRepository.EmployeeReportStates();
                ddl_states.DataSource = states;
                ddl_states.DataTextField = "State";
                ddl_states.DataValueField = "Id";
                ddl_states.DataBind();
                Helper.AddDefaultItem(ddl_states);
            }


            protected void b_search_Click(object sender, EventArgs e)
            {
                var stateId = Convert.ToInt32(ddl_states.SelectedItem.Value);
                var employeeId = Convert.ToInt32(ddl_eployees.SelectedItem.Value);
                var phones = _employeeServices.EmployeeCustomersInState(employeeId, stateId);
                gv_items.DataSource = phones;
                gv_items.DataBind();
            }


            protected void gv_items_DataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(gv_items, "l_Number");
            }
        }
    }
 