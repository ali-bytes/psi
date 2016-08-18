<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="smallDemandReciept.aspx.cs" Inherits="NewIspNL.Pages.smallDemandReciept" %>

<%@ Import Namespace="NewIspNL.Helpers" %>
<%@ Import Namespace="Resources" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <link href="../Content/ace-assest/ReceiptFormate/style.css" rel="stylesheet" type="text/css" />
    <link href="../Content/ace-assest/ReceiptFormate/responsive.css" rel="stylesheet"
        type="text/css" />
     
    <style type="text/css">
        @media print {
            #btnPrint {
                display: none
            }
        }
        #Cautionheader #bydata {
            width: 50%;
            font-size: 11px;
        }
        #Cautionheader #datedata {
            width: 30%;
            float: left;
            font-size: 11px;
        }
        #rec .field {
            height: 20px;
        }
        br {
            /*display: block;*/
            /*margin: 3px 0;*/
            /*padding: 5px;*/
        }
        #Address2 {
            text-align: center;
            width: 70%;
            font-size: 11px;
        }
        #rec {
            font-size: 18px;
             
        }
        td.container > div { width: 100%; height: 100%; overflow:hidden; }
        td.container { height: 20px; }
    </style>
</head>
<body style="direction: rtl;margin: 0">
    <form id="form1" runat="server">
        <div id="box" >
            <div id="all" runat="server" style="width: 9cm">
    <div >

        
        <div id="box-header">
            <div class="receipttitle">ايصال استلام نقدية</div>
            <div id="logo" style="width: 90%;height: 80px">
               <asp:Image runat="server" ID="LImg" Height="80px" /> <!--<img src="../Content/ace-assest/ReceiptFormate/الفاتورة.jpg" alt="img"/>-->
            </div>
            <div id="title" style="margin:0;height: 29%;width: 100%;text-align: center">
                <asp:Label runat="server" ID="HCompany"></asp:Label>
            </div>
        </div>
                    <div id="Cautionheader">
                <div id="datedata" class="pull-left">
                                        <asp:Literal runat="server" ID="lblRecieptNum" Text="رقم الايصال :"></asp:Literal>
                    <asp:Literal ID="RecieptNum" runat="server" />
                </div>
                <div id="bydata">
                                    التاريخ :
                    <%= DateTime.Now.AddHours().ToString("yyyy-M-d", System.Globalization.CultureInfo.InvariantCulture)%>
</div>
            </div>
        <div id="box-inner" >
            <table id="rec" class="to-center" width="100%">
                <tr>
                    <td><span>
                        العميل</span>
                    </td>
                    <td class="field" id="Customer" runat="server">
                        
                    </td>
                </tr>
                <tr>
                    <td><span>
                        تليفون</span>
                    </td>
                    <td class="field">
                        <asp:Literal ID="Phone" runat="server" />
                    </td>
                </tr>
                               <%-- <tr>
                    <td><span>
                        الموبيل</span>
                    </td>
                    <td class="field">
                        <asp:Literal ID="lblMobile" runat="server" />
                    </td>
                </tr>
                                <tr>
                    <td><span>
                        العنوان</span>
                    </td>
                    <td class="field">
                        <asp:Literal ID="lblAddress" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td><span>
                        المحافظة</span>
                    </td>
                    <td class="field">
                        <asp:Literal ID="Gov" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td><span>
                        السنترال</span>
                    </td>
                    <td class="field">
                        <asp:Literal ID="Central" runat="server" />
                    </td>
                </tr>--%>
                <tr>
                    <td><span>
                        مزود الخدمة </span>
                    </td>
                    <td class="field"><asp:Literal runat="server" ID="lblServiceProvider"></asp:Literal></td>
                </tr>
                <tr>
                    <td><span>
                        السرعة</span>
                    </td>
                    <td class="field">
                        <asp:Literal ID="Pack" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td><span>
                        المبلغ</span>
                    </td>
                    <td class="field">
                        <asp:Literal ID="Amount" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td><span>
                        من</span>
                    </td>
                    <td class="field">
                        <asp:Literal ID="StartAt" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td><span>
                        الى</span>
                    </td>
                    <td class="field">
                        <asp:Literal ID="EndAt" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <span>
                        الموظف</span>
                    </td>
                    <td class="field">
                        <asp:Literal ID="Emp" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td><span>
                        التعليق</span>
                    </td>
                    <td class="field" style="height: auto">
                        <asp:Literal ID="Notes" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td><span>الامضاء</span></td>
                    <td class="field"></td>


                </tr>
                <tr>
                    <td style="padding-left:5px;" <%--class="container"--%>>
                        <div style="height: 50px;">
                        <span>ختم</span>
                       </div>
                    </td>
                    <td <%--class="field container"--%>>
                        <table border="1" width="98%" style="min-height: 70px; margin-right: 7px; margin-left: auto;">
                          <tr style="border: 0px"><td style="border: 0px">
                              &nbsp;
                              &nbsp;
                              &nbsp;
                          </td></tr></table>
                         <%--<div style="min-height: 50px; height: 50px;border: 1px">--%>
                        <%-- </div>--%>
                    </td>
                </tr>
            </table>
        </div>

       <div style="margin: 0px; padding:0px; clear: both; height: 6px">
         &nbsp;
       </div>
        
                    <div class="to-center" id="cautiondiv">
                <b runat="server" id="Caution"></b>
            </div>
        <hr/>
        <%-- <div style="margin: 0px; padding:0px; clear: both; height: 6px">
         &nbsp;
       </div>--%>
        <table class="to-center" id="tabelfooter">
            <tr>
                <td>
        <div ID="Address3" runat="server" style="display:inline-block; margin-left: auto; margin-right: auto; text-align: center; font-size: 13px;">
             <%--<asp:Literal ID="Address" runat="server" />--%>
            <%--<asp:Label ID="Address2" runat="server"></asp:Label>--%>
        </div>
                    </td>
                </tr>
            </table>
                   <%-- <table class="to-center" id="tabelfooter">
            <tr style="text-align: right;">
                <td>
                    <asp:Literal ID="Address" runat="server" />
                    <br/>
                </td>--%>
               <%-- <td>
                      <div style=" margin-right: -85px; float: right">
                        <asp:Label  ID="sig" runat="server" Text="<%$Resources:Tokens,sign %>  "></asp:Label>:
                   </div>
                    <div>
                    ختم :
                    
                    </div>

                </td>--%>
           <%-- </tr>
        </table>--%>
        

    </div>
    </div>
    </div>

    <button class="btn btn-success" id="btnPrint"  ><%=Tokens.Print %></button> 
    <script src="../Content/ace-assest/js/jquery-1.11.1.min.js" type="text/javascript"></script> 
     <script type="text/javascript">
         $(document).ready(function () {

             $('#btnPrint').click(function () {
                 $('#box').replaceWith($("#box").html());
                     window.print();
                 });

             //-- New Print Code --
             var afterPrint = function () {
                 window.close();
             };


             //function PrintWindow() {
             //    window.print();
             //}

             //PrintWindow();

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

