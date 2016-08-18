using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using NewIspNL.Services;

namespace NewIspNL.Pages
{
    public partial class DiscoundType : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

                if (Session["User_ID"] == null)
                {
                    Response.Redirect("default.aspx");
                    return;
                }
                Activate();
                types();

         
        }
        private void Activate()
        {
            GvitemData.DataBound += (o, e) => Helper.GridViewNumbering(GvitemData, "LNo");
        }
        public void types()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var types = context.DisTypes.Select(x => x).ToList();
                GvitemData.DataSource = types;
                GvitemData.DataBind();
            }
        }
        protected void Add_Click(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                DisType add = new DisType();
                add.TypeName = typename.Text;
                add.Price = Convert.ToDecimal(typevalue.Text);
                context.DisTypes.InsertOnSubmit(add);
                context.SubmitChanges();
                types();
                Label1.Visible = true;
            }
            Clear();
        }

        private void Clear()
        {
            typename.Text = "";
            typevalue.Text = "";
        }
        protected void Delete(object sender, EventArgs e)
        {
            try
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {

                    var btn = sender as LinkButton;
                    var id = Convert.ToInt32(btn.CommandArgument);


                    var check = context.DisCards.Where(x => x.DisTypeID == id).Select(x => x).ToList();
                    if (check.Count == 0)
                    {
                        var rec = context.DisTypes.Where(x => x.ID == id).Select(x => x).FirstOrDefault();
                        context.DisTypes.DeleteOnSubmit(rec);
                        context.SubmitChanges();
                        types();
                    }

                    else
                    {

                        var users = context.Users.Where(usr => usr.ID == Convert.ToInt32(Session["User_ID"]));
                        var currentuser = users.FirstOrDefault();
                        var cultureService = new CultureService();
                        var culture = cultureService.GetUserCultureName(currentuser.ID);
                        if (culture == "1")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage",
                                "alert('عفوا يوجد كروت خصم مسجلة لهذة الفئة')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage",
                         "alert('Sorry There are Discount Cards Added  To This Category')", true);
                        }
                    }

                }
            }
            catch (Exception)
            {

            }


        }
        protected void Ed(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var btn = sender as LinkButton;


                var id = Convert.ToInt32(btn.CommandArgument);
                HiddenField1.Value = id.ToString();
                var rec = context.DisTypes.Where(x => x.ID == id).Select(x => x).FirstOrDefault();

                typename.Text = rec.TypeName;
                if (rec.Price>0)
                {
                    typevalue.Text = Math.Round((decimal)rec.Price, 2).ToString();
                }
                
                Add.Visible = false;
                Button1.Visible = true;
                cancel.Visible = true;

            }

        }
        protected void cancelbtn(object sender, EventArgs e)
        {
           
                typename.Text = "";
                typevalue.Text = "";
                Add.Visible = true;
                Button1.Visible = false;
                cancel.Visible = false;

      

        }
        protected void Button1_Click(object sender, EventArgs e)
        {

            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {

                var id = Convert.ToInt32(HiddenField1.Value);

                var rec = context.DisTypes.Where(x => x.ID == id).Select(x => x).FirstOrDefault();

                rec.TypeName = typename.Text;
                rec.Price = Convert.ToDecimal(typevalue.Text);
                context.SubmitChanges();
                types();
                Add.Visible = true;
                Button1.Visible = false;
                Label1.Visible = true;
                cancel.Visible = false;
                typename.Text = typevalue.Text = String.Empty;
            }
        }

    }
}