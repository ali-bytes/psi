using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class TicketsReport : CustomPage
    {
     protected void Page_Load(object sender, EventArgs e){
        if(Session["User_ID"] == null)
            return;
    }


    protected void btn_ViewTicket_Click(object sender, EventArgs e){
        using(var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            if(!Page.IsValid)
                return;

            var toDate = Convert.ToDateTime(txt_Date2.Text);
            var fromDate = Convert.ToDateTime(txt_Date1.Text);

            var first = (from usr in dataContext.Users
                where usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])
                select usr.Group.DataLevelID).First();
            if(first != null){
                var dataLevel = first.Value;
                var i = dataContext.Users.Where(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).
                    Select(usr => usr.BranchID).First();
                if(i != null){
                    int userBranchID = i.Value;


                    if(dataLevel == 1) //sys admin
                    {
                        var Connection =
                            new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                        var Cmd = new SqlCommand("PROC_GET_TICKETS", Connection);
                        Cmd.CommandType = CommandType.StoredProcedure;
                        Cmd.Parameters.Add(new SqlParameter("@TICKETSTATUSID", SqlDbType.Int)).Value = DBNull.Value;
                        Cmd.Parameters.Add(new SqlParameter("@USERID", SqlDbType.Int)).Value = DBNull.Value;
                        if(txt_Date1.Text != ""){
                            Cmd.Parameters.AddWithValue("@FROMDATE", fromDate);
                        } else
                            Cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.DateTime)).Value = DBNull.Value;

                        if(txt_Date1.Text != "")
                            Cmd.Parameters.AddWithValue("@TODATE", toDate);
                        else
                            Cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.DateTime)).Value = DBNull.Value;

                        Cmd.Parameters.Add(new SqlParameter("@WorkOrderID", SqlDbType.Int)).Value = DBNull.Value;
                        Cmd.Parameters.Add(new SqlParameter("@BRANCHID", SqlDbType.Int)).Value = DBNull.Value;
                        Cmd.Parameters.Add(new SqlParameter("@BRANCHADMINID", SqlDbType.Int)).Value = DBNull.Value;

                        Connection.Open();
                        var table = new DataTable();
                        table.Load(Cmd.ExecuteReader());
                        Connection.Close();

                        grd_Tickets.DataSource = table;
                        grd_Tickets.DataBind();
                    } else if(dataLevel == 2) //branch admin
                    {
                        var Connection =
                            new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                        var Cmd = new SqlCommand("PROC_GET_TICKETS", Connection);
                        Cmd.CommandType = CommandType.StoredProcedure;
                        Cmd.Parameters.Add(new SqlParameter("@TICKETSTATUSID", SqlDbType.Int)).Value = DBNull.Value;
                        Cmd.Parameters.Add(new SqlParameter("@USERID", SqlDbType.Int)).Value = DBNull.Value;
                        if(txt_Date1.Text != ""){
                            Cmd.Parameters.AddWithValue("@FROMDATE", fromDate);
                        } else
                            Cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.DateTime)).Value = DBNull.Value;

                        if(txt_Date1.Text != "")
                            Cmd.Parameters.AddWithValue("@TODATE", toDate);
                        else
                            Cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.DateTime)).Value = DBNull.Value;

                        Cmd.Parameters.Add(new SqlParameter("@WorkOrderID", SqlDbType.Int)).Value = DBNull.Value;
                        Cmd.Parameters.Add(new SqlParameter("@BRANCHID", SqlDbType.Int)).Value = userBranchID;
                        Cmd.Parameters.Add(new SqlParameter("@BRANCHADMINID", SqlDbType.Int)).Value = Convert.ToInt32(Session["User_ID"]);

                        Connection.Open();
                        var table = new DataTable();
                        table.Load(Cmd.ExecuteReader());
                        Connection.Close();

                        grd_Tickets.DataSource = table;
                        grd_Tickets.DataBind();
                    } else if(dataLevel == 3) //reseller
                    {
                        var Connection =
                            new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                        var Cmd = new SqlCommand("PROC_GET_TICKETS", Connection);
                        Cmd.CommandType = CommandType.StoredProcedure;
                        Cmd.Parameters.Add(new SqlParameter("@TICKETSTATUSID", SqlDbType.Int)).Value = DBNull.Value;
                        Cmd.Parameters.Add(new SqlParameter("@USERID", SqlDbType.Int)).Value = Convert.ToInt32(Session["User_ID"]);
                        if(txt_Date1.Text != ""){
                            Cmd.Parameters.AddWithValue("@FROMDATE", fromDate);
                        } else
                            Cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.DateTime)).Value = DBNull.Value;

                        if(txt_Date1.Text != "")
                            Cmd.Parameters.AddWithValue("@TODATE", toDate);
                        else
                            Cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.DateTime)).Value = DBNull.Value;

                        Cmd.Parameters.Add(new SqlParameter("@WorkOrderID", SqlDbType.Int)).Value = DBNull.Value;
                        Cmd.Parameters.Add(new SqlParameter("@BRANCHID", SqlDbType.Int)).Value = DBNull.Value;
                        Cmd.Parameters.Add(new SqlParameter("@BRANCHADMINID", SqlDbType.Int)).Value = DBNull.Value;

                        Connection.Open();
                        var table = new DataTable();
                        table.Load(Cmd.ExecuteReader());
                        Connection.Close();

                        grd_Tickets.DataSource = table;
                        grd_Tickets.DataBind();
                    }
                }
            }
        }
    }
 
    protected void btnExport_click(object sender, EventArgs e)
    {
        //creating the array of GridViews and calling the Export function
        var gvList = new GridView[] { grd_Tickets };
        GridHelper.Export("TicketsReport.xls", gvList);
    }
}

}