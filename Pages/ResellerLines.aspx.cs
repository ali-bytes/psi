using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class ResellerLines : CustomPage
    {
       


    protected void Page_Load(object sender, EventArgs e){
        if(Session["User_ID"] == null)
            return;

        if(!IsPostBack){
            Bind_ddl_Reseller();
        }
    }


    void Bind_ddl_Reseller(){
        var query = DataLevelClass.GetUserReseller();
        ddl_Reseller.DataSource = query;
        ddl_Reseller.DataBind();
        Helper.AddDefaultItem(ddl_Reseller);
    }


    protected void btn_search0_Click(object sender, EventArgs e)
    {

        tr_CustomerDetails.Visible = false;
        tr_Status.Visible = false;
        Search();

    }

    void Search()
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var groupIdQuery = context.Users.FirstOrDefault(usr => usr.ID == Convert.ToInt32(Session["User_ID"]));
            if (groupIdQuery == null)
            {
                Response.Redirect("UnAuthorized.aspx");
                return;
            }
            var dataLevelId = groupIdQuery.Group.DataLevel.ID;



            if (dataLevelId != 1)
            {

                grd_wo.Columns[12].Visible = false;



            }
        }









        var connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        using (var cmd = new SqlCommand("PROC_SEARCH_WO", connection) { CommandType = CommandType.StoredProcedure })
        {
            
            cmd.Parameters.Add(new SqlParameter("@CustomerName", SqlDbType.NVarChar)).Value = "";
            cmd.Parameters.Add(new SqlParameter("@CustomerGovernorateID", SqlDbType.Int)).Value = -1;
            cmd.Parameters.Add(new SqlParameter("@CustomerPhone", SqlDbType.NVarChar)).Value = "";
            cmd.Parameters.Add(new SqlParameter("@IpPackageID", SqlDbType.Int)).Value = -1;
            cmd.Parameters.Add(new SqlParameter("@CustomerMobile", SqlDbType.NVarChar)).Value = "";
            cmd.Parameters.Add(new SqlParameter("@CustomerEmail", SqlDbType.NVarChar)).Value = "";
            cmd.Parameters.Add(new SqlParameter("@ServiceProviderID", SqlDbType.Int)).Value = -1;
            cmd.Parameters.Add(new SqlParameter("@ServicePackageID", SqlDbType.Int)).Value = -1;
            cmd.Parameters.Add(new SqlParameter("@ResellerID", SqlDbType.Int)).Value =
                Convert.ToInt32(ddl_Reseller.SelectedItem.Value);

            cmd.Parameters.Add(new SqlParameter("@BranchID", SqlDbType.Int)).Value = -1;
            cmd.Parameters.Add(new SqlParameter("@VPI", SqlDbType.NVarChar)).Value = "";
            cmd.Parameters.Add(new SqlParameter("@VCI", SqlDbType.NVarChar)).Value = "";
            cmd.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar)).Value = "";
            cmd.Parameters.Add(new SqlParameter("@WorkOrderStatusID", SqlDbType.Int)).Value = -1;
            connection.Open();
            var table = new DataTable();
            table.Load(cmd.ExecuteReader());
            ViewState["No"] = null;
            grd_wo.DataSource = table;
            grd_wo.DataBind();
            connection.Close();
        }
    }

    protected void lnb_Edit_Click(object sender, EventArgs e){
        ViewWoDetails(Convert.ToInt32(((LinkButton) sender).CommandArgument));
        tr_CustomerDetails.Visible = true;
        tr_Status.Visible = true;
        ViewState.Add("ID", Convert.ToInt32(((LinkButton) sender).CommandArgument));
    }


    void ViewWoDetails(int id){
        using(var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){

            var query = from wo in dataContext.WorkOrders
                where wo.ID == id
                select new{
                    wo.CustomerName,
                    wo.Governorate.GovernorateName,
                    wo.CustomerAddress,
                    wo.CustomerPhone,
                    wo.IpPackage.IpPackageName,
                    wo.CustomerMobile,
                    wo.CustomerEmail,
                    wo.ServiceProvider.SPName,
                    wo.ServicePackage.ServicePackageName,
                    reseller = wo.User.UserName,
                    wo.Branch.BranchName,
                    wo.VPI,
                    wo.VCI,
                    wo.UserName,
                    wo.Status.StatusName,
                    wo.Password,
                    ExtraGigas = wo.ExtraGiga.Name
                };
            var workOrderStatus = from wos in dataContext.WorkOrderStatus
                where wos.WorkOrderID == id
                select new{
                    wos.Status.StatusName,
                    wos.User.UserName,
                    wos.UpdateDate
                };

            lbl_BranchName.Text = query.First().BranchName;
            lbl_Client_UserName.Text = query.First().UserName;
            lbl_CustomerAddress.Text = query.First().CustomerAddress;
            lbl_CustomerEmail.Text = query.First().CustomerEmail;
            lbl_CustomerMobile.Text = query.First().CustomerMobile;
            lbl_CustomerName.Text = query.First().CustomerName;
            lbl_CustomerPhone.Text = query.First().CustomerPhone;
            lbl_ExtraGigas.Text = query.First().ExtraGigas;
            lbl_GovernorateName.Text = query.First().GovernorateName;
            lbl_IpPackageName.Text = query.First().IpPackageName;
            lbl_Password.Text = query.First().Password;
            lbl_ResellerName.Text = query.First().reseller;
            lbl_ServicePackageName.Text = query.First().ServicePackageName;
            lbl_SPName.Text = query.First().SPName;
            lbl_StatusName.Text = query.First().StatusName;
            lbl_VCI.Text = query.First().VCI;
            lbl_VPI.Text = query.First().VPI;
            GridView1.DataSource = workOrderStatus;
            GridView1.DataBind();
        }
    }


    protected void grd_wo_RowDataBound(object sender, GridViewRowEventArgs e){
      
        Helper.GridViewNumbering(grd_wo,"lbl_No");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var status = ((HiddenField) e.Row.FindControl("hdfStatus")).Value;
            if (!string.IsNullOrWhiteSpace(status))
            {
                var activeBtn = ((LinkButton) e.Row.FindControl("lnkActive"));
                var autoSuspendBtn = ((LinkButton)e.Row.FindControl("lnkAutoSuspend"));
                if (status == "6") {activeBtn.Visible = false;
                    autoSuspendBtn.Visible = true;
                }
                else if (status == "12")
                {
                    activeBtn.Visible = true;
                    autoSuspendBtn.Visible = false;
                }
                else
                {
                    activeBtn.Visible = false;
                    autoSuspendBtn.Visible = false;
                }
                    
            }
        }
    }


    protected void b_export_Click(object sender, EventArgs e){
       // BranchPrintExcel = true;
        const string attachment = "attachment; filename=ResellerLines.xls";
        Response.ClearContent();
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/ms-excel";
        Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");

        var sw = new StringWriter();
        var htw = new HtmlTextWriter(sw);
        grd_wo.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
    }


    public override void VerifyRenderingInServerForm(Control control){
        //don't throw any exception!
    }

    protected void btn_Active_Click(object sender, EventArgs e)
    {
        var btn = ((LinkButton) sender).CommandArgument;
        if (!string.IsNullOrWhiteSpace(btn))
        {
            ExecuteMyProccess(btn,6);
        }
    }

    protected void btn_AutoSuspend_Click(object sender, EventArgs e)
    {
        var btn = ((LinkButton)sender).CommandArgument;
        if (!string.IsNullOrWhiteSpace(btn))
        {
            ExecuteMyProccess(btn,12);
        }
    }

    void ExecuteMyProccess(string btn,int statusId)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var rowId = Convert.ToInt32(btn);
            var order = context.WorkOrders.FirstOrDefault(a => a.ID == rowId);
            if (order != null)
            {
                var userId = Convert.ToInt32(Session["User_ID"]);
                var thisday = DateTime.Now.AddHours();
                order.WorkOrderStatusID = statusId;
                global::Db.WorkOrderStatus wos = new global::Db.WorkOrderStatus
                {
                    WorkOrderID = order.ID,
                    StatusID = statusId,
                    UserID = userId,
                    UpdateDate = thisday
                };
                context.WorkOrderStatus.InsertOnSubmit(wos);
                context.SubmitChanges();
                Search();
            }
        }
    }
}
}