using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class AddSaves : CustomPage
    {
         
            protected void Page_Load(object sender, EventArgs e)
            {

                PopulateSaves();
                if (IsPostBack) return;
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var domain = new IspDomian(context);
                    domain.PopulateBranches(ddlBranches);
                }
            }

            protected void GvBox_OnDataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(GvSaves, "LNo");
            }

            protected void btnAdd_Click(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var check = context.Saves.Where(a => a.SaveName == txtSaveName.Text).ToList();
                    if (check.Count == 0)
                    {
                        var newSave = new Save
                        {
                            SaveName = txtSaveName.Text,
                            Total = Convert.ToDecimal(txtSaveCredit.Text),
                            BranchId = Convert.ToInt32(ddlBranches.SelectedItem.Value)
                        };
                        context.Saves.InsertOnSubmit(newSave);
                        context.SubmitChanges();
                          int userId = Convert.ToInt32(Session["User_ID"]);
                        var getsaveid =
                            context.Saves.Where(z => z.SaveName == txtSaveName.Text).Select(z => z).FirstOrDefault();
                        var savehistory = new UserSavesHistory
                        {
                            SaveId = getsaveid.Id,
                            amount = Convert.ToDecimal(txtSaveCredit.Text),
                            ConfirmerUserId = userId,
                            Time = DateTime.Now.AddHours(),
                            Notes = Tokens.firstamount

                            
                        };
                        context.UserSavesHistories.InsertOnSubmit(savehistory);
                        context.SubmitChanges();
                        Msgsuccess.Visible = true;
                        Clear();
                    }
                    else
                    {
                        lblerror.Text = Tokens.AlreadyExist;
                        MsgError.Visible = true;
                    }
                    PopulateSaves();
                }
            }

            protected void grd_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
            {
                GvSaves.EditIndex = -1;
                Clear();
                btnAdd.Visible = true;
                PopulateSaves();
            }

            private void Clear()
            {
                txtSaveName.Text = txtSaveCredit.Text = string.Empty;
                ddlBranches.SelectedIndex = -1;
            }

            private void PopulateSaves()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var saves = context.Saves.Select(a => new
                    {
                        a.Id,
                        a.SaveName,
                        a.Branch.BranchName,
                        a.Total
                    }).ToList();
                    GvSaves.DataSource = saves;
                    GvSaves.DataBind();
                }
            }

            protected void grd_RowDeleting(object sender, GridViewDeleteEventArgs e)
            {
                var datakey = GvSaves.DataKeys[e.RowIndex];
                if (datakey != null)
                {
                    var id = Convert.ToInt32(datakey["Id"]);
                    DeleteRow(id);

                }
            }

            protected void DeleteRow(int saveId)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var userSave = context.UserSaves.Where(a => a.SaveId == saveId).ToList();
                    var savehistory = context.UserSavesHistories.Where(a => a.SaveId == saveId).ToList();
                    if (userSave.Count == 0 && savehistory.Count == 0)
                    {
                        var save = context.Saves.FirstOrDefault(a => a.Id == saveId);
                        if (save != null) context.Saves.DeleteOnSubmit(save);
                        context.SubmitChanges();
                        Msgsuccess.Visible = true;
                    }
                    else
                    {
                        MsgError.Visible = true;
                        lblerror.Text = Tokens.CantDelete;
                    }
                    PopulateSaves();
                }
            }

            protected void grd_RowEditing(object sender, GridViewEditEventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {

                    GvSaves.EditIndex = e.NewEditIndex;
                    var dataKey = GvSaves.DataKeys[e.NewEditIndex];
                    if (dataKey != null)
                    {
                        var id = Convert.ToInt32(dataKey.Value);
                        var save = context.Saves.FirstOrDefault(a => a.Id == id);
                        if (save != null)
                        {
                            txtSaveName.Text = save.SaveName;
                            txtSaveCredit.Text = save.Total.ToString();
                            ddlBranches.SelectedValue = save.BranchId.ToString();
                        }
                    }
                }
                PopulateSaves();
                btnAdd.Visible = false;
            }
            protected void grd_RowUpdating(object sender, GridViewUpdateEventArgs e)
            {
                var dataKey = GvSaves.DataKeys[e.RowIndex];
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    if (dataKey != null)
                    {
                        int id = Convert.ToInt32(dataKey["Id"]);
                        var query = context.Saves.FirstOrDefault(item => item.Id == id);
                        if (query != null)
                        {
                            query.SaveName = txtSaveName.Text;
                            query.Total = Convert.ToDecimal(txtSaveCredit.Text);
                            query.BranchId = Convert.ToInt32(ddlBranches.SelectedItem.Value);
                        }
                    }
                    context.SubmitChanges();
                    Msgsuccess.Visible = true;
                }

                GvSaves.EditIndex = -1;
                Clear();
                btnAdd.Visible = true;
                PopulateSaves();
            }

        }
    }
