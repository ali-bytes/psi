using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
    public partial class BoxCredit : CustomPage
    {

        readonly IBoxCreditRepository _creditRepository = new BoxCreditRepository();
        //static bool _branchPrintExcel;
        /*readonly ISPDataContext _context;


        public Pages_BoxCredit(){
            _context = IspDataContext;
        }*/


        void Populatebox()
        {
            using (var db = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {

                var boexs = db.Boxes.ToList(); //_creditRepository.GetResellersUponUserGroupWithCredit(Convert.ToInt32(Session["User_ID"]));
                DdlBox.DataSource = boexs;
                DdlBox.DataTextField = "BoxName";
                DdlBox.DataValueField = "ID";
                DdlBox.DataBind();
                Helper.AddAllDefaultItem(DdlBox);
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            Populatebox();
        }


        protected void BSave_Click(object sender, EventArgs e)
        {
            var effect = RblEffect.SelectedIndex;
            int boxId = Convert.ToInt32(DdlBox.SelectedItem.Value);
            int userId = Convert.ToInt32(Session["User_ID"]);
            decimal amount = effect == 0 ? Convert.ToDecimal(TbAmount.Text) : Convert.ToDecimal(TbAmount.Text) * -1;
            string notes = "عملية من صفحة حركة صندوق" + " " + TbNotes.Text;
            DateTime time = DateTime.Now.AddHours();
            var result = _creditRepository.SaveBox(boxId, userId, amount, notes, time);
            switch (result)
            {
                case SaveBoxResult.Saved:
                    Message.Text = Tokens.Saved;
                    Clear();
                    Populatebox();
                    break;
                case SaveBoxResult.NoCredit:
                    Message.Text = Tokens.NotEnoughtCreditMsg;
                    break;
            }
           

        }


        void Clear()
        {
            TbAmount.Text = TbNotes.Text = string.Empty;
        }


       
        void getLastAmount(int boxId)
        {
            var history = _creditRepository.BoxCredits(boxId);
            var amo = history.OrderByDescending(x => x.Time).Select(x => new
            {
                Amount = Helper.FixNumberFormat(x.Amount),
                Net = Helper.FixNumberFormat(x.Net),
                Box = x.Box.BoxName,
                //User = x.User1.UserName,
                Date = string.Format("{0:d}", x.Time),
                x.Notes,
                Type = x.Amount < 0 ? Tokens.Subtract : Tokens.Add,
                //RecieptUrl = string.Format("/Pages/ResellerPaymentReciept.aspx?id={0}", x.Id)
            }).FirstOrDefault();

            if (amo==null)
            {
                return;
            }
            Msg.InnerHtml = "رصيد الصندوق : " + amo.Net.ToString();
            Msg.Attributes.Add("class", "alert alert-info");
           
        }

        protected void DdlReseller_SelectedIndexChanged(object sender, EventArgs e)
        {
            var id = string.IsNullOrEmpty(DdlBox.SelectedItem.Value) ? 0 : Convert.ToInt32(DdlBox.SelectedItem.Value);
            if (id == 0)
            {
                return;
            }
            getLastAmount(id);
           

        }


        protected void GvHistory_DataBound(object sender, EventArgs e)
        {
            Helper.GridViewNumbering(GvHistory, "no");
        }
        protected void BtnExport_Click(object sender, EventArgs e)
        {
            //_branchPrintExcel = true;
            const string attachment = "attachment; filename=BoxCredit.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");

            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);
            GvHistory.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
    }
}
