using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class CustomerReportRouter : CustomPage
    {
       
            protected void Page_Load(object sender, EventArgs e)
            {

            }

            protected void btnSearch_click(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    //var orderRepository = new IspEntries();
                    var orderList =
                        DataLevelClass.GetUserWorkOrder(context)
                            .Where(a => a.OfferId != null && a.Offer.withRouter);
                    /*var orderList = orderRepository.GetWorkOrderOrdersByStatusId(6).Where(x=>x.OfferId!=null&&x.Offer.withRouter);*/
                    var newOrderList = new List<WorkOrder>();
                    var index = RblSearch.SelectedIndex;
                    switch (index)
                    {
                        case 0:
                            foreach (var order in orderList)
                            {

                                var orderexist = CheckIfExist(order.ID, context);
                                if (orderexist == null || orderexist.CompanyConfirmerUserId == null || orderexist.CompanyProcessDate == null)
                                {
                                    newOrderList.Add(order);
                                }
                            }
                            break;
                        /* case 1:
                             foreach (var order in orderList)
                             {
                                 var orderexist = CheckIfExist(order.ID, context);
                                 if (orderexist != null && !Convert.ToBoolean(orderexist.IsRecieved) &&
                                     orderexist.CustomerProcessDate == null)
                                 {
                                     newOrderList.Add(order);
                                 }
                             }
                             break;*/
                        case 1:
                            foreach (var order in orderList)
                            {
                                var orderExist = CheckIfExistrec(order.ID, context);
                                if (orderExist != null && Convert.ToBoolean(orderExist.IsRecieved) &&
                                    orderExist.CustomerProcessDate != null)
                                {
                                    newOrderList.Add(order);
                                }
                            }
                            break;

                    }
                    var data = newOrderList.Select(a => new
                    {
                        a.CustomerName,
                        a.Governorate.GovernorateName,
                        a.CustomerPhone,
                        a.RequestNumber,
                        CentralName = a.Central.Name,
                        a.ServicePackage.ServicePackageName,
                        a.ServiceProvider.SPName,
                        a.Status.StatusName,
                        a.Branch.BranchName,
                        Reseller = a.User != null ? a.User.UserName : "_",
                        a.CreationDate,
                        a.Offer.Title,
                        a.RequestDate,
                        a.ID,
                    }).ToList();
                    grd_wo.DataSource = data;
                    grd_wo.DataBind();
                }
            }

            protected RecieveRouter CheckIfExist(int woid, ISPDataContext context)
            {
                var exist = context.RecieveRouters.FirstOrDefault(a => a.WorkOrderIdRecive == woid);
                return exist;
            }
            protected RecieveRouter CheckIfExistrec(int woid, ISPDataContext context)
            {
                var exist = context.RecieveRouters.FirstOrDefault(a => a.WorkOrderId == woid);
                return exist;
            }

            protected void grd_wo_DataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(grd_wo, "l_number");
            }

        }
    }
