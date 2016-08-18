<%@ Page Title="<%$Resources:Tokens,HandleComplaints%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="HandleComplaints.aspx.cs" Inherits="NewIspNL.Pages.HandleComplaints" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<script src="../js/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.custom.min.js" type="text/javascript"></script>--%>
    <div class="view">
        <div id="message">
            <asp:Label runat="server" ID="l_message" ClientIDMode="Static"></asp:Label></div>
        <div>
            <asp:Panel runat="server" ID="container">
                <div class="page-header"><h1><asp:Literal runat="server" Text="<%$Resources:Tokens,Options%>"></asp:Literal></h1></div>
                <div class="well">
                <div>
                    <asp:Label ID="l_from" runat="server" Text="<%$Resources:Tokens,From%>"></asp:Label></div>
                <div>
                    <asp:TextBox ID="tb_from" runat="server" Width="200px" ClientIDMode="Static"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Tokens,Required%>"
                        Display="Dynamic" ControlToValidate="tb_from">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator5" runat="server" ControlToValidate="tb_from"
                        Display="Dynamic" ErrorMessage="<%$Resources:Tokens,ValidDate%>" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                </div>
                <div>
                    <asp:Label ID="l_to" runat="server" Text="<%$Resources:Tokens,To%>"></asp:Label></div>
                <div>
                    <asp:TextBox ID="tb_to" runat="server" Width="200px" ClientIDMode="Static"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Tokens,Required%>"
                        Display="Dynamic" ControlToValidate="tb_to">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="tb_to"
                        Display="Dynamic" ErrorMessage="<%$Resources:Tokens,ValidDate%>" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                    <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToCompare="tb_from"
                        ControlToValidate="tb_to" Display="Dynamic" ErrorMessage="<%$Resources:Tokens,DatePeriodMsg%>" Operator="GreaterThanEqual"
                        Type="Date"></asp:CompareValidator>
                </div>
                <p>
                    <br/>
                    <asp:Button ID="b_show" runat="server" CssClass="btn btn-success" Text="<%$Resources:Tokens,Show%>" Width="90px" OnClick="b_show_Click" />
                </p>
                </div>
            </asp:Panel>
            <div style="margin-top: 15px;">
                <asp:Panel runat="server" ID="p_results">
                    <h3 class="header smaller lighter blue"><asp:Literal runat="server" ID="lblres" Text="<%$Resources:Tokens,Results%>"></asp:Literal></h3>
                    <asp:GridView runat="server" ID="gv_items" ClientIDMode="Static"
                      OnDataBound="gv_items_DataBound" AutoGenerateColumns="False"
                      CssClass="table table-bordered table-condensed text-center">
                        <Columns>
                            <asp:TemplateField HeaderText="#">
                                <ItemTemplate>
                                    <asp:Label ID="l_Number" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Customer" HeaderText="<%$Resources:Tokens,Customer%>" />
                            <asp:BoundField DataField="Governate" HeaderText="<%$Resources:Tokens,Governrate%>" />
                            <asp:BoundField DataField="Phone" HeaderText="<%$Resources:Tokens,Phone%>" />
                            <asp:BoundField DataField="complaint" HeaderText="<%$Resources:Tokens,Complaint%>" />
                            <asp:BoundField DataField="Date" HeaderText="<%$Resources:Tokens,Date%>" />
                            <asp:TemplateField HeaderText="<%$Resources:Tokens,Handle%>">
                                <ItemTemplate>
                                    <asp:LinkButton ID="Button1" runat="server" CommandArgument='<%# Bind("Id") %>' OnClick="Button1_Click"
                                        ToolTip="<%$Resources:Tokens,Handle%>" data-rel="tooltip" CssClass="btn btn-primary btn-sm"><i class="icon-ok bigger-120"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div>
                               <%=Tokens.NoResults %></div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </asp:Panel>
            </div>
        </div>
    </div>
        <script src="../Content/ace-assest/js/jquery-ui-1.10.3.full.min.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $('#tb_from').datepicker({ dateFormat: 'dd-mm-yy' });
            $('#tb_to').datepicker({ dateFormat: 'dd-mm-yy' });

            if ($('#l_message').html() === '') {
                $('#message').css("border", "none");
            } else {
                $('#message').addClass('alert alert-success');
                /*.css("border", "silver solid 1px")
                .css("padding", "4px")
                .css("-moz-box-shadow", "0 0 1px #888")
                .css("-webkit-box-shadow", " 0 0 1px#888")
                .css("box-shadow", "0 0 1px #888");*/
            }

            /*$("#gv_items tr")
                .not(':first')
                .hover(function () {
                    $(this)
                        .css("background-color", "silver");
                },
                    function () {
                        $(this)
                            .css("background-color", "");
                    });*/
        });
    </script>
</asp:Content>
