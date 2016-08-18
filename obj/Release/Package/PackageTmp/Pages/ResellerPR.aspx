<%@ Page Title="<%$Resources:Tokens,ResellerPR%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ResellerPR.aspx.cs" Inherits="NewIspNL.Pages.ResellerPR" %>


<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
<link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>

    <div runat="server" id="l_message" >
    </div>
        <div runat="server" id="Div1" >
    </div>
    <div  runat="server" id="searchPanel">
            <div class="page-header"><h1>
                <%= Tokens.ResellerPR%></h1></div>

        <div class="well">
            <div>
                <asp:Label AssociatedControlID="ddl_reseller" runat="server" Text="<%$Resources:Tokens,Reseller %>"
                    ID="labelReseller"></asp:Label></div>
            <div>
                <asp:DropDownList runat="server" CssClass="width-50 chosen-select" ID="ddl_reseller" 
                    >
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddl_reseller"
                    ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="res"></asp:RequiredFieldValidator>
            </div>
        </div>
        <p>
            <asp:Button CssClass="btn btn-success" runat="server" ID="b_addRequest" Text="<%$Resources:Tokens,PaymentRequest %>"
                OnClick="b_addRequest_Click" ValidationGroup="res"/>
        </p>
    </div>
    <div  runat="server" id="resultsPanel">
        <fieldset>
            <div class="page-header"><h1>
                <%= Tokens.ResellerPR%></h1></div>
            <div>
                <div class="alert alert-info">
                <asp:Label runat="server" Text="<%$Resources:Tokens,selectedInvoice %>"></asp:Label> : <asp:Label  runat="server" ID="lblTotalChecked" ClientIDMode="Static"></asp:Label></div>

                <asp:GridView runat="server" ID="gv_customers" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed text-center"
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
                        <asp:BoundField DataField="Central" HeaderText="<%$ Resources:Tokens,Central%>" />
                        <asp:BoundField DataField="Offer" HeaderText="<%$ Resources:Tokens,Offer %>" />
                        <asp:BoundField DataField="TStart" HeaderText="<%$ Resources:Tokens,From %>" />
                        <asp:BoundField DataField="TEnd" HeaderText="<%$ Resources:Tokens,To%>" />
                        <asp:BoundField DataField="Amount" HeaderText="<%$ Resources:Tokens,Amount%>" />
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,Due %>">
                            <ItemTemplate>
                                <asp:Label ID="gv_lDue" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,Request %>">
                            <HeaderTemplate>

                                <input type="checkbox" id="allselector" name="name" value=" " />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div style="text-align: center">
                                    <asp:CheckBox ID="gv_cbRequested" runat="server" data-check="item" />
                                    <asp:HiddenField ID="gv_hfId" runat="server" Value='<%# Bind("Id") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <hr />
                <asp:HiddenField ID="pagecount" runat="server" />
                 <asp:HiddenField ID="oldcount" runat="server" />
               

                <div class="well">
                    <table>
                        <tr>
                            <td style="width: 150px;">
                                <asp:Label runat="server" ID="Label2" Text="<%$Resources:Tokens,ResellerTotalDue %>"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="l_customersDue" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="Label1" Text="<%$Resources:Tokens,ResellerCredit %>"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="L_ResellerCredit" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td>
                                <asp:Label runat="server" ID="Label3" Text="<%$Resources:Tokens,Selected %>"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="L_seleceted" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblavia" Text="<%$Resources:Tokens,AvailableCredit %>"></asp:Label>
                            </td>
                            <td>:</td>
                            <td>
                                <asp:Label runat="server" ID="lblavailableCredit" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <p>
                    <asp:LinkButton runat="server" ID="b_save" CssClass="btn btn-primary"
                        OnClick="b_save_Click"><i class="icon-save icon-only"></i>&nbsp;<asp:Literal runat="server" Text="<%$Resources:Tokens,Save %>"></asp:Literal></asp:LinkButton>
                </p>
            </div>
        </fieldset>
    </div>
    <script>
        $(document).ready(function () {
            $('#allselector').click(function () {
               
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

        });
    </script>

 
    <script type="text/javascript">

        $(document).ready(function () {
            $(".chosen-select").chosen();
            var sum = 0;
            var checkCheckBox = "table[id*=gv_customers] input[id*='gv_cbRequested']:checkbox";
            $(checkCheckBox).click(function () {//checkmyCheckBox
                if ($(this).is(':checked')) {
                    sum += parseFloat($(this).parents("tr").children("td:eq(11)").text());
                    $(this).parents("tr").addClass('highlightRow');
                }
                else {
                    sum -= parseFloat($(this).parents("tr").children("td:eq(11)").text());
                    $(this).parents("tr").removeClass('highlightRow');
                }
                $("#lblTotalChecked").text(sum);
            
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
                    }).appendTo($pager); 
                }
                $pager.insertBefore($table).find('span.page-number:first').addClass('active');
            });
        });

    </script>
</asp:Content>



