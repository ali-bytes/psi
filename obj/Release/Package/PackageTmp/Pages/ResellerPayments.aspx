<%@ Page Title="<%$ Resources:Tokens,ResellerPayments %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ResellerPayments.aspx.cs" Inherits="NewIspNL.Pages.ResellerPayments" %>


<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
<link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>

    <asp:Panel ID="p_searchInput" runat="server">
        <div class="view">
                    <fieldset>
                        <div class="page-header"><h1>
                            <%=Tokens.SearchOptions %></h1></div>
                            <div class="well">
            <div class="editor-label">
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Tokens,Resellers %>"></asp:Label></div>
            <div class="editor-field">
                <asp:DropDownList ID="ddl_reseller" CssClass="width-40 chosen-select" runat="server" >
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                            ControlToValidate="ddl_reseller" InitialValue=" ">Select request type</asp:RequiredFieldValidator>
            </div>
            <div class="editor-label">
                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Tokens,From %>"></asp:Label>
            </div>
            <div class="editor-field">
                <div>
                    <asp:TextBox ID="tb_from" runat="server" ClientIDMode="Static" Width="200px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                                ControlToValidate="tb_from"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="tb_from"
                                          Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,ValidDate %>" Operator="DataTypeCheck"
                                          Type="Date"></asp:CompareValidator></div>
            </div>
            <div class="editor-label">
                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Tokens,To %>"></asp:Label></div>
            <div class="editor-field">
                <div>
                    <asp:TextBox ID="tb_to" runat="server" ClientIDMode="Static" Width="200px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                                ControlToValidate="tb_to"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="tb_to"
                                          Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,ValidDate %>" Operator="DataTypeCheck"
                                          Type="Date"></asp:CompareValidator></div>
            </div>
            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="tb_to"
                                  ControlToValidate="tb_from" Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,DatePeriodMsg %>"
                                  Operator="LessThan" Type="Date"></asp:CompareValidator>
            <div style="padding: 7px 16px;">
                <asp:RadioButtonList runat="server" ID="RblSearch" ClientIDMode="Static" CssClass="radio">
                    <asp:ListItem Selected="True" Text="<%$Resources:Tokens,ResellerPR %>"></asp:ListItem>
                    <asp:ListItem Text="<%$Resources:Tokens,AllPayments %>"></asp:ListItem>
                </asp:RadioButtonList>
            
            </div>
                        <p>
                            <asp:Button ID="b_search" runat="server" CssClass="btn btn-info" Text="<%$ Resources:Tokens,Search %>" OnClick="b_search_Click"
                                        Width="90px" /></p>
                                        </div>
                            </fieldset>
        </div>
    </asp:Panel>
    <div id="result">
        <asp:Panel ID="Panel1" runat="server">
            <h3 class="header smaller lighter blue"><asp:Literal runat="server" ID="lblsectitle" Text="<%$ Resources:Tokens,Results %>"></asp:Literal></h3>
            <div id="result-output">
                <asp:GridView ID="gv_results" runat="server" AutoGenerateColumns="False" ClientIDMode="Static"
                              OnDataBound="gv_results_DataBound"
                              CssClass="table table-bordered table-condensed text-center">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="gv_l_number" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustomerName" HeaderText="<%$ Resources:Tokens,name %>" />
                        <asp:BoundField DataField="CustomerPhone" HeaderText="<%$ Resources:Tokens,Phone %>" />
                        <asp:BoundField DataField="GovernorateName" HeaderText="<%$ Resources:Tokens,Governrate %>" />
                        <asp:BoundField DataField="Provider" HeaderText="<%$ Resources:Tokens,Provider %>" />
                        <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:Tokens,User %>" />
                        <asp:Boundfield DataField="ProcessDate" DataFormatString="{0:d}" HeaderText="<%$Resources:Tokens,PaymentDate %>"/>
                        <asp:BoundField DataField="Resseller" HeaderText="<%$ Resources:Tokens,Reseller %>" />
                        <asp:BoundField DataField="Branch" HeaderText="<%$ Resources:Tokens,Branch %>" />
                        
                         <asp:BoundField DataField="StartAt" DataFormatString="{0:dd/MM/yyyy}" HeaderText="<%$ Resources:Tokens,From %>" />
                         <asp:BoundField DataField="EndAt" DataFormatString="{0:dd/MM/yyyy}" HeaderText="<%$ Resources:Tokens,To %>" />

                        <asp:BoundField DataField="Status" HeaderText="<%$ Resources:Tokens,Status %>" />
                        <asp:BoundField DataField="Total" HeaderText="<%$ Resources:Tokens,Total %>" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,ResellerDiscount %>" DataField="Discound" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Net %>" DataField="Net" />
                    </Columns>
                    <EmptyDataTemplate>
                        <div>
                            <asp:Literal Text="<%$ Resources:Tokens,NoResults %>" runat="server" /></div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </asp:Panel>
        <asp:LinkButton  runat="server" CssClass="btn btn-primary" OnClick="OnClick"><i class="icon-file-text icon-only"></i>
        &nbsp;<asp:Literal runat="server" Text="<%$Resources:Tokens,Export %>"></asp:Literal></asp:LinkButton>
    </div>
 
    <script type="text/javascript">
        $(document).ready(function () {
            $(".chosen-select").chosen();
            $('#tb_from').datepicker({ dateFormat: 'dd-mm-yy' });
            $('#tb_to').datepicker({ dateFormat: 'dd-mm-yy' });
            $('tr td input[type=radio]').addClass("ace");
            $('tr td label').addClass("lbl");
        });
    </script>
    
</asp:Content>

