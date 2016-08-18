using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewIspNL
{
    public partial class Fawry : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            var connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
            var cmd = new SqlCommand("PROC_GETDemand", connection) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@CustomerPhone", SqlDbType.NVarChar)).Value = txtSearch.Text;


            connection.Open();
            var table = new DataTable();
            table.Load(cmd.ExecuteReader());
            connection.Close();
            grd.DataSource = table;
            grd.DataBind();
        }
    }
}