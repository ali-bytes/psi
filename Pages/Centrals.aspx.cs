using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class Centrals : CustomPage
    {
        
            readonly LCentralRepository _centralRepository = new LCentralRepository();

            readonly IGovernateRepository _governateRepository = new GovernateRepository();


            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack)
                    return;
                MultiView1.SetActiveView(v_index);
                PopulateCenterals();
                PopulateGovernates();
                l_message.Text = "";
            }


            void PopulateGovernates()
            {
                var govs = _governateRepository.Governorates;
                ddl_governates.DataSource = govs;
                ddl_governates.DataTextField = "GovernorateName";
                ddl_governates.DataValueField = "ID";
                ddl_governates.DataBind();
                Helper.AddDefaultItem(ddl_governates);
            }


            void PopulateCenterals()
            {
                var offers = _centralRepository.Centrals;
                gv_index.DataSource = offers.Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Governorate.GovernorateName
                });
                gv_index.DataBind();
            }


            protected void b_new_Click(object sender, EventArgs e)
            {
                MultiView1.SetActiveView(v_AddEdit);
            }


            protected void b_save_Click(object sender, EventArgs e)
            {
                Central central;
                if (hf_id.Value == string.Empty)
                {
                    central = new Central
                    {
                        Name = tb_offer.Text,
                        GovernateId = Convert.ToInt32(ddl_governates.SelectedItem.Value)
                    };
                    _centralRepository.Save(central);
                }
                else
                {
                    central = _centralRepository.Centrals.FirstOrDefault(o => o.Id == Convert.ToInt32(hf_id.Value));
                    if (central != null)
                    {
                        central.Name = tb_offer.Text;
                        central.GovernateId = Convert.ToInt32(ddl_governates.SelectedItem.Value);
                        _centralRepository.Save(central);
                    }
                }

                PopulateCenterals();
                if (central != null) l_message.Text = Tokens.Saved;
                hf_id.Value = string.Empty;
                MultiView1.SetActiveView(v_index);
                Clear();
            }


            void Clear()
            {
                tb_offer.Text = string.Empty;
                ddl_governates.SelectedIndex = -1;
            }


            protected void gv_index_DataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(gv_index, "l_number");
                foreach (GridViewRow row in gv_index.Rows)
                {
                    var deleteButton = row.FindControl("gvb_delete") as LinkButton;
                    var offer = _centralRepository.Centrals.FirstOrDefault(o => o.Id == Convert.ToInt32(deleteButton.CommandArgument));
                    if (deleteButton != null && offer != null && offer.WorkOrders.Any())
                    {
                        deleteButton.Visible = false;
                    }
                }
            }


            protected void gvb_edit_Click(object sender, EventArgs e)
            {
                MultiView1.SetActiveView(v_AddEdit);
                var buttonSender = sender as LinkButton;
                if (buttonSender == null)
                    return;
                var id = Convert.ToInt32(buttonSender.CommandArgument);
                var central = _centralRepository.Centrals.FirstOrDefault(o => o.Id == id);
                if (central == null) return;
                tb_offer.Text = central.Name;
                hf_id.Value = central.Id.ToString(CultureInfo.InvariantCulture);
                ddl_governates.SelectedValue = central.GovernateId.ToString(CultureInfo.InvariantCulture);
            }


            protected void gvb_delete_Click(object sender, EventArgs e)
            {
                var offer = _centralRepository.Centrals.FirstOrDefault(o => o.Id == Convert.ToInt32((sender as LinkButton).CommandArgument));
                if (offer == null)
                    return;
                var deleted = _centralRepository.Delete(offer);
                if (!deleted) { l_message.Text = Tokens.CantDelete; return; }
                PopulateCenterals();
            }
        }
    }
 