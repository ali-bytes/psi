<%@ Page Title="<%$Resources:Tokens,TestOnlineUsers %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="TestOnlineUsers.aspx.cs" Inherits="NewIspNL.Pages.TestOnlineUsers" %>

<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="view">
        <fieldset>
            <div class="page-header"><h1>
                <asp:Label runat="server" ID="lbl1" Text="<%$Resources:Tokens,TestOnlineUsers%>"></asp:Label></h1>
            </div>
            <div class="well">
            <%=Tokens.TestOnlineUsers %> <asp:Label runat="server" ID="lblusersCount"></asp:Label>
           <br/>
           <asp:GridView runat="server" OnDataBound="GvUserdataBounded" ID="GvUser" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed text-center">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="LNo" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="<%$Resources:Tokens,User %>" DataField="UserName"/>
                        </Columns>
                        </asp:GridView>
                     </div>
        </fieldset>
    </div>
   
       
</asp:Content>

