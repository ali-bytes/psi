using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class DisCard : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_ID"] == null)
            {
                Response.Redirect("~/Pages/default.aspx");
                return;
            }

            Activate();
            Cards();

            if (!IsPostBack)
            {
                Fillsearch();
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    Populatetypes(context);

                }
            }
        }
        void Fillsearch()
        {
            List<string> add = new List<string>();
            add.Add("الكروت المستخدمة");
            add.Add("الكروت غيرالمستخدمة");
            add.Add("رقم الكارت");
            searchtype.DataSource = add;
            searchtype.DataBind();
            Helper.AddDefaultItem(searchtype);
        }
        void Populatetypes(ISPDataContext db3)
        {
            var query = db3.DisTypes.Select(gov => gov);
            ddl_Governorates.DataSource = query;
            ddl_Governorates.DataValueField = "ID";
            ddl_Governorates.DataTextField = "TypeName";
            ddl_Governorates.DataBind();
            Helper.AddDefaultItem(ddl_Governorates);
        }
        void Activate()
        {
            GvitemData.DataBound += (o, e) => Helper.GridViewNumbering(GvitemData, "LNo");

        }
        public void Cards()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var cards = context.DisCards.Select(x => new
                {
                    x.DisType.TypeName,
                    x.ID,
                    x.DisType.Price,
                    x.Status,
                    x.WorkOrder.CustomerName,
                    x.WorkOrder.CustomerPhone,
                    x.User.UserName,
                    x.op_date
                }).ToList();
                GvitemData.DataSource = cards;
                GvitemData.DataBind();
            }
        }
        protected void Add_Click(object sender, EventArgs e)
        {


            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var count = Convert.ToInt32(typevalue.Text);
                for (int i = 0; i < count; i++)
                {
                    Db.DisCard add = new Db.DisCard();
                    add.DisTypeID = Convert.ToInt32(ddl_Governorates.SelectedValue);
                    add.Status = false;
                    add.user_id = null;
                    add.op_date = null;
                    context.DisCards.InsertOnSubmit(add);

                }
                context.SubmitChanges();
                Cards();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage",
                            "alert('تم اضافة الكروت ')", true);

            }
        }
        protected void searchtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (searchtype.SelectedValue == "الكروت المستخدمة")
            {
                cardnum.Visible = false;
                Label2.Visible = false;
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var cards = context.DisCards.Where(x => x.Status == true).Select(x => new
                    {
                        x.DisType.TypeName,
                        x.ID,
                        x.DisType.Price,
                        x.Status,
                        x.WorkOrder.CustomerName,
                        x.WorkOrder.CustomerPhone,
                        x.User.UserName,
                        x.op_date
                    }).ToList();
                    GvitemData.DataSource = cards;
                    GvitemData.DataBind();
                }
            }
            else if (searchtype.SelectedValue == "الكروت غيرالمستخدمة")
            {
                cardnum.Visible = false;
                Label2.Visible = false;
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var cards = context.DisCards.Where(x => x.Status == false).Select(x => new
                    {
                        x.DisType.TypeName,
                        x.ID,
                        x.DisType.Price,
                        x.Status,
                        x.WorkOrder.CustomerName,
                        x.WorkOrder.CustomerPhone,
                        x.User.UserName,
                        x.op_date
                    }).ToList();
                    GvitemData.DataSource = cards;
                    GvitemData.DataBind();
                }
            }
            else if (searchtype.SelectedValue == "رقم الكارت")
            {
                cardnum.Visible = true;
                Label2.Visible = true;
            }
        }
        protected void cardnum_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cardnum.Text))
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var id = Convert.ToInt32(cardnum.Text);
                    var cards = context.DisCards.Where(x => x.ID == id).Select(x => new
                    {
                        x.DisType.TypeName,
                        x.ID,
                        x.DisType.Price,
                        x.Status,
                        x.WorkOrder.CustomerName,
                        x.WorkOrder.CustomerPhone,
                        x.User.UserName,
                        x.op_date
                    }).ToList();
                    GvitemData.DataSource = cards;
                    GvitemData.DataBind();
                }
            }
        }


    }
}