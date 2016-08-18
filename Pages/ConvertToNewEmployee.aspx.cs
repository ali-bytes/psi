using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class ConvertToNewEmployee : CustomPage
    {
       
            readonly IEmployeeRepository _employeeRepository = new LEmployeeRepository();

            readonly IspEntries _lIspEntries = new IspEntries();
            readonly IPhonesServices _phonesServices = new PhonesServices();
            readonly IPhonesRepository _phonesRepository = new LPhonesRepository();
            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                PopulateEmployees();
            }
            void PopulateEmployees()
            {
                var employees = GetEmployee();
                ddl_eployees.DataSource = employees;
                ddl_eployees.DataTextField = "UserName";
                ddl_eployees.DataValueField = "ID";
                ddl_eployees.DataBind();
                Helper.AddDefaultItem(ddl_eployees);
            }
            void PopulateNewEmployees(int oldEmplyeeId)
            {
                var employees = GetEmployee();
                ddlNewEmplyee.DataSource = employees.Where(a => a.ID != oldEmplyeeId);
                ddlNewEmplyee.DataTextField = "UserName";
                ddlNewEmplyee.DataValueField = "ID";
                ddlNewEmplyee.DataBind();
                Helper.AddDefaultItem(ddlNewEmplyee);
            }
            List<User> GetEmployee()
            {
                var userId = Convert.ToInt32(Session["User_ID"]);
                var user = _lIspEntries.GetUser(userId);
                if (user == null)
                {
                    return null;
                }
                var employees = user.GroupID == 1 ? _employeeRepository.Employees.Where(a => a.GroupID != 6).ToList() : _employeeRepository.Employees.Where(x => x.ID == userId).ToList();
                return employees;
            }
            protected void gv_items_DataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(gv_items, "l_Number");
            }

            protected void Search(object sender, EventArgs e)
            {
                var emplyeeId = Convert.ToInt32(ddl_eployees.SelectedItem.Value);
                PopulatePhones(emplyeeId);
                PopulateNewEmployees(emplyeeId);
                divSuccess.Visible = false;
            }
            void PopulatePhones(int employeeId)
            {
                var phones = _phonesRepository.Phones.Where(p => p.EmployeeId == employeeId && p.CallStateId == 1).ToList();
                var phonesData = phones.Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Phone1,
                    p.Governate,
                    p.CallState.State,
                    p.CallStateId,
                    p.Offer1,
                    p.Offer2,
                    Employee = p.User.UserName,
                    p.Comment,
                    p.Central,
                    p.Mobile
                }).OrderBy(p => p.Governate).ThenBy(p => p.Name).ToList();
                gv_items.DataSource = phonesData;
                gv_items.DataBind();
            }

            protected void Transfare(object sender, EventArgs e)
            {
                var newEmployeeId = Convert.ToInt32(ddlNewEmplyee.SelectedItem.Value);
                foreach (GridViewRow row in gv_items.Rows)
                {
                    var dataKey = gv_items.DataKeys[row.RowIndex];
                    if (dataKey == null) continue;
                    var id = Convert.ToInt32(dataKey.Value);
                    _phonesServices.UpdateNewEmployee(id, newEmployeeId);
                }
                divSuccess.Visible = true;
                gv_items.DataSource = null;
                gv_items.DataBind();
            }
        }
    }
