using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class ShowCards : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_ID"] == null)
            {
                Response.Redirect("~/Pages/default.aspx");
                return;
            }
            Activate();
            Cards();
        }
        void Activate()
        {
            GvitemData.DataBound += (o, e) => Helper.GridViewNumbering(GvitemData, "LNo");

        }
        public void Cards()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var cards = context.DisCards.Where(z => z.Status != true && z.WorkOrder == null).Select(x => new
                {
                    x.DisType.TypeName,
                    x.ID,
                    x.DisType.Price,


                }).ToList();
                GvitemData.DataSource = cards;

                GvitemData.DataBind();
            }
        }

        [WebMethod]
        public static string InsertData(string phone ,string id)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var rid =Convert.ToInt32(id);
                string retMessage = string.Empty;
                var check = context.WorkOrders.Where(z => z.CustomerPhone == phone).Select(z => z).FirstOrDefault();
                if (check == null)
                {
                   return retMessage = null;
                }
                else
                {
                    var ckeckcard =
                        context.DisCards.Where(x => x.WorkOrder.CustomerPhone == phone).Select(z => z).FirstOrDefault();
                    if (ckeckcard == null)
                    {
                      retMessage = check.CustomerName;

                      var unPDemads = context.Demands.Where(x => x.Paid == false).OrderByDescending(a => a.EndAt).ToList();
                      if (unPDemads.Count == 0)
                      {
                          retMessage = "1";
                      }
                      else
                      {

                          ShowCards d = new ShowCards();
                          var p = d.Check1(phone, unPDemads, rid);
                          if (p == 0)
                          {
                              retMessage = "2";
                          }
                      }
                       
                    }
                    else
                    {
                        retMessage = "";
                    }
                }
               
                return retMessage;
            }


        }

        private decimal Check1(string phone,List<Demand> unPDemads,int id)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {

                //var id = Convert.ToInt32(cardid.Value);
                var workor =
                    context.WorkOrders.Where(z => z.CustomerPhone == phone).Select(z => z.ID).FirstOrDefault();
                var card = context.DisCards.Where(x => x.ID == id).Select(z => z).FirstOrDefault();
                var cardPrice = context.DisTypes.Where(x => x.ID == card.DisTypeID).Select(z => z).FirstOrDefault();

                var price= cardPrice!=null ? cardPrice.Price??0 : 0;
                if (unPDemads.Count == 1)
                {
                    var dm = unPDemads.FirstOrDefault();
                    if (dm.Amount < price)
                    {
                        return 0;
                    }
                    
                }
                else if (unPDemads.Count > 1)
                {
                    var dm = unPDemads.OrderByDescending(x => x.Amount).FirstOrDefault();
                    if (dm.Amount < price)
                    {
                        return 0;
                    }
                }

                  return 1;
            }
        }
       
        protected void Button1_Click(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {

                var userid = Convert.ToInt32(Session["User_ID"]);
                var id = Convert.ToInt32(cardid.Value);
                var workor = context.WorkOrders.Where(z => z.CustomerPhone == txtphone.Text).Select(z => z.ID).FirstOrDefault();
                var card = context.DisCards.Where(x => x.ID == id).Select(z => z).FirstOrDefault();

                card.Status = true;
                card.WorkOrderID = workor;
                card.user_id = userid;
                card.op_date = DateTime.Now.AddHours();
                context.SubmitChanges();
                // gggggggggggggggggggggggggggggggg
                var unPDemads = context.Demands.Where(x => x.Paid == false && x.WorkOrderId == workor).OrderByDescending(a => a.EndAt).ToList();
                var cardPrice = context.DisTypes.Where(x => x.ID == card.DisTypeID).Select(z => z).FirstOrDefault();

                if (unPDemads.Count == 1)
                {
                    var dm = unPDemads.FirstOrDefault();
                    if (dm != null && cardPrice != null && dm.Amount >= cardPrice.Price)
                    {
                       /* var dem = context.Demands.FirstOrDefault(x => x.id == dm.Id);*/

                        dm.Amount = dm.Amount - cardPrice.Price ?? 0;
                        dm.Notes += "كارت خصم رقم : " + card.ID.ToString();
                        context.SubmitChanges();
                    }

                }
                else if (unPDemads.Count > 1)
                {
                    var dm = unPDemads.OrderByDescending(x => x.Amount).FirstOrDefault();
                    if (dm != null && cardPrice != null && dm.Amount >= cardPrice.Price)
                    {
                        dm.Amount = dm.Amount - cardPrice.Price ?? 0;
                        dm.Notes += "كارت خصم رقم : " + card.ID.ToString();
                        context.SubmitChanges();
                    }
                }



                Cards();


            }


        }
  

    }
}