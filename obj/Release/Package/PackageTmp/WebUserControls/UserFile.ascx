<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserFile.ascx.cs" Inherits="NewIspNL.WebUserControls.UserFile" %>
<%@ Import Namespace="Resources" %>
<div>
    <%--<fieldset>
        <div class="page-header">
            <h1>
                <asp:Literal ID="Label20" runat="server" Text="<%$ Resources:Tokens,CustomerFiles %>"></asp:Literal></h1>
        </div>
        <div id="tb_all" runat="server">
            <div id="tr_Upload" runat="server" class="well">
                <div>
                    <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Tokens,SelectFile %>"></asp:Label>
                    <div>
                        <asp:FileUpload ID="FileUpload1" runat="server" />
                        <span>
                            <asp:Button ID="btn_Upload" runat="server" CssClass="btn btn-success" Text="<%$ Resources:Tokens,Add %>"
                                Width="100px" OnClick="btn_Upload_Click" />
                            <span>&nbsp;<asp:Label ID="lbl_Process" runat="server" EnableViewState="False" Font-Bold="True"
                                ForeColor="Red"></asp:Label>
                            </span></span>
                    </div>
                </div>
            </div>
            <div>
            </div>
        </div>
    </fieldset>--%>
</div>
<div class="col-xs-12 col-sm-7" style="margin: 0 22%;">
    <div class="widget-box">
        <div class="widget-header">
            <h4>
                <%=Tokens.CustomerFile%></h4>
            <span class="widget-toolbar">
            <a href="#" data-action="collapse"><i class="icon-chevron-up">
            </i></a>
            <a href="#" data-action="reload"><i class="icon-refresh"></i></a>
            <a href="#"
                data-action="close"><i class="icon-remove"></i></a>
                </span>
        </div>
        <div class="widget-body" id="tb_all" runat="server">
            <div class="widget-main well" id="tr_Upload" runat="server" style="border-bottom: navy;margin-bottom: 0;padding-bottom: 0;">
                <div>
                    <!-- <legend>Form</legend> -->
                    <fieldset>
                        <asp:FileUpload ID="FileUpload1" runat="server" ClientIDMode="Static" multiple="multiple" />
                        <asp:RegularExpressionValidator runat="server" ForeColor="red" ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|png|PNG|)$" ControlToValidate="FileUpload1"  ErrorMessage="<%$Resources:Tokens,uploadimgerror %>"></asp:RegularExpressionValidator>
                    </fieldset>
                    <div class="form-actions center" style="margin-bottom: 0;padding: 0;">  
                                          
                          <%--  <asp:Button ID="" runat="server" CssClass="" Text="<%$ Resources:Tokens,Add %>"
                             OnClick="" />--%>
                             <button type="submit" runat="server" id="btn_Upload" class="btn btn-success btn-block" onserverclick="btn_Upload_Click">
                            <i class="icon-cloud-upload bigger-110"></i>&nbsp;<%=Tokens.Add %></button>
                           
                        <%--<asp:Button runat="server" ID="bSave" CssClass="btn btn-success"  OnClick="btnAdd_Click" Text="<%$Resources:Tokens,Save%>"/><i class="icon-ok bigger-110">&nbsp;</i>
                        <button type="submit" runat="server" id="bSave" class="btn btn-success" onserverclick="btnAdd_Click">
                            <i class="icon-cloud-upload bigger-110"></i>&nbsp;<%=Tokens.Save %></button>
                        <asp:Label runat="server" ID="l_message" Text=""></asp:Label>--%>
                        </div></div></div>
                         <span>&nbsp;<asp:Label ID="lbl_Process" runat="server" EnableViewState="False" Font-Bold="True"
                                ForeColor="Red"></asp:Label>
                            </span>
                <%--<asp:Image ID="ImgAttachment1" runat="server" ImageUrl="~/App_Files/130860139279089276" Height="100px" Width="100px" />--%>
                                   
                        <asp:GridView ID="grd_Files" runat="server" OnDataBound="grd_Files_DataBounded" CssClass="table table-bordered table-responsive"
                            AutoGenerateColumns="False" ClientIDMode="Static" OnRowDataBound="grd_Files_RowDataBound">
                            <Columns>
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblID" Text='<%#Eval("ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWorkOrderID" runat="server" Text='<%#Eval("WorkOrderID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVirtualName" runat="server" Text='<%#Eval("VirtualName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNotes" runat="server" Text='<%#Eval("Notes") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Tokens,Name %>" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnb_Download" runat="server" CausesValidation="False" CommandArgument='<%# Eval("VirtualName") %>'
                                            OnClick="lnb_Download_Click" Text='<%# Eval("FileName") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Tokens,Delete %>" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnb_Delete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("VirtualName") %>'
                                            OnClick="lnb_Delete_Click" Text="Delete"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="<%$ Resources:Tokens,Attachments %>" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:Image ID="ImgAttachment" runat="server" Height="100px" Width="100px" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Label ID="lbl_Process" runat="server"><asp:Literal ID="Literal1" text="<%$ Resources:Tokens,NoFiles %>" runat="server" /></asp:Label></EmptyDataTemplate></asp:GridView></div></div></div><style type="text/css">
                #grd_Files {
                    margin-bottom: 0;
                }
            </style>