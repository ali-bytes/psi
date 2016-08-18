<%@ Page Title="<%$Resources:Tokens,Itemes %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="AddItems.aspx.cs" Inherits="NewIspNL.Pages.AddItems" %>
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
                <h1><asp:Literal ID="Literal8" runat="server" Text="<%$Resources:Tokens,Itemes %>"></asp:Literal>
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
                           <asp:BoundField DataField="Code" HeaderText="<%$Resources:Tokens,Code %>" />
                        <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:Tokens,ItemName %>" />
                        <asp:BoundField DataField="PurchasPrice" HeaderText="<%$Resources:Tokens,PurchasePrice %>" />
                        <asp:BoundField DataField="SellPrice" HeaderText="<%$Resources:Tokens,SellPrice %>" />
                        <asp:BoundField DataField="FirstBalance" HeaderText="<%$Resources:Tokens,BalancebeginingPeriod %>" />
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
               <label class="col-sm-3 control-label no-padding-right"> <asp:Literal ID="Literal4"  runat="server" Text="<%$Resources:Tokens,Code %>"></asp:Literal></label>
               <div class="col-sm-9">
                
                    <asp:TextBox runat="server" clientidmode="Static" ID="Txtcode" CssClass="required-input myText"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="Txtcode" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="additem"></asp:RequiredFieldValidator>
                </div></div> 
                <div class="form-group">
               <label class="col-sm-3 control-label no-padding-right"> <asp:Literal ID="lblday"  runat="server" Text="<%$Resources:Tokens,ItemName %>"></asp:Literal></label>
               <div class="col-sm-9">
                
                    <asp:TextBox runat="server" clientidmode="Static" ID="txtName" CssClass="required-input myText"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtName" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="additem"></asp:RequiredFieldValidator>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
               <label class="col-sm-3 control-label no-padding-right"> <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,PurchasePrice %>"></asp:Literal></label>
               <div class="col-sm-9">
                
                    <asp:TextBox runat="server" clientidmode="Static" ID="txtPurchasePrice" CssClass="required-input myText"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPurchasePrice" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="additem"></asp:RequiredFieldValidator>
                    <asp:CompareValidator runat="server" ID="CVTbpurchaseprice" Display="Dynamic" ErrorMessage="<%$Resources:Tokens,NumbersOnly %>" ControlToValidate="txtPurchasePrice" Type="Double" Operator="DataTypeCheck"></asp:CompareValidator>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
               <label class="col-sm-3 control-label no-padding-right"> <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:Tokens,SellPrice %>"></asp:Literal></label>
               <div class="col-sm-9">
                
                    <asp:TextBox runat="server" clientidmode="Static" ID="txtSellPrice" CssClass="required-input myText"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSellPrice" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="additem"></asp:RequiredFieldValidator>
                    <asp:CompareValidator runat="server" ID="CompareValidator1" ErrorMessage="<%$Resources:Tokens,NumbersOnly %>" Display="Dynamic" ControlToValidate="txtSellPrice" Type="Double" Operator="DataTypeCheck"></asp:CompareValidator>
                </div></div>
                <div class="space-4"></div>
                                                <div class="form-group">
               <label class="col-sm-3 control-label no-padding-right"> <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:Tokens,BalancebeginingPeriod %>"></asp:Literal></label>
               <div class="col-sm-9">
                    <asp:TextBox runat="server" clientidmode="Static" ID="txtFirstBalance" CssClass="required-input myText"></asp:TextBox>
                   <%--<input runat="server" type="text" clientidmode="Static" id="txtFirstBalance" class="required-input"/>--%>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtFirstBalance" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="additem"></asp:RequiredFieldValidator>
                     <asp:CompareValidator runat="server" ID="CompareValidator2" ErrorMessage="<%$Resources:Tokens,NumbersOnly %>" Display="Dynamic" ControlToValidate="txtFirstBalance" Type="Double" Operator="DataTypeCheck"></asp:CompareValidator>
                </div></div>
                                <div class="clearfix form-actions">
										<div class="col-md-offset-3 col-md-9">
											<button ID="BSave" clientidmode="Static" class="btn btn-info save" type="button" runat="server" ValidationGroup="additem">
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

                $(document).keypress(function (e) {
                    if (e.which === 13) {
                        $('#BSave').click();
                    }
                });
                $(".myText").keypress(function (e) {
                    if (e.which === 13) {
                        $('#BSave').click();
                        return false;
                    }
                });
               
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

