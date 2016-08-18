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
using Resources;

namespace NewIspNL.Pages
{
    public partial class BranchVoiceBS : CustomPage
    {
            readonly IspDomian _domian;
            public  BranchVoiceBS()
            {
                _domian = new IspDomian(IspDataContext);
            }
            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                _domian.PopulateBranches(ddl_Branch, true);
            }
            protected void btn_search_Click(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var branch = context.Branches.FirstOrDefault(us => us.ID == Helper.GetDropValue(ddl_Branch));
                    tb_SearchResult.Visible = true;
                    var data = new SearchData();
                    if (branch != null) data.Branch = branch.ID;
                    if (!string.IsNullOrEmpty(TbStartAt.Text)) data.startAt = Convert.ToDateTime(TbStartAt.Text);
                    if (!string.IsNullOrEmpty(TbTo.Text)) data.endAt = Convert.ToDateTime(TbTo.Text);
                    Bind_grd_Transactions(data);

                }
            }


            void Bind_grd_Transactions(SearchData data)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var query = context.RechargeBranchRequests.OrderByDescending(x => x.Time)
                        .Select(x => new
                        {
                            x.Id,
                            x.BranchId,
                            x.RejectReason,
                            x.IsApproved,
                            x.Branch.BranchName,
                            x.ClientName,
                            x.ClientTelephone,
                            x.VoiceCompany.CompanyName,
                            x.Amount,
                            x.Time,
                            x.Notes
                        }).ToList();
                    if (data.Branch != null)
                    {
                        query = query.Where(x => x.BranchId == data.Branch).ToList();
                    }
                    if (data.startAt != null)
                    {
                        query = query.Where(x => x.Time.Value.Date >= data.startAt).ToList();
                    }
                    if (data.endAt != null)
                    {
                        query = query.Where(x => x.Time.Value.Date <= data.endAt).ToList();
                    }
                    grd_Transactions.DataSource = query;
                    grd_Transactions.DataBind();
                    if (query.Count != 0)
                        BtnExport.Visible = true;
                }
            }


            protected void BtnExport_Click(object sender, EventArgs e)
            {
                const string attachment = "attachment; filename=BranchRequests.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/ms-excel";
                var sw = new StringWriter();
                var htw = new HtmlTextWriter(sw);
                grd_Transactions.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
            protected void grd_DataBound(object sender, EventArgs e)
            {
                foreach (GridViewRow row in grd_Transactions.Rows)
                {
                    var label = row.FindControl("lblStatus") as Label;
                    if (label != null)
                    {
                        if (label.Text == "True")
                        {
                            label.Text = Tokens.Approved;
                            label.CssClass = "label label-success arrowed";
                        }
                        else if (label.Text == "False")
                        {
                            label.Text = Tokens.Rejected;
                            label.CssClass = "label label-danger arrowed-in";
                        }
                        else
                        {
                            label.Text = Tokens.PendingRequest;
                            label.CssClass = "label label-warning";
                        }
                    }
                }
            }
            public class SearchData
            {
                public DateTime? startAt { get; set; }

                public DateTime? endAt { get; set; }

                public int? Branch { get; set; }
            }
        }
    }
 