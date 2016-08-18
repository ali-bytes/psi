using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using Resources;

namespace NewIspNL.Pages
{
    public partial class Ads : CustomPage
    {
         
            protected void Page_Load(object sender, EventArgs e)
            {
                if (!IsPostBack)
                {
                    Loadnews();
                }
            }


            void Loadnews()
            {
                using (var DataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var Query = from ns in DataContext.News
                                where ns.ID == 1
                                select ns;
                    txt_Title.Text = Query.First().Title;
                    txt_Details.Text = Query.First().Details;
                }
            }


            protected void Button1_Click(object sender, EventArgs e)
            {
                using (var DataContext2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var Query = from ns in DataContext2.News
                                where ns.ID == 1
                                select ns;
                    Query.First().Title = txt_Title.Text.Trim();
                    Query.First().Details = txt_Details.Text.Trim();
                    DataContext2.SubmitChanges();
                    lbl_ProcessResult.Text = Tokens.AdAdded;
                    lbl_ProcessResult.ForeColor = Color.Green;
                }
            }
        }
    }
 