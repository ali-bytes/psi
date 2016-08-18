<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecieptofOutdoorPayment.aspx.cs" Inherits="NewIspNL.Pages.RecieptofOutdoorPayment" %>


<%@ Import Namespace="NewIspNL.Helpers" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Content/ace-assest/ReceiptFormate/style.css" rel="stylesheet" type="text/css" />
    <link href="../Content/ace-assest/ReceiptFormate/responsive.css" rel="stylesheet"
        type="text/css" />
  
    <style type="text/css">
        .smallcell
        {
            width: 92px;
        }
        .big-cell .field
        {
            width: 82.5%;
        }
        .cell span
        {
            font-size: 74%;
        }
        .big-cell
        {
            margin: 0% 1%;
        }
    </style>
    <script src="../Scripts/jquery-2.0.3.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="box">
        <div id="all">
            <div id="box-header">
                 <div class="receipttitle">ايصال استلام نقدية</div>
                <div id="logo" class="col-xs-12">
                    <asp:Image runat="server" ID="LImg" />
                </div>
                <div id="title" class="col-xs-12">
                    <label id="HCompany" runat="server">
                    </label>
                </div>
            </div>
            <div id="Cautionheader">
                <div id="datedata" class="pull-left">
                    التاريخ :
                    <%= DateTime.Now.AddHours().ToString("yyyy-M-d dddd", System.Globalization.CultureInfo.InvariantCulture)%></div>
                <div id="bydata">
                    بواسطة :
                    <asp:Literal ID="lblEmp" runat="server" /></div>
            </div>
         
            <div id="box-inner">
                <div class="inner">
                    <div class="cell">
                        <span>العميل</span>
                        <div class="field">
                            <asp:Literal ID="lblCustomer" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="inner">
                    <div class="cell">
                        <span>تليفون العميل</span>
                        <div class="field">
                            <asp:Literal ID="lblPhone" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="big-cell">
                    <span>اسم الشركة</span>
                    <div class="field">
                        <asp:Literal ID="lblCompany" runat="server" />
                    </div>
                </div>
                <div class="inner">
                    <div class="cell">
                        <span>مبلغ الفاتورة</span>
                        <div class="field">
                            <asp:Literal ID="lblAmount" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="inner">
                    <div class="cell">
                        <span>تاريخ الدفع</span>
                        <div class="field">
                            <asp:Literal ID="lblPayment" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="big-cell">
                    <span>الملاحظات</span>
                    <div class="field">
                        <asp:Literal ID="lblNotes" runat="server" />
                    </div>
                </div>
            </div>
            <div class="to-center" id="cautiondiv">
                <b runat="server" id="Caution"></b>
            </div>
            <table class="to-center" id="tabelfooter">
                <tr style="text-align: right;">

                    <td>
                        ختم :
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
    <script type="text/javascript">
        $(document).ready(function () {
            //$('#btnPrint').click(function () {
            //    window.print();
            //});

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
