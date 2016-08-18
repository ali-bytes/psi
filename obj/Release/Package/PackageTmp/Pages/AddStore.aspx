<%@ Page Title="<%$Resources:Tokens,AddStore %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="AddStore.aspx.cs" Inherits="NewIspNL.Pages.AddStore" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
            <div class="alert alert-danger" runat="server" ID="MsgError">
    </div>
    <div class="alert alert-success" runat="server" ID="MsgSuccess"></div>
    <div class="view" data-select="0">
        <fieldset>
            <div class="page-header">
                <h1><asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,AddStore %>"></asp:Literal>
                </h1>
            </div>
            <div>
                <div>
                    <button type="button" runat="server" OnServerClick="Newdata" ID="BAdd" CausesValidation="False" class="btn btn-success">
                        <%= Tokens.Add %></button>
                </div>
                <asp:GridView runat="server" ID="GvStore" AutoGenerateColumns="False" CssClass="table table-bordered  table-condensed">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LNo" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="StoreName" HeaderText="<%$Resources:Tokens,AddStore %>" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div style="width: 150px">
                                    <button validationgroup='<%#Eval("Id") %>' runat="server" id="BEdit" class="btn btn-xs btn-primary"
                                            OnServerClick="EditEvent" data-rel="tooltip" title="<%$Resources:Tokens,Edit %>"><i class="icon-edit bigger-120"></i></button>&nbsp;
                                        <asp:LinkButton runat="server" ValidationGroup='<%#Eval("Id") %>' OnClientClick="return confirm('هل انت متأكد من اتمام العملية');" data-rel="tooltip"
                                        ToolTip="<%$Resources:Tokens,Delete %>" OnClick="DeleteEvent" ID="BDelete" CssClass="btn btn-xs btn-danger"><i class="icon-trash bigger-120"></i></asp:LinkButton>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate><%=Tokens.NoResults %></EmptyDataTemplate>
                </asp:GridView><%--onclick="return confirm('هل انت متأكد من اتمام العملية');"--%>
            </div>
        </fieldset>
    </div>
    <div class="view" data-select="1">
        <div>
            <fieldset>
                <div class="page-header">
                   <h1> <%= Tokens.Details %></h1></div>
                <div class="row-fluid">
                    <div class="span6">
                    <div class="well">
                        <div>
                            <label for="txtStoreName">
                                <asp:Literal ID="Literal2" Text="<%$Resources:Tokens,Store %>" runat="server" />
                            </label>
                            <div>
                                <asp:TextBox runat="server" ID="txtStoreName" ValidationGroup="sto"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqName" ValidationGroup="sto" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                            ControlToValidate="txtStoreName" runat="server" />
                            </div>
                        </div>
                        <div>

                            <label for="txtNotes">
                                <asp:Literal ID="Literal5" Text="<%$Resources:Tokens,Notes %>" runat="server" />
                            </label>
                            <div>
                                
                            <asp:TextBox runat="server" ID="txtNotes" TextMode="MultiLine"></asp:TextBox>
                                    
                            </div>
                        </div>
                    </div>
                   </div>
                </div>
                <div>
                    <button class="btn btn-primary" runat="server" ID="BSave" ValidationGroup="sto" ><%= Tokens.Save %>&nbsp;<i class="icon-search icon-only"></i></button> &nbsp; <button class="btn btn-danger" CausesValidation="False" runat="server" ID="bCancel"><%= Tokens.Cancel %>&nbsp;<i class="icon-undo icon-only"></i></button>
                </div>
            </fieldset>
        </div>
    </div>
    <input type="hidden" id="flag" runat="server" ClientIDMode="Static"/>
    <input type="hidden" id="selected" runat="server" ClientIDMode="Static"/>
    <script type="text/javascript">
        $(document).ready(function () {
            var $preview = $('div[data-select="0"]');
            var $edit = $('div[data-select="1"]');
            var flag = $('#flag').val();
            var selected = $('#selected').val();
            if (flag === "0") {
                $preview.show();
                $edit.hide();
                selected.val = "0";
            } else {
                $preview.hide();
                $edit.show();
            }
        });
    </script>
</asp:Content>

