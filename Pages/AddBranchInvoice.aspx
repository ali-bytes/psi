<%@ Page Title="<%$Resources:Tokens,AddBranchInvoice %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="AddBranchInvoice.aspx.cs" Inherits="NewIspNL.Pages.AddBranchInvoice" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="col-xs-12 col-sm-7" style="margin: 0 22%;">
										<div class="widget-box">
											<div class="widget-header">
												<h4><%=Tokens.AddBranchInvoice%></h4>
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
             <asp:FileUpload runat="server" multiple="" ID="fu_sheet" />
            <asp:RequiredFieldValidator runat="server" ID="r_sheet" ValidationGroup="inv"
                 ControlToValidate="fu_sheet" ErrorMessage="<%$ Resources:Tokens,Required %>" Display="Dynamic"></asp:RequiredFieldValidator>
            
                                                            <asp:RegularExpressionValidator runat="server" ForeColor="red" ValidationGroup="inv"  ValidationExpression="^.*\.(xls|XLS|xlsx|XLSX|)$" ControlToValidate="fu_sheet"  ErrorMessage="<%$Resources:Tokens,uploadfilexcelerror %>"></asp:RegularExpressionValidator>
             
                                                            								</fieldset>
                                                        
														<div class="form-actions center">
                                                                 <asp:LinkButton  runat="server" ID="bSave" class="btn btn-primary" ValidationGroup="inv" OnClick="btnAdd_Click"><i class="icon-ok bigger-110"></i>&nbsp;<%=Tokens.Save %></asp:LinkButton>
                                                                                         <a href="../ExcelTemplates/BranchesInvoices.xls" class="btn btn-default">
                                                                                                <i class="icon-cloud-download bigger-120"></i>
                                                                                                 <%=Tokens.Downloadsample %>
                                                                                         </a>
                                                                                         
                                                                 <asp:Label runat="server" ID="l_message" Text=""></asp:Label>
														</div>
													</div>
												</div>
											</div>
										</div>
									</div>
</asp:Content>

