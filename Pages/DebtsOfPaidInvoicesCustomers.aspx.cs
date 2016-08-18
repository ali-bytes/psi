using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class DebtsOfPaidInvoicesCustomers : CustomPage
    {
          readonly IUserSaveRepository _userSave;

            public  DebtsOfPaidInvoicesCustomers()
            {
                _userSave = new UserSaveRepository();
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                Populateunpaid();
                PopulateSvaes();
            }
            protected void GvUnpaid_OnDataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(GvUnpaid, "LNo");
            }
            void PopulateSvaes()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var userId = Convert.ToInt32(Session["User_ID"]);
                    ddlSavesPay.DataSource = _userSave.SavesOfUser(userId, context).Select(a => new
                    {
                        a.Save.SaveName,
                        a.Save.Id
                    });
                    ddlSavesPay.DataBind();
                    Helper.AddDefaultItem(ddlSavesPay);
                }
            }

            void Populateunpaid()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {

                    if (Session["User_ID"] == null) return;
                    var userId = Convert.ToInt32(Session["User_ID"]);
                    if (userId == 0) return;
                    var user = context.Users.FirstOrDefault(a => a.ID == userId);//_searchService.GetUser(userId);
                    if (user == null) return;
                    var debtsInvoices = context.DebtsInvoices.Where(s => s.paid == false).ToList();
                    var dataleve = user.Group.DataLevelID;
                    switch (dataleve)
                    {
                        case 1:
                            break;
                        case 2:
                            debtsInvoices =
                                debtsInvoices.Where(a => DataLevelClass.GetBranchAdminBranchIDs(user.ID).Contains(a.Demand.WorkOrder.BranchID))
                                    .ToList();
                            break;
                        case 3:
                            debtsInvoices = debtsInvoices.Where(a => a.Demand.WorkOrder.ResellerID.Equals(user.ID)).ToList();
                            break;
                    }
                    GvUnpaid.DataSource = debtsInvoices.Select(s => new
                    {
                        s.Id,
                        Customer = s.Demand.WorkOrder.CustomerName,
                        Phone = s.Demand.WorkOrder.CustomerPhone,
                        Offer = s.Demand.Offer != null ? s.Demand.Offer.Title : " ",
                        From = s.Demand.StartAt,
                        To = s.Demand.EndAt,
                        s.Demand.Notes,
                        s.Amount,
                        User = s.Demand.User.UserName,
                        s.Demand.PaymentComment,
                        ForMonth = s.Demand.StartAt.Month,
                        ForYear = s.Demand.StartAt.Year
                    }).ToList();
                    GvUnpaid.DataBind();
                }
            }


            protected void ConfirmPayDemand(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    //var button = sender as Button;
                    if (!string.IsNullOrEmpty(hdfId.Value))
                    {
                        var debtId = Convert.ToInt32(hdfId.Value);
                        var dept = context.DebtsInvoices.FirstOrDefault(x => x.Id == debtId);
                        if (dept != null)
                        {
                            dept.paid = true;
                            context.SubmitChanges();
                            var amount = Convert.ToDouble(dept.Amount);
                            var order = dept.Demand.WorkOrder;
                            //savestepsinsaves(amount, order);
                            var saveId = Convert.ToInt32(ddlSavesPay.SelectedItem.Value);
                            var userId = Convert.ToInt32(Session["User_ID"]);
                            //_userSave.UpdateSave(userId,saveId,amount,"سداد مديونية لفاتورة مدفوعة",order.CustomerName+" - "+order.CustomerPhone,context);
                            _userSave.BranchAndUserSaves(saveId, userId, amount, "سداد مديونية لفاتورة مدفوعة ",
                                order.CustomerName + " - " + order.CustomerPhone, context);
                        }
                        Populateunpaid();
                    }

                }
            }
           
        }
    }
 