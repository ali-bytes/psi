<%@ Page  Title="<%$Resources:Tokens,Box %>"  Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="AddBox.aspx.cs" Inherits="NewIspNL.Pages.AddBox" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="page-header">
							<h1>
								<asp:Literal ID="Literal2" Text="<%$Resources:Tokens,Boxs %>"  runat="server" />
							</h1>
						</div>
                        <div class="col-xs-12">
                        <div class="well">

                <label for="TbBoxName">
                    <asp:Literal ID="LiBoxName" Text="<%$Resources:Tokens,Boxname %>" runat="server" /></label>
                <div style="margin: 0 20px;">
                    <asp:TextBox runat="server" ID="TbBoxName" ClientIDMode="Static" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TbBoxName"
                                                ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="box"></asp:RequiredFieldValidator></div>

                <div class="checkbox"><label> <input type="checkbox" name="form-field-checkbox" id="showcheck" runat="server" class="ace"/><span class="lbl">&nbsp;<%=Tokens.ShowinRechargeResellerRequest %></span></label></div>
                <div class="checkbox"><label><input type="checkbox" name="form-field-checkbox" id="showinResellerppr" runat="server" class="ace"/><span class="lbl">&nbsp;<%=Tokens.showinResellerppr %></span></label></div>
                <div class="checkbox"><label> <input type="checkbox" name="form-field-checkbox" id="showinCustomerDemands" runat="server" class="ace"/><span class="lbl">&nbsp;<%=Tokens.ShowinCustomerDemand %></span></label></div>
                <p>
                    <asp:Button runat="server" ID="BSave" Text="<%$Resources:Tokens,Save %>" ValidationGroup="box" onclick="BSave_Click"  CssClass="btn btn-success"/></p>
                <div id="message">
                    <asp:Literal runat="server" ID="Message"></asp:Literal>
                </div>
            </div></div>
            <h3 class="header smaller lighter blue"><asp:Literal ID="Literal1" Text="<%$Resources:Tokens,Boxs %>"  runat="server" /></h3>
                    <div class="table-responsive">
                        <asp:GridView runat="server" ID="GvBoxes" AutoGenerateColumns="False" DataKeyNames="ID"  OnDataBound="GvBox_OnDataBound"
                                      CssClass="table table-bordered table-hover table-responsive"  
                                      ClientIDMode="Static" OnRowCancelingEdit="grd_RowCancelingEdit" OnRowDeleting="grd_RowDeleting" OnRowEditing="grd_RowEditing"
                                      OnRowUpdating="grd_RowUpdating">
                            <Columns>
                                <asp:TemplateField HeaderText="#">
                                    <ItemTemplate>
                                        <asp:Label ID="LNo" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Tokens,Boxs %>">
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("BoxName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Tokens,CurrentCredit %>">
                                    <ItemTemplate>
                                        <asp:Label ID="Label13" runat="server" Text='<%# Bind("BoxNet") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Tokens,ShowinRechargeResellerRequest %>">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblshow" Text='<%#Bind("ShowBox") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Tokens,showinResellerppr %>">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblshow2" Text='<%#Bind("ShowBoxInResellerPPR") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$Resources:Tokens,ShowinCustomerDemand %>">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblshow3" Text='<%#Bind("ShowInCustomerDemands") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" data-rel="tooltip" CssClass="btn btn-xs btn-success" CommandName="Update" ToolTip="<%$Resources:Tokens,Update%>">
                                            <i class="icon-ok bigger-120"></i>
                                        </asp:LinkButton>
                                        &nbsp;<asp:LinkButton ID="LinkButton2" data-rel="tooltip" runat="server" CausesValidation="False" CssClass="btn btn-xs btn-danger" CommandName="Cancel"
                                                              ToolTip="<%$Resources:Tokens,Cancel%>"><i class="icon-reply bigger-120"></i> </asp:LinkButton>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" data-rel="tooltip" CssClass="btn btn-xs btn-info" runat="server" CausesValidation="False" CommandName="Edit"
                                                        ToolTip="<%$Resources:Tokens,Edit%>"><i class="icon-edit bigger-120"></i></asp:LinkButton>
                                                         &nbsp;
                                                         <asp:LinkButton ID="BDel" data-rel="tooltip" CssClass="btn btn-xs btn-danger" runat="server" OnClick="BDel_OnClick" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm ('تاكيد حذف الصندوق؟');" ToolTip="<%$Resources:Tokens,Delete%>">
                                            <i class="icon-trash bigger-120"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="alert">
                                    <%=Tokens.NoResults %>
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                    
</asp:Content>