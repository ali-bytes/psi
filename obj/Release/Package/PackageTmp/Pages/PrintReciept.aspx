<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="PrintReciept.aspx.cs" Inherits="NewIspNL.Pages.PrintReciept" %>




<%@ Import Namespace="Resources" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        @media print {
            .no-print {
                display: none;
            }
        }
        .pull-right {
            direction: rtl;
            text-align: right;
            border: 1px solid gray;
            padding: 10px;
            width: 19cm;
        }
        .caution {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <% if (Cnfg != null)
       { %>
    <div class="pull-right">
        <div>
            <table style="width: 100%">
                <% var width = string.Format("{0}%", (Convert.ToDecimal(1) / Convert.ToDecimal(3)) * 100); %>
                <tr>
                    <td style="width: <%= width %>">
                        <% if (Cnfg.LogoPosiotion == 1)
                           { %>
                        <img src="../PrintLogos/<%= Cnfg.LogoUrl %>" alt="<%= Cnfg.CompanyName %>" height="100px"/>
                        <% } %>
                    </td>
                    <td style="width: <%= width %>">
                        <% if (Cnfg.LogoPosiotion == 2)
                           { %>
                        <img src="../PrintLogos/<%= Cnfg.LogoUrl %>" alt="<%= Cnfg.CompanyName %>" />
                        <% } %>
                    </td>
                    <td style="width: <%= width %>">
                        <% if (Cnfg.LogoPosiotion == 3)
                           { %>
                        <img src="../PrintLogos/<%= Cnfg.LogoUrl %>" alt="<%= Cnfg.CompanyName %>" />
                        <% } %>
                    </td>
                </tr>
            </table>
        </div>
        <h2>
            <%= Cnfg.CompanyName %></h2>
        <hr />
        <div>
            <table>
                <tr>
                    <td>
                        <asp:Literal ID="LNumber" runat="server" />
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Literal ID="LDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal Text="اسم العميل" runat="server" />
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <strong>
                            <asp:Literal ID="LCustomer" runat="server" /></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal Text="السرعة" runat="server" />
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <strong>
                            <asp:Literal ID="LPackage" runat="server" /></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal Text="نظير" runat="server" />
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <strong>
                            <asp:Literal ID="LFor" runat="server" /></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal Text="بوسطة" runat="server" />
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <strong>
                            <asp:Literal ID="LBy" runat="server" /></strong>
                    </td>
                </tr>
            </table>
            <div>
                <asp:Literal ID="LContact" runat="server" />
            </div>
        </div>
        <% if (!string.IsNullOrWhiteSpace(Cnfg.Caution))
           { %>
        <div class="caution">
            <strong>
                <%= Cnfg.Caution %></strong>
        </div>
        <hr />
        <% } %>
        <div>
            <%= Cnfg.ContactData %>
        </div>
        <div>
            SMART ISP - PIONNERS - www.pit-egypt.com
        </div>
    </div>
    <% }
       else
       { %>
    <span class="no-print">تاكد من توافر طريقة العرض</span>
    <% } %>
    <div class="no-print">
        <button type="button" onclick="window.print()">
            <%=Tokens.Print %></button>
    </div>
    </form>
</body>
</html>
