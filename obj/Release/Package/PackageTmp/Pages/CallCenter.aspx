<%@ Page Title="<%$Resources:Tokens,CallCenter%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="CallCenter.aspx.cs" Inherits="NewIspNL.Pages.CallCenter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<%--    <script src="../js/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.custom.min.js" type="text/javascript"></script>
    <script src="../js/jquery.ui.datepicker-ar.js" type="text/javascript"></script>--%>
    <script src="../Content/ace-assest/js/jquery-ui-1.10.3.full.min.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $('#tb_from').datepicker({ dateFormat: 'dd-mm-yy' });
            $('#tb_to').datepicker({ dateFormat: 'dd-mm-yy' });
            $('#tb_CallDate').datepicker({ dataFormat: 'dd-mm-yy' });
            $('#tb_VisitDate').datepicker({ dataFormat: 'dd-mm-yy' });
        });

    </script>
    <div>
        <asp:Panel ID="p_search" runat="server">
            <div class="page-header"><h1><asp:Literal runat="server" Text="<%$Resources:Tokens,Search %>"></asp:Literal></h1></div>
            <div>
                <div class="editor-label">
                    <asp:Label ID="l_from" runat="server" Text="<%$Resources:Tokens,From %>"></asp:Label></div>
                <div class="editor-label">
                    <asp:TextBox ID="tb_from" runat="server" Width="200px" ClientIDMode="Static"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                        Display="Dynamic" Text="<%$Resources:Tokens,Required %>" ControlToValidate="tb_from"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator5" runat="server" ControlToValidate="tb_from"
                        Display="Dynamic" ErrorMessage="CompareValidator" Operator="DataTypeCheck" Type="Date" Text="<%$Resources:Tokens,ValidDate %>"></asp:CompareValidator>
                </div>
                <div class="editor-label">
                    <asp:Label ID="l_to" runat="server" Text="<%$Resources:Tokens,To %>"></asp:Label></div>
                <div class="editor-label">
                    <asp:TextBox ID="tb_to" runat="server" Width="200px" ClientIDMode="Static"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                        Display="Dynamic" ControlToValidate="tb_to" Text="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="tb_to"
                        Display="Dynamic" ErrorMessage="<%$Resources:Tokens,ValidDate %>" Operator="DataTypeCheck" Type="Date" Text="<%$Resources:Tokens,ValidDate %>"></asp:CompareValidator>
                    <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToCompare="tb_from"
                        ControlToValidate="tb_to" Display="Dynamic" ErrorMessage="CompareValidator" Operator="GreaterThanEqual"
                      Text="<%$Resources:Tokens,DatePeriodMsg %>"  Type="Date"></asp:CompareValidator>
                </div>
                <p>
                    <br/>
                    <asp:Button ID="b_show" runat="server" Text="<%$Resources:Tokens,Show %>" Width="90px" CssClass="btn btn-success" OnClick="b_show_Click" />
                </p>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="p_results">
            <h3 class="header smaller lighter blue"><asp:Literal runat="server" Text="<%$Resources:Tokens,Results %>"></asp:Literal></h3>
            <div runat="server" id="Print">
                <asp:GridView ID="gv_calls" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    CssClass="table table-bordered table-condensed" GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="CustomerName" HeaderText="<%$Resources:Tokens,Customer %>" />
                        <asp:BoundField DataField="Responsible" HeaderText="<%$Resources:Tokens,Responsible %>" />
                        <asp:BoundField DataField="Topic" HeaderText="<%$Resources:Tokens,Topic %>" />
                        <asp:BoundField DataField="Result" HeaderText="<%$Resources:Tokens,Result %>" />
                        <asp:BoundField DataField="VisitDate" DataFormatString="{0:d}" HeaderText="<%$Resources:Tokens,VisitDate %>" />
                        <asp:BoundField DataField="EmployeeName" HeaderText="<%$Resources:Tokens,Employee %>" />
                        <asp:BoundField DataField="Address" HeaderText="<%$Resources:Tokens,Address %>" />
                        <asp:BoundField DataField="CallDate" HeaderText="Call date" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                
                                    <asp:LinkButton ID="gv_lb_edit" runat="server"  CommandArgument='<%# Eval("Id") %>' ToolTip="<%$Resources:Tokens,Edit %>"
                                        OnClick="gv_lb_edit_Click" data-rel="tooltip"><i class="icon-pencil bigger-130 blue"></i></asp:LinkButton>
                                &nbsp;&nbsp;
                                    <asp:LinkButton ID="gv_lb_delete" runat="server" CommandArgument='<%# Eval("Id") %>' ToolTip="<%$Resources:Tokens,Delete %>"
                                        OnClick="gv_lb_delete_Click" data-rel="tooltip"><i class="icon-trash bigger-130 red"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle BackColor="#7C6F57" />
                    <EmptyDataTemplate>
                        <p>
                            <asp:Label runat="server" ID="gv_l_message" Text="<%$Resources:Tokens,NoResults %>"></asp:Label></p>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
            <p>
                <asp:Button ID="Button1" runat="server" Text="<%$Resources:Tokens,Export %>" Width="90px" CssClass="btn btn-primary" OnClick="Button1_Click" />
            </p>
        </asp:Panel>
    </div>
</asp:Content>
