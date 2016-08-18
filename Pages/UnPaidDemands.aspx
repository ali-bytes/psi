<%@ Page Title="<%$Resources:Tokens,UnPaidDemands%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="UnPaidDemands.aspx.cs" Inherits="NewIspNL.Pages.UnPaidDemands" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view">
        <fieldset>
            <div class="page-header">
                <h1><%= (Tokens.Search +" "+Tokens.UnPaidDemands) %></h1></div>
            <div class="row-fluid">
                <div class="col-sm-4">
                    <div class="well">
                        <div>
                            <label for="DdlReseller">
                                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,Reseller %>" runat="server" />
                            </label>
                            <div>
                                <asp:DropDownList ID="DdlReseller" runat="server" EnableViewState="True" />
                            </div>
                        </div>

                        <div>
                            <label for="txtFrom">
                                <asp:Literal runat="server" Text="<%$Resources:Tokens,From %>"></asp:Literal>
                            </label>
                            <div>
                                <asp:TextBox runat="server" data-select="dp" ID="txtFrom"></asp:TextBox>
                            </div>
                        </div>
                                                <div>
                            <label for="">
                                <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:Tokens,To %>"></asp:Literal>
                            </label>
                            <div>
                                <asp:TextBox runat="server" data-select="dp" ID="txtTo"></asp:TextBox>
                            </div>
                        </div>
                                                                        <div>
                            <label for="DdlIpPackages">
                                <asp:Literal ID="Literal12" Text="<%$Resources:Tokens,Ip.Package%>" runat="server" />
                            </label>
                            <div>
                               <asp:DropDownList runat="server" ID="DdlIpPackages" EnableViewState="True" />
                            </div>
                        </div>

                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="well">
                                                <div>
                            <label for="DdlBranchs">
                                <asp:Literal ID="Literal2" Text="<%$Resources:Tokens,Branch %>" runat="server" />
                            </label>
                            <div>
                                <asp:DropDownList ID="DdlBranchs" runat="server" EnableViewState="True" />
                            </div>
                        </div>
                                                <div>
                            <label>
                                <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:Tokens,Provider %>"></asp:Literal>
                            </label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlProvider" EnableViewState="True"/>
                            </div>
                        </div>
                        <div>
                            <label for="DdlGovernorate">
                                <asp:Literal ID="Literal3" Text="<%$Resources:Tokens,Governorate %>" runat="server" />
                            </label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlGovernorate" EnableViewState="True" AutoPostBack="True"
                                                  onselectedindexchanged="DdlGovernorate_SelectedIndexChanged" />
                            </div>
                        </div>
                                                <div>
                            <label for="DdlGovernorate">
                                <asp:Literal ID="Literal10" Text="<%$Resources:Tokens,Service.Package %>" runat="server" />
                            </label>
                            <div>
                               <asp:DropDownList runat="server" ID="DdlPackage" EnableViewState="True" />
                            </div>
                        </div>
                        <div>
                            <label>
                                <asp:Literal runat="server" Text="<%$Resources:Tokens,PaymentType %>"></asp:Literal>
                            </label>
                            <div>
                                <asp:DropDownList runat="server" ID="ddlPaymentType" EnableViewState="True"/>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="well">
                                                <div>
                            <label for="DdlCentral">
                                <asp:Literal ID="Literal4" Text="<%$Resources:Tokens,Central %>" runat="server" />
                            </label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlCentral" EnableViewState="True" />
                            </div>
                        </div>
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
                            <label><%=Tokens.Customer_Status %></label>
                            <div>
                                <asp:DropDownList runat="server" ID="ddlCustomerStatus"/>
                            </div>
                        </div>
                                                <div>
                            <label for="txtFromRequestDate">
                               <asp:Literal runat="server" ID="lblFromRequestDate"></asp:Literal>
                            </label>
                            <div>
                            <asp:TextBox runat="server" ID="txtFromRequestDate" data-select="dp"></asp:TextBox>
                            </div>
                        </div>
                                                <div>
                            <label for="txtToRequestDate">
                                <asp:Literal runat="server" ID="lblToRequestDate" Text="<%$Resources:Tokens,To %>"></asp:Literal>
                            </label>
                            <div>
                            <asp:TextBox runat="server" ID="txtToRequestDate" data-select="dp"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            <p class="col-sm-12">
                <asp:Button Text="<%$Resources:Tokens,Search %>" CssClass="btn btn-success" runat="server"
                    OnClick="SearchDemands" />
            </p>
            </div>

        </fieldset>
    </div>
     <div id="DivSuccess" runat="server"></div>
            <div id="Div1" runat="server"></div>
            <div id="DivCount" runat="server"></div>
    <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <asp:Literal Text="<%$Resources:Tokens,Results %>" runat="server" /></h3>
            <div runat="server" id="Msg">
            </div>
            <p runat="server" id="Pb2">
                <button data-get="pay"  class="btn btn-primary" title="<%=Tokens.Pay %>" data-rel="tooltip"><i class="icon-dollar icon-only bigger-120"></i></button>
                <button id="Button1" data-send="sms" class="btn btn-success" title="<%=Tokens.SendsmsMessage %>" data-rel="tooltip"><i class="icon-mobile-phone icon-only bigger-120"></i></button>
                <span runat="server" id="divbtnDelete"><button id="btnDeleteDemand" type="button" data-get="delete" class="btn btn-danger" title="<%=Tokens.Delete %>" data-rel="tooltip"><i class="icon-trash icon-only bigger-120"></i></button></span>
                
                 <asp:LinkButton class="btn btn-danger pull-right" runat="server" id="SuspsendAll" clientidmode="Static" OnClientClick="return alrt();" OnClick="SuspendOrder">
                        <i class="icon-white icon-ok"></i>&nbsp;<%= Tokens.Suspend %></asp:LinkButton>
            </p>
            <div>
                <asp:GridView CssClass="table table-bordered table-condensed" ClientIDMode="Static"
                    runat="server" ID="GvResults" AutoGenerateColumns="False" DataKeyNames="WorkOrderId" OnDataBound="NumberGrid">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LNo" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Customer %>" DataField="Customer" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Phone %>" DataField="Phone" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Status %>" DataField="Status" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Provider %>" DataField="Provider" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Reseller %>" DataField="Reseller" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Branch %>" DataField="Branch"/>
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Central %>" DataField="Central" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Governorate %>" DataField="Governorate" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Package %>" DataField="servicepack" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Offer %>" DataField="Offer" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Paid %>" DataField="TPaid" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,From %>" DataField="TStartAt" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,To %>" DataField="TEndAt" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Amount %>" DataField="TAmount" />
                        <%--<asp:BoundField HeaderText="<%$Resources:Tokens,RequestDate %>" DataField="RequestDate" DataFormatString="{0:dd/M/yyyy}"/>--%>
                        <asp:BoundField HeaderText="<%$Resources:Tokens,PaymentType %>" DataField="PaymentMethod" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Address %>" DataField="Address" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Mobile %>" DataField="Mobile" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Notes %>" DataField="Notes" />
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <input type="checkbox" id="allselector" name="name"  />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="CbPay" CssClass='<%#Eval("Id") %>' data-check="item"/>
                                <%--<asp:Button Text="<%$Resources:Tokens,Pay %>" CommandArgument='<%#Eval("Id") %>' runat="server" OnClick="PayDemand"/>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <p runat="server" id="Pb1">
                <button  data-get="pay" title="<%=Tokens.Pay %>" class="btn btn-primary" data-rel="tooltip"><i class="icon-dollar icon-only bigger-120"></i></button>
                <button id="btnSend" data-send="sms" class="btn btn-success" title="<%=Tokens.SendsmsMessage %>" data-rel="tooltip"><i class="icon-mobile-phone icon-only bigger-120"></i></button>
                <span runat="server" id="divbtnDelete2"><button id="Button3"  data-get="delete" type="button" class="btn btn-danger" title="<%=Tokens.Delete %>" data-rel="tooltip"><i class="icon-trash icon-only bigger-120"></i></button></span>
            </p>
            
        </fieldset>
    </div>
 
    <asp:LinkButton runat="server" ID="btnExport" CssClass="btn btn-primary" OnClick="btnExport_click"><i class="icon-file-text-alt"></i>&nbsp;<%=Tokens.Export %></asp:LinkButton>
    <div id="comment-dialog" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
                        <div class="modal-dialog">
            <div class="modal-content">
        <div class="modal-header">
            <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                ×</button>
            <h4 id="myModalLabel">
                <%=Tokens.PayDemand%></h4>
        </div>
        <div class="modal-body">
            <div class="bootbox-body">
            <asp:HiddenField runat="server" ID="HfDemandId" ClientIDMode="Static" />
            <div class="well">
                            <label for="ddlSavesPay">
                <%=Tokens.sav %></label>
                <div>
                    <asp:DropDownList runat="server" ID="ddlSavesPay" DataTextField="SaveName" DataValueField="Id" ValidationGroup="comment"/>
                      <asp:RequiredFieldValidator ErrorMessage="<%$Resources:Tokens,Required %>" ControlToValidate="ddlSavesPay"
                                            runat="server" ID="RequiredFieldValidator13" ValidationGroup="comment" />
                </div>
                <label for="TbComment">
                    <%=Tokens.Comment %></label>
                <div>
                    <asp:TextBox runat="server" ID="TbComment" TextMode="MultiLine" ValidationGroup="comment" />
                    <asp:RequiredFieldValidator ErrorMessage="<%$Resources:Tokens,Required %>" ControlToValidate="TbComment"
                        runat="server" ID="RComment" ValidationGroup="comment" />
                </div>
            </div></div>
        </div>
        <div class="modal-footer">
              <asp:Button Text="<%$Resources:Tokens,Pay %>" CssClass="btn btn-primary" runat="server"
                ID="PDialogPay" OnClick="PaySelectedDemands" />
            <button class="btn btn-default" data-dismiss="modal" aria-hidden="true">
                <i class="icon-white icon-arrow-left"></i>
                <%=Tokens.Cancel %></button>
        </div>
        </div>
        </div>

    </div>
    <div id="SmsModal" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
                        <div class="modal-dialog">
            <div class="modal-content">
        <div class="modal-header">
            <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                ×</button>
            <h4 id="H1">
                <%=Tokens.SendsmsMessage%></h4>
        </div>
        <div class="modal-body">
            <div class="bootbox-body">
            <div>
                <asp:Literal runat="server" Text="<%$Resources:Tokens,Message %>" ID="lblMessabe"></asp:Literal>
                <div>                  
                    <asp:TextBox runat="server" ID="txtMessageText" ClientIDMode="Static"  TextMode="MultiLine" CssClass="required-input col-xs-10 col-sm-5" Width="200px"></asp:TextBox>
  <span class="help-inline col-xs-12 col-sm-3">
												<span class="middle"><asp:Literal ID="Literal7" runat="server" Text="<%$Resources:Tokens,littersLimit %>"></asp:Literal></span>
											</span>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMessageText" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="sms"></asp:RequiredFieldValidator>
                </div>
            </div></div>
        </div>
        <div class="modal-footer">
                											<button id="Button2" class="btn btn-info" type="button" runat="server" OnServerClick="SendsmsSelected" ValidationGroup="sms">
												<i class="icon-ok bigger-110"></i>
                                                &nbsp;<asp:Literal runat="server" ID="Literal8" Text="<%$Resources:Tokens,Send %>"></asp:Literal>
											</button>
            <button class="btn btn-default" data-dismiss="modal" aria-hidden="true">
                <i class="icon-white icon-arrow-left"></i>
                <%=Tokens.Cancel %></button>
        </div>
        </div>
        </div>

    </div>
    <div id="deleteDemand" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
                        <div class="modal-dialog">
            <div class="modal-content">
        <div class="modal-header">
            <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                ×</button>
            <h4 id="H2">
                <%=Tokens.DeleteDemand%></h4>
        </div>
        <div class="modal-body">
            <div class="bootbox-body">
            <div>
                <asp:Literal runat="server" Text="حذف المطالبة لن يؤثر على تاريخ مطالبة العميل" ID="Literal9"></asp:Literal>
                
            </div></div>
        </div>
        <div class="modal-footer">
                											<button id="Button4" class="btn btn-info" type="button" runat="server" OnServerClick="DeleteSelectedDemands" >
												<i class="icon-ok bigger-110"></i>
                                                &nbsp;<asp:Literal runat="server" ID="Literal11" Text="<%$Resources:Tokens,Delete %>"></asp:Literal>
											</button>
            <button class="btn btn-default" data-dismiss="modal" aria-hidden="true">
                <i class="icon-white icon-arrow-left"></i>
                <%=Tokens.Cancel %></button>
        </div>
        </div>
        </div>

    </div>
   
    <script type="text/javascript">
        function alrt() {
                var checks = $('input:checked');

                var selected = [];
                checks.each(function () {
                    selected.push($(this).attr('data-check'));
                });
                if (selected.length > 0) {
                    return true;
                } else {
                    alert('<%=Tokens.SelectDemands %>');
                return false;
            }
            
        }

       


        $('button[data-get="pay"]').click(function () {
            var checks = $('#GvResults input[type="checkbox"]');
            for (var i = 0; i < checks.length; i++) {
                if ($(checks[i]).is(':checked')) {
                    $('#comment-dialog').modal('show');
                    return false;
                }
            }
            alert('<%=Tokens.SelectDemands %>');
            return true;

        });
        $('button[data-get="delete"]').click(function () {
            var checks = $('#GvResults input[type="checkbox"]');
            for (var i = 0; i < checks.length; i++) {
                if ($(checks[i]).is(':checked')) {
                    $('#deleteDemand').modal('show');
                    return false;
                }
            }
            alert('<%=Tokens.SelectDemands %>');
            return true;

        });
        $('input[data-select="dp"]').datepicker({
            showOtherMonths: true,
            selectOtherMonths: false,
            dateFormat: 'dd/mm/yy'
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#allselector').click(function () {
                if ($('#allselector').is(':checked')) {
                    $('#GvResults').find('span[data-check="item"] input[type="checkbox"]').each(function () {
                        this.checked = true;
                    });
                } else {
                    $('#GvResults').find('span[data-check="item"] input[type="checkbox"]').each(function () {
                        this.checked = false;
                    });
                }
            });
            if ($('#GvSuspned').width() > 1047) {
                /*$('#all-result-items').css({
                "overflow-y": "auto"
                });*/
                $('#GvSuspned').css({
                    //"font-family": "serif",
                    "font-size": "12px"
                });
            }
            if ($('#GvResults').width() > 1047) {
                $('#GvResults').css({
                    //"font-family": "serif",
                    "font-size": "10px"
                });
            }
            $('button[data-send="sms"]').click(function () {

                var totalRows = $("#<%=GvResults.ClientID %> tr").length;
                if (totalRows > 0) {
                    var checks = $('#GvResults input[type="checkbox"]');

                    for (var i = 0; i < checks.length; i++) {
                        if ($(checks[i]).is(':checked')) {
                            $('#SmsModal').modal('show');
                            return false;
                        }
                    }
                    alert('<%=Tokens.SelectDemands %>');
                    return true;
                }
                alert('<%=Tokens.NoResults %>');
                return true;
            });
        });
    </script>
       
</asp:Content>
