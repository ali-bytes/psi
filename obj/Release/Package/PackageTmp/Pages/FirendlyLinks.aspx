<%@ Page Title="<%$Resources:Tokens,FirendlyLinks%>"  Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="FirendlyLinks.aspx.cs" Inherits="NewIspNL.Pages.FirendlyLinks" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="view">
        <fieldset>
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="v_index" runat="server">
                <div id="message">
                    <asp:Label runat="server" ID="l_message"></asp:Label></div>
                <asp:Panel runat="server" ID="p_index">
                    <div class="page-header"><h1><asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,Index%>"></asp:Literal></h1></div>
                    <div style="padding: 5px;">
                        <p>
                            <asp:LinkButton ID="b_new" CssClass="btn btn-success" runat="server"
                                        Width="90px" OnClick="b_new_Click"><i class="icon-plus-sign"></i>&nbsp;<%=Tokens.New %></asp:LinkButton></p>
                        <asp:GridView ID="gv_index" runat="server"  GridLines="Horizontal"
                                      AutoGenerateColumns="False" OnDataBound="gv_index_DataBound"  CssClass="table table-bordered table-condensed text-center">
                            <Columns>
                                <asp:TemplateField HeaderText="#">
                                    <ItemTemplate>
                                        <asp:Label ID="l_number" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="<%$ Resources:Tokens,Name%>" DataField="PageName"></asp:BoundField>
                                <asp:BoundField DataField="Url" HeaderText="<%$ Resources:Tokens,Url %>" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvb_edit" runat="server" CommandArgument='<%# Bind("Id") %>' OnClick="gvb_edit_Click"
                                         data-rel="tooltip" ToolTip="<%$ Resources:Tokens,Edit%>"><i class="icon-pencil icon-only bigger-140"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvb_delete" runat="server" CommandArgument='<%# Bind("Id") %>' ToolTip="<%$ Resources:Tokens,Delete %>"
                                         data-rel="tooltip" OnClick="gvb_delete_Click"><i class="icon-trash icon-only bigger-140 red"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                            </Columns>
                            <EmptyDataTemplate><%=Tokens.NoResults %></EmptyDataTemplate>
                            <RowStyle HorizontalAlign="Center"></RowStyle>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </asp:View>
            <asp:View ID="v_AddEdit" runat="server">
                <asp:HiddenField ID="hf_id" runat="server" />
                <asp:Panel runat="server" ID="p_add">
                    <div class="page-header"><h1><asp:Literal ID="Literal4" runat="server" Text="<%$Resources:Tokens,FirendlyLinks%>"></asp:Literal></h1></div>
                        <div class="row">
        <div class="col-sm-12">
            <div class="form-horizontal">
                                        <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,Name %>"></asp:Literal></label>
                <div class="col-sm-9">
                                                    <asp:TextBox ClientIDMode="Static" runat="server" ID="tb_title" Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="r_offer" ControlToValidate="tb_title"
                                                            ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div></div>
                                    <div class="space-4"></div>

                        
                        <div class="form-group">
                            <label class="col-sm-3 control-label no-padding-right" for="tb_discount">
                                <asp:Literal ID="Label3" runat="server" Text="<%$ Resources:Tokens,Url%>"></asp:Literal>
                            </label>
                            <div class="col-sm-9">
                                <asp:TextBox ClientIDMode="Static" runat="server" ID="tb_discount" Width="200px" class="col-xs-10 col-sm-5"></asp:TextBox>
                                                                                            <span class="help-inline col-xs-12 col-sm-7">
												<span class="middle">EX: Http://www.gooogle.com</span>
											</span>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="tb_discount"
                                                            ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>

                            </div>

                        </div>
                        
                        <div class="clearfix form-actions">
										<div class="col-md-offset-3 col-md-9">
											<!--<button class="btn btn-info" type="button">
												<i class="icon-ok bigger-110"></i>
												Submit
											</button>-->
<asp:LinkButton runat="server" ID="b_save" OnClick="b_save_Click" CssClass="btn btn-primary"><i class="icon-ok icon-only"></i>&nbsp;<%=Tokens.Save %></asp:LinkButton>
											&nbsp; &nbsp; &nbsp;
											<button class="btn" type="reset">
												<i class="icon-undo bigger-110"></i>
												<%=Tokens.Clear %>
											</button>
										</div>
									</div>

                        <p>
                            
                        </p>
                    </div>
                    </div></div>
                </asp:Panel>
            </asp:View>
        </asp:MultiView>
        </fieldset>
    </div>
</asp:Content>

