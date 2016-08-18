using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewIspNL.Helpers{
    public static class GridHelper{
        public static void ShowAllColumns(DataControlFieldCollection columnCollection){
            foreach(DataControlField column in columnCollection){
                column.Visible = true;
            }
        }


        public static void HideAllColumns(DataControlFieldCollection columnCollection){
            foreach(DataControlField column in columnCollection){
                column.Visible = false;
            }
        }


        public static void ShowExactColumns(DataControlFieldCollection columnCollection, List<string> names){
            foreach(DataControlField column in columnCollection){
                column.Visible = names.Any(x => x == column.HeaderText);
            }
        }


        public static void HideExactColumns(DataControlFieldCollection columnCollection, List<string> names){
            foreach(DataControlField column in columnCollection){
                column.Visible = names.All(x => x != column.HeaderText);
            }
        }
        public static void Export(string fileName, GridView[] gvs)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");

            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);


            foreach (var gv in gvs)
            {
                //   Create a form to contain the grid
                var table = new Table {GridLines = gv.GridLines};
                //   add the header row to the table
                if (gv.HeaderRow != null)
                {
                    PrepareControlForExport(gv.HeaderRow);
                    table.Rows.Add(gv.HeaderRow);
                }
                //   add each of the data rows to the table
                foreach (GridViewRow row in gv.Rows)
                {
                    PrepareControlForExport(row);
                    table.Rows.Add(row);
                }
                //   add the footer row to the table
                if (gv.FooterRow != null)
                {
                    PrepareControlForExport(gv.FooterRow);
                    table.Rows.Add(gv.FooterRow);
                }
                //   render the table into the htmlwriter
                table.RenderControl(htw);
            }
            //   render the htmlwriter into the response
            HttpContext.Current.Response.Write(sw.ToString());
            HttpContext.Current.Response.End();
        }

        private static void PrepareControlForExport(Control control)
        {
            for (int i = 0; i < control.Controls.Count; i++)
            {
                var current = control.Controls[i];
                if (current is LinkButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
                }
                else if (current is ImageButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
                }
                else if (current is HyperLink)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
                }
                else if (current is DropDownList)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
                }
                else if (current is CheckBox)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
                }


                if (current.HasControls())
                {
                    PrepareControlForExport(current);
                }
            }
        }

        public static void ExportOneGrid(string fileName, GridView gvs)
        {
            var attachment = string.Format("attachment; filename={0}.xls",
                fileName);
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            //HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Unicode;
           // HttpContext.Current.Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            HttpContext.Current.Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");

            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);
            gvs.RenderControl(htw);

            HttpContext.Current.Response.Write(sw.ToString());
            HttpContext.Current.Response.End();
        }
    }
}
