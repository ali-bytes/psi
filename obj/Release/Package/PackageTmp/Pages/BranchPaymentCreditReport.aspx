<%@ Page Title="<%$Resources:Tokens,BranchPaymentCreditReport %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="BranchPaymentCreditReport.aspx.cs" Inherits="NewIspNL.Pages.BranchPaymentCreditReport" %>

<%@ Import Namespace="Resources" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="view">
            <fieldset>
                <div class="page-header">
                    <h1><%= Tokens.BranchPaymentCreditReport%></h1>
                </div>


                <div class="row">
                    <div class="col-sm-12">
                        <div class="form-horizontal">
                            <div class="col-sm-4">
                                <div class="well" style="height: 208px;">
                                    <div>
                                         <div>
                                            <div>
                                                 <label for="Ddlbranch">
                                                <%= Tokens.Branch %></label>
                                                <div>
                                                    <asp:DropDownList runat="server" ID="Ddlbranch" ClientIDMode="Static" Width="177px" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Ddlbranch"
                                                        ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div>
                                            <label for="TbFrom">
                                                <%= Tokens.From %></label>
                                            <div>
                                                <asp:TextBox runat="server" ID="TbFrom" data-select="dp" />
                                            </div>
                                        </div>
                                        <div>
                                            <label for="TbTo">
                                                <%= Tokens.To %></label>
                                            <div>
                                                <asp:TextBox runat="server" ID="TbTo" data-select="dp" />
                                                <asp:CompareValidator ID="CV" ErrorMessage="*" ControlToValidate="TbTo" runat="server"
                                                    ControlToCompare="TbFrom" Operator="GreaterThanEqual" Type="Date" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <hr />
                <div class="col-md-12">
                    <button runat="server" onserverclick="Search" type="submit" class="btn btn-success">
                        <i class="icon-search"></i>&nbsp;<%= Tokens.Search %></button>
                </div>
            </fieldset>
        </div>
        <div class="view">
            <fieldset>
                <h3 class="header smaller lighter blue">
                    <%= Tokens.CreditHistory %></h3>
                <table class="alert alert-info">
                    <tr>
                        <td><%=Tokens.Credit %></td>
                        <td>:&nbsp;</td>
                        <td>
                            <asp:Label runat="server" ID="lblLastCredit"></asp:Label></td>
                    </tr>
                </table>
                <div>
                    <asp:GridView runat="server" ID="GvHistory" CssClass="table table-bordered table-condensed"
                        AutoGenerateColumns="False"
                        OnDataBound="GvHistory_DataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="#">
                                <ItemTemplate>
                                    <asp:Label ID="no" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Type" HeaderText="<%$Resources:Tokens,Type %>" />
                            <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Amount %>" />
                            <asp:BoundField DataField="Net" HeaderText="<%$Resources:Tokens,Net %>" />
                            <asp:BoundField DataField="Branch" HeaderText="<%$Resources:Tokens,Branch %>" />
                            <asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,User %>" />
                            <asp:BoundField DataField="Date" HeaderText="<%$Resources:Tokens,Date %>" />
                            <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes %>" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <a data-rel="tooltip" title="<%=Tokens.Reciept %>" href='<%#Eval("RecieptUrl") %>' target="_blank">
                                        <i class="icon-file-text bigger-120 green"></i></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div>
                                <%= Tokens.NoResults %>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>

                </div>
            </fieldset>
        </div>
    </div>
     <link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>
    <script type="text/javascript">
        $(document).ready(function () {
            jQuery(".chosen-select").chosen();
            $('input[data-select="dp"]').datepicker({ dateFormat: 'dd/mm/yy' });
            if ($('#GvResults').width() > 1058) {
                $('#GvResults').css({
                    "font-size": "81%"
                });
            }
        });
        function plswait(id) {

            var type = document.getElementById('<%=Ddlbranch.ClientID%>').value;

            var cocheck = op.getElementsByTagName('input');
            var checkedValues = [];

            for (var i = 0; i < cocheck.length; i++) {
                var checkBoxRef = cocheck[i];

                if (checkBoxRef.checked == true) {
                    checkedValues.push("added");
                }
            }

            if (type == "" || amo == "" || type2 == "" || checkedValues.length == 0) { return; }
            else {
                var check2 = document.getElementById(id);
                check2.disabled = 'true'; check2.value = 'Please wait...';
            }

        }




        $(document).ready(function () {
            $('tr td input[type=radio]').addClass("ace");
            $('tr td label').addClass("lbl").css({ 'padding': '7px' });
        })
    </script>
</asp:Content>
