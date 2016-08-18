<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderReceipt.aspx.cs" Inherits="NewIspNL.Pages.OrderReceipt" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
                <span runat="server" Visible="False" id="Msg" class="alert alert-success"></span>
        <span runat="server" Visible="False" id="Msg2" class="alert alert-danger"></span>
                            <table width="100%" ID="datatable" dir="rtl" runat="server" ClientIDMode="Static">
                                                                <tr>
                                    <td colspan="2"><h2 style="text-align: center">إيصال الدفع المقدم</h2>
                                        <asp:Image ClientIDMode="Static" runat="server" ID="imgSite" alt="logo" style="width: 100%;height: 30%"/> 
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
                                         <asp:Label ID="Label4" for="txtCustomerPhone" Text="<%$Resources:Tokens,Customer.Phone %>" runat="server"/>
                                         </td><td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;">
                                         <asp:Label CssClass="form-control" ID="txtCustomerPhone" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;width: 35%;">
                                         <asp:Label ID="Label15" Text="<%$Resources:Tokens,Amount %>" for="txtPrepaid" runat="server"/>
                                         </td><td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;">
                                     <asp:Label CssClass="form-control" ID="txtPrepaid" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr >
                                    <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;width: 35%;">
                                        <asp:Label ID="Label24" for="lblReceiptNumber" runat="server" Text="<%$Resources:Tokens,ReceiptNumber%>"/>
                                        </td>
                                        <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;">
                                    <asp:Label CssClass="form-control" ID="lblReceiptNumber" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr >
                                    <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;width: 35%;">
                                        <asp:Label ID="Label25" for="txtDate" runat="server" Text="<%$Resources:Tokens,PaymentDate%>"/>
                                        </td>
                                        <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;">
                                    <asp:Label CssClass="form-control" ID="txtDate" runat="server"></asp:Label>
                                    </td>
                                </tr>

                                <tr >
                                    <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;width: 35%;">
                                        <asp:Label ID="Label7" for="lblEmployee" runat="server" Text="<%$Resources:Tokens,Employee%>"/>
                                        </td>
                                        <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;">
                                    <asp:Label CssClass="form-control" ID="lblEmployee" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr >
                                    <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;width: 35%;">
                                        <asp:Label ID="Label8" for="lblBranch" runat="server" Text="<%$Resources:Tokens,Branch%>"/>
                                        </td>
                                        <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;">
                                    <asp:Label CssClass="form-control" ID="lblBranch" runat="server"></asp:Label>
                                    </td>
                                </tr>    
                                 <tr>
                                    <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;width: 35%;">
                                        <asp:Label ID="Label6" for="lblNotes" runat="server" Text="<%$Resources:Tokens,Notes%>"/>
                                        </td>
                                        <td style="text-align: right; padding-right: 20px; direction: rtl;border: 1px solid black;">
                                    <asp:Label CssClass="form-control" ID="lblNotes" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <br/>
                            <button style="display: none" onclick="window.print()">print</button>

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

