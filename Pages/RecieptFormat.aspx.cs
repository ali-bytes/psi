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

namespace NewIspNL.Pages
{
    public partial class RecieptFormat : CustomPage
    {
     
    readonly RecieptCnfgRepository _cnfgRepository;

    //readonly ISPDataContext _context;

    readonly IspDomian _domian;

    public RecieptFormat(){
        var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        _cnfgRepository = new RecieptCnfgRepository(context);
        _domian=new IspDomian(context);
    }


    protected void Page_Load(object sender, EventArgs e){
        Activate();
        if(IsPostBack) return;
        _domian.PopulateBranches(DdlBranch,true);
        flag.Value = "0";
    }


    void Activate(){
        BSearch.ServerClick+=(o,e)=>PreparePage();
        BChangeBranch.ServerClick += (o, e) => flag.Value = "0";
    }


    void PreparePage(){
        PopulatePositions();
        GetCnfg();
    }


    void GetCnfg(){
        flag.Value = "1";
        var cfg = GetCurrentCfg();
        if(cfg == null){
            ImgLogo.Visible = false;
            HfId.Value = TbCaution.Text = Editor2.Content = TbCompanyName.Text = ImgLogo.ImageUrl = string.Empty;
            DdlPosition.SelectedIndex = -1;
            return;
        }
        ImgLogo.Visible = true;
        ImgLogo.ImageUrl = string.Format("{0}{1}", "~/PrintLogos/", cfg.LogoUrl);
        TbCompanyName.Text = cfg.CompanyName;
        DdlPosition.SelectedValue = string.Format("{0}", cfg.LogoPosiotion);
        Editor2.Content = cfg.ContactData;
        TbCaution.Text = cfg.Caution;
        HfId.Value = string.Format("{0}", cfg.Id);
    }


    ReceiptCnfg GetCurrentCfg(){
        var cfg = _cnfgRepository.GetBranchCnfg(Convert.ToInt32(DdlBranch.SelectedItem.Value));
        return cfg;
    }


    void PopulatePositions(){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var positions = context.LogoPositions;
            DdlPosition.DataSource = positions;
            DdlPosition.DataTextField = "Name";
            DdlPosition.DataValueField = "id";
            DdlPosition.DataBind();
            Helper.AddAllDefaultItem(DdlPosition);
        }
    }



    protected void BSave_OnServerClick(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            ReceiptCnfg cnfg = _cnfgRepository.GetBranchCnfg(Convert.ToInt32(DdlBranch.SelectedItem.Value)) ?? new ReceiptCnfg();

            var user = context.Users.FirstOrDefault(x => x.ID == Convert.ToInt32(Session["User_ID"]));
            if(user == null){
                Response.Redirect("~/Pages/default.aspx");
                return;
            }

      

            if(string.IsNullOrWhiteSpace(cnfg.LogoUrl)){
                if(!FLogo.HasFile){
                    LMsgerror.Visible = true;
                    
                    return;
                }
            }
            if(FLogo.HasFile){
                var path = Server.MapPath("~/PrintLogos/") + cnfg.LogoUrl;
                try{
                    File.Exists(path);
                    File.Delete(path);
                }
                catch(Exception){}
                 var extensions = new List<string> { ".JPG", ".GIF",".JPEG",".PNG" };
                 string ex = Path.GetExtension(FLogo.FileName);

                if (!string.IsNullOrEmpty(ex) && extensions.Any(currentExtention => currentExtention == ex.ToUpper()))
                {
                    string virtualName = DateTime.Now.AddHours().ToFileTime().ToString(CultureInfo.InvariantCulture);
                    var fileName = virtualName + ".jpeg";
                    FLogo.SaveAs(Server.MapPath("~/PrintLogos/") + fileName);
                    cnfg.LogoUrl = fileName;
                }
            }

            cnfg.CompanyName = TbCompanyName.Text;
            cnfg.Caution = TbCaution.Text;
            cnfg.ContactData = Editor2.Content;
            cnfg.LogoPosiotion = Convert.ToInt32(DdlPosition.SelectedItem.Value);
            cnfg.BranchId = Convert.ToInt32(DdlBranch.SelectedItem.Value);
            _cnfgRepository.Save(cnfg);
          
            PreparePage();
            LMsgSuccess.Visible = true;
            

        }
    }
}
}