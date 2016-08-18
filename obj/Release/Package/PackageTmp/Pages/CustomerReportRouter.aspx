<%@ Page Title=" <%$Resources:Tokens,CustomerReportRouter%> " Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="CustomerReportRouter.aspx.cs" Inherits="NewIspNL.Pages.CustomerReportRouter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="view">
        <fieldset>
            <div class="page-header"><h1><asp:Literal runat="server" Text="<%$Resources:Tokens,CustomerReportRouter %>"></asp:Literal></h1></div>
            <div class="well">
<%--                <div>
                    <asp:Literal runat="server" Text="<%$Resources:Tokens,From %>"></asp:Literal>
                    <div>
                        <asp:TextBox runat="server" ID="txtFrom" ClientIDMode="Static" ValidationGroup="cus"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ErrorMessage="*" ControlToValidate="txtFrom" ValidationGroup="cus"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div>
                    <asp:Literal runat="server" Text="<%$Resources:Tokens,To %>"></asp:Literal>
                    <div>
                        <asp:TextBox runat="server" ID="txtTo" ClientIDMode="Static" ValidationGroup="cus"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic"
                        ErrorMessage="*" ControlToValidate="txtTo" ValidationGroup="cus"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="<%$Resources:Tokens,DatePeriodMsg %>"
                       ControlToCompare="txtFrom" ControlToValidate="txtTo" Type="Date" Operator="GreaterThanEqual" ></asp:CompareValidator>
                    </div>
                </div>--%>
                <div class="radio">
                      <asp:RadioButtonList runat="server" ID="RblSearch" ClientIDMode="Static" CssClass="radio">
                            <asp:ListItem Selected="True" Text="<%$Resources:Tokens,CustomerReceivedFromCompany1 %>"></asp:ListItem>
                            <%--<asp:ListItem Text="<%$Resources:Tokens,CustomerNotReceived %>"></asp:ListItem>--%>
                            <asp:ListItem Text="<%$Resources:Tokens,CustomerReceived %>"></asp:ListItem>
                      </asp:RadioButtonList>
                </div>
                <br/>
                <asp:Button runat="server" ID="btnSearch" CssClass="btn btn-primary" 
                Text="<%$Resources:Tokens,Search %>" ValidationGroup="cus" OnClick="btnSearch_click"/>
            </div>
        </fieldset>
    </div>
    <div class="view">
        <fieldset>
            <h3 class="smaller header lighter blue"><asp:Literal runat="server" Text="<%$Resources:Tokens,Results %>"></asp:Literal></h3>
            <asp:GridView ID="grd_wo" runat="server" CssClass="table table-bordered table-condensed"
                    ClientIDMode="Static" AutoGenerateColumns="False" DataKeyNames="ID" OnDataBound="grd_wo_DataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="l_number" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustomerName" HeaderText="<%$ Resources:Tokens,Customer %>">
                        </asp:BoundField>
                        <asp:BoundField DataField="CustomerPhone" HeaderText="<%$ Resources:Tokens,Phone %>">
                        </asp:BoundField>
                        <asp:BoundField HeaderText="<%$Resources:Tokens,RequestNumber %>" DataField="RequestNumber" />
                        <asp:BoundField DataField="GovernorateName" HeaderText="<%$ Resources:Tokens,Governrate%>">
                        </asp:BoundField>
                        <asp:BoundField DataField="CentralName" HeaderText="<%$Resources:Tokens,Central %>"/>
                        <asp:BoundField DataField="ServicePackageName" HeaderText="<%$ Resources:Tokens,Package %>">
                        </asp:BoundField>
                        <asp:BoundField DataField="StatusName" HeaderText="<%$ Resources:Tokens,State%>">
                        </asp:BoundField>
                        <asp:BoundField DataField="SPName" HeaderText="<%$ Resources:Tokens,Provider %>">
                        </asp:BoundField>
                        <asp:BoundField DataField="BranchName"  HeaderText="<%$ Resources:Tokens,Branch %>">
                        </asp:BoundField>
                        <asp:BoundField DataField="Reseller" HeaderText="<%$ Resources:Tokens,Reseller %>">
                        </asp:BoundField>
                        <%--<asp:BoundField DataField="CreationDate" HeaderText="<%$Resources:Tokens,CreationDate %>"/>
                        <asp:BoundField DataField="Title" HeaderText="<%$ Resources:Tokens,Offer %>"/>
                        <asp:BoundField DataField="RequestDate" HeaderText="<%$ Resources:Tokens,InvoiceDueDate %>"/>--%>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Tokens,NoResults %>"></asp:Label>
                    </EmptyDataTemplate>
                </asp:GridView>
        </fieldset>
    </div>
    <%--<script type="text/javascript">
        $(document).ready(function () {
            $('#txtTo').datepicker({ dateFormat: 'dd/mm/yy' });
            $('#txtFrom').datepicker({ dateFormat: 'dd/mm/yy' });
        })
    </script>--%>
</asp:Content>

