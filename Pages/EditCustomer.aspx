<%@ Page Title="<%$Resources:Tokens,EditCustomer %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="EditCustomer.aspx.cs" Inherits="NewIspNL.Pages.EditCustomer" %>

<%@ Import Namespace="Resources" %>
<%@ Register Src="../WebUserControls/UserFile.ascx" TagName="UserFile" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view">
        <fieldset>
            <div class="page-header">
               <h1> <asp:Literal ID="Label20" runat="server" Text="<%$Resources:Tokens,Customer.Details %>"></asp:Literal></h1>
            </div>
            <div class="row">
                
                <div class="col-xs-12 col-sm-6">
										<div class="widget-box">
											<div class="widget-header">
                                                	<span class="widget-toolbar">
														<a href="#" data-action="collapse">
															<i class="icon-chevron-up"></i>
														</a>
                                                        														<a href="#" data-action="reload">
															<i class="icon-refresh"></i>
														</a>
														<a href="#" data-action="close">
															<i class="icon-remove"></i>
														</a>
													</span>
											</div>

											<div class="widget-body">
												<div class="widget-main no-padding">
													<div>
														<!-- <legend>Form</legend> -->
                                                        <div class="well">
                        <div>
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Tokens,Customer.Name %>"></asp:Label>
                            <div>
                                <asp:TextBox ID="txt_CustomerName" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                    Text="<%$Resources:Tokens,Required %>" ControlToValidate="txt_CustomerName" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="Label2" runat="server" Text="<%$Resources:Tokens,Customer.Phone %>"></asp:Label>
                            <div>
                                <asp:TextBox ID="txt_CustomerPhone" runat="server" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                    Text="<%$Resources:Tokens,Required %>" ControlToValidate="txt_CustomerPhone"
                                    ValidationGroup="Insert"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="Label7" runat="server" Text="<%$Resources:Tokens,Governrate %>"></asp:Label>
                            <div>
                                <asp:DropDownList ID="ddl_Governorates" runat="server" DataTextField="GovernorateName"
                                    DataValueField="ID" AutoPostBack="True" OnSelectedIndexChanged="ddl_Governorates_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ControlToValidate="ddl_Governorates" ID="RequiredFieldValidator7"
                                    runat="server" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="Label3" runat="server" Text="<%$Resources:Tokens,Mobile %>"></asp:Label>
                            <div>
                                <asp:TextBox ID="txt_CustomerMobile" runat="server" ></asp:TextBox>
                            </div>
                        </div>
                         <div>
                            <asp:Label ID="Label28" runat="server" Text="<%$Resources:Tokens,Mobile2 %>"></asp:Label>
                            <div>
                                <asp:TextBox ID="txt_mobile2" runat="server" ></asp:TextBox>
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="Label8" runat="server" Text="<%$Resources:Tokens,Customer.Address %>"></asp:Label>
                            <div>
                                <asp:TextBox ID="txt_CustomerAddress" runat="server" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                    Text="<%$Resources:Tokens,Required %>" ControlToValidate="txt_CustomerAddress"
                                    ValidationGroup="Insert"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="Label9" runat="server" Text="<%$Resources:Tokens,Customer.Email %>"></asp:Label>
                            <div>
                                <asp:TextBox ID="txt_CustomerEmail" runat="server"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="<%$Resources:Tokens,InvalidMail %>"
                                    Text="<%$Resources:Tokens,InvalidMail %>" ControlToValidate="txt_CustomerEmail"
                                    ValidationGroup="Insert" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    Display="Dynamic"></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="*"
                                    ControlToValidate="txt_CustomerEmail" ValidationGroup="Insert" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <label>
                                <asp:Literal Text="<%$Resources:Tokens,IdNumber %>" runat="server" />
                            </label>
                            <div>
                                <asp:TextBox ID="TbPersonnalId" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div>
                            <label for="TbLineOwner">
                                <asp:Literal Text="<%$Resources:Tokens,LineOwner %>" runat="server" />
                            </label>
                            <div>
                                <asp:TextBox runat="server" ID="TbLineOwner" ClientIDMode="Static"></asp:TextBox>
                                
                            </div>
                        </div> 
                    </div>
													</div>
												</div>
											</div>
										</div>
									</div>
                
                <div class="col-xs-12 col-sm-6">
										<div class="widget-box">
											<div class="widget-header">
                                                	<span class="widget-toolbar">
														<a href="#" data-action="collapse">
															<i class="icon-chevron-up"></i>
														</a>
                                                        														<a href="#" data-action="reload">
															<i class="icon-refresh"></i>
														</a>
														<a href="#" data-action="close">
															<i class="icon-remove"></i>
														</a>
													</span>
											</div>

											<div class="widget-body">
												<div class="widget-main no-padding">
													<div>
														<!-- <legend>Form</legend> --><div class="well">
                        <div>
                            <asp:Label ID="Label19" runat="server" Text="<%$Resources:Tokens,PaymentType %>"></asp:Label>
                            <div>
                                <asp:DropDownList ID="ddl_PaymentType" runat="server" DataTextField="PaymentTypeName"
                                    DataValueField="ID">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                    Text="<%$Resources:Tokens,Required %>" ControlToValidate="ddl_PaymentType" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="Label4" runat="server" Text="<%$Resources:Tokens,Provider %>"></asp:Label>
                            <div>
                                <asp:DropDownList ID="ddl_ServiceProvider" runat="server" 
                                    AutoPostBack="True" OnSelectedIndexChanged="ddl_ServiceProvider_SelectedIndexChanged">
                                </asp:DropDownList><%--DataValueField="ID" DataTextField="SPName"--%>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                    Text="<%$Resources:Tokens,Required %>" ControlToValidate="ddl_ServiceProvider"
                                    ValidationGroup="Insert"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="Label10" runat="server" Text="<%$Resources:Tokens,Service.Package %>"></asp:Label>
                            <div>
                                <asp:DropDownList ID="ddl_ServicePackage" runat="server" DataTextField="ServicePackageName"
                                    DataValueField="ID" AutoPostBack="True" 
                                    onselectedindexchanged="ddl_ServicePackage_SelectedIndexChanged" 
                                    ClientIDMode="Static">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                    ControlToValidate="ddl_ServicePackage" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="Label22" runat="server" Text="<%$Resources:Tokens,Central %>"></asp:Label>
                            <div>
                                <asp:DropDownList ID="ddl_central" runat="server" DataTextField="GovernorateName"
                                    DataValueField="ID">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                    ControlToValidate="ddl_central" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="Label5" runat="server" Text="<%$Resources:Tokens,Ip.Package %>"></asp:Label>
                            <div>
                                <asp:DropDownList ID="ddl_IpPackage" runat="server" DataTextField="IpPackageName"
                                    DataValueField="ID">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                    ControlToValidate="ddl_IpPackage" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="Label23" runat="server" Text="<%$Resources:Tokens,Offer %>"></asp:Label>
                            <div>
                                <asp:DropDownList ID="ddl_offers" runat="server" DataTextField="GovernorateName"
                                    DataValueField="ID" AutoPostBack="True" 
                                    onselectedindexchanged="ddl_ServicePackage_SelectedIndexChanged" 
                                    ClientIDMode="Static">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div>
                            <label>
                                <%= Tokens.RouterSerial %></label>
                            <div>
                                <asp:TextBox runat="server" ID="TbRouterSerail" ClientIDMode="Static"></asp:TextBox>
                            </div>
                        </div>
                        <div>
                            <label runat="server">
                                <%= Tokens.RequestNumber %></label>
                            <div>
                                <asp:TextBox runat="server" ID="TbRequestNumber" ClientIDMode="Static"></asp:TextBox>
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="Label24" runat="server" Text="<%$Resources:Tokens,OfferStartDate %>"></asp:Label>
                            <div>
                                <asp:TextBox runat="server" ID="LOfferStart" data-select="dp"></asp:TextBox>
                            </div>
                        </div>

                         
                    </div>
													</div>
												</div>
											</div>
										</div>
									</div>
                
            </div>
        </fieldset>
    </div>
    <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <asp:Literal ID="Label21" runat="server" Text="<%$Resources:Tokens,AdvancedDetails %>"></asp:Literal>
            </h3>
            <div class="row-fluid">
                <div class="col-xs-12 col-sm-6">
										<div class="widget-box">
											<div class="widget-header">
                                                	<span class="widget-toolbar">
														<a href="#" data-action="collapse">
															<i class="icon-chevron-up"></i>
														</a>
                                                        														<a href="#" data-action="reload">
															<i class="icon-refresh"></i>
														</a>
														<a href="#" data-action="close">
															<i class="icon-remove"></i>
														</a>
													</span>
											</div>

											<div class="widget-body">
												<div class="widget-main no-padding">
													<div>
														<!-- <legend>Form</legend> -->
                                                        <div class="well">
                        <div>
                            <asp:Label ID="Label6" runat="server" Text="<%$Resources:Tokens,Reseller %>"></asp:Label>
                            <div>
                                <asp:DropDownList ID="ddl_Reseller" runat="server" DataTextField="UserName" DataValueField="ID">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                    ControlToValidate="ddl_Reseller" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="Label17" runat="server" Text="<%$Resources:Tokens,Customer.Status %>"></asp:Label>
                            <div>
                                <asp:DropDownList ID="ddl_CustomerStatus" runat="server" DataTextField="StatusName"
                                    DataValueField="ID">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="Label11" runat="server" Text="<%$Resources:Tokens,Branch %>"></asp:Label>
                            <div>
                                <asp:DropDownList ID="ddl_Branch" runat="server" DataTextField="BranchName" DataValueField="ID">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                    ControlToValidate="ddl_Branch" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="Label15" runat="server" Text="<%$Resources:Tokens,Notes %>"></asp:Label>
                            <div>
                                <asp:TextBox ID="txt_Notes" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="Label26" runat="server" Text="<%$Resources:Tokens,WorkorderNumber %>"></asp:Label>
                            <div>
                                <asp:TextBox ID="txt_WorkorderNumber" runat="server" ></asp:TextBox>
                            </div>
                        </div>
                                                <div>
                            <asp:Label ID="Label27" runat="server" Text="<%$Resources:Tokens,WorkorderDate %>"></asp:Label>
                            <div>
                                <asp:TextBox ID="txt_WorkorderDate" runat="server" data-select="dp"></asp:TextBox>
                            </div>
                        </div>

                    </div>
													</div>
												</div>
											</div>
										</div>
									</div>
                <div class="col-xs-12 col-sm-6">
										<div class="widget-box">
											<div class="widget-header">
                                                	<span class="widget-toolbar">
														<a href="#" data-action="collapse">
															<i class="icon-chevron-up"></i>
														</a>
                                                        														<a href="#" data-action="reload">
															<i class="icon-refresh"></i>
														</a>
														<a href="#" data-action="close">
															<i class="icon-remove"></i>
														</a>
													</span>
											</div>

											<div class="widget-body">
												<div class="widget-main no-padding">
													<div>
														<!-- <legend>Form</legend> --><div class="well">
                        <div>
                            <asp:Label ID="Label12" runat="server" Text="<%$Resources:Tokens,VPI %>"></asp:Label>
                            <div>
                                <asp:TextBox ID="txt_VPI" runat="server" ></asp:TextBox>
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="Label13" runat="server" Text="<%$Resources:Tokens,VCI %>"></asp:Label>
                            <div>
                                <asp:TextBox ID="txt_VCI" runat="server" ></asp:TextBox>
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="Label18" runat="server" Text="<%$Resources:Tokens,UserName %>"></asp:Label>
                            <div>
                                <asp:TextBox ID="txt_UserName" runat="server" ></asp:TextBox>
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="Label14" runat="server" Text="<%$Resources:Tokens,Password %>"></asp:Label>
                            <div>
                                <asp:TextBox ID="txt_Password" runat="server" ></asp:TextBox>
                            </div>
                        </div>
                        <div>
                            <asp:Label runat="server" Text="<%$Resources:Tokens,PortNumber %>"></asp:Label>
                            <div>
                                <asp:TextBox runat="server" ID="txt_PortNumber"></asp:TextBox>
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="Label16" runat="server" Text="<%$Resources:Tokens,Block %>"></asp:Label>
                            <div>
                                <asp:TextBox runat="server" ID="txt_BlockNumber"></asp:TextBox>
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="Label25" runat="server" Text="<%$Resources:Tokens,Dslam %>"></asp:Label>
                            <div>
                                <asp:TextBox runat="server" ID="txt_DslamNumber"></asp:TextBox>
                            </div>
                        </div>
                    </div>
													</div>
												</div>
											</div>
										</div>
									</div>
            </div>
        </fieldset>
    </div>
    <div class="view">
        <uc1:UserFile ID="UserFile1" runat="server" />
    </div>

    <div style="text-align: center">
        <asp:LinkButton ID="btn_Update" CssClass="btn btn-primary" runat="server"
            ValidationGroup="Insert" OnClick="btn_Update_Click" Width="100%"><i class="icon-ok icon-only bigger-130"></i>&nbsp;&nbsp;<%=Tokens.UpdateCustomer %></asp:LinkButton>
<%--        &nbsp;
        <% if(CanDelete){ %>
        <asp:LinkButton CssClass="btn btn-danger" ID="btn_Delete" OnClientClick="return areyousure()" runat="server" Text="<%$Resources:Tokens,Delete %>"
            ValidationGroup="Insert" OnClick="btn_Delete_Click" CausesValidation="False"
            Enabled="False" Width="48%"><i class="icon-trash icon-only bigger-130"></i>&nbsp;&nbsp;<%=Tokens.Delete %></asp:LinkButton><% } %>--%>
    </div>
    <p>
        <asp:Label ID="lbl_InsertResult" runat="server" EnableViewState="False" Font-Bold="True"></asp:Label>
    </p>
    <input type="hidden"  value="" runat="server" ID="packs" ClientIDMode="Static"/>
  
    <script type="text/javascript">

        $('input[data-select="dp"]').datepicker({ dateFormat: 'dd/mm/yy' });
        var packs = $('#packs').val();
        if (packs === "show" && !$('#ddl_ServicePackage option:first').is(':selected') && !$('#ddl_offers option:first').is(':selected')) {
            alert('<%= Tokens.SelectedPackageIsNotInOffer %>');
            $('#ddl_ServicePackage').focus();
        }
        function areyousure() {
            return confirm('<%= Tokens.AlertRUS %>');
        }
        jQuery(document).ready(function () {
            $(document).keypress(function (e) {
                var key = e.which;
                if (key === 13) {
                  
                    return false;
                } else {
                    return true;
                }
            });

            jQuery(".chosen").chosen();
            $('input[data-calender="db"]').datepicker({ dateFormat: 'dd-mm-yy' });
        });
    </script>

    <style type="text/css">
        .widget-main .well {
            margin-bottom: 0;
        }
    </style>
</asp:Content>
