<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateCustomers.aspx.cs" Inherits="NewIspNL.CreateCustomers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:DropDownList runat="server" ID="DdlStstus">
       
    </asp:DropDownList>
        <div>
            <asp:Button ID="Button1" Text="text" runat="server" OnClick="Create"/>
            start
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            count
            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        </div>
    </form>
</body>
</html>
