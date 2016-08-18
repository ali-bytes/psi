using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class TransfaresFromSaves : CustomPage
    {
  
    private readonly IUserSaveRepository _userSave;

    public TransfaresFromSaves()
    {
        _userSave=new UserSaveRepository();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        ddlFromSave.DataSource = PopulateSaves();
        ddlFromSave.DataBind();
        Helper.AddDefaultItem(ddlFromSave); 
    }

    protected List<DdlData> PopulateSaves()
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var userId = Convert.ToInt32(Session["User_ID"]);
            var user = context.Users.FirstOrDefault(a => a.ID == userId);
            if (user != null)
            {
                var savesList = new List<Save>();
                switch (user.GroupID)
                {
                    case 1:
                        savesList = context.Saves.ToList();
                        break;
                    case 4:
                        savesList = context.Saves.Where(a => a.BranchId == user.BranchID).ToList();
                        break;

                }
                var data = new List<DdlData>();
                if (savesList.Count > 0)
                {
                    foreach (var save in savesList)
                    {
                        var d = new DdlData
                        {
                            SaveId = save.Id,
                            SaveName = save.SaveName
                        };
                        data.Add(d);
                    }
                    
                }
                else
                {
                    var l = _userSave.SavesOfUser(userId, context);
                    foreach (var item in l)
                    {
                        var d = new DdlData
                        {
                            SaveId = item.Save.Id,
                            SaveName = item.Save.SaveName
                        };
                        data.Add(d);
                    }
                }
                return data;
            }
            else
            {
                return null;
            }
        }
    }

    public void DdltoBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var id = Convert.ToInt32(ddlToSave.SelectedValue);

            Label2.Text = _context.Saves.Where(x => x.Id == id).Select(x => x.Total).FirstOrDefault().ToString();

        }
    }



    public void DdlFromBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {

            ddlToSave.DataSource = PopulateSaves().Where(a => a.SaveId != Helper.GetDropValue(ddlFromSave));
            ddlToSave.DataBind();
            Helper.AddDefaultItem(ddlToSave);
            var id = Convert.ToInt32(ddlFromSave.SelectedValue);

            Label1.Text = _context.Saves.Where(x => x.Id == id).Select(x => x.Total).FirstOrDefault().ToString();

        }
    }



    protected void BtnSave_Click(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            int userId = Convert.ToInt32(Session["User_ID"]);
            int fromid = Convert.ToInt32(ddlFromSave.SelectedItem.Value);
            var toid = Convert.ToInt32(ddlToSave.SelectedItem.Value);
            var fromAmount = Convert.ToDouble(TbAmount.Text)*-1;
            var notesFrom = " تحويل رصيد الى خزنة " + ddlToSave.SelectedItem;
            var notesTo = "تحويل رصيد من خزنة " + ddlFromSave.SelectedItem;
            var fromtotal = Convert.ToDouble(context.Saves.Where(z => z.Id == fromid).Select(z => z.Total).FirstOrDefault());
            if (fromAmount*-1 > fromtotal)
            {
                Message.Text = Tokens.Error+ " "+"رصيد الخزينة لا يكفي ";
            }
            else{
             _userSave.UpdateSave(userId, fromid, fromAmount, notesFrom, TbNotes.Text,context);
             _userSave.UpdateSave(userId,toid, Convert.ToDouble(TbAmount.Text), notesTo,TbNotes.Text,context);
            Message.Text = Tokens.Saved;
            Clear();
            Label1.Text = "";
            Label2.Text = ""; 
            
            }
        }
    }


    void Clear()
    {
        TbAmount.Text = TbNotes.Text = string.Empty;
        ddlToSave.SelectedValue = string.Empty;
    }
}
    public class DdlData
    {
        public string SaveName { get; set; }
        public int SaveId { get; set; }
    }
}