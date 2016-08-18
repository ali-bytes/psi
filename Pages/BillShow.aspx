<%@ Page Title="<%$Resources:Tokens,billsearch %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="BillShow.aspx.cs" Inherits="NewIspNL.Pages.BillShow" %>
<%@ Import Namespace="NewIspNL.Helpers" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
							<h1>
								<asp:Literal ID="Literal2" Text="<%$Resources:Tokens,billsearch %>"  runat="server" />
							</h1>
						</div>
                        <div class="col-xs-12">
                        <div class="well">

                         <div class="form-group">
               <label class="col-sm-3 control-label no-padding-right"> <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:Tokens,Type %>"></asp:Literal></label>
              
                                                    <asp:DropDownList  ID="billkind" CssClass="required-input" runat="server" Height="40px" Width="100px">
                                                         <asp:ListItem Text="<%$Resources:Tokens,--Chose-- %>" Value="0" />
                                                          <asp:ListItem Text="<%$Resources:Tokens,Purchases %>" Value="1" />
                                                        <asp:ListItem Text="<%$Resources:Tokens,Sales %>" Value="2" />
                                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ControlToValidate="billkind" ID="RequiredFieldValidator7"
                                        runat="server" ErrorMessage="*" ValidationGroup="addcus"></asp:RequiredFieldValidator>
              
                   <br/>
                 
                    <div class="form-group">
                            <br/>
               <label class="col-sm-3 control-label no-padding-right"> <asp:Literal ID="lblday" runat="server" Text="<%$Resources:Tokens,InvoiceNumber %>"></asp:Literal></label>
             
                
                    <asp:TextBox runat="server" ID="txtnum" CssClass="required-input"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtnum" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="addcus"></asp:RequiredFieldValidator>
            </div>
                     </div>
                            
                            
                            
                            

                     <div class="clearfix form-actions">
										<div class="col-md-offset-3 col-md-9">       
                            
                                            <asp:Button ID="search" CssClass="btn btn-success" runat="server" ValidationGroup="addcus" Text="<%$Resources:Tokens,Search %>" OnClick="search_Click" />              

 </div></div>
    </div></div>
    
    
    
       <div class="view">
            
            <div>
                <fieldset>
                   
                     <h3 class="header smaller lighter blue">
                        <asp:Literal ID="Literal3" Visible="False" Text="<%$Resources:Tokens,Result %>" runat="server" /></h3>
                    <div>
                        <asp:GridView runat="server" ID="GridView1" OnDataBound="Gvbill_OnDataBound" AutoGenerateColumns="False"
                                      CssClass="table table-bordered table-condensed center">
                            <Columns>
                                
                                <asp:BoundField DataField="InvoiceNumber" HeaderText="<%$Resources:Tokens,InvoiceNumber%>" />
                              
                               
                                <asp:BoundField DataField="Total" HeaderText="<%$Resources:Tokens,Total%>" />
                                <asp:BoundField DataField="Discound" HeaderText="<%$Resources:Tokens,Discount%>" />
                                 
                                 <asp:BoundField DataField="Net" HeaderText="<%$Resources:Tokens,Net%>" />
                               
                                 
                                <asp:BoundField DataField="Paid" HeaderText="<%$Resources:Tokens,Paid%>" />
                                 <asp:BoundField DataField="Remaining" HeaderText="<%$Resources:Tokens,Remaining%>" />
                           
                                <asp:BoundField DataField="invoiceDate" HeaderText="<%$Resources:Tokens,InvoiceDate%>" />
                                    <asp:BoundField DataField="StoreName" HeaderText="<%$Resources:Tokens,Store%>" />
                                 <asp:BoundField DataField="SupplierName" HeaderText="<%$Resources:Tokens,SupplierName%>" />
                               <asp:BoundField DataField="CustomerName" HeaderText="<%$Resources:Tokens,Customer.Name%>" />
                                   <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes%>" />
                               
                                       
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="alert">
                                    <%=Tokens.NoResults %>
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                    
                    
                    
                    
                    
                    
                    
                    

                     <h3 class="header smaller lighter blue">
                        <asp:Literal ID="Literal1" Visible="False" Text="<%$Resources:Tokens,Items %>" runat="server" /></h3>
                    <div>
                        <asp:GridView runat="server" ID="Gvbill" OnDataBound="Gvbill_OnDataBound" AutoGenerateColumns="False"
                                      CssClass="table table-bordered table-condensed center">
                            <Columns>
                                <asp:TemplateField HeaderText="#">
                                    <ItemTemplate>
                                        <asp:Label ID="LNo" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:Tokens,ItemName%>" />
                                <asp:BoundField DataField="Quantity" HeaderText="<%$Resources:Tokens,Quantity%>" />
                                <asp:BoundField DataField="itemprice" HeaderText="<%$Resources:Tokens,Price%>" />
                                 
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="alert">
                                    <%=Tokens.NoResults %>
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </fieldset>
            </div>
        </div>
    
     <div class="view"  runat="server" visible="False" id="Reciept" >
         
                   
                    <div id="Div2"  style="text-align: center;">
                        <table style="width: 100%; height: 150px;" dir="rtl">
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
                            <div  style="text-align: center; width: 100%;">
                      <asp:GridView runat="server" ID="GridView2" AutoGenerateColumns="False"  
                                    CssClass="table table-bordered table-condensed center" Width="100%" >
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
                    
                    <table style="width: 100%; height: 20px;" dir="rtl">
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

         }


             </script>
</asp:Content>
<asp:Content ID="Content2" runat="server" contentplaceholderid="head">
   
</asp:Content>

