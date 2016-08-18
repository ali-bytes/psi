using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class RechargeCustomer : CustomPage
    {
        
 readonly IspDomian _domian;
    private readonly CompanyEntryRepository _companyRepository;
 
    public RechargeCustomer(){
        _domian=new IspDomian(IspDataContext);
        _companyRepository=new CompanyEntryRepository();
       
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        _domian.PopulateResellers(DdlReseller, true);
        PopulatCompany();
    }


    void PopulatCompany(){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var company = context.VoiceCompanies.ToList();
            DdlVoiceCompany.DataSource = company;
            DdlVoiceCompany.DataTextField = "CompanyName";
            DdlVoiceCompany.DataValueField = "Id";
            DdlVoiceCompany.DataBind();
            Helper.AddDefaultItem(DdlVoiceCompany);
        }
    }


    protected void BtnSave_Click(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
           
            var total = Convert.ToDecimal(TbAmount.Text) + Convert.ToDecimal(txtServiceFees.Text);
            var resellerid = Helper.GetDropValue(DdlReseller);
            var lastCredit = context.ResellerCreditsVoices.Where(c => c.ResellerId == resellerid).OrderByDescending(x => x.Id).FirstOrDefault();
            var resellerrequests = context.RechargeClientRequests.Where(a => a.ResellerId == resellerid && a.IsApproved == null).ToList();
            var amount = resellerrequests.Sum(s => s.Amount);
            if(lastCredit != null){
                var newresellercredit = lastCredit.Net - amount;
                if(newresellercredit >= total){
                    context.RechargeClientRequests.InsertOnSubmit(new RechargeClientRequest(){
                        ResellerId = Helper.GetDropValue(DdlReseller),
                        ClientName = TbClientName.Text,
                        ClientTelephone = TbClientPhone.Text,
                        Notes = TbNotes.Text,
                        Amount = total,
                        Time = DateTime.Now.AddHours(),
                        VoiceCompanyID = Helper.GetDropValue(DdlVoiceCompany)
                    });
                    context.SubmitChanges();
                    Message.Text = Tokens.Saved;
                    clear();
                    UpdateHistor(int.Parse(DdlReseller.SelectedItem.Value));
                } else{
                    Message.Text = Tokens.CreditIsntEnough;
                }
            } else{
                Message.Text = Tokens.CreditIsntEnough;
            }

        }
    }


    void clear()
    {
        txtTotal.Text=txtServiceFees.Text=TbAmount.Text = TbClientName.Text = TbClientPhone.Text = TbNotes.Text = string.Empty;
       
    }
    protected void GvHistory_DataBound(object sender, EventArgs e)
    {
        Helper.GridViewNumbering(GvHistory, "no");
    }


    void UpdateHistor(int resellerId){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var history = context.RechargeClientRequests.Where(a => a.ResellerId == resellerId);
            GvHistory.DataSource = history.ToList().OrderByDescending(x => x.Time).Select(x => new{
                x.ID,
                Amount = Helper.FixNumberFormat(x.Amount),
               
                Reseller = x.User.UserName,
                User = x.ClientName,
                Date = x.Time ,
                x.Notes,
                Type = x.Amount < 0 ? Tokens.Subtract : Tokens.Add,
                RecieptUrl = string.Format("ResellerPaymentReciept.aspx?RequestVoiceid={0}", x.ID),
                x.VoiceCompanyID,
                x.VoiceCompany.CompanyName,
                x.VoiceCompany.ServiceFees,
                x.ClientTelephone,
            });
            GvHistory.DataBind();
        }
    }


    protected void DdlReseller_SelectedIndexChanged(object sender, EventArgs e)
    {
        var id = string.IsNullOrEmpty(DdlReseller.SelectedItem.Value) ? 0 : Convert.ToInt32(DdlReseller.SelectedItem.Value);
        if (id == 0)
        {
            return;
        }
        UpdateHistor(id);
    }
    
    protected void DdlVoiceCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            int companyid =string.IsNullOrEmpty(DdlVoiceCompany.SelectedItem.Value)?0: Convert.ToInt32(Helper.GetDropValue(DdlVoiceCompany));
            if (companyid == 0){
                txtServiceFees.Text = string.Empty;
                return;
            }
            var company = _companyRepository.Get(companyid, context);
            txtServiceFees.Text = company.ServiceFees > 0 && company.ServiceFees != null ? company.ServiceFees.ToString() : "0";
        }
    }
}
}