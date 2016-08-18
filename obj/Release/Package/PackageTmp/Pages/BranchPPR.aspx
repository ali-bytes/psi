<%@ Page Title="<%$Resources:Tokens,BranchPPR%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="BranchPPR.aspx.cs" Inherits="NewIspNL.Pages.BranchPPR" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<style>
   .approve-header {
       padding:9px 15px;
    border-bottom:1px solid #eee;
    background-color: #BEEB9F;
    -webkit-border-top-left-radius: 5px;
    -webkit-border-top-right-radius: 5px;
    -moz-border-radius-topleft: 5px;
    -moz-border-radius-topright: 5px;
       -ms-border-top-left-radius: 5px;
       border-top-left-radius: 5px;
       -ms-border-top-right-radius: 5px;
       border-top-right-radius: 5px;
   }
    .reject-header {
       padding:9px 15px;
    border-bottom:1px solid #eee;
    background-color: #F4B57D;
    -webkit-border-top-left-radius: 5px;
    -webkit-border-top-right-radius: 5px;
    -moz-border-radius-topleft: 5px;
    -moz-border-radius-topright: 5px;
       -ms-border-top-left-radius: 5px;
       border-top-left-radius: 5px;
       -ms-border-top-right-radius: 5px;
       border-top-right-radius: 5px;
   }

</style>
<asp:HiddenField ID="reload" runat="server" />
<div runat="server" id="Div1" >
    </div>
    <div runat="server" id="portalRequest" >
    </div>
    <div class="row">
    <div class="view">
        <div class="page-header"><h1><%=Tokens.BranchPPR %></h1></div>
        <asp:Panel runat="server" ID="p_resquest">
            <div class="well">
                <asp:Label AssociatedControlID="ddl_branchs" runat="server" Text="<%$Resources:Tokens,Branch %>"
                    ID="labelReseller"></asp:Label>
                <div>
                    <asp:DropDownList runat="server" ID="ddl_branchs" Width="150px" ClientIDMode="Static"
                        ValidationGroup="search">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddl_branchs"
                        Text="<%$Resources:Tokens,Required %>" ErrorMessage="<%$Resources:Tokens,Required %>"
                        ValidationGroup="search"></asp:RequiredFieldValidator>
                </div>
            </div>
            <p>
                <asp:LinkButton runat="server" ID="b_addRequest"
               OnClientClick="hidval()"     CssClass="btn btn-success" OnClick="b_addRequest_Click" ValidationGroup="search"><i class="icon-search icon-only"></i>&nbsp;<%=Tokens.Search%></asp:LinkButton>
                <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hf_user" runat="server" />
                <asp:Label runat="server" ID="l_message"></asp:Label>
            </p>
        </asp:Panel>
    </div>
    <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <%= Tokens.Requests %></h3>
            <asp:GridView ID="gv_customers" runat="server" AutoGenerateColumns="False" ClientIDMode="Static"
                CssClass="table table-bordered table-condensed"
                OnDataBound="gv_customers_DataBound" Width="100%">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="gv_lNumber" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CustomerName" HeaderText="<%$Resources:Tokens,Customer %>" />
                    <asp:BoundField DataField="CustomerPhone" HeaderText="<%$Resources:Tokens,Phone %>" />
                    <asp:BoundField DataField="ServicePackageName" HeaderText="<%$Resources:Tokens,Package %>"/>
                    <asp:BoundField DataField="SPName" HeaderText="<%$Resources:Tokens,Provider %>" />
                    <asp:BoundField DataField="StatusName" HeaderText="<%$ Resources:Tokens,Status%>" />
                    <asp:BoundField DataField="GovernorateName" HeaderText="<%$Resources:Tokens,Governrate %>" />
                    <asp:BoundField DataField="Branch" HeaderText="<%$Resources:Tokens,Branch %>" />
                    <asp:BoundField DataField="Title" HeaderText="<%$Resources:Tokens,Offer %>"/>
                    <asp:BoundField DataField="RequestDate" HeaderText="<%$Resources:Tokens,Request.Date %>" />
                    <asp:BoundField DataField="Total" HeaderText="<%$Resources:Tokens,Total %>" />
                    <asp:BoundField DataField="Start" HeaderText="<%$Resources:Tokens,From %>" />
                    <asp:BoundField DataField="End" HeaderText="<%$Resources:Tokens,To %>" />
                    <asp:BoundField DataField="user" HeaderText="<%$Resources:Tokens,UserName %>" />
                    <asp:BoundField DataField="Password" HeaderText="<%$Resources:Tokens,Password %>" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <span>
                                <asp:Button ID="gv_bConfirm"  runat="server" ClientIDMode="Static" CommandArgument='<%# Bind("ID") %>'
                                    data-bind="Confirm" data-fruit='<%#Eval("Total")%>' CssClass="btn btn-primary btn-xs" Text="<%$Resources:Tokens,Confirm %>"
                                    ToolTip='<%# Bind("ID") %>' />&nbsp;&nbsp;
                                <asp:HiddenField ID="gv_hf_id" runat="server" Value='<%# Bind("ID") %>' />
                                <asp:Button CssClass="btn btn-danger btn-xs" ID="Button1" runat="server" CommandArgument='<%# Bind("ID") %>'
                                    Text="<%$Resources:Tokens,Reject %>" data-bind="Reject" ToolTip='<%# Bind("ID") %>' />
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                        <%= Tokens.NoResults %>
                </EmptyDataTemplate>
            </asp:GridView>
        </fieldset>
        <div>
            <asp:LinkButton runat="server" ID="Export" CssClass="btn btn-primary" OnClick="Export_OnClick" ><i class="icon-file"></i>&nbsp;<%=Tokens.Export %></asp:LinkButton></div>
    </div>
    <div id="confirm" class="bootbox modal fade " tabindex="-1" role="dialog" aria-hidden="true"
        style="margin-right: 200px;">
         <div class="modal-dialog ">
              <div class="modal-content">
                   <div class="modal-header approve-header">
            <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                ×</button>
                        <h2 id="H5"><%=Tokens.Approve %></h2>
        </div>
                  <div class="modal-body">
                                 <div class="bootbox-body">
        <div class="well">
        <table width="500">

                            <tr>
                    <td>
                        <label><%=Tokens.Box %></label>
                        <asp:DropDownList runat="server" ID="ddlBox" ClientIDMode="Static"/>
                        
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlBox"
                                                    Display="Dynamic" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="confirm"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            <tr>
                <td><label><%=Tokens.DiscoundFromBox %></label>
                    <asp:TextBox runat="server" ID="txtDiscoundBox" ClientIDMode="Static"></asp:TextBox></td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtDiscoundBox"
                                                Display="Dynamic" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="confirm"></asp:RequiredFieldValidator>
                </td>
            </tr>
                        <tr>
                <td colspan="2">
                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Tokens, ConfirmReason %>"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 125px">
                    <asp:TextBox ID="TextBox1" runat="server" Rows="3" TextMode="MultiLine" ValidationGroup="confirm"
                        Width="356px" />
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TextBox1"
                        Text="<%$Resources:Tokens,Required %>" ErrorMessage="<%$Resources:Tokens,Required %>"
                        ValidationGroup="confirm"></asp:RequiredFieldValidator>
                </td>
            </tr>
             </table>
             </div> </div> </div>
             <div class="modal-footer">
                <p align="center" colspan="2">
                    <asp:HiddenField ID="hf_confirm" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField runat="server" ID="hf_boxId" ClientIDMode="Static"/>
                    <asp:Button ID="b_confirm" runat="server" aria-disabled="false" ClientIDMode="Static"
                        CssClass="btn btn-success ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only ui-state-hover"
                        OnClick="b_confirm_Click" role="button" Text="<%$Resources:Tokens,Save %>" ValidationGroup="confirm"
                        Width="100px" UseSubmitBehavior="false" OnClientClick="plswait(this.id)" />
                    &nbsp;
                    <button id="cancel-dialog" type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="True">
                    <i class="icon-white icon-remove"></i>&nbsp;<%=Tokens.Cancel %></button>&nbsp;
                </p>
            </div>
       
 </div>
              </div>
    </div>
    <div id="dialog1"  class="bootbox modal fade reject" tabindex="-1" role="dialog" aria-hidden="true"
        style="margin-right: 200px;">
        <div class="modal-dialog">
              <div class="modal-content">
                   <div class="modal-header reject-header">
            <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                ×</button>
                        <h2><%=Tokens.Reject %></h2>
        </div>
                  <div class="modal-body">
                                 <div class="bootbox-body">
        <div class="well">
            <div>
        <table width="500">
            <tr>
                <td colspan="2">
                    <span class="label label-warning">
                    <h4>
                <%=Tokens.RejectReason %></h4></span> &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 125px">
                    <asp:TextBox ID="txt_RejectReason" runat="server" Rows="3" TextMode="MultiLine" Width="356px"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_RejectReason"
                        Display="Dynamic" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="x"></asp:RequiredFieldValidator>
                </td>
            </tr>
             </table>
                </div>
            </div>
    </div>
    </div>
            <div class="modal-footer">
                <p align="center" colspan="2">
                    <asp:HiddenField ID="hf_rejectionId" runat="server" ClientIDMode="Static" />
                    <asp:Button ID="btn_reject" runat="server" aria-disabled="false" ClientIDMode="Static"
                        CssClass="btn btn-danger ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only ui-state-hover"
                        OnClick="btn_reject_Click" role="button" Text="<%$Resources:Tokens,Save %>" ValidationGroup="x"
                        Width="100px" />
                    &nbsp;
                     <button id="cancel-dialog" type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="True">
                    <i class="icon-white icon-remove"></i>&nbsp;<%=Tokens.Cancel %></button>&nbsp;
                </p>
            </div>
       
 </div>
    </div>

    </div>
    </div>
 
    <script type="text/javascript">
        

        function plswait(id) {

            var type = document.getElementById('<%=ddlBox.ClientID%>').value;
             var amo = document.getElementById('<%=txtDiscoundBox.ClientID%>').value;

           
             var com = document.getElementById('<%=TextBox1.ClientID%>').value;



             if (type == "" || amo == "" || com == "") { return; }
             else {
                 var check2 = document.getElementById(id);
                 check2.disabled = 'true'; check2.value = 'Please wait...';
             }

         }




        $(document).ready(function () {
            // confirm
           <%-- var confirm = $('#confirm').dialog({
                autoOpen: false,
                width: 600,
                resizable: false,
                draggable: false,
                title: '<%= Tokens.ConfirmReason %>',
                modal: true,
                position: 'center'
            });
            confirm.parent().appendTo(jQuery("form:first"));--%>


            $('#showdialog').click(function () {
                var x = $(this).attr("Title");
                $('#hf_confirm').val(x);
                //$('#confirm').dialog('open');
                $("div[id$='confirm']").modal('show');
                return false;
            });


            $('input[data-bind="Confirm"]').click(function () {
                var x = $(this).attr("Title");
                $('#hf_confirm').val(x);
                var v = $(this).attr('data-fruit');
                $('#txtDiscoundBox').val(v);
                //$('#confirm').dialog('open');
                $("div[id$='confirm']").modal('show');
                return false;
            });




          <%--  var dlg = $('#dialog1').dialog({
                autoOpen: false,
                width: 600,

                resizable: false,
                draggable: false,
                title: '<%= Tokens.RejectReason %>',
                modal: true,
                position: 'center'
            });
            dlg.parent().appendTo(jQuery("form:first"));--%>

            $('input[data-bind="Reject"]').click(function () {
                var x = $(this).attr("Title");
                $('#hf_rejectionId').val(x);
                //$('#dialog1').dialog('open');
                $("div[id$='dialog1']").modal('show');
                return false;
            });



            $('#ddl_branchs').change(function () {
                //                var selection = $('#ddl_branchs').val();
                //                $('#HiddenField1').val(selection);
                //                $('#ddl_branchs').attr('disabled', 'disabled');
            });
            $('#ddlBox').change(function () {
                var s = $('#ddlBox').val();
                $('#hf_boxId').val(s);
            });

            //pagination
            $("#gv_customers").each(function () {
                var currentPage = 0;
                var numPerPage = 250;
                var $table = $(this);
                $table.bind('repaginate', function () {
                    $table.find('tbody tr').hide().slice(currentPage * numPerPage, (currentPage + 1) * numPerPage).show();
                });
                $table.trigger('repaginate');
                var numRows = $table.find('tbody tr').length;
                var numPages = Math.ceil(numRows / numPerPage);
                var $pager = $("<ul class='pagination'></ul>");
                for (var page = 0; page < numPages; page++) {
                    var num = page + 1;
                    $("<li></li>").html("<a>" + num + "</a>").bind('click', {
                            newPage: page
                        }, function (event) {
                            currentPage = event.data['newPage'];
                            $table.trigger('repaginate');
                            $(this).addClass('active').siblings().removeClass('active');
                        }).appendTo($pager); //.addClass('clickable');
                }
                $pager.insertBefore($table).find('span.page-number:first').addClass('active');
            });
        });

        function hidval() {
            var selection = $('#ddl_branchs').val();
            $('#HiddenField1').val(selection);
            $('#ddl_branchs').attr('disabled', 'disabled');
        }
    </script>
</asp:Content>
