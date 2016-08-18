<%@ Page Title="<%$Resources:Tokens,ChangeActivationDate%>" EnableEventValidation="false" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ChangeActivationDate.aspx.cs" Inherits="NewIspNL.Pages.ChangeActivationDate" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-header"><h1><%= Tokens.ChangeActivationDate%></h1></div>
    <div class="view">
        <fieldset>
            <div>
                <div class="well">
                    <div runat="server" id="GovBox">
                        <div>
                            <asp:Label ID="Label34" runat="server" Text="<%$Resources:Tokens,Governrate%>"></asp:Label>
                        </div>
                        <div>
                            <asp:DropDownList ID="ddl_Governorates" runat="server" Width="155px" DataTextField="GovernorateName"
                                DataValueField="ID">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="ddl_Governorates" ID="RequiredFieldValidator12"
                                Text="<%$Resources:Tokens,Required %>" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                ValidationGroup="SearchVG"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div>
                        <div>
                            <asp:Label ID="Label31" runat="server" Text="<%$Resources:Tokens,Phone %>"></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="txt_CustomerPhone" runat="server" Width="150px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Tokens,Required%>"
                                Text="<%$Resources:Tokens,Required%>" ControlToValidate="txt_CustomerPhone" ValidationGroup="SearchVG"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                
                <div>
                    <br/>
                    <div>
                        
                        <asp:Button ID="btn_search" type="submit" UseSubmitBehavior="True" runat="server" Text="<%$Resources:Tokens,Search %>" Width="100px"
                            OnClick="btn_search_Click" ValidationGroup="SearchVG" CssClass="btn btn-primary"/>
                        <asp:Label ID="lbl_SearchResult" runat="server" EnableViewState="False" Font-Bold="True"
                            ForeColor="Red"></asp:Label>
                        <br />
                    </div>
                </div></div>
            </div>
        </fieldset>
    </div>
    <div class="view">
        <div ><!--style="direction: ltr; padding: 20px;"-->
            <asp:GridView runat="server" ID="gv_requests" AutoGenerateColumns="False"
                CellPadding="4"  GridLines="None"
                CssClass="table table-bordered table-condensed text-center">
                <Columns>
                    <asp:BoundField DataField="CustomerName" HeaderText="<%$Resources:Tokens,Customer %>" />
                    <asp:BoundField DataField="CustomerPhone" HeaderText="<%$Resources:Tokens,Phone %>" />
                    <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Tokens,Branch %>" />
                    <asp:BoundField DataField="ServicePackageName" HeaderText="<%$Resources:Tokens,Package %>" />
                    <asp:BoundField DataField="GovernorateName" HeaderText="<%$Resources:Tokens,Governrate %>" />
                    <asp:TemplateField HeaderText="<%$Resources:Tokens,Activation.Date %>">
                        <ItemTemplate>
                            <asp:TextBox ID="tb_date" runat="server" CssClass="fordate" Text='<%# Bind("UpdateDate") %>'
                                Width="90px"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="Button1" runat="server" CommandArgument='<%# Bind("ID") %>' OnClick="Button1_Click"
                                ToolTip="<%$Resources:Tokens,Save %>" data-rel="tooltip"><i class="icon-ok-sign bigger-150 green"></i></asp:LinkButton>
                            <asp:HiddenField ID="hf_id" runat="server" Value='<%# Bind("ID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EditRowStyle BackColor="#7C6F57" />
                <EmptyDataTemplate>
                    <span>
                        <asp:Literal Text="<%$Resources:Tokens,NoResults %>" runat="server" /></span>
                </EmptyDataTemplate>
                
            </asp:GridView>
            <div id="myMessage" style="text-align: center; padding: 15px; color: rgb(0, 134, 14);
                font-size: 16px;">
                <asp:Label ID="l_message" runat="server"></asp:Label>
            </div>
        </div>
    </div>
    <script src="../Content/ace-assest/js/jquery-ui-1.10.3.full.min.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $("input").keypress(function (event) {
                if (event.which === 13 || event.which === 10) {
                    event.preventDefault();
                   $("input[type='submit']").click();
                }
            });

            $('.fordate').datepicker({ dateFormat: 'dd/mm/yy' });

        });
        window.setInterval(hideMessage(), 10000);
        function hideMessage() {
            $('#myMessage').hide("slow");
        }
    </script>
</asp:Content>
