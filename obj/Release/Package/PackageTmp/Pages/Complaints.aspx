<%@ Page Title="<%$Resources:Tokens,Complaints%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="Complaints.aspx.cs" Inherits="NewIspNL.Pages.Complaints" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="page-header"><h1><asp:Literal runat="server" Text="<%$Resources:Tokens,NewComplaint %>"></asp:Literal></h1></div>
        <div id="message" style="background-color: rgb(192, 221, 0); color: rgb(76, 76, 76);
            margin: 5px;">
            <asp:Label runat="server" ID="l_message" ClientIDMode="Static"></asp:Label></div>
        <%--<div class="view">

                <div class="well">
                    <div>
                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Tokens,Customer%>"></asp:Label>
                        
                    </div>
                    <div>
                        <asp:Label ID="Label2" runat="server" Text="<%$Resources:Tokens,Governrate%>"></asp:Label>
                        
                    </div>
                    <div>
                        <asp:Label ID="Label3" runat="server" Text="<%$Resources:Tokens,Phone%>"></asp:Label>
                        
                    </div>
                    <div>
                        <asp:Label ID="Label4" runat="server" Text="<%$Resources:Tokens,Complaint%>"></asp:Label>
                        
                    </div>
                    <div>
                        <asp:Label ID="Label5" runat="server" Text="<%$Resources:Tokens,Date%>"></asp:Label>
                        
                    </div>
                    <p>
                        <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" Text="<%$Resources:Tokens,Save%>" OnClick="Button1_Click"
                            Width="90px" /></p>
                </div>
            
        </div>--%>
        
                <div class="col-sm-12">
                                <asp:Panel runat="server" ID="containerPanel"
                ClientIDMode="Static">
            <div class="form-horizontal">
                
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,Customer %>"></asp:Literal></label>
                <div class="col-sm-9">
                    <span class="input-icon">
                            <asp:TextBox ID="tb_customer" runat="server" Width="250px"></asp:TextBox><i class="icon-user blue"></i></span>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tb_customer"
                                runat="server" ErrorMessage="<%$Resources:Tokens,Required%>">*</asp:RequiredFieldValidator>
                        
                </div></div>
                <div class="space-4"></div>
                                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,Governrate %>"></asp:Literal></label>
                <div class="col-sm-9">
                    <div>
                            <asp:TextBox ID="tb_gov" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="tb_customer"
                                runat="server" ErrorMessage="<%$Resources:Tokens,Required%>">*</asp:RequiredFieldValidator>
                        </div>
                </div></div>
                <div class="space-4"></div>
                                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal3" runat="server" Text="<%$Resources:Tokens,Phone%>"></asp:Literal></label>
                <div class="col-sm-9">
                    <div>
                            <asp:TextBox ID="tb_phone" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="tb_customer"
                                runat="server" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tb_phone"
                                Display="Dynamic" ErrorMessage="<%$Resources:Tokens,Least5%>" ValidationExpression="^[0-9]{5,12}$"></asp:RegularExpressionValidator>
                        </div>
                </div></div>
                <div class="space-4"></div>
                                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal5" runat="server" Text="<%$Resources:Tokens,Date %>"></asp:Literal></label>
                <div class="col-sm-9">
                    <div>
                            <asp:TextBox ID="tb_date" runat="server" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="tb_customer"
                                runat="server" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                        </div>
                </div></div>
                <div class="space-4"></div>
                                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal4" runat="server" Text="<%$Resources:Tokens,Complaint %>"></asp:Literal></label>
                <div class="col-sm-9">
                    <div>
                            <asp:TextBox ID="tb_body" runat="server" Height="90px" TextMode="MultiLine" Width="250px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="tb_customer"
                                runat="server" ErrorMessage="<%$Resources:Tokens,Required%>">*</asp:RequiredFieldValidator>
                        </div>
                </div></div>

                                                <div class="clearfix form-actions">
										<div class="col-md-offset-3 col-md-9">
											<button id="BSave" class="btn btn-primary" type="button" runat="server" OnServerClick="Button1_Click">
												<i class="icon-ok bigger-110"></i>
												<%=Tokens.Save %>
											</button>

											&nbsp; &nbsp; &nbsp;
											<button class="btn btn-default" type="reset">
												<i class="icon-undo bigger-110"></i>
												<%=Tokens.reset %>
											</button>
                                            </div>
                </div></div>
                    </asp:Panel>
        
        
        
        

    </div>
    </div>
    <script src="../Content/ace-assest/js/jquery-ui-1.10.3.full.min.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $('#tb_date').datepicker({ dateFormat: 'dd-mm-yy' });
            if ($('#l_message').html() === '') {
                $('#message').css("border", "none");
            } else {
                $('#message')
                    .css("border", "silver solid 1px")
                    .css("padding", "4px")
                    .css("-moz-box-shadow", "0 0 1px #888")
                    .css("-webkit-box-shadow", " 0 0 1px#888")
                    .css("box-shadow", "0 0 1px #888");
            }
            $('input[value="Reject"]').click(function () {
            });
        });
    </script>
</asp:Content>
