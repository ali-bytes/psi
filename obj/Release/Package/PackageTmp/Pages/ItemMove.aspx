<%@ Page Title="<%$Resources:Tokens,ItemMove %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ItemMove.aspx.cs" Inherits="NewIspNL.Pages.ItemMove" %>

 
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

     <div class="page-header" >
                <h1><asp:Literal ID="Literal8" runat="server" Text="<%$Resources:Tokens,ItemMove %>"></asp:Literal>
                </h1>
            </div>
    
       <div class="well">
                        <div>
                            <label>
                                <asp:Literal ID="Label1" runat="server" Text="<%$ Resources:Tokens,Chose %>"></asp:Literal></label>
                            <div>
                                <asp:DropDownList ID="ddlItems" runat="server" Width="205px" DataTextField="ItemName" DataValueField="ItemId" AutoPostBack="false">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                            ControlToValidate="ddlItems"></asp:RequiredFieldValidator>
                            </div>
                        </div>
              <br/>
                        <div>
                        
                      <%=Tokens.From %> <asp:TextBox ID="datefrom" runat="server" data-select="dt"> </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                            ControlToValidate="datefrom"></asp:RequiredFieldValidator>
                       
                        </div>
                        <br/>
                          <div>
                           
                  <%=Tokens.To %>   <asp:TextBox ID="datetoo" runat="server" data-select="dt"></asp:TextBox>
                   <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                            ControlToValidate="datetoo"></asp:RequiredFieldValidator>
                            
                        </div>

                        <p>
                            <br/>
                          <button id="Button1"  runat="server" class="btn btn-primary" OnServerClick="Search">
                                <i class="icon-edit"></i>&nbsp;<%=Tokens.Search %></button>
                                <asp:Button runat="server" OnClick="btnExport_OnClick" CssClass="btn btn-danger" Text="<%$Resources:Tokens,Export %>" ID="btnExportToExcel"/>
                        </p>
                    </div>
    
      <div dir="rtl" style="text-align: center; ">
                      <asp:GridView runat="server" ID="GvitemData" AutoGenerateColumns="False"  CssClass="table table-bordered table-condensed center" Width="100%" >
                        <Columns >
                              <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LNo" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField DataField="ItemName" HeaderText="اسم الصنف" />
                            <asp:BoundField DataField="Quantity" HeaderText="الكمية" />
                            <asp:BoundField DataField="itemprice" HeaderText="السعر" />
                            <asp:BoundField DataField="total" HeaderText="اجمالي الصنف" />
                            <asp:BoundField DataField="invoiceDate" HeaderText="التاريخ"   DataFormatString="{0:dd/MM/yyyy}"/>
                            <asp:BoundField DataField="StoreName" HeaderText="المخزن" />
                            <asp:BoundField DataField="TypeName" HeaderText="نوع العملية" />
                             <asp:BoundField DataField="SupplierName" HeaderText="المورد" />
                            <asp:BoundField DataField="CustomerName" HeaderText="العميل" />
                            <asp:BoundField DataField="Notes" HeaderText="ملاحظات" />

                               </Columns>
                        <EmptyDataTemplate>
                            <div class="alert">
                                <%= Tokens.NoResults %>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                    </div>
                    <script type="text/javascript">
                        $(function () {
                            $('input[data-select="dt"]').datepicker({
                                showOtherMonths: true,
                                selectOtherMonths: false,
                                dateFormat: 'dd/mm/yy'
                            });
                        });
                    </script>
    
    
    

</asp:Content>

