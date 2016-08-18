using System;
using System.Configuration;
using System.Linq;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class TransfersFromBoxes : CustomPage
    {
        

    readonly IspDomian _domian;

    //readonly ISPDataContext _context;
    readonly IBoxCreditRepository _creditRepository = new BoxCreditRepository();

    public TransfersFromBoxes()
    {
        _domian=new IspDomian(IspDataContext);

    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if(IsPostBack)return;
        _domian.PopulateBoxes(DdlFromBox);
    }



    public void DdltoBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var id = Convert.ToInt32(DdlToBox.SelectedValue);
            var firstOrDefault = _context.BoxCredits.OrderByDescending(a => a.ID).FirstOrDefault(a => a.BoxId == id);
            if (firstOrDefault != null)
                Label2.Text =
                    firstOrDefault.Net.ToString();
        }
    }

    public void DdlFromBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var Tobox = _context.Boxes.Where(a => a.ID != Helper.GetDropValue(DdlFromBox)).ToList();
            if (Tobox.Count != 0)
            {
                DdlToBox.DataSource = Tobox;
                DdlToBox.DataTextField = "BoxName";
                DdlToBox.DataValueField = "ID";
                DdlToBox.DataBind();
                Helper.AddDefaultItem(DdlToBox);
                var id = Convert.ToInt32(DdlFromBox.SelectedValue);
                var firstOrDefault = _context.BoxCredits.OrderByDescending(a => a.ID).FirstOrDefault(a => a.BoxId == id);
                if (firstOrDefault != null)
                    Label1.Text =
                        firstOrDefault.Net.ToString();
            }

        }
    }




    protected void BtnSave_Click(object sender, EventArgs e){
        int userId = Convert.ToInt32(Session["User_ID"]);
        int fromid = Convert.ToInt32(DdlFromBox.SelectedItem.Value);
        var toid = Convert.ToInt32(DdlToBox.SelectedItem.Value);
        var fromAmount =Convert.ToDecimal(TbAmount.Text) * -1;
        var notes = " تحويل رصيد الي صندوق " +"'"+ DdlToBox.SelectedItem+"'"+"- "+ TbNotes.Text;
        var notes2 = " تحويل رصيد من صندوق " + "'" + DdlFromBox.SelectedItem + "'" + "- " + TbNotes.Text;
        var fromresulte = _creditRepository.SaveBox(fromid, userId, fromAmount,notes, DateTime.Now.AddHours());
        var Toresulte = _creditRepository.SaveBox(toid, userId, Convert.ToDecimal(TbAmount.Text), notes2, DateTime.Now.AddHours());
        switch (fromresulte)
        {
            case SaveBoxResult.Saved:
                if(Toresulte != SaveBoxResult.Saved)return;
                Message.Text = Tokens.Saved;
                Clear();
                _domian.PopulateBoxes(DdlFromBox);
                break;
            case SaveBoxResult.NoCredit:
                Message.Text = Tokens.NotEnoughtCreditMsg;
                break;
        }
        Label1.Text = "";
        Label2.Text = "";
    }


    void Clear(){
        TbAmount.Text = TbNotes.Text = string.Empty;
        DdlToBox.SelectedValue = string.Empty;
    }
}
}