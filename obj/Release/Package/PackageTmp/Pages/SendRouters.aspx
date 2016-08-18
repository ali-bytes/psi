<%@ Page Title="<%$Resources:Tokens,SendRouters %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="SendRouters.aspx.cs" Inherits="NewIspNL.Pages.SendRouters" %>


<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>
     <link rel="stylesheet" type="text/css" href="http://cdn.datatables.net/1.10.9/css/jquery.dataTables.min.css" />
     <link rel="stylesheet" type="text/css" href="http://cdn.datatables.net/tabletools/2.2.4/css/dataTables.tableTools.css" />
   
 
      <div class="view">
        <fieldset>
        <div class="page-header"><h1><asp:Label ID="lbl1" runat="server" Text="<%$Resources:Tokens,SendRouters %>"></asp:Label></h1></div>
        <div runat="server" id="Msgsuccess" Visible="False" class="alert alert-success"><%=Tokens.Saved %></div>
        <div runat="server" id="MsgError" Visible="False" class="alert alert-danger"></div>
        <div id="drRouters">
            <asp:Label  ID="Label34" AssociatedControlID="ddlRouters" runat="server" Text="<%$ Resources:Tokens,Routers %>"></asp:Label>
                            <div>
                                
                        <asp:DropDownList ID="ddlRouters"  ClientIDMode="Static" runat="server" Width="150px" CssClass="width-60 chosen-select">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ControlToValidate="ddlRouters" ID="RequiredFieldValidator1"
                                                    runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>" 
                                                    ValidationGroup="customer"></asp:RequiredFieldValidator>
                      
                    </div><br/>
        </div>
                   
            
            
               <fieldset>
            <div class="row">
                <div class="col-sm-4">
                    <div class="well">
                        <div>
                            <label for="DdlReseller">
                                <%= Tokens.Reseller %></label>
                            <div>
                                <asp:DropDownList runat="server" CssClass="chosen-select" ID="DdlReseller" ClientIDMode="Static" >
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div>
                            <label for="DdlBranch">
                                <%= Tokens.Branch %></label>
                            <div>
                                <asp:DropDownList runat="server" CssClass="chosen-select" ID="DdlBranch" ClientIDMode="Static" >
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="well">
                        <div>
                            <label for="DdlGovernorate">
                                <%= Tokens.State %></label>
                            <div>
                                <asp:DropDownList runat="server" CssClass="chosen-select" ID="Ddlstate" ClientIDMode="Static" >
                                </asp:DropDownList>
                            </div>
                        </div>
                        
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="well">
                        <div style="margin-bottom: 60px;">
                            <label for="DdlSeviceProvider">
                                <%=Tokens.Service_Provider %></label>
                            <div>
                                <asp:DropDownList runat="server" CssClass="chosen-select" ID="DdlSeviceProvider" ClientIDMode="Static" >
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <p>
                <button validationgroup="foo" class="btn btn-success" runat="server" id="BSearch"  OnServerClick="FilterResults" >
                    <i class="icon-white icon-search"></i>&nbsp;
                    <%= Tokens.All %>
                </button>
                <button validationgroup="foo" class="btn btn-success" runat="server" id="Button1"  OnServerClick="FilterResults2" >
                    <i class="icon-white icon-search"></i>&nbsp;
                    <%= Tokens.Search %>
                </button>
                 <asp:HiddenField runat="server" ID="hdnUrl" ClientIDMode="Static"/>
            </p>
        </fieldset>
        
            
            
            
            
            
             
        <asp:GridView ID="grd_wo" runat="server" CssClass="table table-bordered table-condensed text-center"
                    ClientIDMode="Static" AutoGenerateColumns="False" DataKeyNames="ID" OnDataBound="grd_wo_DataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="l_number" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustomerName" HeaderText="<%$ Resources:Tokens,Customer %>">
                        </asp:BoundField>
                        <asp:BoundField DataField="CustomerPhone" HeaderText="<%$ Resources:Tokens,Phone %>">
                        </asp:BoundField>
                        <asp:BoundField HeaderText="<%$Resources:Tokens,RequestNumber %>" DataField="RequestNumber" />
                        <asp:BoundField DataField="GovernorateName" HeaderText="<%$ Resources:Tokens,Governrate%>">
                        </asp:BoundField>
                        <asp:BoundField DataField="CentralName" HeaderText="<%$Resources:Tokens,Central %>"/>
                        <asp:BoundField DataField="ServicePackageName" HeaderText="<%$ Resources:Tokens,Package %>">
                        </asp:BoundField>
                        <asp:BoundField DataField="StatusName" HeaderText="<%$ Resources:Tokens,State%>">
                        </asp:BoundField>
                         <asp:BoundField DataField="activationdate" HeaderText="<%$ Resources:Tokens,Activation.Date%>">
                        </asp:BoundField>
                        <asp:BoundField DataField="SPName" HeaderText="<%$ Resources:Tokens,Provider %>">
                        </asp:BoundField>
                        <asp:BoundField DataField="BranchName"  HeaderText="<%$ Resources:Tokens,Branch %>">
                        </asp:BoundField>
                        <asp:BoundField DataField="Reseller" HeaderText="<%$ Resources:Tokens,Reseller %>">
                        </asp:BoundField>
                      
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,RecieveToCustomer %>">
                            <ItemTemplate>
                                <button type="button" runat="server" data-work='<%#Eval("ID") %>' class="btn btn-success btn-sm" data-rel="tooltip"
                                 id="btntoCustomer" data-app="customer"  clientidmode="Static" title="<%$Resources:Tokens,RecieveToCustomer %>"><i class="icon-ok icon-only"></i></button>
                               </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="#FF3300" Text="<%$ Resources:Tokens,NoResults %>"></asp:Label>
                    </EmptyDataTemplate>
                </asp:GridView>
              
              
                <asp:HiddenField runat="server" ID="hdfId" ClientIDMode="Static"/>
               
                <div id="CustomerModel" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="modelheader"
        aria-hidden="true">
                                                        <div class="modal-dialog">
            <div class="modal-content">
        <div class="modal-header">
            <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                ×</button>
            <h4 id="modelheader">
                <%= Tokens.RecieveToCustomer %></h4>
        </div>
        <div class="modal-body">
            <div class="bootbox-body">
            <div>
                <div>
                    <div id="dropDiv"></div>

                <asp:Label runat="server" Text="<%$Resources:Tokens,AttachFile %>"></asp:Label>
                <div>
                    <asp:FileUpload runat="server" ID="fileUpload1"/>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"
                    ControlToValidate="fileUpload1" ValidationGroup="customer"></asp:RequiredFieldValidator>
                </div>
                <div>
                    <asp:FileUpload runat="server" ID="fileUpload2"/>
                </div>
                </div>
            </div>
            </div>
        </div>
        <div class="modal-footer">
            <asp:Button runat="server" CssClass="btn btn-primary" ID="btnRecieveToCustomer" 
                ValidationGroup="customer" Text="<%$Resources:Tokens,Save %>" 
                onclick="btnRecieveToCustomer_Click"/>
            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="True"><%=Tokens.Cancel %></button>
        </div>
        </div></div>
        </div>
        </fieldset>
    </div>
     <script type="text/javascript">
         $(function () {

             $(".chosen-select").chosen();
             $("#drRouters").appendTo($("#dropDiv"));
             $('button[data-app="customer"]').click(function () {
                 var woid = $(this).attr('data-work');
                 $('#hdfId').val(woid);
                 $('#CustomerModel').modal('show');
             });

         });
           </script>
      <script type="text/javascript" src="http://cdn.datatables.net/1.10.9/js/jquery.dataTables.min.js"></script>
     <script type="text/javascript" src="http://cdn.datatables.net/tabletools/2.2.4/js/dataTables.tableTools.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('table').each(function () {
                $(this).prepend('<thead></thead>');
                $(this).find('thead').append($(this).find("tr:eq(0)"));
            });

            var table = $('#grd_wo').dataTable({
                "iDisplayLength": 50
            });
            var tableTools = new $.fn.dataTable.TableTools(table, {
                'aButtons': [
                    {
                        'sExtends': 'xls',
                        'sButtonText': 'Save to Excel',
                        'sFileName': 'SendRouters.xls'
                    },
                    {
                        'sExtends': 'print',
                        'bShowAll': true
                    },
                    'copy'
                ],
                'sSwfPath': '//cdn.datatables.net/tabletools/2.2.4/swf/copy_csv_xls_pdf.swf'
            });
            $(tableTools.fnContainer()).insertBefore('#grd_wo_wrapper');
        });

      
    </script>

</asp:Content>

