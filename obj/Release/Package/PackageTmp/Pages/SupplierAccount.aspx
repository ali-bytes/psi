<%@ Page Title="<%$Resources:Tokens,SupplierAccount %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="SupplierAccount.aspx.cs" Inherits="NewIspNL.Pages.SupplierAccount" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
               <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
    <div class="page-header">
        <h1><asp:Literal runat="server" ID="lbltitle" Text="<%$Resources:Tokens,SupplierAccount %>"></asp:Literal></h1>
    </div>
        <div class="well col-md-12">
            <div class="col-md-6">
        <asp:Literal runat="server" ID="lblstores" Text="<%$Resources:Tokens,Suppliers %>"></asp:Literal>
        <div>
            <asp:DropDownList runat="server" ID="ddlSupplier" DataTextField="SupplierName" AutoPostBack="True"
                        DataValueField="Id" OnSelectedIndexChanged="ddlSupplier_SelectedIndexChanged"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlSupplier" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="supp"></asp:RequiredFieldValidator>
        </div>
        <asp:Literal runat="server" ID="lblRemainig" Text="<%$Resources:Tokens,Paid %>"></asp:Literal>
        <div>
        <asp:TextBox runat="server" ID="txtPaid" ontextchanged="txtPaid_TextChanged" AutoPostBack="True"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPaid" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="supp"></asp:RequiredFieldValidator>
        </div>
        </div>
       <div class="col-md-6">
           <asp:Literal runat="server" Text="<%$Resources:Tokens,Remaining %>" ID="lblrem"></asp:Literal>
           <div>
               <asp:TextBox runat="server" ID="txtRemaining" Enabled="False"></asp:TextBox>
               <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtRemaining" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="supp"></asp:RequiredFieldValidator>
           </div>
                      <asp:Literal runat="server" Text="<%$Resources:Tokens,Remaining %>" ID="Literal1"></asp:Literal>
           <div>
               <asp:TextBox runat="server" ID="txtNextRemain" Enabled="False"></asp:TextBox>
               <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtNextRemain" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="supp"></asp:RequiredFieldValidator>
           </div>
       </div>
    </div> <asp:Button runat="server" ID="btnSearch" Text="<%$Resources:Tokens,Pay %>" CssClass="btn btn-success" ValidationGroup="supp" OnClick="Pay" UseSubmitBehavior="false" OnClientClick="plswait(this.id) " />
    <br/>
    <div>
        <h3 class="header smaller lighter blue">
            <asp:Literal runat="server" Text="<%$Resources:Tokens,Results %>"></asp:Literal>
        </h3>
                <asp:GridView runat="server" ID="GvCredites" ClientIDMode="Static" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed text-center col-md-12 col-sm-12">
                    <Columns>
 <asp:BoundField DataField="Paymentdate" HeaderText="<%$Resources:Tokens,PaymentDate %>"/>
                        <asp:BoundField DataField="Total" HeaderText="<%$Resources:Tokens,Total %>"/>
                        <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Paid %>"/>
                        <asp:BoundField DataField="Remaining" HeaderText="<%$Resources:Tokens,Remaining %>"/>
                        <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,UserName %>"/>
                        
                        <asp:BoundField DataField="PaymentComment" HeaderText="<%$Resources:Tokens,PaymentComment %>"/>
                        <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes %>"/>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Literal runat="server" Text="<%$Resources:Tokens,NoResults %>" ID="lblNoRes"></asp:Literal>
                        </EmptyDataTemplate>
                        </asp:GridView>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    
    
    
    <script type="text/javascript">
        

        function plswait(id) {

            var type = document.getElementById('<%=ddlSupplier.ClientID%>').value;
            var amo = document.getElementById('<%=txtPaid.ClientID%>').value;
            
            var amo2 = document.getElementById('<%=txtNextRemain.ClientID%>').value;
          



            if (type == "" || amo == "" || amo2 == "") { return; }
            else {
                var check2 = document.getElementById(id);
                check2.disabled = 'true'; check2.value = 'Please wait...';
            }

        }
           </script>
</asp:Content>



