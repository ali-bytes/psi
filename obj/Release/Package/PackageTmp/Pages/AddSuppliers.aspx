<%@ Page Title="<%$Resources:Tokens,Suppliers %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="AddSuppliers.aspx.cs" Inherits="NewIspNL.Pages.AddSuppliers" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
                            <div class="alert alert-success" runat="server" id="SuccDiv" Visible="False">
                            <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,ProcessDone %>"></asp:Literal>
                        </div>
                                                <div class="alert alert-danger" runat="server" id="ErrorDiv" Visible="False">
                            <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:Tokens,SavingError %>"></asp:Literal>
                        </div>
                        <div data-select="0">
                                        <div class="page-header">
                <h1><asp:Literal ID="Literal8" runat="server" Text="<%$Resources:Tokens,Suppliers %>"></asp:Literal>
                </h1>
            </div>
            <div>
                <div>
                    <button type="button" runat="server" OnServerClick="Newdata" ID="BAdd" CausesValidation="False" class="btn btn-success">
                        <%= Tokens.Add %></button><br/>
                </div>
                <asp:GridView runat="server" ID="GvSuppliers" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed text-center col-md-12 col-sm-12">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LNo" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="SupplierName" HeaderText="<%$Resources:Tokens,SupplierName %>" />
                        <asp:BoundField DataField="SupplierPhone" HeaderText="<%$Resources:Tokens,Phone %>" />
                        <asp:BoundField DataField="SupplierMobile1" HeaderText="<%$Resources:Tokens,Mobile %>" />
                        <asp:BoundField DataField="SupplierEmail" HeaderText="<%$Resources:Tokens,Email %>" />
                        <asp:BoundField DataField="SupplierPersonalId" HeaderText="<%$Resources:Tokens,NationalId %>" />
                        <asp:BoundField DataField="GovernorateName" HeaderText="<%$Resources:Tokens,Governorate %>" />
                        <asp:BoundField DataField="SupplierAddress" HeaderText="<%$Resources:Tokens,Address %>" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div>
                                    <button ValidationGroup='<%#Eval("Id") %>' runat="server" id="BEdit" class="btn btn-xs btn-primary"
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
                        </div>
                        <div data-select="1">
                                            <div class="page-header">
							<h1>
                                 <asp:Literal ID="l_CustomerDetails" runat="server" Text="<%$ Resources:Tokens,Details %>"></asp:Literal>
							</h1>
						</div>
    												<div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal"> 
                <div class="form-group">
               <label class="col-sm-3 control-label no-padding-right"> <asp:Literal ID="lblday" runat="server" Text="<%$Resources:Tokens,SupplierName %>"></asp:Literal></label>
               <div class="col-sm-9">
                
                    <asp:TextBox runat="server" ID="txtName" CssClass="required-input"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtName" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="addsup"></asp:RequiredFieldValidator>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
               <label class="col-sm-3 control-label no-padding-right"> <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,Phone %>"></asp:Literal></label>
               <div class="col-sm-9">
                
                    <asp:TextBox runat="server" ID="txtPhone" CssClass="required-input"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPhone" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="addsup"></asp:RequiredFieldValidator>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
               <label class="col-sm-3 control-label no-padding-right"> <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:Tokens,Mobile %>"></asp:Literal></label>
               <div class="col-sm-9">
                
                    <asp:TextBox runat="server" ID="txtMobile" CssClass="required-input"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMobile" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="addsup"></asp:RequiredFieldValidator>
                </div></div>
                <div class="space-4"></div>
                                                <div class="form-group">
               <label class="col-sm-3 control-label no-padding-right"> <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:Tokens,Mobile2 %>"></asp:Literal></label>
               <div class="col-sm-9">
                    <asp:TextBox runat="server" ID="txtMobile2" CssClass="required-input"></asp:TextBox>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
               <label class="col-sm-3 control-label no-padding-right"> <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:Tokens,Address %>"></asp:Literal></label>
               <div class="col-sm-9">
                
                    <asp:TextBox runat="server" ID="txtAddress" CssClass="required-input"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAddress" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="addsup"></asp:RequiredFieldValidator>
                </div></div>
                <div class="space-4"></div>
                                                <div class="form-group">
               <label class="col-sm-3 control-label no-padding-right"> <asp:Literal ID="Literal16" runat="server" Text="<%$Resources:Tokens,Governorate %>"></asp:Literal></label>
               <div class="col-sm-9">
                                                    <asp:DropDownList ID="ddl_Governorates" CssClass="required-input" runat="server"
                                        DataTextField="GovernorateName" DataValueField="ID" >
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ControlToValidate="ddl_Governorates" ID="RequiredFieldValidator7"
                                        runat="server" ErrorMessage="*" ValidationGroup="addsup"></asp:RequiredFieldValidator>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
               <label class="col-sm-3 control-label no-padding-right"> <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:Tokens,Email %>"></asp:Literal></label>
               <div class="col-sm-9">
                
                    <asp:TextBox runat="server" ID="txtEmail" CssClass="required-input"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtEmail" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="addsup"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="أدخل الايميل بشكل صحيح"
                                        ControlToValidate="txtEmail" ValidationGroup="addsup" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        Display="Dynamic"></asp:RegularExpressionValidator>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
               <label class="col-sm-3 control-label no-padding-right"> <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:Tokens,NationalId %>"></asp:Literal></label>
               <div class="col-sm-9">
                
                    <asp:TextBox runat="server" ID="txtPersonalId" CssClass="required-input"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtPersonalId" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="addsup"></asp:RequiredFieldValidator>
                </div></div>
                                <div class="clearfix form-actions">
										<div class="col-md-offset-3 col-md-9">
											<button ID="BSave" class="btn btn-info" type="button" runat="server" ValidationGroup="addsup">
												<i class="icon-ok bigger-110"></i>
												<%=Tokens.Save %>
											</button>

											&nbsp; &nbsp; &nbsp;
                                            <button class="btn" CausesValidation="False" runat="server" ID="bCancel"><%= Tokens.Cancel %>&nbsp;<i class="icon-undo icon-only"></i></button>
<%--											<button class="btn" type="reset">
												<i class="icon-undo bigger-110"></i>
												<%=Tokens.reset %>
											</button>--%>
										</div>
									</div>
                </div>
                </div>

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

