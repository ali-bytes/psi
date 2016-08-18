using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
    public partial class RechargeAccountReseller : CustomPage
    {
     
    readonly IspDomian _domian;

    readonly ISPDataContext _context;


    public RechargeAccountReseller(){
        _domian = new IspDomian(IspDataContext);
        _context = IspDataContext;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if(IsPostBack)return;
        _domian.PopulateResellers(DdlReseller, true);
       
        PopulateBoxes();
        var textOfRechargeAccount = _context.TextOfRechargeAccounts.FirstOrDefault();
        if(textOfRechargeAccount != null) lblTextOfRechargeAccount.Text = textOfRechargeAccount.Text;
    }


    void PopulateBoxes(){
        DdlBoxes.DataSource = _context.Boxes.Where(a => a.ShowBox == true).ToList();
        DdlBoxes.DataTextField = "BoxName";
        DdlBoxes.DataValueField = "ID";
        DdlBoxes.DataBind();
        Helper.AddDefaultItem(DdlBoxes);
    }


    protected void BtnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FUReciept.FileName))
        {
            Message.Text = Tokens.AttachFile;
            return;
        }

         var extensions = new List<string> { ".JPG", ".GIF",".JPEG",".PNG" };
            string ex = Path.GetExtension(FUReciept.FileName);

        if (!string.IsNullOrEmpty(ex) && extensions.Any(currentExtention => currentExtention == ex.ToUpper()))
        {
            string virtualName = DateTime.Now.AddHours().ToFileTime().ToString(CultureInfo.InvariantCulture);
            string filename = virtualName + ".jpeg";
            //string filename = DateTime.Now.Millisecond + Path.ChangeExtension(FUReciept.FileName, ".jpeg"); 
            FUReciept.SaveAs(Server.MapPath(string.Format("../Attachments/{0}", filename)));
            var direct = RblEffect.SelectedIndex;

            _context.RechargeRequests.InsertOnSubmit(new RechargeRequest()
            {
                ResellerId = Helper.GetDropValue(DdlReseller),
                BoxId = Helper.GetDropValue(DdlBoxes),
                DepositorName = TbDepositor.Text,
                Amount = Convert.ToDecimal(TbAmount.Text),
                Time = DateTime.Now.AddHours(),
                RecieptImage = filename,
                CreditORVoice = direct
            });
            _context.SubmitChanges();
            Message.Text = Tokens.Saved;
            clear();

            System.Threading.Thread.Sleep(3000);
        }
       
        

    }

    
    void clear(){
        TbAmount.Text = TbDepositor.Text = string.Empty;
        DdlBoxes.SelectedValue = DdlReseller.SelectedValue = null;
    }
}
}