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
    public partial class ServiceProviders : CustomPage
    {
    
    protected void Page_Load(object sender, EventArgs e){
        if(!IsPostBack){
            using(var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
                Bind_grd(_context);
            }
        }
    }


    protected void btn_Insert_Click(object sender, EventArgs e){
        using(var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var Item = new ServiceProvider{
                SPName = txt_Name.Text.Trim()
            };
            _context.ServiceProviders.InsertOnSubmit(Item);
            _context.SubmitChanges();
            lbl_InsertResult.Text = Tokens.ItemAdded;
            lbl_InsertResult.ForeColor = Color.Green;
            Bind_grd(_context);
        }
    }


    void Bind_grd(ISPDataContext _context){
        
            var providers = _context.ServiceProviders.Select(x =>
                new{
                    x.SPName,
                    x.ID,
                    Active = x.Active == null || x.Active.Value
                });
            grd.DataSource = providers;
            grd.DataBind();
        }
    


    protected void grd_RowDeleting(object sender, GridViewDeleteEventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var dataKey = grd.DataKeys[e.RowIndex];
            if (dataKey == null)return;
            var id = Convert.ToInt32(dataKey["ID"]);
            var orders = context.WorkOrders.Where(a => a.ServiceProviderID == id);
            if (orders.Any())
            {
                lbl_InsertResult.Text = Tokens.CantDelete;
                return;
            }
            var query = context.ServiceProviders.Where(a => a.ID == id);
            var entity = query.First();
            context.Pricings.DeleteAllOnSubmit(entity.Pricings);
            context.ResellerPackagesDiscounts.DeleteAllOnSubmit(entity.ResellerPackagesDiscounts);
            context.BranchPackagesDiscounts.DeleteAllOnSubmit(entity.BranchPackagesDiscounts);
            context.OfferProviders.DeleteAllOnSubmit(entity.OfferProviders);
            context.WorkOrderHistories.DeleteAllOnSubmit(entity.WorkOrderHistories);
            context.BranchesDiscounts.DeleteAllOnSubmit(entity.BranchesDiscounts);
            context.UserProviders.DeleteAllOnSubmit(entity.UserProviders);
            context.OptionSuspendProviders.DeleteAllOnSubmit(entity.OptionSuspendProviders);
            context.OptionInvoiceProviders.DeleteAllOnSubmit(entity.OptionInvoiceProviders);
            context.OptionProviders.DeleteAllOnSubmit(entity.OptionProviders);
            context.SubmitChanges();
            context.ServiceProviders.DeleteOnSubmit(entity);
            context.SubmitChanges();
            Bind_grd(context);
        }
    }


    protected void grd_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            grd.EditIndex = -1;
            Bind_grd(context);
        }
    }


    protected void grd_RowEditing(object sender, GridViewEditEventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            grd.EditIndex = e.NewEditIndex;
            Bind_grd(context);
        }
    }


    protected void grd_RowUpdating(object sender, GridViewUpdateEventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var dataKey = grd.DataKeys[e.RowIndex];
            if (dataKey == null)return;
            var id = Convert.ToInt32(dataKey["ID"]);
            var query = context.ServiceProviders.Where(a => a.ID == id);
            var entity = query.First();
            entity.SPName = ((TextBox) (grd.Rows[e.RowIndex].FindControl("TextBox1"))).Text.Trim();
            entity.Active = ((CheckBox) (grd.Rows[e.RowIndex].FindControl("CB1"))).Checked;

            context.SubmitChanges();
            grd.EditIndex = -1;
            Bind_grd(context);
        }
    }
}
}