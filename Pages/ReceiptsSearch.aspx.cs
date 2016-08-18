using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using Resources;

namespace NewIspNL.Pages
{
    public partial class ReceiptsSearch : CustomPage
    {
     
    protected void Page_Load(object sender, EventArgs e) {}


    protected void btn_Search_Click(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){

            var receipt = context.Receipts.FirstOrDefault(rec => rec.ID == Convert.ToDecimal(txt_ReceiptNo.Text));

            if(receipt != null){
                var demand = context.Demands.FirstOrDefault(x => x.Id == receipt.DemandId);
                if(demand != null){
                    fs_ReceiptDetails.Visible = true;
                    lbl_ReceiptAmount.Text = demand.Amount.ToString();
                    lbl_ReceiptDate.Text = demand.PaymentDate.ToString();
                    lbl_ReceiptNo.Text = receipt.ID.ToString();
                    lbl_ReceiptNotes.Text = demand.PaymentComment;
                    var order = demand.WorkOrder;
                    lbl_customerphone.Text = order == null ? "-" : order.CustomerPhone;
                    lbl_customername.Text = order == null ? "-" : order.CustomerName;
                    lbl_username.Text = demand.User.UserName;
                    lbl_Message.Text = string.Empty;
                } else{
                    lbl_Message.Text = Tokens.NoResults;
                    lbl_Message.ForeColor = Color.Red;
                    fs_ReceiptDetails.Visible = false;
                    lbl_ReceiptAmount.Text =
                        lbl_ReceiptDate.Text =
                            lbl_ReceiptNo.Text =
                                lbl_ReceiptNotes.Text =
                                    lbl_customerphone.Text = string.Empty;
                }

            } else{
                lbl_Message.Text = Tokens.NoResults;
                lbl_Message.ForeColor = Color.Red;
                fs_ReceiptDetails.Visible = false;
                lbl_ReceiptAmount.Text =
                    lbl_ReceiptDate.Text =
                        lbl_ReceiptNo.Text =
                            lbl_ReceiptNotes.Text =
                                lbl_customerphone.Text = string.Empty;
            }
        }
    }
}
}