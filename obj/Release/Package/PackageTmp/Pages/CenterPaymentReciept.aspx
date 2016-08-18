<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="CenterPaymentReciept.aspx.cs" Inherits="NewIspNL.Pages.CenterPaymentReciept" %>
<%@ Import Namespace="Resources" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        @media print { .noprint{ display: none;} }
         #rec{ line-height: 24pt; width: 100%}
        #rec tr { border-bottom: 1px solid #C9C9C9;  }
        #all { width: 18cm; padding: 5px; height: 14.85cm}
        .to-center{ text-align: center; padding: 20px;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="all">
            <div>
                <asp:Image runat="server" ID="ImgLogo" style="height: 130px;max-height: 130px;" /></div>
            <h1 class="to-center">
                <%= Tokens.PaymentReceipt %>
            </h1>
            <table id="rec">
                <tr>
                    <td>
                        <%= Tokens.Company %>
                    </td>
                    <td>
                        :&nbsp;&nbsp;
                    </td>
                    <td runat="server" id="Companyname">
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= Tokens.Center %>
                    </td>
                    <td>
                        :&nbsp;&nbsp;
                    </td>
                    <td runat="server" id="Center">
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= Tokens.Amount %>
                    </td>
                    <td>
                        :&nbsp;&nbsp;
                    </td>
                    <td runat="server" id="Amount">
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= Tokens.Date %>
                    </td>
                    <td>
                        :&nbsp;&nbsp;
                    </td>
                    <td runat="server" id="Date">
                    </td>
                </tr>
<%--                                <tr runat="server" id="NameRow">
                    <td>
                        <%=Tokens.Customer_Name%>
                    </td>
                    <td>
                        :&nbsp;&nbsp;
                    </td>
                    <td runat="server" id="CustomerName">
                    </td>
                </tr>
                <tr runat="server" id="PhoneRow">
                                        <td>
                        <%=Tokens.Customer_Phone%>
                    </td>
                    <td>
                        :&nbsp;&nbsp;
                    </td>
                    <td runat="server" id="CustomerPhone">
                    </td>
                </tr>--%>
                <tr>
                    <td>
                        <%= Tokens.Notes %>
                    </td>
                    <td>
                        :&nbsp;&nbsp;
                    </td>
                    <td runat="server" id="Notes">
                    </td>
                </tr>
            </table>
        </div>
        <div class="noprint">
            <button class="btn btn-success" style="display: none"  onclick="window.print()">
                <i class="icon-white icon-print"></i>
                &nbsp;
                <%= Tokens.Print %>
            </button>
        </div>
    </form>
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

</body>
</html>
