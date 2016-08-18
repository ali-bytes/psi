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
    public partial class Purchases : CustomPage
    {

    public IStoreAccounts Accounts;
    public IUserSaveRepository _UserSave;

    public Purchases()
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
                //if(id != null){
                var data = new InvoiceDetailsClass();
                if (id != null && id.Text != @"0")
                {
                    data.ItemId = Convert.ToInt32(id.ValidationGroup);
                }
                var label = item.FindControl("lblItemName") as Label;
                if (label != null) data.ItemName = label.Text;
                var label1 = item.FindControl("lblQuantity") as Label;
                if (label1 != null) data.Quantity =Convert.ToDecimal(label1.Text);
                var label2 = item.FindControl("lblPrice") as Label;
                if (label2 != null) data.Price =Convert.ToDecimal(label2.Text);
                list.Add(data);
            }
            return list;
        } 
        //set{}
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Activate();
        //var list = Invoice;
        
        if(IsPostBack)return;
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            Helper.AddTextBoxesText(new[] { txtTotal, txtDiscoundPercent, txtDiscound,txtNet,txtPaid,txtRemaining }, "0");
            PopulateSuppliers(context);
            PopulateStore(context);
            PopulateItems(context);
            var userId = Convert.ToInt32(Session["User_ID"]);
            PopulateSaves(userId,context);

        }
    }

    void PopulateSuppliers(ISPDataContext context)
    {
        var suppliers = context.Suppliers.ToList();
        ddlSuppliers.DataSource = suppliers;
        ddlSuppliers.DataBind();
        Helper.AddDefaultItem(ddlSuppliers);
    }

    void PopulateStore(ISPDataContext db)
    {
        var stores = db.Stores.ToList();
        ddlStore.DataSource = stores;
        ddlStore.DataBind();
        Helper.AddDefaultItem(ddlStore);
    }

    void PopulateSaves(int userId,ISPDataContext context)
    {
        ddlSaves.DataSource = _UserSave.SavesOfUser(userId, context).Select(a => new
        {
            a.Save.SaveName,
            a.Save.Id
        });
        ddlSaves.DataBind();
        Helper.AddDefaultItem(ddlSaves);
    }
    void PopulateItems(ISPDataContext context)
    {
        var items = context.Items.ToList();
        ddlItems.DataSource = items;
        ddlItems.DataBind();
        Helper.AddDefaultItem(ddlItems);
    }
    protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlItems.SelectedIndex != 0)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var index = Convert.ToInt32(ddlItems.SelectedItem.Value);
                var item = context.Items.FirstOrDefault(a => a.Id == index);
                if(item==null)return;
                txtPrice.Text = Helper.FixNumberFormat(item.PurchasPrice); //.ToString(CultureInfo.InvariantCulture);

                try
                {
                    Txtcode.Text = item.Code;
                }
                catch (Exception)
                {
                    Txtcode.Text = "";

                }
            }
        }
    }

    protected void AddOneInvoice(object sender, EventArgs e)
    {
        var details = new InvoiceDetailsClass
        {
            ItemId = Convert.ToInt32(ddlItems.SelectedItem.Value),
            ItemName = ddlItems.SelectedItem.Text,
            Price =Convert.ToDecimal(txtQuantity.Text)*Convert.ToDecimal(txtPrice.Text),
            Quantity = Convert.ToDecimal(txtQuantity.Text)
        };
        var l = new List<InvoiceDetailsClass>();
        if(Invoice!=null && Invoice.Count!=0)l.AddRange(Invoice);
        
        l.Add(details);
        GvInvoice.DataSource = l;//Invoice;
        GvInvoice.DataBind();
        txtNet.Text=txtTotal.Text =Helper.FixNumberFormat(l.Sum(a => a.Price));
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
                    txtTotal.Text =Helper.FixNumberFormat(list.Sum(a => a.Price));
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
        if (invoices.Count > 0)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {        var saveId = Convert.ToInt32(ddlSaves.SelectedItem.Value);
                var userId = Convert.ToInt32(Session["User_ID"]);
                var notesofsave = string.Format("رقم الفاتورة {0}", txtInvoiceNumber.Text);
                var added = _UserSave.BranchAndUserSaves(saveId, userId,(Convert.ToDouble(txtPaid.Text)*-1),
                    "فاتورة شراء جديدة من مورد", notesofsave, context);
                if (added.Equals(SaveResult.Saved))
                {
                    var storeId = Convert.ToInt32(ddlStore.SelectedItem.Value);
                    var supplierId = Convert.ToInt32(ddlSuppliers.SelectedItem.Value);
                    var now = Convert.ToDateTime(txtDate.Text);
                    var newinvoice = new InvoiceHeader
                    {
                        InvoiceNumber = txtInvoiceNumber.Text,
                        SupplierId = supplierId,
                        invoiceDate = now,
                        StoreId = storeId,
                        Total = Convert.ToDecimal(txtTotal.Text),
                        Discound = Convert.ToDecimal(txtDiscound.Text),
                        Net = Convert.ToDecimal(txtNet.Text),
                        Paid = Convert.ToDecimal(txtPaid.Text),
                        Remaining = Convert.ToDecimal(txtRemaining.Text),
                        TypeId = 1,
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
                            itemprice = Convert.ToDecimal(txtPrice.Text)
                        };
                        context.InvoiceDetails.InsertOnSubmit(newDetails);
                       
                        var storeTransaction =
                            context.StoreTransactions.OrderByDescending(a => a.Id)
                                .FirstOrDefault(a => a.ItemId == invoice.ItemId && a.StoreId == storeId);
                        var addTransaction = new StoreTransaction
                        {
                            ItemId = storeTransaction != null ? storeTransaction.ItemId : invoice.ItemId,
                            StoreId = storeTransaction != null ? storeTransaction.StoreId : storeId,
                            Quantity = invoice.Quantity,
                            Date = now,
                            Notes = string.Format("{0} - {1}", txtNotes.Text, "اضافة اصناف فى المخزن من صفحة المشتريات"),
                        };
                        context.StoreTransactions.InsertOnSubmit(addTransaction);
                        context.SubmitChanges();
                    }

                    if (newinvoice.Net > 0)
                    {
                        var note = string.Format("فاتورة رقم {0}", newinvoice.InvoiceNumber);
                        Accounts.AddInAccount(supplierId, null, Convert.ToDecimal(newinvoice.Net),
                            Convert.ToDecimal(newinvoice.Paid), now, "فاتورة جديدة من مورد", userId,
                            note, context);
                    }
                    ClearControls(this);
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

        }
    }
    void ClearControls(Control ctr)
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
                if (tmpctr.HasControls()) ClearControls(tmpctr);
                else
                {
                    var box = tmpctr as TextBox;
                    if (box != null) box.Text = string.Empty;
                }
            }
        }
        GvInvoice.DataSource = null;
        GvInvoice.DataBind();
    }
    protected void txtDiscoundPercent_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(txtTotal.Text) && !string.IsNullOrWhiteSpace(txtDiscoundPercent.Text))
        {
            var total = Convert.ToDecimal(txtTotal.Text);
            var percent =(total* Convert.ToDecimal(txtDiscoundPercent.Text))/100;
           var txtdis= txtDiscound.Text = percent.ToString(CultureInfo.InvariantCulture);
            txtNet.Text = Helper.FixNumberFormat(total - Convert.ToDecimal(txtdis)); 
        }
    }

    protected void txtPaid_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(txtPaid.Text)&&!string.IsNullOrWhiteSpace(txtNet.Text))
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
                txtPrice.Text = Helper.FixNumberFormat(item.PurchasPrice); 
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