<%@ Page Title="<%$Resources:Tokens,SupplierAccountStatement %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="SupplierAccountStatement.aspx.cs" Inherits="NewIspNL.Pages.SupplierAccountStatement" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>
     <div class="page-header" >
                <h1><asp:Literal ID="Literal8" runat="server" Text="<%$Resources:Tokens,SupplierAccountStatement %>"></asp:Literal>
                </h1>
            </div>
    
       <div class="well">
                        <div>
                            <label>
                                <asp:Literal runat="server" ID="lblstores" Text="<%$Resources:Tokens,Suppliers %>"></asp:Literal></label>
                            <div>
                                 <asp:DropDownList runat="server" ID="ddlSupplier" ClientIDMode="Static" CssClass="chosen" DataTextField="SupplierName" DataValueField="Id"/>
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
    
      <div dir="rtl" style="text-align: center;">
          <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                      <asp:GridView runat="server" ID="GvitemData" AutoGenerateColumns="False"  CssClass="table table-bordered table-condensed center" Width="100%" >
                        <Columns >
                              <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LNo" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField DataField="Date" HeaderText="التاريخ"   DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="Debit" HeaderText="مدين" />
                            <asp:BoundField DataField="Credit" HeaderText="دائن" />
                            <asp:BoundField DataField="Description" HeaderText="البيان" />
                           

                               </Columns>
                        <EmptyDataTemplate>
                            <div class="alert">
                                <%= Tokens.NoResults %>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
              </asp:PlaceHolder>

          <asp:PlaceHolder ID="PlaceHolder2" runat="server">
           <asp:GridView runat="server" ID="GVTotal" AutoGenerateColumns="False"  CssClass="table table-bordered table-condensed center" Width="100%" >
                        <Columns >
                             <asp:BoundField DataField="Description" HeaderText="البيان" />
                            <asp:BoundField DataField="Debit" HeaderText="مدين" />
                            <asp:BoundField DataField="Credit" HeaderText="دائن" />
                             </Columns>
                        <EmptyDataTemplate>
                            <div class="alert">
                                <%= Tokens.NoResults %>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
          </asp:PlaceHolder>
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
