using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using Db;
using NewIspNL.Models;
using Resources;

namespace NewIspNL.WebUserControls
{
    public partial class WebUserControls_PendingActivationl : UserControl{
        //readonly ISPDataContext _context;

        readonly Loc _loc;
        public int CustomersCount { get; set; }

        public WebUserControls_PendingActivationl(){
            _loc = new Loc();
            //_context = new ISPDataContext();
            CustomersCount = new int();
            Orders=new List<WorkOrder>();
        }

        public List<WorkOrder> Orders { get; set; }
        public List<RequestCountModel> PendingActivations { get; set; }
        public List<CountName> StatModels { get; set; }

        public bool Proceed { get; set; }


        protected void Page_Load(object sender, EventArgs e){
            if (this.Visible)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    if (IsPostBack) return;
                    //var context = new ISPDataContext();
                    Orders = context.WorkOrders.ToList();
                    CustomersCount = Orders.Count();
                    StatModels = new List<CountName>();
                    PendingActivations = new List<RequestCountModel>();
                    var userId = Convert.ToInt32(Session["User_ID"]);
                    var user = context.Users.FirstOrDefault(u => u.ID == userId);
                    if (user == null || user.GroupID == null || user.GroupID.Value != 1)
                    {
                        Proceed = false;
                        return;
                    }
                    Proceed = true;
                    var requests = context.Status.ToList();
                    var models = new List<RequestCountModel>();
                    foreach (var request in requests) CreatePendingItem(user, request, models);
                    PendingActivations = models;
                    FilterStutus(models);
                }
            }
        }

        void FilterStutus(List<RequestCountModel> models){
            foreach(int state in Enum.GetValues(typeof(StatusName))){
                switch(state){
                    case 0 :
                        StatModels.Add(new CountName{
                            Count = models.Where(x => x.Status.ID < 6).Sum(x => x.Count),
                            TName = _loc.IterateResource(Tokens.New)
                        });
                        break;
                    case 1 :
                        StatModels.Add(new CountName{
                            Count = models.Where(x => x.Status.ID == 6).Sum(x => x.Count),
                            TName = _loc.IterateResource(Tokens.Active)
                        });
                        break;
                    case 2 :
                        StatModels.Add(new CountName{
                            Count = models.Where(x => x.Status.ID == 11).Sum(x => x.Count),
                            TName = _loc.IterateResource(Tokens.Suspend)
                        });
                        break;
                    case 3 :
                        StatModels.Add(new CountName{
                            Count = models.Where(x => x.Status.ID == 8 || x.Status.ID == 9).Sum(x => x.Count),
                            TName = _loc.IterateResource(Tokens.MenuCancel)
                        });
                        break;
                    case 4 :
                        StatModels.Add(new CountName{
                            Count = models.Where(x => x.Status.ID == 10).Sum(x => x.Count),
                            TName = _loc.IterateResource(Tokens.MenuHold)
                        });
                        break;
                    case 5 :
                        StatModels.Add(new CountName{
                            Count = models.Where(x => x.Status.ID == 7).Sum(x => x.Count),
                            TName = _loc.IterateResource(Tokens.SystemProblem)
                        });
                        break;
                }
            }
        }


        static void CreatePendingItem(User user, Status request, ICollection<RequestCountModel> models){
            if(user.Group.DataLevelID == null) return;
            var count = EditControls.GetPendingActivationCount(user.ID, user.Group.DataLevelID.Value, 0, request.ID);
            models.Add(new RequestCountModel{
                Status = request,
                Count = count
            });
        }
    }
}
