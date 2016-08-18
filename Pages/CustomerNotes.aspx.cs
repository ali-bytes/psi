using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.SearchService;
using NewIspNL.Helpers;
using NewIspNL.Services.DemandServices;
using Resources;

namespace NewIspNL.Pages
{
    public partial class CustomerNotes : CustomPage
    {
       
            readonly ISPDataContext _context;

            readonly IspDomian _domian;

            readonly OrderHintService _hintService;


            public  CustomerNotes()
            {
                _context = IspDataContext;
                _domian = new IspDomian(_context);
                _hintService = new OrderHintService(_context);
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                Activate();
                if (IsPostBack) return;
                _domian.PopulateUsersWithOutResellers(DdlUsers);
            }


            void Save()
            {
                var orderId = Convert.ToInt32(selectedOrderId.Value);
                var order = _domian.GetWorkOrder(orderId);
                if (order == null) return;
                _hintService.Submit(orderId, false, Convert.ToInt32(Session["User_ID"]), DateTime.Now.AddHours(), TbNote.Text, Convert.ToInt32(DdlUsers.SelectedItem.Value), true);
                msg.InnerHtml = Tokens.Saved;
                TbNote.Text = string.Empty;
                DdlUsers.SelectedIndex = -1;
            }


            void Search()
            {
                var orders = DataLevelClass.GetUserWorkOrder();
                orders = ByPhone.Checked ? orders.Where(wo => wo.CustomerPhone.Equals(TbNamePhone.Text)).ToList() : orders.Where(wo => wo.CustomerName.Equals(TbNamePhone.Text)).ToList();
                GvCustomer.DataSource = orders.Select(x => SearchEngine.ToCustomerResult(x, _context));
                GvCustomer.DataBind();
            }


            void Activate()
            {
                GvCustomer.DataBound += (s, e) => Helper.GridViewNumbering(GvCustomer, "LNo");
                BSave.ServerClick += (s, e) => Save();
                BSearch.ServerClick += (s, e) => Search();
            }
        }
    }
 