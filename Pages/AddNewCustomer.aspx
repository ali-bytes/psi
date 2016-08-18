<%@ Page Title="<%$ Resources:Tokens,AddNewCustomer %>"  Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="AddNewCustomer.aspx.cs" Inherits="NewIspNL.Pages.AddNewCustomer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Import Namespace="Resources" %>
<%@ Register Src="../WebUserControls/UserFile.ascx" TagName="UserFile" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

<link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>

    <div>
        <div class="view well" id="tr_SimpleDetails" runat="server" visible="false">
            <fieldset>
            <div class="page-header">
							<h1>
                                 <asp:Literal ID="l_CustomerDetails" runat="server" Text="<%$ Resources:Tokens,Customer.Details %>"></asp:Literal>
							</h1>
						</div>
                
                <div class="row">
                
         <div class="col-xs-12 col-sm-4">
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

														<fieldset>
                                                            <div class="well">
                        <div>
                            <div>
                                <asp:Label ID="Label1" AssociatedControlID="txt_CustomerName" runat="server" Text="<%$ Resources:Tokens,Customer.Name %>"></asp:Label>
                                <div>
                                    <asp:TextBox ID="txt_CustomerName" runat="server" CssClass="required-input"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                        ControlToValidate="txt_CustomerName" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div>
                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Tokens,Customer.Phone %>"></asp:Label>
                                <div>
                                    <asp:UpdatePanel ID="up_phone" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox CssClass="required-input" ID="txt_CustomerPhone" ClientIDMode="Static" runat="server" AutoPostBack="True"
                                                OnTextChanged="txt_CustomerPhone_TextChanged" ValidationGroup="Insert"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                                                ControlToValidate="txt_CustomerPhone" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                            <asp:Label ForeColor="Red" ID="Literal1" runat="server" ClientIDMode="Static"></asp:Label>
                                            <asp:FilteredTextBoxExtender ID="txt_CustomerPhone_FilteredTextBoxExtender" runat="server"
                                                FilterType="Numbers" TargetControlID="txt_CustomerPhone" Enabled="True">
                                            </asp:FilteredTextBoxExtender>
                                            
                                        </ContentTemplate>
                                    </asp:UpdatePanel><% if (ActiveValidation)
                                               { %>
                                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txt_CustomerPhone" ValidationGroup="Insert"
                                             ErrorMessage="رقم التليفون يجب الا  يقل عن 8 ارقام" ValidationExpression="[0-9]{8,10}"  Display="Dynamic"></asp:RegularExpressionValidator><% } %>
                                </div>
                            </div>
                            <div>
                                <asp:Label ID="Label7" AssociatedControlID="ddl_Governorates" runat="server" Text="<%$ Resources:Tokens,Customer.City %>"></asp:Label>
                                <div>
                                    <asp:DropDownList ID="ddl_Governorates"  CssClass="required-input width-45 chosen" runat="server"
                                        DataTextField="GovernorateName" DataValueField="ID" AutoPostBack="True" OnSelectedIndexChanged="ddl_Governorates_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ControlToValidate="ddl_Governorates" ID="RequiredFieldValidator7"
                                        runat="server" ErrorMessage="*" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div>
                                <asp:Label AssociatedControlID="txt_CustomerAddress" ID="Label8" runat="server" Text="<%$ Resources:Tokens,Customer.Address %>"></asp:Label>
                                <div>
                                    <asp:TextBox CssClass="required-input" ID="txt_CustomerAddress" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                        ControlToValidate="txt_CustomerAddress" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div>
                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Tokens,Customer.Email %>"></asp:Label>
                                <div>
                                    <asp:TextBox ID="txt_CustomerEmail" CssClass="required-input" runat="server"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                        ControlToValidate="txt_CustomerEmail" ValidationGroup="Insert" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        Display="Dynamic"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                        ControlToValidate="txt_CustomerEmail" ValidationGroup="Insert" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div>
                                <label for="TbPersonnalId">
                                    <asp:Literal ID="Literal3" Text="<%$ Resources:Tokens,Id.Number %>" runat="server" /></label>
                                <div>
                                    <asp:TextBox ID="TbPersonnalId" runat="server" ClientIDMode="Static" CssClass="required-input"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                        ControlToValidate="TbPersonnalId" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                        </div>
                    </div>
														</fieldset>
                                                        
													</div>
												</div>
											</div>
										</div>
									</div>
         <div class="col-xs-12 col-sm-4">
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

														<fieldset>
                                                            <div>
                        <div class="well">

                            <div>
                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Tokens,Service.Provider%>"></asp:Label>
                                <div>
                                    <asp:DropDownList CssClass="required-input width-60 chosen" ID="ddl_ServiceProvider" runat="server"
                                        DataTextField="SPName" OnSelectedIndexChanged="ddl_ServiceProvider_SelectedIndexChanged"
                                        AutoPostBack="True" DataValueField="ID">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                        ControlToValidate="ddl_ServiceProvider" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div>
                                <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Tokens,Offer %>"></asp:Label>
                                <div>
                                    <asp:DropDownList ID="ddl_offer" CssClass="width-60 chosen" runat="server" AutoPostBack="True" ClientIDMode="Static"
                                        OnSelectedIndexChanged="ddl_offer_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div>
                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Tokens,Service.Package %>"></asp:Label>
                                <div>
                                    <asp:DropDownList CssClass="required-input width-60 chosen" ID="ddl_ServicePackage" runat="server"
                                        DataTextField="ServicePackageName" ClientIDMode="Static" DataValueField="ID">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                        ControlToValidate="ddl_ServicePackage" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            
                            <div>
                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Tokens,Central %>"></asp:Label>
                                <div>
                                    <asp:DropDownList ID="ddl_centrals" CssClass="required-input width-60 chosen" runat="server" DataTextField="GovernorateName"
                                        DataValueField="ID">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ControlToValidate="ddl_centrals" ID="RequiredFieldValidator20"
                                        runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div>
                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Tokens,Ip.Package %>"></asp:Label>
                                <div>
                                    <asp:DropDownList ID="ddl_IpPackage" CssClass="width-60 chosen" runat="server" DataTextField="IpPackageName"
                                        DataValueField="ID" Enabled="False">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*"
                                        ControlToValidate="ddl_IpPackage" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div>
                                <asp:Label ID="Label22" AssociatedControlID="TbCreationDate" runat="server" Text="<%$ Resources:Tokens,CreationDate %>"></asp:Label>
                                <div>
                                    <asp:TextBox ID="TbCreationDate" runat="server" data-x="dt"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                                ControlToValidate="TbCreationDate" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
														</fieldset>
                                                        
													</div>
												</div>
											</div>
										</div>
									</div>
         <div class="col-xs-12 col-sm-4">
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

														<fieldset>
                                                            <div class="well">
                        <div>
                            <div>
                                <asp:Label ID="Label30" AssociatedControlID="txt_CustomerMobile" runat="server" Text="<%$ Resources:Tokens,Customer.Mobile %>"></asp:Label>
                                <div>
                                    <asp:TextBox ID="txt_CustomerMobile" runat="server" CssClass="required-input"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                                                 FilterType="Numbers" TargetControlID="txt_CustomerMobile" Enabled="True">
                                    </asp:FilteredTextBoxExtender>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                        ControlToValidate="txt_CustomerMobile" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div>
                                <asp:Label ID="Label31" AssociatedControlID="txt_CustomerMobile2" runat="server" Text="<%$ Resources:Tokens,Customer.Mobile2 %>"></asp:Label>
                                <div>
                                    <asp:TextBox ID="txt_CustomerMobile2" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server"
                                        FilterType="Numbers" TargetControlID="txt_CustomerMobile2" Enabled="True">
                                    </asp:FilteredTextBoxExtender>
                                </div>
                            </div>
                            <div>
                                <label for="TbLineOwner">
                                    <asp:Literal ID="Literal2" Text="<%$Resources:Tokens,LineOwner %>" runat="server" />
                                </label>
                                <div>
                                    <asp:TextBox runat="server" ID="TbLineOwner" ClientIDMode="Static" CssClass="required-input"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                        ControlToValidate="TbLineOwner" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                                                        <div>
                                <asp:Label ID="Label32" AssociatedControlID="txt_Notes" runat="server" Text="<%$ Resources:Tokens,Notes %>"></asp:Label>
                                <div>
                                    <asp:TextBox ID="txt_Notes" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            
                        </div>
                    </div>
														</fieldset>
                                                        
													</div>
												</div>
											</div>
										</div>
									</div>           
                </div>
            </fieldset>
        </div>
        <div class="view" id="tr_FullDetails" runat="server" visible="false">
            <div class="well">
                <fieldset>
                    <div class="page-header">
                        <h1><asp:Literal ID="l_AdvancedDetails" runat="server" Text="<%$Resources:Tokens,AdvancedDetails %>"></asp:Literal></h1>
                    </div>
                    <div class="row">
                                 <div class="col-xs-12 col-sm-4">
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

														<fieldset>
                                                            <div>
                            <div class="well">
                                <div>
                                    <asp:Label ID="Label11"  runat="server" Text="<%$Resources:Tokens,Branch %>"></asp:Label>
                                    <div>
                                        <asp:DropDownList ID="ddl_Branch" CssClass="width-60 chosen" OnSelectedIndexChanged="BranchChanged" AutoPostBack="True" runat="server" Width="155px" DataTextField="BranchName"
                                                          DataValueField="ID">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                                    ControlToValidate="ddl_Branch" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div>
                                    <asp:Label ID="Label6" runat="server" Text="<%$Resources:Tokens,Reseller %>"></asp:Label>
                                    <div>
                                        <asp:DropDownList ID="ddl_Reseller" CssClass="width-60 chosen" runat="server" Width="155px" DataTextField="UserName"
                                            DataValueField="ID">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div>
                                    <asp:Label ID="Label17" runat="server" Text="<%$Resources:Tokens,Status %>"></asp:Label>
                                    <div>
                                        <asp:DropDownList ID="ddl_CustomerStatus" CssClass="width-60 chosen" runat="server" Width="155px" DataTextField="StatusName"
                                            DataValueField="ID">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                            ControlToValidate="ddl_CustomerStatus" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div>
                                    <label for="TbRouterSerail">
                                        <%= Tokens.RouterSerial %>
                                    </label>
                                    <div>
                                        <asp:TextBox runat="server" ID="TbRouterSerail" ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </div>
                                <div>
                                    <label for="TbRequestNumber">
                                        <%= Tokens.RequestNumber %>
                                    </label>
                                    <div>
                                        <asp:TextBox runat="server" ID="TbRequestNumber" ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </div>
                                <div>
                                    <label><%=Tokens.PortNumber %></label>
                                    <div>
                                        <asp:TextBox runat="server" ID="TbPortNumber" ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </div>
                                
                                                                
                            </div>
                        </div>
														</fieldset>
                                                        
													</div>
												</div>
											</div>
										</div>
									</div>
                                 <div class="col-xs-12 col-sm-4">
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

														<fieldset>
                                                             <div class="span4">
                            <div class="well">
                                <div>
                                    <asp:Label ID="Label18" runat="server" Text="<%$Resources:Tokens,UserName %>"></asp:Label>
                                    <div>
                                        <asp:TextBox ID="txt_UserName" runat="server" Width="150px"></asp:TextBox>
                                    </div>
                                </div>
                                <div>
                                    <asp:Label ID="Label14" runat="server" Text="<%$Resources:Tokens,Password %>"></asp:Label>
                                    <div>
                                        <asp:TextBox ID="txt_Password" runat="server" Width="150px"></asp:TextBox>
                                    </div>
                                </div>
                                <div>
                                    <asp:Label ID="Label12" runat="server" Text="<%$Resources:Tokens,VPI %>"></asp:Label>
                                    <div>
                                        <asp:TextBox ID="txt_VPI" runat="server" Width="150px"></asp:TextBox>
                                    </div>
                                </div>
                                <div>
                                    <asp:Label ID="Label13" runat="server" Text="<%$Resources:Tokens,VCI %>"></asp:Label>
                                    <div>
                                        <asp:TextBox ID="txt_VCI" runat="server" Width="150px"></asp:TextBox>
                                    </div>
                                </div>
                                <div>
                                    <asp:Label ID="Label16" runat="server" Text="<%$Resources:Tokens,WorkorderNumber %>"></asp:Label>
                                    <div>
                                        <asp:TextBox runat="server" ID="txt_WorkorderNumber" Width="150px"></asp:TextBox>
                                    </div>
                                </div>
                                <div>
                                    <asp:Label ID="Label23" runat="server" Text="<%$Resources:Tokens,WorkorderDate %>"></asp:Label>
                                    <div>
                                        <asp:TextBox runat="server" ID="txt_WorkorderDate" Width="150px" class="form-control" data-x="dt"></asp:TextBox>
                                    </div>
                                </div>
                                
                            </div>
                        </div>
														</fieldset>
                                                        
													</div>
												</div>
											</div>
										</div>
									</div>
                                 <div class="col-xs-12 col-sm-4">
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

														<fieldset>
                                                             <div class="span4">
                            <div class="well">
                                                            <div>
                                <asp:Label ID="Label19" runat="server" AssociatedControlID="ddl_PaymentType" Text="<%$ Resources:Tokens,PaymentType %>"></asp:Label>
                                <div>
                                    <asp:DropDownList CssClass="required-input width-45 chosen" ID="ddl_PaymentType" runat="server" DataTextField="PaymentTypeName"
                                        DataValueField="ID">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                        ControlToValidate="ddl_PaymentType" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                                <div>
                                    <label for="TbPrepaid">
                                        <asp:Literal Text="<%$Resources:Tokens,Prepaid %>" runat="server" />
                                    </label>
                                    <div>
                                        <asp:TextBox runat="server" ID="TbPrepaid" ClientIDMode="Static"></asp:TextBox>
                                        <asp:CompareValidator runat="server" ID="CVTbPrepaid" ErrorMessage="<%$Resources:Tokens,NumbersOnly %>" ControlToValidate="TbPrepaid" Type="Double" Operator="DataTypeCheck"></asp:CompareValidator>
                                    </div>
                                </div>
                                 <div>
                                    <label for="TbInstallationCost">
                                        <asp:Literal Text="<%$Resources:Tokens,InstallationCost %>" runat="server" />
                                        
                                    </label>
                                    <div>
                                        <asp:TextBox runat="server" ID="TbInstallationCost" ClientIDMode="Static"></asp:TextBox>
                                        <asp:CompareValidator 
                                            runat="server" ID="CVTbInstallationCost"  ErrorMessage="<%$Resources:Tokens,NumbersOnly %>"
                                            ControlToValidate="TbInstallationCost" 
                                            Type="Double" 
                                            Operator="DataTypeCheck">
                                        </asp:CompareValidator>
                                    </div>
                                       </div> 
                                       <div>
                                    <label for="TbContractingCost">
                                        <asp:Literal Text="<%$Resources:Tokens,ContractingCost %>" runat="server" />
                                    </label>
                                    <div>
                                        <asp:TextBox runat="server" ID="TbContractingCost" ClientIDMode="Static"></asp:TextBox>
                                        <asp:CompareValidator 
                                            runat="server" ID="CompareValidator1"  ErrorMessage="<%$Resources:Tokens,NumbersOnly %>"
                                            ControlToValidate="TbContractingCost" 
                                            Type="Double" 
                                            Operator="DataTypeCheck">
                                        </asp:CompareValidator>
                                    </div>
                                       </div> 
                                <div>
                                    <label><%=Tokens.Dslam %></label>
                                    <div>
                                        <asp:TextBox runat="server" ID="TbDslamNumber" ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </div>
                                <div>
                                    <label><%=Tokens.Block %></label>
                                    <div>
                                        <asp:TextBox runat="server" ID="txt_BlockNumber" ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
														</fieldset>
                                                        
													</div>
												</div>
											</div>
										</div>
									</div>
                        
                    </div>
                </fieldset>
            </div>
        </div>
        <div class="view">
            <div class="row-fluid">
                <div class="span12">
                    <uc1:UserFile ID="UserFile1" runat="server" CanEdit="True" />
                </div>
            </div>
        </div>
    </div>
    <div>
        <p>
        <asp:LinkButton CssClass="btn btn-primary btn-block" ID="btn_Add" runat="server" ValidationGroup="Insert" ClientIDMode="Static"
       OnClick="btn_Add_Click" style="width: 70%;margin: 5% 16%;"><i class="icon-ok"></i> &nbsp; <%=Tokens.AddCustomer %></asp:LinkButton>
        
            <%--<asp:Button ID="" runat="server" Text="<%$Resources:Tokens,AddCustomer %>"
                ValidationGroup="" OnClick="" CssClass="" />--%>
            <span>
                <asp:Label ID="lbl_InsertResult" runat="server" EnableViewState="False" Font-Bold="True"></asp:Label></span>
        </p>
    </div>
    <input type="hidden" value="" runat="server" id="packs" clientidmode="Static" />

   <script src="../Content/ace-assest/js/jquery-ui-1.10.3.full.min.js" type="text/javascript"></script>
     
    <script type="text/javascript">
        $(document).ready(function () {
            //$(".chosen-select").chosen();
            $('input[data-x="dt"]').datepicker({
                showOtherMonths: true,
                selectOtherMonths: false,
                dateFormat: 'dd/mm/yy'
            });
            // $('input[data-x="dt"]').datepicker({ dateFormat: 'dd/mm/yy' });
            // $('#TbOfferStart').datepicker({ dateFormat: 'dd/mm/yy' });
            var packs = $('#packs').val();
            if (packs === "show" && !$('#ddl_ServicePackage option:first').is(':selected') && !$('#ddl_offer option:first').is(':selected')) {
                alert('<%= Tokens.SelectedPackageIsNotInOffer %>');
                $('#ddl_ServicePackage').focus();
            }
            //            var phonenum = $("#txt_CustomerPhone").length;
            //           if (phonenum < 8) {
            //               alert("رقم التليفون يجب الا  يقل عن 8 ارقام");
            //               $("#txt_CustomerPhone").focus();
            //           }
            //           $("#btn_Add").click(function() {
            ////               var labl = $("#Literal1").attr("color");
            ////                if (labl==="#F00" || labl==="red"){
            ////                   alert("Error");
            ////                  $("#txt_CustomerPhone").focus();
            ////                    return false;
            ////                }
            ////               return true;
            //           });
        });

        jQuery(document).ready(function () {
            jQuery(".chosen").chosen();
        });

    </script>
    <style type="text/css">
           fieldset div div.well
           {
               border-bottom: white;
               }
    </style>
</asp:Content>
