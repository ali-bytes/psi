<%@ Page Title="<%$Resources:Tokens,CutomerNotes%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="SearchOrdersNotes.aspx.cs" Inherits="NewIspNL.Pages.SearchOrdersNotes" %>


<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view" id="result">
        <fieldset>
            <div class="page-header"><h1>
                <%= Tokens.CutomerNotes %></h1></div>
            <div runat="server" id="Msg">
            </div>
            <div>
                <asp:GridView ShowHeader="False" runat="server" ID="GvResults" AutoGenerateColumns="False"
                    CssClass="table table-bordered table-condensed">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LNo" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div class="pull">
                                    <div>
                                        <b>
                                            <%= Tokens.Name %></b>:&nbsp;<span><asp:Literal Text='<%#Eval("Name") %>' runat="server" /></span></div>
                                    <div>
                                        <b>
                                            <%= Tokens.Phone %></b>:&nbsp;<span><asp:Literal Text='<%#Eval("Phone") %>' runat="server" /></span></div>
                                    <div>
                                        <b>
                                            <%= Tokens.Governorate %></b>:&nbsp;<span><asp:Literal Text='<%#Eval("Governate") %>'
                                                runat="server" /></span></div>
                                    <div>
                                        <b>
                                            <%= Tokens.Provider %></b>:&nbsp;<span><asp:Literal Text='<%#Eval("Provider") %>'
                                                runat="server" /></span></div>
                                    <div>
                                        <b>
                                            <%= Tokens.State %></b>:&nbsp;<span><asp:Literal Text='<%#Eval("State") %>' runat="server" /></span></div>
                                    <div>
                                        <b>
                                            <%= Tokens.Package %></b>:&nbsp;<span><asp:Literal Text='<%#Eval("Package") %>' runat="server" /></span></div>
                                    <div>
                                        <b>
                                            <%= Tokens.Offer %></b>:&nbsp;<span><asp:Literal Text='<%#Eval("Offer") %>' runat="server" /></span></div>
                                    <div>
                                        <b>
                                            <%= Tokens.Branch %></b>:&nbsp;<span><asp:Literal Text='<%#Eval("Branch") %>' runat="server" /></span></div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div class="pull">
                                    <div>
                                        <b>
                                            <%= Tokens.Central %></b>:&nbsp;<span><asp:Literal Text='<%#Eval("Central") %>' runat="server" /></span></div>
                                    <div>
                                        <b>
                                            <%= Tokens.Activation_Date %></b>:&nbsp;<span><asp:Literal Text='<%#Eval("TActivation") %>'
                                                runat="server" /></span></div>
                                    <div>
                                        <b>
                                            <%= Tokens.Reseller %></b>:&nbsp;<span><asp:Literal Text='<%#Eval("Reseller") %>'
                                                runat="server" /></span></div>
                                    <div>
                                        <b>
                                            <%= Tokens.Time %></b>:&nbsp;<span><asp:Literal Text='<%#Eval("Time") %>' runat="server" /></span></div>
                                    <div>
                                        <b>
                                            <%= Tokens.User %></b>:&nbsp;<span><asp:Literal Text='<%#Eval("User") %>' runat="server" /></span></div>
                                    <div>
                                        <b>
                                            <%= Tokens.Done %></b>:&nbsp;<span><asp:Literal Text='<%#Eval("TProcessed") %>' runat="server" /></span></div>
                                    <div style="color: #000094;">
                                        <b>
                                            <%= Tokens.CustomerNote %></b>:&nbsp;<span><asp:Literal Text='<%#Eval("Note") %>'
                                                runat="server" /></span></div>
                                    <% if (CanProcess)
                                       {%>
                                    <button runat="server" type="button" class="btn btn-primary btn-sm" data-select="s" data-id='<%#Eval("Id") %>' title="<%$Resources:Tokens,Process %>" data-rel="tooltip" clientidmode="Static">
                                       <i class="icon-fire icon-only"></i></button>
                                    <%  } %>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
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
            <h3 id="note-note-modal">
                <%= Tokens.Done %>
            </h3>
        </div>
        <div class="modal-body">
            <div class="bootbox-body">
            <div class="well">
                <div>
                    <div>
                        <asp:TextBox runat="server" ID="TbComment" ValidationGroup="c"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RTbComment" ValidationGroup="c" ErrorMessage="<%$Resources:Tokens,Required %>"
                            ControlToValidate="TbComment" runat="server" />
                    </div>
                </div>
            </div>
            </div>
        </div>
        <div class="modal-footer">
            <%--<button id="BProcessNote" runat="server" causesvalidation="True" validationgroup="c"
                class="btn btn-primary">
                <%= Tokens.Done %></button>--%>
            <asp:Button ID="BProcessNote1" runat="server" causesvalidation="True" validationgroup="c" Text="<%$Resources:Tokens,Done %>" class="btn btn-primary" OnClick="BProcessNote1_Click" />
            <span id="cancelReactivate" class="btn" data-dismiss="modal">
                <%= Tokens.Cancel %></span>
        </div>
        </div>
        </div>
    </div>
    <input type="hidden" id="selected" runat="server" value="0" ClientIDMode="Static" />
 
    <script type="text/javascript">
        $(function () {
            $('input[data-select="dp"]').datepicker({
                showOtherMonths: true,
                selectOtherMonths: false,
                dateFormat: 'dd/mm/yy'
            });
            $('button[data-select="s"]').on('click', function () {
                var id = $(this).attr('data-id');
                $('#selected').val(id);
                console.log(id);
                $('#note-modal').modal('show');
            });
        });
    </script>
</asp:Content>
