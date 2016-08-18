<%@ Page Title="<%$Resources:Tokens,CenterPaymentCredit%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="CenterPaymentCredit.aspx.cs" Inherits="NewIspNL.Pages.CenterPaymentCredit" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="page-header"><h1><asp:Literal runat="server" Text="<%$Resources:Tokens,CenterPaymentCredit %>"></asp:Literal></h1></div>

        <div class="row">
        <div class="col-sm-12">
            <div class="form-horizontal">
                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal3" runat="server" Text="<%$Resources:Tokens,Center %>"></asp:Literal></label>
                <div class="col-sm-9">
                                    <div>
                        <asp:DropDownList runat="server" ID="DdlUsers" ClientIDMode="Static" AutoPostBack="True"
                                          OnSelectedIndexChanged="DdlUsers_SelectedIndexChanged" Width="178px"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DdlUsers"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                </div></div>
                <div class="space-4"></div>
                 <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal4" runat="server" Text="<%$Resources:Tokens,sav %>"></asp:Literal></label>
                <div class="col-sm-9">
                                        <div>
                        <asp:DropDownList runat="server" ID="ddlSaves" ClientIDMode="Static" DataTextField="SaveName" DataValueField="Id" Width="178px"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlSaves"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="ltAmount" runat="server" Text="<%$Resources:Tokens,Amount %>"></asp:Literal></label>
                <div class="col-sm-9">
                                        <div>
                        <asp:TextBox runat="server" ID="TbAmount" ClientIDMode="Static" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TbAmount"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="TbAmount"
                                            ErrorMessage="<%$Resources:Tokens,NumbersOnly %>" MaximumValue="99999999999"
                                            MinimumValue=".1" Type="Double"></asp:RangeValidator>
                    </div>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="ltNotes"  runat="server" Text="<%$Resources:Tokens,Notes %>"></asp:Literal></label>
                <div class="col-sm-9">
                    <asp:TextBox runat="server" ID="TbNotes" ClientIDMode="Static" TextMode="MultiLine" />
                </div></div>
                <div class="space-4"></div>
                                
                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="ltEffect" runat="server" Text="<%$Resources:Tokens,Effect %>"></asp:Literal></label>
                <div class="col-sm-9">
                                        <div>
                        <asp:RadioButtonList runat="server" ID="RblEffect" ClientIDMode="Static" RepeatDirection="Horizontal">
                            <asp:ListItem Selected="True" Text="<%$Resources:Tokens,Add %>"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Tokens,Subtract %>"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div></div>
                                                <div class="clearfix form-actions">
										<div class="col-md-offset-3 col-md-9">
											<button id="BSave" class="btn btn-primary" type="button" runat="server" OnServerClick="BSave_Click">
												<i class="icon-ok bigger-110"></i>
												<%=Tokens.Save %>
											</button>

											&nbsp; &nbsp; &nbsp;
											<button class="btn btn-default" type="reset">
												<i class="icon-undo bigger-110"></i>
												<%=Tokens.reset %>
											</button>
                <div id="message">
                    <asp:Literal runat="server" ID="Message"></asp:Literal>
                </div>
										</div>
									</div>
                </div></div>
    
    <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <asp:Literal ID="Literal2" Text="<%$Resources:Tokens,CreditHistory %>" runat="server" /></h3>
             <table>
                    <tr>
                        <td><%=Tokens.Credit %></td>
                        <td>:&nbsp;</td>
                        <td><asp:Label runat="server" ID="lblLastCredit"></asp:Label></td>
                    </tr>
                </table>
            <div>
                <asp:GridView runat="server" ID="GvHistory" CssClass="table table-bordered table-condensed text-center"
                              AutoGenerateColumns="False" OnDataBound="GvHistory_DataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="no" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Type" HeaderText="<%$Resources:Tokens,Type %>" />
                        <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Amount %>" />
                        <asp:BoundField DataField="Net" HeaderText="<%$Resources:Tokens,Net %>" />
                        <asp:BoundField DataField="Center" HeaderText="<%$Resources:Tokens,Center %>" />
                        <asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,User %>" />
                        <asp:BoundField DataField="Date" HeaderText="<%$Resources:Tokens,Date %>" />
                        <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes %>" />
                        
                        <%--<asp:TemplateField HeaderText="<%$Resources:Tokens,Attachments %>">
                            <ItemTemplate>
                                <a target="_blank" href="<%#Eval("link") %>"><%#Eval("link") %></a>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <a href='<%#Eval("RecieptUrl") %>' target="_blank" title="<%= Tokens.Reciept %>" data-rel="tooltip">
                                    <i class="icon-file-text bigger-190 green"></i>
                                    </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div>
                            <%= Tokens.NoResults %></div>
                    </EmptyDataTemplate>
                </asp:GridView>
               
            </div>
        </fieldset>
    </div></div>
</asp:Content>

