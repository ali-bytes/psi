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
    public partial class HandelCustomerRecharge : CustomPage
    {
       
            //readonly ISPDataContext _context;
            readonly IResellerCreditVoiceRepository _creditRepository;
            readonly IBoxCreditRepository _boxCreditRepository;
            readonly IspDomian _domian;
            //private readonly CompanyEntryRepository _companyRepository;
            public HandelCustomerRecharge()
            {
                //_context = IspDataContext;
                _domian = new IspDomian(IspDataContext);
                _creditRepository = new ResellerCreditVoiceRepository();
                _boxCreditRepository = new BoxCreditRepository();
                //_companyRepository=new CompanyEntryRepository();
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                mv_container.SetActiveView(v_search);
                if (IsPostBack) return;
                // _domian.PopulateResellers(ddl_reseller, true);
                CheckOnReseller();
                populateBoxes();
            }


            void populateBoxes()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    ddlBox.DataSource = context.Boxes.Where(a => a.ShowBoxInResellerPPR == true);
                    ddlBox.DataTextField = "BoxName";
                    ddlBox.DataValueField = "ID";
                    ddlBox.DataBind();
                    Helper.AddDefaultItem(ddlBox);
                }
            }


            void CheckOnReseller()
            {
                using (var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var resellers = DataLevelClass.GetUserReseller();
                    var ResellerHasRequest = new List<User>();
                    foreach (var item in resellers)
                    {
                        var check = _context.RechargeClientRequests.FirstOrDefault(ch => ch.ResellerId == item.ID && ch.IsApproved == null);
                        if (check != null)
                        {
                            ResellerHasRequest.Add(item);
                        }
                    }
                    ddl_reseller.DataSource = ResellerHasRequest;
                    ddl_reseller.DataTextField = "UserName";
                    ddl_reseller.DataValueField = "ID";
                    ddl_reseller.DataBind();
                    Helper.AddDefaultItem(ddl_reseller);

                }
            }


            protected void b_addRequest_Click(object sender, EventArgs e)
            {
                FindRequests();
            }
            protected void gv_customers_DataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(gv_customers, "gv_lNumber");

                if (hf_user.Value != "6") return;
                var cols = gv_customers.Columns;
                cols[6].Visible = false;
                b_changeReseller.Visible = false;
            }


            void FindRequests()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var requests = context.RechargeClientRequests
                        .Where(r =>
                            r.IsApproved == null && r.ResellerId == Convert.ToInt32(HiddenField1.Value))
                        .Select(x => new
                        {
                            x.ID,
                            x.ClientName,
                            x.ResellerId,
                            x.User.UserName,
                            x.ClientTelephone,
                            x.Amount,
                            x.Time,
                            x.VoiceCompany.CompanyName
                        }).ToList();
                    gv_customers.DataSource = requests;
                    gv_customers.DataBind();
                    mv_container.SetActiveView(v_results);
                    //p_rDetails.GroupingText = Tokens.Requests + "&nbsp;" + "(" + gv_customers.Rows.Count + ")";
                }
            }


            protected void b_changeReseller_Click(object sender, EventArgs e)
            {
                mv_container.SetActiveView(v_search);

                gv_customers.DataSource = null;
                gv_customers.DataBind();
            }


            protected void gv_bConfirm_Click(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var id = Convert.ToInt32(hf_ApprovedId.Value);
                    var boxid = Convert.ToInt32(hf_boxId.Value);
                    //var confirmButton = sender as Button;
                    //if (confirmButton == null) return;
                    //var id = Convert.ToInt32(confirmButton.CommandArgument);
                    var request = context.RechargeClientRequests.FirstOrDefault(r => r.ID == id);
                    if (request != null)
                    {
                        request.IsApproved = true;
                        request.RejectReason = string.Empty;
                        var comisson = Convert.ToDecimal(request.VoiceCompany.CommissionResellerOrBranch);
                        var requestamount = Convert.ToDecimal(request.Amount);
                        var amount = requestamount - comisson;
                        var result = _creditRepository.SaveVoice(Convert.ToInt32(request.ResellerId), Convert.ToInt32(Session["User_ID"]), (amount * -1), request.ClientName + " - " + request.ClientTelephone, DateTime.Now.AddHours());
                        switch (result)
                        {
                            case SaveVoiceResult.Saved:
                                if (boxid > 0)
                                {
                                    var notes = "طلب سداد فاتورة موزع" + " " + request.ClientName + " - " + request.ClientTelephone;
                                    _boxCreditRepository.SaveBox(boxid, Convert.ToInt32(Session["User_ID"]), Convert.ToDecimal(txtDiscoundBox.Text) * -1, notes, DateTime.Now.AddHours());
                                }
                                l_message.Text = Tokens.Saved;
                                context.SubmitChanges();
                                break;
                            case SaveVoiceResult.NoCredit:
                                l_message.Text = Tokens.NotEnoughtCreditMsg;
                                break;
                        }
                        FindRequests();

                    }
                }
            }


            protected void btn_reject_Click(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var request = context.RechargeClientRequests.FirstOrDefault(r => r.ID == Convert.ToInt32(hf_rejectionId.Value));
                    if (request != null)
                    {
                        request.RejectReason = txt_RejectReason.Text;
                        request.IsApproved = false;
                    }

                    context.SubmitChanges();
                    FindRequests();
                    mv_container.SetActiveView(v_results);
                    txt_RejectReason.Text = string.Empty;
                    l_message.Text = Tokens.Rejected;
                }
            }
        }
    }
 