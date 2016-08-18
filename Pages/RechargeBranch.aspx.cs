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
    public partial class RechargeBranch : CustomPage
    {
    
    readonly IspDomian _domian;
    private readonly CompanyEntryRepository _companyRepository;

    //readonly ISPDataContext _context;

    public RechargeBranch(){
        _domian=new IspDomian(IspDataContext);
        _companyRepository=new CompanyEntryRepository();
       
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        _domian.PopulateBranches(DdlBranch,true);
      
        PopulatCompany();
    }


    void PopulatCompany(){
        using(var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var company = _context.VoiceCompanies.ToList();
            DdlVoiceCompany.DataSource = company;
            DdlVoiceCompany.DataTextField = "CompanyName";
            DdlVoiceCompany.DataValueField = "Id";
            DdlVoiceCompany.DataBind();
            Helper.AddDefaultItem(DdlVoiceCompany);
        }
    }


    protected void BtnSave_Click(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
           
            var amot = Convert.ToDecimal(txtServiceFees.Text);
            var am = Convert.ToDecimal(TbAmount.Text);
            var total = am + amot;
            var branchid = Helper.GetDropValue(DdlBranch);
            var lastCredit = context.BranchCreditVoices.Where(c => c.BranchId == branchid).OrderByDescending(x => x.Id).FirstOrDefault();
            var branchrequests = context.RechargeBranchRequests.Where(a => a.BranchId == branchid && a.IsApproved == null).ToList();
            var amount = branchrequests.Sum(s => s.Amount);
            if(lastCredit != null){
                var newbranchcredit = lastCredit.Net - amount;
                if (newbranchcredit >= (total))
                {
                    context.RechargeBranchRequests.InsertOnSubmit(new RechargeBranchRequest(){
                        BranchId = Helper.GetDropValue(DdlBranch),
                        ClientName = TbClientName.Text,
                        ClientTelephone = TbClientPhone.Text,
                        Notes = TbNotes.Text,
                        Amount = total,
                        Time = DateTime.Now.AddHours(),
                        VoiceCompanyId = Helper.GetDropValue(DdlVoiceCompany)
                    });
                    context.SubmitChanges();
                    Message.Text = Tokens.Saved;
                    clear();
                    UpdateHistor(int.Parse(DdlBranch.SelectedItem.Value));
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


    void UpdateHistor(int BranchId){
        using(var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var history = _context.RechargeBranchRequests.Where(a => a.BranchId == BranchId);
            GvHistory.DataSource = history.ToList().OrderByDescending(x => x.Time).Select(x => new{
                x.Id,
                Amount = Helper.FixNumberFormat(x.Amount),
                Branche = x.Branch.BranchName,
                User = x.ClientName,
                Date = x.Time,
                x.Notes,
                Type = x.Amount < 0 ? Tokens.Subtract : Tokens.Add,
                RecieptUrl = string.Format("BranchePaymentReciept.aspx?RequestBrancheid={0}", x.Id),
                x.VoiceCompany.CompanyName,
                x.ClientTelephone,
                x.VoiceCompany.ServiceFees
            });
            GvHistory.DataBind();
        }
    }


    protected void DdlBranche_SelectedIndexChanged(object sender, EventArgs e)
    {
        var id = string.IsNullOrEmpty(DdlBranch.SelectedItem.Value) ? 0 : Convert.ToInt32(DdlBranch.SelectedItem.Value);
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
            if(companyid==0){txtServiceFees.Text = string.Empty;
                return;
            }
            var company = _companyRepository.Get(companyid, context);
            txtServiceFees.Text = company.ServiceFees > 0 && company.ServiceFees!=null ? company.ServiceFees.ToString() : "0";
        }
    }
}
}