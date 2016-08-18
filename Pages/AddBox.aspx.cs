using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using NewIspNL.Helpers;
using Resources;
using Db;

namespace NewIspNL.Pages
{
    public partial class AddBox : CustomPage
    {

        //readonly ISPDataContext _context;

        readonly IspEntries _ispEntries;

        public AddBox()
        {
            var context = IspDataContext;
            _ispEntries = new IspEntries(context);
        }
        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public bool CanAdd { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            HandlePrivildges();
            PopulateBoxes();
        }
        protected void grd_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GvBoxes.EditIndex = -1;
            Clear();
            BSave.Visible = true;
            PopulateBoxes();
        }
        protected void grd_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var dataKey = GvBoxes.DataKeys[e.RowIndex];
            if (dataKey != null)
            {
                var id = Convert.ToInt32(dataKey["ID"]);
                DeleteBox(id);
            }
        }


        protected void grd_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GvBoxes.EditIndex = e.NewEditIndex;
            var dataKey = GvBoxes.DataKeys[e.NewEditIndex];
            if (dataKey != null)
            {
                var id = dataKey.Value;
                using (var context2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var firstOrDefault = context2.Boxes.FirstOrDefault(x => x.ID == int.Parse(id.ToString()));
                    if (firstOrDefault != null)
                    {
                        TbBoxName.Text = firstOrDefault.BoxName;
                        showcheck.Checked = Convert.ToBoolean(firstOrDefault.ShowBox);
                        showinResellerppr.Checked = Convert.ToBoolean(firstOrDefault.ShowBoxInResellerPPR);
                        showinCustomerDemands.Checked = Convert.ToBoolean(firstOrDefault.ShowInCustomerDemands);
                    }
                }
            }

            PopulateBoxes();
            BSave.Visible = false;
        }
        protected void grd_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var dataKey = GvBoxes.DataKeys[e.RowIndex];
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (dataKey != null)
                {
                    var id = Convert.ToInt32(dataKey["ID"]);
                    var query = context.Boxes.Where(item => item.ID == id);
                    var entity = query.First();
                    entity.BoxName = TbBoxName.Text;
                    entity.ShowBox = showcheck.Checked;
                    entity.ShowBoxInResellerPPR = showinResellerppr.Checked;
                    entity.ShowInCustomerDemands = showinCustomerDemands.Checked;
                }
                context.SubmitChanges();
            }

            GvBoxes.EditIndex = -1;
            Clear();
            BSave.Visible = true;
            PopulateBoxes();
        }


        void Clear()
        {
            TbBoxName.Text = string.Empty;
            showcheck.Checked = showinResellerppr.Checked = showinCustomerDemands.Checked = false;
        }


        protected void BDel_OnClick(object sender, EventArgs e)
        {
            var btn = sender as LinkButton;
            if (btn == null) return;

            var id = Convert.ToInt32(btn.CommandArgument);
            DeleteBox(id);
        }

        void HandlePrivildges()
        {
            //Session["User_ID"] = 1;
            int userId = Convert.ToInt32(Session["User_ID"]);
            CanEdit = _ispEntries.UserHasPrivlidge(userId, "EditDemand");
            CanDelete = _ispEntries.UserHasPrivlidge(userId, "DeleteDemand");
            CanAdd = _ispEntries.UserHasPrivlidge(userId, "AddDemand");
        }
        void PopulateBoxes()
        {
            using (var db = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var allboxes = db.Boxes.Select(x => new
                {
                    x.ID,
                    x.BoxName,
                    x.ShowBox,
                    x.ShowBoxInResellerPPR,
                    x.ShowInCustomerDemands,
                    BoxNet = x.BoxCredits.OrderByDescending(a => a.ID).FirstOrDefault(a => a.BoxId == x.ID).Net
                }).ToList();
                GvBoxes.DataSource = allboxes;
                GvBoxes.DataBind();
            }
        }
        protected void GvBox_OnDataBound(object sender, EventArgs e)
        {
            Helper.HideShowControl(GvBoxes, "EditBtn", CanEdit);
            Helper.HideShowControl(GvBoxes, "DeleteBtn", CanDelete);
            Helper.GridViewNumbering(GvBoxes, "LNo");
            // Helper.GridViewNumbering(GvBoxes, "LNo");
            //Helper.HideShowControl(GvPaid, "Unpay", CanUnpay);
        }

        protected void BSave_Click(object sender, EventArgs e)
        {
            using (var db2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var check = db2.Boxes.Where(a => a.BoxName == TbBoxName.Text).ToList();
                if (check.Count == 0)
                {
                    var box = new Box
                    {
                        BoxName = TbBoxName.Text,
                        ShowBox = showcheck.Checked,
                        ShowBoxInResellerPPR = showinResellerppr.Checked,
                        ShowInCustomerDemands = showinCustomerDemands.Checked
                    };
                    db2.Boxes.InsertOnSubmit(box);
                    db2.SubmitChanges();
                    Message.Text = Tokens.Saved;
                    Clear();
                    PopulateBoxes();
                }
                else
                {
                    Message.Text = Tokens.AlreadyExist;
                }
            }
        }
        protected void DeleteBox(int boxId)
        {
            using (var db3 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var check = db3.BoxCredits.Where(a => a.BoxId == boxId).ToList();
                if (check.Count == 0)
                {
                    var deletedrow = db3.Boxes.FirstOrDefault(s => s.ID == boxId);
                    if (deletedrow != null) db3.Boxes.DeleteOnSubmit(deletedrow);
                    db3.SubmitChanges();
                    PopulateBoxes();
                    Message.Text = Tokens.Deleted;
                }
                else
                {
                    Message.Text = Tokens.CantDelete;
                }
            }

        }


       
    }
}
