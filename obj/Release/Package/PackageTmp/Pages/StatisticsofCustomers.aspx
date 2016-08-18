<%@ Page Title="<%$Resources:Tokens,StatisticsofCustomers %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="StatisticsofCustomers.aspx.cs" Inherits="NewIspNL.Pages.StatisticsofCustomers" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-header">
        <h1>
            <asp:Literal runat="server" Text="<%$Resources:Tokens,StatisticsofCustomers %>" ID="lblheader"></asp:Literal></h1>
    </div>
    <div class="row">
        <div class="col-sm-6">
            <div class="dd" id="nestable">
                <ol class="dd-list">
                    <asp:DataList ID="StatusList" runat="server" Width="100%">
                        <ItemTemplate>
                            <li class="dd-item" data-id='<%#Eval("ID") %>'><a href="<%#Eval("Url") %>" target="_blank">
                                <div class="dd-handle">
                                    <asp:Literal runat="server" Text='<%#Eval("Name") %>' ID="lblname"></asp:Literal>
                                    <i class="pull-right red">
                                        (&nbsp;<asp:Literal runat="server" Text='<%#Eval("count") %>'></asp:Literal>&nbsp;)</i>
                                </div>
                            </a></li>
                        </ItemTemplate>
                    </asp:DataList>
                </ol>
            </div>
        </div>
    </div>
</asp:Content>
