<%@ Page Title="<%$Resources:Tokens,EmployeesReports%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="EmployeesReports.aspx.cs" Inherits="NewIspNL.Pages.EmployeesReports" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view">
        <div id="message">
            <asp:Label runat="server" ID="l_message" ClientIDMode="Static"></asp:Label></div>
        <asp:Panel runat="server" ID="p_entery">
            <div class="page-header"><h1><asp:Literal runat="server" Text="<%$Resources:Tokens,Phones%>"></asp:Literal></h1></div>
            <div class="well">
            <div>
                <asp:Label runat="server" ID="Label2" Text="<%$Resources:Tokens,Employee%>"></asp:Label></div>
            <div>
                <asp:DropDownList runat="server" ID="ddl_eployees" Width="200px">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                    ControlToValidate="ddl_eployees"></asp:RequiredFieldValidator>
            </div>
            <div>
                <asp:Label runat="server" ID="Label1" Text="<%$Resources:Tokens,States%>"></asp:Label></div>
            <div>
                <asp:DropDownList ID="ddl_states" runat="server" Width="200px">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                    ControlToValidate="ddl_states"></asp:RequiredFieldValidator>
            </div>
            <p>
                <br/>
                <asp:LinkButton runat="server" ID="b_search" CssClass="btn btn-success" OnClick="b_search_Click"
                    Width="90px" ><i class="icon-search"></i>&nbsp;<%=Tokens.Search %></asp:LinkButton>
            </p>
            </div>
            <div style="text-align: center">
                <asp:GridView runat="server" ID="gv_items" ClientIDMode="Static"
                  OnDataBound="gv_items_DataBound" AutoGenerateColumns="False"
                  CssClass="table table-bordered table-condensed">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="l_Number" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Phone1" HeaderText="<%$Resources:Tokens,Phone%>" />
                        <asp:BoundField DataField="Name" HeaderText="<%$Resources:Tokens,Name%>" />

                        <asp:BoundField DataField="Governate" HeaderText="<%$Resources:Tokens,Governrate%>" />
                         <asp:BoundField DataField="Offer1" HeaderText="<%$Resources:Tokens,Offer1%>" />
                        <asp:BoundField DataField="Offer2" HeaderText="<%$Resources:Tokens,Offer2%>" />
                        <asp:BoundField />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=Tokens.NoResults %>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </asp:Panel>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            if ($('#l_message').html() === '') {
                $('#message').css("border", "none");
            } else {
                $('#message').css("border", "silver solid 1px").css("padding", "4px")
                    .css("-moz-box-shadow", "0 0 1px #888")
                    .css("-webkit-box-shadow", " 0 0 1px#888")
                    .css("box-shadow", "0 0 1px #888");
            }
            $("#gv_items tr").not(':first').hover(function () {
                $(this).css("background-color", "rgb(243, 255, 195)");
            }, function () {
                $(this).css("background-color", "");
            });
        });
    </script>
</asp:Content>
