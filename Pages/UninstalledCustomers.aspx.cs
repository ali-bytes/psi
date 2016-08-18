using System;
using System.Configuration;
using System.Linq;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using NewIspNL.Services;
using Resources;

namespace NewIspNL.Pages
{
    public partial class UninstalledCustomers : CustomPage
    {
       
   

    readonly IspDomian _domian;
    private readonly IUserSaveRepository _userSave;

    readonly WorkOrderRepository _orderRepository;
    public UninstalledCustomers()
    {
       var context = IspDataContext;
        _domian = new IspDomian(context);
        _orderRepository = new WorkOrderRepository();
        _userSave=new UserSaveRepository();
    }


    public bool CanProcess { get; set; }


    protected void Page_Load(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            Activate();
            var user = context.Users.FirstOrDefault(u => u.ID == Convert.ToInt32(Session["User_ID"]));
            CanProcess = user != null && user.GroupID != null && user.GroupID.Value != 6;
            if(IsPostBack) return;
            IsInSearch.Value = "0";
            _domian.PopulateResellerswithDirectUser(DdlReseller, true); //_domian.PopulateResellers(DdlReseller, true);
            _domian.PopulateBranches(DdlBranch, true);
            var option = OptionsService.GetOptions(context, true);
            if (option.IntallationDiscound)
            {
                BInstall.Visible = false;
                GvReport.Columns[10].Visible = true;
                GvReport.Columns[9].Visible = false;
            }
            else
            {
                BInstall.Visible = true;
                GvReport.Columns[10].Visible = false;
                GvReport.Columns[9].Visible = true;
            }
            PopulateSaves(context);
            SearchOrders();
        }
    }

    void PopulateSaves(ISPDataContext context)
    {
        var userId = Convert.ToInt32(Session["User_ID"]);
        ddlSaves.DataSource = _userSave.SavesOfUser(userId, context).Select(a=>new
        {
            a.Save.SaveName,
            a.Save.Id
        });
        ddlSaves.DataBind();
        Helper.AddDefaultItem(ddlSaves);
    }


   
    public void SearchOrders()
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            IsInSearch.Value = "1";
            var userWorkOrder = DataLevelClass.GetUserWorkOrder(context);
            if (DdlBranch.SelectedIndex > 0)
            {
                userWorkOrder = userWorkOrder
                    .Where(x =>
                        x.BranchID == Convert.ToInt32(DdlBranch.SelectedItem.Value)).ToList();
            }
            if (DdlReseller.SelectedIndex > 0)
            {
                if (DdlReseller.SelectedItem.Value != @"0")
                {
                    userWorkOrder = userWorkOrder
                        .Where(x => x.ResellerID == Convert.ToInt32(DdlReseller.SelectedItem.Value)).ToList();
                }
                else
                {
                    userWorkOrder = userWorkOrder.Where(x => x.ResellerID == null).ToList();
                }
            }
            var activewor = userWorkOrder.Where(x => x.WorkOrderStatusID == 6 && x.Installed != null).ToList();
            var orders = activewor
                .Where(x => x.Installed != null && x.Installed.Value == false).ToList();

            if (!string.IsNullOrWhiteSpace(TbActivation.Text))
            {

                try
                {
                    var time = Convert.ToDateTime(TbActivation.Text);
                    orders = _orderRepository
                        .OrderbyActivationDate(orders, time);
                }
                catch (Exception) { }
            }

            GvReport.DataSource = orders.Select(x => new
            {
                x.ID,
                x.CustomerName,
                x.CustomerPhone,
                x.Governorate.GovernorateName,
                x.Status.StatusName,
                x.Branch.BranchName,
                x.ServicePackage.ServicePackageName,
                Reseller = x.ResellerID == null ? "-" : _domian.GetUserName(Convert.ToInt32(x.ResellerID)),
                Process = CanProcess,
                x.ServiceProvider.SPName
            });
            GvReport.DataBind();
        }
    }
    protected void BInstall_OnClick(object sender, EventArgs e){
        using(var context = IspDataContext){//new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
        if(!CanProcess) return;
            foreach (GridViewRow row in GvReport.Rows)
            {
                var check = row.FindControl("CbInstall") as CheckBox;
                if (check == null || !check.Checked) continue;
                var hf = row.FindControl("HfId") as HiddenField;
                if (hf == null || string.IsNullOrEmpty(hf.Value)) continue;
                var id = Convert.ToInt32(hf.Value);
                var order = _domian.GetWorkOrder(id);
                if (order == null) continue;
                var userId = Convert.ToInt32(Session["User_ID"]);
                order.Installed = true;
                order.Installer = userId;
                order.InstallationTime = DateTime.Now.AddHours();
                var note = order.Notes;
                order.Notes = note + " - " + txtNote.Text;
                context.SubmitChanges();
                _domian.Commit();
                successMsg.Visible = true;
                successMsg.InnerText = Tokens.Saved;
            }

            SearchOrders();
    }
    }


    protected void GvReport_DataBound(object sender, EventArgs e){
        Helper.GridViewNumbering(GvReport, "lNo");
    }


    void Activate(){
        BSearch.ServerClick += (o, e) => SearchOrders();
        //BResearch.ServerClick += (o, e) => IsInSearch.Value = "0";
    }

    protected void Installorder(object sender, EventArgs e)
    {
            if (hfOnce == null || string.IsNullOrEmpty(hfOnce.Value)) return;
            var id = Convert.ToInt32(hfOnce.Value); //HfId.Value);
            var order = _domian.GetWorkOrder(id);
            if (order == null) return;
            var userId = Convert.ToInt32(Session["User_ID"]);
            if (order.ResellerID != null)
            {
                ManageInstallation(order,userId);
            }
            else
            {
                if (order.Prepaid > 0)
                {
                    successMsg.Visible = true;
                    ManageInstallation(order,userId);
                    successMsg.InnerText += "هذا العميل له مدفوع مقدما";
                    
                }
                else
                {
                    mpe_PrePaid.Show();
                    lblModalTitle.Text = @"هذا العميل ليس له مدفوع مقدما";
                }
            }
        SearchOrders();
    }
    protected void Btncancel(object sender, EventArgs e)
    {
        mpe_PrePaid.Hide();
    }

    protected void BtnManagePrepaid(object sender, EventArgs e)
    {
        if (hfOnce == null || string.IsNullOrEmpty(hfOnce.Value)) return;
        var id = Convert.ToInt32(hfOnce.Value); //HfId.Value);
        var order = _domian.GetWorkOrder(id);
        if (order == null) return;
        var userId = Convert.ToInt32(Session["User_ID"]);
        if (order.Prepaid <= 0 || order.Prepaid == null)
        {
            var amount = Convert.ToDecimal(txtPrePaid.Text);
            Savestepsinsaves(order.CustomerName, order.CustomerPhone, Convert.ToDouble(amount));
            order.Installed = true;
            order.Installer = userId;
            order.InstallationTime = DateTime.Now.AddHours();
            var note = order.Notes;
            order.Notes = note + " - " + txtNote.Text;
            order.Prepaid = amount;
            _domian.Commit();
            successMsg.Visible = true;
            successMsg.InnerText = Tokens.Saved;
            txtNote.Text = txtPrePaid.Text = string.Empty;
            SearchOrders();
        }

    }

    void Savestepsinsaves(string name, string phone, double amount)
    {
        try
        {
            using (var db6 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                int userId = Convert.ToInt32(Session["User_ID"]);
                var not = "المدفوع مقدما عند تركيب عميل" + " "+Tokens.Customer_Name + " : " + name + " - " +
                              Tokens.Customer_Phone + " : " + phone;
                var saveId = Convert.ToInt32(ddlSaves.SelectedItem.Value);
                _userSave.BranchAndUserSaves(saveId, userId, Convert.ToDouble(amount),
                    "مدفوع مقدما من صفحة عملاء بانتظار التركيب", not, db6);
                #region Same Code
              
                #endregion
            }
        }
        catch { }
    }
    void ManageInstallation(WorkOrder order,int userId)
    {
        var resellerCredit = new ResellerCreditRepository();
        var bill = new PriceServices();
        var now = DateTime.Now.AddHours();
        var resellerId = Convert.ToInt32(order.ResellerID);
        var billamount = bill.BillDefault(order, now.Month, now.Year, null).Net;
        var notes = "تخصيم دفعة مقدم " + Tokens.Customer_Name + " : " + order.CustomerName +" - "+ Tokens.Customer_Phone + " : "+order.CustomerPhone;
        var result =resellerId!=0?resellerCredit.Save(resellerId, userId, billamount * -1, notes, now):SaveResult.Saved;
        switch (result)
        {
            case SaveResult.Saved:
                order.Installed = true;
                order.Installer = userId;
                order.InstallationTime = DateTime.Now.AddHours();
                var note = order.Notes;
                order.Notes = note + " - " + txtNote.Text;
                order.Prepaid = billamount;
                _domian.Commit();
                successMsg.Visible = true;
                successMsg.InnerText = Tokens.Saved;
                txtNote.Text=txtPrePaid.Text = string.Empty;
                break;
            case SaveResult.NoCredit:
                errorMsg.Visible = true;
                errorMsg.InnerText = Tokens.ResellerCreditIsnotEnough;
                break;

        }
    }
}

}