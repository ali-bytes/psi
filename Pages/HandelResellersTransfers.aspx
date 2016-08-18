<%@ Page Title="<%$Resources:Tokens,HandelResellersTransfers %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="HandelResellersTransfers.aspx.cs" Inherits="NewIspNL.Pages.HandelResellersTransfers" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>

    <div class="page-header">
        <h1>
            <asp:Literal runat="server" ID="lbltitle" Text="<%$Resources:Tokens,HandelResellersTransfers %>"></asp:Literal></h1>
    </div>
    <div id="message" runat="server">
    </div>
    <div class="row">
        <div class="col-xs-12">
            <div class="well">
            <div style="padding-bottom: 5px;">
                <asp:Label runat="server" Text="<%$Resources:Tokens,Reseller %>" ID="labelReseller"></asp:Label></div>
            <div>
                <asp:DropDownList runat="server" CssClass="width-60 chosen-select" ID="ddlReseller" Width="150px" ClientIDMode="Static">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlReseller"
                                            ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="se"></asp:RequiredFieldValidator>
            </div>
            <p><br/>
                <asp:LinkButton runat="server" ID="btnSearch" CssClass="btn btn-success" ValidationGroup="se"
                            OnClick="Search_Click"><i class="icon-search"></i>&nbsp;<asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,Search %>"></asp:Literal></asp:LinkButton>
            </p>
            </div>
            <h3 class="header smaller lighter blue"><asp:Literal runat="server" Text="<%$Resources:Tokens,Results %>"></asp:Literal></h3>
            <asp:GridView runat="server" ID="gvRequests" AutoGenerateColumns="False"
                              ForeColor="Black" GridLines="Horizontal" CssClass="table table-bordered table-condensed text-center"
                              OnDataBound="gv_customers_DataBound" Width="100%"
                              ClientIDMode="Static" >
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="gv_lNumber" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,Reseller %>" />
                        <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Amount %>" />
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,TransferFrom%>">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblFrom" Text='<%#Eval("TransferFrom") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,TransferTo%>">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblTo" Text='<%#Eval("TransferTo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,UserName %>" />
                        <asp:BoundField DataField="RequestDate" DataFormatString="{0:dd-MM-yyyy}" HeaderText="<%$Resources:Tokens,RequestDate%>" />
                        <asp:TemplateField>
                            <ItemTemplate>
                               <%-- <asp:LinkButton ID="gv_bConfirm" runat="server" CssClass="btn btn-success btn-sm"
                                            CommandArgument='<%# Bind("Id") %>' OnClick="Confirm_Click" data-rel="tooltip" ToolTip="<%$Resources:Tokens,Confirm %>"><i class="icon-only icon-ok"></i></asp:LinkButton>
                                     --%>           
                                 <asp:Button ID="Button2" runat="server" CssClass="btn btn-success btn-sm" Text="<%$Resources:Tokens,Confirm %>"
                                            CommandArgument='<%# Bind("Id") %>' OnClick="Confirm_Click" UseSubmitBehavior="false"  OnClientClick="save()" />
                                
                                
                                
                                <button id="Button1" type="button" class="btn btn-danger btn-xs" title="<%=Tokens.Reject %>"
                                                onclick="rejectbtn(<%#Eval("Id")%>)">
                                                <%=Tokens.Reject %></button>
                                
                                

   <%--                             <asp:LinkButton ID="Button1" runat="server" data-val="Reject" OnClick="Reject_Click"
                                            CommandArgument='<%# Bind("Id") %>' CssClass="btn btn-danger btn-sm" data-rel="tooltip" ToolTip="<%$Resources:Tokens,Reject %>"><i class="icon-only icon-remove"></i></asp:LinkButton>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <%=Tokens.NoResults %>
                    </EmptyDataTemplate>
                            
                </asp:GridView>
        </div>
    </div>
       <div  aria-labelledby="myModalLabel"  id="newrej" class="bootbox modal fade" tabindex="-1" role="dialog" aria-hidden="true"
        style="margin-right: 200px;">
        <div  style="margin-right: 300px; margin-top: 200px; background-color: snow; width: 400px" >
        <table width="500">
            <tr>
                <td colspan="2">
                    &nbsp;
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
            <tr>
                <td align="center" colspan="2">
                    <asp:HiddenField ID="hf_rejectionId" runat="server" ClientIDMode="Static" />
                    <asp:Button aria-disabled="false" role="button" ID="btn_reject" runat="server" OnClick="Reject_Click"
                        Text="<%$Resources:Tokens,Save%>" Width="100px" ClientIDMode="Static" CssClass="btn btn-danger"
                        ValidationGroup="x" />
                    &nbsp;
                </td>
            </tr>
        </table>
        </div>
    </div>
<script type="text/javascript">
    function save() {

        var table = document.getElementById('<%=gvRequests.ClientID%>');

          if (table.rows.length > 0) {
              //loop the gridview table
              for (var i = 1; i < table.rows.length; i++) {
                  //get all the input elements
                  var inputs = table.rows[i].getElementsByTagName("input");
                  for (var j = 0; j < inputs.length; j++) {
                      //get the textbox1
                      if (inputs[j].id.indexOf("Button2") > -1) {
                          $(inputs[j]).attr('disabled', 'disabled');

                      }

                  }

              }
          }

               

      }
    function rejectbtn(x) {

        $("#hf_rejectionId").val(x);


        $("div[id$='newrej']").modal('show');

    };



    $(document).ready(function () {
        $(".chosen-select").chosen();
    });
</script>

</asp:Content>
