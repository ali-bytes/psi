<%@ Page Title="<%$Resources:Tokens,CenterDemands%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="CenterDemands.aspx.cs" Inherits="NewIspNL.Pages.CenterDemands" %>
<%@ Import Namespace="NewIspNL.Helpers" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div runat="server" id="Mother">
        <div  runat="server" id="Msg" class="alertMsg"></div>
        <div class="view white">
            <fieldset>
                <div class="page-header"><h1>
                    <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,CenterDemands%>" runat="server" /></h1></div>
                <div class="well">
                    <div>
                        <div runat="server" ID="GovBox">
                            <div>
                                <label for="DdGovernorates">
                                    <asp:Literal ID="Literal2" Text="<%$Resources:Tokens,Governrate %>" runat="server" /></label>
                            </div>
                            <div>
                                <asp:DropDownList runat="server" ID="DdGovernorates">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DdGovernorates"
                                                            ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <div>
                                <label for="TPhone">
                                    <asp:Literal ID="Literal3" Text="<%$Resources:Tokens,Phone%>" runat="server" /></label>
                            </div>
                            <div>
                                <asp:TextBox runat="server" ID="TPhone" ClientIDMode="Static"> </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TPhone"
                                                            ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <button class="btn btn-success" type="submit" runat="server" id="BSearch" ClientIDMode="Static" onserverclick="BSearch_OnServerClick">
                                        <i class="icon-white icon-search"></i>&nbsp;<%=Tokens.Search %></button>
                                </td>
                                <td>
                                    <span id="go-to-details" class="btn btn-success"><i class="icon-white icon-question-sign">
                                                                                     </i>&nbsp;<%=Tokens.Details %>
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField runat="server" ID="HfCustomerId" Value="0" ClientIDMode="Static" />
                    </div>
                </div>
            </fieldset>
        </div>
        <div class="view">
            <fieldset>
                <h3 class="header smaller lighter blue">
                    <%= Tokens.Customer %></h3>
                <div class="alert alert-danger" id="suspendMsg" runat="server">
                </div>
                <%--                <p runat="server" id="handleCanSuspend">
                    <button class="btn btn-primary" runat="server" id="Suspsend" onserverclick="SuspendOrder">
                        <i class="icon-white icon-ok"></i>&nbsp;<%= Tokens.Suspend %></button>
                </p>
                <p id="handelcanActive">
                    <button class="btn btn-success" runat="server" Visible="False" id="Active" onserverclick="Activeorder">
                        <i class="icon-white icon-ok"></i>&nbsp;<%= Tokens.ToActiveCustomer %></button>
                </p>--%>
                <div>
                    <asp:GridView runat="server" ID="GvCustomerData" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed">
                        <Columns>
                            <asp:BoundField DataField="Customer" HeaderText="<%$Resources:Tokens,Customer%>" />
                            <asp:BoundField DataField="Phone" HeaderText="<%$Resources:Tokens,Phone%>" />
                            <asp:BoundField DataField="State" HeaderText="<%$Resources:Tokens,State%>" />
                            <asp:BoundField DataField="Package" HeaderText="<%$Resources:Tokens,Package%>" />
                            <asp:BoundField DataField="Branch" HeaderText="<%$Resources:Tokens,Branch%>" />
                            <asp:BoundField DataField="ServicProvider" HeaderText="<%$Resources:Tokens,Service.Provider %>"/>
                            <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,Reseller%>" />
                            <asp:BoundField DataField="Offer" HeaderText="<%$Resources:Tokens,Offer%>" />
                            <asp:BoundField DataField="Central" HeaderText="<%$Resources:Tokens,Central%>" />
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="alert">
                                <%= Tokens.NoResults %>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </fieldset>
        </div>
        <div class="view">
            <div>
                <fieldset>
                    <h3 class="header smaller lighter blue">
                        <asp:Literal ID="Literal4" Text="<%$Resources:Tokens,Unpaid %>" runat="server" /></h3>
                    <div>
                        <asp:GridView runat="server" ID="GvUnpaid" AutoGenerateColumns="False" OnDataBound="GvUnpaid_OnDataBound"
                                      CssClass="table table-bordered table-condensed">
                            <Columns>
                                <asp:TemplateField HeaderText="#">
                                    <ItemTemplate>
                                        <asp:Label ID="LNo" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Customer" HeaderText="<%$Resources:Tokens,Center%>" />
                                <asp:BoundField DataField="Phone" HeaderText="<%$Resources:Tokens,Phone%>" />
                                <asp:BoundField DataField="Offer" HeaderText="<%$Resources:Tokens,Offer%>" />
                                <asp:BoundField DataField="From" HeaderText="<%$Resources:Tokens,From%>" />
                                <asp:BoundField DataField="To" HeaderText="<%$Resources:Tokens,To%>" />
                                <asp:BoundField DataField="ForMonth" HeaderText="<%$Resources:Tokens,Month%>" />
                                <asp:BoundField DataField="ForYear" HeaderText="<%$Resources:Tokens,Year%>" />
                                <%--<asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes%>" />--%>
                                <asp:TemplateField HeaderText="<%$Resources:Tokens,Notes%>">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%#Eval("Notes") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Amount%>" />
                                <asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,User%>" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button data-comment="comment" ID="BPay" Visible="False" CssClass="btn btn-success btn-xs"
                                                    runat="server" data-fruit='<%#Eval("Amount") %>' CommandArgument='<%#Eval("Id") %>' data-demand='<%#Eval("Id") %>'
                                                    Text="<%$Resources:Tokens,Pay %>"/>
                                        <%--   <asp:Button CssClass="btn btn-primary" runat="server" ID="EditBtn" CommandArgument='<%#Eval("Id") %>'
                                                Text="<%$Resources:Tokens,Edit %>" data-demand='<%#Eval("Id") %>' data-from='<%#Eval("From") %>'
                                                data-to='<%#Eval("To") %>' data-notes='<%#Eval("Notes") %>' data-amount='<%#Eval("Amount") %>'
                                                data-select="for-edit"></asp:Button>
                                    <asp:Button CssClass="btn btn-danger" runat="server" ID="DeleteBtn" OnClientClick="return areyousure()"
                                                CommandArgument='<%#Eval("Id") %>' OnClick="DeleteCurrentDemand" Text="<%$Resources:Tokens,Delete %>">
                                    </asp:Button>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="alert">
                                    <%=Tokens.NoResults %>
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                    <div class="well">
                        <table>
                            <tr>
                                <td><%=Tokens.Total %></td>
                                <td>&nbsp;:&nbsp;</td>
                                <td><asp:Label runat="server" ID="lblTotalUnpaid"></asp:Label></td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
            </div>
        </div>
        <div class="view">
            
            <div>
                <fieldset>
                    <h3 class="header smaller lighter blue">
                        <asp:Literal ID="Literal5" Text="<%$Resources:Tokens,Paid %>" runat="server" /></h3>
                    <div>
                        <asp:GridView runat="server" ID="GvPaid" OnDataBound="GvPaid_OnDataBound" AutoGenerateColumns="False"
                                      CssClass="table table-bordered table-condensed">
                            <Columns>
                                <asp:TemplateField HeaderText="#">
                                    <ItemTemplate>
                                        <asp:Label ID="LNo" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Customer" HeaderText="<%$Resources:Tokens,Center%>" />
                                <asp:BoundField DataField="Phone" HeaderText="<%$Resources:Tokens,Phone%>" />
                                <asp:BoundField DataField="Offer" HeaderText="<%$Resources:Tokens,Offer%>" />
                                <asp:BoundField DataField="From" HeaderText="<%$Resources:Tokens,From%>" />
                                <asp:BoundField DataField="To" HeaderText="<%$Resources:Tokens,To%>" />
                                <asp:BoundField DataField="ForMonth" HeaderText="<%$Resources:Tokens,Month%>" />
                                <asp:BoundField DataField="ForYear" HeaderText="<%$Resources:Tokens,Year%>" />
                                <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes%>" />
                                <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Amount%>" />
                                <asp:BoundField DataField="PaymentComment" HeaderText="<%$Resources:Tokens,Comment%>" />
                                <asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,User%>" />
                                <asp:BoundField DataField="PaymentDate" HeaderText="<%$Resources:Tokens,PaymentDate%>" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <%--<asp:Button CssClass="btn btn-danger" runat="server" ID="Unpay" CommandArgument='<%#Eval("Id") %>'
                                                Text="<%$Resources:Tokens,CancelPayment %>" OnClick="CancelPayment"></asp:Button>--%>
                                        &nbsp;<% if(WidthOption.WidthOfReciept == false){ %><a target="_blank" href='<%#string.Format("DemandReciept.aspx?d={0}",QueryStringSecurity.Encrypt(Eval("Id").ToString()))  %>' title='<%=Tokens.Print %>' data-rel="tooltip"><i class="icon-print bigger-150 blue"></i></a><% } else{ %><a target="_blank" href='<%#string.Format("smallDemandReciept.aspx?d={0}",QueryStringSecurity.Encrypt(Eval("Id").ToString())) %>' title="<%= Tokens.Print %>" data-rel="tooltip"><i class="icon-print bigger-150 blue"></i></a><% } %> 
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="alert">
                                    <%=Tokens.NoResults %>
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </fieldset>
            </div>
        </div>
    </div>
    <span style="display: none" id="get-url" data-url="<%=DetailsUrl %>"></span>
    <div id="comment-dialog"  class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                                        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
            <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                ×</button>
            <h4 id="H5">
                <%=Tokens.PaymentComment %></h4>
        </div>
                <div class="modal-body">
                                 <div class="bootbox-body">
        <asp:HiddenField runat="server" ID="HfDemandId" ClientIDMode="Static" />
        <div class="well">
            <label for="txtComment">
                <%=Tokens.Comment %></label>
            <div>
                <asp:TextBox runat="server" ID="txtComment" TextMode="MultiLine" ValidationGroup="comment" />
                <asp:RequiredFieldValidator ErrorMessage="<%$Resources:Tokens,Required %>" ControlToValidate="txtComment"
                                            runat="server" ID="RComment" ValidationGroup="comment" />
            </div>
        </div>
        
    </div></div>
    <div class="modal-footer">
        <p>
            <asp:Button Text="<%$Resources:Tokens,Pay %>" runat="server" CssClass="btn btn-info" ValidationGroup="comment" ID="PDialogPay" OnClick="PayDemand" />
           
                            <button id="cancel-dialog" type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="True">
                    <%=Tokens.Cancel %></button>
        </p>
    </div>
    </div></div>
    </div>
 
    <script type="text/javascript">
        function areyousure() {
            return confirm('<%= Tokens.Delete %>');
        }

        $(document).ready(function () {
            var customerId = $("#HfCustomerId").val();

            /*var commentDialog = $('#comment-dialog').dialog({
                autoOpen: false,
                resizable: false,
                draggable: false,
                title: "<%=Tokens.PaymentComment %>",
            modal: true,
            position: 'center'
        });
        commentDialog.parent().appendTo(jQuery("form:first"));*/
        $('input[data-comment="comment"]').click(function () {
            if (parseInt(customerId) === 0) {
                alert("<%=Tokens.SearchCustomerFirst %>");
                    return true;
                } else {

                    //$(commentDialog).dialog('open');
                    var demandId = $(this).attr("data-demand");
                    $('#HfDemandId').val(demandId);
                    var v = $(this).attr('data-fruit');
                    $('#txtDiscoundBox').val(v);
                    $("#comment-dialog").modal("show");
                    return false;
                }
            });
            /* $('#ddlbox').change(function () {
                 var s = $('#ddlbox').val();
                 $('#hf_boxId').val(s);
             });*/
            $('input[data-select="dt"]').datepicker({ dateFormat: 'dd/mm/yy' });
            $('#go-to-details').click(function () {
                if (customerId === "0") {
                    alert("<%=Tokens.SearchCustomerFirst %>");
                    return;
                } else {
                    window.location = $('#get-url').attr("data-url") + customerId;
                }
            });
            });
    </script>
                                    <script type="text/javascript">
                                        $(document).ready(function () {
                                            $('#TPhone').keypress(function (e) {
                                                var key = e.which;
                                                if (key == 13) {
                                                    $('#bSearch').click();
                                                    return false;
                                                } else {
                                                    return true;
                                                }
                                            });
                                        });
    </script>
</asp:Content>

