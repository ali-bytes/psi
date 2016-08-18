using System;
 
using Db;
using NewIspNL.Helpers;
using NewIspNL.Services;

namespace NewIspNL
{
    public partial class CreateCustomers : CustomPage{
    
        readonly ISPDataContext _context;


        public CreateCustomers()
        {
            _context = IspDataContext;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            Helper.PopulateDrop(_context.Status, DdlStstus, "ID", "StatusName");
        }


        protected void Create(object sender, EventArgs e)
        {
            var service = new WorkOrderService(_context);

            var statusId = 5;
            if (DdlStstus.SelectedIndex > 0)
            {
                statusId = Convert.ToInt32(DdlStstus.SelectedItem.Value);
            }
            service.CreateSingleOrders(null, statusId, null);
        }
    }
}