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
    public partial class Governorates : CustomPage
    {
        //readonly ISPDataContext _dataContext;


            protected void Page_Load(object sender, EventArgs e)
            {
                if (!IsPostBack)
                {
                    Bind_grd();
                }
            }


            protected void btn_Insert_Click(object sender, EventArgs e)
            {
                using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var item = new Governorate
                    {
                        GovernorateName = txt_Name.Text.Trim(),
                        Code = TbCode.Text
                    };
                    dataContext.Governorates.InsertOnSubmit(item);
                    dataContext.SubmitChanges();
                    lbl_InsertResult.Text = Tokens.ItemAdded;
                    lbl_InsertResult.ForeColor = Color.Green;
                    Bind_grd();
                }
            }


            void Bind_grd()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var query = context.Governorates;
                    grd.DataSource = query;
                    grd.DataBind();
                }
            }


            protected void grd_RowDeleting(object sender, GridViewDeleteEventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var dataKey = grd.DataKeys[e.RowIndex];
                    if (dataKey != null)
                    {
                        var id = Convert.ToInt32(dataKey["ID"]);
                        var query = context.Governorates.Where(item => item.ID == id);
                        if (!query.Any()) return;
                        var orders = context.WorkOrders.Where(a => a.CustomerGovernorateID == id);
                        if (orders.Any())
                        {
                            lbl_InsertResult.Text = Tokens.CantDelete;
                            return;
                        }
                        var entity = query.First();
                        context.WorkOrderHistories.DeleteAllOnSubmit(entity.WorkOrderHistories);
                        context.Centrals.DeleteAllOnSubmit(entity.Centrals);
                        context.Suppliers.DeleteAllOnSubmit(entity.Suppliers);
                        context.Customers.DeleteAllOnSubmit(entity.Customers);
                        context.SubmitChanges();
                        context.Governorates.DeleteOnSubmit(entity);
                    }
                    context.SubmitChanges();
                    Bind_grd();
                }
            }


            protected void grd_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
            {
                grd.EditIndex = -1;
                Bind_grd();
            }


            protected void grd_RowEditing(object sender, GridViewEditEventArgs e)
            {
                grd.EditIndex = e.NewEditIndex;
                Bind_grd();
            }


            protected void grd_RowUpdating(object sender, GridViewUpdateEventArgs e)
            {
                using (var context1 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var dataKey = grd.DataKeys[e.RowIndex];
                    if (dataKey != null)
                    {
                        var id = Convert.ToInt32(dataKey["ID"]);
                        var query = context1.Governorates.Where(item => item.ID == id);
                        var entity = query.First();
                        entity.GovernorateName = ((TextBox)(grd.Rows[e.RowIndex].FindControl("TbName"))).Text.Trim();
                        entity.Code = ((TextBox)(grd.Rows[e.RowIndex].FindControl("TbCode"))).Text;
                    }
                    context1.SubmitChanges();
                    grd.EditIndex = -1;
                    Bind_grd();
                }
            }
        }
    }
 