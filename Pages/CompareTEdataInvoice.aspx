<%@ Page Title="<%$Resources:Tokens,CompareTEdataInvoice %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="CompareTEdataInvoice.aspx.cs" Inherits="NewIspNL.Pages.CompareTEdataInvoice" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <%--  <div >
         <div class="col-md-12 col-sm-6 col-xs-6">
                            <div class="well row">
                                    <label for="DdlYear">
                                        <%=Tokens.Year %>
                                    </label>
                                        <asp:DropDownList runat="server" ID="DdlYear">
                                        </asp:DropDownList>
                                    <label for="DdlMonth">
                                        <%=Tokens.Month %></label>
                                        <asp:DropDownList runat="server" ID="DdlMonth">
                                        </asp:DropDownList>
                            </div>
                        </div>
        </div>--%>
     <div class="row">
         <div class="col-xs-12 col-sm-7" style="margin: 0 21%;">
										<div class="widget-box">
											<div class="widget-header">
												<h4><%=Tokens.LoadSheet%></h4>
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
             <asp:FileUpload runat="server" multiple="" ID="fu_sheet"/>
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="tedata" 
                 ControlToValidate="fu_sheet" ErrorMessage="<%$ Resources:Tokens,Required %>" Display="Dynamic"></asp:RequiredFieldValidator>
             <asp:RegularExpressionValidator runat="server" ForeColor="red" ValidationGroup="tedata"  ValidationExpression="^.*\.(xls|XLS|xlsx|XLSX|)$" ControlToValidate="fu_sheet"  ErrorMessage="<%$Resources:Tokens,uploadfilexcelerror %>"></asp:RegularExpressionValidator>
                  
                                                            								</fieldset>
                                                        
														<div class="form-actions center">
                                                                 <%--<asp:Button runat="server" ID="bSave" CssClass="btn btn-success"  OnClick="btnAdd_Click" Text="<%$Resources:Tokens,Save%>"/><i class="icon-ok bigger-110">&nbsp;</i>--%>
                                                                 <asp:LinkButton style="width: 160px" runat="server" id="bSave" class="btn btn-primary" OnClick="btnArrangeInvoice_Click" ValidationGroup="tedata"><i class="icon-save bigger-110"></i>&nbsp;<%=Tokens.Compare%></asp:LinkButton>
                                                                                         <a href="../ExcelTemplates/ArrangedInvoice.xls" class="btn btn-default">
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
                                    </div>
                   <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-responsive">
                    <Columns>
                        <asp:BoundField DataField="CustomerNumber" HeaderText="Customer Name" />
                        <asp:BoundField DataField="PackageName" HeaderText="Package name" />
                        <asp:BoundField DataField="NetAmout" HeaderText="Net amount" />
                        <asp:BoundField DataField="StartAt" HeaderText="Start date" />
                        <asp:BoundField DataField="EndAt" HeaderText="End date" />
                        <asp:BoundField DataField="Description" HeaderText="Description" />
                        <asp:BoundField DataField="PhoneNumber" HeaderText="Phone number" />
                        <asp:BoundField DataField="Exchange" HeaderText="Exchange" />
                        <asp:BoundField DataField="OurAmount" HeaderText="Our Amount" />
                        <asp:BoundField DataField="OurStartAt" HeaderText="Our StartAt" />
                        <asp:BoundField DataField="OurEndAt" HeaderText="Our EndAt" />
                        <asp:BoundField DataField="IsDiffrent" HeaderText="IsDiffrent" />
                    </Columns>
                </asp:GridView>
</asp:Content>

