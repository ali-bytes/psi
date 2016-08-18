<%@ Page Title="<%$Resources:Tokens,CustomerAccount %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="CustomerAccount.aspx.cs" Inherits="NewIspNL.Pages.CustomerAccount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>
    <%-- <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>--%>
    <div class="page-header">
        <h1><asp:Literal runat="server" ID="lbltitle" Text="<%$Resources:Tokens,CustomerAccount %>"></asp:Literal></h1>
    </div>
        <div class="well col-md-12">
            <div class="col-md-6">
        <asp:Literal runat="server" ID="lblstores" Text="<%$Resources:Tokens,Customers %>"></asp:Literal>
        <div>
           <asp:DropDownList runat="server" ID="ddlCustomer" ClientIDMode="Static" CssClass="chosen-select" DataTextField="CustomerName" DataValueField="Id" AutoPostBack="True" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" />
          <asp:RequiredFieldValidator ID="RequiredFieldValidator34" runat="server" ControlToValidate="ddlCustomer" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="supp"></asp:RequiredFieldValidator>

             </div>
        <asp:Literal runat="server" ID="lblRemainig" Text="<%$Resources:Tokens,Paid %>"></asp:Literal>
        <div>
        <asp:TextBox runat="server" ID="txtPaid"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPaid" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="supp"></asp:RequiredFieldValidator>
       <asp:RegularExpressionValidator ID="regexpName" runat="server"     
                                    ErrorMessage="<%$Resources:Tokens,Required %>" 
                                    ControlToValidate="txtPaid"     
                                    ValidationExpression="^\$?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$" />
             </div>
        </div>
       <div class="col-md-6">
           <asp:Literal runat="server" Text="<%$Resources:Tokens,Remaining %>" ID="lblrem"></asp:Literal>
           <div>
               <asp:Label runat="server" ID="lblRemaining" ></asp:Label>
               <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="supp"></asp:RequiredFieldValidator>--%>
           </div>
                    
         
       </div>
    </div> <asp:Button runat="server" ID="btnSearch" Text="<%$Resources:Tokens,Pay %>" CssClass="btn btn-success" OnClick="Pay" UseSubmitBehavior="false" />
    <br/>
    <div>
        <h3 class="header smaller lighter blue">
            <asp:Literal runat="server" Text="<%$Resources:Tokens,Results %>"></asp:Literal>
        </h3>
                <asp:GridView runat="server" ID="GvCredites" ClientIDMode="Static" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed text-center col-md-12 col-sm-12">
                    <Columns>
                       <asp:BoundField DataField="Paymentdate" HeaderText="<%$Resources:Tokens,PaymentDate %>"/>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,Total %>">

                          <ItemTemplate>
                          
                          <asp:Label ID="Label1" runat="server" Text='<%# Math.Round((decimal)Eval("Total"),2) %>'></asp:Label>
                          
                          </ItemTemplate>
                          
                          </asp:TemplateField>
                        <%--<asp:BoundField DataField="Total" HeaderText="<%$Resources:Tokens,Total %>" />--%>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,Paid %>">

                          <ItemTemplate>
                          
                          <asp:Label ID="Label1" runat="server" Text='<%# Math.Round((decimal)Eval("Amount"),2) %>'></asp:Label>
                          
                          </ItemTemplate>
                          
                          </asp:TemplateField>
                        <%--<asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Paid %>" />--%>
                        <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,UserName %>"/>
                        <asp:BoundField DataField="PaymentComment" HeaderText="<%$Resources:Tokens,PaymentComment %>"/>
                        <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes %>"/>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Literal runat="server" Text="<%$Resources:Tokens,NoResults %>" ID="lblNoRes"></asp:Literal>
                        </EmptyDataTemplate>
                        </asp:GridView>
    </div>
 <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
     <script type="text/javascript">
         $(document).ready(function() {
             $(".chosen-select").chosen();

           <%--  function plswait(id) {

                 var type = document.getElementById('<%=ddlCustomer.ClientID%>').value;
                 var amo = document.getElementById('<%=txtPaid.ClientID%>').value;

                


                 if (type == "" || amo == "" || amo2 == "") {
                     return;
                 } else {
                     var check2 = document.getElementById(id);
                     check2.disabled = 'true';
                     check2.value = 'Please wait...';
                 }

             }--%>
         });
           </script>
</asp:Content>
