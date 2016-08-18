<%@ Page Title="<%$Resources:Tokens ,BranchBalanceSheet%>"  Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="BranchBS.aspx.cs" Inherits="NewIspNL.Pages.BranchBS" %>
<%@ Import Namespace="Resources" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%">
        <tr id="tr_Search" runat="server">
            <td>
                <div class="row">
                <fieldset>
                    <div class="page-header">
                       <h1> <asp:Label ID="l_Search" runat="server" Text="<%$Resources:Tokens,Search %>"></asp:Label></h1>
                    </div>
                    <div class="well">
                    <table width="100%">
                        <tr>
                            <td style="width: 60px">
                                <asp:Label ID="Label34" runat="server" Text="<%$Resources:Tokens,Branches %>"></asp:Label>
                            </td>
                            <td class="style1" style="width: 162px">
                                <asp:DropDownList ID="ddl_Branchs" runat="server" Width="155px" DataTextField="BranchName"
                                                  DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ControlToValidate="ddl_Branchs" ID="RequiredFieldValidator12"
                                                            runat="server"  ErrorMessage="<%$Resources:Tokens,Required %>"
                                                            ValidationGroup="SearchVG"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td  colspan="2">
                                <br/>
                                <asp:LinkButton ID="btn_search" runat="server" Width="100px"
                                            OnClick="btn_search_Click" ValidationGroup="SearchVG" CssClass="btn btn-success"><i class="icon-search icon-only"></i>&nbsp;<%=Tokens.Search %></asp:LinkButton>
                            </td>
                        </tr>
                    </table></div>
                </fieldset>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="row">
                <table width="100%" id="tb_SearchResult" runat="server" visible="false">
                    <tr>
                        <td>
                            <asp:GridView OnDataBound="PutDownloadLinks" Width="100%" ID="grd_Transactions" runat="server" AutoGenerateColumns="False"
                            CssClass="table table-bordered table-responsive center"
                                          CellPadding="4" ForeColor="Black" GridLines="Horizontal" 
                                AllowPaging="True" OnPageIndexChanging="grd_Transactions_PageIndexChanging"
                                          PageSize="25">
                                <Columns>
                                    <asp:BoundField DataField="CreationDate" HeaderText="<%$Resources:Tokens,Date %>"/>
                                    <asp:BoundField DataField="Description" HeaderText="<%$Resources:Tokens,Description %>"/>
                                    <asp:BoundField DataField="DepitAmmount" HeaderText="<%$Resources:Tokens,ImvoiceBS %>"/>
                                    <asp:BoundField DataField="CreditAmmount" HeaderText="<%$Resources:Tokens,CreditBS %>"/>
                                    <asp:BoundField DataField="Total" HeaderText="<%$Resources:Tokens,DepitBS %>"/>
                                    <asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,User %>" />
                                     <asp:TemplateField HeaderText="<%$Resources:Tokens,DownloadInvoice %>">
                                <ItemTemplate>
                                    <asp:HyperLink CssClass="icon-paper-clip icon-only bigger-125 green" runat="server" ID="hl" ToolTip='<%#Eval("FileUrl") %>'  target="_blank"></asp:HyperLink><%--Text="<%$Resources:Tokens,Download %>"--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                             <div >
                       <h3 class="header smaller lighter blue">
                            <asp:Label ID="Label13" runat="server" 
                                       Text="<%$ Resources:Tokens,Totals %>"></asp:Label>
                        </h3>
                    <table   class="table table-bordered table-condensed center" >
                        <tr>
                            <td style="background-color: cadetblue;font-weight: bold;color: white"><asp:Label ID="lblcurrentdemand" runat="server" Text="" Visible="False"></asp:Label></td>
                             <td style="background-color: cadetblue;font-weight: bold;color: #FFCC66;font-size: large"> <asp:Label ID="saf" runat="server" Text="" Visible="False"></asp:Label></td>
                        </tr>
                      

                    </table>

                 </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <fieldset>
                                <h3 class="header smaller lighter blue">
                                    <asp:Label ID="l_BranchDiscount" runat="server" Text="<%$Resources:Tokens,BranchDiscount %>"></asp:Label>
                                </h3>
                                <asp:GridView ID="grd_Discount" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                              ForeColor="Black" GridLines="Horizontal" Width="100%" BackColor="White" BorderColor="#CCCCCC"
                                              BorderStyle="None" BorderWidth="1px">
                                    <Columns>
                                        <asp:BoundField DataField="SPName" HeaderText="<%$Resources:Tokens,Service.Provider %>">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SPTName" HeaderText="<%$Resources:Tokens,PaymentType %>">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DiscountPercent" HeaderText="<%$Resources:Tokens,DiscountPercent %>">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>
                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                    <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                                    <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                    <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                    <SortedDescendingHeaderStyle BackColor="#242121" />
                                </asp:GridView>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <table width="100%" runat="server" id="tb_ProcessPayment">
                                <tr>
                                    <td style="width: 108px">
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Tokens,ProcessType %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="Select1" ClientIDMode="Static">
                                            <asp:ListItem Text="<%$Resources:Tokens,Chose %>" />
                                            <asp:ListItem Text="<%$Resources:Tokens,Add %>" />
                                            <asp:ListItem Text="<%$Resources:Tokens,Subtract %>" />
                                            <asp:ListItem Text="<%$Resources:Tokens,Payment %>" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" id="tb_Add" style="display: none;">
                                <tr>
                                    <td style="width: 108px">
                                        <asp:Label ID="Label2" runat="server" Text="<%$Resources:Tokens,Add %>"></asp:Label>
                                    </td>
                                    <td style="width: 413px">
                                        <asp:TextBox ID="txt_Add" runat="server" Width="145px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender TargetControlID="txt_Add" ValidChars="1,2,3,4,5,6,7,8,9,0,."
                                                                     ID="FilteredTextBoxExtender1" runat="server" Enabled="True">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txt_Add"
                                                                    ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="vg_Add"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 108px">
                                        <asp:Label ID="Label35" runat="server" Text="<%$Resources:Tokens,Comment %>"></asp:Label>
                                    </td>
                                    <td style="width: 413px">
                                        <asp:TextBox ID="txt_AddComment" runat="server" Width="402px" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txt_AddComment"
                                                                    ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="vg_Add"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 108px">
                                    </td>
                                    <td style="width: 413px">
                                        <asp:Button ID="btn_Add" runat="server" Text="<%$Resources:Tokens,Submit %>" Width="97px"
                                                    ValidationGroup="vg_Add" OnClick="btn_Add_Click" UseSubmitBehavior="false" OnClientClick="plswait(this.id)" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" id="tb_Sub" style="display: none;">
                                <tr>
                                    <td style="width: 108px">
                                        <asp:Label ID="Label3" runat="server" Text="<%$Resources:Tokens,Subtract %>"></asp:Label>
                                    </td>
                                    <td style="width: 413px">
                                        <asp:TextBox ID="txt_Sub" runat="server" Width="145px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender TargetControlID="txt_Sub" ValidChars="1,2,3,4,5,6,7,8,9,0,."
                                                                     ID="FilteredTextBoxExtender2" runat="server" Enabled="True">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_Sub"
                                                                    ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="vg_Sub"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 108px">
                                        <asp:Label ID="Label4" runat="server" Text="<%$Resources:Tokens,Comment %>"></asp:Label>
                                    </td>
                                    <td style="width: 413px">
                                        <asp:TextBox ID="txt_SubComment" runat="server" Width="402px" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_SubComment"
                                                                    ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="vg_Sub"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 108px">
                                    </td>
                                    <td style="width: 413px">
                                        <asp:Button ID="btn_Sub" runat="server" Text="<%$Resources:Tokens,Submit %>" Width="97px"
                                                    ValidationGroup="vg_Sub" OnClick="btn_Sub_Click" UseSubmitBehavior="false" OnClientClick="plswait(this.id)" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" id="tb_Paid" style="display: none;" runat="server">
                                <tr>
                                    <td style="width: 108px">
                                        <asp:Label ID="Label5" runat="server" Text="<%$Resources:Tokens,Paid %>"></asp:Label>
                                    </td>
                                    <td style="width: 155px">
                                        <asp:TextBox ID="txt_Paid" runat="server" Width="145px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender TargetControlID="txt_Paid" ValidChars="1,2,3,4,5,6,7,8,9,0,."
                                                                     ID="FilteredTextBoxExtender3" runat="server" Enabled="True">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td style="width: 20px">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txt_Paid"
                                                                    ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="vg_Paid"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 229px">
                                      <%--  <asp:RadioButtonList ID="rbl_Distination" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="<%$Resources:Tokens,Save %>"></asp:ListItem>
                                            <asp:ListItem Text="<%$Resources:Tokens,Bank1 %>"></asp:ListItem>
                                            <asp:ListItem Text="<%$Resources:Tokens,Bank2 %>"></asp:ListItem>
                                        </asp:RadioButtonList>--%>
                                    </td>
                                    <td>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="rbl_Distination"
                                                                    ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="vg_Paid"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr >
                                    <td style="width: 108px">
                                    <asp:Label ID="Label12" runat="server" Text="<%$Resources:Tokens,sav %>"></asp:Label></td>
                                    <td colspan="3">
                                        <div><asp:DropDownList runat="server" ID="ddlSaves" DataTextField="SaveName" DataValueField="Id" ValidationGroup="vg_Paid"/>
                    <asp:RequiredFieldValidator ControlToValidate="ddlSaves" ValidationGroup="vg_Paid" ID="RequiredFieldValidator6" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 108px">
                                        <asp:Label ID="Label6" runat="server" Text="<%$Resources:Tokens,Comment %>"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_PaidComment" runat="server" Width="402px" Rows="5" TextMode="MultiLine"></asp:TextBox>
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
                                       
                                        <asp:Button ID="btn_Payment" runat="server" Text="<%$Resources:Tokens,Submit %>"
                                                    Width="97px" ValidationGroup="vg_Paid" OnClick="btn_Payment_Click" UseSubmitBehavior="false" OnClientClick="plswait(this.id)" />
                                        &nbsp;<asp:Button ID="btn_ViewReceipt" runat="server" Text="<%$Resources:Tokens ,PaymentReceipt%>" Width="120px"
                                                          ValidationGroup="vg_Paid" Enabled="False" OnClick="btn_ViewReceipt_Click" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbl_Process" runat="server" EnableViewState="False" Font-Bold="True"
                                       ForeColor="Red"></asp:Label>
                            <asp:ModalPopupExtender ID="mpe_Receipt" runat="server" BehaviorID="mdl" BackgroundCssClass="modalBackground"
                                                    PopupControlID="div_Receipt" TargetControlID="Label34" Drag="True" DynamicServicePath=""
                                                    Enabled="True">
                            </asp:ModalPopupExtender>
                            <asp:Panel id="div_Receipt" runat="server" class="modalPopup">
                                <table border="1" width="700px" id="tb_Receipt" runat="server" ClientIDMode="Static">
                                    <tr>
                                        <td style="text-align: center">
                                            <h3 style="float: right; margin: 10px 90px 0 0;">
                                                إيصال استلام نقدية</h3>
                                            <div style="float: left">
                                            <asp:Image runat="server" ID="ImgLogo" AlternateText="Logo" Height="80px"/>
                                            </div>
                                            <%--<img src="../Templates/AhlanTheme/mini-Logo.jpg" alt="Logo" style="height: 88px; width: 150px" /></div>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 20px; text-align: left; vertical-align: middle">
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lbl_ReceiptNo" runat="server" Font-Bold="True"></asp:Label>
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lbl_dt" runat="server" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="direction: rtl; padding-right: 20px; text-align: right;">
                                            <asp:Label ID="Label7" runat="server" Text="إستلمت من السيد : "></asp:Label>
                                            <asp:Label ID="lbl_CustomerName" runat="server" CssClass="receiptvlaue"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                    <td style="text-align: right; padding-right: 20px; direction: rtl">
                                        <asp:Label ID="Label8" runat="server" Text="مبلغ : " />
                                        <asp:Label ID="LBL_Amount" runat="server" CssClass="receiptvlaue"></asp:Label>
                                    </td>
                                </tr>
                                    <tr>
                                        <td style="direction: rtl; display: none; padding-right: 20px; text-align: right;">
                                            <asp:Label ID="Label9" runat="server" Text="نظير : "></asp:Label>
                                            <asp:Label ID="lbl_For" runat="server" CssClass="receiptvlaue"></asp:Label>
                                        </td>
                                    </tr>
                                                                    <tr>
                                    <td style="text-align: right; padding-right: 20px; direction: rtl">
                                        <asp:Label ID="Label10" runat="server" Text="المستخدم : "></asp:Label>
                                        <asp:Label ID="lbl_User" runat="server" CssClass="receiptvlaue"></asp:Label>
                                    </td>
                                </tr>
                                    <tr>
                                        <td style="text-align: center">
                                            <asp:Label ID="Label11" runat="server" Text="تنبيه " Font-Bold="True"></asp:Label>
                                            <asp:Label ID="LCaution" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="background-color: #F3F3F3; direction: rtl">
                                        <div runat="server" ID="DAddress"></div>
                                        </td>
                                    </tr>
                                </table>
                                <center>
<%--                                    <asp:Button ID="btn_Print" runat="server" Text="<%$Resources:Tokens ,Print%>" Width="100px" 
                                                onclick="btn_Print_Click" />--%>
                                                <a target="_blank" style="width: 100px" id="btnPrint" class="btn btn-info"><%=Tokens.Print %></a>
                                    &nbsp;
                                    <asp:Button ID="btn_Cancel" runat="server" CssClass="btn btn-default" Text="<%$Resources:Tokens ,Cancel%>" Width="100px" />
                                </center>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                </div>
            </td>
        </tr>
    </table>
  <%:System.Web.Optimization.Scripts.Render("~/bundles/paymentBs")%>
    <%--<script src="../Content/app/paymentBs.js" type="text/javascript"> </script>--%>
    <script type="text/javascript">
        


        function plswait(id) {

            var amoadd = document.getElementById('<%=txt_Add.ClientID%>').value;
             var comadd = document.getElementById('<%=txt_AddComment.ClientID%>').value;

             var amosub = document.getElementById('<%=txt_Sub.ClientID%>').value;
             var comsub = document.getElementById('<%=txt_SubComment.ClientID%>').value;


             var amopay = document.getElementById('<%=txt_Paid.ClientID%>').value;
            <%-- var op = document.getElementById('<%=rbl_Distination.ClientID%>');
             var bankpay = op.getElementsByTagName('input');--%>
             var savepay = document.getElementById('<%=ddlSaves.ClientID%>').value;
            var compay = document.getElementById('<%=txt_PaidComment.ClientID%>').value;

             if (id == "ctl00_ContentPlaceHolder1_btn_Add") {
                 if (amoadd == "" || comadd == "") { return; }
                 else {
                     var check1 = document.getElementById(id);
                     check1.disabled = 'true'; check1.value = 'Please wait...';
                 }
             }
             else if (id == "ctl00_ContentPlaceHolder1_btn_Sub") {

                 if (amosub == "" || comsub == "") { return; }
                 else {
                     var check2 = document.getElementById(id);
                     check2.disabled = 'true'; check2.value = 'Please wait...';
                 }

             }

             else if (id == "ctl00_ContentPlaceHolder1_btn_Payment") {
                 //var checkedValues = [];

                 //for (var i = 0; i < bankpay.length; i++) {
                 //    var checkBoxRef = bankpay[i];

                 //    if (checkBoxRef.checked == true) {
                 //        checkedValues.push("added");
                 //    }
                 //}

                 if (amopay == "" || savepay == "" || compay == "") { return; }
                 else {
                     var check3 = document.getElementById(id);
                     check3.disabled = 'true'; check3.value = 'Please wait...';
                 }


             }

         }










        $(document).ready(function () {
            $('#Select1').change(function () {
                var one = $('#tb_Add');
                var two = $('#tb_Sub');
                var three = $('#ctl00_ContentPlaceHolder1_tb_Paid');
                var t1 = "<%= Tokens.Add %>";
                var t2 = "<%= Tokens.Subtract %>";
                var t3 = "<%= Tokens.Payment %>";
                showHidThree(one, two, three, t1, t2, t3, $(this).val());
            });
            $('#btnPrint').click(function () {
                var ele = $('#tb_Receipt').prop('outerHTML');
                var mywindow = window.open('', 'new div');
                mywindow.document.write('<html><head><title>إيصال</title>');
                /*optional stylesheet*/ //mywindow.document.write('<link rel="stylesheet" href="main.css" type="text/css" />');
                mywindow.document.write('</head><body >');
                mywindow.document.write(ele);
                mywindow.document.write('</body></html>');
                mywindow.document.target = '_blank';
                mywindow.print();
                mywindow.close();
                HideModalPopup();
                return true;
            });

            function HideModalPopup() {
                $find("mdl").hide();
                
            };

            /*$('button[data-val="paid"]').click(function () {
                //var woid = $(this).attr('data-work');
                //$('#hdfId').val(woid);
                $('#PaidModal').modal('show');
            });*/
        });

    </script>
   
</asp:Content>