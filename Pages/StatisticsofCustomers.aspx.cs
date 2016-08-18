using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain.Abstract;
using Resources;

namespace NewIspNL.Pages
{
    public partial class StatisticsofCustomers : CustomPage
    {


    private readonly IRequestNotifiy _requestNotifiy;

    public StatisticsofCustomers()
    {
        _requestNotifiy=new RequestNotifiy();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        PopulateList();

    }

    void PopulateList()
    {
        var localizer = new Loc();
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var userId = Convert.ToInt32(Session["User_ID"]);
                        var user = context.Users.FirstOrDefault(usr => usr.ID == userId);
            if (user != null)
            {
                var id = user.GroupID;
                if (id != null)
                {
                    int groupId = id.Value;
                    var privilages =
                        context.GroupPrivileges.Where(
                            a => a.GroupID == groupId && a.privilege.ParentID == 13 && a.privilege.ISLinked.Value && a.privilege.Url.Contains("Reports.aspx")).Select(a => new StatisticsRequest
                            {
                                Id = a.ID,
                                Url = a.privilege.Url,
                                Name= localizer.IterateResource(a.privilege.LinkedName),
                                Count= EditControls.GetStateWoCount(a.privilege.Url,userId,groupId),
                            }).ToList();
                   
                    StatusList.DataSource = privilages;
                    StatusList.DataBind();
                }
            }
         
        }
    }

}
    public class StatisticsRequest
    {
        public int Id { set; get; }
        public string Url { set; get; }
        public string Name { set; get; }
        public int Count { set; get; }
    }
}