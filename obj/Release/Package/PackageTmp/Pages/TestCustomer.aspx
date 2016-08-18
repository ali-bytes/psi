<%@ Page Title="<%$Resources:Tokens,TestCustomer%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="TestCustomer.aspx.cs" Inherits="NewIspNL.Pages.TestCustomer" %>

 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
<link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>

    <% if (ShowSucess)
       {%>
        <div class="alert alert-success">
                <%= Tokens.CustomerAddedSuccessfully %>
        </div>

    <%} %>
  
    <div class="row">
        <div class="col-sm-5">
            <div class="widget-box">
                <div class="widget-header">
                    <h4>
                        <%=Tokens.Search %></h4>
                </div>
                <div class="widget-body">
                    <div class="widget-main no-padding">
                        <div>
                     
                            <fieldset style="padding: 16px;">
                                <div class="span4">
                                    <div>
                                        <div>
                                            <label for="TbPhone">
                                                <asp:Literal Text="<%$Resources:Tokens,Phone %>" runat="server" ID="lblphone"/>
                                            </label>
                                            <div>
                                                <asp:TextBox runat="server" ID="TbPhone" ClientIDMode="Static" />
                                                <asp:RequiredFieldValidator runat="server" ID="v2" ControlToValidate="TbPhone" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                <span style="color: red;" id="nOnlySpan"></span>
                                            </div>
                                        </div>
                                        <div>
                                            <label for="DdlGovernate">
                                                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,Governrate %>" runat="server" />
                                            </label>
                                            <div>
                                                <asp:DropDownList runat="server" CssClass="width-35 chosen" ID="DdlGovernate" />
                                                <asp:RequiredFieldValidator runat="server" ID="v1" ControlToValidate="DdlGovernate"
                                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <p>
                                    </p>
                                </div>
                            </fieldset>
                            <div class="form-actions center">
                               <button type="submit" class="btn btn-sm btn-success" clientidmode="Static" runat="server"
                                    id="BSearch" onserverclick="BSearch_OnClick">
                                    <%=Tokens.Add %>
                                    <i class="icon-plus icon-on-right bigger-110"></i>
                                </button>
                                <br/>
                                <asp:Literal ID="LMessage" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
                                    <asp:ModalPopupExtender ID="mpe_Receipt" runat="server" BackgroundCssClass="modalBackground"
                                                PopupControlID="div_Receipt" TargetControlID="lblphone" Drag="True" 
                                                DynamicServicePath="" Enabled="True">
                        </asp:ModalPopupExtender>
                        <asp:Panel id="div_Receipt" runat="server" class="modalPopup" ClientIDMode="Static" Visible="False">
    <div id="receptmodal" class="bootbox modal fade in" tabindex="-1" role="dialog" aria-labelledby="stateSelector"
        aria-hidden="false" style="display: block;">
        <div class="modal-dialog">
            <div class="modal-content">
                <div id="receptmodal2">
                <div class="modal-header">
                  <h4 id="stateSelector">
                        <%= Tokens.ReceiptDetails %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                                   <div class="well col-sm-12 form-horizontal">

                            <table width="100%" ID="datatable" dir="rtl" runat="server" ClientIDMode="Static">
                                                                <tr>
                                    <td colspan="2"><h2 style="text-align: center">إيصال</h2>
                                        <img runat="server" ID="imgSite" ClientIDMode="Static" alt="logo" style="width: 100%;height: 30%"/> 
                                        <h4 style="margin: 5px;"><asp:Literal runat="server" ID="lblCompanyName"></asp:Literal></h4>
                                        </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;width: 35%;">
                                        <asp:Label for="txtCustomerName" runat="server" ID="lblCustomerName" Text="<%$Resources:Tokens,Customer.Name %>"/> 
                                        </td><td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;">
                                        <asp:Label CssClass="form-control" ID="txtCustomerName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;width: 35%;">
                                         <asp:Label ID="Label3" for="txtCustomerPhone" Text="<%$Resources:Tokens,Customer.Phone %>" runat="server"/>
                                         </td><td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;">
                                         <asp:Label CssClass="form-control" ID="txtCustomerPhone" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;width: 35%;">
                                         <asp:Label ID="Label15" Text="<%$Resources:Tokens,Prepaid %>" for="txtPrepaid" runat="server"/>
                                         </td><td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;">
                                     <asp:Label CssClass="form-control" ID="txtPrepaid" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr >
                                    <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;width: 35%;">
                                        <asp:Label ID="Label24" for="txtInstallationCost" runat="server" Text="<%$Resources:Tokens,InstallationCost%>"/>
                                        </td>
                                        <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;">
                                    <asp:Label CssClass="form-control" ID="txtInstallationCost" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                                                <tr >
                                    <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;width: 35%;">
                                        <asp:Label ID="Label25" for="txtContractingCost" runat="server" Text="<%$Resources:Tokens,ContractingCost%>"/>
                                        </td>
                                        <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;">
                                    <asp:Label CssClass="form-control" ID="txtContractingCost" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                                                                                <tr >
                                    <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;width: 35%;">
                                        <asp:Label ID="Label1" for="lblDate" runat="server" Text="<%$Resources:Tokens,Date%>"/>
                                        </td>
                                        <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;">
                                    <asp:Label CssClass="form-control" ID="lblDate" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                                                                                                                <tr >
                                    <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;width: 35%;">
                                        <asp:Label ID="Label2" for="lblEmployee" runat="server" Text="<%$Resources:Tokens,Employee%>"/>
                                        </td>
                                        <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;">
                                    <asp:Label CssClass="form-control" ID="lblEmployee" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                                                                                                                                                <tr >
                                    <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;width: 35%;">
                                        <asp:Label ID="Label4" for="lblBranch" runat="server" Text="<%$Resources:Tokens,Branch%>"/>
                                        </td>
                                        <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;">
                                    <asp:Label CssClass="form-control" ID="lblBranch" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>

                        </div>
                    </div>
                </div>
                    </div>
                <div class="modal-footer">
                    <button id="btnPrint" class="btn btn-info" type="button" causesvalidation="True">
                        <%= Tokens.Print %></button>
                       <a id="lnkConta" target="_blank" class="btn btn-primary" runat="server"><%=Tokens.ContractsForEmployee %></a>
                    <%--<button  runat="server" class="btn btn-primary"  type="button" ClientIDMode="Static" onclick="Button1_Click"><%=Tokens.ContractsForEmployee %></button>--%>
                    <%--<asp:LinkButton ID="LinkButton1" runat="server" class="btn btn-primary" onclick="Button1_Click"><%=Tokens.ContractsForEmployee %></asp:LinkButton>--%>
                    <button class="btn btn-default" type="button"  id="btnCancel" data-dismiss="modal" aria-hidden="true"><%=Tokens.Cancel %></button>
                        
                </div>
            </div>
        </div>
    </div>
    </asp:Panel> 
 <%:System.Web.Optimization.Scripts.Render("~/bundles/numberOnly")%>
     
    <script type="text/javascript">
        $(document).ready(function () {

            //$(".chosen-select").chosen();

            numberOnly($('#TbPhone'), $('#nOnlySpan'), "<%=Tokens.NumbersOnly %>");
            
            $('#btnPrint').click(function () {
                var ele = $('#receptmodal2').prop('outerHTML');
                var mywindow = window.open('', 'new div');
                mywindow.document.write('<html><head><title>إيصال</title>');
                 mywindow.document.write('</head><body >');
                mywindow.document.write(ele);
                mywindow.document.write('</body></html>');
                mywindow.document.target = '_blank';
                //$('table img').src($("#imgSite").src);
                mywindow.print();
                mywindow.close();
                return true;
            });
            $('#btnCancel').click(function () {
                $('#receptmodal').css("display", "none");
            });
        });

        jQuery(document).ready(function () {
            jQuery(".chosen").chosen();
        });
    </script>
</asp:Content>
