<%@ Page Title="<%$ Resources:Tokens,ManaulInvoices %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ManaulInvoices.aspx.cs" Inherits="NewIspNL.Pages.ManaulInvoices" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div runat="server" id="Msgsuccess" Visible="False" class="alert alert-success"><%=Tokens.Saved %></div>
    <div class="row">
                             <div class="col-xs-12 col-sm-7" style="margin: 0 21%;">
										<div class="widget-box">
											<div class="widget-header">
												<h4><asp:Literal ID="Literal1" Text="<%$Resources:Tokens,MenuManaulInvoices%>" runat="server" /></h4>
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

                <asp:FileUpload runat="server" ID="fu_sheet" multiple/>
                <asp:RequiredFieldValidator runat="server" ID="r_sheet" 
                                            ControlToValidate="fu_sheet" ValidationGroup="fil" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                
          <asp:RegularExpressionValidator runat="server" ForeColor="red" ValidationGroup="fil" ValidationExpression="^.*\.(xls|XLS|xlsx|XLSX|)$" ControlToValidate="fu_sheet"  ErrorMessage="<%$Resources:Tokens,uploadfilexcelerror %>"></asp:RegularExpressionValidator>
                  
                                                        
                                                          </div>
            <div class="form-actions center">
                <asp:LinkButton runat="server" ID="bSave" onclick="bSave_Click" ValidationGroup="fil"
                           Width="160px" CssClass="btn btn-primary"><i class="icon-save icon-only"></i>&nbsp;<asp:Literal runat="server" Text="<%$Resources:Tokens,Save %>"></asp:Literal></asp:LinkButton>
                                           
                    <a href="../ExcelTemplates/Invoices.xls" class="btn btn-default">
                        <i class="icon-cloud-download bigger-120"></i>
                        <%=Tokens.Downloadsample %>
                    </a>
                <br/>
                <asp:Label runat="server" ID="l_message" Text=""></asp:Label>
            </div>
												    </div>
                                                    </div>
                                                    </div>
                                                    </div>
          
            <hr/>
            <div>
                <asp:GridView runat="server" ID="gv_errors" ondatabound="gv_errors_DataBound" 
                              AutoGenerateColumns="False"  
                               CellPadding="4" CssClass="table table-bordered table-condensed text-center"
                              GridLines="Vertical">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="gv_l_Number" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="error" HeaderText="<%$ Resources:Tokens,Error%>" />
                    </Columns>
                </asp:GridView>
            </div>

    </div>
</asp:Content>


