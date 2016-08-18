<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="Expenses.aspx.cs" Inherits="NewIspNL.Pages.Expenses" %>
  
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        #txtComment {
            height: auto;
        }
    </style>
    <div class="view">
        <fieldset>
            <div class="page-header"><h1><asp:Literal runat="server" ID="lblTitle"></asp:Literal></h1></div>
            <div class="well">
            <table width="100%" id="tb_Paid" style="display: block;">
                <tr>
                    <td colspan="5" >
                        <asp:Label ID="lbl_Save" runat="server" ClientIDMode="Static"></asp:Label>
                    </td>
                </tr>
                <tr id="tr_OutGoingType" runat="server" visible="false">
                    <td>
                        <asp:Label ID="Label7" runat="server" Text="<%$Resources:Tokens,OutgoingTypes %>"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_OutgoingTypes" runat="server" Width="145px" 
                                          DataTextField="Name" DataValueField="ID" ClientIDMode="Static">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="ddl_OutgoingTypes" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                    ValidationGroup="vg_Paid"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 108px">
                        <asp:Label ID="Label5" runat="server" Text="<%$Resources:Tokens,Value %>"></asp:Label>
                    </td>
                    <td style="width: 155px">
                        <span class="input-icon"><asp:TextBox ID="txt_Paid" ClientIDMode="Static" runat="server" Width="145px"></asp:TextBox><i class="icon-dollar"></i></span>
                        <asp:FilteredTextBoxExtender TargetControlID="txt_Paid" ValidChars="1,2,3,4,5,6,7,8,9,0,."
                                                     ID="FilteredTextBoxExtender3" runat="server" Enabled="True">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td style="width: 20px">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txt_Paid"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>"   ValidationGroup="vg_Paid"></asp:RequiredFieldValidator>
                    </td>
                 <%--   <td style="width: 229px">
                        <asp:RadioButtonList ID="rbl_Distination" runat="server" 
                                             RepeatDirection="Horizontal">
                            <asp:ListItem Text="<%$Resources:Tokens,Save %>"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Tokens,Bank1 %>"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Tokens,Bank2 %>"></asp:ListItem>
                   
                        </asp:RadioButtonList>
                    </td>--%>
                <%--    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="rbl_Distination"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="vg_Paid"></asp:RequiredFieldValidator>
                    </td>--%>
                </tr>
                <tr><td style="width: 108px">
                        <asp:Label runat="server" Text="<%$Resources:Tokens,sav %>"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList runat="server" ID="ddlSaves" DataTextField="SaveName"
                         DataValueField="Id" Width="145px" ValidationGroup="vg_Paid" ClientIDMode="Static"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlSaves" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                    ValidationGroup="vg_Paid"></asp:RequiredFieldValidator>
                    </td>
                    </tr>
                <tr>
                    <td style="width: 108px">
                        <asp:Label ID="Label6" runat="server" Text="<%$Resources:Tokens,Comment %>"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txt_PaidComment" runat="server" Width="402px" Rows="5" 
                                     TextMode="MultiLine" ClientIDMode="Static"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_PaidComment"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="vg_Paid"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 108px">
                    </td>
                    <td colspan="3">
                        <asp:Button ID="btn_Payment" CssClass="btn btn-success" runat="server" Text="<%$Resources:Tokens,Submit %>" Width="97px" 
                                    ValidationGroup="vg_Paid" onclick="btn_Payment_Click"  UseSubmitBehavior="false" OnClientClick="plswait(this.id) " />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="5" align="center">
                        <asp:Label ID="lbl_Process" EnableViewState="False" runat="server" 
                                   Font-Bold="True"></asp:Label> </td>
                </tr>
            </table>
            </div>
        </fieldset>
    </div>
                            <asp:ModalPopupExtender ID="mpe_Receipt" runat="server" BackgroundCssClass="modalBackground"
                                                PopupControlID="div_Receipt" TargetControlID="lbl_Save" Drag="True" 
                                                DynamicServicePath="" Enabled="True" CancelControlID="btnCancel">
                        </asp:ModalPopupExtender>
                        <asp:Panel id="div_Receipt" runat="server" class="modalPopup" ClientIDMode="Static">
    <div id="receptmodal" class="bootbox modal fade in" tabindex="-1" role="dialog" aria-labelledby="stateSelector"
        aria-hidden="false" style="display: block;">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
<%--                    <button type="button" class="bootbox-close-button close" data-dismiss="modal">
                        ×</button>--%>
                    <h4 id="stateSelector">
                        <%= Tokens.ReceiptDetails %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <asp:HiddenField runat="server" ID="RejectedRequestId" ClientIDMode="Static" />
                        <div class="well col-sm-12 form-horizontal">
<%--                            <label for="txtSaves" class="col-sm-2">
                                <%= Tokens.SaveValue %>
                            </label>--%>
                            <table width="100%" ID="datatable" dir="rtl" runat="server" ClientIDMode="Static">
                            <tr>
                               
                                    <td colspan="2"> <h3 style="text-align: center">ايصال</h3>
                                    <div style="float: right">
                                    <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
                                    <br/>
                                    <asp:Label runat="server" ID="lblBranch"></asp:Label>
                                    <br/>
                                    <asp:Label runat="server" ID="lblDate"></asp:Label></div>
                                    <img runat="server" ID="imglogo" ClientIDMode="Static" alt="logo" style="width: 25%;float: left"/>
                                    </td>

                                </tr>
                                <tr>
                                    <td style="border: 1px solid black;width: 25%;text-align: center">
                                        <asp:Label for="txtType" runat="server" ID="lblType"/> 
                                        </td><td style="text-align: right;border: 1px solid black;">
                                        <asp:Label CssClass="form-control" ID="txtType" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border: 1px solid black;width: 25%;text-align: center">
                                         <asp:Label for="txtValue" Text="<%$Resources:Tokens,Value %>" runat="server"/>
                                         </td><td style="text-align: right;border: 1px solid black;">
                                         <asp:Label CssClass="form-control" ID="txtValue" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border: 1px solid black;width: 25%;text-align: center">
                                         <asp:Label Text="<%$Resources:Tokens,sav %>" for="txtSaves" runat="server"/>
                                         </td><td style="text-align: right;border: 1px solid black;">
                                     <asp:Label CssClass="form-control" ID="txtSaves" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                                                <tr>
                                    <td style="text-align: center;border: 1px solid black;width: 25%;">
                                         <asp:Label ID="Label1" Text="<%$Resources:Tokens,Employee %>" for="lblEmployeName" runat="server"/>
                                         </td><td style="text-align: right;border: 1px solid black;">
                                     <asp:Label CssClass="form-control" ID="lblEmployeName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr >
                                    <td style="border: 1px solid black;width: 25%;text-align: center">
                                        <asp:Label for="txtComment" runat="server" Text="<%$Resources:Tokens,Comment%>"/>
                                        </td>
                                        <td style="text-align: right;border: 1px solid black;">
                                    <asp:Label CssClass="form-control" ID="txtComment" ClientIDMode="Static" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                            </table>

                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnPrint" class="btn btn-primary" type="button" causesvalidation="True">
                        <%= Tokens.Print %></button>
                    <asp:Button CssClass="btn btn-default" runat="server" ID="btnCancel" data-dismiss="modal" aria-hidden="true" Text="<%$Resources:Tokens,Cancel %>"/>
                        
                </div>
            </div>
        </div>
    </div>
    </asp:Panel>
 
    
    <script type="text/javascript">

        function plswait(id) {

            var type = document.getElementById('<%=ddl_OutgoingTypes.ClientID%>').value;
            var amo = document.getElementById('<%=txt_Paid.ClientID%>').value;
            var sa = document.getElementById('<%=ddlSaves.ClientID%>').value;
            var com = document.getElementById('<%=txt_PaidComment.ClientID%>').value;
       <%--     var op = document.getElementById('<%=rbl_Distination.ClientID%>');--%>
            //var cocheck = op.getElementsByTagName('input');
            
            //var checkedValues = [];

            //for (var i = 0; i < cocheck.length; i++) {
            //    var checkBoxRef = cocheck[i];

            //    if (checkBoxRef.checked == true) {
            //        checkedValues.push( "added");
            //    }
            //}

            if (type == "" || amo == "" || sa == "" || com == "" ) { return; }
               else {
                   var check2 = document.getElementById(id);
                   check2.disabled = 'true'; check2.value = 'Please wait...';
               }

           }



        $(document).ready(function () {
            $('#btnPrint').click(function () {
                var ele = $('#datatable').prop('outerHTML');
                var mywindow = window.open('', 'new div');
                mywindow.document.write('<html><head><title>إيصال</title>');
                /*optional stylesheet mywindow.document.write('<link rel="stylesheet" href="../Content/ace-assest/css/ace-rtl.min.css" />');*/ //
                mywindow.document.write('</head><body onload="window.print();">');
                mywindow.document.write(ele);
                mywindow.document.write('</body></html>');
                mywindow.document.target = '_blank';
                //$('table img').src($('#imglogo').src);
                mywindow.print();
                mywindow.close();
                return true;
            });
        });
    </script>
</asp:Content>
