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
    public partial class Transfers : CustomPage
    {
        private readonly IUserSaveRepository _userSave;
        readonly IspDomian _domian;
        readonly IBoxCreditRepository _creditRepository = new BoxCreditRepository();

        public Transfers()
        {
            _userSave = new UserSaveRepository();
            _domian = new IspDomian(IspDataContext);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
          
            if(!IsPostBack)
            { 
            PopulateSaves2();
            ddlFromSave.DataBind();
            Helper.AddDefaultItem(ddlFromSave);
            Helper.AddDefaultItem(ddlToSave); }
        }

        public void PopulateSaves2()
        {
            List<string> add = new List<string> {Tokens.__Chose__,Tokens.sav, Tokens.Boxs};
            fromdrop.DataSource = add;
            fromdrop.DataBind();
            todrop.DataSource = add;
            todrop.DataBind();

        }

        public void fromchange(object sender, EventArgs eventArgs)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (fromdrop.SelectedItem.ToString() == Tokens.Boxs)
                {
                 
                    ddlFromSave.DataSource = context.Boxes;
                    ddlFromSave.DataTextField = "BoxName";
                    ddlFromSave.DataValueField = "ID";
                    ddlFromSave.DataBind();
                    Helper.AddDefaultItem(ddlFromSave);
                }
                else if (fromdrop.SelectedItem.ToString() == Tokens.sav)
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
                        ddlFromSave.DataSource = data;
                        ddlFromSave.DataTextField = "SaveName";
                        ddlFromSave.DataValueField = "SaveId";
                       
                        ddlFromSave.DataBind();
                        Helper.AddDefaultItem(ddlFromSave);
                    }
                    
 }
                    else
                    {
                        return;
                    }
               
            }
        }



        public void tochange(object sender, EventArgs eventArgs)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (todrop.SelectedItem.ToString() == Tokens.Boxs && fromdrop.SelectedItem.ToString() == Tokens.Boxs)
                {
                    var Tobox = context.Boxes.Where(a => a.ID != Helper.GetDropValue(ddlFromSave)).ToList();
                    if (Tobox.Count != 0)
                    {
                        ddlToSave.DataSource = Tobox;
                        ddlToSave.DataTextField = "BoxName";
                        ddlToSave.DataValueField = "ID";
                        ddlToSave.DataBind();
                        Helper.AddDefaultItem(ddlToSave);
                    }
                }
                else if (todrop.SelectedItem.ToString() == Tokens.sav && fromdrop.SelectedItem.ToString() == Tokens.sav)
                {

                   ddlToSave.DataSource = PopulateSaves().Where(a => a.SaveId != Helper.GetDropValue(ddlFromSave));
                   ddlToSave.DataTextField = "SaveName";
                   ddlToSave.DataValueField = "SaveId";
        ddlToSave.DataBind();
        Helper.AddDefaultItem(ddlToSave);
                       
                    

                }
                else if (todrop.SelectedItem.ToString() == Tokens.Boxs)
                {
                    var Tobox = context.Boxes.ToList();
                    if (Tobox.Count != 0)
                    {
                        ddlToSave.DataSource = Tobox;
                        ddlToSave.DataTextField = "BoxName";
                        ddlToSave.DataValueField = "ID";
                        ddlToSave.DataBind();
                        Helper.AddDefaultItem(ddlToSave);
                    }
                }
                else if (todrop.SelectedItem.ToString() == Tokens.sav)
                {
                    ddlToSave.DataSource = PopulateSaves();
                    ddlToSave.DataTextField = "SaveName";
                    ddlToSave.DataValueField = "SaveId";
                    ddlToSave.DataBind();
                    Helper.AddDefaultItem(ddlToSave);
                }
            }
        }


        public void FromValue(object sender, EventArgs eventArgs)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (fromdrop.SelectedItem.ToString() == Tokens.sav)
                {
                    var firstOrDefault = context.Saves.FirstOrDefault(x => x.Id == Helper.GetDropValue(ddlFromSave));
                    if (firstOrDefault != null)
                        Label1.Text = firstOrDefault.Total.ToString();
                }
                else if (fromdrop.SelectedItem.ToString() == Tokens.Boxs)
                {
                    var allboxes = context.Boxes.Where(x => x.ID == Helper.GetDropValue(ddlFromSave)).Select(x => new
                    {
                        x.ID,

                        BoxNet = x.BoxCredits.OrderByDescending(a => a.ID).FirstOrDefault(a => a.BoxId == x.ID).Net
                    }).FirstOrDefault();
                    if (allboxes != null) Label1.Text = allboxes.BoxNet.ToString();
                }
            }
        }
        public void toValue(object sender, EventArgs eventArgs)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (todrop.SelectedItem.ToString() == Tokens.sav)
                {
                    var firstOrDefault = context.Saves.FirstOrDefault(x => x.Id == Helper.GetDropValue(ddlToSave));
                    if (firstOrDefault != null)
                        Label2.Text = firstOrDefault.Total.ToString();
                }
                else if (todrop.SelectedItem.ToString() == Tokens.Boxs)
                {
                    var allboxes = context.Boxes.Where(x => x.ID == Helper.GetDropValue(ddlToSave)).Select(x => new
                    {
                        x.ID,
                       
                        BoxNet = x.BoxCredits.OrderByDescending(a => a.ID).FirstOrDefault(a => a.BoxId == x.ID).Net
                    }).FirstOrDefault();
                    if (allboxes != null) Label2.Text = allboxes.BoxNet.ToString();
                }
            }
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

        void Clear()
        {
            TbAmount.Text = TbNotes.Text = string.Empty;
            ddlToSave.SelectedValue = string.Empty;
        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var allboxes = context.Boxes.Where(x => x.ID == Helper.GetDropValue(ddlFromSave)).Select(x => new
                {
                    x.ID,

                    BoxNet = x.BoxCredits.OrderByDescending(a => a.ID).FirstOrDefault(a => a.BoxId == x.ID).Net
                }).FirstOrDefault();
                if (fromdrop.SelectedItem.ToString() == Tokens.sav && todrop.SelectedItem.ToString() == Tokens.sav)
                {
                    int userId = Convert.ToInt32(Session["User_ID"]);
                    int fromid = Convert.ToInt32(ddlFromSave.SelectedItem.Value);
                    var toid = Convert.ToInt32(ddlToSave.SelectedItem.Value);
                    if (fromid == toid)
                    {
                        Message.Text = "لا يمكن التحويل لنفس الخزنة";
                        return;

                    }
                    var fromAmount = Convert.ToDouble(TbAmount.Text) * -1;
                    var notesFrom = " تحويل رصيد الى خزنة " + ddlToSave.SelectedItem;
                    var notesTo = "تحويل رصيد من خزنة " + ddlFromSave.SelectedItem;
                    var fromtotal = Convert.ToDouble(context.Saves.Where(z => z.Id == fromid).Select(z => z.Total).FirstOrDefault());
                    if (fromAmount * -1 > fromtotal)
                    {
                        Message.Text = Tokens.Error + " " + "رصيد الخزينة لا يكفي ";
                    }
                    else
                    {
                        _userSave.UpdateSave(userId, fromid, fromAmount, notesFrom, TbNotes.Text, context);
                        _userSave.UpdateSave(userId, toid, Convert.ToDouble(TbAmount.Text), notesTo, TbNotes.Text, context);
                        Message.Text = Tokens.Saved;
                        Clear();
                    }
                }
                else if (fromdrop.SelectedItem.ToString() == Tokens.sav && todrop.SelectedItem.ToString() == Tokens.Boxs)
                {
                    int userId = Convert.ToInt32(Session["User_ID"]);
                    int fromid = Convert.ToInt32(ddlFromSave.SelectedItem.Value);
                    var toid = Convert.ToInt32(ddlToSave.SelectedItem.Value);
                    var fromAmount = Convert.ToDouble(TbAmount.Text) * -1;
                    var notesFrom = " تحويل رصيد الى صندوق " + ddlToSave.SelectedItem;
                    var notesTo = "تحويل رصيد من خزنة " + ddlFromSave.SelectedItem;
                    var fromtotal = Convert.ToDouble(context.Saves.Where(z => z.Id == fromid).Select(z => z.Total).FirstOrDefault());
                    if (fromAmount * -1 > fromtotal)
                    {
                        Message.Text = Tokens.Error + " " + "رصيد الخزينة لا يكفي ";
                    }
                    else
                    {
                        _userSave.UpdateSave(userId, fromid, fromAmount, notesFrom, TbNotes.Text, context);
                        _creditRepository.SaveBox(toid, userId, Convert.ToDecimal(TbAmount.Text), notesTo, DateTime.Now.AddHours());
                        Message.Text = Tokens.Saved;
                        Clear();
                    }
                }
                else if (fromdrop.SelectedItem.ToString() ==  Tokens.Boxs && todrop.SelectedItem.ToString() ==  Tokens.Boxs)
                {
                    int userId = Convert.ToInt32(Session["User_ID"]);
                    int fromid = Convert.ToInt32(ddlFromSave.SelectedItem.Value);
                    var toid = Convert.ToInt32(ddlToSave.SelectedItem.Value);
                    if (fromid == toid)
                    {
                        Message.Text = "لا يمكن التحويل لنفس الصندوق";
                        return;

                    }
                    var fromAmount = Convert.ToDecimal(TbAmount.Text) * -1;
                    var notes = " تحويل رصيد الي صندوق " + "'" + ddlToSave.SelectedItem + "'" + "- " + TbNotes.Text;
                    var notes2 = " تحويل رصيد من صندوق " + "'" + ddlFromSave.SelectedItem + "'" + "- " + TbNotes.Text;
                    var fromresulte = _creditRepository.SaveBox(fromid, userId, fromAmount, notes, DateTime.Now.AddHours());
                    var Toresulte = _creditRepository.SaveBox(toid, userId, Convert.ToDecimal(TbAmount.Text), notes2, DateTime.Now.AddHours());

                
                    if (allboxes.BoxNet < Convert.ToDecimal(TbAmount.Text))
                    {

                        Message.Text = Tokens.Error + " " + "رصيد الصندوق لا يكفي ";
                    }
                    else
                    {
                        switch (fromresulte)
                        {
                            case SaveBoxResult.Saved:
                                if (Toresulte != SaveBoxResult.Saved) return;
                                Message.Text = Tokens.Saved;
                                Clear();
                                _domian.PopulateBoxes(ddlFromSave);
                                break;
                            case SaveBoxResult.NoCredit:
                                Message.Text = Tokens.NotEnoughtCreditMsg;
                                break;
                        }
                    }
                }
                else if (fromdrop.SelectedItem.ToString() == Tokens.Boxs && todrop.SelectedItem.ToString() == Tokens.sav)
                {
                   

                int userId = Convert.ToInt32(Session["User_ID"]);
                int fromid = Convert.ToInt32(ddlFromSave.SelectedItem.Value);
                var toid = Convert.ToInt32(ddlToSave.SelectedItem.Value);
              
                var fromAmount = Convert.ToDecimal(TbAmount.Text) * -1;
                var notes = " تحويل رصيد الي خزينة " + "'" + ddlToSave.SelectedItem + "'" + "- " + TbNotes.Text;
                var notes2 = " تحويل رصيد من صندوق " + "'" + ddlFromSave.SelectedItem + "'" + "- " + TbNotes.Text;
                var fromresulte = _creditRepository.SaveBox(fromid, userId, fromAmount, notes, DateTime.Now.AddHours());
                _userSave.UpdateSave(userId, toid, Convert.ToDouble(TbAmount.Text) , notes2, TbNotes.Text, context);
                    if (allboxes.BoxNet < Convert.ToDecimal(TbAmount.Text))
                    {

                        Message.Text = Tokens.Error + " " + "رصيد الصندوق لا يكفي ";
                    }
                    else
                    {
                        switch (fromresulte)
                        {
                            case SaveBoxResult.Saved:

                                Message.Text = Tokens.Saved;
                                Clear();
                                _domian.PopulateBoxes(ddlFromSave);
                                break;
                            case SaveBoxResult.NoCredit:
                                Message.Text = Tokens.NotEnoughtCreditMsg;
                                break;
                        }
                    }
                }

                Label1.Text = "";
                Label2.Text = "";
            }
        }

    }
}