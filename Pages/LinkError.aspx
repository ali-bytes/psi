<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LinkError.aspx.cs" Inherits="NewIspNL.Pages.LinkError" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Link Error</title>
</head>
<body>
    <form id="form1" runat="server">
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <div style="text-align: center; background-color: #ffc; border-style: solid; border-width: thin;
        border-color: Red">
        <br />
        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Tokens,WorngLink %>"
            Font-Bold="True" ForeColor="Red"></asp:Label>
        <br />
        <br />
    </div>
    </form>
</body>
</html>