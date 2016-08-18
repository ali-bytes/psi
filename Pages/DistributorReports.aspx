<%@ Page Title="<%$Resources:Tokens,DistributorReports %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="DistributorReports.aspx.cs" Inherits="NewIspNL.Pages.DistributorReports" %>

<%@ Import Namespace="Resources" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-header">
        <h1>
            <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,DistributorReports %>" runat="server" /></h1>
    </div>
    <div class="well">
        <div>
            <label for="DdlReseller">
                <%= Tokens.Reseller %></label>
            <div>
                <asp:DropDownList ValidationGroup="rrs" runat="server" ID="DdlReseller" ClientIDMode="Static">
                </asp:DropDownList>
               <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="*" ControlToValidate="DdlReseller"
                    runat="server" ValidationGroup="rrs" />--%>
            </div>
            <label for="DdlBranch">
                <%= Tokens.Branch %></label>
            <div>
                <asp:DropDownList ValidationGroup="rrs" runat="server" ID="DdlBranch" ClientIDMode="Static">
                </asp:DropDownList>
               <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="*" ControlToValidate="DdlBranch"
                    runat="server" ValidationGroup="rrs" />--%>
            </div>
             <label for="ddlCustomerType">
                <%= Tokens.CustomerType %></label>
            <div>
                <asp:DropDownList ValidationGroup="rrs" runat="server" ID="ddlCustomerType" ClientIDMode="Static">
                    <asp:ListItem Text="<%$Resources:Tokens,All %>" Value="0" />
                    <asp:ListItem Text="<%$Resources:Tokens,InternalCustomer %>" Value="1" />
                    <asp:ListItem Text="<%$Resources:Tokens,ExternalCustomer %>" Value="2" />
                </asp:DropDownList>
              <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="*" ControlToValidate="ddlCustomerType"
                    runat="server" ValidationGroup="rrs" />--%>
            </div>
        </div>

    </div>
    <div>
        <div class="well col-md-12">
            <div>
                <label class="col-md-1">
                    <asp:Literal runat="server" Text="<%$Resources:Tokens,From %>"></asp:Literal></label>
                <div class="col-md-3">
                    <asp:TextBox runat="server" ID="txtFrom" date-select="db"></asp:TextBox>
                </div>
                <label class="col-md-1">
                    <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:Tokens,To %>"></asp:Literal></label>
                <div class="col-md-3">
                    <asp:TextBox runat="server" ID="txtTo" date-select="db"></asp:TextBox>
                </div>
            </div>
            <br />
            <div class="col-md-12">
            <button class="btn btn-success" runat="server" id="BSearch" onserverclick="btnSearch_Click">
                <i class="icon-white icon-search"></i>&nbsp;
                        <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,Search %>"></asp:Literal>
            </button>
                </div>
        </div>
    </div>
    <%-- <div>
        <div class="well col-md-12">
            <div>
                 <button class="btn btn-success" runat="server" id="BSearch" onserverclick="btnSearch_Click">
                <i class="icon-white icon-search"></i>&nbsp;
                        <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,Search %>"></asp:Literal>
            </button>
                 </div>
             </div>
          </div>--%>
    <div class="view" id="resultPanel">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <asp:Literal ID="Literal31" Text="<%$Resources:Tokens,Results %>" runat="server" /></h3>
            <div runat="server" class="alert alert-success" id="Msg" visible="False">
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:Tokens,Saved %>"></asp:Literal>
            </div>
            <div>
                <asp:GridView CssClass="table table-bordered table-condensed text-center" DataKeyNames="Id"
                    runat="server" ID="GvResults" AutoGenerateColumns="False" OnDataBound="NumberGrid">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LNo" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Customer %>" DataField="Customer" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Phone %>" DataField="Phone" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,CustomerType %>" DataField="CustomerType" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Date %>" DataField="Date" />
                         <asp:BoundField HeaderText="<%$Resources:Tokens,User %>" DataField="User" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Amount %>" DataField="Amount" />
                       <%-- <asp:TemplateField>
                            <ItemTemplate>
                                <a id="btnPay" data-rel="tooltip" title="<%=Tokens.Reciept %>" href='<%#string.Format("ResellerDemandReciept.aspx?id={0}",QueryStringSecurity.Encrypt(Eval("demandId").ToString())) %>' target="_blank">
                                    <i class="icon-file-text icon-only bigger-130"></i>
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="alert alert-info" runat="server" id="divTotal" visible="False">
                <label>
                    <asp:Literal runat="server" Text="<%$Resources:Tokens,Total %>"></asp:Literal></label>
                :&nbsp;<asp:Label runat="server" ID="lblTotal"></asp:Label>
            </div>
            <div>
             <asp:Button runat="server" OnClick="btnExport_OnClick" CssClass="btn btn-danger" Text="<%$Resources:Tokens,Export %>" ID="btnExportToExcel" />
            </div>
        </fieldset>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            $('input[date-select="db"]').datepicker({ dateFormat: 'dd-mm-yy' });
        });</script>
</asp:Content>
