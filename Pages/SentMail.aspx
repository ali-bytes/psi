<%@ Page Title="<%$Resources:Tokens,SentMail%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="SentMail.aspx.cs" Inherits="NewIspNL.Pages.SentMail" %>

<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <style type="text/css">
        .messagebar-item-left, .messagebar-item-right {
            height: 20px;
        }
    </style>
    
    
        <div class="page-header">
							<h1>
								<asp:Label runat="server" Text="<%$Resources:Tokens,SentMail%>" ID="l1"></asp:Label></h1>
						</div>
    <div class="row">
							<div class="col-xs-12">
								<!-- PAGE CONTENT BEGINS -->

								<div class="row">
									<div class="col-xs-12">
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
													<a  href="Inbox.aspx" data-target="inbox">
														<i class="blue icon-inbox bigger-130"></i>
														<span class="bigger-110"><%=Tokens.Inbox %></span>
													</a>
												</li>

												<li class="active">
													<a data-toggle="tab" href="SentMail.aspx" ><!--data-target="sent" data-toggle="tab"-->
														<i class="orange icon-location-arrow bigger-130 "></i>
														<span class="bigger-110"><%=Tokens.SentMail %></span>
													</a>
												</li>

											
											</ul>

											<div class="tab-content no-border no-padding">
												<div class="tab-pane in active">
													<div class="message-container">
														<div id="id-message-list-navbar" class="message-navbar align-center clearfix">
														         <%--<input type="checkbox" class="checkbox"  id="selecctall" style="margin-right: 1px; float: right;" value=""/>--%>
															<div class="message-bar">
																<div class="message-infobar" id="id-message-infobar">
																	<span class="blue bigger-150"><%=Tokens.SentMail%></span>
																	<span class="grey bigger-110">(<asp:Literal runat="server" ID="lblInboxcount"></asp:Literal> unread messages)</span>
                                                                </div>
                                                                
                                                                <!--ال Toolpar البديل للرسائل-->
																<div class="message-toolbar hide">
																	
                                                                    <button runat="server" class="btn btn-xs btn-message" id="read" onserverclick="SaveRead_OnServerClick">
                                                                        <i class="icon-eye-open bigger-125"></i>
                                                                        <span class="bigger-110"><%=Tokens.DoneRead %></span>
                                                                    </button>
																	<a href="#" class="btn btn-xs btn-message">
																		<i class="icon-trash bigger-125"></i>
																		<span class="bigger-110"><%=Tokens.Delete %></span>
																	</a>
																</div>
															</div>

															<div>
																<div class="messagebar-item-left">
																	<label class="inline middle">
																		<input type="checkbox" id="id-toggle-all" class="ace" />
																		<span class="lbl"></span>
																	</label>

																	&nbsp;
																	<div class="inline position-relative">
																		<a href="#" data-toggle="dropdown" class="dropdown-toggle">
																			<i class="icon-caret-down bigger-125 middle"></i>
																		</a>

																		<ul class="dropdown-menu dropdown-lighter dropdown-100">
																			<li>
																				<a id="id-select-message-all" href="#">All</a>
																			</li>

																			<li>
																				<a id="id-select-message-none" href="#">None</a>
																			</li>

																			<li class="divider"></li>

																			<li>
																				<a id="id-select-message-unread" href="#">Unread</a>
																			</li>

																			<li>
																				<a id="id-select-message-read" href="#">Read</a>
																			</li>
																		</ul>
																	</div>
																</div>

																<div class="messagebar-item-right">
																	<div class="inline position-relative">
																		<a href="#" data-toggle="dropdown" class="dropdown-toggle">
																			Sort &nbsp;
																			<i class="icon-caret-down bigger-125"></i>
																		</a>

																		<ul class="dropdown-menu dropdown-lighter pull-right dropdown-100">
																			<li>
																				<a href="#">
																					<i class="icon-ok green"></i>
																					Date
																				</a>
																			</li>

																			<li>
																				<a href="#">
																					<i class="icon-ok invisible"></i>
																					From
																				</a>
																			</li>

																			<li>
																				<a href="#">
																					<i class="icon-ok invisible"></i>
																					Subject
																				</a>
																			</li>
																		</ul>
																	</div>
																</div>
<%--                                                                <p style="margin-right: 1px; float: right">ali</p>--%>
																<div class="nav-search minimized">
																	<form class="form-search">
																		<span class="input-icon">
																			<input type="text" autocomplete="off" class="input-small nav-search-input" placeholder="Search inbox ..." />
																			<i class="icon-search nav-search-icon"></i>
																		</span>
																	</form>
																</div>
															</div>
														</div>

														

														<div id="id-message-new-navbar" class="hide message-navbar align-center clearfix">
															<div class="message-bar">
																<div class="message-toolbar">
																	<a href="#" class="btn btn-xs btn-message">
																		<i class="icon-save bigger-125"></i>
																		<span class="bigger-110">Save Draft</span>
																	</a>

																	<a href="#" class="btn btn-xs btn-message">
																		<i class="icon-remove bigger-125"></i>
																		<span class="bigger-110">Discard</span>
																	</a>
																</div>
															</div>

															<div class="message-item-bar">
																<div class="messagebar-item-left">
																	<a href="#" class="btn-back-message-list no-hover-underline">
																		<i class="icon-arrow-left blue bigger-110 middle"></i>
																		<b class="middle bigger-110">Back</b>
																	</a>
																</div>

																<div class="messagebar-item-right">
																	<span class="inline btn-send-message">
																		<button type="button" class="btn btn-sm btn-primary no-border">
																			<span class="bigger-110">Send</span>

																			<i class="icon-arrow-right icon-on-right"></i>
																		</button>
																	</span>
																</div>
															</div>
														</div>

														<div class="message-list-container">
														    <div class="message-list" id="message-list">

															<%if(AllMessages.Count==0)return;
                 foreach (var message in AllMessages)
															   {%>
															      <div class="message-item <%=message.CssClass %>">
																	<label class="inline">
																		<input type="checkbox" class="ace" />
																		<span class="lbl"></span>
																	</label>

																	<i class="message-star icon-star orange2"></i>
																	<span class="sender" title="<%=message.From %>"><%=message.From%> </span>
																	<span class="time"><%=message.Time%></span>
                                                                    <span class="date <%=message.CssClass %>"><%=message.Date%></span>
                                                                    <span class="attachment">
                                                                    <%=message.CssAttachment %></span> 
                                                                    
                                                                     <span class="summary">
																		<span class="text">
																			<%--<a id="HyperLink1" class="<%=message.CssClass %>" href="<%=string.Format("MessageReader.aspx?id={0}",message.Id) %>" target="_blank"><%=message.Subject %></a>--%>
																
                                                                            
                                                                           <% name= message.Subject;
                                                                              mesid = message.Id;
                                                                           %>
																	        <%--<asp:LinkButton ID="di" OnClientClick=" res(<%#message.  %>)"  CommandArgument='<%#this.mesid%> '  runat="server"><%=this.name%></asp:LinkButton>--%>
															
                                                                   <a href="#"  onclick='res(<%=this.mesid%>)' id="di" ><%=this.name%></a>
																
                                                                           
                                                                        
                                                                        </span>
																	</span>
																</div>
															   <% }%> 
															</div>
														</div><!-- /.message-list-container -->

														<div class="message-footer clearfix">
															<div class="pull-left"> <asp:Literal runat="server" ID="lbltotal"></asp:Literal> messages total </div>

															
														</div>

														<div class="hide message-footer message-footer-style2 clearfix">
															<div class="pull-left"> simpler footer </div>

															<div class="pull-right">
																<div class="inline middle"> message 1 of 151 </div>

																&nbsp; &nbsp;
																<ul class="pagination middle">
																	<li class="disabled">
																		<span>
																			<i class="icon-angle-left bigger-150"></i>
																		</span>
																	</li>

																	<li>
																		<a href="#">
																			<i class="icon-angle-right bigger-150"></i>
																		</a>
																	</li>
																</ul>
															</div>
														</div>
													</div><!-- /.message-container -->
												</div><!-- /.tab-pane -->
											</div><!-- /.tab-content -->
										</div><!-- /.tabbable -->
									</div><!-- /.col -->
								</div><!-- /.row -->

								<form id="id-message-form" class="hide form-horizontal message-form  col-xs-12">
									<div class="">
										<div class="form-group">
											<label class="col-sm-3 control-label no-padding-right" for="form-field-recipient">Recipient:</label>

											<div class="col-sm-9">
												<span class="input-icon">
													<input type="email" name="recipient" id="form-field-recipient" data-value="alex@doe.com" value="alex@doe.com" placeholder="Recipient(s)" />
													<i class="icon-user"></i>
												</span>
											</div>
										</div>

										<div class="hr hr-18 dotted"></div>

										<div class="form-group">
											<label class="col-sm-3 control-label no-padding-right" for="form-field-subject">Subject:</label>

											<div class="col-sm-6 col-xs-12">
												<div class="input-icon block col-xs-12 no-padding">
													<input maxlength="100" type="text" class="col-xs-12" name="subject" id="form-field-subject" placeholder="Subject" />
													<i class="icon-comment-alt"></i>
												</div>
											</div>
										</div>

										<div class="hr hr-18 dotted"></div>

										<div class="form-group">
											<label class="col-sm-3 control-label no-padding-right">
												<span class="inline space-24 hidden-480"></span>
												Message:
											</label>

											<div class="col-sm-9">
												<div class="wysiwyg-editor"></div>
											</div>
										</div>

										<div class="hr hr-18 dotted"></div>

										<div class="form-group no-margin-bottom">
											<label class="col-sm-3 control-label no-padding-right">Attachments:</label>

											<div class="col-sm-9">
												<div id="form-attachments">
													<input type="file" name="attachment[]" />
												</div>
											</div>
										</div>

										<div class="align-right">
											<button id="id-add-attachment" type="button" class="btn btn-sm btn-danger">
												<i class="icon-paper-clip bigger-140"></i>
												Add Attachment
											</button>
										</div>

										<div class="space"></div>
									</div>
								</form>

								<!-- PAGE CONTENT ENDS -->
							</div><!-- /.col -->
						</div>
                            <style>
    #message-list{
    overflow-y: auto;
height: 550px;}
a.l {
    color: #555;
}
span.message-unread {
    font-weight: bold;
color: #609fc4;
}
</style>
    
    <script type="text/javascript">
        
        $('.checkbox').checkbox();
        $('#selecctall').click(function () {  //on click 
            if (this.checked) { // check select status
                $('input[type="checkbox"]').each(function () { //loop through each checkbox
                    this.checked = true;  //select all checkboxes
                });
            } else {
                $('input[type="checkbox"]').each(function () { //loop through each checkbox
                    this.checked = false; //deselect all checkboxes 
                });
            }
        });



        function res(id) {
    
          
           
       
            $.ajax({
                type: "Post",
                contentType: "application/json; charset=utf-8",
                url: "SentMail.aspx/Directclick",
                dataType: "json",
                data:"{'mid':'" + id + "'}",

                success: function (data) {
                              
                    var obj = data.d;

                    window.location.replace("MessageReader.aspx?id="+ obj);
                },
                error: function (result) {

                    alert("Error");
                }
            });
        };
    </script>
</asp:Content>

