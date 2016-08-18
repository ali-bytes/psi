<%@ Page Title="<%$ Resources:Tokens,ResellerHistory %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ResellerHistory.aspx.cs" Inherits="NewIspNL.Pages.ResellerHistory" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style>#GItems{ text-align: center; width: 500px; }#GItemsCustomers{ text-align: center; width: 100%; }</style>
    <section>

        <div class="view">
            <fieldset>
                
                <div class="page-header"><h1><asp:Literal ID="Literal2" Text="<%$ Resources:Tokens,ResellerHistory %>" runat="server" /></h1></div>
                <div class="well">
                <div>
                    <label for="DResellers">
                        <asp:Literal Text="<%$ Resources:Tokens,Reseller %>" runat="server" /></label>
                    <div>
                        <asp:DropDownList runat="server" ID="DResellers" ClientIDMode="Static" Width="178px"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="DResellers"
                                                    ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div>
                    <div>
                        <label for="TbFrom"><asp:Literal ID="Literal1" Text="<%$ Resources:Tokens,From %>" runat="server" /></label>
                        <div><asp:TextBox runat="server" ID="TbFrom" ClientIDMode="Static"></asp:TextBox>
                        
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                        ControlToValidate="TbFrom" ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>

                            <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="TbFrom"
                                                  Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,ValidDate %>" Operator="DataTypeCheck"
                                                  Type="Date"></asp:CompareValidator>
                        </div>
                    </div>
                </div>
                <div>
                    <div>
                        <label for="TbTo">
                            <asp:Literal Text="<%$ Resources:Tokens,To %>" runat="server" /></label>
                        <div><asp:TextBox runat="server" ClientIDMode="Static" ID="TbTo"></asp:TextBox>
                        
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                        ControlToValidate="TbTo" ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                        
                            <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="TbTo"
                                                  Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,ValidDate %>" Operator="DataTypeCheck"
                                                  Type="Date"></asp:CompareValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="TbTo"
                                                  ControlToValidate="TbFrom" Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,DatePeriodMsg %>"
                                                  Operator="LessThan" Type="Date"></asp:CompareValidator>
                        </div>
                    </div>
                </div>
                <p>
                    <br/>
                    <asp:Button runat="server" ID="BSearch" OnClick="BSearch_OnClick" 
                                Text="<%$ Resources:Tokens,Search %>" CssClass="btn btn-success"/>
                </p>
                </div>
            </fieldset>
        </div>
        <div class="view">
            <fieldset>
                <h3 class="header smaller lighter blue">
                    <asp:Literal Text="<%$ Resources:Tokens,Stocks %>" runat="server" /></h3>
                <div>
                    <asp:GridView ID="GItems" ClientIDMode="Static" runat="server" Width="100%"
                        CssClass="table table-bordered table-condensed">
                        <EmptyDataTemplate>
                            <div>
                                <asp:Literal Text="<%$ Resources:Tokens,NoResults %>" runat="server" /></div>
                        </EmptyDataTemplate>

                    </asp:GridView>
                </div>
            </fieldset>
        </div>
        <div class="view">
            <fieldset>
                <h3 class="header smaller lighter blue">
                    <asp:Literal Text="<%$ Resources:Tokens,Customers %>" runat="server" /></h3>
                <div>
                    <asp:GridView ID="GItemsCustomers" ClientIDMode="Static" runat="server"
                     CssClass="table table-bordered table-condensed"
                     AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="Consumer" HeaderText="<%$ Resources:Tokens,Consumer %>" />
                            <asp:BoundField DataField="Operation" HeaderText="<%$ Resources:Tokens,Operation %>" Visible="False" />
                            <asp:BoundField DataField="Phone" HeaderText="<%$ Resources:Tokens,Phone %>" />
                            <asp:BoundField DataField="Governate" HeaderText="<%$ Resources:Tokens,Governrate %>" />
                            <asp:BoundField DataField="Branch" HeaderText="<%$ Resources:Tokens,Branch %>" />
                            <asp:BoundField DataField="Provider" HeaderText="<%$ Resources:Tokens,Provider %>" />
                            <asp:BoundField DataField="Reseller" HeaderText="<%$ Resources:Tokens,Reseller %>" />
                            <asp:BoundField DataField="Quantity" HeaderText="<%$ Resources:Tokens,Quantity %>" Visible="False" />
                            <asp:BoundField DataField="IpPackage" HeaderText="<%$ Resources:Tokens,Ip.Package %>" />
                            <asp:BoundField DataField="Package" HeaderText="<%$ Resources:Tokens,Package %>" />
                            <asp:BoundField DataField="Status" HeaderText="<%$ Resources:Tokens,Status %>" />
                            <asp:BoundField DataField="Time" HeaderText="<%$ Resources:Tokens,Time %>" />
                        </Columns>
                        <EmptyDataTemplate>
                            <div>
                                <asp:Literal Text="<%$ Resources:Tokens,NoResults %>" runat="server" /></div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </fieldset>
        </div>
    </section>

    <script>

        $(document).ready(function () {
            $('#TbFrom').datepicker({ dateFormat: 'dd-mm-yy' });
            $('#TbTo').datepicker({ dateFormat: 'dd-mm-yy' });
        });

    </script>
</asp:Content>
