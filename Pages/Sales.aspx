<%@ Page Title="<%$Resources:Tokens,Sales %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="Sales.aspx.cs" Inherits="NewIspNL.Pages.Sales" %>

<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style type="text/css">
    #btnAddAllInvoices {
        margin-bottom: 10px;
    }
</style>
    <div class="page-header">
        <h1><asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,Sales %>"></asp:Literal></h1>
    </div>
    
    <div class="row">

<div class="col-md-12 well">
            <div class="col-md-6">
                <asp:Literal runat="server" ID="lblNumber" Text="<%$Resources:Tokens,InvoiceNumber %>"></asp:Literal>
                <div>
                    <asp:TextBox runat="server" ID="txtInvoiceNumber" Enabled="False"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtInvoiceNumber" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="allinvoice"></asp:RequiredFieldValidator>
                </div>
                <asp:Literal runat="server" ID="lblsupplier" Text="<%$Resources:Tokens,Customer %>"></asp:Literal>
                <div>
                    <asp:DropDownList runat="server" ID="ddlCustomer" DataTextField="CustomerName" DataValueField="Id"/>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlCustomer" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="allinvoice"></asp:RequiredFieldValidator>
                </div>
                                                <asp:Literal runat="server" ID="Literal7" Text="<%$Resources:Tokens,UserSaves %>"></asp:Literal>
                <div>
                    <asp:DropDownList runat="server" ID="ddlSaves" DataTextField="SaveName" DataValueField="Id"/>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="ddlSaves" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="allinvoice"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-md-6">
                <asp:Literal runat="server" ID="lblDate" Text="<%$Resources:Tokens,Date %>"></asp:Literal>
                <div>
                    <asp:TextBox runat="server" ID="txtDate" data-x="dt"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDate" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="allinvoice"></asp:RequiredFieldValidator>
                </div>
                <asp:Literal runat="server" ID="lblStore" Text="<%$Resources:Tokens,Store %>"></asp:Literal>
                <div>
                               <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
                    <asp:DropDownList runat="server" ID="ddlStore" DataTextField="StoreName" AutoPostBack="True"
                        DataValueField="Id" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlStore" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="allinvoice"></asp:RequiredFieldValidator>
                        </ContentTemplate></asp:UpdatePanel>
                </div>
            </div>
        </div>
           <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
        <div class="col-md-12 well">
            <div class="col-md-6" style="margin-bottom: 10px;">
                
             <asp:Literal ID="Literal8"  runat="server" Text="<%$Resources:Tokens,Code %>"></asp:Literal>
           
                   <div>
                    <asp:TextBox OnTextChanged="GetItem" AutoPostBack="True" runat="server" ID="Txtcode" CssClass="required-input"></asp:TextBox>
                    </div>
            <asp:Literal runat="server" ID="lblItems" Text="<%$Resources:Tokens,ItemName %>"></asp:Literal>
            <div>
                <asp:DropDownList runat="server" ID="ddlItems" DataTextField="ItemName" DataValueField="ItemId" AutoPostBack="True"
                OnSelectedIndexChanged="ddlItem_SelectedIndexChanged"/>
            </div>
            <asp:Literal runat="server" ID="lblQuantity" Text="<%$Resources:Tokens,Quantity %>"></asp:Literal>
            <div>
                <asp:TextBox runat="server" ID="txtQuantity"></asp:TextBox>
            </div>
            </div>
            <div class="col-md-6">
                <asp:Literal runat="server" ID="lblPrice" Text="<%$Resources:Tokens,Price %>"></asp:Literal>
                <div>
                    <asp:TextBox runat="server" ID="txtPrice"></asp:TextBox>
                </div>
            </div>
            <br/>
           <div class="col-md-12">
               <asp:Button runat="server" ID="btnAddoneInvoice" Text="<%$Resources:Tokens,Save %>" CssClass="btn btn-success" OnClick="AddOneInvoice"/>
           </div>
        </div>
        <asp:GridView runat="server" ID="GvInvoice" ClientIDMode="Static" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed text-center col-md-12 col-sm-12">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LNo" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,ItemName %>">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblItemName" Text='<%#Eval("ItemName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$Resources:Tokens,Quantity %>">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblQuantity" Text='<%#Eval("Quantity") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$Resources:Tokens,ItemPrice %>">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblItemPrice" Text='<%#Eval("ItemPrice") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="<%$Resources:Tokens,Total %>">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblPrice" Text='<%#Eval("Price") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ValidationGroup='<%#Eval("ItemId") %>'  data-rel="tooltip" 
                                        ToolTip="<%$Resources:Tokens,Delete %>" OnClick="DeleteInvoice" ID="BDelete" CssClass="btn btn-xs btn-danger"><i class="icon-trash bigger-120"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>
                        </asp:GridView>
        <div class="col-md-12 well" >
            <div class="col-md-6">
                <asp:Literal runat="server" ID="lblTotal" Text="<%$Resources:Tokens,Total %>"></asp:Literal>
                <div>
                    <asp:TextBox runat="server" ID="txtTotal"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="rquTotal" ControlToValidate="txtTotal" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="allinvoice"></asp:RequiredFieldValidator>
                </div>
                                <asp:Literal runat="server" ID="Literal12" Text="<%$Resources:Tokens,DiscountPercent %>"></asp:Literal>
                <div>
                    
                    <asp:TextBox runat="server" ID="txtDiscoundPercent" 
                        OnTextChanged="txtDiscoundPercent_TextChanged" AutoPostBack="True"></asp:TextBox>&nbsp;<i>%</i>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtDiscoundPercent" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="allinvoice"></asp:RequiredFieldValidator>
                </div>
                                <asp:Literal runat="server" ID="Literal3" Text="<%$Resources:Tokens,Discount %>"></asp:Literal>
                <div>
                    <asp:TextBox runat="server" ID="txtDiscound"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtDiscound" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="allinvoice"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-md-6">
                <asp:Literal runat="server" ID="Literal4" Text="<%$Resources:Tokens,Net %>"></asp:Literal>
                <div>
                    <asp:TextBox runat="server" ID="txtNet"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ControlToValidate="txtNet" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="allinvoice"></asp:RequiredFieldValidator>
                </div>
                                <asp:Literal runat="server" ID="Literal5" Text="<%$Resources:Tokens,Paid %>"></asp:Literal>
                <div>
                    <asp:TextBox runat="server" ID="txtPaid" OnTextChanged="txtPaid_TextChanged" AutoPostBack="True"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtPaid" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="allinvoice"></asp:RequiredFieldValidator>
                </div>
                                <asp:Literal runat="server" ID="Literal6" Text="<%$Resources:Tokens,Remaining %>"></asp:Literal>
                <div>
                    <asp:TextBox runat="server" ID="txtRemaining"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtRemaining" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="allinvoice"></asp:RequiredFieldValidator>
                </div>
            </div>
            <asp:Literal runat="server" ID="lblNotes" Text="<%$Resources:Tokens,Notes %>"></asp:Literal>
            <div>
                <asp:TextBox runat="server" TextMode="MultiLine" ID="txtNotes" Width="100%"></asp:TextBox>
            </div>
        </div>
         
                        <%--OnClientClick="return confirm('هل انت متأكد من اتمام العملية');"--%>

                        <asp:LinkButton runat="server" ValidationGroup="allinvoice" 
            ID="btnAddAllInvoices" CssClass="btn btn-primary btn-block" ClientIDMode="Static" 
            onclick="btnAddAllInvoices_Click"><i class="icon-ok"></i>&nbsp;<%=Tokens.Save %></asp:LinkButton>     
            <br/>                                                           <div class="alert alert-success" runat="server" id="SuccDiv" Visible="False">
                            <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,ProcessDone %>"></asp:Literal>
                        </div>
                        <div class="alert alert-danger" runat="server" id="errorDiv" Visible="False">
                            <asp:Literal ID="lblerror" runat="server" Text="<%$Resources:Tokens,ErrorMsg %>"></asp:Literal>
                        </div>
                        
        </ContentTemplate>
        </asp:UpdatePanel></div>
            <div class="view" runat="server" visible="False" id="halfA4">
         
                   
                    <div id="MyDiv1" dir="rtl">
                        <table style="width: 100%; height: 150px;" dir="rtl">
                            <tr>
                                <td class="style2">
                                    <strong>اسم الشركة /<asp:Label ID="comp" runat="server"></asp:Label>
                                    </strong></td>
                                <td class="style3" rowspan="4">
                                    <asp:Image ID="Image1" runat="server" Height="123px" Width="153px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                    <strong>التاريخ / </strong>
                                    <asp:Label ID="date" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                    <strong>المستخدم/</strong><asp:Label ID="user" runat="server" 
                                        Style="font-size: large"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                    <strong>اسم العميل/</strong><asp:Label ID="cusname" runat="server" 
                                        Style="font-size: large"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="style6" colspan="2" style="text-align: center">
                                    <div style="text-align: center;margin-right: 100px">
                                    <strong>فاتورة مبيعات رقم /<asp:Label ID="billnum" runat="server"></asp:Label>
                                    </strong></div>
                                </td>
                            </tr>
                            </table>
                            <div dir="rtl" style="text-align: center; ">
                      <asp:GridView runat="server" ID="GvCustomerData" AutoGenerateColumns="False"  CssClass="table table-bordered table-condensed center" Width="100%" >
                        <Columns >
                            <asp:BoundField DataField="ItemName" HeaderText="اسم الصنف" />
                            <asp:BoundField DataField="Quantity" HeaderText="الكمية" />
                            <asp:BoundField DataField="itemprice" HeaderText="سعر الوحدة" />
                            <asp:BoundField DataField="total" HeaderText="الاجمالي" />
                               </Columns>
                        <EmptyDataTemplate>
                            <div class="alert">
                                <%= Tokens.NoResults %>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                    </div>
                    
                    <table style="width: 100%; height: 20px;" dir="rtl">
                            <tr>
                                <td class="style9" style="text-align: right">
                                    الاجمالي /<asp:Label ID="total" runat="server" ></asp:Label>
                                </td>
                                <td class="style8" style="text-align: right">
                                    الخصم/<asp:Label ID="dis" runat="server"></asp:Label>
                                </td>
                                <td class="style3" style="text-align: right">
                                    الصافي/<asp:Label ID="totalafterdis" runat="server"></asp:Label>
                                </td>
                                <td>
                                    المدفوع/<asp:Label ID="repaid" runat="server"></asp:Label>
                                </td>
                            </tr>
                            
                            <tr>
                                <td class="style1" colspan="3" style="text-align: right">
                                    ملاحظات/<asp:Label ID="note" runat="server"></asp:Label>
                                </td>
                            </tr>
                            
                            </table>
                        </div>
                         <div align="center">
                        <input id="Button5" class="btn btn-success" type="button" onclick="Print('MyDiv1');"
                            value="طباعة" /></div>
                             </div>
                             
                             
                             <div class="view"  runat="server" visible="False" id="Reciept" style="width: 8cm">
         
                   
                    <div id="Div2"  style="width: 8cm; text-align: center;">
                        <table style="width: 8cm; height: 150px;" dir="rtl">
                            <tr>
                                <td class="style5">
                                    <strong>اسم الشركة /<asp:Label ID="comp2" runat="server" 
                                        style="font-size: small"></asp:Label>
                                    </strong></td>
                                <td class="style3" rowspan="4">
                                    <asp:Image ID="Image2" runat="server" Height="92px" Width="116px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style5">
                                    <strong>التاريخ / </strong>
                                    <asp:Label ID="date2" runat="server" Style="font-size: medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="style5">
                                    <strong>المستخدم/</strong><asp:Label ID="user2" runat="server" 
                                        Style="font-size: medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="style5">
                                    <strong>اسم العميل/</strong><asp:Label ID="cusname2" runat="server" 
                                        Style="font-size: medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="style7" colspan="2">
                                   
                                    <strong>فاتورة مبيعات رقم /</strong><asp:Label ID="billnum2" runat="server"></asp:Label>
                                   
                                </td>
                            </tr>
                            </table>
                            <div  style="text-align: center; width: 8cm;">
                      <asp:GridView runat="server" ID="GridView1" AutoGenerateColumns="False"  
                                    CssClass="table table-bordered table-condensed center" Width="8cm" >
                        <Columns >
                            <asp:BoundField DataField="ItemName" HeaderText="اسم الصنف" />
                            <asp:BoundField DataField="Quantity" HeaderText="الكمية" />
                            <asp:BoundField DataField="itemprice" HeaderText="السعر" />
                            <asp:BoundField DataField="total" HeaderText="الاجمالي" />
                               </Columns>
                        <EmptyDataTemplate>
                            <div class="alert">
                                <%= Tokens.NoResults %>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                    </div>
                    
                    <table style="width: 8cm; height: 20px;" dir="rtl">
                            <tr>
                                <td class="style4" style="text-align: right">
                                    الاجمالي /<asp:Label ID="total2" runat="server" ></asp:Label>
                                </td>
                                <td class="style7">
                                    الخصم/<asp:Label ID="dis2" runat="server"></asp:Label>
                                </td>
                            </tr>
                            
                            <tr>
                                <td class="style4" style="text-align: right">
                                    الصافي/<asp:Label ID="totalafterdis2" runat="server"></asp:Label>
                                </td>
                                <td >
                                    المدفوع/<asp:Label ID="repaid2" runat="server"></asp:Label>
                                </td>
                            </tr>
                            
                            <tr>
                                <td class="style1"  colspan="2">
                                    &nbsp;ملاحظات/<asp:Label ID="note2" runat="server"></asp:Label>
                                </td>
                            </tr>
                            
                            </table>
                        </div>
                         <div align="center">
                        <input id="Button1" class="btn btn-success" type="button" onclick="Print('Div2');"
                            value="طباعة" /></div>
                             </div> 
    
 
    <script type="text/javascript">
        $(document).ready(function () {
            $('input[data-x="dt"]').datepicker({
                dateFormat: 'dd/mm/yy'
            });
          
            $('#footerdiv').css({ "position": "inherit" });

        });
        function Print(div) {

            var printContent = document.getElementById(div);
            var windowUrl = '';  /*'about:blank';*/
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();
            var printWindow = window.open(windowUrl, windowName, 'right=50000,top=50000,width=0,height=0');
            printWindow.document.write(printContent.innerHTML);
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            printWindow.close();

        } function areyousure() {
            return confirm('<%= Tokens.AlertRUS %>');
            }
            function showDiv(showdiv) {
                document.getElementById(showdiv).style.display = "block";
            }
    </script>
</asp:Content>


