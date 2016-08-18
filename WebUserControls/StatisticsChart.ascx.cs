using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using Db;
using NewIspNL.Helpers;
using NewIspNL.Models;
using NewIspNL.Services;
using Resources;

namespace NewIspNL.WebUserControls
{
    public partial class WebUserControls_StatisticsChart : UserControl{
        //readonly WorkOrderService _orderService;


        public WebUserControls_StatisticsChart(){
            //_orderService = new WorkOrderService(new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]));
            ContanierModels = new List<ReportItemsContanierModel>();
        }
        public int CustomersCount { get; set; }

        public List<WorkOrder> Orders { get; set; }

        //public bool Proceed { get; set; }

        public List<ReportItemsContanierModel> ContanierModels { get; set; }

        protected void Page_Load(object sender, EventArgs e){
            if (this.Visible)
            {
                CustomersCount = 0;
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var userId = Convert.ToInt32(Session["User_ID"]);
                    var user = context.Users.FirstOrDefault(x => x.ID == userId);
                    if (user == null || user.GroupID == null)
                    {
                        //Proceed = false;
                        return;
                    }

                    if (user.Group.DataLevelID == null) return;
                    //Proceed = user.GroupID.Value == 1; 
                    var levelId = user.Group.DataLevelID.Value;
                    CustomersCount = EditControls.CustomerCount(user.ID, levelId);
               
                    var loc = new Loc();
                    var months = PrepareMonths(loc);
                    ContanierModels = new List<ReportItemsContanierModel>();
                    var today = DateTime.Now.AddHours();
                    foreach (int state in Enum.GetValues(typeof (StatusName)))
                    {
                        if (state != 6 && state > 3) continue;
                        var statusIds = new List<int>();
                        string tStateName = string.Empty;
                        string stateName = string.Empty;
                        statusIds = CreateStates(state, statusIds, loc, ref tStateName, ref stateName);
                        MakeFinalReportItem(state, tStateName, today, months, statusIds, userId, levelId, ContanierModels,
                            stateName,context);
                    }
                }
            }
        }


        static List<int> CreateStates(int state, List<int> statusIds, Loc _loc, ref string tStateName, ref string stateName){
            switch(state){
                case 0 :
                    statusIds = new List<int>{
                        1,
                        2,
                        3,
                        4,
                        5
                    };
                    stateName = Tokens.New;
                    tStateName = _loc.IterateResource(Tokens.New);
                    break;
                case 1 :
                    statusIds = new List<int>{
                        6
                    };
                    stateName = Tokens.Active;
                    tStateName = _loc.IterateResource(Tokens.Active);
                    break;
                case 2 :
                    statusIds = new List<int>{
                        11
                    };
                    stateName = Tokens.Suspend;
                    tStateName = _loc.IterateResource(Tokens.Suspend);
                    break;
                case 3 :
                    statusIds = new List<int>{
                        8,
                        9
                    };
                    stateName = Tokens.Cancel;
                    tStateName = _loc.IterateResource(Tokens.Cancel);
                    break;

                case 6 :
                    statusIds = new List<int>{
                        0
                    };
                    stateName = Tokens.IsNew;
                    tStateName = _loc.IterateResource(Tokens.IsNew);
                    break;
            }
            return statusIds;
        }


        void MakeFinalReportItem(int state, string tStateName, DateTime today, List<MonthModel> months, List<int> statusIds, int userId, int levelId, List<ReportItemsContanierModel> models, string orginalStateName,ISPDataContext context){
            var stateId = state + 1;
            string stateName = tStateName;
            var item = CreateReportItem(stateId, stateName);
            HandleItemData(today, item, months, statusIds, userId, levelId,context);
            item.Name = orginalStateName;
            models.Add(item);
        }


        public void HandleItemData(DateTime today, ReportItemsContanierModel item, List<MonthModel> months, List<int> statusIds, int userId, int levelId,ISPDataContext context){
            var orderService=new WorkOrderService(context);
            for(int i = 0;i < 6;i++){
                var start = today.AddMonths(i * -1);
                var monthStart = new DateTime(start.Year, start.Month, 1);
                var premonth = monthStart.AddMonths(-1);
                var premonthEnd = new DateTime(premonth.Year, premonth.Month, DateTime.DaysInMonth(premonth.Year, premonth.Month));
                item.Names.Add(months.First(x => x.Position == premonth.Month).Name);
                int accumulatedCount = 0;
                foreach(var statusId in statusIds){
                    accumulatedCount += orderService.CustomerInStateCount(premonth, premonthEnd, statusId, Orders,userId,levelId);
                }
                item.Values.Add(accumulatedCount);
            }
        }


        static ReportItemsContanierModel CreateReportItem(int stateId, string stateName){
            return new ReportItemsContanierModel{
                Name = "New",
                Id = stateId,
                TName = stateName,
                Names = new List<string>(),
                Values = new List<int>()
            };
        }


        static List<MonthModel> PrepareMonths(Loc loc){
            var months = new List<MonthModel>();
            for(int i = 1;i <= 12;i++){
                var localizedName = loc.IterateResource("M" + i);
                months.Add(new MonthModel{
                    Name = localizedName,
                    Position = i
                });
            }
            return months;
        }
    }
}
