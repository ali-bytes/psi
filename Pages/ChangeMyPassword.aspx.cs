using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using NewIspNL.Services;
using Resources;

namespace NewIspNL.Pages
{
    public partial class ChangeMyPassword : CustomPage
    {
        
            private readonly CultureService _culture;
            public  ChangeMyPassword()
            {
                _culture = new CultureService();
            }
            protected void Page_Load(object sender, EventArgs e)
            {
                using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {

                    if (Session["User_ID"] == null)
                        return;
                    if (!IsPostBack)
                    {
                        PopulateCultures();
                        var query = dataContext.Users.FirstOrDefault(a => a.ID == Convert.ToInt32(Session["User_ID"]));
                        if (query != null)
                        {
                            lbl_LoginName.Text = query.LoginName;
                            lbl_UserName.Text = query.UserName;
                            ddlCulture.SelectedValue = _culture.GetUserCultureName(query.ID);
                        }

                    }
                }
            }

            void PopulateCultures()
            {
                var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
                var cultures = context.Cultures;
                ddlCulture.DataSource = cultures;
                ddlCulture.DataTextField = "Name";
                ddlCulture.DataValueField = "Id";
                ddlCulture.DataBind();
                Helper.AddDefaultItem(ddlCulture);
            }



            protected void btn_UpdatePassword_Click(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    //var DataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
                    var query = context.Users.FirstOrDefault(a => a.ID == Convert.ToInt32(Session["User_ID"]));

                    if (query != null)
                    {
                        query.LoginPassword = Security.EncodePassword(txt_LoginPassword.Text);
                        if (ddlCulture.SelectedIndex != 0 && ddlCulture.SelectedValue != "-1")
                            _culture.UpdateUserCulture(Convert.ToInt32(ddlCulture.SelectedItem.Value), query.ID);
                        context.SubmitChanges();
                        lblProcessResult.Text = Tokens.Saved;//.PasswordChanged;
                        lblProcessResult.ForeColor = Color.Green;
                    }

                }
            }
        }
    }
 