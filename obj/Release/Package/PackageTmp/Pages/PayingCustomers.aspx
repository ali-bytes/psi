<%@ Page Title="<%$Resources:Tokens,PayingCustomers %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="PayingCustomers.aspx.cs" Inherits="NewIspNL.Pages.PayingCustomers" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
                <div class="page-header"><h1>
                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,PayingCustomers %>" runat="server" /></h1></div>
            <div>
                <div class="well">
                    <div>
                        <label for="DdlReseller">
                            <%= Tokens.Reseller %></label>
                        <div>
                            <asp:DropDownList ValidationGroup="rs" runat="server" ID="DdlReseller" ClientIDMode="Static">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RddlReseller" ErrorMessage="*" ControlToValidate="DdlReseller"
                                runat="server" />
                        </div>
                    </div>
                     <div >

                 
                        <div>
                            <label for="DdlYear">
                                <%=Tokens.Year %>
                            </label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlYear">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div>
                            <label for="DdlMonth">
                                <%=Tokens.Month %></label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlMonth">
                                </asp:DropDownList>
                            </div>
                        </div>
                   
                
            </div>
                    <br/>
                    <button class="btn btn-success" validationgroup="rs" runat="server" id="BSearch" OnServerClick="SearchDemands">
                        <i class="icon-white icon-search"></i>&nbsp;
                        <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,Search %>"></asp:Literal>
                    </button>
                </div>
            </div>
            <div class="view" id="resultPanel">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <asp:Literal ID="Literal3" Text="<%$Resources:Tokens,Results %>" runat="server" /></h3>
           <div runat="server" class="alert alert-success" id="Msg" Visible="False">
               <asp:Literal runat="server" Text="<%$Resources:Tokens,Saved %>"></asp:Literal>
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
                        <asp:BoundField HeaderText="<%$Resources:Tokens,ReceiptNumber %>" DataField="Id" />
                        
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Service.Package %>" DataField="servicepack" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Provider %>" DataField="Provider" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Reseller %>" DataField="Reseller" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Central %>" DataField="Central" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Governorate %>" DataField="Governorate" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Offer %>" DataField="Offer" />
                      
                        <asp:BoundField HeaderText="<%$Resources:Tokens,From %>" DataField="TStartAt" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,To %>" DataField="TEndAt" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Notes %>" DataField="Notes" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Amount %>" DataField="TAmount" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton runat="server"  id="btnPay" OnClick="Pay" CommandArgument='<%#Eval("Id") %>'
                                 CssClass="btn btn-warning btn-sm" data-rel="tooltip" ToolTip="<%$Resources:Tokens,Pay %>"><i class="icon-dollar icon-only bigger-130"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </fieldset>
    </div>
    <script type="text/javascript">
        var windowObjectReference;

        function openRequestedPopup(id) {
            windowObjectReference = window.open("ResellerDemandReciept.aspx?id=" + id,
              "DescriptiveWindowName",
              "menubar=yes,location=yes,resizable=yes,scrollbars=yes,status=yes");
        }
    </script>
</asp:Content>


