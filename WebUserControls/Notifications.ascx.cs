using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using Resources;
using Db;
using NewIspNL.Models;

namespace NewIspNL.WebUserControls
{
    public partial class Notifications : System.Web.UI.UserControl
    {
        
            public int TicketCount { get; set; }
            public int CustomersCount { get; set; }
            public int Datalevel { get; set; }
            public string Active { get; set; }
            public string New { get; set; }
            public string MenuHold { get; set; }
            public string MenuSuspend { get; set; }
            public string Cancel { get; set; }
            public string SystemProblem { get; set; }
            public string ActiveDecimal { get; set; }
            public string NewDecimal { get; set; }
            public string MenuholdDecimal { get; set; }
            public string MenuSuspendDecimal { get; set; }
            public string CancelDecimal { get; set; }
            public string SystemProblemDecimal { get; set; }
            readonly Loc _loc;

            public Notifications()
            {
                _loc = new Loc();
                CustomersCount = new int();
            }
            protected void Page_Load(object sender, EventArgs e)
            {
                //using (var context = new ISPDataContext())
                {
                    var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"].ToString(CultureInfo.InvariantCulture));
                    var user = context.Users.FirstOrDefault(u => u.ID == Convert.ToInt32(Session["User_ID"]));

                    var option = context.Options.FirstOrDefault();
                    if (option != null && user != null && user.GroupID != null)// == 1
                    {
                        Datalevel = Convert.ToInt32(user.Group.DataLevelID);
                        //sc1.Visible = PendingActivationlPanel.Visible = counters.Visible = option.ShowStatistic;
                        if (option.ShowStatistic)
                        {
                            //var orders = context.WorkOrders.ToList();context.WorkOrders.Count();//
                            CustomersCount = EditControls.CustomerCount(user.ID, Datalevel);
                            TicketCount = EditControls.GetTicketCount("ViewTickets.aspx?ts=mELirpUhRYksFj7k8/XBcQ==", user.ID, user.GroupID.Value);
                            var requests = context.Status.ToList();
                            var models = new List<RequestCountModel>();
                            if (user.Group.DataLevelID == null) return;
                            foreach (var request in requests) CreatePendingItem(user, request, models);
                            FilterStutus(models);
                        }

                    }

                }
            }
            void FilterStutus(List<RequestCountModel> models)
            {
                if (CustomersCount > 0)
                {
                    foreach (int state in Enum.GetValues(typeof(StatusName)))
                    {

                        switch (state)
                        {
                            case 0:
                                var a = models.Where(x => x.Status.ID < 6).Sum(x => x.Count);
                                New = _loc.IterateResource(Tokens.New) + " " + a;
                                NewDecimal = Convert.ToDouble((a * 100) / CustomersCount).ToString(CultureInfo.InvariantCulture);
                                break;
                            case 1:
                                var b = models.Where(x => x.Status.ID == 6).Sum(x => x.Count);
                                Active = _loc.IterateResource(Tokens.Active) + " " + b;

                                ActiveDecimal = Convert.ToDouble((b * 100) / CustomersCount).ToString(CultureInfo.InvariantCulture); //Helper.FixNumberFormat(
                                break;
                            case 2:
                                var c = models.Where(x => x.Status.ID == 11).Sum(x => x.Count);
                                MenuSuspend = _loc.IterateResource(Tokens.Suspend) + " " + c;

                                MenuSuspendDecimal = Convert.ToDouble((c * 100) / CustomersCount).ToString(CultureInfo.InvariantCulture);
                                break;
                            case 3:
                                var d = models.Where(x => x.Status.ID == 8 || x.Status.ID == 9).Sum(x => x.Count);
                                Cancel = _loc.IterateResource(Tokens.MenuCancel) + " " + d;

                                CancelDecimal = Convert.ToDouble((d * 100) / CustomersCount).ToString(CultureInfo.InvariantCulture);
                                break;
                            case 4:
                                var e = models.Where(x => x.Status.ID == 10).Sum(x => x.Count);
                                MenuHold = _loc.IterateResource(Tokens.MenuHold) + " " + e;

                                MenuholdDecimal = Convert.ToDouble((e * 100) / CustomersCount).ToString(CultureInfo.InvariantCulture);
                                break;
                            case 5:
                                var f = models.Where(x => x.Status.ID == 7).Sum(x => x.Count);
                                SystemProblem = _loc.IterateResource(Tokens.SystemProblem) + " " + f;

                                SystemProblemDecimal = Convert.ToDouble((f * 100) / CustomersCount).ToString(CultureInfo.InvariantCulture);
                                break;
                        }
                    }
                }
            }


            static void CreatePendingItem(User user, Status request, ICollection<RequestCountModel> models)
            {
                if (user.Group.DataLevelID == null) return;
                var count = EditControls.GetPendingActivationCount(user.ID, user.Group.DataLevelID.Value, 0, request.ID);
                models.Add(new RequestCountModel
                {
                    Status = request,
                    Count = count
                });
            }


        }
    }
 