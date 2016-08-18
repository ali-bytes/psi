using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class anotherCustomersPayments : CustomPage  
    {
        
            readonly IspDomian _domian;
            readonly IBoxCreditRepository _boxCreditRepository;
            private readonly IUserSaveRepository _userSave;
            public  anotherCustomersPayments()
            {
                _domian = new IspDomian(IspDataContext);
                _boxCreditRepository = new BoxCreditRepository();
                _userSave = new UserSaveRepository();
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                //_domian.PopulateBoxes(DdlBox);
                PopulateBoxes();
                PopulatCompany();
                PopulateSaves();
            }

            private void PopulateBoxes()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    DdlBox.DataSource = context.Boxes.Where(a => a.ShowInCustomerDemands == true);
                    DdlBox.DataTextField = "BoxName";
                    DdlBox.DataValueField = "ID";
                    DdlBox.DataBind();
                    Helper.AddDefaultItem(DdlBox);
                }
            }

        void PopulatCompany()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var company = context.VoiceCompanies.ToList();
                    DdlVoiceCompany.DataSource = company;
                    DdlVoiceCompany.DataTextField = "CompanyName";
                    DdlVoiceCompany.DataValueField = "Id";
                    DdlVoiceCompany.DataBind();
                    Helper.AddDefaultItem(DdlVoiceCompany);
                }
            }
            void PopulateSaves()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var userId = Convert.ToInt32(Session["User_ID"]);
                    ddlSaves.DataSource = _userSave.SavesOfUser(userId, context).Select(a => new
                    {
                        a.Save.SaveName,
                        a.Save.Id
                    });
                    ddlSaves.DataBind();
                    Helper.AddDefaultItem(ddlSaves);
                }
            }


            protected void BtnSave_Click(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var boxid = Helper.GetDropValue(DdlBox);
                    var userId = Convert.ToInt32(Session["User_ID"]);
                    var boxAmount = Convert.ToDecimal(TbBoxAmount.Text);
                    var lastCredit = context.BoxCredits.Where(c => c.BoxId == boxid).OrderByDescending(x => x.ID).FirstOrDefault();
                    //var customerrequests = context.CustomerPayments.Where(a => a.BoxId == boxid).ToList();
                    //var amount = customerrequests.Sum(s => s.BoxAmount);
                    if (lastCredit != null)
                    {
                       // var newresellercredit = lastCredit.Net - amount;
                       
                            context.CustomerPayments.InsertOnSubmit(new CustomerPayment()
                            {
                                BoxId = boxid,
                                CustomerName = TbClientName.Text,
                                CustomerTelephone = TbClientPhone.Text,
                                Notes = TbNotes.Text,
                                ConfirmerId = userId,
                                InvoiceAmount = Convert.ToDecimal(TbInvoiceAmount.Text),
                                BoxAmount = boxAmount,
                                Time = DateTime.Now.AddHours(),
                                VoiceCompanyId = Helper.GetDropValue(DdlVoiceCompany)
                            });
                            context.SubmitChanges();
                            if (boxid != null)
                            {
                                var result = _boxCreditRepository.SaveBox(Convert.ToInt32(boxid), userId, boxAmount * -1,
                                    "دفع لعميل من خارج السيستم" + " " + TbClientName.Text + " - " + TbClientPhone.Text,
                                    DateTime.Now.AddHours());
                                switch (result)
                                {
                                    case SaveBoxResult.Saved:
                                        //if(boxid > 0){
                                        //  _boxCreditRepository.SaveBox(boxid, Convert.ToInt32(Session["User_ID"]), Convert.ToDecimal(txtDiscoundBox.Text) * -1, request.ClientName + " - " + request.ClientTelephone, DateTime.Now.AddHours());
                                        //}
                                        Message.Text = Tokens.Saved;
                                        context.SubmitChanges();
                                        break;
                                    case SaveBoxResult.NoCredit:
                                        Message.Text = Tokens.NotEnoughtCreditMsg;
                                        break;
                                }
                            }
                            var saveId = Convert.ToInt32(ddlSaves.SelectedItem.Value);
                            var res = _userSave.BranchAndUserSaves(saveId, userId, Convert.ToDouble(TbInvoiceAmount.Text),
                                 "دفع لعميل من خارج السيستم" + TbClientName.Text + " - " + TbClientPhone.Text, TbNotes.Text, context);
                            switch (res)
                            {
                                case SaveResult.Saved:
                                    Message.Text = Tokens.Saved;
                                    context.SubmitChanges();
                                    break;
                                case SaveResult.NoCredit:
                                    Message.Text = Tokens.NotEnoughtCreditMsg;
                                    break;
                            }
                            //Message.Text = Tokens.Saved;
                            Clear();
                            UpdateHistor(int.Parse(DdlBox.SelectedItem.Value));
                     
                    }
                    else
                    {
                        Message.Text = Tokens.CreditIsntEnough;
                    }

                }
            }


            void Clear()
            {
                TbBoxAmount.Text = TbInvoiceAmount.Text = TbClientName.Text = TbClientPhone.Text = TbNotes.Text = string.Empty;
                //DdlReseller.SelectedValue = null;
            }
            protected void GvHistory_DataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(GvHistory, "no");
            }


            void UpdateHistor(int boxId)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var history = context.CustomerPayments.Where(a => a.BoxId == boxId);
                    GvHistory.DataSource = history.ToList().OrderByDescending(x => x.Time).Select(x => new
                    {
                        x.ID,
                        BoxAmount = Helper.FixNumberFormat(x.BoxAmount),
                        InvoiceAmount = Helper.FixNumberFormat(x.InvoiceAmount),
                        //Net = Helper.FixNumberFormat(x.Net),
                        Box = x.Box.BoxName,
                        User = x.CustomerName,
                        Date = x.Time /*string.Format("{0:d}", x.Time)*/,
                        x.Notes,
                        Type = x.InvoiceAmount < 0 ? Tokens.Subtract : Tokens.Add,
                        RecieptUrl = string.Format("ResellerPaymentReciept.aspx?Customerpaymentsid={0}", x.ID),
                        x.VoiceCompanyId,
                        x.VoiceCompany.CompanyName,
                        x.CustomerTelephone,
                    });
                    GvHistory.DataBind();
                }
            }


            protected void DdlBox_SelectedIndexChanged(object sender, EventArgs e)
            {
                var id = string.IsNullOrEmpty(DdlBox.SelectedItem.Value) ? 0 : Convert.ToInt32(DdlBox.SelectedItem.Value);
                if (id == 0) return;
                UpdateHistor(id);
            }
        }
    }
 