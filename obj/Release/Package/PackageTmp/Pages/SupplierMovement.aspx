<%@ Page Title="<%$Resources:Tokens,SupplierMovement %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="SupplierMovement.aspx.cs" Inherits="NewIspNL.Pages.SupplierMovement" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>
     <div class="page-header" >
                <h1><asp:Literal ID="Literal8" runat="server" Text="<%$Resources:Tokens,SupplierMovement %>"></asp:Literal>
                </h1>
            </div>
    
       <div class="well">
                        <div>
                            <asp:Literal runat="server" ID="lblstores" Text="<%$Resources:Tokens,Suppliers %>"></asp:Literal>
                            <div>
                                <asp:DropDownList runat="server" ID="ddlSupplier" ClientIDMode="Static" CssClass="chosen" DataTextField="SupplierName" DataValueField="Id" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator34" runat="server" ControlToValidate="ddlSupplier" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="supp"></asp:RequiredFieldValidator>

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
                         <asp:BoundField DataField="invoiceDate" HeaderText="التاريخ"   DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="InvoiceNumber" HeaderText="رقم الفاتورة" />
                            <asp:BoundField DataField="SupplierName" HeaderText="اسم المورد" />

                            <asp:TemplateField HeaderText="<%$Resources:Tokens,Total %>">

                          <ItemTemplate>
                          
                          <asp:Label ID="Label1" runat="server" Text='<%# Math.Round((decimal)Eval("Total"),2) %>'></asp:Label>
                          
                          </ItemTemplate>
                          
                          </asp:TemplateField>
                           <%-- <asp:BoundField DataField="Total" HeaderText="الإجمالي" />--%>
                            <asp:TemplateField HeaderText="<%$Resources:Tokens,Discount %>">

                          <ItemTemplate>
                          
                          <asp:Label ID="Label1" runat="server" Text='<%# Math.Round((decimal)Eval("Discound"),2) %>'></asp:Label>
                          
                          </ItemTemplate>
                          
                          </asp:TemplateField>
                            <%--<asp:BoundField DataField="Discound" HeaderText="الخصم" />--%>
                             <asp:TemplateField HeaderText="<%$Resources:Tokens,Net %>">

                          <ItemTemplate>
                          
                          <asp:Label ID="Label1" runat="server" Text='<%# Math.Round((decimal)Eval("Net"),2) %>'></asp:Label>
                          
                          </ItemTemplate>
                          
                          </asp:TemplateField>
                            <%--<asp:BoundField DataField="Net" HeaderText="الصافى" />--%>
                              
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
                        jQuery(document).ready(function () {
                            jQuery(".chosen").chosen();
                        });
                    </script>
    
    
    
</asp:Content>
