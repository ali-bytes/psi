<%@ Page Title="<%$Resources:Tokens,CallCenterEntry%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="CallCenterEntry.aspx.cs" Inherits="NewIspNL.Pages.CallCenterEntry" %>

<%@ Import Namespace="Resources" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="page-header"><h1><asp:Literal runat="server" Text="<%$Resources:Tokens,Calleditor %>"></asp:Literal></h1></div>
<%--    <script src="../js/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.custom.min.js" type="text/javascript"></script>
    <script src="../js/jquery.ui.datepicker-ar.js" type="text/javascript"></script>--%>
    <script src="../Content/ace-assest/js/jquery-ui-1.10.3.full.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $('#tb_VisitDate').datepicker({ dateFormat: 'dd-mm-yy' });
            $('#tb_CallDate').datepicker({ dateFormat: 'dd-mm-yy' });
        });

    </script>
    
    
    <div class="row">
        <div class="col-sm-12">
            <div class="form-horizontal">
                      <asp:Panel ID="p_editor" runat="server">
                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,Customer.Name %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div class="editor-label">
                <asp:TextBox ID="tb_CustomerName" runat="server" Width="200px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                    Display="Dynamic" ControlToValidate="tb_CustomerName" ValidationGroup="save" Text="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
            </div>
                </div></div>
                <div class="space-4"></div>

                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,Responsible %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div class="editor-label">
                <asp:TextBox ID="tb_Responsible" runat="server" Width="200px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                    Display="Dynamic" ControlToValidate="tb_Responsible" ValidationGroup="save" Text="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
            </div>
                </div></div>
                
                <div class="space-4"></div>
                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal5" runat="server" Text="<%$Resources:Tokens,VisitDate %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div class="editor-label">
                <asp:TextBox ID="tb_VisitDate" runat="server" Width="200px" ClientIDMode="Static"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                    Display="Dynamic" ControlToValidate="tb_VisitDate" ValidationGroup="save" Text="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToValidate="tb_VisitDate"
                    Display="Dynamic" ErrorMessage="<%$Resources:Tokens,ValidDate %>" Operator="DataTypeCheck" Type="Date" Text="<%$Resources:Tokens,ValidDate %>"></asp:CompareValidator>
            </div>
                </div></div>
                <div class="space-4"></div>
                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal6" runat="server" Text="<%$Resources:Tokens,Employee %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div class="editor-label">
                <asp:TextBox ID="tb_EmployeeName" runat="server" Width="200px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                    Display="Dynamic" ControlToValidate="tb_EmployeeName" ValidationGroup="save" Text="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
            </div>
                </div></div>
                <div class="space-4"></div>
                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal7" runat="server" Text="<%$Resources:Tokens,Address %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div class="editor-label">
                <asp:TextBox ID="tb_Address" runat="server" Width="200px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                    Display="Dynamic" ControlToValidate="tb_Address" ValidationGroup="save" Text="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
            </div>
                </div></div>
                <div class="space-4"></div>
                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal8" runat="server" Text="<%$Resources:Tokens,CallDate %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div class="editor-label">
                <asp:TextBox ID="tb_CallDate" runat="server" Width="200px" ClientIDMode="Static"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                    Display="Dynamic" ControlToValidate="tb_CallDate" ValidationGroup="save" Text="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="tb_CallDate"
                    Display="Dynamic" ErrorMessage="<%$Resources:Tokens,ValidDate %>" Operator="DataTypeCheck" Type="Date" Text="<%$Resources:Tokens,ValidDate %>"></asp:CompareValidator>
            </div>
                </div></div>
                <div class="space-4"></div>
                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal3" runat="server" Text="<%$Resources:Tokens,Topic %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div class="editor-label">
                <asp:TextBox ID="tb_Topic" runat="server" Width="350px" Height="60px" TextMode="MultiLine"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                    Display="Dynamic" ControlToValidate="tb_Topic" ValidationGroup="save" Text="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
            </div>
                </div></div>
                <div class="space-4"></div>
                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal4" runat="server" Text="<%$Resources:Tokens,Result %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div class="editor-label">
                <asp:TextBox ID="tb_Result" runat="server" Width="350px" Height="60px" TextMode="MultiLine"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                    Display="Dynamic" ControlToValidate="tb_Result" ValidationGroup="save" Text="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
            </div>
                </div></div>
                
                                <div class="clearfix form-actions">
										<div class="col-md-offset-3 col-md-9">
											<button id="Button2" class="btn btn-success" type="button" runat="server" OnServerClick="b_save_Click" ValidationGroup="save">
												<i class="icon-ok bigger-110"></i>
												<%=Tokens.Save %>
											</button>

											&nbsp; &nbsp; &nbsp;
											<button class="btn btn-primary" type="reset">
												<i class="icon-undo bigger-110"></i>
												<%=Tokens.reset %>
											</button>
                                                        <div id="message">
                <asp:Label runat="server" ID="l_message"></asp:Label>
            </div>
										</div>
									</div>
                                    </asp:Panel>          
            </div>
        </div>
    </div>
</asp:Content>
