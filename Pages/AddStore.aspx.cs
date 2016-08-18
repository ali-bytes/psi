using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class AddStore : CustomPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Activate();
            if (IsPostBack) return;
            flag.Value = "0";
            PopulateDetails();
            MsgError.Visible = false;
            MsgSuccess.Visible = false;
        }


        private void PopulateDetails()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var list = context.Stores.ToList();
                GvStore.DataSource = list;
                GvStore.DataBind();
            }
        }


        void AddItem()
        {
            Clear();
            flag.Value = "1";
            txtStoreName.Text = txtNotes.Text = string.Empty;
        }


        void Clear()
        {
            MsgSuccess.Visible = MsgError.Visible = false;
            MsgSuccess.InnerHtml = MsgError.InnerHtml = string.Empty;
        }


        protected void DeleteEvent(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var btn = sender as LinkButton;
                if (btn == null) return;
                var id = Convert.ToInt32(btn.ValidationGroup);
                var detail = context.Stores.FirstOrDefault(a => a.Id == id);
                if (detail == null) return;
                context.Stores.DeleteOnSubmit(detail);
                context.SubmitChanges();
                MsgSuccess.Visible = true;
                MsgSuccess.InnerHtml = Tokens.Deleted;
                flag.Value = "0";
                PopulateDetails();
            }
        }


        protected void EditEvent(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                flag.Value = "1";
                MsgSuccess.Visible = MsgError.Visible = false;
                MsgSuccess.InnerHtml = MsgError.InnerHtml = string.Empty;
                var btn = sender as HtmlButton;
                if (btn == null) return;
                var id = Convert.ToInt32(btn.ValidationGroup);
                selected.Value = btn.ValidationGroup;
                var detail = context.Stores.FirstOrDefault(a => a.Id == id);
                if (detail == null) return;
                txtStoreName.Text = detail.StoreName;
                txtNotes.Text = detail.Notes;
            }
        }



        private void Save()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var chk = Convert.ToInt32(selected.Value);
                var storeName = txtStoreName.Text;
                var notes = txtNotes.Text;

                if (chk == 0)
                {
                    //new
                    if (!string.IsNullOrWhiteSpace(selected.Value) && chk != 0) return;
                    var item = new Store
                    {
                        StoreName = storeName,
                        Notes = notes,
                    };
                    context.Stores.InsertOnSubmit(item);
                    context.SubmitChanges();
                    MsgSuccess.Visible = true;
                    MsgSuccess.InnerHtml = Tokens.Saved;
                    MsgError.Visible = false;
                    MsgError.InnerHtml = string.Empty;
                    flag.Value = "0";
                    PopulateDetails();
                }
                else
                {
                    // edit
                    var id = chk;
                    var detail = context.Stores.FirstOrDefault(a => a.Id == id);
                    if (detail == null) return;
                    detail.StoreName = storeName;
                    detail.Notes = notes;
                    context.SubmitChanges();
                    MsgSuccess.Visible = true;
                    MsgSuccess.InnerHtml = Tokens.Saved;
                    MsgError.Visible = false;
                    MsgError.InnerHtml = string.Empty;
                    flag.Value = "0";
                    PopulateDetails();
                }
            }
        }

        protected void Newdata(object sender, EventArgs e)
        {
            selected.Value = "0";
        }

        void Activate()
        {
            GvStore.DataBound += (o, e) => Helper.GridViewNumbering(GvStore, "LNo");
            BAdd.ServerClick += (o, e) => AddItem();
            BSave.ServerClick += (o, e) => Save();
            bCancel.ServerClick += (o, e) =>
            {
                flag.Value = "0";
                Clear();
            };

        }

    }
}
