using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class DeleteAllCustomers : CustomPage
    {
       
            readonly IWorkOrderRepository _orderRepository;


            public  DeleteAllCustomers()
            {
                _orderRepository = new WorkOrderRepository();
            }


            protected void Page_Load(object sender, EventArgs e) { }


            protected void DeleteCustomers(object sender, EventArgs e)
            {
                var orders = _orderRepository.WorkOrders.ToList();
                var undeleted = (orders.Select(workOrder => new
                {
                    workOrder,
                    notdeleted = _orderRepository.Delete(workOrder.ID)
                }).Where(@t => @t.notdeleted).Select(@t => @t.workOrder)).ToList();
                if (undeleted.Any())
                {
                    GvUndeleted.DataSource = undeleted.Select(x => new
                    {
                        x.ID,
                        x.CustomerName,
                        x.Governorate.GovernorateName,
                    });
                    GvUndeleted.DataBind();
                }
                else
                {
                    msg.InnerHtml = Tokens.Deleted;
                }
            }


            protected void GvUndeleted_OnDataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(GvUndeleted, "l_number");
            }
        }
    }
 