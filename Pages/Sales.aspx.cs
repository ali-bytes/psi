using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
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
    public partial class Sales : CustomPage
    {
     
    public IStoreAccounts Accounts;
    public IUserSaveRepository _UserSave;

    public Sales()
    {
        Accounts=new StoreAccounts();
        _UserSave=new UserSaveRepository();
    }
    public List<InvoiceDetailsClass> Invoice
    {
        get
        {
            var list = new List<InvoiceDetailsClass>();
            foreach (GridViewRow item in GvInvoice.Rows)
            {
                var id = item.FindControl("BDelete") as LinkButton;
               
                var data = new InvoiceDetailsClass();
                if (id != null && id.Text != @"0")
                {
                    data.ItemId = Convert.ToInt32(id.ValidationGroup);
                }
                var label = item.FindControl("lblItemName") as Label;
                if (label != null) data.ItemName = label.Text;
                var label1 = item.FindControl("lblQuantity") as Label;
                if (label1 != null) data.Quantity = Convert.ToDecimal(label1.Text);
                var label2 = item.FindControl("lblPrice") as Label;
                if (label2 != null) data.Price = Convert.ToDecimal(label2.Text);
                var ItemPrice = item.FindControl("lblItemPrice") as Label;
                if (ItemPrice != null) data.ItemPrice = ItemPrice.Text;
                list.Add(data);
            }
            return list;
        }
       
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        halfA4.Visible = false;
        Reciept.Visible = false;
        Activate();
       
          using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {      
            var lastid = context.InvoiceHeaders.OrderByDescending(a => a.Id).FirstOrDefault();
            if(lastid!=null)txtInvoiceNumber.Text = (Convert.ToInt32(lastid.Id) + 1).ToString(CultureInfo.InvariantCulture);
            
            if (IsPostBack) return;
            Helper.AddTextBoxesText(new[] { txtTotal, txtDiscoundPercent, txtDiscound, txtNet, txtPaid, txtRemaining }, "0");
            PopulateCustomers(context);
            PopulateStore(context);
         
            var userId = Convert.ToInt32(Session["User_ID"]);
            PopulateSaves(userId, context);

        }
    }

    void PopulateCustomers(ISPDataContext context)
    {
        var customers = context.Customers.ToList();
        ddlCustomer.DataSource = customers;
        ddlCustomer.DataValueField = "id";
        ddlCustomer.DataTextField = "CustomerName";
        ddlCustomer.DataBind();
        Helper.AddDefaultItem(ddlCustomer);
    }

    void PopulateStore(ISPDataContext db)
    {
        var stores = db.Stores.ToList();
        ddlStore.DataSource = stores;
        ddlStore.DataBind();
        Helper.AddDefaultItem(ddlStore);
    }
    void PopulateSaves(int userId, ISPDataContext context)
    {
        ddlSaves.DataSource = _UserSave.SavesOfUser(userId, context).Select(a => new
        {
            a.Save.SaveName,
            a.Save.Id
        });
        ddlSaves.DataBind();
        Helper.AddDefaultItem(ddlSaves);
    }

    protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlStore.SelectedIndex != 0)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var storeId = Convert.ToInt32(ddlStore.SelectedItem.Value);
                var allitemInStore =
                    context.StoreTransactions.Where(a => a.StoreId == storeId && a.Quantity > 0).Select(a => new
                    {
                        a.Item.ItemName,
                        a.ItemId
                    }).ToList();
                ddlItems.DataSource = allitemInStore.Distinct().ToList();
                ddlItems.DataBind();
                Helper.AddDefaultItem(ddlItems);
            }
        }
    }

   
    protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlItems.SelectedIndex != 0)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var index = Convert.ToInt32(ddlItems.SelectedItem.Value);
                var item = context.Items.FirstOrDefault(a => a.Id == index);
                if (item == null) return;
                txtPrice.Text = Helper.FixNumberFormat(item.SellPrice);
          
       

        try
        {
            Txtcode.Text = item.Code;
        }
        catch (Exception)
        {
            Txtcode.Text = "";
          
        }
 }  }

    }

    protected void AddOneInvoice(object sender, EventArgs e)
    {
        var details = new InvoiceDetailsClass
        {
            ItemId = Convert.ToInt32(ddlItems.SelectedItem.Value),
            ItemName = ddlItems.SelectedItem.Text,
            ItemPrice = txtPrice.Text,
            Price = Convert.ToDecimal(txtQuantity.Text) * Convert.ToDecimal(txtPrice.Text),
            Quantity = Convert.ToDecimal(txtQuantity.Text)
        };
        errorDiv.Visible = false;
        lblerror.Text = "";

        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var transaction =
                              context.StoreTransactions.OrderByDescending(a => a.Id)
                                  .Where(a => a.ItemId == details.ItemId && a.StoreId == Convert.ToInt32(ddlStore.SelectedItem.Value));
            var storeTransaction = transaction.FirstOrDefault();

            var itemQuantity = transaction.Sum(a => a.Quantity);

           decimal currentQ= Invoice.Where(x => x.ItemId == details.ItemId).ToList().Sum(a => a.Quantity);

           if (storeTransaction == null || itemQuantity < (details.Quantity + currentQ))
            {
                errorDiv.Visible = true;
                lblerror.Text =
                    string.Format(
                        " هذا الصنف {0} غير موجود فى المخزن أو كميته فى المخزن غير كافية لاتمام العملية ",
                        details.ItemName);
                return;

            }
        }

        var l = new List<InvoiceDetailsClass>();
        if (Invoice != null && Invoice.Count != 0) l.AddRange(Invoice);
        l.Add(details);
        GvInvoice.DataSource = l;
        GvInvoice.DataBind();
        txtNet.Text=txtTotal.Text = Helper.FixNumberFormat(l.Sum(a => a.Price));
    }

    private void Activate()
    {
        GvInvoice.DataBound += (o, e) => Helper.GridViewNumbering(GvInvoice, "LNo");
    }
    protected void DeleteInvoice(object sender, EventArgs e)
    {
        try
        {
           
            {
                var btn = sender as LinkButton;
                if (btn == null) return;
                var id = Convert.ToInt32(btn.ValidationGroup);
                var list = Invoice;
                foreach (InvoiceDetailsClass item in list)
                {
                    if (item.ItemId == id)
                    {
                        list.Remove(item);
                        break;
                       
                    }
                }
                GvInvoice.DataSource = list;
                GvInvoice.DataBind();
                txtTotal.Text = Helper.FixNumberFormat(list.Sum(a => a.Price));
            }
        }
        catch (Exception)
        {
            errorDiv.Visible = true;
            SuccDiv.Visible = false;
        }
    }
    protected void btnAddAllInvoices_Click(object sender, EventArgs e)
    {

        var invoices = Invoice;
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            if (invoices.Count > 0)
            {
//todo
                Session["invid"] = txtInvoiceNumber.Text;

                var saveId = Convert.ToInt32(ddlSaves.SelectedItem.Value);
                var userId = Convert.ToInt32(Session["User_ID"]);
                var notesofsave = string.Format("رقم الفاتورة {0}", txtInvoiceNumber.Text);
                var added = _UserSave.BranchAndUserSaves(saveId, userId, Convert.ToDouble(txtPaid.Text),
                    "فاتورة بيع جديدة", notesofsave, context);
                if (added.Equals(SaveResult.Saved))
                {
                    var storeId = Convert.ToInt32(ddlStore.SelectedItem.Value);
                    var customerId = Convert.ToInt32(ddlCustomer.SelectedItem.Value);
                    var now = Convert.ToDateTime(txtDate.Text);
                    var newinvoice = new InvoiceHeader
                    {
                        InvoiceNumber = txtInvoiceNumber.Text,
                        CustomerId = customerId,
                        invoiceDate = now,
                        StoreId = storeId,
                        Total = Convert.ToDecimal(txtTotal.Text),
                        Discound = Convert.ToDecimal(txtDiscound.Text),
                        Net = Convert.ToDecimal(txtNet.Text),
                        Paid = Convert.ToDecimal(txtPaid.Text),
                        Remaining = Convert.ToDecimal(txtRemaining.Text),
                        TypeId = 2,
                        Notes = txtNotes.Text
                    };
                    context.InvoiceHeaders.InsertOnSubmit(newinvoice);
                    context.SubmitChanges();
                    var headId = newinvoice.Id;
                    if (headId == 0) return;
                    foreach (var invoice in invoices)
                    {
                        var newDetails = new InvoiceDetail
                        {
                            HeadId = headId,
                            ItemId = invoice.ItemId,
                            Quantity = invoice.Quantity,
                            //todo
                            itemprice = invoice.ItemPrice != null ? Convert.ToDecimal(invoice.ItemPrice) : default(decimal)
                        };
                        context.InvoiceDetails.InsertOnSubmit(newDetails);
                       
                        InvoiceDetailsClass invoice1 = invoice;
                        var transaction =
                            context.StoreTransactions.OrderByDescending(a => a.Id)
                                .Where(a => a.ItemId == invoice1.ItemId && a.StoreId == storeId);
                        var storeTransaction = transaction.FirstOrDefault();
                      
                        var itemQuantity = transaction.Sum(a => a.Quantity);
                        var diffrent = itemQuantity - invoice.Quantity;
                        if (storeTransaction == null || itemQuantity < 0 || diffrent < 0)
                        {
                            errorDiv.Visible = true;
                            lblerror.Text =
                                string.Format(
                                    " هذا الصنف {0} غير موجود فى المخزن أو كميته فى المخزن غير كافية لاتمام العملية ",
                                    invoice.ItemName);
                            continue;
                        }
                        var addTransaction = new StoreTransaction
                        {
                            ItemId = storeTransaction.ItemId,
                            StoreId = storeTransaction.StoreId,
                            Quantity = invoice.Quantity*-1, 
                            Date = now,
                            Notes = string.Format("{0} - {1}", txtNotes.Text, "اخراج اصناف من المخزن من صفحة المبيعات"),
                        };
                        context.StoreTransactions.InsertOnSubmit(addTransaction);
                        context.SubmitChanges();
                    }

                    if (newinvoice.Net > 0)
                    {
                        var note = string.Format("فاتورة رقم {0}", newinvoice.InvoiceNumber);
                        Accounts.AddInAccount(null, customerId, Convert.ToDecimal(newinvoice.Net),
                            Convert.ToDecimal(newinvoice.Paid), now, "فاتورة جديدة من عملية بيع", userId,
                            note, context);
                    }
                    ClearControls(this, context);
                    SuccDiv.Visible = true;
                    errorDiv.Visible = false;
                }
                else
                {
                    SuccDiv.Visible = false;
                    errorDiv.Visible = true;
                    lblerror.Text = Tokens.CreditIsntEnough;
                }
            }


            //todo:modify
            var check = context.Options.Select(x => x.WidthOfReciept).FirstOrDefault();
            if (check == true)
            {
                Reciept.Visible = true;
                Recipet(context);
            }
            else
            {


                halfA4.Visible = true;
                FillHalfA4Div(context);
            }
        }
    }

    private void FillHalfA4Div(ISPDataContext db)
    {
     
        {
            var invid = Convert.ToInt32(Session["invid"]);
            var invv =
                (from g in db.InvoiceDetails where g.InvoiceHeader.InvoiceNumber == invid.ToString() select g)
                    .FirstOrDefault();
            var userId = Convert.ToInt32(Session["User_ID"]);
            var username = db.Users.Where(x => x.ID == userId).Select(x => x.UserName).FirstOrDefault();
            user.Text = username;


            cusname.Text = ddlCustomer.SelectedItem.ToString();
            date.Text = invv.InvoiceHeader.invoiceDate.Value.ToShortDateString();
            comp.Text = db.ReceiptCnfgs.Select(x => x.CompanyName).FirstOrDefault();
            Image1.ImageUrl = "../PrintLogos/" + db.ReceiptCnfgs.Select(x => x.LogoUrl).FirstOrDefault();
            billnum.Text = invid.ToString();

            total.Text = invv.InvoiceHeader.Total.Value.ToString("#.##");
            dis.Text = invv.InvoiceHeader.Discound.Value.ToString("#.##");
            totalafterdis.Text = invv.InvoiceHeader.Net.Value.ToString("#.##");
            note.Text = invv.InvoiceHeader.Notes;
            repaid.Text = invv.InvoiceHeader.Paid.Value.ToString("#.##");
            var inv = invid.ToString();
            var griddata = (from g in db.InvoiceDetails
                where g.InvoiceHeader.InvoiceNumber == inv
                select new
                {

                    g.Item.ItemName,
                    g.Quantity,
                    g.itemprice,
                    total = g.Quantity*g.itemprice.Value


                }).ToList();

            GvCustomerData.DataSource = griddata;
            GvCustomerData.DataBind();
        }
    }

    //todo
    private void Recipet(ISPDataContext db)
    {
        var invid2 = Convert.ToInt32(Session["invid"]);
        var invv2 = (from g in db.InvoiceDetails where g.InvoiceHeader.InvoiceNumber == invid2.ToString() select g).FirstOrDefault();
        var userId2 = Convert.ToInt32(Session["User_ID"]);
        var username2 = db.Users.Where(x => x.ID == userId2).Select(x => x.UserName).FirstOrDefault();
        user2.Text = username2;


        cusname2.Text = ddlCustomer.SelectedItem.ToString();
        date2.Text = invv2.InvoiceHeader.invoiceDate.Value.ToShortDateString();
        comp2.Text = db.ReceiptCnfgs.Select(x => x.CompanyName).FirstOrDefault();
        Image2.ImageUrl ="../PrintLogos/"+ db.ReceiptCnfgs.Select(x => x.LogoUrl).FirstOrDefault();
        billnum2.Text = invid2.ToString();

        total2.Text = invv2.InvoiceHeader.Total.Value.ToString("#.##");
        dis2.Text = invv2.InvoiceHeader.Discound.Value.ToString("#.##");
        totalafterdis2.Text = invv2.InvoiceHeader.Net.Value.ToString("#.##");
        note2.Text = invv2.InvoiceHeader.Notes;
        repaid2.Text = invv2.InvoiceHeader.Paid.Value.ToString("#.##");
        var inv2 = invid2.ToString();
        var griddata2 = (from g in db.InvoiceDetails
                         where g.InvoiceHeader.InvoiceNumber == inv2
                         select new
                         {

                             g.Item.ItemName,
                             g.Quantity,
                             g.itemprice,
                             total =g.itemprice.Value


                         }).ToList();

        GridView1.DataSource = griddata2;
        GridView1.DataBind();
    }
    void ClearControls(Control ctr,ISPDataContext db)
    {
        if (!ctr.HasControls())
        {
            var box = ctr as TextBox;
            if (box != null) box.Text = string.Empty;
        }
        else
        {
            foreach (Control tmpctr in ctr.Controls)
            {
                if (tmpctr.HasControls()) ClearControls(tmpctr,db);
                else
                {
                    var box = tmpctr as TextBox;
                    if (box != null) box.Text = string.Empty;
                }
            }
        }
        GvInvoice.DataSource = null;
        GvInvoice.DataBind();

            var lastid = db.InvoiceHeaders.OrderByDescending(a => a.Id).FirstOrDefault();
            if (lastid != null)
                txtInvoiceNumber.Text = (Convert.ToInt32(lastid.Id) + 1).ToString(CultureInfo.InvariantCulture);
    }
    protected void txtDiscoundPercent_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(txtTotal.Text) && !string.IsNullOrWhiteSpace(txtDiscoundPercent.Text))
        {
            var total = Convert.ToDecimal(txtTotal.Text);
            var percent = (total * Convert.ToDecimal(txtDiscoundPercent.Text)) / 100;
            var txtdis = txtDiscound.Text = percent.ToString(CultureInfo.InvariantCulture);
            txtNet.Text = Helper.FixNumberFormat(total - Convert.ToDecimal(txtdis)); 
        }
    }

    protected void txtPaid_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(txtPaid.Text) && !string.IsNullOrWhiteSpace(txtNet.Text))
        {
            var net = Convert.ToDecimal(txtNet.Text);
            var paid = Convert.ToDecimal(txtPaid.Text);
            txtRemaining.Text = net > paid ? Helper.FixNumberFormat(net - paid) : "0";

        }
    }

        protected void GetItem(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var code = Txtcode.Text;
                var item = context.Items.FirstOrDefault(x => x.Code == code);
                try
                {
                    if (item == null) return;
                    txtPrice.Text = Helper.FixNumberFormat(item.SellPrice);
                    ddlItems.SelectedValue = item.Id.ToString();
                }
                catch (Exception)
                {
                    Txtcode.Text = "";
                }
            }

        }
    }
}