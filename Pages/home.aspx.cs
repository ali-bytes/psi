using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Db;
using System.Web.UI.WebControls;
using BL.Concrete;
using NewIspNL.Helpers;
using NewIspNL.Models;
using NewIspNL.Services.DemandServices;
using Resources;

namespace NewIspNL.Pages
{
    public partial class home :CustomPage  
    {
        public bool ShowCal { get; set; }
        public bool ShowOffers { get; set; }
        public bool ShowNotes { get; set; }
            public bool CanProcess { get; set; }

            protected void Page_Load(object sender, EventArgs e)
            {
                
                fill_popup();
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    if (Session["User_ID"] == null) Response.Redirect("default.aspx");
                    var userId = Convert.ToInt32(Session["User_ID"]);
                    var user = context.Users.FirstOrDefault(u => u.ID == userId);
                    var ispEntries = new IspEntries();

                    Activate();

                    var option = context.Options.FirstOrDefault();
                    var currentuser = user;
                    // counters for all users
                    if (option == null || currentuser == null || currentuser.GroupID == null) return;
                    //if user has privilage to see counters
                    var showStatistics = ispEntries.UserHasPrivlidge(user.ID, "StatisticsInHome");
                    if (showStatistics && option.ShowStatistic)
                    {
                        //Notifications1.Visible = homee.Visible = option.ShowStatistic;

                        ShowCal = true;
                        ShowOffers = true;
                        ShowNotes = true;
                        Notifications1.Visible = LastOffers.Visible =
                       homee.Visible = cal.Visible = Div1.Visible = option.ShowStatistic;
                       Fill_offers();
                       var canActive = ispEntries.UserHasPrivlidge(user.ID, "AddOffer.aspx");
                       addoffer.Visible = canActive;
                       Search();
                    }
                    username.Text = user.UserName;

                    CanProcess = user != null && user.GroupID != null && user.GroupID != 6;
                    if (IsPostBack) return;
                   
                    // hdnUserId.Value = Convert.ToString(Session["User_ID"]);
                }
               
            }
            void Search()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var notes =
                        context.WorkOrderNotes.Where(
                            x => x.ToUserId == Convert.ToInt32(Session["User_ID"]) && x.Processed == false);
                   
                    GvResults.DataSource = notes.Select(x => OrderNoteModel.To(x, context)).ToList(); 
                    GvResults.DataBind();
                }
            }
            public void fill_popup()
            {
                using (var context =
                    new ISPDataContext(
                        ConfigurationManager.AppSettings["ConnectionString"].ToString(CultureInfo.InvariantCulture)))
                {
                    var ti = context.News.Select(x => x.Title).FirstOrDefault();
                    adstitle.Text = ti;
                    var de = context.News.Select(x => x.Details).FirstOrDefault();
                    adsdetails.Text = de;
                }
            }
            public void Fill_offers()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {

                    var offers = context.OffersDetails.Select(z => new
                    {
                        z.Name,
                        Data = z.Data.Length > 150 ? Regex.Replace(z.Data.Substring(0, 150), @"<[^>]+>|&nbsp;", "").Trim() : Regex.Replace(z.Data, @"<[^>]+>|&nbsp;", "").Trim(),
                        ImageUrl = string.Format("~/_offerDetailsImages/{0}", z.ImageUrl),
                        z.Id

                    }).ToList();
                    DataList1.DataSource = offers.OrderByDescending(a=>a.Id);
                    DataList1.DataBind();
                }
            }

            public void Redirect(object sender, EventArgs e)
            {
                var stringId = ((LinkButton)sender).CommandArgument;
                var id = QueryStringSecurity.Encrypt(stringId);
                Response.Redirect("LastOffers.aspx?woid=" + id);
            }
            void Activate()
            {
                BProcessNote.ServerClick += (o, e) => Process(Convert.ToInt32(selected.Value));
                GvResults.DataBound += (o, e) => Helper.GridViewNumbering(GvResults, "LNo");
            }
            protected void Process(int id)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var _hintService = new OrderHintService(context);
                    var done = _hintService.Process(id, true, TbComment.Text);
                    Msg.InnerHtml = done ? Tokens.Saved : Tokens.SavingError;
                }
                Search();
            }
            protected void GvResults_RowDataBound(object sender, GridViewRowEventArgs e)
            {

            }
        }
    }
 