using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class FirendlyLinks : CustomPage
    {
       
            //readonly ISPDataContext _ispDataContext;


            public  FirendlyLinks()
            {
                //_ispDataContext = IspDataContext;
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                MultiView1.SetActiveView(v_index);
                PopulateLinks();
                l_message.Text = "";
            }



            void PopulateLinks()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var link = context.FriendlyLinks;
                    gv_index.DataSource = link.Select(x => new
                    {
                        x.Id,
                        x.PageName,
                        x.Url
                    });
                    gv_index.DataBind();
                }
            }


            protected void b_new_Click(object sender, EventArgs e)
            {
                MultiView1.SetActiveView(v_AddEdit);
                hf_id.Value = string.Empty;
            }



            protected void b_save_Click(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    FriendlyLink link;
                    if (hf_id.Value == string.Empty)
                    {
                        link = new FriendlyLink
                        {
                            PageName = tb_title.Text,
                            Url = tb_discount.Text,
                        };
                        context.FriendlyLinks.InsertOnSubmit(link);
                        context.SubmitChanges();

                    }
                    else
                    {
                        var id = Convert.ToInt32(hf_id.Value);
                        link = context.FriendlyLinks.FirstOrDefault(x => x.Id == id);
                        if (link != null)
                        {
                            link.PageName = tb_title.Text;
                            link.Url = (tb_discount.Text);
                        }
                    }
                    context.SubmitChanges();
                    PopulateLinks();
                    if (link != null) l_message.Text = string.Format("{0}", Tokens.Saved);
                    hf_id.Value = string.Empty;
                    MultiView1.SetActiveView(v_index);
                    Clear();
                }
            }


            void Clear()
            {
                foreach (var text in p_add.Controls.OfType<TextBox>())
                {
                    text.Text = string.Empty;
                }

            }


            protected void gv_index_DataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(gv_index, "l_number");
            }


            protected void gvb_edit_Click(object sender, EventArgs e)
            {
                using (var context1 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    MultiView1.SetActiveView(v_AddEdit);
                    var buttonSender = sender as LinkButton;
                    if (buttonSender == null) return;
                    var id = Convert.ToInt32(buttonSender.CommandArgument);
                    var link = context1.FriendlyLinks.FirstOrDefault(x => x.Id == id);
                    if (link == null) return;
                    tb_title.Text = link.PageName;
                    tb_discount.Text = link.Url;
                    hf_id.Value = link.Id.ToString(CultureInfo.InvariantCulture);
                }
            }


            protected void gvb_delete_Click(object sender, EventArgs e)
            {
                using (var context2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var button = sender as LinkButton;
                    if (button != null)
                    {
                        var linkId = Convert.ToInt32(button.CommandArgument);
                        var link = context2.FriendlyLinks.FirstOrDefault(x => x.Id == linkId);
                        if (link == null) return;
                        context2.FriendlyLinks.DeleteOnSubmit(link);
                        context2.SubmitChanges();
                    }
                    PopulateLinks();
                }
            }

        }
    }
 