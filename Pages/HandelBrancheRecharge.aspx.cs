using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class HandelBrancheRecharge : CustomPage
    {
        
            //readonly ISPDataContext _context;
            readonly IBranchCreditVoiceRepository _creditRepository;
            readonly IBoxCreditRepository _boxCreditRepository;
            //readonly IspDomian _domian;
            public  HandelBrancheRecharge()
            {
                //var _context = IspDataContext;
                //_domian = new IspDomian(IspDataContext);
                _creditRepository = new BranchCreditVoiceRepository();
                _boxCreditRepository = new BoxCreditRepository();
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                mv_container.SetActiveView(v_search);
                if (IsPostBack) return;
                // _domian.PopulateResellers(ddl_reseller, true);
                CheckOnBranches();
                PopulateBoxes();
            }


            void PopulateBoxes()
            {
                using (var context1 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    ddlBox.DataSource = context1.Boxes.Where(a => a.ShowBoxInResellerPPR == true);
                    ddlBox.DataTextField = "BoxName";
                    ddlBox.DataValueField = "ID";
                    ddlBox.DataBind();
                    Helper.AddDefaultItem(ddlBox);
                }
            }


            void CheckOnBranches()
            {
                using (var context2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var branch = DataLevelClass.GetUserBranches();
                    var branchHasRequest = new List<Branch>();
                    foreach (var item in branch)
                    {
                        var check = context2.RechargeBranchRequests.FirstOrDefault(ch => ch.BranchId == item.ID && ch.IsApproved == null);
                        if (check != null)
                        {
                            branchHasRequest.Add(item);
                        }
                    }
                    ddl_Branch.DataSource = branchHasRequest;
                    ddl_Branch.DataTextField = "BranchName";
                    ddl_Branch.DataValueField = "ID";
                    ddl_Branch.DataBind();
                    Helper.AddDefaultItem(ddl_Branch);

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
                cols[8].Visible = false;
                b_changeReseller.Visible = false;
            }


            void FindRequests()
            {
                using (var context3 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var requests = context3.RechargeBranchRequests
                        .Where(r =>
                            r.IsApproved == null && r.BranchId == Convert.ToInt32(HiddenField1.Value))
                        .Select(x => new
                        {
                            x.Id,
                            x.ClientName,
                            x.BranchId,
                            x.Branch.BranchName,
                            x.ClientTelephone,
                            x.Amount,
                            x.Time,
                            x.VoiceCompany.CompanyName
                        }).ToList();
                    gv_customers.DataSource = requests;
                    gv_customers.DataBind();
                    mv_container.SetActiveView(v_results);
                    // p_rDetails.GroupingText = Tokens.Requests + "&nbsp;" + "(" + gv_customers.Rows.Count + ")";
                }
            }


            protected void b_ChangeBranch_Click(object sender, EventArgs e)
            {
                mv_container.SetActiveView(v_search);

                gv_customers.DataSource = null;
                gv_customers.DataBind();
            }


            protected void gv_bConfirm_Click(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var id = Convert.ToInt32(hf_confirm.Value);
                    var boxid = Convert.ToInt32(hf_boxId.Value);
                    //var confirmButton = sender as Button;
                    //if (confirmButton == null) return;
                    //var id = Convert.ToInt32(confirmButton.CommandArgument);
                    var request = context.RechargeBranchRequests.FirstOrDefault(r => r.Id == id);
                    if (request != null)
                    {
                        request.IsApproved = true;
                        request.RejectReason = string.Empty;
                        var comisson = Convert.ToDecimal(request.VoiceCompany.CommissionResellerOrBranch);
                        var requestamount = Convert.ToDecimal(request.Amount);
                        var amount = requestamount - comisson;
                        //var amount = Convert.ToDecimal(request.Amount) * -1;
                        var result = _creditRepository.SaveVoice(Convert.ToInt32(request.BranchId), Convert.ToInt32(Session["User_ID"]), amount * -1, request.ClientName + " - " + request.ClientTelephone, DateTime.Now.AddHours());

                        switch (result)
                        {
                            case SaveVoiceResult.Saved:
                                if (boxid > 0)
                                {
                                    var notes = "طلب سداد فاتورة فرع" + " " + request.ClientName + " - " + request.ClientTelephone;
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
                using (var datacontext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var request = datacontext.RechargeBranchRequests.FirstOrDefault(r => r.Id == Convert.ToInt32(hf_rejectionId.Value));
                    if (request != null)
                    {
                        request.RejectReason = txt_RejectReason.Text;
                        request.IsApproved = false;
                    }

                    datacontext.SubmitChanges();
                    FindRequests();
                    mv_container.SetActiveView(v_results);
                    txt_RejectReason.Text = string.Empty;
                    l_message.Text = Tokens.Rejected;
                }
            }

        }
    }
 