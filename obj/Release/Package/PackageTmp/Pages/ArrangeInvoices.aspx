<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ArrangeInvoices.aspx.cs" Inherits="NewIspNL.Pages.ArrangeInvoices" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">

    <div class="view" id="searchPanel">
        <fieldset>
            <div class="page-header">
                <h1><%= Tokens.ArrangeInvoices%></h1></div>  
                 <div id="message"style="background-color: #dff0d8; color: #468847;border-color: #d6e9c6;
            margin: 5px;">
            <asp:Label runat="server" ID="l_message" ClientIDMode="Static"></asp:Label></div>
            <div class="row">
                <div class="col-sm-6">
                    <div class="well">
                        <div>
                            <label><%=Tokens.Invoice %></label>
                            <div>
                                <asp:FileUpload runat="server" ID="fu_sheet"></asp:FileUpload>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Tokens,Required%>"
                                                            ControlToValidate="fu_sheet"></asp:RequiredFieldValidator>
                                                           <asp:RegularExpressionValidator runat="server" ForeColor="red"  ValidationExpression="^.*\.(xls|XLS|xlsx|XLSX|)$" ControlToValidate="fu_sheet"  ErrorMessage="<%$Resources:Tokens,uploadfilexcelerror %>"></asp:RegularExpressionValidator>
                  
           
                                
                                 </div>
                        </div>
                    </div>
                    <p>
                        <asp:Button runat="server" ID="btnArrangeInvoice" 
                                    Text="<%$Resources:Tokens,Add %>" CssClass="btn btn-success" 
                                    onclick="btnArrangeInvoice_Click"/>
                                    <a href="../ExcelTemplates/ArrangedInvoice.xls" class="btn btn-default"><i class="icon-cloud-download bigger-120"></i><%=Tokens.Downloadsample %></a>
                    </p>
                </div>
            </div>
        </fieldset>
    </div>
    <div id="grexport" runat="server">
        <div class="view">
            <fieldset>

                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-responsive">
                    <Columns>
                        <asp:BoundField DataField="Customernumber" HeaderText="Customer Number" />
                        <asp:BoundField DataField="Customername" HeaderText="Customer Name" />
                        <asp:BoundField DataField="packagename" HeaderText="Package name" />
                        <asp:BoundField DataField="Netamout" HeaderText="Net amount" />
                        <asp:BoundField DataField="Startdate" HeaderText="Start date" />
                        <asp:BoundField DataField="Enddate" HeaderText="End date" />
                        <asp:BoundField DataField="Description" HeaderText="Description" />
                        <asp:BoundField DataField="Phonenumber" HeaderText="Phone number" />
                        <asp:BoundField DataField="Exchange" HeaderText="Exchange" />
                        <asp:BoundField DataField="CumstomerBranch" HeaderText="CumstomerBranch" />
                        <asp:BoundField DataField="CustomerReseller" HeaderText="CustomerReseller" />
                        <asp:BoundField DataField="CustomerOffer" HeaderText="CustomerOffer" />
                        <asp:BoundField DataField="CustomerStatus" HeaderText="Customer Status" />
                        <asp:BoundField DataField="systemPackgeName" HeaderText="System Packge Name" />
                    </Columns>
                </asp:GridView>

                
               
                <p>
                    <asp:Button runat="server" CssClass="btn btn-primary" ID="ExportNotFounded" OnClick="ExportNotFounded_click" Text="<%$Resources:Tokens,ExportNotFoundedInvoices %>"/>
                </p>
            </fieldset>
        </div>
    </div>
        <div>
            <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-responsive">
                    <Columns>
                        <asp:BoundField DataField="Customernumber" HeaderText="Customer Number" />
                       <%-- <asp:BoundField DataField="Customername" HeaderText="Customer Name" />--%>
                      <%--  <asp:BoundField DataField="packagename" HeaderText="Package name" />
                        <asp:BoundField DataField="Netamout" HeaderText="Net amount" />
                        <asp:BoundField DataField="Startdate" HeaderText="Start date" />
                        <asp:BoundField DataField="Enddate" HeaderText="End date" />
                        <asp:BoundField DataField="Description" HeaderText="Description" />--%>
                        <asp:BoundField DataField="Phonenumber" HeaderText="Phone number" />
                       <%-- <asp:BoundField DataField="Exchange" HeaderText="Exchange" />
                        <asp:BoundField DataField="CumstomerBranch" HeaderText="CumstomerBranch" />
                        <asp:BoundField DataField="CustomerReseller" HeaderText="CustomerReseller" />
                        <asp:BoundField DataField="CustomerOffer" HeaderText="CustomerOffer" />
                        <asp:BoundField DataField="CustomerStatus" HeaderText="Customer Status" />--%>
                    </Columns>
                </asp:GridView> 
        </div>
</div>
</asp:Content>

