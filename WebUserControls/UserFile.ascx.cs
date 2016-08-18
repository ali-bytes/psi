using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using Db;
using System.Web.UI.WebControls;
using NewIspNL.Helpers;
using Resources;


namespace NewIspNL.WebUserControls
{
    public partial class UserFile :  UserControl
    {

        public List<WorkOrderFile> Files { get; set; }

        public bool CanEdit
        {
            get { return (bool)ViewState["CanEdit"]; }
            set
            {
                tr_Upload.Visible = value;
                //grd_Files.Columns[1].Visible = value;
                grd_Files.Columns[5].Visible = value;
                ViewState["CanEdit"] = value;
            }
        }

        public int Woid
        {
            get
            {
                if (ViewState["WOID"] != null)
                {
                    return (int)ViewState["WOID"];
                }
                return 0;
            }
            set
            {
                //BindGrd(value);
                ViewState["WOID"] = value;
                //populateFiles(value);
            }
        }

        public List<WorkOrderFile> FilesList
        {
            get
            {
                var list = new List<WorkOrderFile>();
                foreach (GridViewRow item in grd_Files.Rows)
                {
                    // var c = grd_Files.Rows.Count;
                    var id = item.FindControl("lblID") as Label;
                    //if(id != null){
                    var data = new WorkOrderFile();
                    if (id != null && id.Text != @"0")
                    {
                        data.ID = Convert.ToInt32(id.Text);
                    }
                    var label = item.FindControl("lblWorkOrderID") as Label;
                    if (label != null) data.WorkOrderID = Convert.ToInt32(label.Text);
                    var label1 = item.FindControl("lblVirtualName") as Label;
                    if (label1 != null) data.VirtualName = label1.Text;
                    var label2 = item.FindControl("lblNotes") as Label;
                    if (label2 != null) data.Notes = label2.Text;
                    var linkButton = item.FindControl("lnb_Download") as LinkButton;
                    if (linkButton != null) data.FileName = linkButton.Text;

                    list.Add(data);
                    //}
                }
                return list;
            }

        }

        public bool GetFiles
        {
            get
            {
                if (Woid != 0)
                {
                    BindGrd(Woid);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState.Add("CanEdit", true);
                //ViewState.Add("WOID", 0);
                if (Woid == 0) return;
                BindGrd(Woid);
            }

        }


        public void BindGrd(int woid)
        {
            using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var filesQuery = dataContext.WorkOrderFiles.Where(a => a.WorkOrderID == woid).ToList();
                var filesList = new List<WorkOrderFile>();
                foreach (WorkOrderFile wof in filesQuery)
                {
                    filesList.Add(wof);
                }
                grd_Files.DataSource = filesList;
                grd_Files.DataBind();
                //Session.Add("FilesList", _FilesList);
            }
        }

        protected void grd_Files_DataBounded(object sender, EventArgs e)
        {
           
          
            using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var groupIdQuery = dataContext.Users.FirstOrDefault(usr => usr.ID == Convert.ToInt32(Session["User_ID"]));
                if (groupIdQuery != null)
                {
                    var groupId = groupIdQuery.GroupID;
                    if (groupId != null && groupId==6)
                    {
                                grd_Files.Columns[5].Visible = false;
                        
                    }
                }

            }

        }

       
        protected void btn_Upload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                HttpFileCollection uploadedFiles = Request.Files;
                var filesList = new List<WorkOrderFile>();
                for (int i = 0; i < uploadedFiles.Count; i++)
                {
                    HttpPostedFile userPostedFile = uploadedFiles[i];

                    string virtualName = DateTime.Now.AddHours().ToFileTime().ToString(CultureInfo.InvariantCulture);
                    string fileName = userPostedFile.FileName;

                    var extensions = new List<string> {".jpg",".JPG",".gif",".GIF",".jpeg",".JPEG",".png",".PNG"};

                    string ex = Path.GetExtension(userPostedFile.FileName);

                    if (!string.IsNullOrEmpty(ex) && extensions.Any(currentExtention => currentExtention == ex))
                    {
                        userPostedFile.SaveAs(Server.MapPath(ConfigurationManager.AppSettings["App_Files"]) + "/" +
                                              virtualName + ".jpeg");

                        var wof = new WorkOrderFile
                        {
                            VirtualName = virtualName + ".jpeg",
                            WorkOrderID = (int) ViewState["WOID"],
                            FileName = fileName
                        };

                        if (FilesList != null && FilesList.Count != 0)
                            filesList = FilesList;
                        filesList.Add(wof);
                    }

                }
                if (filesList.Count > 0)
                {
                    grd_Files.DataSource = filesList;
                    grd_Files.DataBind();

                    lbl_Process.Text = Tokens.ItemAdded;
                    lbl_Process.ForeColor = Color.Green;
                }
                else
                {
                    lbl_Process.Text = Tokens.UploadFile;
                    lbl_Process.ForeColor = Color.Red;
                }
                   
                
            }
            else
            {
                lbl_Process.Text = Tokens.UploadFile;
                lbl_Process.ForeColor = Color.Red;
            }
        }


        protected void lnb_Download_Click(object sender, EventArgs e)
        {
            string virtualname = ((LinkButton)sender).CommandArgument;

            string filePath = Server.MapPath(ConfigurationManager.AppSettings["App_Files"]) + "/" + virtualname;
            string path = filePath;
            if (!File.Exists(path))
            {

                lbl_Process.Text = Tokens.Filesdeleted;
                lbl_Process.ForeColor = Color.Red;
                return;
            }
            var file = new FileInfo(path);
            FileStream ff = file.OpenRead();
            var filebyte = new byte[file.Length];
            ff.Read(filebyte, 0, Convert.ToInt32(file.Length));
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + ((LinkButton)sender).Text);
            Response.Flush();
            Response.OutputStream.Write(filebyte, 0, filebyte.Length);
            Response.End();
            Response.Flush();
            
        }


        protected void lnb_Delete_Click(object sender, EventArgs e)
        {
            using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                string virtualname = ((LinkButton)sender).CommandArgument;
                var filesList = FilesList;//(List<WorkOrderFile>) Session["FilesList"];
                foreach (WorkOrderFile wof in filesList)
                {
                    if (wof.VirtualName == virtualname)
                    {
                        filesList.Remove(wof);
                        var de = dataContext.WorkOrderFiles.FirstOrDefault(a => a.VirtualName == wof.VirtualName);
                        if (de != null) dataContext.WorkOrderFiles.DeleteOnSubmit(de);
                        dataContext.SubmitChanges();

                        break;
                    }
                }
                //Session["FilesList"] = _FilesList;
                //BindGrd(WOID);
                grd_Files.DataSource = filesList;
                grd_Files.DataBind();
            }
        }

        protected void grd_Files_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkbtn=(LinkButton)e.Row.FindControl("lnb_Download");

                string filePath = string.Format("/App_Files/{0}", lnkbtn.CommandArgument);
                System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)e.Row.FindControl("ImgAttachment");
                img.ImageUrl = filePath;
                img.Attributes.Add("data-content", filePath);

            }
        }
    }
}
