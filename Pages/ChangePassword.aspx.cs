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
using Resources;

namespace NewIspNL.Pages
{
    public partial class ChangePassword : CustomPage
    {
         
            protected void Page_Load(object sender, EventArgs e)
            {
                if (!IsPostBack)
                    Bind_ddl_Users();
            }


            void Bind_ddl_Users()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var user = context.Users.Where(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).Select(usr => usr.Group.DataLevelID).First();
                    if (user != null)
                    {
                        int DataLevel = user.Value;
                        var first = context.Users.Where(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).Select(usr => usr.BranchID).First();
                        if (first != null)
                        {
                            int UserBranchID = first.Value;
                            ddl_Users.SelectedValue = null;
                            ddl_Users.Items.Clear();

                            ddl_Users.AppendDataBoundItems = true;

                            if (DataLevel == 1) //Admin Level
                            {
                                var Query = context.Users.Select(usr => usr);
                                ddl_Users.DataSource = Query;
                                ddl_Users.DataBind();
                            }
                            if (DataLevel == 2) //Branch Level
                            {
                                var Query = context.Users.Where(usr => usr.BranchID == UserBranchID);
                                ddl_Users.DataSource = Query;
                                ddl_Users.DataBind();
                            }
                        }
                    }
                    Helper.AddDefaultItem(ddl_Users);
                }
            }


            protected void btn_Edit_Click(object sender, EventArgs e)
            {
                tr_NewPassword.Visible = true;
                ViewState.Add("ID", ddl_Users.SelectedItem.Value);
            }


            protected void btn_Change_Click(object sender, EventArgs e)
            {
                using (var DataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var Query = from usr in DataContext.Users
                                where usr.ID == Convert.ToInt32(ViewState["ID"])
                                select usr;
                    User UpdatedUser = Query.First();
                    UpdatedUser.LoginPassword = Security.EncodePassword(txt_LoginPassword.Text);
                    DataContext.SubmitChanges();
                    tr_NewPassword.Visible = false;
                    ViewState["ID"] = null;
                    ddl_Users.SelectedIndex = -1;
                    txt_LoginPassword.Text = "";
                    lbl_ProcessResult.Text = Tokens.PasswordChangedForUser + UpdatedUser.UserName + "<br/>" + Tokens.LoginNameForUser + UpdatedUser.LoginName + Tokens.EndLoginNameForUser;
                    lbl_ProcessResult.ForeColor = Color.Green;
                }
            }


            protected void btn_Change0_Click(object sender, EventArgs e)
            {
                tr_NewPassword.Visible = false;
                ViewState["ID"] = null;
                ddl_Users.SelectedValue = "-1";
                txt_LoginPassword.Text = "";
            }
        }
    }
 