<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="ResellerPaymentReciept.aspx.cs" Inherits="NewIspNL.Pages.ResellerPaymentReciept" %>




<%@ Import Namespace="Resources" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 
    <link href="../Content/ace-assest/ReceiptFormate/style.css" rel="stylesheet" type="text/css" />
    <link href="../Content/ace-assest/ReceiptFormate/responsive.css" rel="stylesheet"
        type="text/css" />
   
</head>
<body style="direction: rtl;">
    <form id="form1" runat="server" >
        <div id="box">
            <div id="all" runat="server" ><!--style="border: 1px solid gray;"-->
                    <div id="box-header">
            <div id="logo" runat="server">
               <asp:Image runat="server" ID="ImgLogo"  /> 
            </div>
            <div id="title" runat="server">
                <%= Tokens.PaymentReceipt %>
            </div>
        </div>
            
            <div id="box-inner">
            <table id="rec" class="to-center" width="100%">
                <tr>
                    <td>
                        <span>
                        <%= Tokens.Company %></span>
                    </td>
                    <td runat="server" id="Companyname" class="field">
                    </td>
                </tr>
                <tr runat="server" id="resellerrow">
                    <td>
                        <span >
                            <%= Tokens.Reseller %></span>
                    </td>
                    <td runat="server" id="Reseller" class="field">
                    </td>
                </tr>
                <tr>
                    <td>
                        <span>
                        <%= Tokens.Amount %></span>
                    </td>
                    <td runat="server" id="Amount" class="field">
                    </td>
                </tr>
                <tr>
                    <td>
                        <span>
                        <%= Tokens.Date %></span>
                    </td>
                    <td runat="server" id="Date" class="field">
                    </td>
                </tr>
                                <tr runat="server" id="NameRow">
                    <td>
                        <span>
                        <%=Tokens.Customer_Name%></span>
                    </td>
                    <td runat="server" id="CustomerName" class="field">
                    </td>
                </tr>
                <tr runat="server" id="PhoneRow">
                                        <td>
                                            <span>
                        <%=Tokens.Customer_Phone%></span>
                    </td>
                    <td runat="server" id="CustomerPhone" class="field">
                    </td>
                </tr>
                <tr runat="server" id="userNameRow">
                    <td><span><asp:Literal ID="lbluser" runat="server" Text="<%$Resources:Tokens,User %>"></asp:Literal></span></td>
                    <td runat="server" id="UserName" class="field"></td>
                </tr>
                <tr>
                    <td>
                       <span> <%= Tokens.Notes %></span>
                    </td>
                    <td runat="server" id="Notes" class="field">
                    </td>
                </tr>
            </table></div>
            

            <div class="to-center" id="cautiondiv">
                <b runat="server" id="Caution"></b>
            </div>
                    <table class="to-center" id="tabelfooter">
            <tr style="text-align: right;">

                <td>ختم :</td>
            </tr>
        </table>
        </div>
        </div>
            
    <!--box-->
        <div class="noprint">
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

