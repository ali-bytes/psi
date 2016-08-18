using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    public partial class HandlePhones : CustomPage
    {
       
            readonly IPhoneDataRepository _dataRepository = new LPhoneDataRepository();

            readonly IEmployeeRepository _employeeRepository = new LEmployeeRepository();

            readonly IspEntries _lIspEntries = new IspEntries();

            readonly IPhonesRepository _phonesRepository = new LPhonesRepository();


            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                PopulateEmployees();
                l_message.Text = string.Empty;
                PopulateServiceProvider();
                PopulateIpPacks();
                PopulateCities();
                PopulateServicePacks();
                PopulatePaymentTypes();
                PopulateReasons();
                PopulateRejectReasons();
                Fillcentral();
            }

        public void Fillcentral()
        {
            using (var db = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var cen = db.Phones.Select(z => z.Central).Distinct().ToList();

                    ddl_centrals.DataSource = cen;

           
                ddl_centrals.DataBind();
                Helper.AddDefaultItem(ddl_centrals);
            }
        }

            protected void b_search_Click(object sender, EventArgs e)
            {
                PopulatePhones();
            }


            protected void gv_items_DataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(gv_items, "l_Number");
            }


            protected void Button1_Click(object sender, EventArgs e)
            {
                Phone phone = RetrievePhone();
                if (phone == null) return;
                var data = new PhoneData
                {
                    Address = tb_address.Text,
                    Email = tb_email.Text,
                    Notes = tb_notes.Text,
                    PhoneId = phone.Id,
                    CityId = Convert.ToInt32(ddl_city.SelectedItem.Value),
                    ProviderId = Convert.ToInt32(ddl_provider.SelectedItem.Value),
                    PaymentTypeId = Convert.ToInt32(ddl_paymentType.SelectedItem.Value),
                    ServicePackId = Convert.ToInt32(ddk_servicePack.SelectedItem.Value),
                    IpPack = Convert.ToInt32(ddl_ipPack.SelectedItem.Value),
                    NationalId = txtNationalId.Text.Trim(),
                    LineOwner = txtLineOwner.Text.Trim()
                };
                phone.CallStateId = 2;

                _dataRepository.Save(data);
                _phonesRepository.Save(phone);
                PopulatePhones();
                Reset();
            }



            protected void b_pendSave_Click(object sender, EventArgs e)
            {
                var appoinment = Convert.ToDateTime(tb_nextAppointment.Text);
                var phone = RetrievePhone();
                phone.CallStateId = 3;
                phone.Appointment = appoinment;
                phone.Comment = txtComment.Text;
                _phonesRepository.Save(phone);
                PopulatePhones();
                Reset();
            }


            protected void b_saveReject_Click(object sender, EventArgs e)
            {
                var phone = RetrievePhone();
                phone.CallStateId = 4;
                phone.Comment = txtRejectComment.Text;
                _phonesRepository.Save(phone);
                PopulatePhones();
                Reset();
            }


            #region Helpers


            void Reset()
            {
                foreach (Control control in p_behaviors.Controls)
                {
                    var textBox = control as TextBox;
                    if (textBox != null)
                    {
                        textBox.Text = string.Empty;
                    }
                    var drop = control as DropDownList;
                    if (drop != null)
                    {
                        drop.SelectedIndex = 0;
                    }
                }
            }


            Phone RetrievePhone()
            {
                var id = Convert.ToInt32(hf_rejectionId.Value);
                var phone = _phonesRepository.Phones.FirstOrDefault(p => p.Id == id);
                return phone;
            }


            void PopulatePhones()
            {
                var employeeId = Convert.ToInt32(ddl_eployees.SelectedItem.Value);
                var centralname = ddl_centrals.SelectedItem.ToString();
                PopulatePhones(employeeId, centralname);
            }


            void PopulateServiceProvider()
            {
                var providers = _lIspEntries.ServiceProviders();
                ddl_provider.DataSource = providers;
                ddl_provider.DataTextField = "SPName";
                ddl_provider.DataValueField = "ID";
                ddl_provider.DataBind();
                Helper.AddDefaultItem(ddl_provider);
            }


            void PopulateIpPacks()
            {
                var packs = _lIspEntries.IpPackages();
                ddl_ipPack.DataSource = packs;
                ddl_ipPack.DataValueField = "ID";
                ddl_ipPack.DataTextField = "IpPackageName";
                ddl_ipPack.DataBind();
                Helper.AddDefaultItem(ddl_ipPack);
            }


            void PopulateCities()
            {
                var cities = _lIspEntries.Cities();
                ddl_city.DataSource = cities;
                ddl_city.DataValueField = "ID";
                ddl_city.DataTextField = "GovernorateName";
                ddl_city.DataBind();
                Helper.AddDefaultItem(ddl_city);
            }


            void PopulateServicePacks()
            {
                var servicePack = _lIspEntries.ServicePackages();
                ddk_servicePack.DataSource = servicePack;
                ddk_servicePack.DataValueField = "ID";
                ddk_servicePack.DataTextField = "ServicePackageName";
                ddk_servicePack.DataBind();
                Helper.AddDefaultItem(ddk_servicePack);
            }


            void PopulatePaymentTypes()
            {
                var paymentTypes = _lIspEntries.PaymentTypes();
                ddl_paymentType.DataSource = paymentTypes;
                ddl_paymentType.DataValueField = "ID";
                ddl_paymentType.DataTextField = "PaymentTypeName";
                ddl_paymentType.DataBind();
                Helper.AddDefaultItem(ddl_paymentType);
            }


            void PopulateEmployees()
            {
                var userId = Convert.ToInt32(Session["User_ID"]);
                var user = _lIspEntries.GetUser(userId);
                if (user == null)
                {
                    return;
                }
                // Edited by Ashraf
                if (user.GroupID == 1)
                {
                    var employees = _employeeRepository.Employees.Where(a => a.GroupID != 6).ToList();
                    ddl_eployees.DataSource = employees;
                    ddl_eployees.DataTextField = "UserName";
                    ddl_eployees.DataValueField = "ID";
                    ddl_eployees.DataBind();
                    Helper.AddDefaultItem(ddl_eployees);
                }
                else
                {
                    var employees = _employeeRepository.Employees.Where(x => x.ID == Convert.ToInt32(Session["User_ID"])).ToList();
                    /*var currentuser = (from a in IspDataContext.Users
                        where a.ID == user.ID
                        select new{
                            a.ID,
                            a.UserName,
                        }).FirstOrDefault();*/
                    ddl_eployees.DataSource = employees;
                    ddl_eployees.DataTextField = "UserName";
                    ddl_eployees.DataValueField = "ID";
                    ddl_eployees.DataBind();
                    Helper.AddDefaultItem(ddl_eployees);
                }


                // var employees = _employeeRepository.Employees.ToList();

            }


            void PopulateReasons()
            {
                var reasons = _lIspEntries.RejectionReasons();
                ddl_resons.DataSource = reasons;
                ddl_resons.DataTextField = "Reason";
                ddl_resons.DataValueField = "Id";
                ddl_resons.DataBind();
                Helper.AddDefaultItem(ddl_resons);
            }


            void PopulateRejectReasons()
            {
                var reasons = _lIspEntries.RejectionReasons();
                ddl_reject2.DataSource = reasons;
                ddl_reject2.DataTextField = "Reason";
                ddl_reject2.DataValueField = "Id";
                ddl_reject2.DataBind();
                Helper.AddDefaultItem(ddl_reject2);
            }


            void PopulatePhones(int employeeId,string centralname)
            {

                var phones = _phonesRepository.Phones.Where(p => p.EmployeeId == employeeId && p.CallStateId == 1&&p.Central==centralname).ToList();
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
                    Employee = p.User.UserName
                }).OrderBy(p => p.Governate).ThenBy(p => p.Name).ToList();
                gv_items.DataSource = phonesData;
                gv_items.DataBind();
            }


            #endregion
        }

    }
 