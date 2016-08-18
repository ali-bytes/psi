<%@ Page Title="<%$Resources:Tokens,BranchPR%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="BranchPR.aspx.cs" EnableEventValidation="false" Inherits="NewIspNL.Pages.BranchPR" %>
<%@ Import Namespace="NewIspNL.Helpers" %>
<%@ Import Namespace="Resources" %>
 

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div runat="server" id="Div1" >
    </div>
    <div runat="server" id="portalRequest" >
    </div>
       <div class="row">
        <div class="view">
            <asp:Panel runat="server" ID="p_resquest">
                <div class="page-header"><h1><asp:Literal runat="server" Text="<%$Resources:Tokens,Requests %>"></asp:Literal></h1></div>
                <div class="well">
                    <div>
                        <asp:Label AssociatedControlID="ddl_branches" runat="server" Text="<%$Resources:Tokens,Branch %>"
                            ID="labelReseller"></asp:Label>
                        <div>
                            <asp:DropDownList runat="server" ID="ddl_branches" Width="150px" ClientIDMode="Static"
                                ValidationGroup="search">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddl_branches"
                                ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="search"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <p>
                    <asp:Button runat="server" CssClass="btn btn-success" ID="b_addRequest" Text="<%$Resources:Tokens,Search %>"
                        OnClick="b_addRequest_Click" ValidationGroup="search" Width="100px"/>
                    <div><asp:Label ID="l_message" runat="server"></asp:Label></div>
               
                </p>
            </asp:Panel>
        </div>
        <div class="view">
            <asp:Panel runat="server" ID="p_rDetails">
                <h3 class="header smaller lighter blue"><asp:Literal runat="server" Text="<%$Resources:Tokens,RequestDetails %>"></asp:Literal></h3>
                <asp:HiddenField runat="server" ID="resellerId" />
                <div class="alert alert-info">
                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Tokens,selectedInvoice %>"></asp:Label> : <asp:Label  runat="server" ID="lblTotalChecked" ClientIDMode="Static"></asp:Label></div>
                <asp:GridView runat="server" ID="gv_customers" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed"
                    OnDataBound="gv_customers_DataBound" ClientIDMode="Static">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="gv_lNumber" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" HeaderText="<%$ Resources:Tokens,Customer %>" />
                        <asp:BoundField DataField="Phone" HeaderText="<%$ Resources:Tokens,Phone%>" />
                        <asp:BoundField DataField="Governorate" HeaderText="<%$ Resources:Tokens,Governrate %>" />
                        <asp:BoundField DataField="Status" HeaderText="<%$ Resources:Tokens,Status%>" />
                        <asp:BoundField DataField="Package" HeaderText="<%$Resources:Tokens,Package %>"/>
                        <asp:BoundField DataField="Provider" HeaderText="<%$ Resources:Tokens,Provider %>" />
                        <%--<asp:BoundField DataField="Reseller" HeaderText="<%$ Resources:Tokens,Reseller %>" />--%>
                        <asp:BoundField DataField="Central" HeaderText="<%$ Resources:Tokens,Central%>" />
                        <asp:BoundField DataField="Offer" HeaderText="<%$ Resources:Tokens,Offer %>" />
                        <asp:BoundField DataField="TStart" HeaderText="<%$ Resources:Tokens,From %>" />
                        <asp:BoundField DataField="TEnd" HeaderText="<%$ Resources:Tokens,To%>" />
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,Due %>">
                            <ItemTemplate>
                                <asp:Label ID="gv_lDue" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                                    <ItemTemplate>
                                       <%-- <% if(WidthOption.WidthOfReciept == false){ %> <a target="_blank" href='<%#string.Format("DemandReciept.aspx?d={0}",QueryStringSecurity.Encrypt(Eval("Id").ToString())) %>' class="btn btn-success btn-xs" title='<%=Tokens.Print %>' data-rel="tooltip">
                                                                                                 <i class="icon-print bigger-120"></i></a><% } else{ %>
                                                                                                     <a target="_blank" href='<%#string.Format("smallDemandReciept.aspx?d={0}",QueryStringSecurity.Encrypt(Eval("Id").ToString())) %>' class="btn btn-success btn-xs" title='<%=Tokens.Print %>' data-rel="tooltip">
                                                                                                       <i class="icon-print bigger-120"></i></a>
                                        <% } %> --%>
                                         <% if(WidthOption.WidthOfReciept == false){ %>  <asp:LinkButton ID="LinkBtnEdit" runat="server" CssClass="btn btn-primary btn-xs" CommandArgument='<%# Bind("Id") %>' OnCommand="LinkBtnDemandReciept_Command"  OnClientClick="var originalTarget = document.forms[0].target; document.forms[0].target = '_blank'; setTimeout(function () { document.forms[0].target = originalTarget; }, 500);" ><i class="icon-print icon-only bigger-120"></i></asp:LinkButton><% } else{ %>
                                                                                                     <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-primary btn-xs" CommandArgument='<%# Bind("Id") %>' OnCommand="LinkBtnsmallDemandReciept_Command"  OnClientClick="var originalTarget = document.forms[0].target; document.forms[0].target = '_blank'; setTimeout(function () { document.forms[0].target = originalTarget; }, 500);" ><i class="icon-print icon-only bigger-120"></i></asp:LinkButton>
                                        <% } %> 
                                    </ItemTemplate>
                                </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,Request %>">
                            <HeaderTemplate>
                                <%--<asp:CheckBox ID="gv_checkAll" runat="server" AutoPostBack="True" OnCheckedChanged="gv_checkAll_CheckedChanged" />--%>
                                <input type="checkbox" id="allselector" name="name"  />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div style="text-align: center">
                                    <asp:CheckBox ID="gv_cbRequested" runat="server" data-check="item"/>
                                    <asp:HiddenField ID="gv_hfId" runat="server" Value='<%# Bind("ID") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div>
                    <asp:Button CssClass="btn btn-success" runat="server" ID="Export" Text="<%$Resources:Tokens,Export %>"
                        OnClick="Export_OnClick" Width="100px"/></div><br/>
                <div class="well">
                    <table class="table table-bordered table-condensed" style="width: 26%;margin: 0 37%;">
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="Label2" Text="<%$Resources:Tokens,CustomersTotalDue %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="l_customersDue" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="Label1" Text="<%$Resources:Tokens,BranchCredit %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="L_branchCredit"></asp:Label>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td>
                                <asp:Label runat="server" ID="Label3" Text="<%$Resources:Tokens,Selections %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="L_seleceted" Text=""></asp:Label>
                            </td>
                        </tr>
                                                <tr>
                            <td>
                                <asp:Label runat="server" ID="lblavia" Text="<%$Resources:Tokens,AvailableCredit %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblavailableCredit" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <p>
                    <asp:Button runat="server" CssClass="btn btn-primary" ID="b_save" Text="<%$Resources:Tokens,Save %>" Width="100px"
                        OnClick="b_save_Click" />
                </p>
            </asp:Panel>
        </div>
        <div>
            &nbsp;<asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#ddl_branches').change(function () {
                var selection = $('#ddl_branches').val();
                $('#HiddenField1').attr("value", selection);
            });
        });
    </script>
        <script type="text/javascript">
           

            $(document).ready(function () {


                var sum =0;
                var checkCheckBox = "table[id*=gv_customers] input[id*='gv_cbRequested']:checkbox";
                $(checkCheckBox).click(function () {//checkmyCheckBox
                    if ($(this).is(':checked')) {
                        sum = sum + parseFloat($(this).parents("tr").children("td:eq(11)").text());
                        $(this).parents("tr").addClass('highlightRow');
                    }
                    else {
                        sum = sum - parseFloat($(this).parents("tr").children("td:eq(11)").text());
                        $(this).parents("tr").removeClass('highlightRow');
                    }
                   
                    $("#lblTotalChecked").text(sum);
                    //$("#<%=gv_customers.ClientID %> [id*=lblTotal]").text(sum);
                });
            });

    </script>
    
        <script type="text/javascript">
            $(document).ready(function () {
                $('#allselector').click(function () {
                    /* var inputs = $('span[data-check="item"] input[type="checkbox"]');
                    if ($('#allselector').is(':checked')) {
                    for (var i = 0; i < inputs.length; i++) {
                    $(inputs[i]).attr('checked', 'checked');
                    }
                    } else { //if($('#allselector').is(''))
                    for (var k = 0; k < inputs.length; k++) {
                    $(inputs[k]).removeAttr('checked');
                    }
                    }*/
                    if ($('#allselector').is(':checked')) {
                        $('#gv_customers').find('span[data-check="item"] input[type="checkbox"]').each(function () {
                            this.checked = true;
                        });
                    } else {
                        $('#gv_customers').find('span[data-check="item"] input[type="checkbox"]').each(function () {
                            this.checked = false;
                        });
                    }
                });
                //pagination
                $("#gv_customers").each(function () {
                    var currentPage = 0;
                    var numPerPage = 500;
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
    </script>
</asp:Content>
