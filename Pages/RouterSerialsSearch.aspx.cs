using System;
using System.Collections.Generic;

using NewIspNL.Domain.SearchService;
using NewIspNL.Models;

namespace NewIspNL.Pages
{
    public partial class RouterSerialsSearch : CustomPage
    {
      
    readonly ISearchEngine _searchEngine;


    public RouterSerialsSearch(){
        _searchEngine = new SearchEngine(IspDataContext);
    }


    public bool CanEdit { get; set; }
    public int GroupId { get; set; }


    public List<CustomerResult> Results { get; set; }


    protected void Page_Load(object sender, EventArgs e){
        var userId = Convert.ToInt32(Session["User_ID"]);
        var canEditModel = _searchEngine.EditCustomer(userId);
        CanEdit = canEditModel.CanEdit;
        GroupId = canEditModel.GroupId;
    }



    protected void BSearch_OnClick(object sender, EventArgs e){
        Search();
    }


    void Search(){
        Results = _searchEngine.SearchByRouterSerial(TbName.Text);
    }
}
}