<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FixWorkorderreq.aspx.cs" Inherits="NewIspNL.FixWorkorderreq" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:FileUpload runat="server" ID="fu_sheet"></asp:FileUpload>
        <asp:Button runat="server" ID="Fix" Text="Fix" OnClick="Fix_OnClick"/>
    </div>
    </form>
</body>
</html>
