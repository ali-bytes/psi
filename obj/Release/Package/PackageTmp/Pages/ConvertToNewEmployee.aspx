<%@ Page Title="<%$Resources:Tokens,ConvertToNewEmployee %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ConvertToNewEmployee.aspx.cs" Inherits="NewIspNL.Pages.ConvertToNewEmployee" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="page-header"><h1><asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,ConvertToNewEmployee %>"></asp:Literal></h1></div>
    <div class="alert alert-success" runat="server" id="divSuccess" Visible="False"><asp:Literal ID="lblsuc" runat="server" Text="<%$Resources:Tokens,ProcessDone %>"></asp:Literal></div>
    <div class="well">
                <div>
                    <asp:Label runat="server" ID="Label2" Text="<%$Resources:Tokens,Employee%>"></asp:Label></div>
                <div>
                    <asp:DropDownList runat="server" ID="ddl_eployees" Width="150px"/>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Tokens,Required%>"
                                                ControlToValidate="ddl_eployees" ValidationGroup="emp"></asp:RequiredFieldValidator>
                </div>
                <p>
                    <br/>
                    <asp:LinkButton runat="server" ID="b_search" ValidationGroup="emp" OnClick="Search"
                                 CssClass="btn btn-success"><i class="icon-search"></i>&nbsp;<asp:Literal runat="server" ID="lblb_search" Text="<%$Resources:Tokens,Search%>"></asp:Literal></asp:LinkButton>
                </p>
                </div>
                <div>
                <h3 class="header smaller lighter blue"><asp:Label runat="server" ID="lblres" Text="<%$Resources:Tokens,Customers %>"></asp:Label></h3>
                <asp:GridView runat="server" ID="gv_items" DataKeyNames="Id"
                        ClientIDMode="Static" CssClass="table table-bordered table-responsive text-center"
                         OnDataBound="gv_items_DataBound" Width="100%" AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField HeaderText="#">
                                <ItemTemplate>
                                    <asp:Label ID="l_Number" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Phone1" HeaderText="<%$Resources:Tokens,Phone%>" />
                            <asp:BoundField DataField="Name" HeaderText="<%$Resources:Tokens,Name%>" />
                            <asp:BoundField DataField="Governate" HeaderText="<%$Resources:Tokens,Governrate%>" />
                            <asp:BoundField DataField="State" HeaderText="<%$Resources:Tokens,State%>" />
                            <asp:BoundField DataField="Offer1" HeaderText="<%$Resources:Tokens,Offer1%>" />
                            <asp:BoundField DataField="Offer2" HeaderText="<%$Resources:Tokens,Offer2%>" />
                            <asp:BoundField DataField="Employee" HeaderText="<%$Resources:Tokens,Employee%>" />
                            <asp:BoundField DataField="Comment" HeaderText="<%$Resources:Tokens,Comment%>" />
                            <asp:BoundField DataField="Central" HeaderText="<%$Resources:Tokens,Central%>" />
                            <asp:BoundField DataField="Mobile" HeaderText="<%$Resources:Tokens,Mobile%>" />
<%--                            <asp:TemplateField>
                                <ItemTemplate>

                                                <asp:LinkButton ID="b_approve" runat="server" ToolTip="<%$Resources:Tokens,Approve%>" CommandArgument='<%# Bind("Id") %>' data-rel="tooltip"
                                                    OnClick="b_approve_Click" CssClass="btn btn-success btn-sm"><i class="icon-ok icon-only"></i></asp:LinkButton>
                                                    &nbsp;&nbsp;
                                                <a id="b_reject" data-val="Reject" class="btn btn-danger btn-sm" runat="server" title="<%$Resources:Tokens,Reject%>" data-rel="tooltip"
                                                     tabindex='<%#Eval("Id") %>' clientidmode="Static"><i class="icon-remove icon-only"></i></a><%--CommandArgument='<%# Bind("Id") %>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                        <EmptyDataTemplate>
                            <div>
                                <%=Tokens.NoResults %></div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
                <div>
                    <h3 class="header smaller lighter blue"><asp:Label runat="server" ID="lbltrans" Text="<%$Resources:Tokens,TransferTo %>"></asp:Label></h3>
                <div class="well">
                <div>
                    <asp:Label runat="server" ID="Label1" Text="<%$Resources:Tokens,Employee%>"></asp:Label></div>
                <div>
                    <asp:DropDownList runat="server" ID="ddlNewEmplyee" Width="150px"/>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Tokens,Required%>"
                                                ControlToValidate="ddl_eployees" ValidationGroup="emp2"></asp:RequiredFieldValidator>
                </div>
                <p>
                    <br/>
                    <asp:LinkButton runat="server" ID="LinkButton1" ValidationGroup="emp2" OnClick="Transfare"
                                 CssClass="btn btn-primary"><i class="icon-refresh"></i>&nbsp;<asp:Literal runat="server" ID="Literal2" Text="<%$Resources:Tokens,Confirm%>"></asp:Literal></asp:LinkButton>
                </p>
                </div>
                </div>
</asp:Content>

