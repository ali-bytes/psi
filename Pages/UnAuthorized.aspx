<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="UnAuthorized.aspx.cs" Inherits="NewIspNL.Pages.UnAuthorized" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>UnAuthorized</title>
    <style type="text/css">
        .style1 {
            width: 100%;
        }
        .style2 {
            width: 316px;
        }
        .style3 {
            width: 325px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table class="style1">
        <tr>
            <td style="width: 30%">
            </td>
            <td class="style3">
            </td>
            <td style="width: 30%">
            </td>
        </tr>
        <tr>
            <td class="style2">
            </td>
            <td align="center">
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Tokens,AuthorizationError %>"
                    Font-Bold="True" ForeColor="Red"></asp:Label>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="style2">
            </td>
            <td class="style3">
            </td>
            <td>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
