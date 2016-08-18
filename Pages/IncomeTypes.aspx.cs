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
    public partial class IncomeTypes : CustomPage
    {
        
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

                    var item = new RevenueType
                    {
                        Name = txt_Name.Text.Trim()
                    };
                    dataContext.RevenueTypes.InsertOnSubmit(item);
                    dataContext.SubmitChanges();
                    lbl_InsertResult.Text = Tokens.ItemAdded;
                    lbl_InsertResult.ForeColor = Color.Green;
                    Bind_grd();
                }
            }


            void Bind_grd()
            {
                using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var query = dataContext.RevenueTypes.ToList();
                    grd.DataSource = query;
                    grd.DataBind();
                }
            }


            protected void grd_RowDeleting(object sender, GridViewDeleteEventArgs e)
            {
                using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var dataKey = grd.DataKeys[e.RowIndex];
                    if (dataKey != null)
                    {
                        var id = Convert.ToInt32(dataKey["ID"]);
                        var query = dataContext.RevenueTypes.Where(a => a.Id == id);
                        var entity = query.First();
                        var exist = dataContext.IncomingExpenses.Where(a => a.RevenueTypeId == entity.Id).ToList();
                        if (exist.Count == 0)
                        {
                            dataContext.RevenueTypes.DeleteOnSubmit(entity);
                        }
                        else
                        {
                            lbl_InsertResult.Text = Tokens.CantDelete;
                            lbl_InsertResult.ForeColor = Color.Red;
                        }
                    }
                    dataContext.SubmitChanges();
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
                using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var dataKey = grd.DataKeys[e.RowIndex];
                    if (dataKey != null)
                    {
                        int id = Convert.ToInt32(dataKey["ID"]);
                        var query = dataContext.RevenueTypes.Where(a => a.Id == id);
                        var entity = query.First();
                        entity.Name = ((TextBox)(grd.Rows[e.RowIndex].FindControl("TextBox1"))).Text.Trim();
                    }
                    dataContext.SubmitChanges();
                    grd.EditIndex = -1;
                    Bind_grd();
                }
            }
        }
    }
 