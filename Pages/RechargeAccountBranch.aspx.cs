using System;
using System.Collections.Generic;
using System.Configuration;
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
    public partial class RechargeAccountBranch : CustomPage
    {
    
    readonly IspDomian _domian;

  


    public RechargeAccountBranch(){
        _domian = new IspDomian(IspDataContext);
      
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if(IsPostBack)return;
        _domian.PopulateBranches(DdlBranch, true);
      
        PopulateBoxes();
       
    }


    void PopulateBoxes(){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            DdlBoxes.DataSource = context.Boxes.Where(a => a.ShowBox == true).ToList();
            DdlBoxes.DataTextField = "BoxName";
            DdlBoxes.DataValueField = "ID";
            DdlBoxes.DataBind();
            Helper.AddDefaultItem(DdlBoxes);
        }
    }


    protected void BtnSave_Click(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            if(string.IsNullOrWhiteSpace(FUReciept.FileName)){
                Message.Text = Tokens.AttachFile;
                return;
            }
            string fileName = FUReciept.FileName;
            var extensions = new List<string> { ".JPG", ".GIF",".JPEG",".PNG" };
            string ex = Path.GetExtension(FUReciept.FileName);

            if (!string.IsNullOrEmpty(ex) && extensions.Any(currentExtention => currentExtention == ex.ToUpper()))
            {
                string virtualName = DateTime.Now.AddHours().ToFileTime().ToString(CultureInfo.InvariantCulture);
                string filename = virtualName + ".jpeg";
                //string filename = DateTime.Now.Millisecond + FUReciept.FileName;
                FUReciept.SaveAs(Server.MapPath(string.Format("../Attachments/{0}", filename)));
                var direct = RblEffect.SelectedIndex;

                context.RechargeRequestBranches.InsertOnSubmit(new RechargeRequestBranch()
                {
                    BranchId = Helper.GetDropValue(DdlBranch),
                    BoxId = Helper.GetDropValue(DdlBoxes),
                    DepositorName = TbDepositor.Text,
                    Amount = Convert.ToDecimal(TbAmount.Text),
                    Time = DateTime.Now.AddHours(),
                    RecieptImage = filename,
                    CreditOrVoice = direct
                });
                context.SubmitChanges();
                Message.Text = Tokens.Saved;
                clear();
            }

           
        }
    }


    void clear(){
        TbAmount.Text = TbDepositor.Text = string.Empty;
        DdlBoxes.SelectedValue = DdlBranch.SelectedValue = null;
    }
}
}