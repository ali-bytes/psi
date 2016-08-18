<%@ Page Title="<%$Resources:Tokens,PreviewPhonesStates%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="PreviewPhonesStates.aspx.cs" Inherits="NewIspNL.Pages.PreviewPhonesStates" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view">
        <div>
            <asp:Panel runat="server" ID="containerPanel" ClientIDMode="Static">
                <div class="page-header"><h1><asp:Literal runat="server" Text="<%$Resources:Tokens,Phones%>"></asp:Literal></h1></div>
                <div style="padding: 15px;">
                    <div class="well">
                    <div>
                        <asp:DropDownList ID="ddl_states" runat="server" Width="200px">
                        </asp:DropDownList>
                    </div>
                    <p>
                        <br/>
                        <asp:Button ID="b_preview" runat="server" CssClass="btn btn-success" Text="<%$Resources:Tokens,Preview%>" OnClick="b_preview_Click" />
                    </p>
                        </div>
                    <div style="margin-top: 15px;">
                        <asp:GridView runat="server" ID="gv_items" 
                        CssClass="table table-bordered table-responsive text-center"
                        ClientIDMode="Static"  OnDataBound="gv_items_DataBound" Width="100%" AutoGenerateColumns="False">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="l_Number" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Phone1" HeaderText="<%$Resources:Tokens,Phone%>" />
                                <asp:BoundField DataField="Name" HeaderText="<%$Resources:Tokens,Name%>" />
                                <asp:BoundField DataField="Governate" HeaderText="<%$Resources:Tokens,Governrate%>" />
                                <asp:BoundField DataField="Offer1" HeaderText="<%$Resources:Tokens,Offer1%>" />
                                <asp:BoundField DataField="Offer2" HeaderText="<%$Resources:Tokens,Offer2%>" />
                                <asp:BoundField DataField="State" HeaderText="<%$Resources:Tokens,State%>" />
                                <asp:BoundField DataField="Employee" HeaderText="<%$Resources:Tokens,Employee%>" />
                                <asp:BoundField DataField="Comments" HeaderText="<%$Resources:Tokens,Comment%>" />
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="b_approve" runat="server" Text="<%$Resources:Tokens,Approve%>" CssClass="btn btn-success" CommandArgument='<%# Bind("Id") %>' />
                                                </td>
                                                <td>
                                                    <asp:Button ID="b_reject" runat="server" Text="<%$Resources:Tokens,Reject%>" CssClass="btn btn-danger" CommandArgument='<%# Bind("Id") %>'
                                                        ToolTip='<%# Bind("Id") %>' ClientIDMode="Static" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div>
                                    <%=Tokens.NoResults %></div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            $("#gv_items tr")
                .not(':first')
                .hover(function () {
                    $(this)
                        .css("background-color", "rgb(243, 255, 195)");
                },
                    function () {
                        $(this)
                            .css("background-color", "");
                    });
        });
    </script>
</asp:Content>

