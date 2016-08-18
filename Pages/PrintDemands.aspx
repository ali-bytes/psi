<%@ Page Title="<%$ Resources:Tokens,PrintDemands %>" EnableEventValidation="false" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="PrintDemands.aspx.cs" Inherits="NewIspNL.Pages.PrintDemands" %>


<%@ Import Namespace="NewIspNL.Helpers" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  
    <style type="text/css">
      
        @media print {
            .no-print {
                display: none;

            }
             .ftb {
    border-collapse: collapse;
}

.ftb, th, td {
    border: 1px solid black !important;
}

        }
    </style>
    <div class="no-print">
        <div>
            <div class="view">
                <fieldset>
                    <div class="page-header">
                        <h1>
                            <%= Tokens.Search %></h1>
                    </div>
                    <div class="well">
                        <div>
                            <label for="DdlPaymentTypes">
                                <%= Tokens.PaymentTypes %></label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlPaymentTypes" ClientIDMode="Static">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RddlPaymentTypes" ErrorMessage="*" ControlToValidate="DdlPaymentTypes"
                                    runat="server" ValidationGroup="dem" />
                            </div>
                        </div>
                        <div>
                            <label for="DdlYear">
                                <%=Tokens.Year %>
                            </label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlYear" Width="106px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="*" ControlToValidate="DdlYear"
                                    runat="server" ValidationGroup="dem" />
                            </div>
                        </div>
                        <div>
                            <label for="DdlMonth">
                                <%=Tokens.Month %></label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlMonth" Width="106px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="*" ControlToValidate="DdlMonth"
                                    runat="server" ValidationGroup="dem" />
                            </div>
                        </div>
                    </div>
                    <button class="btn btn-success" runat="server" id="BSearch" validationgroup="dem">
                        <i class="icon-white icon-search"></i>&nbsp;
                        <%= Tokens.Search %>
                    </button>
                </fieldset>
            </div>
        </div>
        <div class="view" id="divdata">
            <fieldset>
                <h3 class="header smaller lighter blue">
                    <%= Tokens.CustomerDemands %></h3>
                <asp:GridView AutoGenerateColumns="False" runat="server" CssClass="table table-bordered table-condensed text-center"
                    ID="GvDemands" OnDataBound="BindGrid">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblNu"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" HeaderText="<%$Resources:Tokens,Customer %>" />
                        <asp:BoundField DataField="Phone" HeaderText="<%$Resources:Tokens,Phone %>" />
                        <asp:BoundField DataField="Mobile" HeaderText="<%$Resources:Tokens,Mobile %>" />
                        <asp:BoundField DataField="Governorate" HeaderText="<%$Resources:Tokens,Governorate %>" />
                        <asp:BoundField DataField="Address" HeaderText="<%$Resources:Tokens,Address %>" />
                        <asp:BoundField DataField="Provider" HeaderText="<%$Resources:Tokens,Provider %>" />
                        <asp:BoundField DataField="Package" HeaderText="<%$Resources:Tokens,Package %>" />
                        <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Amount %>" />
                        <asp:BoundField DataField="Offer" HeaderText="<%$Resources:Tokens,Offer %>" />
                        <asp:BoundField DataField="TStart" HeaderText="<%$Resources:Tokens,From %>" />
                        <asp:BoundField DataField="TEnd" HeaderText="<%$Resources:Tokens,To %>" />
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <input type="checkbox" id="selectAllDemand" data-rel="tooltip" title="<%=Tokens.Print %>" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Checkbox1" type="checkbox" runat="server" data-select="input" />
                                <% if (PrintModel != null)
                                   { %>
                                <%--  <div data-select="print-area" style="display: none;margin-bottom: 7px;">
                                                 <div id="box"><div id="all" class="rds-table-stacked">
                                                                   <div id="box-header" >
                                                                       <div id="logo">
                                                                           <img id="LImg" alt="logo" src="<%= PrintModel.LogoUrl %>">
                                                                       </div>
                                                                       <div id="title">
                                                                                                                                       <h1 id="HCompany">
                                                                <%= PrintModel.CompanyName %></h1>
                                                                                     <div id="datedata" class="pull-left">
                    التاريخ :
                    <%= DateTime.Now.AddHours().ToString("yyyy-M-d dddd", System.Globalization.CultureInfo.InvariantCulture)%></div>
                                                                       </div>
                                                                   </div>
                                                                   <table id="box-inner">
                                                                       <div class="inner">
                                                                                               <div class="rds-column">
                        <span class="rds-cell">العميل</span>
                        <div class="rds-cell">
                            <asp:Literal ID="Literal15" Text='<%#Eval("Name") %>' runat="server" />
                        </div>
                    </div>
                                        <div class="cell">
                        <span>التليفون</span>
                        <div class="field">
                           <asp:Literal ID="Literal11" Text='<%#Eval("Phone") %>' runat="server" />
                        </div>
                    </div>
                                        <div class="cell">
                        <span>المحافظة</span>
                        <div class="field">
                            <asp:Literal ID="Literal12" Text='<%#Eval("Governorate") %>' runat="server" />
                        </div>
                    </div>
                                        <div class="cell">
                        <span>السنترال</span>
                        <div class="field">
                             <asp:Literal ID="Literal13" Text='<%#Eval("Central") %>' runat="server" />
                        </div>
                    </div>
                                                            <div class="cell">
                        <span>عنوان العميل</span>
                        <div class="field">
                             <asp:Literal ID="Literal1" Text='<%#Eval("Address") %>' runat="server" />
                        </div>
                    </div>
                                        <div class="cell">
                        <span>السرعة</span>
                        <div class="field">
                             <asp:Literal ID="Literal14" Text='<%#Eval("Package") %>' runat="server" />
                        </div>
                    </div>
                                                                       </div>
                                                                       <div class="inner">
                                                                                                                   <div class="cell">
                        <span>المبلغ</span>
                        <div class="field">
                             <asp:Literal ID="Literal16" Text='<%#Eval("Amount") %>' runat="server" />
                        </div>
                    </div>
                                                            <div class="cell">
                        <span>من</span>
                        <div class="field">
                            <asp:Literal ID="Literal17" Text='<%#Eval("TStart") %>' runat="server" />
                        </div>
                    </div>
                                                            <div class="cell">
                        <span>الى</span>
                        <div class="field">
                             <asp:Literal ID="Literal18" Text='<%#Eval("TEnd") %>' runat="server" />
                        </div>
                    </div>
                                                            <div class="cell">
                        <span>الموظف</span>
                        <div class="field">
                             <asp:Literal ID="Literal19" Text='<%#Eval("User") %>' runat="server" />
                        </div>
                    </div>
                                                            <div class="cell">
                        <span>الموبيل</span>
                        <div class="field">
                             <asp:Literal ID="Literal2" Text='<%#Eval("Mobile") %>' runat="server" />
                        </div>
                    </div>
                                                            <div class="cell">
                        <span>الملاحظات</span>
                        <div class="field">
                             <asp:Literal ID="Literal20" Text='<%#Eval("Notes") %>' runat="server" />
                        </div>
                    </div>
                                                                       </div>
                                                                   </table>
         <div style="margin: 0px; padding:0px; clear: both; height: 6px">
         &nbsp;
       </div>
             <div class="to-center" id="cautiondiv">
                <b id="Caution"><%= PrintModel.Caution %></b>
            </div>
            <table class="to-center" id="tabelfooter">
                <tr style="text-align: right;">
                    <td>
                        <%= PrintModel.ContactData %>
                    </td>
                </tr>
            </table>

                                        
                                        </div>
                                    </div></div>--%>

                                <div id="box" data-select="print-area" style="display: none !important;margin-bottom: 7px;direction: rtl !important;">
                                    <div id="all">
                                        <div id="box-header" style=" margin: 0 auto !important;direction: rtl !important;">
                                            <div>
                                                <div id="logo">
                                                    <%--<img id="LImg" alt="logo" src="<%= PrintModel.LogoUrl %>"/>--%>
                                                </div>

                                            </div>
                                            <div id="title" style=" margin: 0 auto !important;direction: rtl !important;">

                                                <table class="ftb" style=" margin: 0 auto !important;direction: rtl !important;">
                                                    <tr>
                                                        <td class="clr">بواسطة :
                                                        </td>
                                                        <td>
                                                            <asp:Literal ID="Literal19" Text='<%#Eval("User") %>' runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="clr">التاريخ : 
                                                        </td>
                                                        <td>
                                                            <%= DateTime.Now.AddHours().ToString("yyyy-M-d dddd", System.Globalization.CultureInfo.InvariantCulture)%> 
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="clr">
                                                            <asp:Literal runat="server" ID="lblRecieptNum" Text="رقم الايصال :"></asp:Literal>
                                                        </td>
                                                        <td>
                                                            <asp:Literal runat="server" ID="RecieptNum"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                </table>

                                            </div>
                                            <div id="companycorn" style=" margin: 0 auto !important;direction: rtl !important;">
                                                <h3 style="margin: 0px 0 0 0 !important;">
                                                    <label class="stretch" id="HCompany" >
                                                        <%= PrintModel.CompanyName %>
                                                    </label>
                                                </h3>
                                            </div>
                                        </div>
                                        <div id="box-inner" style=" margin: 0 auto !important;">
                                            <div class="inner" style=" margin: 0 auto !important;">
                                                <table class="ftb" style=" margin: 0 auto !important;direction: rtl !important; border-collapse: collapse !important;">
                                                    <tr>
                                                        <td width="15%" class="clr" style=" border: 1px solid black !important; ">العميل
                                                        </td>
                                                        <td width="20%"  style=" border: 1px solid black !important; ">
                                                             <asp:Literal ID="Literal15" Text='<%#Eval("Name") %>' runat="server" />
                                                        </td>
                                                        <td width="15%" class="clr" style=" border: 1px solid black !important; ">تليفون
                                                        </td>
                                                        <td width="35%" style=" border: 1px solid black !important; ">
                                                             <asp:Literal ID="Literal11" Text='<%#Eval("Phone") %>' runat="server" />
                                                        </td>

                                                    </tr>
                                                      <tr>
                                                        <td class="clr" style=" border: 1px solid black !important; ">المحافظة
                                                        </td>
                                                        <td style=" border: 1px solid black !important; ">
                                                            <asp:Literal ID="Literal1" Text='<%#Eval("Governorate") %>' runat="server" />
                                                        </td>
                                                        <td class="clr" style=" border: 1px solid black !important; ">السنترال
                                                        </td>
                                                        <td style=" border: 1px solid black !important; ">
                                                           <asp:Literal ID="Literal13" Text='<%#Eval("Central") %>' runat="server" />
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td width="15%" class="clr" style=" border: 1px solid black !important; ">من
                                                        </td>
                                                        <td width="30%" style=" border: 1px solid black !important; ">
                                                           <asp:Literal ID="Literal17" Text='<%#Eval("TStart") %>' runat="server" />
                                                        </td>
                                                        <td width="15%" class="clr" style=" border: 1px solid black !important; ">الى
                                                        </td>
                                                        <td style=" border: 1px solid black !important; ">
                                                            <asp:Literal ID="Literal18" Text='<%#Eval("TEnd") %>' runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="clr" style=" border: 1px solid black !important; ">عنوان العميل
                                                        </td>
                                                        <td style=" border: 1px solid black !important; ">
                                                            <asp:Literal ID="Literal2" Text='<%#Eval("Address") %>' runat="server" />
                                                        </td>
                                                        <td class="clr" style=" border: 1px solid black !important; ">السرعة
                                                        </td>
                                                        <td style=" border: 1px solid black !important; ">
                                                          <asp:Literal ID="Literal14" Text='<%#Eval("Package") %>' runat="server" />
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td class="clr" style=" border: 1px solid black !important; ">ملاحظات
                                                        </td>
                                                        <td colspan="3" style=" border: 1px solid black !important; ">
                                                             <asp:Literal ID="Literal20" Text='<%#Eval("Notes") %>' runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="clr" style=" border: 1px solid black !important; ">المبلغ
                                                        </td>
                                                        <td style=" border: 1px solid black !important; ">
                                                          <asp:Literal ID="Literal16" Text='<%#Eval("Amount") %>' runat="server" />
                                                        </td>
                                                        <td class="clr" style=" border: 1px solid black !important; ">الإمضاء
                                                        </td>
                                                        <td style=" border: 1px solid black !important; ">
                                                            <asp:Literal ID="ss" runat="server" />
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>
                                        </div>

                                        <div style="margin: 0px !important; padding: 0px !important; clear: both !important; height: 6px !important">
                                            &nbsp;
                                        </div>

                                        <div id="tabelfooter">
                                          <%--  <table class="signfooter">
                                                <tr style="text-align: center;">
                                                    <td class="clr">الختـم</td>
                                                </tr>
                                                <tr>
                                                    <td rowspan="3" id="sign" style="height: 67px"></td>
                                                </tr>
                                            </table>--%>

                                            <table class="compInf" style=" margin: 0 auto !important">
                                                <tr>
                                                    <td class="clr" style="text-align: center !important;">Company</td>
                                                </tr>
                                                <tr>
                                                    <td rowspan="3" id="compInformation">
                                                        <label id="Address" style="margin-bottom: 0px !important; font-size: 6pt !important;"> <%= PrintModel.ContactData %></label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="to-center" id="cautiondiv" style=" margin: 0 auto !important">
                                                <b id="Caution"><%= PrintModel.Caution %></b>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <% } %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </fieldset>

        </div>
    </div>

    <div class="alert alert-info" runat="server" id="lblTotal" visible="False"></div>
    <button class="btn btn-primary" type="button" id="print">
        <i class="icon-print icon-only"></i>&nbsp;<%= Tokens.Print %></button>
    <asp:LinkButton CssClass="btn btn-default" ID="btnExport" runat="server" OnClick="Export" Visible="False">
                   <i class="icon-file-text icon-only"></i>&nbsp;<%=Tokens.Export %>
    </asp:LinkButton>

    <div id="to-print">
    </div>

     <script src="../Content/ace-assest/js/jquery-1.11.1.min.js" type="text/javascript"></script> 
    

    <script type="text/javascript">
        $(function () {
            var $printArea = $('#to-print');
            $('#print').on('click', function () {
                var checks = $('input[data-select="input"]');
                for (var i = 0; i < checks.length; i++) {
                    var item = $(checks[i]);
                    if (item.is(':checked')) {
                        var rec = item.next('div[data-select="print-area"]').clone();
                        $printArea.append($(rec).show());
                    }
                }

                printDiv();
            });

            $('#selectAllDemand').click(function () {  //on click 
                if (this.checked) { // check select status
                    $('input[type="checkbox"]').each(function () { //loop through each checkbox
                        this.checked = true;  //select all checkboxes
                    });
                } else {
                    $('input[type="checkbox"]').each(function () { //loop through each checkbox
                        this.checked = false; //deselect all checkboxes 
                    });
                }
            });
        });

        //var afterPrint = function () {
        //    window.close();
        //};

        //function PrintWindow() {
        //    window.print();
        //}
        //window.onafterprint = afterPrint;

        function printDiv() {
            ////Get the HTML of div
            //var divElements = document.getElementById('to-print').outerHTML;
            //Get the HTML of whole page

            //Reset the page's HTML with div's HTML only
            //document.body.innerHTML =
            //  "<html><head><title></title>" +
            //      "<link href='../Content/ace-assest/ReceiptFormate/style.css' rel='stylesheet' type='text/css' media='print' /> <link href='../Content/ace-assest/ReceiptFormate/responsive.css' rel='stylesheet' type='text/css' media='print' /> <link href='../Content/p.css' rel='stylesheet' media='print' /> </head><body>" +
            //      "<style type='text/css'>@media print {.smallcell{width: 92px;}.cell span{font-size: 74%;width: 72px;text-align: left;} #box {width:20cm}}</style>" +
            //  divElements + "</body>";
            //window.print();


            var ele = $('#to-print').prop('outerHTML');
            var mywindow = window.open('', 'new div');
            mywindow.document.write('<html><head><title>إيصال</title>');
            //mywindow.document.write("<link href='../Content/ace-assest/ReceiptFormate/style.css' rel='stylesheet' type='text/css' media='print' /> <link href='../Content/ace-assest/ReceiptFormate/responsive.css' rel='stylesheet' type='text/css' media='print' /> </head><body>");
            mywindow.document.write('</head><body >');
            mywindow.document.write(ele);
            mywindow.document.write('</body></html>');
            mywindow.document.target = '_blank';
            mywindow.print();
            //mywindow.close();


        }
    </script>

</asp:Content>

