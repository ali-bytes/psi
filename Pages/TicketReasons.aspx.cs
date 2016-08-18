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
    public partial class TicketReasons : CustomPage
    {
       
    protected void Page_Load(object sender, EventArgs e){
        if(!IsPostBack){
            Bind_grd();
        }
    }


    protected void btn_Insert_Click(object sender, EventArgs e){
        using(var DataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var Item = new TicketReason();
            Item.Title = txt_Name.Text.Trim();
            DataContext.TicketReasons.InsertOnSubmit(Item);
            DataContext.SubmitChanges();
            lbl_InsertResult.Text = Tokens.ItemAdded;
            lbl_InsertResult.ForeColor = Color.Green;
            Bind_grd();
        }
    }


    void Bind_grd(){
        using(var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var query = from item in dataContext.TicketReasons
                select item;
            grd.DataSource = query;
            grd.DataBind();
        }
    }


    protected void grd_RowDeleting(object sender, GridViewDeleteEventArgs e){
        using(var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var dataKey = grd.DataKeys[e.RowIndex];
            if (dataKey == null)return;
            var id = Convert.ToInt32(dataKey["ID"]);
            var query = from item in dataContext.TicketReasons
                where item.ID == id
                select item;
            var entity = query.First();
            //dataContext.Tickets.DeleteAllOnSubmit(entity.Tickets);
            if (entity.Tickets.Any())
            {
                lbl_InsertResult.Text = Tokens.CantDelete;
                lbl_InsertResult.ForeColor = Color.Red;
                return;
            }
            dataContext.TicketReasons.DeleteOnSubmit(entity);
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
            var Query = from Item in DataContext.TicketReasons
                where Item.ID == ID
                select Item;
            TicketReason Entity = Query.First();
            Entity.Title = ((TextBox) (grd.Rows[e.RowIndex].FindControl("TextBox1"))).Text.Trim();
            DataContext.SubmitChanges();
            grd.EditIndex = -1;
            Bind_grd();
        }
    }
}

}