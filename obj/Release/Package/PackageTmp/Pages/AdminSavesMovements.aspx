<%@ Page Title="<%$Resources:Tokens,AdminSavesMovements %>"Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="AdminSavesMovements.aspx.cs" Inherits="NewIspNL.Pages.AdminSavesMovements" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <link rel="stylesheet" type="text/css" href="http://cdn.datatables.net/1.10.9/css/jquery.dataTables.min.css" />
     <link rel="stylesheet" type="text/css" href="http://cdn.datatables.net/tabletools/2.2.4/css/dataTables.tableTools.css" />
   
       <div class="view">
       <fieldset>
           <div class="page-header"><h1><asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,AdminSavesMovements %>"></asp:Literal></h1></div>
           <div class="well">
               <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,sav %>"></asp:Literal>
               <div>
                   <asp:DropDownList runat="server" ID="ddlSaves" DataTextField="SaveName" DataValueField="Id"/>
               </div>
                              <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:Tokens,Employee %>"></asp:Literal>
               <div>
                   <asp:DropDownList  runat="server" ID="ddlEmployee" DataTextField="UserName" DataValueField="ID"/>
<%--    CssClass="width-80 chosen-select"    <select class="width-80 chosen-select" id="form-field-select-3" data-placeholder="Choose a Country...">
																<option value="">&nbsp;</option>
																<option value="AL">Alabama</option>
																<option value="AK">Alaska</option>
																<option value="AZ">Arizona</option>
																<option value="AR">Arkansas</option>
																<option value="CA">California</option>
																<option value="CO">Colorado</option>
																<option value="CT">Connecticut</option>
																<option value="DE">Delaware</option>
																<option value="FL">Florida</option>
                                                                </select>--%>
               </div>
               <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:Tokens,Branch %>"></asp:Literal>
               <div>
                    <asp:DropDownList ID="ddl_Branchs" runat="server" Width="155px" DataTextField="BranchName"
                                                  DataValueField="ID">
                                </asp:DropDownList>
               </div>
                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:Tokens,From %>"></asp:Literal>
               <div>
                   <asp:TextBox runat="server" data-app="db" ID="txtFrom"></asp:TextBox>
               </div>
               <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:Tokens,To %>"></asp:Literal>
               <div>
                   <asp:TextBox runat="server" data-app="db" ID="txtTo"></asp:TextBox>
               </div>
               <asp:Button runat="server" ID="btnSaerch" CssClass="btn btn-primary" Text="<%$Resources:Tokens,Search %>"
               ValidationGroup="mov" OnClick="btnSearch_Click"/>
           </div>
       </fieldset>
   </div>
      <div class="view">
       <fieldset>
           <legend><asp:Literal ID="Literal6" runat="server" Text="<%$Resources:Tokens,Results %>"></asp:Literal></legend>
           <asp:GridView runat="server" ID="gv_Results" AutoGenerateColumns="False" ClientIDMode="Static"
           CssClass="table table-bordered text-center">
               <Columns>
                   <asp:BoundField DataField="Time" HeaderText="<%$Resources:Tokens,Date %>"/>
                   <asp:BoundField DataField="SaveName" HeaderText="<%$Resources:Tokens,Name %>"/>
                   <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Amount %>"/>
<%--                   <asp:BoundField DataField="Total" HeaderText="<%$Resources:Tokens,Total %>"/>--%>
                   <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,UserName %>"/>
                   <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes %>"/>
                   <asp:BoundField DataField="Notes2" HeaderText="<%$Resources:Tokens,Comment %>"/>
               </Columns>
           </asp:GridView>
           <div class="alert alert-info">
                <table><tr>
                   <td>
                   <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:Tokens,CurrentCreditForPeriod %>"></asp:Literal>&nbsp;   :   &nbsp;
               </td><td>
                     <asp:Label runat="server" ID="lblPeriodTotal"></asp:Label>
               </td>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                   <td>
                   <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:Tokens,CurrentCredit %>"></asp:Literal> &nbsp;: &nbsp;
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
           //$(".chosen-select").chosen();
           $('input[data-app="db"]').datepicker({
               showOtherMonths: true,
               selectOtherMonths: false,
               dateFormat: 'dd/mm/yy'
           });

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
                       'sFileName': 'AdminSavesMovements.xls'
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
       });
   </script>
</asp:Content>

