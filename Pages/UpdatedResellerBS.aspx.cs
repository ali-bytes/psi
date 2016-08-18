using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class UpdatedResellerBS : CustomPage
    {
       readonly IspDomian _domain;
    private readonly IUserSaveRepository _userSave;
    //static bool BranchPrintExcel;

    public UpdatedResellerBS()
    {
        _domain = new IspDomian(IspDataContext);
        _userSave=new UserSaveRepository();
    }


    protected void Page_Load(object sender, EventArgs e){
        
            if(IsPostBack) return;
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            _domain.PopulateResellers(ddl_reseller);
            var x = context.Users.FirstOrDefault(a => a.ID == Convert.ToInt32(Session["User_ID"]));
            if (x != null)
            {
                hf_user.Value = x.GroupID.ToString();
                PopulateSaves(x.ID, context);
            }
            Privilages();

        }
    }

    void PopulateSaves(int userId,ISPDataContext context)
    {
        ddlSaves.DataSource = _userSave.SavesOfUser(userId, context).Select(a => new
        {
            a.Save.SaveName,
            a.Save.Id
        });
        ddlSaves.DataBind();
        Helper.AddDefaultItem(ddlSaves);
    }

    protected void GV_BalanceSheet_DataBound(object sender, EventArgs e)
    {
        Helper.GridViewNumbering(GV_BalanceSheet, "gv_lNumber");
    }
    protected void gv_ResellerPayment_DataBound(object sender, EventArgs e)
    {
        Helper.GridViewNumbering(gv_ResellerPayment, "gv_lNumber");

    }


    void Privilages(){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var privilages = context.UpdatedResellerBSPrivilages.Where(s => s.GroupId == Convert.ToInt32(hf_user.Value)).ToList();
            if(privilages.Count != 0){
                var colsBS = GV_BalanceSheet.Columns;
                var colRP = gv_ResellerPayment.Columns;
                foreach(var item in privilages){
                    if(item.ItemName == "اضافة فواتير الموزع"){
                        divAmount.Visible = true;
                    }
                    if (item.ItemName == "تعديل")
                    {
                        colsBS[5].Visible = true;
                    }
                    if (item.ItemName == "اضافة مدفوعات الموزع")
                    {
                        divpay.Visible = true;
                    }
                    if (item.ItemName == "حذف")
                    {
                        colRP[3].Visible = true;
                    }
                    if (item.ItemName == "طباعه")
                    {
                        colRP[2].Visible = true;
                    }
                }


            }
        }
    }


    protected void BDel_OnClick(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var btn = sender as LinkButton;
            if(btn == null) return;

            var id = Convert.ToInt32(btn.CommandArgument);
            var todelete = context.UpdatedResellerPayments.FirstOrDefault(a => a.Id == id);
            if(todelete != null) context.UpdatedResellerPayments.DeleteOnSubmit(todelete);
            context.SubmitChanges();
            PopulateResellerPayment(Helper.GetDropValue(ddl_reseller));
            GetTotalCredit();
        }
    }


    protected void btnsearch_Click(object sender, EventArgs e)
    {
        var reseller = Helper.GetDropValue(ddl_reseller);
        PopulateResellerBalance(reseller);
        PopulateResellerPayment(reseller);
        data.Visible = true;
        GetTotalCredit();
    }


    protected void PopulateResellerPayment(int ? ResellerId){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var payments = context.UpdatedResellerPayments.Where(p => p.ResellerId == Helper.GetDropValue(ddl_reseller)).ToList();

            gv_ResellerPayment.DataSource = payments;
            gv_ResellerPayment.DataBind();
            lblTotalPayment.Text = payments.Sum(a => a.Total).ToString();
            GetTotalCredit();
        }
    }


    protected void PopulateResellerBalance(int ? resellerId){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var balance = context.UpdatedResellerBs.Where(p => p.ResellerId == Helper.GetDropValue(ddl_reseller)).Select(x => new{
                x.Invoice,
                x.InvoiceAfterReview,
                x.InvoiceBeforeReview,
                User = x.User.UserName,
                x.ID,
                Reseller = x.User1.UserName
            }).ToList();

            GV_BalanceSheet.DataSource = balance;
            GV_BalanceSheet.DataBind();
            lblTotalBeforeReview.Text = balance.Sum(a => a.InvoiceBeforeReview).ToString();
            lblTotalAfterReview.Text = balance.Sum(s => s.InvoiceAfterReview).ToString();

        }
    }


    protected void btnPayment_Click(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var reseller = Helper.GetDropValue(ddl_reseller);
            context.UpdatedResellerPayments.InsertOnSubmit(new UpdatedResellerPayment(){
                ResellerId = reseller,
                Total = Convert.ToDecimal(txtAmountPayment.Text),
                Notes = txtNotes.Text,
                Time = DateTime.Now.AddHours()
            });
            var resellerdata = context.Users.FirstOrDefault(a => a.ID == reseller);
            var saveId = Convert.ToInt32(ddlSaves.SelectedItem.Value);
            var userId = Convert.ToInt32(Session["User_ID"]);
            if (resellerdata != null)
            {
                var note = "دفع موزع من كشف حساب موزع معدل " + " " + Tokens.Reseller + " : " + resellerdata.UserName +
                           " - " +
                           Tokens.Phone + " : " + resellerdata.UserPhone;
                var res=_userSave.BranchAndUserSaves(saveId, userId, Convert.ToDouble(txtAmountPayment.Text),note,txtNotes.Text,context );
                switch (res)
                {
                    case SaveResult.Saved:
                        l_message.InnerHtml = Tokens.Saved;
                        l_message.Visible = true;
                        context.SubmitChanges();
                        break;
                    case SaveResult.NoCredit:
                        l_message.InnerHtml = Tokens.NotEnoughtCreditMsg;
                        l_message.Visible = true;
                        break;
                }
            } //Savestepsinsaves(resellerdata.UserName,resellerdata.UserPhone,Convert.ToDouble(txtAmountPayment.Text));
            context.SubmitChanges();
            PopulateResellerPayment(reseller);
            txtAmountPayment.Text = txtNotes.Text = string.Empty;
            ddlSaves.SelectedIndex = -1;
            GetTotalCredit();
        }
    }


    protected void b_save_Click(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var reseller = Helper.GetDropValue(ddl_reseller);
            context.UpdatedResellerBs.InsertOnSubmit(new UpdatedResellerB(){
                Invoice = txtmonthyear.Text,
                InvoiceAfterReview = Convert.ToDecimal(txtAmountAfterReview.Text),
                InvoiceBeforeReview = Convert.ToDecimal(txtAmountBeforeReview.Text),
                ResellerId = reseller,
                UserId = Convert.ToInt32(Session["User_ID"])
            });
            context.SubmitChanges();
            PopulateResellerBalance(reseller);
            txtmonthyear.Text = txtAmountAfterReview.Text = txtAmountBeforeReview.Text = string.Empty;
            GetTotalCredit();
        }
    }


    protected void GV_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GV_BalanceSheet.EditIndex = -1;
        PopulateResellerBalance(Helper.GetDropValue(ddl_reseller));
    }


    protected void GV_RowUpdating(object sender, GridViewUpdateEventArgs e){
        using(var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var dataKey = GV_BalanceSheet.DataKeys[e.RowIndex];
            if(dataKey != null){
                int id = Convert.ToInt32(dataKey["ID"]);
                var query = _context.UpdatedResellerBs.Where(Item => Item.ID == id);
                UpdatedResellerB entity = query.First();
                entity.Invoice = ((TextBox) (GV_BalanceSheet.Rows[e.RowIndex].FindControl("TbInvoice"))).Text.Trim();
                entity.InvoiceAfterReview = Convert.ToDecimal(((TextBox) (GV_BalanceSheet.Rows[e.RowIndex].FindControl("TbAfterReview"))).Text.Trim());
                entity.InvoiceBeforeReview = Convert.ToDecimal(((TextBox) (GV_BalanceSheet.Rows[e.RowIndex].FindControl("TbBeforeReview"))).Text.Trim());
                _context.SubmitChanges();
                GV_BalanceSheet.EditIndex = -1;
                PopulateResellerBalance(entity.ResellerId);
                GetTotalCredit();
            }
        }
    }


    protected void GV_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GV_BalanceSheet.EditIndex = e.NewEditIndex;
        PopulateResellerBalance(Helper.GetDropValue(ddl_reseller));
    }


    void GetTotalCredit()
    {
        if (!string.IsNullOrEmpty(lblTotalAfterReview.Text) || !string.IsNullOrEmpty(lblTotalPayment.Text))
        {
            var afterReview = Convert.ToDecimal(lblTotalAfterReview.Text);
            var totalPayment = Convert.ToDecimal(lblTotalPayment.Text);
            lblTotalCredit.Text = (afterReview - totalPayment).ToString();
        }
        else
        {
            lblTotalCredit.Text = @"0";
        }
    }


    protected void btnExport_click(object sender, EventArgs e){
        var gvList = new GridView[] { GV_BalanceSheet,gv_ResellerPayment };
       GridHelper.Export("UpdatedResellerBS.xls", gvList);
    }
    }
}