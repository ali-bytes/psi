using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class BoxBS :  CustomPage
    {
     
            //static bool BranchPrintExcel;
            readonly IspDomian _domian;


            public BoxBS()
            {
                //ISPDataContext _context = IspDataContext;
                _domian = new IspDomian(IspDataContext);
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                _domian.PopulateBoxes(ddl_Box);
            }


            protected void btn_search_Click(object sender, EventArgs e)
            {
                using (var db = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var box = db.Boxes.FirstOrDefault(user => user.ID == Helper.GetDropValue(ddl_Box));
                    if (box == null) return;
                    tb_SearchResult.Visible = true;
                    ViewState.Add("ID", box.ID);
                    //ViewState.Add("BranchID", box.BranchID);
                    Bind_grd_Transactions();
                    //rbl_Distination.Items[1].Enabled = box.BranchID != 1 ? (rbl_Distination.Items[2].Enabled = false) : (rbl_Distination.Items[2].Enabled = true);
                    //GetResellerDiscount(box.ID);
                }
            }


            void Bind_grd_Transactions()
            {
                using (var db1 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var startAt = Convert.ToDateTime(TbStartAt.Text);
                    var endAt = Convert.ToDateTime(TbTo.Text);
                    var boxId = Convert.ToInt32(ViewState["ID"]);
                    var query = db1.BoxCredits
                        .Where(boxTransaction => boxTransaction.BoxId == boxId && boxTransaction.Time != null && boxTransaction.Time.Value.Date >= startAt.Date && boxTransaction.Time.Value.Date <= endAt.Date)
                        .OrderByDescending(a => a.Time).ToList()
                        .Select(x => new
                        {
                            x.Box.BoxName,
                            x.User.UserName,
                            x.Amount,
                            x.Time,
                            x.Notes,
                            Total = Helper.FixNumberFormat(Convert.ToDouble(x.Net)),
                            Reseller = (x.RechargeRequestId == null && x.RechargeBranchId == null) ? "" : x.RechargeRequestId != null ? x.RechargeRequest.User.UserName : x.RechargeRequestBranch.Branch.BranchName,
                            link = (x.RechargeRequestId == null && x.RechargeBranchId == null) ? "#" : x.RechargeRequestId != null ? string.Format("../Attachments/{0}", x.RechargeRequest.RecieptImage) : string.Format("../Attachments/{0}", x.RechargeRequestBranch.RecieptImage),
                            Depositer = (x.RechargeRequestId == null && x.RechargeBranchId == null) ? "" : x.RechargeRequestId != null ? x.RechargeRequest.DepositorName : x.RechargeRequestBranch.DepositorName,
                        }).ToList();

                    grd_Transactions.DataSource = query;
                    grd_Transactions.DataBind();
                    if (query.Count != 0)
                        BtnExport.Visible = true;

                }
            }


            protected void BtnExport_Click(object sender, EventArgs e)
            {
                //BranchPrintExcel = true;
                const string attachment = "attachment; filename=BoxTransactions.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/ms-excel";
                Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");

                var sw = new StringWriter();
                var htw = new HtmlTextWriter(sw);
                grd_Transactions.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
        }
    }
