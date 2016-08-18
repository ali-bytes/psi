<%@ Page Title="<%$Resources:Tokens,UsersSaveMovments %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="UsersSaveMovments.aspx.cs" Inherits="NewIspNL.Pages.UsersSaveMovments" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <link rel="stylesheet" type="text/css" href="http://cdn.datatables.net/1.10.9/css/jquery.dataTables.min.css" />
     <link rel="stylesheet" type="text/css" href="http://cdn.datatables.net/tabletools/2.2.4/css/dataTables.tableTools.css" />
   
   
      <div class="view">
       <fieldset>
           <div class="page-header"><h1><asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,UsersSaveMovments %>"></asp:Literal></h1></div>
           <div class="well">
               <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,sav %>"></asp:Literal>
               <div>
                   <asp:DropDownList runat="server" ID="ddlSaves" DataTextField="SaveName" DataValueField="Id" Width="178px"/>
                   <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlSaves"
                   ErrorMessage="*" ValidationGroup="mov" ID="req1"></asp:RequiredFieldValidator>
               </div>
               <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:Tokens,From %>"></asp:Literal>
               <div>
                   <asp:TextBox runat="server" data-app="db" ID="txtFrom" Width="178px"></asp:TextBox>
                   <asp:RequiredFieldValidator runat="server" ID="requ2" ControlToValidate="txtFrom"
                   ErrorMessage="*" ValidationGroup="mov"></asp:RequiredFieldValidator>
               </div>
               <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:Tokens,To %>"></asp:Literal>
               <div>
                   <asp:TextBox runat="server" data-app="db" ID="txtTo" Width="178px"></asp:TextBox>
                   <asp:RequiredFieldValidator runat="server" ID="req3" ControlToValidate="txtTo"
                   ErrorMessage="*" ValidationGroup="mov"></asp:RequiredFieldValidator>
               </div>
               <br/>
               <asp:Button runat="server" ID="btnSaerch" CssClass="btn btn-primary" Text="<%$Resources:Tokens,Search %>"
               ValidationGroup="mov" OnClick="btnSearch_click"/>
           </div>
       </fieldset>
   </div>
   <div class="view">
       <fieldset>
           <h3 class="header smaller lighter blue"><asp:Literal ID="Literal5" runat="server" Text="<%$Resources:Tokens,Results %>"></asp:Literal></h3>
           <asp:GridView runat="server" ID="gv_Results" AutoGenerateColumns="False" ClientIDMode="Static"
           CssClass="table table-bordered table-condensed text-center">
               <Columns>
                   <asp:BoundField DataField="Time" HeaderText="<%$Resources:Tokens,Date %>"/>
                   <asp:BoundField DataField="SaveName" HeaderText="<%$Resources:Tokens,Name %>"/>
                   <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Amount %>"/>

                   <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,UserName %>"/>
                   <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes %>"/>
                   <asp:BoundField DataField="Notes2" HeaderText="<%$Resources:Tokens,Comment %>"/>
               </Columns>
           </asp:GridView>
           <div class="alert alert-info">
               <table><tr>
                   <td>
                   <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:Tokens,CurrentCreditForPeriod %>"></asp:Literal>&nbsp;   :   &nbsp;
               </td><td>
                     <asp:Label runat="server" ID="lblPeriodTotal"></asp:Label>
               </td>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                   <td>
                   <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:Tokens,CurrentCredit %>"></asp:Literal> &nbsp;: &nbsp;
               </td><td>
                     <asp:Label runat="server" ID="lblTotal"></asp:Label>
               </td>
               </tr></table>
               
             
           </div>
       </fieldset>
   </div>
    <script type="text/javascript" src="http://cdn.datatables.net/1.10.9/js/jquery.dataTables.min.js"></script>
     <script type="text/javascript" src="http://cdn.datatables.net/tabletools/2.2.4/js/dataTables.tableTools.min.js"></script>
   
   <script type="text/javascript">
       $(document).ready(function () {
           $('input[data-app="db"]').datepicker({ dateFormat: 'dd/mm/yy' });
           $('table').each(function () {
               $(this).prepend('<thead></thead>');
               $(this).find('thead').append($(this).find("tr:eq(0)"));
           });
           var table = $('#gv_Results').dataTable({
               "iDisplayLength": 50,
               "aLengthMenu": [[10, 25, 50, 100, 250, 500, -1], [10, 25, 50, 100, 250, 500, "All"]]
           });
           var tableTools = new $.fn.dataTable.TableTools(table, {
               'aButtons': [
                   {
                       'sExtends': 'xls',
                       'sButtonText': 'Save to Excel',
                       'sFileName': 'UsersSaveMovments.xls'
                   },
                   {
                       'sExtends': 'print',
                       'bShowAll': true
                   },
                   'copy'
               ],
               'sSwfPath': '//cdn.datatables.net/tabletools/2.2.4/swf/copy_csv_xls_pdf.swf'
           });
           $(tableTools.fnContainer()).insertBefore('#gv_Results_wrapper');
       })
   </script>
</asp:Content>


