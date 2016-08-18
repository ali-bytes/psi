<%@ Page Title="<%$Resources:Tokens,ResellersTransformationReport%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ResellersTransformationReport.aspx.cs" Inherits="NewIspNL.Pages.ResellersTransformationReport" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div class="page-header">
        <h1>
            <asp:Literal runat="server" ID="lbltitle" Text="<%$Resources:Tokens,Search %>"></asp:Literal></h1>
    </div>

    <div class="row">
        <div class="col-xs-12">
            <div class="well">
            <div style="padding-bottom: 5px;">
                <asp:Label runat="server" Text="<%$Resources:Tokens,Reseller %>" ID="labelReseller"></asp:Label></div>
            <div>
                <asp:DropDownList runat="server" ID="ddlReseller" Width="150px" ClientIDMode="Static">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlReseller"
                                            ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="se"></asp:RequiredFieldValidator>
            </div>
                        <div class="editor-label">
                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Tokens,From %>"></asp:Label>
            </div>
            <div class="editor-field">
                <div>
                    <asp:TextBox ID="txtFrom" runat="server" ClientIDMode="Static" Width="200px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                                ControlToValidate="txtFrom"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txtFrom"
                                          Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,ValidDate %>" Operator="DataTypeCheck"
                                          Type="Date"></asp:CompareValidator></div>
            </div>
            <div class="editor-label">
                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Tokens,To %>"></asp:Label></div>
            <div class="editor-field">
                <div>
                    <asp:TextBox ID="txtTo" runat="server" ClientIDMode="Static" Width="200px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                                ControlToValidate="txtTo"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="txtTo"
                                          Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,ValidDate %>" Operator="DataTypeCheck"
                                          Type="Date"></asp:CompareValidator></div>
            </div>
            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtTo"
                                  ControlToValidate="txtFrom" Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,DatePeriodMsg %>"
                                  Operator="LessThan" Type="Date"></asp:CompareValidator>
            <p><br/>
                <asp:LinkButton runat="server" ID="btnSearch" CssClass="btn btn-success" ValidationGroup="se"
                            OnClick="Search_Click"><i class="icon-search"></i>&nbsp;<asp:Literal ID="Literal21" runat="server" Text="<%$Resources:Tokens,Search %>"></asp:Literal></asp:LinkButton>
            </p>
            </div>
            <h3 class="header smaller lighter blue"><asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,Results %>"></asp:Literal></h3>
            <asp:GridView runat="server" ID="gvRequests" AutoGenerateColumns="False"
                              ForeColor="Black" GridLines="Horizontal" CssClass="table table-bordered table-condensed text-center"
                              OnDataBound="gv_customers_DataBound" Width="100%"
                              ClientIDMode="Static" >
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="gv_lNumber" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,Reseller %>" />
                        <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Amount %>" />
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,TransferFrom%>">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblFrom" Text='<%#Eval("TransferFrom") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,TransferTo%>">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblTo" Text='<%#Eval("TransferTo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,UserName %>" />
                        <asp:BoundField DataField="RequestDate" DataFormatString="{0:dd-MM-yyyy}" HeaderText="<%$Resources:Tokens,RequestDate%>" />
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,RequestStatus %>">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblStatus" Text='<%#Eval("Status") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <%=Tokens.NoResults %>
                    </EmptyDataTemplate>
                            
                </asp:GridView>
        </div>
    </div>
     
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtFrom").datepicker({ dateFormat: 'dd-mm-yy' });
            $("#txtTo").datepicker({ dateFormat: 'dd-mm-yy' });
         
        });
    </script>
</asp:Content>


