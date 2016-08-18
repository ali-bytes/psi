<%@ Page Title="<%$Resources:Tokens,ContractsByEmployee%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ContractsForEmployee.aspx.cs" Inherits="NewIspNL.Pages.ContractsForEmployee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-header"><h1><asp:Literal runat="server" Text="<%$Resources:Tokens,ContractsCount%>"></asp:Literal></h1></div>
    <div class="row">
        <asp:Panel runat="server" ID="p_count">
            <div style="line-height: 14px; margin-top: 10px;">
                <asp:GridView runat="server" ID="gv_counts" ClientIDMode="Static"
                 AutoGenerateColumns="False" OnDataBound="gv_counts_DataBound"
                 CssClass="table table-bordered table-condensed text-center">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" HeaderText="<%$Resources:Tokens,Name%>" />
                        <asp:BoundField DataField="Phone" HeaderText="<%$Resources:Tokens,Phone%>" />
                        <asp:BoundField DataField="Count" HeaderText="<%$Resources:Tokens,Count%>" />
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>
    </div>
    <script src="../Content/ace-assest/js/jquery-ui-1.10.3.full.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript">
        $(document).ready(function () {

            $("#gv_counts tr").not(':first').hover(function () {
                $(this).css("background-color", "silver");
            }, function () {
                $(this).css("background-color", "");
            });
        });
    </script>--%>
</asp:Content>
