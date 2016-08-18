<%@ Page Title="<%$Resources:Tokens,RechargeResellerReport %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="RechargeResellerReport.aspx.cs" Inherits="NewIspNL.Pages.RechargeResellerReport" %>


<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
<link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>

            <div class="view">
        <fieldset>
            <div class="page-header"><h1><asp:Label ID="Label1" runat="server" Text="<%$Resources:Tokens,RechargeResellerReport %>"></asp:Label></h1></div>
            <div class="well">
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,Reseller%>"></asp:Literal>
                <div>
                    <asp:DropDownList runat="server" CssClass="width-60 chosen-select" ID="ddlReseller" Width="178px"/>
                </div>
                 <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Tokens,From %>"></asp:Label>
                            <div>
                            <asp:TextBox ID="tb_from" runat="server" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tb_from"
                                                        Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="tb_from"
                                                  Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,ValidDate %>" Operator="DataTypeCheck"
                                                  Type="Date"></asp:CompareValidator>
                                                  </div>
                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Tokens,To %>"></asp:Label>
                            <div>
                            <asp:TextBox ID="tb_to" runat="server" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tb_to"
                                                        Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="tb_to"
                                                  Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,ValidDate %>" Operator="DataTypeCheck"
                                                  Type="Date"></asp:CompareValidator>
                            &nbsp;
                                                  </div>
                                                  <br/>
                <asp:LinkButton runat="server" ID="btnSearch"  
                    CssClass="btn btn-success" onclick="btnSearch_Click"><i class="icon-search icon-only"></i>&nbsp;<asp:Literal runat="server" Text="<%$Resources:Tokens,Search %>"></asp:Literal></asp:LinkButton>
            </div>
        </fieldset>
    </div>
    <div class="view">
        <fieldset>
                <asp:GridView ID="grd" runat="server" Width="100%" 
                              AutoGenerateColumns="False" DataKeyNames="ID" 
                              CssClass="table table-bordered table-condensed text-center"
                              GridLines="None" OnDataBound="grd_DataBound">
                    <Columns>
                                                    <asp:TemplateField HeaderText="#">
                                <ItemTemplate>
                                    <asp:Label ID="l_Number" runat="server" Text="Label"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,Reseller %>" />
                        <asp:BoundField DataField="BoxName" HeaderText="<%$Resources:Tokens,Box %>" />
                        <asp:BoundField DataField="Time" HeaderText="<%$Resources:Tokens,RequestDate%>" />
                        <asp:BoundField DataField="DepositorName" HeaderText="<%$Resources:Tokens,Depositor %>" />
                        <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Amount %>" />
                                                <asp:TemplateField HeaderText="<%$Resources:Tokens,RequestStatus%>">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblApproved" Text='<%#Eval("IsApproved") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,DirectingBalanceTo%>">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lbldirection" Text='<%#Eval("CreditORVoice") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RejectReason" HeaderText="<%$Resources:Tokens,RejectReason %>"/>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,Attachments %>">
                            <ItemTemplate>
                                <a href='<%#Eval("Url") %>' class="icon-paper-clip icon-only bigger-130" target="_blank" data-rel="tooltip" title="<%=Tokens.Download %>"></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=Tokens.NoResults %>
                            </EmptyDataTemplate>
                </asp:GridView>

        </fieldset>
    </div>
 
        <script type="text/javascript">
            $(document).ready(function () {
                $(".chosen-select").chosen();
                $('#tb_from').datepicker({ dateFormat: 'dd-mm-yy' });
                $('#tb_to').datepicker({ dateFormat: 'dd-mm-yy' });
            });

    </script>
</asp:Content>


