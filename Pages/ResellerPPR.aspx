<%@ Page Title="<%$Resources:Tokens,ResellerPPR%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ResellerPPR.aspx.cs" Inherits="NewIspNL.Pages.ResellerPPR" %>





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
<link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>
      <div runat="server" id="Div1" >
    </div>
    <asp:MultiView runat="server" ID="mv_container">
        <asp:View runat="server" ID="v_search">
            <div class="view">
                <fieldset>
                    <div class="page-header">
                        <h1>
                            <%=Tokens.Request %></h1>
                    </div>
                    <asp:Panel runat="server" ID="p_resquest">
                        <div class="well">
                            <div style="padding-bottom: 5px;">
                                <asp:Label runat="server" Text="<%$Resources:Tokens,Reseller %>" ID="labelReseller"></asp:Label></div>
                            <div>
                                <asp:DropDownList runat="server" CssClass="width-50 chosen-select" ID="ddl_reseller" Width="150px" ClientIDMode="Static">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddl_reseller"
                                    ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                            </div>
                            <p>
                                <br />
                                <asp:Button runat="server" ID="b_addRequest" Text="<%$Resources:Tokens,Search %>"
                                    OnClientClick="hidval()" OnClick="b_addRequest_Click" CssClass="btn btn-success" />
                                <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
                                <asp:HiddenField ID="hf_user" runat="server" />
                            </p>
                        </div>
                    </asp:Panel>
                </fieldset>
            </div>
        </asp:View>
        <asp:View runat="server" ID="v_results">
            <div class="view">
                <fieldset>
                    <div class="page-header">
                        <h1>
                            <%=Tokens.Requests %></h1>
                    </div>
                    <asp:Panel runat="server" ID="p_rDetails">
                        <div style="padding: 5px;" id="container">
                            <p>
                                <asp:Button runat="server" ID="b_changeReseller" Text="<%$Resources:Tokens,ChangeReseller %>"
                                    OnClick="b_changeReseller_Click" CssClass="btn btn-primary" /></p>
                            <br />
                            <asp:GridView runat="server" ID="gv_customers" AutoGenerateColumns="False" ForeColor="Black"
                                GridLines="Horizontal" CssClass="table table-bordered table-condensed text-center"
                                OnDataBound="gv_customers_DataBound" Width="100%" ClientIDMode="Static">
                                <Columns>
                                    <asp:TemplateField HeaderText="#">
                                        <ItemTemplate>
                                            <asp:Label ID="gv_lNumber" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CustomerName" HeaderText="<%$Resources:Tokens,Customer %>" />
                                    <asp:BoundField DataField="CustomerPhone" HeaderText="<%$Resources:Tokens,Phone %>" />
                                    <asp:BoundField DataField="RequestDate" HeaderText="<%$Resources:Tokens,RequestDate%>" />
                                    <asp:BoundField DataField="ServicePackageName" HeaderText="<%$Resources:Tokens,Package %>" />
                                    <asp:BoundField DataField="SPName" HeaderText="<%$Resources:Tokens,Provider %>" />
                                    <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,Reseller %>" />
                                    <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Tokens,Branch %>" />
                                    <asp:BoundField DataField="Title" HeaderText="<%$Resources:Tokens,Offer %>" />
                                    <asp:BoundField DataField="StatusName" HeaderText="<%$ Resources:Tokens,Status%>" />
                                    <asp:BoundField DataField="GovernorateName" HeaderText="<%$Resources:Tokens,Governorate %>" />
                                    <asp:BoundField DataField="Total" HeaderText="<%$Resources:Tokens,Total %>" />
                                    <asp:BoundField DataField="Start" HeaderText="<%$Resources:Tokens,From %>" />
                                    <asp:BoundField DataField="End" HeaderText="<%$Resources:Tokens,To %>" />
                                    <asp:BoundField DataField="user" HeaderText="<%$Resources:Tokens,UserName %>" />
                                    <asp:BoundField DataField="Password" HeaderText="<%$Resources:Tokens,Password %>" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                           
                                            <button id="gv_bConfirm" type="button" class="btn btn-success btn-xs" title="<%=Tokens.Confirm %>"
                                                onclick="acceptbtn(<%#Eval("ID")%>,<%#Eval("Total")%>)">
                                                <%=Tokens.Confirm%></button>
                                            &nbsp;
                                            <asp:HiddenField ID="gv_hf_id" runat="server" Value='<%# Bind("ID") %>' />
                                            <button id="Button1" type="button" class="btn btn-danger btn-xs" title="<%=Tokens.Reject %>"
                                                onclick="rejectbtn(<%#Eval("ID")%>)">
                                                <%=Tokens.Reject %></button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <%=Tokens.NoResults %>
                                </EmptyDataTemplate>
                            </asp:GridView>
                            <div>
                            </div>
                            <div style="text-align: center">
                                <asp:Label runat="server" ID="l_message"></asp:Label></div>
                        </div>
                    </asp:Panel>
                </fieldset>
            </div>
        </asp:View>
    </asp:MultiView>
    <div  aria-labelledby="myModalLabel"  id="newrej" class="bootbox modal fade" tabindex="-1" role="dialog" aria-hidden="true"
        style="margin-right: 200px;">
         <div class="modal-dialog">
              <div class="modal-content">
                   <div class="modal-header reject-header">
            <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                ×</button>
            <h4 id="H5">
                <%=Tokens.Reject %></h4>
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
             <div class="modal-footer">
                <p align="center" colspan="2">
                    <asp:HiddenField ID="hf_rejectionId" runat="server" ClientIDMode="Static" />
                    <asp:Button aria-disabled="false" role="button" ID="btn_reject" runat="server" OnClick="btn_reject_Click"
                        Text="<%$Resources:Tokens,Save%>" Width="100px" ClientIDMode="Static" CssClass="btn btn-danger"
                        ValidationGroup="x" />
                    &nbsp;
                     <button id="cancel-dialog" type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="True">
                    <i class="icon-white icon-remove"></i>&nbsp;<%=Tokens.Cancel %></button>&nbsp;
                </p>
          </div>
       
        </div>
    </div>
              </div>
    </div>
    <div id="newaccept" class="bootbox modal fade" tabindex="-1" role="dialog" aria-hidden="true"
        style="margin-right: 200px;">
         <div class="modal-dialog">
              <div class="modal-content">
                    <div class="modal-header approve-header">
            <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                ×</button>
            <h4 id="H5">
                <%=Tokens.Approve %></h4>
        </div>
                  <div class="modal-body">
                                 <div class="bootbox-body">
        <div class="well">
          <div   >
        <table width="500">
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                        <%=Tokens.Box %></label>
                    <asp:DropDownList runat="server" ID="ddlBox" ClientIDMode="Static" />
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlBox"
                        Display="Dynamic" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="y"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                        <%=Tokens.DiscoundFromBox %></label>
                    <asp:TextBox runat="server" ID="txtDiscoundBox" ClientIDMode="Static"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtDiscoundBox"
                        Display="Dynamic" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="y"></asp:RequiredFieldValidator>
                </td>
            </tr>
          
            <tr>
                <td style="width: 125px">
                    <label>
                        <%=Tokens.Comment %></label>
                    <asp:TextBox ID="txtApprovComment" runat="server" Rows="3" TextMode="MultiLine" Width="356px"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="hf_Approved" runat="server" ControlToValidate="txtApprovComment"
                        Display="Dynamic" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="y"></asp:RequiredFieldValidator>
                </td>
            </tr>
            </table>
            </div>
            </div>
                                     </div></div>
                 <div class="modal-footer">
                <p align="center" colspan="2">
                    <asp:HiddenField ID="hf_ApprovedId" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField runat="server" ID="hf_boxId" ClientIDMode="Static" />
                    <asp:Button aria-disabled="false" role="button" UseSubmitBehavior="false"  ID="Button2" runat="server" OnClick="gv_bConfirm_Click"
                        Text="<%$Resources:Tokens,Save%>" Width="100px" ClientIDMode="Static" CssClass="btn btn-info" OnClientClick="save()"
                        ValidationGroup="y" />
                    &nbsp;
                    <button id="cancel-dialog" type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="True">
                    <i class="icon-white icon-remove"></i>&nbsp;<%=Tokens.Cancel %></button>&nbsp;
                </p>
            </div>
        
        </div>
                  

    </div>
        </div>
    
   
       
  <script type="text/javascript">


      $(document).ready(function () {

          $(".chosen-select").chosen();
         

          $("#gv_customers").each(function () {
              var currentPage = 0;
              var numPerPage = 250;
              var $table = $(this);
              $table.bind("repaginate", function () {
                  $table.find("tbody tr").hide().slice(currentPage * numPerPage, (currentPage + 1) * numPerPage).show();
              });
              $table.trigger("repaginate");
              var numRows = $table.find("tbody tr").length;
              var numPages = Math.ceil(numRows / numPerPage);
              var $pager = $("<ul class='pagination'></ul>");
              for (var page = 0; page < numPages; page++) {
                  var num = page + 1;
                  $("<li></li>").html("<a>" + num + "</a>").bind("click", {
                      newPage: page
                  }, function (event) {
                      currentPage = event.data["newPage"];
                      $table.trigger("repaginate");
                      $(this).addClass("active").iblings().removeClass("active");
                  }).appendTo($pager); 
              }
              $pager.insertBefore($table).find("span.page-number:first").addClass("active");
          });
      });
      var $ = jQuery.noConflict();
      function hidval() {
          var selection = $("#ddl_reseller").val();
          $("#HiddenField1").val(selection);
          $("#ddl_reseller").attr("disabled", "disabled");
      };

      function rejectbtn(x) {

          $("#hf_rejectionId").val(x);


          $("div[id$='newrej']").modal('show');

      };

      function acceptbtn(a, v) {

          $("#hf_ApprovedId").val(a);
          $("#txtDiscoundBox").val(v);

          $("#newaccept").modal('show');

      };
      function save() {

          var s = $("#ddlBox").val();
          $("#hf_boxId").val(s);
          

          var amount = document.getElementById('<%=txtDiscoundBox.ClientID%>').value;
          var res = document.getElementById('<%=ddlBox.ClientID%>').value;
          var box = document.getElementById('<%=txtApprovComment.ClientID%>').value;
         

          if (amount == "" || res == "" || box == "" ) {
              return;
          } else {
              var check2 = document.getElementById('<%=Button2.ClientID%>');
                check2.disabled = 'true'; check2.value = 'Please wait...';
            }

      };



    </script>
   
</asp:Content>
