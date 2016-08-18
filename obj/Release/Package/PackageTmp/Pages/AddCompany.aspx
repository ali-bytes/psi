<%@ Page Title="<%$Resources:Tokens,AddCompany %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="AddCompany.aspx.cs" Inherits="NewIspNL.Pages.AddCompany" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server"> 

            <style type="text/css">
        img[data-select="offer-image"]
        {
            max-width: 40px;
        }
    </style>
        <div class="alert alert-danger" runat="server" ID="MsgError">
    </div>
    <div class="alert alert-success" runat="server" ID="MsgSuccess"></div>
    <div class="row">
    
    <div class="view" data-select="0">
        <fieldset>
        <div class="page-header">
							<h1>
<asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,AddCompany %>"></asp:Literal>
							</h1>
						</div>
            <div>
                
                <div class="table-responsive">
                <asp:GridView runat="server" ID="GvCompany" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-responsive">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LNo" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CompanyName" HeaderText="<%$Resources:Tokens,Company %>" />
                        <asp:BoundField DataField="ServiceFees" HeaderText="<%$Resources:Tokens,ServiceFees %>" />
                        <asp:BoundField DataField="CommissionResellerOrBranch" HeaderText="<%$Resources:Tokens,CommissionResellerOrBranch %>" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Image data-select="offer-image" runat="server" ID="Img" ImageUrl='<%#Eval("CompanyImageUrl") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div style="width: 150px">
                                    <button validationgroup='<%#Eval("Id") %>' runat="server" id="BEdit" class="btn btn-xs btn-primary"
                                            onserverclick="EditEvent" data-rel="tooltip" title='<%$Resources:Tokens,Edit %>'>
                                       <i class="icon-edit bigger-120"></i></button>&nbsp;
                                    <button validationgroup='<%#Eval("Id") %>' runat="server" id="BDelete" class="btn btn-xs btn-danger"
                                            onserverclick="DeleteEvent" data-rel="tooltip" title='<%$Resources:Tokens,Delete %>'>
                                        <i class="icon-trash bigger-120"></i></button>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </div>
                <div style="text-align: center;">
                    <button type="button" runat="server" style="width: 57%;" OnServerClick="Newdata" ID="BAdd" CausesValidation="False" class="btn btn-success btn-block">
                        <%= Tokens.Add %></button><br />
                </div>
            </div>
        </fieldset>
    </div>
    <div class="view" data-select="1">
    <div class="col-xs-12 col-sm-4" style="margin: 0 35%;">
										<div class="widget-box">
											<div class="widget-header">
												<h4><%= Tokens.Details %></h4>
                                                													<span class="widget-toolbar">
														<a href="#" data-action="collapse">
															<i class="icon-chevron-up"></i>
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

														<fieldset style="padding:16px;"><div>
                            <label for="txtCompanyName">
                                <asp:Literal ID="Literal2" Text="<%$Resources:Tokens,Company %>" runat="server" />
                            </label>
                            <div>
                                <asp:TextBox runat="server" ID="txtCompanyName"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqName" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                            ControlToValidate="txtCompanyName" runat="server" />
                            </div>
                        </div>
                        <div>

                            <label for="txtServiceFees">
                                <asp:Literal ID="Literal3" Text="<%$Resources:Tokens,ServiceFees %>" runat="server" />
                            </label>
                            <div>
                                
                            <asp:TextBox runat="server" ID="txtServiceFees"></asp:TextBox>
                                <%--<asp:RequiredFieldValidator ID="RTbDetails" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                            ControlToValidate="txtServiceFees" runat="server" />--%>
                                    
                            </div>
                        </div>
                        <div>

                            <label for="txtCommission">
                                <asp:Literal ID="Literal5" Text="<%$Resources:Tokens,CommissionResellerOrBranch %>" runat="server" />
                            </label>
                            <div>
                                
                            <asp:TextBox runat="server" ID="txtCommission"></asp:TextBox>
                                <%--<asp:RequiredFieldValidator ID="RTbDetails" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                            ControlToValidate="txtCommission" runat="server" />--%>
                                    
                            </div>
                        </div>
                        <div>
                            <label for="FuImag">
                                <asp:Literal ID="Literal4" Text="<%$Resources:Tokens,Upload %>" runat="server" />
                            </label>
                            <div>
                                <asp:FileUpload runat="server"  Width="100px" ID="FuImag"></asp:FileUpload>
                               <asp:RegularExpressionValidator runat="server" ForeColor="red" ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|png|PNG|)$" ControlToValidate="FuImag"  ErrorMessage="<%$Resources:Tokens,uploadimgerror %>"></asp:RegularExpressionValidator>

                            </div><!--CssClass="file-label"-->
                        </div>
														</fieldset>
                                                        <div class="span6">
                        <div>
                            <asp:Image runat="server" ID="ImgOffer" style="max-width: 323px;"/>
                        </div>
                    </div>
														<div class="form-actions center">
                                                        <button class="btn btn-primary" runat="server" ID="BSave" ><i class="icon-arrow-right icon-on-right bigger-110"></i>&nbsp;<%= Tokens.Save %></button> &nbsp;
                                                         <button class="btn btn-danger" CausesValidation="False" runat="server" ID="bCancel"><%= Tokens.Cancel %></button>
														</div>
													</div>
												</div>
											</div>
										</div>
									</div>
    </div>
    </div>
    
    <input type="hidden" id="flag" runat="server" clientidmode="Static"/>
    <input type="hidden" id="selected" runat="server" clientidmode="Static"/>
       <script type="text/javascript">
           $(document).ready(function () {
               var $preview = $('div[data-select="0"]');
               var $edit = $('div[data-select="1"]');
               var flag = $('#flag').val();
               var selected = $('#selected').val();
               if (flag === "0") {
                   $preview.show();
                   $edit.hide();
                   selected.val = "0";
               } else {
                   $preview.hide();
                   $edit.show();
               }
           });
    </script>
</asp:Content>

