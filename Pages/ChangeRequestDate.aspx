<%@ Page Title="<%$Resources:Tokens,ChangeRequestDate%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ChangeRequestDate.aspx.cs" Inherits="NewIspNL.Pages.ChangeRequestDate" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-header"><h1><%=Tokens.ChangeRequestDate %></h1></div>
    <div class="view">
        <fieldset>
          
            <div class="well">
            <div id="GovDiv" runat="server">
                <div>
                    <asp:Label ID="Label34" runat="server" Text="<%$Resources:Tokens,Governrate%>"></asp:Label>
                </div>
                <div>
                    <asp:DropDownList ID="ddl_Governorates" runat="server" Width="155px" DataTextField="GovernorateName"
                                      DataValueField="ID">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ControlToValidate="ddl_Governorates" ID="RequiredFieldValidator12"
                                                runat="server"  ErrorMessage="<%$Resources:Tokens,Required%>" ValidationGroup="SearchVG"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div>
                <div>
                    <asp:Label ID="Label31" runat="server" Text="<%$Resources:Tokens,Phone%>"></asp:Label>
                </div>
                <div>
                    <asp:TextBox ID="txt_CustomerPhone" runat="server" Width="150px" ClientIDMode="Static"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Tokens,Required%>"
                                                ControlToValidate="txt_CustomerPhone" ValidationGroup="SearchVG"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div>
                <div>
                    <br />
                    <asp:Button ID="btn_search" runat="server" CssClass="btn btn-success" ClientIDMode="Static" Text="<%$Resources:Tokens,Search%>" Width="100px" OnClick="btn_search_Click"
                                ValidationGroup="SearchVG" UseSubmitBehavior="True" />
                    <asp:Label ID="lbl_SearchResult" runat="server" EnableViewState="False" Font-Bold="True"
                               ForeColor="Red"></asp:Label>
                    <br />
                </div>
            </div>
            </div>
        </fieldset>
    </div>
    <div style=" padding: 20px;">
        <asp:GridView runat="server" ID="gv_requests" AutoGenerateColumns="False"
            CssClass="table table-bordered table-condensed text-center">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="CustomerName" HeaderText="<%$Resources:Tokens,Customer%>" />
                <asp:BoundField DataField="CustomerPhone" HeaderText="<%$Resources:Tokens,Phone%>" />
                <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Tokens,Branch%>" />
                <asp:BoundField DataField="ServicePackageName" HeaderText="<%$Resources:Tokens,Package%>" />
                <asp:BoundField DataField="GovernorateName" HeaderText="<%$Resources:Tokens,Governrate%>" />
                <asp:TemplateField HeaderText="<%$Resources:Tokens,Request.Date%>">
                    <ItemTemplate>
                        <%--Don't remove css class it is for JQuery--%>
                        <asp:TextBox CssClass="fordate" ID="tb_date" runat="server" Text='<%# Bind("UpdateDate") %>' Width="90px"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="Button1" runat="server" CommandArgument='<%# Bind("ID") %>' OnClick="Button1_Click"
                            ToolTip="<%$Resources:Tokens,Save%>" data-rel="tooltip"><i class="icon-ok-sign bigger-150 green icon-only"></i></asp:LinkButton>
                        <asp:HiddenField ID="hf_id" runat="server" Value='<%# Bind("ID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EditRowStyle BackColor="#7C6F57" />
            <EmptyDataTemplate>
                <span><%=Tokens.NoResults%></span>
            </EmptyDataTemplate>
        </asp:GridView>
        <div id="myMessage" style="text-align: center; padding: 15px; color: rgb(0, 134, 14);
            font-size: 16px;">
            <asp:Label ID="l_message" runat="server"></asp:Label>
        </div>
    </div>
 
    <script type="text/javascript">
        $(document).ready(function () {
            $('.fordate').datepicker({ dateFormat: 'dd/mm/yy' });
            $('#txt_CustomerPhone').keypress(function (e) {
                var key = e.which;
                if (key == 13) {
                    $('#btn_search').click();
                    return false;
                } else {
                    return true;
                }
            });
        });
        window.setInterval(hideMessage(), 10000);
        function hideMessage() {
            $('#myMessage').hide("slow");
        }
    </script>
</asp:Content>
