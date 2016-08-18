<%@ Page Title="<%$Resources:Tokens,DistributorInvoices %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="DistributorInvoices.aspx.cs" Inherits="NewIspNL.Pages.DistributorInvoices" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        @media print {
            hr {
                border: none;
                height: 1px;
                /* Set the hr color */
                color: #333; /* old IE */
                background-color: #333; /* Modern Browsers */
            }
        }
    </style>
    <link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css" />
    <div class="view">
        <fieldset>
            <div class="page-header">
                <h1>
                    <asp:Label ID="Label37" runat="server" Text="<%$ Resources:Tokens,Search %>"></asp:Label></h1>
            </div>
            <div id="Div1" runat="server"></div>
            <br />
            <div id="Div2" runat="server"></div>
            <div class="well">
                <div>
                    <div style="width: 113px">
                        <asp:Label ID="Label36" runat="server" Text="<%$ Resources:Tokens,Phone %>"></asp:Label>
                    </div>
                    <div>
                        <asp:TextBox ID="txtPhone" ClientIDMode="Static" runat="server" Width="170px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                            ControlToValidate="txtPhone" ValidationGroup="SearchVG"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div>
                    <div style="width: 113px">
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Tokens,Governorate %>"></asp:Label>
                    </div>
                    <div>
                        <select id="areaCode" runat="server" clientidmode="Static" class="chosen" size="1">
                            <option value="">Area</option>
                            <option value="84">84 - Fayoum</option>
                            <option value="2">2 - Cairo</option>
                            <option value="18">18 - Badrasheen</option>
                            <option value="55">55 - Sharkia</option>
                            <option value="86">86 - Menia</option>
                            <option value="66">66 - Port Said</option>
                            <option value="96">96 - Quina</option>
                            <option value="95">95 - Luxor</option>
                            <option value="92">92 - Wadi Gadid</option>
                            <option value="69">69 - South Sinai</option>
                            <option value="40">40 - Gharbia</option>
                            <option value="3">3 - Alex</option>
                            <option value="13">13 - Qaliobia</option>
                            <option value="15">15 - El3sher Men Ramadan</option>
                            <option value="62">62 - Suez</option>
                            <option value="93">93 - souhag</option>
                            <option value="82">82 - Beni Souif</option>
                            <option value="65">65 - Red Sea</option>
                            <option value="50">50 - Daqahlia</option>
                            <option value="57">57 - Damietta</option>
                            <option value="47">47 - Kafr El-Sheikh</option>
                            <option value="88">88 - Assiut</option>
                            <option value="97">97 - Aswan</option>
                            <option value="64">64 - Ismalia</option>
                            <option value="46">46 - Marsa Matrouh</option>
                            <option value="48">48 - Menoufia</option>
                            <option value="68">68 - North Sinai</option>
                            <option value="45">45 - Behira</option>
                        </select>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                            ControlToValidate="areaCode" ValidationGroup="SearchVG"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <p>
                    <br />
                    <asp:Button ID="btn_search" runat="server" CssClass="btn btn-success" Text="<%$ Resources:Tokens,Search %>"
                        Width="100px" OnClick="btn_search_Click" ValidationGroup="SearchVG" ClientIDMode="Static" />
                </p>
            </div>
        </fieldset>
    </div>
    <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <asp:Label ID="Label39" runat="server" Text="<%$ Resources:Tokens,Results %>"></asp:Label>
                <asp:Label ID="externalCust" runat="server" CssClass="alert-info" Text="عميل خارجي" Visible="False"></asp:Label>
                <asp:Label ID="ispCust" runat="server" CssClass="alert-info" Text="عميل داخلي" Visible="False"></asp:Label>
            </h3>
            <asp:GridView ID="grd_invoice" runat="server" AutoGenerateColumns="False" OnRowDataBound="grd_invoice_RowDataBound" ClientIDMode="Static" CssClass="table table-bordered table-condensed center">
                <Columns>
                    <asp:TemplateField HeaderText="#">
                        <ItemTemplate>
                            <asp:Label ID="no" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" HeaderText="<%$ Resources:Tokens,Customer %>" />
                    <asp:BoundField DataField="CustomerNumber" HeaderText="<%$ Resources:Tokens,CustomerNumber %>" />
                    <asp:BoundField DataField="Phone" HeaderText="<%$ Resources:Tokens,Phone %>" />
                    <asp:BoundField DataField="Speed" HeaderText="<%$ Resources:Tokens,PackageType %>" />
                    <asp:BoundField DataField="StartDate" HeaderText="<%$ Resources:Tokens,From %>" />
                    <asp:BoundField DataField="EndDate" HeaderText="<%$ Resources:Tokens,To %>" />
                    <asp:BoundField DataField="Amount" HeaderText="<%$ Resources:Tokens,Amount %>" />
                    <asp:TemplateField HeaderText="<%$ Resources:Tokens,Pay %>" ShowHeader="False">
                        <ItemTemplate>
                            <span runat="server" data-rel="tooltip" tooltip="<%$ Resources:Tokens,Pay %>" title="<%$Resources:Tokens,Pay %>" class="btn btn-success btn-sm"
                                data-phone='<%#Eval("Phone") %>' data-select="pay">
                                <i class="icon-dollar"></i></span>
                            <%-- <asp:LinkButton ID="lnb_Pay" CssClass="btn btn-success btn-sm" runat="server" CausesValidation="False" data-demand='<%#Eval("Id") %>'
                                ToolTip="<%$ Resources:Tokens,Pay %>" data-rel="tooltip"><i class="icon-dollar icon-only"></i></asp:LinkButton>--%>
                        </ItemTemplate>

                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Red" Text="<%$ Resources:Tokens,NoInvoices %>"></asp:Label>
                </EmptyDataTemplate>
            </asp:GridView>
        </fieldset>
    </div>
    <div id="payModal" class="bootbox modal fade" tabindex="-1" role="dialog"
        aria-labelledby="myModalLabel" aria-hidden="true">
        <!-- -->
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                        ×</button>
                    <h4 id="H1" class="modal-title">
                        <%=Tokens.Pay %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <div class="bootbox-form">
                            <div id="savesDiv" runat="server">
                                <label for="ddlSavesPay">
                                    <%=Tokens.sav %></label>
                                <div>
                                    <asp:DropDownList runat="server" ID="ddlSavesPay" DataTextField="SaveName" DataValueField="Id" />
                                    <asp:RequiredFieldValidator ErrorMessage="<%$Resources:Tokens,Required %>" ControlToValidate="ddlSavesPay"
                                        runat="server" ID="RequiredFieldValidator13" ValidationGroup="comment" />
                                </div>
                            </div>
                            <div>
                            </div>
                            <label for="TbComment">
                                <%=Tokens.Comment %></label>
                            <div>
                                <asp:TextBox runat="server" ID="TbComment" TextMode="MultiLine" />
                                <asp:RequiredFieldValidator ErrorMessage="<%$Resources:Tokens,Required %>" ControlToValidate="TbComment"
                                    runat="server" ID="RComment" ValidationGroup="comment" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnReactive" ValidationGroup="comment" CssClass="btn btn-info"
                        Text="<%$Resources:Tokens,Save %>" OnClick="lnb_Pay_Click" />
                    <button class="btn btn-default" data-dismiss="modal" aria-hidden="true">
                        <asp:Literal ID="Literal17" runat="server" Text="<%$Resources:Tokens,Cancel %>"></asp:Literal></button>
                </div>

            </div>
        </div>
    </div>
    <div id="printdiv" style="display: none">
        <% if (Cnfg != null)
           { %>
        <div class="pull-left">
            <div>
                <table style="width: 100%">
                    <% var width = string.Format("{0}%", (Convert.ToDecimal(1) / Convert.ToDecimal(3)) * 100); %>
                    <tr>
                        <td style="width: <%= width %>">
                            <% if (Cnfg.LogoPosiotion == 1)
                               { %>
                            <img src="../PrintLogos/logo_print.gif" alt="<%= Cnfg.CompanyName %>" height="100px" />
                            <% } %>
                        </td>
                        <td style="width: <%= width %>">
                            <% if (Cnfg.LogoPosiotion == 2)
                               { %>
                            <img src="../PrintLogos/logo_print.gif" alt="<%= Cnfg.CompanyName %>" />
                            <% } %>
                        </td>
                        <td style="width: <%= width %>">
                            <% if (Cnfg.LogoPosiotion == 3)
                               { %>
                            <img src="../PrintLogos/logo_print.gif" alt="<%= Cnfg.CompanyName %>" />
                            <% } %>
                        </td>
                    </tr>
                </table>
            </div>
            <div align="center">
                <h2>
                    <%= Cnfg.CompanyName %></h2>
            </div>
            <hr />
            <div>
                <table>
                    <tr>
                        <td>
                            <asp:Literal Text="Transaction Number" runat="server" />
                        </td>
                        <td>:
                        </td>
                        <td>
                            <asp:Literal ID="LNumber" runat="server" />
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <asp:Literal Text="Receipt Date" runat="server" />
                        </td>
                        <td>:
                        </td>
                        <td>
                            <asp:Literal ID="LDate" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal Text="Customer Name" runat="server" />
                        </td>
                        <td>:
                        </td>
                        <td>
                            <strong>
                                <asp:Literal ID="LCustomer" runat="server" /></strong>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal Text="Amount" runat="server" />
                        </td>
                        <td>:
                        </td>
                        <td>
                            <strong>
                                <asp:Literal ID="LPackage" runat="server" /></strong>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal Text="Payment Method" runat="server" />
                        </td>
                        <td>:
                        </td>
                        <td>
                            <strong>
                                <asp:Literal ID="LFor" runat="server" /></strong>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal Text="By" runat="server" />
                        </td>
                        <td>:
                        </td>
                        <td>
                            <strong>
                                <asp:Literal ID="LBy" runat="server" /></strong>
                        </td>
                    </tr>
                </table>
                <div>
                    <asp:Literal ID="LContact" runat="server" />
                </div>
            </div>
            <% if (!string.IsNullOrWhiteSpace(Cnfg.Caution))
               { %>
            <div class="caution" align="center">
                <strong>
                    <%= Cnfg.Caution %></strong>
            </div>

            <% } %>

            <hr />
        </div>
        <% }
           else
           { %>
        <span class="no-print">تاكد من توافر طريقة العرض</span>
        <% } %>
    </div>
    <div class="no-print">
        <button id="prnt" type="button">
            <%=Tokens.Print %></button>
    </div>
    <asp:HiddenField runat="server" ID="hdCustomerPhone" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="HdGovNumber" ClientIDMode="Static" />
    <script type="text/javascript" src="~/Content/ace-assest/js/chosen.jquery.min.js"></script>

    <script type="text/javascript">
        jQuery(document).ready(function () {
            jQuery(".chosen").chosen();
        });
        $(document).ready(function () {

            $('span[data-select="pay"]').click(function () {
                var phone = $(this).attr('data-phone');
                $('#hdCustomerPhone').val(phone);
                $('#HdGovNumber').val($('#areaCode').val());
                $('#payModal').modal('show');
            });


            $('#prnt').click(function () {
                $('#printdiv').css("display", "block");
                var ele = $('#printdiv').prop('outerHTML');
                var mywindow = window.open('', 'new div');
                mywindow.document.write('<html><head><title>إيصال</title>');
                mywindow.document.write('</head><body >');
                mywindow.document.write(ele);
                mywindow.document.write('</body></html>');
                mywindow.document.target = '_blank';
                mywindow.print();
                mywindow.close();
                $('#printdiv').css("display", "none");
                return true;
            });

            <%--  $('a[data-comment="comment"]').click(function () {
                if (parseInt(customerId) === 0) {
                    alert("<%=Tokens.SearchCustomerFirst %>");
                     //$('#ddlbox').val('-1');
                     return true;
                 } else {

                     //$(commentDialog).dialog('open');

                     var demandId = $(this).attr("data-demand");
                     $('#HfDemandId').val(demandId);
                     var realAmount = $(this).attr('data-fruit');
                     var v = parseFloat($(this).attr('data-fruit'));
                     var credit = parseFloat($("#hdnCredit").val());
                     var pluscredit = credit;
                     if (credit < 0) pluscredit = credit * (-1);
                     var minusV = v;
                     if (v < 0) minusV = v * (-1);
                     var l = 0;
                     if (minusV < pluscredit) {
                         l = pluscredit - minusV;
                     } else {
                         l = v;
                     }
                     if (v < 0) $("#txtDiscoundBox").val(realAmount);
                     else $("#txtDiscoundBox").val(l);
                     $("#TbAmountincomment").val(realAmount);
                     $("#comment-dialog").modal('show');
                     return false;
                 }
             });--%>
        });
    </script>
</asp:Content>
