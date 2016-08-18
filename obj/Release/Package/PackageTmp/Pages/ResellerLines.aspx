<%@ Page Title="<%$ Resources:Tokens,ResellerLines %>" EnableEventValidation="false" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ResellerLines.aspx.cs" Inherits="NewIspNL.Pages.ResellerLines" %>


<%@ Import Namespace="NewIspNL.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%">
        <tr>
            <td>
                <div class="view">
                <fieldset>
                    <div class="page-header"><h1>
                        <asp:Label ID="Label34" runat="server" Text="<%$ Resources:Tokens,SearchOptions %>"></asp:Label></h1>
                    </div>
                    <div class="well">
                    <table>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Tokens,Reseller %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:DropDownList ID="ddl_Reseller" runat="server" Width="155px" DataTextField="UserName"
                                                  DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                                            ControlToValidate="ddl_Reseller" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td colspan="2"><br/>
                                <asp:Button ID="btn_search0" runat="server" Text="<%$ Resources:Tokens,Search %>"
                                            Width="100px" OnClick="btn_search0_Click" CssClass="btn btn-success" ValidationGroup="Insert"/>
                            </td>
                        </tr>
                    </table>
                    </div>
                </fieldset>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="view">
                <fieldset>
                   <h3 class="header smaller lighter blue">
                        <asp:Label ID="Label35" runat="server" Text="<%$ Resources:Tokens,Results %>"></asp:Label>
                    </h3>
                    <div>
                        <asp:GridView ID="grd_wo" runat="server" AutoGenerateColumns="False" Width="100%"
                                      GridLines="Horizontal" 
                                      OnRowDataBound="grd_wo_RowDataBound" CssClass="table table-bordered table-condensed">
                            <Columns>
                                <asp:TemplateField HeaderText="#">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_No" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CustomerPhone" HeaderText="<%$ Resources:Tokens,Phone %>">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="GovernorateName" HeaderText="<%$ Resources:Tokens,Governrate %>">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CustomerName" HeaderText="<%$ Resources:Tokens,Customer %>">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="StatusName" HeaderText="<%$ Resources:Tokens,Status %>">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SPName" HeaderText="<%$ Resources:Tokens,Provider%>">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="BranchName" HeaderText="<%$ Resources:Tokens,Branch%>">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ServicePackageName" HeaderText="<%$ Resources:Tokens,Package %>" />
                                <asp:BoundField DataField="ResellerName" HeaderText="<%$ Resources:Tokens,Reseller%>">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CreationDate" HeaderText="<%$ Resources:Tokens,CreationDate %>">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="<%$ Resources:Tokens,Details %>">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="HyperLink1" CssClass="btn btn-info btn-sm" target="_blank" runat="server" NavigateUrl='<%#string.Format("Search.aspx?sm=s&id={0}",QueryStringSecurity.Encrypt(Eval("ID").ToString())) %>'
                                                       title="<%$Resources:Tokens,Details%>" data-rel="tooltip"><i class="icon-building icon-only"></i></asp:HyperLink>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Tokens,ViewDetails %>">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnb_Edit" runat="server" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-default btn-sm"
                                                 data-rel="tooltip" OnClick="lnb_Edit_Click" ToolTip="<%$Resources:Tokens,Details%>"><i class="icon-file-text-alt icon-only"></i></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HiddenField runat="server" ID="hdfStatus" Value='<%#Eval("WorkOrderStatusID") %>'/>
                                        <asp:LinkButton runat="server" data-rel="tooltip" ID="lnkActive" OnClick="btn_Active_Click" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-success btn-sm" ToolTip="<%$Resources:Tokens,MenuReactivate %>"><i class="icon-only icon-ok"></i></asp:LinkButton>
                                        <asp:LinkButton runat="server" data-rel="tooltip" ID="lnkAutoSuspend" OnClick="btn_AutoSuspend_Click" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-primary btn-sm" ToolTip="<%$Resources:Tokens,AutoSuspend %>"><i class="icon-only icon-ban-circle"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Tokens,NoResults %>"></asp:Label>
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <div style="text-align: center">
                            <asp:LinkButton runat="server" CssClass="btn btn-primary" ID="b_export" OnClick="b_export_Click"
                                        Width="100px"><i class="icon-file"></i>&nbsp;<asp:Literal runat="server" Text="<%$Resources:Tokens,Export %>"></asp:Literal></asp:LinkButton>
                        </div>
                    </div>
                </fieldset>
                </div>
            </td>
        </tr>
        <tr id="tr_CustomerDetails" runat="server" visible="false">
            <td>
                <fieldset>
                    <h3 class="header smaller lighter blue">
                        <asp:Label ID="Label36" runat="server" Text="<%$ Resources:Tokens,Customer.Details %>"></asp:Label>
                    </h3>
                    <table width="100%">
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Tokens,Customer.Name%>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:Label ID="lbl_CustomerName" runat="server"></asp:Label>
                            </td>
                            <td style="width: 147px">
                                <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Tokens,Customer.City %>"></asp:Label>
                            </td>
                            <td style="width: 167px">
                                <asp:Label ID="lbl_GovernorateName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label33" runat="server" Text="<%$ Resources:Tokens,Customer.Address %>"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lbl_CustomerAddress" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Tokens,Customer.Phone %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:Label ID="lbl_CustomerPhone" runat="server"></asp:Label>
                            </td>
                            <td style="width: 147px">
                                <asp:Label ID="Label16" runat="server"></asp:Label>
                            </td>
                            <td style="width: 167px">
                                <asp:Label ID="lbl_IpPackageName" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Tokens,Customer.Mobile %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:Label ID="lbl_CustomerMobile" runat="server"></asp:Label>
                            </td>
                            <td style="width: 147px">
                                <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Tokens,Customer.Email %>"></asp:Label>
                            </td>
                            <td style="width: 167px">
                                <asp:Label ID="lbl_CustomerEmail" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Tokens,Provider %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:Label ID="lbl_SPName" runat="server"></asp:Label>
                            </td>
                            <td style="width: 147px">
                                <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Tokens,Service.Package %>"></asp:Label>
                            </td>
                            <td style="width: 167px">
                                <asp:Label ID="lbl_ServicePackageName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label24" runat="server" Text="Reseller:"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:Label ID="lbl_ResellerName" runat="server"></asp:Label>
                            </td>
                            <td style="width: 147px">
                                <asp:Label ID="Label25" runat="server" Text="<%$ Resources:Tokens,Branch %>"></asp:Label>
                            </td>
                            <td style="width: 167px">
                                <asp:Label ID="lbl_BranchName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Tokens,VPI %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:Label ID="lbl_VPI" runat="server"></asp:Label>
                            </td>
                            <td style="width: 147px">
                                <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Tokens,VCI %>"></asp:Label>
                            </td>
                            <td style="width: 167px">
                                <asp:Label ID="lbl_VCI" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label28" runat="server" Text="<%$ Resources:Tokens,UserName %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:Label ID="lbl_Client_UserName" runat="server"></asp:Label>
                            </td>
                            <td style="width: 147px">
                                <asp:Label ID="Label29" runat="server" Text="<%$ Resources:Tokens,Status %>"></asp:Label>
                            </td>
                            <td style="width: 167px">
                                <asp:Label ID="lbl_StatusName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Tokens,Password %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:Label ID="lbl_Password" runat="server"></asp:Label>
                            </td>
                            <td style="width: 147px">
                                <asp:Label ID="Label32" runat="server" Text="<%$ Resources:Tokens,Extra.Gigas %>"></asp:Label>
                            </td>
                            <td style="width: 167px">
                                <asp:Label ID="lbl_ExtraGigas" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
        <tr id="tr_Status" runat="server" visible="false">
            <td>
                <fieldset>
                    <h3 class="header smaller lighter blue">
                        <asp:Label ID="Label37" runat="server" Text="<%$ Resources:Tokens,CustomerStatuseHistory %>"></asp:Label>
                    </h3>
                    <asp:GridView ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="False"
                         GridLines="None" CssClass="table table-bordered table-condensed text-center">

                        <Columns>
                            <asp:BoundField DataField="StatusName" HeaderText="<%$ Resources:Tokens,Status %>" />
                            <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:Tokens,Admin_Employee %>" />
                            <asp:TemplateField HeaderText="<%$ Resources:Tokens,Time %>">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("UpdateDate") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("UpdateDate") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <br />
                </fieldset>
            </td>
        </tr>
    </table>
</asp:Content>

