<%@ Page Title="<%$Resources:Tokens,OutgoingReport %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="OutgoingReport.aspx.cs" Inherits="NewIspNL.Pages.OutgoingReport" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div class="view">
        <fieldset>
            <div class="page-header"><h1><asp:Label ID="Label1" runat="server" Text="<%$Resources:Tokens,OutgoingReport %>"></asp:Label></h1></div>
            <div class="well">
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,OutgoingTypes%>"></asp:Literal>
                <div>
                    <asp:DropDownList runat="server" ID="ddlIncomeType" DataTextField="Name" DataValueField="Id" Width="178px"/>
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
                <asp:Button runat="server" ID="btnSearch" Text="<%$Resources:Tokens,Search %>" 
                    CssClass="btn btn-success" onclick="btnSearch_Click" ClientIDMode="Static" Width="83px"/>
                    <asp:Button runat="server" ID="btnTotals" CssClass="btn btn-primary" OnClick="GetTotal" Text="<%$Resources:Tokens,Totals %>"/>
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
                            <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Tokens,BranchName %>"/>
                            <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Amount %>" />
                            <asp:BoundField DataField="Comment" HeaderText="<%$Resources:Tokens,Comment %>" />
                            <asp:BoundField DataField="Date" HeaderText="<%$Resources:Tokens,Date %>" DataFormatString="{0:d}" />
                            <asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,UserName %>" />
                            <asp:BoundField DataField="Name" HeaderText="<%$Resources:Tokens,OutgoingTypes %>"/>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=Tokens.NoResults %>
                            </EmptyDataTemplate>
                </asp:GridView>
               <div class="alert alert-info" style="margin-top: 15px;">
                    <table>
                        <tr>
                            <td><%=Tokens.Total %></td>
                            <td> : </td>
                            <td><asp:Label runat="server" ID="lblTotal"></asp:Label></td>
                        </tr>
                    </table>
                    
                </div>
        </fieldset>
    </div>
    <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue"><asp:Label runat="server" Text="<%$Resources:Tokens,ExpensesTotal %>"></asp:Label></h3>
             <asp:GridView ID="GVTotals" runat="server" Width="100%" 
                              AutoGenerateColumns="False"
                              CssClass="table table-bordered table-condensed text-center"
                              GridLines="None" OnDataBound="GVT_DataBound">
                    <Columns>
                                                    <asp:TemplateField HeaderText="#">
                                <ItemTemplate>
                                    <asp:Label ID="l_Number" runat="server" Text="Label"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="<%$Resources:Tokens,OutgoingTypes %>" DataField="Name"/>
                            <asp:BoundField HeaderText="<%$Resources:Tokens,Total %>" DataField="Total"/>
                            </Columns>
                            </asp:GridView>
        </fieldset>
    </div>
 
    <script>

        $(document).ready(function () {
            $('#tb_from').datepicker({ dateFormat: 'dd-mm-yy' });
            $('#tb_to').datepicker({ dateFormat: 'dd-mm-yy' });
         
        });
    </script>
</asp:Content>



