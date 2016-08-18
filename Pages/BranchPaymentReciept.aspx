<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="BranchPaymentReciept.aspx.cs" Inherits="NewIspNL.Pages.BranchPaymentReciept" %>
 

<%@ Import Namespace="Resources" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
  
            <link href="../Content/ace-assest/ReceiptFormate/style.css" rel="stylesheet" type="text/css" />
    <link href="../Content/ace-assest/ReceiptFormate/responsive.css" rel="stylesheet"
        type="text/css" /> 

</head>
<body dir="rtl">
    <form id="form1" runat="server">
        <div id="box">
    <div id="all" runat="server">
                                        <div id="box-header">
            <div id="logo" runat="server">
               <asp:Image runat="server" ID="ImgLogo"  /> <!--<img src="../Content/ace-assest/ReceiptFormate/الفاتورة.jpg" alt="img"/>-->
            </div>
            <div id="title" runat="server">
                <%= Tokens.PaymentReceipt %>
            </div>
        </div>

        <div id="box-inner">
        <table id="rec" class="to-center" width="100%">
            <tr>
                <td><span>
                    <%= Tokens.Company %></span>
                </td>
                <td runat="server" id="Companyname" class="field">
                </td>
            </tr>
            <tr>
                <td><span>
                    <%= Tokens.Branch %></span>
                </td>
                <td runat="server" id="Branch" class="field">
                </td>
            </tr>
            <tr>
                <td><span>
                    <%= Tokens.Amount %></span>
                </td>
                <td runat="server" id="Amount" class="field">
                </td>
            </tr>
            <tr>
                <td><span>
                    <%= Tokens.Date %></span>
                </td>
                <td runat="server" id="Date" class="field">
                </td>
            </tr>
                                            <tr runat="server" id="userNameRow">
                    <td><span><asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,UserName %>"></asp:Literal></span></td>
                    <td runat="server" id="UserName" class="field"></td>
                </tr>
            <tr>
                <td><span>
                    <%= Tokens.Notes %></span>
                </td>
                <td runat="server" id="Notes" class="field" style="height: auto">
                </td>
            </tr>
        </table></div>
<%--        <table class="to-center">
            <tr>
                <td class="span12" colspan="2">
                    <b runat="server" id="Caution"></b>
                </td>
            </tr> 
            <tr style="text-align: right;">
                <td>
                    <asp:Literal ID="Address" runat="server" />
                </td>
                <td>ختم :</td>
            </tr>
        </table>--%>
                            <div class="to-center" id="cautiondiv">
                <b runat="server" id="Caution"></b>
            </div>
                    <table class="to-center" id="tabelfooter">
                                        <td>
                    <asp:Literal ID="Address" runat="server" />
                </td>
            <tr style="text-align: right;">
                <td>ختم :</td>
            </tr>
        </table>

    </div>  </div>              <div class="noprint">
            <button class="btn btn-success" style="display: none"  onclick="window.print()">
                <i class="icon-white icon-print"></i>
                &nbsp;
                <%= Tokens.Print %>
            </button>
        </div>
        
         <script src="../Content/ace-assest/js/jquery-1.11.1.min.js" type="text/javascript"></script> 

         <script type="text/javascript">
             $(document).ready(function () {

                 var afterPrint = function () {
                     window.close();
                 };

                 function PrintWindow() {
                     window.print();
                 }

                 PrintWindow();

                 //--CLOSE After print For firefox only
                 window.onafterprint = afterPrint;

                 //--CLOSE After print For chrome and other browsers
                 if (window.matchMedia) {
                     var mediaQueryList = window.matchMedia('print');
                     mediaQueryList.addListener(function (mql) {
                         if (mql.matches) {
                             //do some code before print
                         } else {
                             $(document).one('mouseover', afterPrint);
                         }
                     });
                 }
             });


    </script>
  
    </form>
</body>
</html>
