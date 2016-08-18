<%@ Page Title="<%$ Resources:Tokens,Message %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="MessageReader.aspx.cs" Inherits="NewIspNL.Pages.MessageReader" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit.HTMLEditor" Assembly="AjaxControlToolkit, Version=4.1.7.123, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        <div class="page-header">
							<h1>
								<asp:Label runat="server" Text="<%$Resources:Tokens,Message%>" ID="Label3"></asp:Label></h1>
						</div> 
                         <div class="alert alert-success" Visible="False" runat="server" id="LiMessage"></div>
                        <div class="row">
							<div class="col-xs-12" style="margin-bottom: 12px;">
							    		<div class="tabbable">
											<ul id="inbox-tabs" class="inbox-tabs nav nav-tabs padding-16 tab-size-bigger tab-space-1">
												<li class="li-new-mail pull-right">
													<a  href="MessageTo.aspx" data-target="write" class="btn-new-mail"><!--data-toggle="tab"-->
														<span class="btn bt1n-small btn-purple no-border">
															<i class=" icon-envelope bigger-130"></i>
															<span class="bigger-110"><%=Tokens.MessageTo %></span>
														</span>
													</a>
												</li><!-- ./li-new-mail -->

												<li>
													<a  href="Inbox.aspx" data-target="inbox"><!--data-toggle="tab"-->
														<i class="blue icon-inbox bigger-130"></i>
														<span class="bigger-110"><%=Tokens.Inbox %></span>
													</a>
												</li>

												<li>
													<a  href="SentMail.aspx" ><!--data-target="sent" data-toggle="tab"-->
														<i class="orange icon-location-arrow bigger-130 "></i>
														<span class="bigger-110"><%=Tokens.SentMail %></span>
													</a>
												</li>

											
											</ul>
                                           
                            </div>
                            </div>
                            
                            <div class="message-content" id="id-message-content">
									<div class="message-header clearfix" style="height: 100px">
										<div class="pull-left">
											<span class="blue bigger-125"> <asp:Literal runat="server" ID="l_subject"></asp:Literal> </span>

											<div class="space-4"></div>

											<i class="icon-star orange2 mark-star"></i>

											&nbsp;
											<img class="middle" alt="John's Avatar" src="../Content/ace-assest/avatars/avatar.png" width="32" />
											&nbsp;
											<a href="#" class="sender"><asp:Literal runat="server" ID="l_from"></asp:Literal></a>

											&nbsp;
											<i class="icon-time bigger-110 orange middle"></i>
											<span class="time"><asp:Label runat="server" ID="l_date"></asp:Label>
                            &nbsp;
                            </span>
										</div>

										<div class="action-buttons pull-right">
											<a href="#ReplayModal" title="Replay" data-rel="tooltip" data-toggle="modal">
												<i class="icon-reply green icon-only bigger-130"></i>
											</a>


										</div>
									</div>

									<div class="hr hr-double"></div>

									<div class="message-body">
										
										    <asp:Label runat="server" ID="l_body"></asp:Label>
										
									</div>

									<div class="hr hr-double"></div>

									<div class="message-attachment clearfix">
										<div class="attachment-title">
											<span class="blue bolder bigger-110"><%=Tokens.Attachments %></span>
											&nbsp;
											<span class="grey"><!--(2 files, 4.5 MB)-->&nbsp;</span>

											<div class="inline position-relative">
												<a href="#" data-toggle="dropdown" class="dropdown-toggle">
													&nbsp;
													<i class="icon-caret-down bigger-125 middle"></i>
												</a>

											</div>
										</div>

										&nbsp;
										<ul class="attachment-list pull-left list-unstyled">
											<li runat="server" id="attachmentFiles1" Visible="False">
												<a href="#" class="attached-file inline">
													<i class="icon-film bigger-110 middle"></i>
													<span class="attached-name middle">Document1</span>
												</a>

												<div class="action-buttons inline">
													<a target="_blank" runat="server" id="attachment1">
														<i   class="icon-download-alt bigger-125 blue"></i>
													</a>

													<a href="#">
														<i class="icon-trash bigger-125 red"></i>
													</a>
												</div>
											</li>

											<li runat="server" id="attachmentFiles2" Visible="False">
												<a href="#" class="attached-file inline">
													<i class="icon-film bigger-110 middle"></i>
													<span class="attached-name middle">Document2</span>
												</a>

												<div class="action-buttons inline">
													<a target="_blank" runat="server" id="attachment2">
														<i class="icon-download-alt bigger-125 blue"></i>
													</a>

													<a href="#">
														<i class="icon-trash bigger-125 red"></i>
													</a>
												</div>
											</li>
                                            <li runat="server" id="attachmentFiles3" Visible="False">
												<a href="#" class="attached-file inline">
													<i class="icon-film bigger-110 middle"></i>
													<span class="attached-name middle">Document3</span>
												</a>

												<div class="action-buttons inline">
													<a target="_blank" runat="server" id="attachment3">
														<i class="icon-download-alt bigger-125 blue"></i>
													</a>

													<a href="#">
														<i class="icon-trash bigger-125 red"></i>
													</a>
												</div>
											</li>
                                            
										</ul>

										
									</div>
								</div> <!-- /.message-content -->
                                
       <!--Replay Modal-->
       <div class="bootbox modal fade" id="ReplayModal" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal">
                        ×</button><h4 class="modal-title">
                            <%=Tokens.Reply %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <div class="bootbox-form">
                        
                                <div style="line-height: 20pt;">

            <div>
                الى :  <asp:Literal runat="server" ID="LiReplyTo"></asp:Literal>
            </div>
            <label>
                عنوان الرسالة</label>
            <div>
                <asp:TextBox runat="server" ClientIDMode="Static" ID="TbReplyTitle" Style="width: 100%"></asp:TextBox></div>
            <label>
                نص الرد</label>
            <div>
               
                <cc1:Editor  ID="Editor1" runat="server" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="Editor1"
                                                            Display="Dynamic" ErrorMessage="<%$Resources:Tokens,Required%>"></asp:RequiredFieldValidator>
                                                   

                <asp:Label runat="server" ClientIDMode="Static" ID="lblReplayBody"></asp:Label>
            </div>

        </div>
                        </div>

                    </div>
                </div>
                
                
                 <div class="form-group no-margin-bottom">
                                                    <label class="col-sm-3 control-label no-padding-left">
                                                        <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:Tokens,Attachments %>"></asp:Literal>:</label>
                                                    <div class="col-sm-9">
                                                        <div id="form-attachments">
                                                            <div class="row file-input-container">
                                                                <div class="col-sm-7">
                                                                    <div class="ace-file-input width-90 inline">
                                                                        <asp:FileUpload runat="server" ID="UploadAttachFile" />
                                                                    </div>
                                                                    <div class="ace-file-input width-90 inline">
                                                                        <asp:FileUpload runat="server" ID="FileUpload2" />
                                                                    </div>
                                                                    <div class="ace-file-input width-90 inline">
                                                                        <asp:FileUpload runat="server" ID="FileUpload3" />
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                
                
                

                <div class="modal-footer">
                    <asp:LinkButton runat="server" ClientIDMode="Static" OnClick="BSend_OnClick" CssClass="btn btn-success"><i class="icon-arrow-right icon-on-right"></i>&nbsp;<%=Tokens.Send %></asp:LinkButton>
                    &nbsp;<button data-bb-handler="cancel" data-dismiss="modal" type="button" class="btn btn-primary">
                        <%=Tokens.Cancel %></button>
                </div>
                
                
                
                                
                

                
                

            </div>
        </div>
    </div>

    </div>

</asp:Content>

