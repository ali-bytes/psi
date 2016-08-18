<%@ Page Title="<%$Resources:Tokens,ManageResellerRequests %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ManageResellerRequests.aspx.cs" Inherits="NewIspNL.Pages.ManageResellerRequests" %>
<%@ Import Namespace="Resources" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
            <div class="page-header">
							<h1><asp:Label ID="lblTitle" Text="<%$Resources:Tokens,ManageResellerRequests %>" runat="server"></asp:Label>
							</h1>
						</div>
                        <div class="col-md-12">
                            <div class="alert alert-success" style="display: none" id="divSuccess"><asp:Label runat="server" ID="lblSusc" Text="<%$Resources:Tokens,ProcessDone %>"></asp:Label></div>
                            <div class="alert alert-danger" style="display: none" id="divError"><asp:Label runat="server" ID="Label1" Text="<%$Resources:Tokens,ErrorMsg %>"></asp:Label></div>
                           <div class="alert alert-danger" style="display: none" id="divErroruserexist"><asp:Label runat="server" ID="Label3" Text="<%$Resources:Tokens,UserNameAlreadyExist %>"></asp:Label></div>
                             <asp:HiddenField runat="server" ID="hdfMsg" ClientIDMode="Static"/>
                         
                          
                       </div>
     <div id="all-result-items" style="overflow-y: auto; width: 100%" >
                              <asp:GridView ID="grd_Requests" runat="server" ClientIDMode="Static" AutoGenerateColumns="False"
                    CssClass="table table-bordered table-condensed text-center" style="margin-right: 0px;" DataKeyNames="Id" Wrap="true"
                     OnRowDataBound="grd_Requests_RowDataBound" >
                                  
                    <Columns >
                        <asp:TemplateField HeaderText="#" >
                            <ItemTemplate >
                                <asp:Label ID="lbl_No" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CompanyArabicName" HeaderText="<%$ Resources:Tokens,Company %>" />
                           
                        <asp:BoundField DataField="CompanyEnglishName" HeaderText="<%$ Resources:Tokens,EnglishCompanyName %>"/>
                        <asp:BoundField DataField="CompanyType" HeaderText="<%$ Resources:Tokens,CompanyType %>" />
                        <asp:BoundField DataField="EmployeeNumbers" HeaderText="<%$ Resources:Tokens,EmployeeNumbers%>" />
                        <asp:BoundField DataField="CompanyActivities" HeaderText="<%$ Resources:Tokens,CompanyActivites %>"/>
                        <asp:BoundField DataField="CompanyAddress" HeaderText="<%$ Resources:Tokens,Address%>"/>
                        <asp:BoundField DataField="CompanyTelephone" HeaderText="<%$ Resources:Tokens,Phone %>"/>
                        <asp:BoundField DataField="FaxNumber" HeaderText="<%$ Resources:Tokens,Fax %>"/>
                        <asp:BoundField DataField="CompanyEmail" HeaderText="<%$ Resources:Tokens,CompanyEmail %>"/>
                        <asp:BoundField DataField="CompanyMobile" HeaderText="<%$Resources:Tokens,CompanyMobile %>"/>
                        <asp:BoundField DataField="ResellerName" HeaderText="<%$ Resources:Tokens,Reseller %>" />
                        <asp:BoundField DataField="ResellerNationalNumber" HeaderText="<%$ Resources:Tokens,NationalId %>" />
                        <asp:BoundField DataField="ResellerMobile" HeaderText="<%$ Resources:Tokens,Mobile%>"  />
                        <asp:BoundField DataField="ResellerEmail" HeaderText="<%$ Resources:Tokens,Email %>"/>
                        <asp:BoundField DataField="ResellerUsername" HeaderText="<%$ Resources:Tokens,UserName%>"/>
                        <asp:BoundField DataField="ResellerPassword" HeaderText="<%$Resources:Tokens,Password %>"/>
                     
                        <asp:TemplateField >
                            <ItemTemplate > 
                                 <button type="button" id="btnApprove" data-pop="1" data-id='<%#Eval("Id") %>'  data-rel="tooltip" class="btn btn-success btn-xs" title="<%=Tokens.Confirm %>"><i class="icon-white icon-ok bigger-120"></i></button>
                                
                                  <asp:LinkButton CssClass="btn btn-danger btn-xs" data-rel="tooltip" ToolTip="<%$Resources:Tokens,Reject %>" OnClick="RejectRequest"
                                 runat="server" ID="lnkReject" CommandArgument='<%#Eval("Id") %>'><i class="icon-white icon-remove bigger-120"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Red" Text="<%$ Resources:Tokens,NoRequests%>"></asp:Label>
                    </EmptyDataTemplate>
                </asp:GridView>
                           </div> 
                       
                        <div id="approveResellerModal" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog">
        <div class="modal-content">
        <div class="modal-header">
            <button type="button" class="bootbox-close-button close" data-dismiss="modal">
                ×</button>
            <h4 id="myModalLabel">
                <%=Tokens.Approve %></h4>
        </div>
        <div class="modal-body">
                         <asp:HiddenField runat="server" ClientIDMode="Static" ID="hdf_ID"/>
        <div class="bootbox-body container">
        <asp:UpdatePanel runat="server" ID="updatePanel">
            <ContentTemplate>
                
            <div class="col-md-6">
                <div>
                    <label for="TbDaysCount">
                        <%= Tokens.Group %></label>
                    <div>
                                        <asp:DropDownList ID="ddl_Groups" runat="server"  DataTextField="GroupName"
                                            DataValueField="ID" onselectedindexchanged="ddl_Groups_SelectedIndexChanged1">
                                        </asp:DropDownList>
                                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ValidationGroup="ded" ControlToValidate="ddl_Branches"
                            ErrorMessage="*">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
                <div runat="server" id="RowAccountManager">
                    <label for="TbDaysCount">
                        <%= Tokens.ResellerAccountManager %></label>
                    <div>
                        <asp:DropDownList runat="server" ID="ddlAccountManager" Width="155px"/>
                        <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="ddlAccountManager" ID="RequiredFieldValidator1"
                            ValidationGroup="ded" runat="server" />
                    </div>
                </div>
                <div>
                    <label for="TbUnsuspendDate">
                        <%= Tokens.Branch %>
                    </label>
                    <div>
                                                                <asp:DropDownList ID="ddl_Branches" runat="server" DataTextField="BranchName"
                                            DataValueField="ID">
                                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="sdffsder" ValidationGroup="ded" ControlToValidate="ddl_Branches"
                            ErrorMessage="*">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>                
                
                 <%--<div>
                    <label for="TbDaysCount">
                        <%= Tokens.sav %></label>
                    <div>
                                        <asp:CheckBoxList CssClass="checkbox" runat="server" ID="chlSaves" DataTextField="SaveName" DataValueField="Id">
                                        </asp:CheckBoxList>
                    </div>--%>
                </div>
            </div>
            <div class="col-md-6">

                <div>
                    <label for="TbUnsuspendDate">
                        <%= Tokens.Governorate %>
                    </label>
                    <div>
<asp:DropDownList runat="server" ID="ddlGovernorate" Width="155px"/>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                            ValidationGroup="ded" ControlToValidate="ddlGovernorate"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div>
                    <label for="TbUnsuspendDate">
                        <%= Tokens.privilegofManageRequest %>
                    </label>
                    <div>

                                       <asp:DropDownList runat="server" ID="ddlManageReuestPrivilege" Width="155px" CssClass="dropdown">
                                           <asp:ListItem Value="0" Text="<%$Resources:Tokens,All %>"></asp:ListItem>
                                           <asp:ListItem Value="1" Text="<%$Resources:Tokens,Resellers %>"></asp:ListItem>
                                           <asp:ListItem Value="2" Text="<%$Resources:Tokens,DirectUsersOnly %>"></asp:ListItem>
                                       </asp:DropDownList>
                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                            ValidationGroup="ded" ControlToValidate="ddlManageReuestPrivilege"></asp:RequiredFieldValidator>
                    </div>
                </div>
                 <div>
                    <label for="TbUnsuspendDate">
                        <%= Tokens.AccountIsStopped %>
                    </label>
                    <div>
                        <asp:CheckBox ID="chb_IsAccountStopped" runat="server" />
                    </div>
                </div>
               <%-- <div>
                    <label><%=Tokens.UsersPageNotes %></label>
                    <div>
                                                                <asp:CheckBoxList ID="chl_Branchs" runat="server" DataTextField="BranchName" DataValueField="ID">
                                        </asp:CheckBoxList>
                    </div>
                </div>--%>
            </div>
                        </ContentTemplate>
        </asp:UpdatePanel>
            </div>
        </div>
        <div class="modal-footer">
                        <div id="unsuspend-result">
            </div>

                        <button class="btn btn-primary" type="button" runat="server" id="btnApproveModal"
                                OnServerClick="ApproveRequest" validationgroup="ded" data-bb-handler="confirm">
                            <%=Tokens.Approve %></button>

                        <button class="btn btn-default" type="button" data-dismiss="modal" data-bb-handler="cancel">
                            <%=Tokens.Cancel %></button>
        </div>
        </div>
        </div>
    </div>
                      
                        <script type="text/javascript">
                            $(function () {
                                if ($('#grd_Requests').width() > 1058) {
                                    $('#grd_Requests').css({
                                        //"font-family": "serif",
                                        "font-size": "10px"
                                    });
                                }
                                var div = $('#hdfMsg').val();
                                if (div === "1") {
                                    $('#divSuccess').show();
                                    $('#divError').hide();
                                } else if (div === "0") {
                                    $('#divError').show();
                                    $('#divSuccess').hide();
                                } else if (div === "2") {
                                    $('#divSuccess').hide();
                                    $('#divError').hide();
                                    $('#divErroruserexist').show();
                                }
                            });
                            $("button[data-pop]").click(function () {
                                var id = $(this).attr('data-id');
                                $('#hdf_ID').val(id);
                                $('#approveResellerModal').modal('show');
                            });
                        </script>
</asp:Content>


