using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using NewIspNL.Service;
using Resources;

namespace NewIspNL.Pages
{
    public partial class DiscountsRewards : CustomPage
    {
        ISPDataContext pioent = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_ID"]!= null)
            {

               
                txtMessage.Visible = false;
                txtError.Visible = false;

                if (!IsPostBack)
                {
                    fill_month();
                    fill_kind();
                    fill_emps();
                  fill_grid();
                }
                
                  
               

            }
            else
            {
                Response.Redirect("~\\default.aspx");
            }
        }


        public void fill_kind()
        {
            List<string> a = new List<string>();
            a.Add("اضافي");
            a.Add("خصم");

            DropDownList2.DataSource = a;
            DropDownList2.DataBind();


        }
        public void fill_month()
        {
            List<string> a = new List<string>();
            a.Add("1");
            a.Add("2");
            a.Add("3");
            a.Add("4");
            a.Add("5");
            a.Add("6");
            a.Add("7");
            a.Add("8");
            a.Add("9");
            a.Add("10");
            a.Add("11");
            a.Add("12");

            DropDownList3.DataSource = a;
            DropDownList3.DataBind();


        }
        public void fill_emps()
        {
            var a= pioent.Employes.ToList();
        
            DropDownList1.DataSource = a;
            DropDownList1.DataValueField = "Id";
            DropDownList1.DataTextField = "Name";
            DropDownList1.DataBind();


        }

        public void fill_grid()
        {
            try
            {
  int id = Convert.ToInt32(DropDownList1.SelectedValue);
            var a = (from g in pioent.Rewards
                     where g.empid == id
                     select new
                     {
                         g.Employe.Name,
                         g.kind,
                         g.value,
                         g.note,
                         g.date
                     }).ToList();
            GridView1.DataSource = a;
            GridView1.DataBind();

            }
            catch (Exception)
            {
                
              
            }
          
        }
        public void fill_grid2()
        {
            try
            {
            int num = Convert.ToInt32(DropDownList3.SelectedValue);
            int id = Convert.ToInt32(DropDownList1.SelectedValue);
            var a = (from g in pioent.Rewards
                     where g.empid == id && g.date.Month == num
                     select new
                     {
                         g.Employe.Name,
                         g.kind,
                         g.value,
                         g.note,
                         g.date
                     }).ToList();
            GridView1.DataSource = a;
            GridView1.DataBind();
            }
        catch (Exception)
        {


        }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
               
                DateTime date = DateTime.Now;
                date = Convert.ToDateTime(date.ToString("dd-MM-yyyy"));
                Reward emp = new Reward();

                emp.empid = Convert.ToInt32(DropDownList1.SelectedValue);
                emp.kind = DropDownList2.SelectedItem.ToString();
                emp.note = TextBox2.Text;
                emp.value = Convert.ToDecimal(TextBox1.Text);
                emp.date = date.AddHours();
                pioent.Rewards.InsertOnSubmit(emp);
                pioent.SubmitChanges();
                txtMessage.Visible = true;



                fill_grid();
                TextBox1.Text = "";
                txtMessage.Text = Tokens.Request_Added_successfully;
                TextBox2.Text = "";
            }
            catch (Exception)
            {

                txtError.Visible = true;

                txtError.Text = "عفوا يوجد خطأ في البيانات";
            }

        }

       

        protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
        {
            fill_grid2();
        }
    }
}