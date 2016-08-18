using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class Groups : CustomPage
    {
       
            ArrayList _lstSelectPrivilages;

            private readonly ArrayList _lstSelectPrivilagesUpdate = new ArrayList();

            //ISPDataContext DataContext = new ISPDataContext(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
            protected void Page_Load(object sender, EventArgs e)
            {
                //if (Session["User_ID"] == null)
                //    return;
                ManageVisiblity();
                if (Page.IsPostBack) return;
                LoadPrivTable();
                var table = (DataTable)ViewState["TPrivliges"];
                BindPrivilages(table, treePRIVILAGES2);
                Bind_ddl_Groups();
                Bind_ddl_DataLevel();
            }


            void LoadPrivTable()
            {
                var Connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                var Cmd = new SqlCommand("Select * from privileges", Connection);
                Connection.Open();
                var table = new DataTable();
                table.Load(Cmd.ExecuteReader());
                Connection.Close();
                var dv = new DataView(table)
                {
                    Sort = "PrivOrder" + " ASC"
                };
                table = dv.ToTable();

                foreach (DataRow row in table.Rows)
                {
                    var orginal = row["LinkedName"].ToString();
                    var Localizer = new Loc();
                    var localized = Localizer.IterateResource(orginal);
                    row["LinkedName"] = localized;
                }
                ViewState["TPrivliges"] = table;


            }


            void Bind_ddl_Groups()
            {
                using (var DataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var Query = DataContext.Groups.Select(grp => grp);
                    ddl_Groups.DataSource = Query;
                    ddl_Groups.DataBind();
                    Helper.AddDefaultItem(ddl_Groups);
                }
            }


            protected void btn_Insert_Click(object sender, EventArgs e)
            {
                using (var DataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    _lstSelectPrivilages = new ArrayList();
                    foreach (TreeNode treeNode in treePRIVILAGES2.Nodes)
                    {
                        RecursiveNode(treeNode, "Insert");
                    }
                    var group_Obj = new Group();
                    group_Obj.GroupName = txt_GroupName.Text.Trim();
                    group_Obj.DataLevelID = Convert.ToInt32(ddl_DataLevel.SelectedValue);
                    DataContext.Groups.InsertOnSubmit(group_Obj);
                    DataContext.SubmitChanges();

                    InsertTreeItems(group_Obj.ID);
                    treePRIVILAGES2.Nodes.Clear();
                    LoadPrivTable();
                    var tablename = (DataTable)ViewState["TPrivliges"];
                    BindPrivilages(tablename, treePRIVILAGES2);
                    txt_GroupName.Text = "";
                    lbl_ProcessResult.Text = Tokens.ItemAdded;
                    lbl_ProcessResult.ForeColor = Color.Green;
                    treePRIVILAGES2.ExpandAll();
                    Bind_ddl_Groups();
                }
            }


            void RecursiveNode(TreeNode treeNode, string InsertOrUpdate)
            {
                _lstSelectPrivilages = new ArrayList();
                if (treeNode.Checked)
                {
                    if (InsertOrUpdate == "Insert")
                    {
                        _lstSelectPrivilages.Add(treeNode.Value);
                    }
                    else
                    {
                        _lstSelectPrivilagesUpdate.Add(treeNode.Value);
                    }
                }
                else if (treeNode.ChildNodes.Count < 1)
                {
                    return;
                }
                else
                {
                    bool flag = false;
                    for (int i = 0; i < treeNode.ChildNodes.Count; i++)
                    {
                        if (treeNode.ChildNodes[i].Checked)
                        {
                            flag = true;
                        }
                    }
                    if (flag)
                    {
                        _lstSelectPrivilages.Add(treeNode.Value);
                    }
                    foreach (TreeNode childNode in treeNode.ChildNodes)
                    {
                        RecursiveNode(childNode, InsertOrUpdate);
                    }
                }
            }

            void BindPrivilages(DataTable table, TreeView myTree)
            {
                foreach (DataRow privilage in table.Rows)
                {
                    var newNode = new TreeNode
                    {
                        Value = privilage["ID"].ToString(),
                        Text = privilage["LinkedName"].ToString()
                    };
                    AttachByData(newNode, Convert.ToInt32(privilage["ParentID"]), myTree);
                }
            }


            void AttachByData(TreeNode newNode, int parentId, TreeView myTree)
            {
                if (parentId == 0)
                {
                    myTree.Nodes.Add(newNode);
                }
                else
                {
                    foreach (TreeNode existNode in myTree.Nodes)
                    {
                        FindRecursive(newNode, existNode, parentId);
                    }
                }
            }


            void FindRecursive(TreeNode newNode, TreeNode existNode, int parentId)
            {
                if (existNode.Value == parentId.ToString())
                {
                    existNode.ChildNodes.Add(newNode);
                }
                else
                {
                    foreach (TreeNode node in existNode.ChildNodes)
                    {
                        FindRecursive(newNode, node, parentId);
                    }
                }
            }


            protected void btn_Edit_Click(object sender, EventArgs e)
            {
                using (var DataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    UpdateGroup();
                    if (ddl_Groups.SelectedItem.Value == "-1")
                    {
                        ManageVisiblity();
                        var table2 = (DataTable)ViewState["TPrivliges"];
                        BindPrivilages(table2, treePRIVILAGES2);
                        treePRIVILAGES2.ExpandAll();
                    }
                    else
                    {
                        UpdateGroup();
                        txt_GroupName.Text = ddl_Groups.SelectedItem.Text;
                        ddl_DataLevel.SelectedValue = Convert.ToString((from grp in DataContext.Groups
                                                                        where grp.ID == Convert.ToInt32(ddl_Groups.SelectedValue)
                                                                        select grp.DataLevelID).FirstOrDefault());
                        var table2 = (DataTable)ViewState["TPrivliges"];
                        treePRIVILAGES2.Nodes.Clear();
                        BindPrivilages(table2, treePRIVILAGES2);
                        var GroupPrivilages = new ListBox();
                        var Query = from GP in DataContext.GroupPrivileges
                                    where GP.GroupID == int.Parse(ddl_Groups.SelectedItem.Value)
                                    select GP;
                        GroupPrivilages.DataSource = Query;
                        GroupPrivilages.DataTextField = "PrivilegeID";
                        GroupPrivilages.DataValueField = "PrivilegeID";
                        GroupPrivilages.DataBind();
                        foreach (ListItem item in GroupPrivilages.Items)
                        {
                            foreach (TreeNode treeNode in treePRIVILAGES2.Nodes)
                            {
                                RecursiveUpdate(treeNode, item.Value);
                            }
                        }
                        treePRIVILAGES2.ExpandAll();
                        if (ddl_Groups.SelectedItem.Value == "1" || ddl_Groups.SelectedItem.Value == "2")
                        {
                            btn_Delete.Enabled = false;
                        }
                        else
                            btn_Delete.Enabled = true;
                    }
                }
            }


            void ManageVisiblity()
            {
                btn_Insert.Visible = true;
                btn_Update.Visible = false;
                btn_Delete.Visible = false;
                btn_Cancel.Visible = false;
            }


            void UpdateGroup()
            {
                btn_Insert.Visible = false;
                btn_Update.Visible = true;
                btn_Delete.Visible = true;
                btn_Cancel.Visible = true;
            }


            protected void btn_Cancel_Click(object sender, EventArgs e)
            {
                ManageVisiblity();
                txt_GroupName.Text = "";
                treePRIVILAGES2.Nodes.Clear();
                BindPrivilages((DataTable)ViewState["TPrivliges"], treePRIVILAGES2);
                treePRIVILAGES2.ExpandAll();
            }


            protected void btn_Update_Click(object sender, EventArgs e)
            {
                using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    int groupId = Convert.ToInt32(ddl_Groups.SelectedItem.Value);
                    var query = dataContext.GroupPrivileges.Where(gp => gp.GroupID == groupId);
                    /*foreach(GroupPrivilege tmpGp in query){
                    dataContext.GroupPrivileges.DeleteOnSubmit(tmpGp);
                    dataContext.SubmitChanges();
                }*/

                    dataContext.GroupPrivileges.DeleteAllOnSubmit(query);
                    dataContext.SubmitChanges();

                    InsertTreeItems(groupId);
                    var group = dataContext.Groups.First(grp => grp.ID == groupId);
                    group.GroupName = txt_GroupName.Text;
                    group.DataLevelID = Convert.ToInt32(ddl_DataLevel.SelectedValue);
                    dataContext.SubmitChanges();
                    Bind_ddl_Groups();

                    lbl_ProcessResult.Text = Tokens.ItemAdded;
                    lbl_ProcessResult.ForeColor = Color.Green;

                    treePRIVILAGES2.Nodes.Clear();
                    LoadPrivTable();
                    BindPrivilages((DataTable)ViewState["TPrivliges"], treePRIVILAGES2);
                    treePRIVILAGES2.ExpandAll();
                    Bind_ddl_Groups();
                    txt_GroupName.Text = "";
                }
            }


            void RecursiveUpdate(TreeNode treeNode, string PrivId)
            {
                if (treeNode.Value == PrivId)
                {
                    treeNode.Checked = true;
                    if (treeNode.ChildNodes.Count > 0)
                    {
                        foreach (TreeNode childNode in treeNode.ChildNodes)
                        {
                            RecursiveUpdate(childNode, childNode.Value);
                        }
                    }
                }
                else if (treeNode.ChildNodes.Count < 1)
                {
                    return;
                }
                else
                {
                    foreach (TreeNode childNode in treeNode.ChildNodes)
                    {
                        RecursiveUpdate(childNode, PrivId);
                    }
                }
            }


            protected void btn_Delete_Click(object sender, EventArgs e)
            {
                using (var DataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var UsersQuery = from usr in DataContext.Users
                                     where usr.Group.ID == Convert.ToInt32(ddl_Groups.SelectedItem.Value)
                                     select usr;
                    if (UsersQuery.Count() > 0)
                    {
                        lbl_ProcessResult.Text = Tokens.GroupErrorMsg;
                    }
                    else
                    {
                        int GroupId = Convert.ToInt32(ddl_Groups.SelectedItem.Value);
                        var Query = from GP in DataContext.GroupPrivileges
                                    where GP.GroupID == GroupId
                                    select GP;
                        foreach (GroupPrivilege tmpGP in Query)
                        {
                            DataContext.GroupPrivileges.DeleteOnSubmit(tmpGP);
                            DataContext.SubmitChanges();
                        }
                        var DeletedGroupQuery = from Grp in DataContext.Groups
                                                where Grp.ID == Convert.ToInt32(ddl_Groups.SelectedItem.Value)
                                                select Grp;
                        Group grp = DeletedGroupQuery.First();
                        DataContext.Groups.DeleteOnSubmit(grp);
                        DataContext.SubmitChanges();

                        txt_GroupName.Text = "";
                        treePRIVILAGES2.Nodes.Clear();
                        BindPrivilages((DataTable)ViewState["TPrivliges"], treePRIVILAGES2);
                        treePRIVILAGES2.ExpandAll();
                        Bind_ddl_Groups();
                        lbl_ProcessResult.Text = Tokens.GroupDeleted;
                        lbl_ProcessResult.ForeColor = Color.Green;
                    }
                }
            }


            //readonly ISPDataContext _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
            void InsertTreeItems(int groupID)
            {
                using (var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var selectedPrivs = new List<int>();


                    foreach (TreeNode node in treePRIVILAGES2.Nodes)
                    {
                        //   InsertNodeItems(node, groupID);
                        selectedPrivs.AddRange(GetPrivId(node, selectedPrivs));
                    }

                    var result = new List<int>();
                    foreach (var selectedPriv in selectedPrivs)
                    {
                        if (result.All(x => x != selectedPriv))
                        {
                            result.Add(selectedPriv);
                        }
                    }

                    if (result.Any())
                    {
                        var privList = result.Select(priv => new GroupPrivilege
                        {
                            GroupID = groupID,
                            PrivilegeID = priv
                        }).ToList();
                        _context.GroupPrivileges.InsertAllOnSubmit(privList);
                        _context.SubmitChanges();
                    }
                }
            }


            IEnumerable<int> GetPrivId(TreeNode node, ICollection<int> ids)
            {

                if (node.Checked)
                {
                    var priId = Convert.ToInt32(node.Value);
                    var exist = ids.All(x => x != priId);
                    if (exist)
                    {
                        ids.Add(priId);
                    }

                }

                if (node.ChildNodes.Count > 0)
                {
                    foreach (TreeNode treeNode in node.ChildNodes)
                    {
                        GetPrivId(treeNode, ids);
                    }
                }
                return ids;
            }

            void Bind_ddl_DataLevel()
            {
                using (var DataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    ddl_DataLevel.SelectedValue = null;
                    ddl_DataLevel.Items.Clear();

                    ddl_DataLevel.AppendDataBoundItems = true;
                    var Query = from datalevel in DataContext.DataLevels
                                select datalevel;
                    ddl_DataLevel.DataSource = Query;
                    ddl_DataLevel.DataBind();
                    Helper.AddDefaultItem(ddl_DataLevel);
                }
            }
        }
    }
 