<%@ Page Title="<%$Resources:Tokens,SuspendRequest%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="SuspendByDays.aspx.cs" Inherits="NewIspNL.Pages.SuspendByDays" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="page-header"><h1><%=Tokens.SuspendRequest %></h1></div>
    <div class="alert alert-danger" runat="server" id="MsgErro" Visible="False"><asp:Literal runat="server" ID="lblerror" Text="<%$Resources:Tokens,Error %>"></asp:Literal></div>
    <div class="alert alert-success" runat="server" id="MsgSuscc" Visible="False"><asp:Literal runat="server" ID="lblsuscee" Text="<%$Resources:Tokens,ProcessDone %>"></asp:Literal></div>
    <div class="well">
                        <label for="txtSuspendDaysCount">
                        <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,DaysCount%>" runat="server" />
                    </label>

                    <div>
                        <asp:TextBox runat="server" ID="txtSuspendDaysCount" ClientIDMode="Static"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RTbSus" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                    ControlToValidate="txtSuspendDaysCount" runat="server" />
                        <asp:CompareValidator runat="server" ID="xxx" ControlToValidate="txtSuspendDaysCount"
                                              Type="Integer" Operator="DataTypeCheck" ErrorMessage="<%$Resources:Tokens,NumbersOnly %>"></asp:CompareValidator>
                    </div><br/>
                    <asp:LinkButton runat="server" ID="btnAddRequests" CssClass="btn btn-success" OnClick="AddRequest">
                        <i class="icon-arrow-right icon-on-right"></i>&nbsp;&nbsp;<%=Tokens.Add_Request %>
                    </asp:LinkButton>
                    </div>
</asp:Content>


