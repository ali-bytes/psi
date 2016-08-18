using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class ExtraGigas : CustomPage
    {
       
            readonly IspEntries _ispEntries;


            public  ExtraGigas()
            {
                _ispEntries = new IspEntries();
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                MultiView1.SetActiveView(v_index);
                PopulateReasons();
                l_message.Text = string.Empty;
            }


            void PopulateReasons()
            {
                var reasons = _ispEntries.ExtraGigasToPreview();
                GvItems.DataSource = reasons;
                GvItems.DataBind();
            }


            protected void BtnNew_Click(object sender, EventArgs e)
            {
                MultiView1.SetActiveView(v_AddEdit);
            }


            protected void b_save_Click(object sender, EventArgs e)
            {
                ExtraGiga reason;
                string name = TbName.Text;
                decimal price = Convert.ToDecimal(TbPrice.Text);
                if (hf_id.Value == string.Empty)
                {
                    reason = new ExtraGiga
                    {
                        Name = name,
                        Price = price
                    };
                    _ispEntries.AddExtraGiga(reason);
                }
                else
                {
                    reason = _ispEntries.ExtraGigaById(Convert.ToInt32(hf_id.Value));
                    if (reason != null)
                    {
                        reason.Name = name;
                        reason.Price = price;
                    }
                }
                _ispEntries.Commit();
                Clear();
                PopulateReasons();
                if (reason != null) l_message.Text = Tokens.Saved;
                hf_id.Value = string.Empty;
                MultiView1.SetActiveView(v_index);
            }


            void Clear()
            {
                Helper.Reset(p_add);
            }


            protected void gv_index_DataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(GvItems, "l_number");
            }


            protected void gvb_edit_Click(object sender, EventArgs e)
            {
                MultiView1.SetActiveView(v_AddEdit);
                var btn = Helper.GetLinkButton(sender);
                if (btn == null) return;
                var id = Convert.ToInt32(btn.CommandArgument);
                var extraGiga = _ispEntries.ExtraGigaById(id);
                if (extraGiga == null) return;
                TbName.Text = extraGiga.Name;
                TbPrice.Text = Helper.FixNumberFormat(extraGiga.Price);
                hf_id.Value = extraGiga.Id.ToString(CultureInfo.InvariantCulture);
            }


            protected void gvb_delete_Click(object sender, EventArgs e)
            {
                var btn = Helper.GetLinkButton(sender);
                if (btn == null)
                {
                    return;
                }
                var extraGiga = _ispEntries.ExtraGigaById(Convert.ToInt32(btn.CommandArgument));
                if (extraGiga == null)
                    return;
                var orders = _ispEntries.GetWorkOrderByExtraGiga(extraGiga.Id);
                if (orders.Any())
                {
                    l_message.Text = Tokens.CantDelete;
                    return;
                }
                _ispEntries.DeleteExtraGiga(extraGiga);
                _ispEntries.Commit();
                l_message.Text = Tokens.Deleted;
                PopulateReasons();
            }


            protected void CancelProcess(object sender, EventArgs e)
            {
                Helper.Reset(v_AddEdit);
                MultiView1.SetActiveView(v_index);
            }
        }
    }
 