<%@ Page Title="<%$Resources:Tokens,UnpaidResellerDemands%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ResellerUnpaidDemands.aspx.cs" Inherits="NewIspNL.Pages.ResellerUnpaidDemands" %>

<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
<link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>
      <link rel="stylesheet" type="text/css" href="http://cdn.datatables.net/1.10.9/css/jquery.dataTables.min.css" />
     <link rel="stylesheet" type="text/css" href="http://cdn.datatables.net/tabletools/2.2.4/css/dataTables.tableTools.css" />
   
    <div class="view" id="searchPanel" runat="server">
        <fieldset>
            <div class="page-header"><h1>
                <%= (Tokens.Search + " " + Tokens.UnpaidResellerDemands)%></h1></div>
            <div class="row-fluid" runat="server" ID="Forsearch">
                <div class="col-sm-8">
                    <div class="well">
                        <div>
                            <label for="DdlReseller">
                                <asp:Literal Text="<%$Resources:Tokens,Reseller %>" runat="server" />
                            </label>
                            <div>
                                <asp:DropDownList ID="DdlReseller" CssClass="width-60 chosen-select" runat="server" EnableViewState="True" DataTextField="UserName"
                                          DataValueField="ID" />
                                <asp:RequiredFieldValidator ID="rrrr" ErrorMessage="*" ControlToValidate="DdlReseller"
                                                            runat="server" />
                            </div>
                            <br/>
                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Tokens,MenuServiceProviders %>"></asp:Label>
                            <div>
                                                                <asp:CheckBoxList ClientIDMode="Static" runat="server" ID="providerlist" 
                                                                    RepeatDirection="Horizontal">
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                            <div class="well" style="height: 178px">
                                <div>
                                    <label for="DdlYear">
                                        <%=Tokens.Year %>
                                    </label>
                                    <div>
                                        <asp:DropDownList runat="server" ID="DdlYear">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div>
                                    <label for="DdlMonth">
                                        <%=Tokens.Month %></label>
                                    <div>
                                        <asp:DropDownList runat="server" ID="DdlMonth">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                </div>
                    <div class="row-fluid">
                        

            </div>
            <p>
                <asp:Button ID="BSearch" Text="<%$Resources:Tokens,Search %>" CssClass="btn btn-success" runat="server" ClientIDMode="Static"
                            OnClick="SearchDemands" />&nbsp;<span runat="server" ID="Msg"></span>
            </p>
           
        </fieldset>
    </div>
    <div class="view" id="resultPanel" runat="server">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,Results %>" runat="server" /></h3>
           
            <div id="all-result-items" style="overflow-y: auto; width: 100%">
                <asp:GridView CssClass="table table-bordered table-condensed text-center" ClientIDMode="Static"
                         Wrap="true" style="margin-right: 0px;"    runat="server" ID="GvResults" AutoGenerateColumns="False" OnDataBound="NumberGrid" OnRowDataBound="GvResults_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LNo" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Customer %>" DataField="Customer" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Phone %>" DataField="Phone" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Status %>" DataField="Status" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Service.Package %>" DataField="servicepack" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Provider %>" DataField="Provider" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Reseller %>" DataField="Reseller" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Central %>" DataField="Central" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Governorate %>" DataField="Governorate" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Offer %>" DataField="Offer" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Paid %>" DataField="TPaid" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,From %>" DataField="TStartAt" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,To %>" DataField="TEndAt" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Notes %>" DataField="Notes" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Amount %>" DataField="TAmount" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,ResellerDiscount %>" DataField="TResellerDiscount" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Net %>" DataField="TResellerNet" />
                        <asp:TemplateField HeaderText="<%$ Resources:Tokens,Comment%>">
                        <ItemTemplate>
                            <button type="button" id="lnb_Comment" clientidmode="Static" data-select="comment" data-id='<%#Eval("Id") %>' 
                                class="btn btn-info btn-sm"  data-rel="tooltip" ><span class="icon-comment icon-only bigger-110"></span></button>
                           
                             <asp:LinkButton ID="lnb_AllComment" runat="server" OnClientClick="hist(this); return false;" CommandArgument='<%#Eval("Id") %>' clientidmode="Static" data-select="allcomment" data-id='<%#Eval("Id") %>' 
                                class="btn btn-success btn-sm"  data-rel="tooltip" title="<%$Resources:Tokens,AllComment %>"><span class="icon-th-list icon-only bigger-110"></span></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                       
                    </Columns>
                </asp:GridView>
            </div>
            <div>
                <div class="span3">
                    <asp:GridView ShowHeader="False" runat="server" ID="GvReport"  CssClass="table table-bordered table-responsive"/>
                </div>
                <div class="span9">
                    <asp:Button Text="<%$Resources:Tokens,ExportandFinishInvoice %>" ID="btnCreatInvoice" CssClass="btn btn-primary" runat="server" OnClick="CreateInvoice"/>&nbsp;
                    <button class="btn btn-success"  type="button" runat="server" OnServerClick="SearchAgain"><%=Tokens.BackToSearch %></button>&nbsp;
<%--                    <asp:Button runat="server" OnClick="btnExport_OnClick" CssClass="btn btn-danger" Text="<%$Resources:Tokens,Export %>" ID="btnExportToExcel"/>--%>
                </div>
            </div>
           
        </fieldset>
    </div>
     <div id="commentModal" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel4"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal">
                        ×</button>
                    <h4 id="myModalLabel4">
                        <%=Tokens.Comment %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <asp:HiddenField runat="server" ID="demmandId" ClientIDMode="Static" />
                        <div class="well">
                            <label for="txtComment">
                            </label>
                            <div>
                                <asp:TextBox runat="server" ID="txtComment" ValidationGroup="rr" ClientIDMode="Static"
                                    TextMode="MultiLine" />
                                <asp:RequiredFieldValidator ValidationGroup="rr" ErrorMessage="*" ControlToValidate="txtComment"
                                    ID="RR" runat="server" />
                                <br/>
                                <asp:RegularExpressionValidator runat="server" ID="valInput"
                                    ControlToValidate="txtComment" ValidationGroup="rr"
                                    ValidationExpression="^[\s\S]{0,200}$"
                                    ErrorMessage="Please enter a maximum of 200 characters"
                                    Display="Dynamic">Maximum of 200 characters</asp:RegularExpressionValidator>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div id="reject-results"></div>
                    <button class="btn btn-primary" validationgroup="rr" type="button" causesvalidation="True"
                        runat="server" onserverclick="AddComment">
                        <%=Tokens.AddComment %></button>
                    <button class="btn btn-danger" type="button" data-dismiss="modal" aria-hidden="true">
                        <%=Tokens.Cancel %></button>
                </div>
            </div>
        </div>
    </div>
      <div id="historyModal" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true" style="width: 100%">
                                                    <div class="modal-dialog">
            <div class="modal-content">
                <asp:HiddenField ID="count" runat="server" ClientIDMode="Static" />
                <div class="modal-header">
            <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                ×</button>
            <h4 id="myModalLabel">
                History</h4>
                     <div id="DivSuccess" runat="server"></div>
            <div id="Div1" runat="server"></div>
            <div id="DivCount" runat="server"></div>
        </div>
                <div class="modal-body">
                                 <div class="bootbox-body">
                                     <table style="border: 1px solid gray" id="tbHistory" class="table table-bordered table-condensed text-center">
                                         <thead><tr>
                                         <th>user</th>
                                         <th>comment</th>
                                         <th>date</th>
                                         </tr>
                                         </thead>
                                         <tbody></tbody>
                                     </table>
                <%--<asp:GridView ID="grdHistory" runat="server" AutoGenerateColumns="False" ClientIDMode="Static"
                    OnDataBound="grdHistory_DataBound" DataKeyNames="ID" CssClass="table table-bordered table-condensed">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="gv_l_number" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="User" HeaderText="<%$ Resources:Tokens,User %>" />
                        <asp:BoundField DataField="Comment" HeaderText="<%$ Resources:Tokens,Phone %>" />
                        <asp:BoundField DataField="Date" DataFormatString="{0:d}" HeaderText="<%$ Resources:Tokens,Date %>" />
                    </Columns>
                    <EmptyDataTemplate>

                            <asp:Literal Text="<%$ Resources:Tokens,NoResults %>" runat="server" />
                    </EmptyDataTemplate>
                </asp:GridView>--%>
                </div></div>
                <div class="modal-footer">
            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="True">Close</button>
        </div>
                </div></div>
            </div>
    <asp:HiddenField runat="server" ID="HfSerched" />
   <script type="text/javascript" src="http://cdn.datatables.net/1.10.9/js/jquery.dataTables.min.js"></script>
     <script type="text/javascript" src="http://cdn.datatables.net/tabletools/2.2.4/js/dataTables.tableTools.min.js"></script>
   
    <script type="text/javascript">
        
        $(function () {
           
            var isSearched = $("#HfSerched").val();
            var searchBtn = $("#searchPanel");
            var resultsPanel = $("#resultPanel");
            if (isSearched === "1") {
                $(searchBtn).hide();
                $(resultsPanel).show();
            } else {
                $(searchBtn).show();
                $(resultsPanel).hide();
            }

            if ($("#GvResults").width() > 950) {
                $("#all-result-items").css({
                    "overflow-y": "auto"
                });
                $("#GvResults").css({
               
                    "font-size": "12px"
                });
            }
        });
       
    </script>
    <style type="text/css">
        #providerlist tbody tr td {
            padding: 5px 20px;
        }
    </style>
    
    <script type="text/javascript">
        $(document).ready(function() {
            $(".chosen-select").chosen();
            $("tr td input[type=checkbox]").addClass("ace");
            $("tr td label").addClass("lbl").css({ 'padding': '7px' });

            $('button[data-select="comment"]').click(function() {
                var id = $(this).attr("data-id");
                $('#demmandId').val(id);
                $('#commentModal').modal('show');
            });


//$('LinkButton[data-select="allcomment"]').click(function () {
            //    var id = $(this).attr("data-id");
            //    GetHistory(parseInt(id));
            //});

            window.hist = function(it) {
                var id = $(it).attr("data-id");
                GetHistory(parseInt(id));
                return false;
            }

            function GetHistory(id) {
                $.ajax({
                    type: "POST",
                    url: "ResellerUnpaidDemands.aspx/GetHistory",
                    data: '{id: ' + id + '}',
                    contentType: "application/json",
                    dataType: "json",
                    success: function(response) {
                        var d = JSON.parse(response.d);
                        var table = $('#tbHistory tbody');
                        table.empty();
                        $.each(d, function(i, v) {
                            var date = new Date(parseInt(v.Date.slice(6, v.Date.indexOf(")"))));
                            table.append('<tr><td>' + v.User + '</td><td>'
                                + v.Comment + '</td><td>' + date.toLocaleString()
                                + '</td></tr>');
                        });

                        $('#historyModal').modal('show');
                        return false;
                    },
                    error: function(response) {
                        alert("error");
                    }
                });
            }


        });
        $(document).ready(function () {
            $('table').each(function () {
                $(this).prepend('<thead></thead>');
                $(this).find('thead').append($(this).find("tr:eq(0)"));
            });
            var table = $('#GvResults').dataTable({
                "iDisplayLength": 50,
                "aLengthMenu": [[10, 25, 50, 100, 250, 500, -1], [10, 25, 50, 100, 250, 500, "All"]]
            });
            var tableTools = new $.fn.dataTable.TableTools(table, {
                'aButtons': [
                    {
                        'sExtends': 'xls',
                        'sButtonText': 'Save to Excel',
                        'sFileName': 'ResellerInvoices.xls'
                    },
                    {
                        'sExtends': 'print',
                        'bShowAll': true
                    },
                    'copy'
                ],
                'sSwfPath': '//cdn.datatables.net/tabletools/2.2.4/swf/copy_csv_xls_pdf.swf'
            });
            $(tableTools.fnContainer()).insertBefore('#GvResults_wrapper');
        });
    </script>
     
</asp:Content>

