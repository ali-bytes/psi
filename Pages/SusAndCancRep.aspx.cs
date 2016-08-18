using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using Antlr.Runtime;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using Resources;
using Tokens = Resources.Tokens;

namespace NewIspNL.Pages
{
    public partial class SusAndCancRep : CustomPage
    {
        readonly IspDomian _domian;
        public SusAndCancRep()
        {
            _domian = new IspDomian(IspDataContext);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["User_ID"] == null)
            {
                Response.Redirect("default.aspx");
           
            }
            if (!IsPostBack)
            {
                fill_drop();
                export_div.Visible = false;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            fill_Grid();
        }

        public void fill_drop()
        {
            List<string>add=new List<string>();
            add.Add("غرامة 30 جنية لان عدد ايام السسبند اصبح 90 يوم");
            add.Add("غرامة الكانسل خلال السنة الأولى");
            add.Add("المطالبات المدفوعة عن طريق خدمة فوري");
            ddlrepType.DataSource = add;
            ddlrepType.DataBind();
            _domian.PopulateResellerswithDirectUser(ddlReseller, true);
            _domian.PopulateBranches(ddlBranch, true);
        }
        protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd_Requests.PageIndex = e.NewPageIndex;
            fill_Grid();
        }
       
        protected void grd_Requests_OnDataBound(object sender, EventArgs e)
        {
            Helper.GridViewNumbering(grd_Requests, "LNo");
        }

        public void fill_Grid()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {


                var datefrom = Convert.ToDateTime(tb_from.Text);
                var dateto = Convert.ToDateTime(tb_to.Text).AddHours(23).AddMinutes(59);
               
               

               if(ddlrepType.Text=="المطالبات المدفوعة عن طريق خدمة فوري")
               {
                   if (ddlBranch.SelectedIndex <= 0 && ddlReseller.SelectedIndex <= 0)
                   {
                       GetPayedDemand(context, datefrom, dateto, -1, 0);
                       return;
                   }
                   if (ddlBranch.SelectedIndex > 0 && ddlReseller.SelectedItem.Value == "0")
                   {
                       var branchId = Convert.ToInt32(ddlBranch.SelectedItem.Value);
                       GetPayedDemand(context, datefrom, dateto, 0, branchId);
                       return;
                   }
                   if (ddlBranch.SelectedIndex > 0 && ddlReseller.SelectedIndex > 0)
                   {
                       var resellerId = Convert.ToInt32(ddlReseller.SelectedItem.Value);
                       var branchId = Convert.ToInt32(ddlBranch.SelectedItem.Value);
                       GetPayedDemand(context, datefrom, dateto, resellerId, branchId);
                       return;
                   }

                   if (ddlReseller.SelectedItem.Value == "0")
                   {

                       GetPayedDemand(context, datefrom, dateto, 0,0);
                       return;
                   }
                  
                   if (ddlReseller.SelectedIndex > 0)
                   {
                       var resellerId = Convert.ToInt32(ddlReseller.SelectedItem.Value);
                       GetPayedDemand(context, datefrom, dateto, resellerId, 0);
                       return;
                   }
                  
                   if (ddlBranch.SelectedIndex > 0)
                   {
                       var branchId = Convert.ToInt32(ddlBranch.SelectedItem.Value);
                       GetPayedDemand(context, datefrom, dateto, -1, branchId);
                       return;
                   }
                  

               }
               else {
                   if (ddlBranch.SelectedIndex == 0 && ddlReseller.SelectedIndex == 0)
                   {
                       GetValue(context, datefrom, dateto, -1, 0);
                       return;
                   }
                   if (ddlBranch.SelectedIndex > 0 && ddlReseller.SelectedItem.Value == "0")
                   {
                       var branchId = Convert.ToInt32(ddlBranch.SelectedItem.Value);
                       GetValue(context, datefrom, dateto, 0, branchId);
                       return;
                   }
                   if (ddlBranch.SelectedIndex > 0 && ddlReseller.SelectedIndex > 0)
                   {
                       var resellerId = Convert.ToInt32(ddlReseller.SelectedItem.Value);
                       var branchId = Convert.ToInt32(ddlBranch.SelectedItem.Value);
                       GetValue(context, datefrom, dateto, resellerId, branchId);
                       return;
                   }

                   if (ddlReseller.SelectedItem.Value == "0")
                   {
                      
                       GetValue(context, datefrom, dateto, 0, 0);
                       return;
                   }
                  
                   if (ddlReseller.SelectedIndex > 0)
                   {
                       var resellerId = Convert.ToInt32(ddlReseller.SelectedItem.Value);
                       GetValue(context, datefrom, dateto, resellerId, 0);
                       return;
                   }
                   
                   
                   if (ddlBranch.SelectedIndex > 0)
                   {
                       var branchId = Convert.ToInt32(ddlBranch.SelectedItem.Value);
                       GetValue(context, datefrom, dateto, -1, branchId);
                       return;
                   }
                  
                  
               }

              

            }
        }

        private void GetValue(ISPDataContext context, DateTime datefrom, DateTime dateto, int resellerId, int branchId)
        {
            var data = context.Demands.Select(z => new
            {
                payDate = z.PaymentDate,
                Customer = z.WorkOrder.CustomerName,
                Phone = z.WorkOrder.CustomerPhone,
                Governorate = z.WorkOrder.Governorate.GovernorateName,
                Central = z.WorkOrder.Central.Name,
                CurrentServicePackageName = z.WorkOrder.ServicePackage.ServicePackageName,
                Status = z.WorkOrder.Status.StatusName,
                Provider = z.WorkOrder.ServiceProvider.SPName,
                Reseller = z.WorkOrder.User == null ? "-" : z.WorkOrder.User.UserName,
                Offer = z.WorkOrder.Offer.Title,
                Branch = z.WorkOrder.Branch.BranchName,
                TStartAt = z.StartAt,
                TEndAt = z.EndAt,
                TAmount = z.Amount,
                PaymentMethod = z.WorkOrder.PaymentType.PaymentTypeName,
                Address = z.WorkOrder.CustomerAddress,
                Mobile = z.WorkOrder.CustomerMobile,
                z.PaymentComment,
                notes = z.Notes,
                style = z.Notes == "غرامة الكانسل خلال السنة الأولى" ? "label-light" : "",
                  resellerId=z.WorkOrder.ResellerID,
                branchId=z.WorkOrder.BranchID
            }).ToList();
             if (branchId > 0 && resellerId > 0)
            {
                data = data.Where(z => z.notes == ddlrepType.Text && (z.TStartAt >= datefrom && z.TStartAt <= dateto) && z.branchId == branchId && z.resellerId == resellerId).ToList();
            }
             else if (branchId > 0 && resellerId == 0)
             {
                 data = data.Where(z => z.notes == ddlrepType.Text && (z.TStartAt >= datefrom && z.TStartAt <= dateto) && z.branchId == branchId && z.resellerId == null).ToList();
             }
             else if (resellerId > 0)
            {
                data = data.Where(z => z.notes == ddlrepType.Text && (z.TStartAt >= datefrom && z.TStartAt <= dateto) && z.resellerId == resellerId).ToList();

            }else if (resellerId == 0)
            {
                data = data.Where(z => z.notes == ddlrepType.Text && (z.TStartAt >= datefrom && z.TStartAt <= dateto) && z.resellerId == null).ToList();


            }else if (branchId > 0)
            {
                data = data.Where(z => z.notes == ddlrepType.Text && (z.TStartAt >= datefrom && z.TStartAt <= dateto) && z.branchId == branchId).ToList();
            }
            else
            {
                 data = data.Where(z => z.notes == ddlrepType.Text && (z.TStartAt >= datefrom && z.TStartAt <= dateto)).ToList();
            }
          
            grd_Requests.DataSource = data;
            grd_Requests.DataBind();
            var totaldem = data.Select(z => z.TAmount).ToList().Sum();
                //context.Demands.Where(
                //    z => z.Notes == ddlrepType.Text && (z.StartAt >= datefrom && z.StartAt <= dateto))
                //    .Select(z => z.Amount)
                //    .ToList()
                //    .Sum();
            total.Text = Tokens.Total + totaldem;
            if (data.Count != 0)
            {
                export_div.Visible = true;
            }
        }

        private void GetPayedDemand(ISPDataContext context, DateTime datefrom, DateTime dateto,int resellerId,int branchId)
        {
            var data = context.Demands.Select(z => new
            {
                payDate = z.PaymentDate,
                Customer = z.WorkOrder.CustomerName,
                Phone = z.WorkOrder.CustomerPhone,
                Governorate = z.WorkOrder.Governorate.GovernorateName,
                Central = z.WorkOrder.Central.Name,
                CurrentServicePackageName = z.WorkOrder.ServicePackage.ServicePackageName,
                Status = z.WorkOrder.Status.StatusName,
                Provider = z.WorkOrder.ServiceProvider.SPName,
                Reseller = z.WorkOrder.User == null ? "-" : z.WorkOrder.User.UserName,
                Offer = z.WorkOrder.Offer.Title,
                Branch = z.WorkOrder.Branch.BranchName,
                TStartAt = z.StartAt,
                TEndAt = z.EndAt,
                TAmount = z.Amount,
                PaymentMethod = z.WorkOrder.PaymentType.PaymentTypeName,
                Address = z.WorkOrder.CustomerAddress,
                Mobile = z.WorkOrder.CustomerMobile,
                z.PaymentComment,
                notes = z.Notes,
                style = z.Notes == "غرامة الكانسل خلال السنة الأولى" ? "label-light" : "",
                resellerId=z.WorkOrder.ResellerID,
                branchId=z.WorkOrder.BranchID
               
            }) .ToList();
            if (branchId > 0 && resellerId > 0)
            {
                data = data.Where(z => z.PaymentComment == "تم الدفع من خلال خدمة فورى" && (z.payDate >= datefrom && z.payDate <= dateto) && z.resellerId == resellerId && z.branchId == branchId)
                .ToList();
            }
            else if (branchId > 0 && resellerId == 0)
            {
                data = data.Where(z => z.PaymentComment == "تم الدفع من خلال خدمة فورى" && (z.payDate >= datefrom && z.payDate <= dateto) && z.resellerId == null && z.branchId == branchId)
             .ToList();
            }
            else if (resellerId > 0)
            {
                data = data.Where(z => z.PaymentComment == "تم الدفع من خلال خدمة فورى" && (z.payDate >= datefrom && z.payDate <= dateto) && z.resellerId == resellerId)
                .ToList();
            }else if (resellerId == 0)
            {
                data = data.Where(z => z.PaymentComment == "تم الدفع من خلال خدمة فورى" && (z.payDate >= datefrom && z.payDate <= dateto) && z.resellerId == null)
              .ToList();
            }
            else if (branchId > 0)
            {
                data =
                    data.Where(
                        z =>
                            z.PaymentComment == "تم الدفع من خلال خدمة فورى" &&
                            (z.payDate >= datefrom && z.payDate <= dateto) && z.branchId == branchId)
                        .ToList();
            }
            else
            {
                data =
                    data.Where(
                        z =>
                            z.PaymentComment == "تم الدفع من خلال خدمة فورى" &&
                            (z.payDate >= datefrom && z.payDate <= dateto))
                        .ToList();
            }

            grd_Requests.DataSource = data;
            grd_Requests.DataBind();
            var totaldem =data.Select(z => z.TAmount).ToList().Sum();
                //context.Demands.Where(
                //    z =>
                //        z.PaymentComment == "تم الدفع من خلال خدمة فورى" &&
                //        (z.PaymentDate.Value.Date >= datefrom && z.PaymentDate.Value.Date <= dateto)).Select(z => z.Amount)
                //    .ToList()
                //    .Sum();
            total.Text = Tokens.Total + totaldem;
            if (data.Count != 0)
            {
                export_div.Visible = true;
            }
        }


        protected void b_export_Click(object sender, EventArgs e)
        {
            // BranchPrintExcel = true;
            const string attachment = "attachment; filename=Suspend and Cancelled Outage Cost.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");

            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);
            grd_Requests.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }

    }
}