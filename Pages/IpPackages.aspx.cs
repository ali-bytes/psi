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
    public partial class IpPackages : CustomPage
    {
      
        //readonly ISPDataContext _ispData = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
  

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    Bind_grd(context);
                }
            }
        }


        protected void btn_Insert_Click(object sender, EventArgs e)
        {
            using (var context1 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var item = new IpPackage
                {
                    IpPackageName = txt_Name.Text.Trim()
                };
                context1.IpPackages.InsertOnSubmit(item);
                context1.SubmitChanges();
                lbl_InsertResult.Text = Tokens.ItemAdded;
                lbl_InsertResult.ForeColor = Color.Green;
                Bind_grd(context1);
            }
        }


        void Bind_grd(ISPDataContext context)
        {

            var query = from item in context.IpPackages
                        select item;
            grd.DataSource = query;
            grd.DataBind();

        }


        protected void grd_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (var context2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var dataKey = grd.DataKeys[e.RowIndex];
                if (dataKey != null)
                {
                    var id = Convert.ToInt32(dataKey["ID"]);
                    Delete(context2, id);
                }
            }
        }


        void Delete(ISPDataContext context, int id)
        {
            var package = context.IpPackages.FirstOrDefault(item => item.ID == id);
            if (package == null) return;
            if (package.WorkOrders.Any())
            {
                lbl_InsertResult.Text = Tokens.CantDelete;
                return;
            }

            context.WorkOrderRequests.DeleteAllOnSubmit(package.WorkOrderRequests);
            context.SubmitChanges();

            context.IpPackages.DeleteOnSubmit(package);
            context.SubmitChanges();
            lbl_InsertResult.Text = Tokens.Deleted;
            Bind_grd(context);
        }


        protected void grd_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            using (var context3 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                grd.EditIndex = -1;
                Bind_grd(context3);
            }
        }


        protected void grd_RowEditing(object sender, GridViewEditEventArgs e)
        {
            using (var context4 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                grd.EditIndex = e.NewEditIndex;
                Bind_grd(context4);
            }
        }


        protected void grd_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            using (var context5 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var dataKey = grd.DataKeys[e.RowIndex];
                if (dataKey != null)
                {
                    var id = Convert.ToInt32(dataKey["ID"]);
                    var query = from item in context5.IpPackages
                                where item.ID == id
                                select item;
                    var entity = query.First();
                    entity.IpPackageName = ((TextBox)(grd.Rows[e.RowIndex].FindControl("TextBox1"))).Text.Trim();
                }
                context5.SubmitChanges();
                grd.EditIndex = -1;
                Bind_grd(context5);
            }
        }



        protected void BDel_OnClick(object sender, EventArgs e)
        {
            using (var context6 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var linkButton = sender as LinkButton;
                if (linkButton == null)
                {
                    return;
                }
                var id = Convert.ToInt32(linkButton.CommandArgument);
                Delete(context6, id);
            }
        }
    }
}