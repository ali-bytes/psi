using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using NewIspNL.Services;
using Resources;

namespace NewIspNL.Pages
{
    public partial class CustomerFiles : CustomPage
    {

      
            public OptionsService Options;

            public  CustomerFiles()
            {
                Options = new OptionsService();
            }
            protected void Page_Load(object sender, EventArgs e)
            {
                if (!IsPostBack)
                {
                    //Session["FilesList"] = null; //clear List
                    UserFile1.Woid = 0;
                    Bind_ddl_Governorates();
                    //CanEdit();
                }
            }
            void PopulateFiles(int woid)
            {
                UserFile1.Woid = woid;
                var l = UserFile1.GetFiles;
            }

            /*void CanEdit()
            {
                using (var contex=new ISPDataContext())
                {
                    var userId= Convert.ToInt32(Session["User_ID"]);
                    var user = contex.Users.FirstOrDefault(a => a.ID == userId);
                    var groupPrivilegeQuery = contex.GroupPrivileges.Where(gp => gp.Group.ID == user.GroupID).Select(gp => gp.privilege.Name);
                    UserFile1.CanEdit = groupPrivilegeQuery.Contains("EditCustomer.aspx");
                }
        
            }*/

            protected void btn_search_Click(object sender, EventArgs e)
            {
                //Session["FilesList"] = null;
                LoadCustomerFiles();
            }


            void Bind_ddl_Governorates()
            {
                using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var option = OptionsService.GetOptions(dataContext, true); //contex.Options.FirstOrDefault();
                    if (option != null && option.IncludeGovernorateInSearch)
                    {
                        GovBox.Visible = true;
                        var query = dataContext.Governorates.Select(gov => gov);
                        ddl_Governorates0.SelectedValue = null;
                        ddl_Governorates0.Items.Clear();

                        ddl_Governorates0.AppendDataBoundItems = true;
                        ddl_Governorates0.DataSource = query;
                        ddl_Governorates0.DataBind();
                        Helper.AddDefaultItem(ddl_Governorates0, Tokens.__Chose__);
                    }
                    else
                    {
                        GovBox.Visible = false;
                    }
                }
            }


            void LoadCustomerFiles()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    //var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
                    List<WorkOrder> userWorkOrders = DataLevelClass.GetUserWorkOrder();
                    WorkOrder targetWo;
                    if (rbl_searchType.SelectedValue == "1" && GovBox.Visible)
                    {
                        targetWo = context.WorkOrders.FirstOrDefault(wo => wo.CustomerGovernorateID == Convert.ToInt32(ddl_Governorates0.SelectedItem.Value)
                                                                           && wo.CustomerPhone == txt_CustomerPhone0.Text.Trim());
                    }
                    else
                    {
                        targetWo = context.WorkOrders.FirstOrDefault(wo => wo.CustomerPhone.Contains(txt_CustomerPhone0.Text));
                    }
                    if (targetWo != null)
                    {
                        IEnumerable<bool> matchedList = userWorkOrders.Select(tmpwo => tmpwo.ID == targetWo.ID);
                        if (matchedList.Contains(true))
                        {
                            //var flag = UserFile1.CanEdit = false;

                            //var groupIdQuery = context.Users.FirstOrDefault(usr => usr.ID == Convert.ToInt32(Session["User_ID"]));
                            //if (groupIdQuery != null)
                            //{
                            //    var groupId = groupIdQuery.GroupID;
                            //    if (groupId != null)
                            //    {
                            //        var groupIDvalu = groupId.Value;
                            //        var groupPrivilegeQuery = context.GroupPrivileges.Where(gp => gp.Group.ID == groupIDvalu);
                            //        foreach (GroupPrivilege tmpgp in groupPrivilegeQuery)
                            //        {
                            //            if (tmpgp.privilege.Name == "EditCustomer.aspx")
                            //            {
                            //                flag = true;
                            //                break;
                            //            }
                            //        }
                            //    }
                            //}

                            //if (flag) UserFile1.CanEdit = true;

                            UserFile1.Visible = true;
                            UserFile1.CanEdit = true;
                            UserFile1.Woid = targetWo.ID;
                            PopulateFiles(targetWo.ID);
                            //var l = UserFile1.GetFiles;
                            btn_Update.Visible = true;

                        }
                        else
                        {
                            UserFile1.Visible = false;
                            btn_Update.Visible = false;
                        }
                    }
                    else
                    {
                        UserFile1.Visible = false;
                        btn_Update.Visible = false;
                    }
                }
            }


            void SaveFiles(int woid)
            {
                using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    UserFile1.Woid = woid;
                    var fl = UserFile1.FilesList;
                    var fl2 = new List<WorkOrderFile>();
                    foreach (WorkOrderFile tempwof in fl)
                    {
                        tempwof.WorkOrderID = woid;
                        fl2.Add(new WorkOrderFile
                        {
                            FileName = tempwof.FileName,
                            VirtualName = tempwof.VirtualName,
                            WorkOrderID = tempwof.WorkOrderID,
                            Notes = tempwof.Notes
                        });
                    }
                    var query = from wof in dataContext.WorkOrderFiles
                                where wof.WorkOrderID == woid
                                select wof;

                    dataContext.WorkOrderFiles.DeleteAllOnSubmit(query);
                    dataContext.SubmitChanges();
                    dataContext.WorkOrderFiles.InsertAllOnSubmit(fl2);
                    dataContext.SubmitChanges();
                }
            }


            protected void btn_Update_Click(object sender, EventArgs e)
            {
                SaveFiles(UserFile1.Woid);


                lbl_InsertResult.Text = Tokens.CustomerFilesSaved;
                lbl_InsertResult.ForeColor = Color.Green;
            }
        }
    }
 