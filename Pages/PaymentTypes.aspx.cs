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
    public partial class PaymentTypes : CustomPage
    {
       
    protected void Page_Load(object sender, EventArgs e){
        if(!IsPostBack){
            Bind_grd();
        }
    }


    protected void btn_Insert_Click(object sender, EventArgs e){
        using(var DataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var Item = new PaymentType();
            Item.PaymentTypeName = txt_PaymentTypeName.Text.Trim();
            Item.Amount = Convert.ToDouble(txt_Amount.Text.Trim());
            DataContext.PaymentTypes.InsertOnSubmit(Item);
            DataContext.SubmitChanges();
            lbl_InsertResult.Text = Tokens.ItemAdded;
            lbl_InsertResult.ForeColor = Color.Green;
            Bind_grd();
        }
    }


    void Bind_grd(){
        using(var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var query = from item in dataContext.PaymentTypes
                select item;
            grd.DataSource = query;
            grd.DataBind();
        }
    }


    protected void grd_RowDeleting(object sender, GridViewDeleteEventArgs e){
        using(var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var dataKey = grd.DataKeys[e.RowIndex];
            if (dataKey != null)
            {
                var id = Convert.ToInt32(dataKey["ID"]);
                var query = from item in dataContext.PaymentTypes
                    where item.ID == id
                    select item;
                var entity = query.First();
                var orders = dataContext.WorkOrders.Where(a => a.PaymentTypeID == id);
                if (orders.Any())
                {
                    lbl_InsertResult.Text = Tokens.CantDelete;
                    return;
                }
                dataContext.PaymentTypes.DeleteOnSubmit(entity);
            }
            dataContext.SubmitChanges();
            Bind_grd();
        }
    }


    protected void grd_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e){
        grd.EditIndex = -1;
        Bind_grd();
    }


    protected void grd_RowEditing(object sender, GridViewEditEventArgs e){
        grd.EditIndex = e.NewEditIndex;
        Bind_grd();
    }


    protected void grd_RowUpdating(object sender, GridViewUpdateEventArgs e){
        using(var DataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){           
            int ID = Convert.ToInt32(grd.DataKeys[e.RowIndex]["ID"]);
            var Query = from Item in DataContext.PaymentTypes
                where Item.ID == ID
                select Item;
            PaymentType Entity = Query.First();

            Entity.PaymentTypeName = ((TextBox) (grd.Rows[e.RowIndex].FindControl("TextBox1"))).Text.Trim();
            Entity.Amount = Convert.ToDouble(((TextBox) (grd.Rows[e.RowIndex].FindControl("TextBox2"))).Text.Trim());
            DataContext.SubmitChanges();
            grd.EditIndex = -1;
            Bind_grd();
        }
    }
}
}