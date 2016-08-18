<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="UpdatedResellerReceipt.aspx.cs" Inherits="NewIspNL.Pages.UpdatedResellerReceipt" %>

<%@ Import Namespace="Resources" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
                <tr runat="server" id="companyrow">
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
                        <%= Tokens.Reseller %>
                    </td>
                    <td>
                        :&nbsp;&nbsp;
                    </td>
                    <td runat="server" id="Reseller">
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
            <button class="btn btn-success"  onclick="window.print()">
                <i class="icon-white icon-print"></i>
                &nbsp;
                <%= Tokens.Print %>
            </button>
        </div>
    </form>
</body>
</html>
