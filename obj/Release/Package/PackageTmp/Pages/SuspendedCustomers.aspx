<%@ Page Title="<%$ Resources:Tokens,SuspendedCustomers %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="SuspendedCustomers.aspx.cs" Inherits="NewIspNL.Pages.SuspendedCustomers" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
       <link rel="stylesheet" type="text/css" href="http://cdn.datatables.net/1.10.9/css/jquery.dataTables.min.css" />
     <link rel="stylesheet" type="text/css" href="http://cdn.datatables.net/tabletools/2.2.4/css/dataTables.tableTools.css" />
   
    <style type="text/css">
        .modal-dialog {
            width: 810px;
        }
        
        body .modal-content {
    /* new custom width */
    width: 1000px;
    /* must be half of the width, minus scrollbar on the left (30px) */
    /*margin-left: -280px;*/
}
    </style>
    
       <%-- <asp:Panel ID="Panel1" runat="server">--%>
            <div class="page-header"><h1><asp:Literal runat="server" ID="lblindex" Text="<%$ Resources:Tokens,Results %>"></asp:Literal></h1></div>
           
          <%--  <div id="result-output">--%>
                              <%--  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>    --%>
                <asp:GridView ID="gv_customers"  runat="server" AutoGenerateColumns="False" ClientIDMode="Static" CssClass="table table-bordered table-condensed" OnRowDataBound="gv_customers_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="gv_l_number" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustomerName" HeaderText="<%$ Resources:Tokens,Customer %>" />
                        <asp:BoundField DataField="CustomerPhone" HeaderText="<%$ Resources:Tokens,Phone%>" />
                        <asp:BoundField DataField="GovernorateName" HeaderText="<%$ Resources:Tokens,Governrate %>" />
                        <asp:BoundField DataField="Provider" HeaderText="<%$ Resources:Tokens,Provider%>" />
                        <asp:BoundField DataField="Resseller" HeaderText="<%$ Resources:Tokens,Reseller %>" />
                        <asp:BoundField DataField="Branch" HeaderText="<%$ Resources:Tokens,Branch %>" />
                        <asp:BoundField DataField="Days" HeaderText="<%$ Resources:Tokens,Days %>" />
                        <asp:BoundField DataField="RequestDate" DataFormatString="{0:d}" HeaderText="<%$ Resources:Tokens,SuspendDate %>" />
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="color: blue; font-weight: bold;">
                            <asp:Literal Text="<%$ Resources:Tokens,NoResults %>" runat="server" /></div>
                    </EmptyDataTemplate>
                </asp:GridView>

          <%--  </ContentTemplate>
            </asp:UpdatePanel>--%>
           <%-- </div>--%>
            <div id="pop-me-up" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true" style="width: 100%">
                                                    <div class="modal-dialog">
            <div class="modal-content">
                <asp:HiddenField ID="count" runat="server" ClientIDMode="Static" />
                <div class="modal-header">
            <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                ×</button>
            <h4 id="myModalLabel">
                Over Suspend days</h4>
                    <div class="row">
                        <div class="col-md-offset-5">
                         <asp:Button ID="btn_cancelAll" runat="server" Text="<%$ Resources:Tokens,CancelCustomers %>"
                                       CssClass="btn btn-warning" OnClick="btn_cancelAll_Click"/>
                            </div>
                    </div>
                     <div id="DivSuccess" runat="server"></div>
            <div id="Div1" runat="server"></div>
            <div id="DivCount" runat="server"></div>
        </div>
                <div class="modal-body">
                                 <div class="bootbox-body">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ClientIDMode="Static" DataKeyNames="ID" CssClass="table table-bordered table-condensed" OnRowDataBound="GridView1_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="gv_l_number" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustomerName" HeaderText="<%$ Resources:Tokens,Name %>" />
                        <asp:BoundField DataField="CustomerPhone" HeaderText="<%$ Resources:Tokens,Phone %>" />
                        <asp:BoundField DataField="GovernorateName" HeaderText="<%$ Resources:Tokens,Governrate%>" />
                        <asp:BoundField DataField="Provider" HeaderText="<%$ Resources:Tokens,Provider %>" />
                        <asp:BoundField DataField="RequestDate" DataFormatString="{0:d}" HeaderText="<%$ Resources:Tokens,Request.Date %>" />
                        <asp:BoundField DataField="Resseller" HeaderText="<%$ Resources:Tokens,Reseller %>" />
                        <asp:BoundField DataField="Branch" HeaderText="<%$ Resources:Tokens,Branch %>" />
                        <asp:BoundField DataField="Days" HeaderText="<%$ Resources:Tokens,Days %>" />
                         <asp:BoundField DataField="req" HeaderText="<%$ Resources:Tokens,PendingRequest %>" />
                        <asp:TemplateField>
                                                <HeaderTemplate>
                            <input id="selectall"  type="checkbox"/>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="selecttocancel" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>

                            <asp:Literal Text="<%$ Resources:Tokens,NoResults %>" runat="server" />
                    </EmptyDataTemplate>
                </asp:GridView>
                </div></div>
                <div class="modal-footer">
            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="True">Close</button>
        </div>
                </div></div>
            </div>
       <%-- </asp:Panel>--%>
     <script type="text/javascript" src="http://cdn.datatables.net/1.10.9/js/jquery.dataTables.min.js"></script>
     <script type="text/javascript" src="http://cdn.datatables.net/tabletools/2.2.4/js/dataTables.tableTools.min.js"></script>
   
    <script type="text/javascript">
        $(document).ready(function () {
            
            $('#footerdiv').css("display", "none");
           
            $('table').each(function() {
                $(this).prepend('<thead></thead>');
                $(this).find('thead').append($(this).find("tr:eq(0)"));
            });
            $('table').dataTable({
                "iDisplayLength": 50
            });
            var table = $('#gv_customers').dataTable();
            var tableTools = new $.fn.dataTable.TableTools(table, {
                'aButtons': [
                    {
                        'sExtends': 'xls',
                        'sButtonText': 'Save to Excel',
                        'sFileName': 'Data.xls'
                    },
                    {
                        'sExtends': 'print',
                        'bShowAll': true
                    },
                    'copy'
                ],
                'sSwfPath': '//cdn.datatables.net/tabletools/2.2.4/swf/copy_csv_xls_pdf.swf'
            });
            $(tableTools.fnContainer()).insertBefore('#gv_customers_wrapper');
            var pdiv = $("#gv_customers_wrapper").parent().closest('div');
            pdiv.css("position", "absolute");
            //$('#footerdiv').css({
            //    position: "fixed",
            //    bottom :0
            //});
            $(document.body).append($('#footerdiv'));
            $('#gv_customers').on('draw.dt', function () {
                $('#footerdiv').css("display", "block"); 
                $("#gv_customers_next").append($('#footerdiv'));
            });
        });

        $(function () {
            var count = $('#count').val();
            if (count == "true") {
          
                $('#pop-me-up').modal('show');
            }
            $('#selectall').click(function (event) {  //on click 
                if (this.checked) { // check select status
                    $('input[type="checkbox"]').each(function () { //loop through each checkbox
                        this.checked = true;  //select all checkboxes with class "checkbox1"               
                    });
                } else {
                    $('input[type="checkbox"]').each(function () { //loop through each checkbox
                        this.checked = false; //deselect all checkboxes with class "checkbox1"                       
                    });
                }
            });
            //$("#GridView1 td").each(function () {
            //    $(this).html().replace("True", "Yes");
            //    $(this).html().replace("False", "No");
            //});
           
        });
    </script>

</asp:Content>
