using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using NewIspNL.Services.DemandServices;
using Resources;

namespace NewIspNL.Pages
{
    public partial class SearchOrdersNotes : CustomPage
    {
   
    readonly OrderHintService _hintService;

    //readonly ISPDataContext _context;
    public bool CanProcess { get; set; }
    public SearchOrdersNotes(){
        var _context = IspDataContext;
        _hintService = new OrderHintService(_context);
    }


    protected void Page_Load(object sender, EventArgs e){
        using(var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            Activate();
            var user = _context.Users.FirstOrDefault(u => u.ID == Convert.ToInt32(Session["User_ID"]));
            CanProcess = user != null && user.GroupID != null && user.GroupID != 6;
            if(IsPostBack) return;
            Search();
        }
    }


    void Search(){
        
        var orders = DataLevelClass.GetUserWorkOrder();
        var notes = _hintService.SearchModels(orders, false, Convert.ToInt32( Session["User_ID"]));
        GvResults.DataSource = notes;
        GvResults.DataBind();
    }


    protected void Process(int id){
        var done = _hintService.Process(id, true,TbComment.Text);
        Msg.InnerHtml = done ? Tokens.Saved : Tokens.SavingError;
        Search();
    }


    void Activate(){
        //BProcessNote.ServerClick += (o, e) => Process(Convert.ToInt32(selected.Value));
        GvResults.DataBound += (o, e) => Helper.GridViewNumbering(GvResults, "LNo");
    }

    protected void BProcessNote1_Click(object sender, EventArgs e)
    {
        Process(Convert.ToInt32(selected.Value));
        var s="";
    }
}
}