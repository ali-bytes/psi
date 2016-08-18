using System;
using System.Configuration;
using System.Linq;
using System.Text;
using Db;
using NewIspNL.Domain;

namespace NewIspNL.Pages
{
    public partial class PrintReciept :CustomPage
    {
      
    readonly RecieptCnfgRepository _cnfgRepository;


    public PrintReciept(){
        var _context = IspDataContext;
        _cnfgRepository = new RecieptCnfgRepository(_context);
    }


    public ReceiptCnfg Cnfg { get; set; }


    protected void Page_Load(object sender, EventArgs e){
        /*using(*/
        var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
            var userId = Convert.ToInt32(Session["User_ID"]);
            Cnfg = _cnfgRepository.GetCnfg(userId);
            if(string.IsNullOrWhiteSpace(Request.QueryString["i"])) return;
            var receiptID = Convert.ToInt32(Request.QueryString["i"]);
            var currentReceipt = _context.Receipts.First(res => res.ID == receiptID);
            var currentUsersTransaction = _context.UsersTransactions.First(ut => ut.ID == currentReceipt.UserTransationID);
            var data = new StringBuilder();
            data.Append(currentUsersTransaction.WorkOrder.CustomerName + "\t");
            data.Append("&nbsp; ,&nbsp;تليفون: " + currentUsersTransaction.WorkOrder.CustomerPhone + "\t");
            data.Append("&nbsp; ,&nbsp;محافظة: " + currentUsersTransaction.WorkOrder.Governorate.GovernorateName);
            LCustomer.Text = data.ToString();
            LFor.Text = currentReceipt.Notes;
            LNumber.Text = string.Format("{0}", currentReceipt.ID);
            if(currentReceipt.PrcessDate != null) LDate.Text = currentReceipt.PrcessDate.Value.ToShortDateString();
            var user = _context.Users.FirstOrDefault(u => u.ID == Convert.ToInt32(Session["User_ID"]));
            LBy.Text = user == null ? "" : user.UserName;
            LPackage.Text = currentUsersTransaction.WorkOrder.ServicePackage.ServicePackageName;
        
    }
}
}