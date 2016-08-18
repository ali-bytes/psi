<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="ResellerDemandReciept.aspx.cs" Inherits="NewIspNL.Pages.ResellerDemandReciept" %>




<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="NewIspNL.Helpers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <link href="../Content/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #rcpt-content td
        {
            border-bottom: 1px solid rgb(213, 213, 213);
            padding-right: 25px;
            padding-top: 7px;
        }
        #rcpt-content tr:first-child
        {
            border-top: 1px solid rgb(213, 213, 213);
        }
    </style>
</head>
<body style="direction: rtl;">
    <form id="form1" runat="server">
    <div style="border: 1px solid gray; max-width: 21cm">
        <table style="width: 21cm">
            <tr>
                <td class="span4" style="padding: 15px">
                    <h3 id="HCompany" runat="server">
                    </h3>
                </td>
                <td class="span4">
                     <span>رقم الايصال</span>&nbsp; <span>
                    <asp:Literal runat="server" ID="RecieptNum"></asp:Literal></span>
                </td>
                <td style="padding: 15px">
                    <asp:Image runat="server" ID="LImg" />
                </td>

            </tr>
        </table>
         <div style="text-align: center; margin-left: 8px;width: 4cm">
                <span>بواسطة</span>&nbsp; <span>
                                            <asp:Literal ID="Emp" runat="server" /></span></div>
        <div>
            <div style="text-align: left; margin-left: 18px;float: left">
                <span>التاريخ</span>&nbsp; <span>
                                               <%= DateTime.Now.AddHours().ToString("yyyy-M-d dddd",CultureInfo.InvariantCulture)%></span></div>
                <div style="text-align: center; margin-left: 18px;width: 18cm">
               </div>

        </div>
        <div style="font-size: 13pt; width: 100%; line-height: 12pt; font-weight: 700">
            <table id="rcpt-content" width="100%" style="text-align: right; border-color: rgb(184, 184, 184);
                border-left: 0; border-right: 0;">
                <tr>
                    <td ><!--class="span2"-->
                        العميل
                    </td>
                    <td ><!--class="span1"-->
                        :
                    </td>
                    <td ><!--class="span8"-->
                        <asp:Literal ID="Customer" runat="server" />
                    </td>
                    <td>
                        السرعة
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:Literal ID="Pack" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        تليفون
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:Literal ID="Phone" runat="server" />
                    </td>
                    <td>
                        المبلغ
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:Literal ID="Amount" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        المحافظة
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:Literal ID="Gov" runat="server" />
                    </td>
                    <td>
                        من
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:Literal ID="StartAt" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        السنترال
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:Literal ID="Central" runat="server" />
                    </td>
                    <td>
                        الى
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:Literal ID="EndAt" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        مزود الخدمة 
                    </td>
                    <td> : </td>
                    <td><asp:Literal runat="server" ID="lblServiceProvider"></asp:Literal></td>
                    <td>
                        ملاحظات
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:Literal ID="Notes" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <table style="text-align: center">
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
        </table>
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

