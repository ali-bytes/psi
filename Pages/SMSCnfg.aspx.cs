using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class SMSCnfg : CustomPage
    {
    
    readonly SMSData _cnfgRepository;
    private readonly SMSNotifications _notifications;

   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        MultiView1.SetActiveView(v_index);
        PopulateCnfgs();
        PopulateNotifications();
        l_message.Text = "";
    }


    public SMSCnfg(){
        var context=new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        _cnfgRepository = new SMSData(context);
        _notifications=new SMSNotifications(context);
    }


    void PopulateCnfgs(){
        var smsCnfgs = _cnfgRepository.SmsCnfgs;
        gv_index.DataSource = smsCnfgs;
        gv_index.DataBind();
    }

    private void PopulateNotifications()
    {
        var noti = _notifications.GetList();
        CheckNotification.DataSource = noti;
        CheckNotification.DataTextField = "CaseName";
        CheckNotification.DataValueField = "Id";
        CheckNotification.DataBind();
        foreach (var item in noti)
        {
            FillChecks(item);
        }
    }

    private void FillChecks(SMSCaseNotification sms)
    {
        foreach (ListItem item in CheckNotification.Items)
        {
            if (sms.Id == Convert.ToInt32(item.Value))
            {
                var l = Convert.ToBoolean(sms.Send);
                if (l)
                {
                    item.Selected = true;
                    var selected =
                        _notifications.SmsNotification.FirstOrDefault(a => a.Id == Convert.ToInt32(item.Value));
                    switch (selected.Id)
                    {
                        case 1:
                             txt0CustomerInvoice.Text=selected.Message;
                            break;
                        case 3:
                            txt1ResellerInvoice.Text=selected.Message;
                            break;
                        case 4:
                            txt2BranchInvoice.Text=selected.Message;
                            break;
                        case 5:
                            txt3CustomerPaymentInvoice.Text=selected.Message;
                            break;
                        case 6:
                            txt4AddCreditToReseller.Text=selected.Message;
                            break;
                        case 7:
                            txt5AddCreditToBranch.Text=selected.Message;
                            break;
                        case 8:
                            txt6ResellerPaymentFromBs.Text=selected.Message;
                            break;
                        case 9:
                            txt7BranchPaymentFromBs.Text=selected.Message;
                            break;
                        case 10:
                            txt8ConfirmRechargeReseller.Text=selected.Message;
                            break;
                        case 11:
                            txt9ConfirmRechargeBranch.Text=selected.Message;
                            break;
                        case 12:
                            txt10RunLine.Text=selected.Message;
                            break;
                        case 13:
                            txt11Suspend.Text=selected.Message;
                            break;
                        case 14:
                            txt12Cancle.Text=selected.Message;
                            break;
                        case 15:
                            txt13ChangePackage.Text=selected.Message;
                            break;
                        case 16:
                            txt14AddIPPackage.Text=selected.Message;
                            break;
                        case 17:
                            txt15AddExtraGiga.Text=selected.Message;
                            break;
                        case 18:
                            txt16AddNewCustomer.Text = selected.Message;
                            break;
                    }

                }
            }
        }
    }


    protected void b_save_Click(object sender, EventArgs e)
    {
        global::Db.SMSCnfg smsCnfg;
        
        if (hf_id.Value == string.Empty)
        {
            smsCnfg = new global::Db.SMSCnfg
            {
                UserName = txtUserName.Text,
                Password = txtPassword.Text,
                Sender = txtSender.Text,
                UrlAPI = txtUrl.Text,
                sendsms = chkSend.Checked
            };
        }
        else
        {
            smsCnfg = _cnfgRepository.SmsCnfgs.FirstOrDefault(o => o.Id == Convert.ToInt32(hf_id.Value));
            if (smsCnfg != null)
            {
                smsCnfg.UserName = txtUserName.Text;
                smsCnfg.Password = txtPassword.Text;
                smsCnfg.Sender = txtSender.Text;
                smsCnfg.UrlAPI = txtUrl.Text;
                smsCnfg.sendsms = chkSend.Checked;
            }
           
            foreach (ListItem item in CheckNotification.Items)
            {
                var se = _notifications.SmsNotification.FirstOrDefault(s => s.Id ==Convert.ToInt32(item.Value));
                if (item.Selected)
                {
                    
                  
                    if (se != null)
                    {
                        se.Send = true;
                      
                            switch (se.Id)
                            {
                                case 1:
                                    se.Message = txt0CustomerInvoice.Text;
                                    break;
                                case 3:
                                    se.Message = txt1ResellerInvoice.Text;
                                    break;
                                case 4:
                                    se.Message = txt2BranchInvoice.Text;
                                    break;
                                case 5:
                                    se.Message = txt3CustomerPaymentInvoice.Text;
                                    break;
                                case 6:
                                    se.Message = txt4AddCreditToReseller.Text;
                                    break;
                                case 7:
                                    se.Message = txt5AddCreditToBranch.Text;
                                    break;
                                case 8:
                                    se.Message = txt6ResellerPaymentFromBs.Text;
                                    break;
                                case 9:
                                    se.Message = txt7BranchPaymentFromBs.Text;
                                    break;
                                case 10:
                                    se.Message = txt8ConfirmRechargeReseller.Text;
                                    break;
                                case 11:
                                    se.Message = txt9ConfirmRechargeBranch.Text;
                                    break;
                                case 12:
                                    se.Message = txt10RunLine.Text;
                                    break;
                                case 13:
                                    se.Message = txt11Suspend.Text;
                                    break;
                                case 14:
                                    se.Message = txt12Cancle.Text;
                                    break;
                                case 15:
                                    se.Message = txt13ChangePackage.Text;
                                    break;
                                case 16:
                                    se.Message = txt14AddIPPackage.Text;
                                    break;
                                case 17:
                                    se.Message = txt15AddExtraGiga.Text;
                                    break;
                                case 18:
                                    se.Message = txt16AddNewCustomer.Text;
                                    break;
                                default:
                                    se.Message = "";
                                    break;
                            }
                        

                    }
                    _notifications.Save(se);
                }
                else
                {
                    se.Send = false;
                    _notifications.Save(se);
                }
            }
        }
        _cnfgRepository.Save(smsCnfg);
        PopulateCnfgs();
        if (smsCnfg != null) l_message.Text = string.Format(Tokens.Saved);
        hf_id.Value = string.Empty;
        Clear();
        MultiView1.SetActiveView(v_index);
    }


    void Clear()
    {
        foreach (var control in p_add.Controls)
        {
            var tb = control as TextBox;
            if (tb != null)
            {
                tb.Text = string.Empty;
            }
            else
            {
                var cb = control as CheckBox;
                if (cb != null)
                {
                    cb.Checked = false;
               }
           }
        }
    }


    protected void gv_index_DataBound(object sender, EventArgs e)
    {
        Helper.GridViewNumbering(gv_index, "l_number");
    }


    protected void gvb_edit_Click(object sender, EventArgs e)
    {
        MultiView1.SetActiveView(v_AddEdit);
        var buttonSender = sender as LinkButton;
        if (buttonSender == null) return;
        var id = Convert.ToInt32(buttonSender.CommandArgument);
        var smsCnfg = _cnfgRepository.SmsCnfgs.FirstOrDefault(o => o.Id == id);
        if (smsCnfg == null) return;
        txtUserName.Text = smsCnfg.UserName;
        txtPassword.Text = smsCnfg.Password;
        txtSender.Text = smsCnfg.Sender;
        txtUrl.Text = smsCnfg.UrlAPI;
        chkSend.Checked = Convert.ToBoolean(smsCnfg.sendsms);
        hf_id.Value = smsCnfg.Id.ToString(CultureInfo.InvariantCulture);


    }


    protected void gvb_delete_Click(object sender, EventArgs e)
    {
        var cnfg = _cnfgRepository.SmsCnfgs.FirstOrDefault(o => o.Id == Convert.ToInt32((sender as Button).CommandArgument));
        if (cnfg == null) return;
        l_message.Text = string.Format(Tokens.Saved);
        _cnfgRepository.Delete(cnfg);
        PopulateCnfgs();
    }
}
}