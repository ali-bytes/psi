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
    public partial class Requests : CustomPage
    {
       protected void Page_Load(object sender, EventArgs e){
        if(!IsPostBack){
            Bind_grd();
        }
    }


    protected void btn_Insert_Click(object sender, EventArgs e){
        using(var DataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var Item = new Request{
                RequestName = txt_Name.Text.Trim()
            };
            DataContext.Requests.InsertOnSubmit(Item);
            DataContext.SubmitChanges();
            lbl_InsertResult.Text = Tokens.ItemAdded;
            lbl_InsertResult.ForeColor = Color.Green;
            Bind_grd();
        }
    }


    void Bind_grd(){
        using(var DataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var Query = from Item in DataContext.Requests
                select Item;
            grd.DataSource = Query;
            grd.DataBind();
        }
    }


    protected void grd_RowDeleting(object sender, GridViewDeleteEventArgs e){
        using(var DataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            int ID = Convert.ToInt32(grd.DataKeys[e.RowIndex]["ID"]);
            var Query = from Item in DataContext.Requests
                where Item.ID == ID
                select Item;
            Request Entity = Query.First();
            DataContext.Requests.DeleteOnSubmit(Entity);
            DataContext.SubmitChanges();
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
            var Query = from Item in DataContext.Requests
                where Item.ID == ID
                select Item;
            Request Entity = Query.First();
            Entity.RequestName = ((TextBox) (grd.Rows[e.RowIndex].FindControl("TextBox1"))).Text.Trim();
            DataContext.SubmitChanges();
            grd.EditIndex = -1;
            Bind_grd();
        }
    }
}
}