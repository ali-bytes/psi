<%@ Page Title="<%$Resources:Tokens,CompanyHistory%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="CompanyHistory.aspx.cs" Inherits="NewIspNL.Pages.CompanyHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 
    <script>

        $(document).ready(function () {
            $('#TbFrom').datepicker({ dateFormat: 'dd-mm-yy' });
            $('#TbTo').datepicker({ dateFormat: 'dd-mm-yy' });
        });

    </script>
    <div class="page-header"><h1><asp:Literal ID="Literal3" Text="<%$Resources:Tokens,CompanyHistory%>" runat="server" /></h1></div>
    <div class="view">
        <fieldset>
        <div class="well">
                                                        <label for="TbFrom">
                            <asp:Literal ID="Literal2" Text="<%$Resources:Tokens,From%>" runat="server" /></label>
                        <div>
                            <asp:TextBox runat="server" ID="TbFrom" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TbFrom"
                                ErrorMessage="<%$Resources:Tokens,Required%>" Text="<%$Resources:Tokens,Required%>"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="TbFrom"
                                Display="Dynamic" ErrorMessage="<%$Resources:Tokens,ValidDate%>" Text="<%$Resources:Tokens,ValidDate%>"
                                Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                        </div>
                        <label for="TbTo">
                            <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,To%>" runat="server" /></label>
                        <div>
                            <asp:TextBox runat="server" ClientIDMode="Static" ID="TbTo"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TbTo"
                                ErrorMessage="<%$Resources:Tokens,Required%>" Text="<%$Resources:Tokens,Required%>"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="TbTo"
                                Display="Dynamic" ErrorMessage="<%$Resources:Tokens,ValidDate%>" Text="<%$Resources:Tokens,ValidDate%>"
                                Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="TbTo"
                                ControlToValidate="TbFrom" Display="Dynamic" ErrorMessage="<%$Resources:Tokens,DatePeriodMsg%>"
                                Text="<%$Resources:Tokens,DatePeriodMsg%>" Operator="LessThan" Type="Date"></asp:CompareValidator>
                        </div>

                        <p>
                            <br/>
                    <asp:Button runat="server" CssClass="btn btn-success" ID="BSearch" OnClick="BSearch_OnClick" Text="<%$Resources:Tokens,Search%>" />
                </p>
                    </div>
                            </fieldset>
    </div>
        <div class="view">
            <fieldset>
                <legend>
                    <asp:Literal Text="<%$Resources:Tokens,Items%>" runat="server" /></legend>
                <div>
                    <asp:GridView ID="GItems" ClientIDMode="Static" runat="server" 
                        GridLines="Horizontal" AutoGenerateColumns="False"
                        CssClass="table table-bordered table-condensed">
                        <Columns>
                            <asp:BoundField DataField="Time" HeaderText="<%$ Resources:Tokens,Time %>" />
                            <asp:BoundField DataField="Quantity" HeaderText="<%$ Resources:Tokens,Quantity %>" />
                            <asp:BoundField DataField="Operation" HeaderText="<%$ Resources:Tokens,Operation %>" />
                            <asp:BoundField DataField="Consumer" HeaderText="<%$ Resources:Tokens,Consumer %>" />
                        </Columns>
                        <EmptyDataTemplate>
                            <div>
                                <asp:Literal Text="<%$Resources:Tokens,NoResults%>" runat="server" /></div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </fieldset>
        </div>

</asp:Content>
