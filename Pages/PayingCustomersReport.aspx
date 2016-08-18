<%@ Page Title="<%$Resources:Tokens,PayingCustomersReport %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="PayingCustomersReport.aspx.cs" Inherits="NewIspNL.Pages.PayingCustomersReport" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="NewIspNL.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
                    <div class="page-header"><h1>
                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,PayingCustomersReport %>" runat="server" /></h1></div>
            <div>
                <div class="well">
                    <div>
                        <label for="DdlReseller">
                            <%= Tokens.Reseller %></label>
                        <div>
                            <asp:DropDownList ValidationGroup="rrs" runat="server" ID="DdlReseller" ClientIDMode="Static">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RddlReseller" ErrorMessage="*" ControlToValidate="DdlReseller"
                                runat="server" ValidationGroup="rrs"/>
                        </div>
                        <label><asp:Literal runat="server" Text="<%$Resources:Tokens,From %>"></asp:Literal></label>
                        <div>
                            <asp:TextBox runat="server" ID="txtFrom" date-select="db"></asp:TextBox>
                        </div>
                                                <label><asp:Literal ID="Literal4" runat="server" Text="<%$Resources:Tokens,To %>"></asp:Literal></label>
                        <div>
                            <asp:TextBox runat="server" ID="txtTo" date-select="db"></asp:TextBox>
                        </div>
                    </div>
                    <br/>
                    <button class="btn btn-success" validationgroup="rrs" runat="server" id="BSearch" OnServerClick="btnSearch_Click">
                        <i class="icon-white icon-search"></i>&nbsp;
                        <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,Search %>"></asp:Literal>
                    </button>
                </div>
            </div>
            <div class="view" id="resultPanel">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <asp:Literal ID="Literal31" Text="<%$Resources:Tokens,Results %>" runat="server" /></h3>
           <div runat="server" class="alert alert-success" id="Msg" Visible="False">
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
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Status %>" DataField="Status" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Service.Package %>" DataField="servicepack" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Provider %>" DataField="Provider" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Date %>" DataField="Date"/>
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Amount %>" DataField="Amount" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <a id="btnPay" data-rel="tooltip" title="<%=Tokens.Reciept %>" href='<%#string.Format("ResellerDemandReciept.aspx?id={0}",QueryStringSecurity.Encrypt(Eval("demandId").ToString())) %>' target="_blank">
                                    <i class="icon-file-text icon-only bigger-130"></i>
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="alert alert-info" runat="server" id="divTotal" Visible="False">
                <label><asp:Literal runat="server" Text="<%$Resources:Tokens,Total %>"></asp:Literal></label>
                :&nbsp;<asp:Label runat="server" ID="lblTotal"></asp:Label>
            </div>
        </fieldset>
    </div>
 
    <script type="text/javascript">
        $(document).ready(function () {
            $('input[date-select="db"]').datepicker({ dateFormat: 'dd-mm-yy' });
        });</script>
</asp:Content>


