<%@ Page Title="<%$Resources:Tokens,MenuBranchDirectCustomers%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="BranchDirectCustomers.aspx.cs" Inherits="NewIspNL.Pages.BranchDirectCustomers" %>
<%@ Import Namespace="Resources" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%">
        <tr>
            <td>
                <div class="row">
                <fieldset>
                    <div class="page-header">
                        <h1><asp:Label ID="l_SearchCriteria" runat="server" Text="<%$Resources:Tokens,SearchOptions %>"></asp:Label></h1>
                    </div>
                    <div class="well">
                    <table width="100%">
                        <tr>
                            <td style="width: 61px">
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Tokens,Branch %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:DropDownList ID="ddl_Branch" runat="server" Width="155px" DataTextField="BranchName"
                                    DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                            <td colspan="3">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                    ControlToValidate="ddl_Branch" ValidationGroup="Insert" ></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="5">
                                <br/>
                                <asp:Button ID="btn_search" runat="server" Text="<%$Resources:Tokens,Search %>" Width="55px" ValidationGroup="Insert"
                                    OnClick="btn_search_Click" CssClass="btn btn-success"/>
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
                <div class="row">
                <asp:GridView ID="grd_Customers" Width="100%" runat="server" 
                    GridLines="None" AutoGenerateColumns="False" 
                    ondatabound="grd_Customers_DataBound1"
                    CssClass="table table-bordered table-condensed">
                    <Columns>
                             <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="gv_l_number" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustomerName" HeaderText="<%$Resources:Tokens,Customer.Name %>">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CustomerPhone" HeaderText="<%$Resources:Tokens,Customer.Phone %>">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="GovernorateName" HeaderText="<%$Resources:Tokens,Governrate %>">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ServicePackageName" HeaderText="<%$Resources:Tokens,Service.Package %>">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SPName" HeaderText="<%$Resources:Tokens,Service.Provider %>">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="StatusName" HeaderText="<%$Resources:Tokens,Status %>">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UpdateDate" HeaderText="<%$Resources:Tokens,Date %>">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                    </Columns>
                    
                </asp:GridView>
                <p><asp:LinkButton runat="server" ID="Exprot" CssClass="btn btn-primary" onclick="Exprot_Click"><%=Tokens.Export %>&nbsp;<i class="icon-file icon-only"></i></asp:LinkButton></p>
                </div>
            </td>
        </tr>
    </table>
    
</asp:Content>
