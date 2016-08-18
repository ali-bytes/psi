<%@ Page Title="<%$Resources:Tokens,CustomerNotes%>" ClientIDMode="Static" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="CustomerNotes.aspx.cs" Inherits="NewIspNL.Pages.CustomerNotes" %>
   

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view">
        <fieldset>
            <div class="page-header">
                <h1><%= Tokens.CutomerNotes%></h1></div>
            <div class="well">
                <div>
                    <label style="display: initial">
                        <input type="radio" runat="server" name="by" id="ByPhone" checked="True" />&nbsp;<%= Tokens.Phone %>
                    </label>
                    <label style="display: initial">
                        <input type="radio" runat="server" name="by" id="ByName" />&nbsp;<%= Tokens.Name %>
                    </label>
                </div>
                <hr />
                <div>
                    <label>
                        <%= Tokens.Phone %>&nbsp;/&nbsp;<%= Tokens.Name %></label>
                        <div>
                    <asp:TextBox runat="server" ID="TbNamePhone" />
                    <asp:RequiredFieldValidator ErrorMessage="*" ValidationGroup="search" ControlToValidate="TbNamePhone"
                        runat="server" ID="RNP" /></div>
                </div>
               
                <p>
                    <br/>
                    <button runat="server" id="BSearch" class="btn btn-success" validationgroup="search">
                        <i class="icon-white icon-search"></i>&nbsp;<%= Tokens.Search %></button>
                    &nbsp;
                </p>
            </div>
        </fieldset>
    </div>
    <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <%= Tokens.Results %></h3>
            <div>
                <div runat="server" id="msg">
                </div>
                <div>
                    <asp:GridView runat="server" AutoGenerateColumns="False" ID="GvCustomer" CssClass="table table-bordered table-condensed">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="LNo" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Customer" HeaderText="<%$Resources:Tokens,Customer %>" />
                            <asp:BoundField DataField="Phone" HeaderText="<%$Resources:Tokens,Phone %>" />
                            <asp:BoundField DataField="State" HeaderText="<%$Resources:Tokens,State %>" />
                            <asp:BoundField DataField="Branch" HeaderText="<%$Resources:Tokens,Branch %>" />
                            <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,Reseller %>" />
                            <asp:BoundField DataField="Offer" HeaderText="<%$Resources:Tokens,Offer %>" />
                            <asp:BoundField DataField="Central" HeaderText="<%$Resources:Tokens,Central %>" />
                            <asp:BoundField DataField="Package" HeaderText="<%$Resources:Tokens,Package %>" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <button type="button" class="btn btn-primary btn-xs" title="<%= Tokens.Add %>" data-rel="tooltip" data-select="note-modal" data-id="<%#Eval("Id") %>">
                                        <i class="icon-only icon-plus-sign"></i>
                                        </button>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </fieldset>
    </div>
    <div id="note-modal" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="note-note-modal"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
        <div class="modal-header">
            <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                &times;</button>
            <h4 id="note-note-modal">
                <%= Tokens.CustomerNote %>
            </h4>
        </div>
        <div class="modal-body">
            <div class="bootbox-body">
            <div class="well">
                <div>
                    <label><%= Tokens.CustomerNote %></label>
                    <div>
                        <asp:TextBox runat="server" ID="TbNote" ClientIDMode="Static" TextMode="MultiLine"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RTbNote" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                    ControlToValidate="TbNote" runat="server" />
                    </div>
                </div>
                <div>
                    <label for="DdlUsers"><%= Tokens.User %></label>
                    <div>
                        <asp:DropDownList runat="server" ID="DdlUsers"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="dfgd" ErrorMessage="*" ControlToValidate="DdlUsers"
                            runat="server" />
                    </div>
                </div>
            </div>
            </div>
        </div>
        <div class="modal-footer">
            <button id="BSave" runat="server" causesvalidation="True" validationgroup="reactivate"
                class="btn btn-primary">
                <%= Tokens.Save %></button>
            <span id="cancelReactivate" data-dismiss="modal" class="btn">
                <%= Tokens.Cancel %></span>
        </div>
        </div>
        </div>
    </div>
    <input type="hidden" id="selectedOrderId" runat="server" />
    <script type="text/javascript">
        $(function () {
            $('button[data-select="note-modal"]').on('click', function () {
                var input = $(this);
                $('#selectedOrderId').val($(input).attr("data-id"));
                $('#note-modal').modal('show');
            });
            $('#TbNamePhone').keypress(function (e) {
                var key = e.which;
                if (key == 13) {
                    $('#bSearch').click();
                    return false;
                } else {
                    return true;
                }
            });
        })
    </script>
</asp:Content>
