<%@ Page Title="<%$Resources:Tokens,ResellerCollectionCommission %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ResellerCollectionCommission.aspx.cs" Inherits="NewIspNL.Pages.ResellerCollectionCommission" %>

<%@ Import Namespace="Resources" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="view">
        <div id="msg" runat="server" Visible="False"></div>
        <div class="page-header">
            <h1>
                <asp:Literal runat="server" Text="<%$Resources:Tokens,ResellerPackagesDiscounts %>"></asp:Literal></h1>
        </div>
        <div>
            <div class="well">
                <div class="col-md-6">
                    <div>
                        <label for="DdlReseller">
                            <%= Tokens.Reseller %></label>
                        <div>
                            <asp:DropDownList runat="server" CssClass="width-60 chosen" ID="DdlReseller">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="DdlReseller" ID="R1" ValidationGroup="ed"
                                runat="server" />
                        </div>
                    </div>
                   <%-- <div>
                        <label for="DdlProvider">
                            <%= Tokens.Provider %></label>
                        <div>
                            <asp:DropDownList runat="server" CssClass="width-60 chosen" ID="DdlProvider">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="DdlProvider" runat="server" ValidationGroup="ed"
                                ID="r2" />
                        </div>
                    </div>--%>
                </div>
                <div>
                    <div class="form-group">
                        <label for="TbHisClientsCommission" class="col-sm-3 control-label">
                            <asp:Literal Text="<%$Resources:Tokens,HisClientsCommission %>" runat="server" />
                        </label>
                        <div class="input-group col-md-3">
                            <asp:TextBox runat="server" ID="TbHisClientsCommission" TextMode="Number" class="form-control" ClientIDMode="Static"></asp:TextBox>
                            <span class="input-group-addon">جنية</span>
                            <asp:RequiredFieldValidator ID="RTClientsCommission" ValidationGroup="ed" ErrorMessage="<%$Resources:Tokens,Required %>"
                                ControlToValidate="TbHisClientsCommission" runat="server" />
                        </div>
                    </div>
                    <br />

                    <label for="TbExternalCustomersCommission" class="col-sm-3 control-label">
                        <asp:Literal Text="<%$Resources:Tokens,ExternalCustomersCommission %>" runat="server" />
                    </label>
                    <div class="input-group col-md-3">
                        <asp:TextBox runat="server" ID="TbExternalCustomersCommission" TextMode="Number" class="form-control" ClientIDMode="Static"></asp:TextBox>
                        <span class="input-group-addon">جنية</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="ed" ErrorMessage="<%$Resources:Tokens,Required %>"
                            ControlToValidate="TbExternalCustomersCommission" runat="server" />
                    </div>
                    <br />
                    <p>
                        <button id="btnAdd" class="btn btn-success" runat="server" causesvalidation="True" ValidationGroup="ed"
                            onserverclick="Add">
                            <i class="icon-white icon-save"></i>&nbsp;<%= Tokens.Add %></button>
                    </p>
                </div>
            </div>
           <%-- <p>
                <button id="b_new" class="btn btn-success" runat="server" 
                    onserverclick="Search">
                    <i class="icon-white icon-search"></i>&nbsp;<%= Tokens.Search %></button>
            </p>--%>
            <div>
                <asp:GridView ID="gv_index" runat="server" CssClass="table table-bordered table-condensed text-center"
                    AutoGenerateColumns="False" OnDataBound="gv_index_DataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div>
                                    <asp:Label ID="l_number" runat="server" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,Reseller %>" />
                        <%--<asp:BoundField DataField="Provider" HeaderText="<%$Resources:Tokens,Provider %>" />--%>
                        <asp:BoundField DataField="HisClientsCommission" HeaderText="<%$Resources:Tokens,HisClientsCommission %>" />
                        <asp:BoundField DataField="ExternalCustomersCommission" HeaderText="<%$Resources:Tokens,ExternalCustomersCommission %>" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <button type="button" class="btn btn-primary btn-sm" onclick="editDialog(<%#Eval("ResellerId")%>,<%#Eval("ProviderId")%>,<%#Eval("HisClientsCommission")%>,<%#Eval("ExternalCustomersCommission")%>)" title="<%=Tokens.Edit %>">
                                    <%=Tokens.Edit %><i class="icon-only icon-edit"></i></button>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <div id="EditModal" class="bootbox modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                    <h4>
                        <%= Tokens.Edit %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <div class="well">
                                <div class="col-md-6">
                                    <label for="DdlReseller">
                                        <%= Tokens.Reseller %></label>
                                    <div>
                                      
                                        <asp:DropDownList runat="server" CssClass="width-90" ID="DdlResellerEdit" ClientIDMode="Static" >
                                        </asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="DdlResellerEdit" ID="RequiredFieldValidator2"
                                            runat="server"  ValidationGroup="m"/>--%>
                                    </div>
                                </div>
                               <%-- <div class="col-md-6">
                                    <label for="DdlProvider">
                                        <%= Tokens.Provider %></label>
                                    <div>
                                       
                                        <asp:DropDownList runat="server" CssClass="width-90" ID="DdlProviderEdit" ClientIDMode="Static" >
                                        </asp:DropDownList>--%>
                                       <%-- <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="DdlProviderEdit" runat="server"
                                            ID="RequiredFieldValidator3"  ValidationGroup="m" />--%>
                                 <%--   </div>
                                </div>
                          --%>
                            <div class="form-inline">
                                <label for="TbDialogHisClientsCommission">
                                    <asp:Literal Text="<%$Resources:Tokens,HisClientsCommission %>" runat="server" />
                                </label>
                                <div>
                                    <asp:TextBox runat="server" ID="TbDialogHisClientsCommission" TextMode="Number"  ClientIDMode="Static"></asp:TextBox>

                                    <asp:RequiredFieldValidator ID="RTdClientsCommission" ValidationGroup="m" ErrorMessage="<%$Resources:Tokens,Required %>"
                                        ControlToValidate="TbDialogHisClientsCommission" runat="server" />
                                </div>
                                <label for="TbDialogExternalCustomersCommission">
                                    <asp:Literal Text="<%$Resources:Tokens,ExternalCustomersCommission %>" runat="server" />
                                </label>
                                <div>
                                    <asp:TextBox runat="server" ID="TbDialogExternalCustomersCommission" TextMode="Number" ClientIDMode="Static"></asp:TextBox>

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatord" ValidationGroup="m" ErrorMessage="<%$Resources:Tokens,Required %>"
                                        ControlToValidate="TbDialogExternalCustomersCommission" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" runat="server" onserverclick="EditCommission"
                        class="btn btn-primary" ValidationGroup="m">
                        <i class="icon-white icon-ok"></i>&nbsp;<%= Tokens.Save %></button>&nbsp;
            <span data-dismiss="modal" class="btn btn-default"><i class="icon-white icon-arrow-left"></i>&nbsp;<%= Tokens.Cancel %></span>

                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdResellerId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdProviderId" runat="server" ClientIDMode="Static" />
    <link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css" />
    <script type="text/javascript">

        $(document).ready(function () {
            window.editDialog = function (resellerId, providerId, hisCom, externalComm) {
                var ddlRes = $("#DdlResellerEdit").val(resellerId);
                //var ddlProv = $("#DdlProviderEdit").val(providerId);
                $("#hdResellerId").val(resellerId);
                //$("#hdProviderId").val(providerId);
                $("#TbDialogHisClientsCommission").val(hisCom);
                $("#TbDialogExternalCustomersCommission").val(externalComm);
                ddlRes.prop('disabled', true);
                //ddlProv.prop('disabled', true);
                $('#EditModal').modal('show');
            };
        });
        jQuery(document).ready(function () {
            jQuery(".chosen").chosen();
        });
    </script>
</asp:Content>
