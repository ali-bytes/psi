<%@ Page Title="<%$Resources:Tokens,Inbox%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Inbox.aspx.cs" Inherits="NewIspNL.Pages.Inbox" %>

<%@ Import Namespace="Resources" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .messagebar-item-left, .messagebar-item-right {
            height: 20px;
        }
    </style>
    <div class="page-header">
        <h1>
            <asp:Label runat="server" Text="<%$Resources:Tokens,Inbox%>" ID="l1"></asp:Label></h1>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <!-- PAGE CONTENT BEGINS -->

            <div class="row">
                <div class="col-xs-12">
                    <div class="tabbable">
                        <ul id="inbox-tabs" class="inbox-tabs nav nav-tabs padding-16 tab-size-bigger tab-space-1">
                            <li class="li-new-mail pull-right">
                                <a href="MessageTo.aspx" data-target="write" class="btn-new-mail">
                                    <!--data-toggle="tab"-->
                                    <span class="btn bt1n-small btn-purple no-border">
                                        <i class=" icon-envelope bigger-130"></i>
                                        <span class="bigger-110"><%=Tokens.MessageTo %></span>
                                    </span>
                                </a>
                            </li>
                            <!-- ./li-new-mail -->

                            <li class="active">
                                <a data-toggle="tab" href="#inbox" data-target="inbox">
                                    <i class="blue icon-inbox bigger-130"></i>
                                    <span class="bigger-110"><%=Tokens.Inbox %></span>
                                </a>
                            </li>

                            <li>
                                <a href="SentMail.aspx">
                                    <!--data-target="sent" data-toggle="tab"-->
                                    <i class="orange icon-location-arrow bigger-130 "></i>
                                    <span class="bigger-110"><%=Tokens.SentMail %></span>
                                </a>
                            </li>

                            <%--<li>
													<a data-toggle="tab" href="#draft" data-target="draft">
														<i class="green icon-pencil bigger-130"></i>
														<span class="bigger-110">Draft</span>
													</a>
												</li>--%>
                        </ul>

                        <div class="tab-content no-border no-padding">
                            <div class="tab-pane in active">
                                <div class="message-container">
                                    <div id="id-message-list-navbar" class="message-navbar align-center clearfix">
                                        <div class="message-bar">
                                            <div class="message-infobar" id="id-message-infobar">
                                                <span class="blue bigger-150"><%=Tokens.Inbox%></span>
                                                <span class="grey bigger-110">(<asp:Literal runat="server" ID="lblInboxcount"></asp:Literal>
                                                    unread messages)</span>
                                            </div>
                                            <!--ال Toolpar البديل للرسائل-->
                                            <div class="message-toolbar hide">

                                                <button runat="server" class="btn btn-xs btn-message" id="read" onserverclick="SaveRead_OnServerClick">
                                                    <i class="icon-eye-open bigger-125"></i>
                                                    <span class="bigger-110"><%=Tokens.DoneRead %></span>
                                                </button>
                                                <button runat="server" class="btn btn-xs btn-message" id="btnUnread" onserverclick="UnRead_OnServerClick">
                                                    <i class="icon-eye-close bigger-125"></i>
                                                    <span class="bigger-110"><%=Tokens.UnRead %></span>
                                                </button>
                                                <button class="btn btn-xs btn-message" id="delete" runat="server" onserverclick="DeleteMsg_OnServerClick">
                                                    <i class="icon-trash bigger-125"></i>
                                                    <span class="bigger-110"><%=Tokens.Delete %></span>
                                                </button>
                                            </div>
                                        </div>

                                        <div>
                                            <div class="messagebar-item-left">
                                                <label class="inline middle">
                                                    <input type="checkbox" id="id-toggle-all" class="ace" />
                                                    <span class="lbl"></span>
                                                </label>

                                                &nbsp;
                                                <div class="inline ">
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
                                                <div class="inline ">
                                                    <a href="#" data-toggle="dropdown" class="dropdown-toggle">Sort &nbsp;
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

                                            <div class="nav-search minimized" style="display: none">
                                                <form class="form-search" >
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
                                            <asp:DataList ID="DataList1" oncontextmenu="return false;" runat="server" Width="100%" OnItemCommand="dtlCat_ItemCommand">
                                                <ItemTemplate>
                                                    <div class='<%#Eval("CssClass","message-item {0}") %>' style="border-bottom-width: 1px;">
                                                        <label class="inline">
                                                            <asp:CheckBox type="checkbox" CssClass="ace" ID="readIt" runat="server" /><%--Checked='<%#Eval("DoneRead") %>'--%>
                                                            <span class="lbl"></span>
                                                        </label>

                                                        <i class="message-star icon-star orange2"></i>
                                                        <span class="sender" title="<%#Eval("From") %>"><%#Eval("From")%> </span>
                                                        <span class="time"><%#Eval("Time")%></span>
                                                        <span class="date <%#Eval("CssClass") %>"><%#Eval("Date")%></span>
                                                        <span class="attachment">
                                                            <%#Eval("CssAttachment") %></span>

                                                        <span class="summary">
                                                            <span class="text">
                                                                <a id="HyperLink1" style='<%#Eval("Id") %>' class='<%#Eval("CssClass")%>'
                                                                    runat="server"></a>

                                                                <a title="<%#Eval("Id") %>" oncontextmenu="res2(<%#Eval("Id") %>)" class='<%#Eval("CssClass")%> show' onclick='res(<%#Eval("Id") %>)' id="di"><%#Eval("Subject") %></a>


                                                            </span>
                                                        </span>
                                                        <%--  <asp:LinkButton  style='<%#Eval("Id") %>' class='<%#Eval("CssClass")%>'    CommandArgument='<%# Eval("Id") %>' ID="btn_Directcon" runat="server"  CommandName="View"  ><%#Eval("Subject") %> </asp:LinkButton>--%>
                                                    </div>

                                                </ItemTemplate>




                                            </asp:DataList>

                                        </div>
                                    </div>
                                    <!-- /.message-list-container -->

                                    <div class="message-footer clearfix">
                                        <div class="pull-left">
                                            <asp:Literal runat="server" ID="lbltotal"></asp:Literal>
                                            messages total </div>


                                    </div>

                                    <div class="hide message-footer message-footer-style2 clearfix">
                                        <div class="pull-left">simpler footer </div>

                                        <div class="pull-right">
                                            <div class="inline middle">message 1 of 151 </div>

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
                                </div>
                                <!-- /.message-container -->
                            </div>
                            <!-- /.tab-pane -->
                        </div>
                        <!-- /.tab-content -->
                    </div>
                    <!-- /.tabbable -->
                </div>
                <!-- /.col -->
            </div>
            <!-- /.row -->

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
        </div>
        <!-- /.col -->
    </div>

    <script type="text/javascript">



        //function show(id) {
        //    var menu = [{
        //            name: 'Open',

        //            title: 'Open',
        //            fun: function () {
        //                $.ajax({
        //                    type: "Post",
        //                    contentType: "application/json; charset=utf-8",
        //                    url: "Inbox.aspx/Directclick",
        //                    dataType: "json",
        //                    data: "{'mid':'" + id + "'}",

        //                    success: function (data) {

        //                        var obj = data.d;

        //                        window.location.replace("MessageReader.aspx?id=" + obj);
        //                    },
        //                    error: function (result) {

        //                        alert("Error");
        //                    }
        //                });
        //            }
        //        }, {
        //            name: 'Open New Tab',

        //            title: 'Open New Tab',
        //            fun: function () {
        //                $.ajax({
        //                    type: "Post",
        //                    contentType: "application/json; charset=utf-8",
        //                    url: "Inbox.aspx/Directclick",
        //                    dataType: "json",
        //                    data: "{'mid':'" + id + "'}",

        //                    success: function (data) {

        //                        var obj = data.d;

        //                        window.open("MessageReader.aspx?id=" + obj, '_blank');
        //                    },
        //                    error: function (result) {

        //                        alert("Error");
        //                    }
        //                });
        //            }
        //        }];

        //    $('.show').contextMenu(menu);
        //}
    </script>


    <%--<div class="view">
        <fieldset>
            <div class="well">
                <div class="inbox-head">
                    <div style="float: right; padding: 3px; width: 250px;">
                        <div>
                            <asp:Label runat="server" ID="l_time" Text="<%$Resources:Tokens,From%>"></asp:Label></div>
                    </div>
                    <div style="float: left; padding: 3px; width: 500px;">
                        <div>
                            <asp:Label runat="server" ID="Label1" Text="<%$Resources:Tokens,Subject%>"></asp:Label>
                        </div>
                    </div>
                </div>
                <asp:DataList ID="DataList1" runat="server">
                    <ItemTemplate>
                        <div class="item <%#Eval("CssClass") %>">
                        <div class="info">
                            <div>
                                <asp:Label runat="server" ID="l_from" Text='<%#Bind("From") %>'></asp:Label></div>
                            <div>
                                <asp:Label runat="server" ID="l_date" Text='<%#Bind("Date") %>'></asp:Label></div>
                            <div>
                                <asp:Label runat="server" ID="l_time" Text='<%#Bind("Time") %>'></asp:Label></div>
                        </div>
                        <div class="subject">
                            <div>
                                <asp:LinkButton ID="HyperLink1" runat="server" CommandArgument='<%#Bind("Id") %>'
                                                OnClick="HyperLink1_Click" Text='<%#Bind("Subject") %>'></asp:LinkButton>
                            </div>
                            <div class="isDone">
                                <asp:CheckBox ID="readIt" runat="server" Checked='<%#Eval("DoneRead") %>' /></div>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
                <div>
                    <button type="button" runat="server" id="SaveRead" onserverclick="SaveRead_OnServerClick">
                        <asp:Literal Text="<%$Resources:Tokens,DoneRead %>" runat="server" /></button>
                </div>
                
            </div>
        </fieldset>
    </div>
<style>
        .inbox-head{
            /*background-color: white; margin-top: 2px; width: 800px;*/
            text-align: center;
            padding: 5px;
        }
        .item{
            background-color: whitesmoke;
            border-bottom: 1px gray solid;
            height: 55px;
            line-height: 15px;
           
            padding: 2px;
            width: 800px;
        }
        .item:hover{
            background-color: rgb(255, 255, 255);
        }
        .subject{
            float: left;
            height: 55px;
            width: 500px;
            padding-top: 15px;
            margin-bottom: 0px;
            text-align: center;
        }

        .subject a{
            color: black;
            font-weight: bold;
            text-decoration: none;
        }
        .subject a:hover{
            color: rgb(30, 197, 205);
        }

        .info{
            float: right;
            width: 250px;
        }
    </style>--%>
    <style>
        #message-list {
            overflow-y: auto;
            height: 550px;
        }

        a.l {
            color: #555;
        }

        span.message-unread {
            font-weight: bold;
            color: #609fc4;
        }
    </style>
    <script type="text/javascript">
        jQuery(function ($) {
            $(document).keydown(function(e) {
                if (e.keyCode === 13) {
                    alert('you pressed enter ^_^');
                }
            });
            //handling tabs and loading/displaying relevant messages and forms
            //not needed if using the alternative view, as described in docs
            var prevTab = 'inbox';
            $('#inbox-tabs a[data-toggle="tab"]').on('show.bs.tab', function (e) {
                var currentTab = $(e.target).data('target');
                if (currentTab == 'write') {
                    Inbox.show_form();
                } else {
                    if (prevTab == 'write')
                        Inbox.show_list();

                    //load and display the relevant messages 
                }
                prevTab = currentTab;
            });



            //basic initializations
            $('.message-list .message-item input[type=checkbox]').removeAttr('checked');
            $('.message-list').delegate('.message-item input[type=checkbox]', 'click', function () {
                $(this).closest('.message-item').toggleClass('selected');
                if (this.checked) Inbox.display_bar(1); //display action toolbar when a message is selected
                else {
                    Inbox.display_bar($('.message-list input[type=checkbox]:checked').length);
                    //determine number of selected messages and display/hide action toolbar accordingly
                }
            });


            //check/uncheck all messages
            $('#id-toggle-all').removeAttr('checked').on('click', function () {
                if (this.checked) {
                    Inbox.select_all();
                } else Inbox.select_none();
            });

            //select all
            $('#id-select-message-all').on('click', function (e) {
                e.preventDefault();
                Inbox.select_all();
            });

            //select none
            $('#id-select-message-none').on('click', function (e) {
                e.preventDefault();
                Inbox.select_none();
            });

            //select read
            $('#id-select-message-read').on('click', function (e) {
                e.preventDefault();
                Inbox.select_read();
            });

            //select unread
            $('#id-select-message-unread').on('click', function (e) {
                e.preventDefault();
                Inbox.select_unread();
            });

            /////////

            //display first message in a new area
            /*$('.message-list .message-item:eq(0) .text').on('click', function () {
                //show the loading icon
                $('.message-container').append('<div class="message-loading-overlay"><i class="icon-spin icon-spinner orange2 bigger-160"></i></div>');
    
                $('.message-inline-open').removeClass('message-inline-open').find('.message-content').remove();
    
                var message_list = $(this).closest('.message-list');
    
                //some waiting
                setTimeout(function () {
    
                    //hide everything that is after .message-list (which is either .message-content or .message-form)
                    message_list.next().addClass('hide');
                    $('.message-container').find('.message-loading-overlay').remove();
    
                    //close and remove the inline opened message if any!
    
                    //hide all navbars
                    $('.message-navbar').addClass('hide');
                    //now show the navbar for single message item
                    $('#id-message-item-navbar').removeClass('hide');
    
                    //hide all footers
                    $('.message-footer').addClass('hide');
                    //now show the alternative footer
                    $('.message-footer-style2').removeClass('hide');
    
    
                    //move .message-content next to .message-list and hide .message-list
                    message_list.addClass('hide').after($('.message-content')).next().removeClass('hide');
    
                    //add scrollbars to .message-body
                    $('.message-content .message-body').slimScroll({
                        height: 200,
                        railVisible: true
                    });
    
                }, 500 + parseInt(Math.random() * 500));
            });*/


            //display second message right inside the message list
            /* $('.message-list .message-item:eq(1) .text').on('click', function () {
                 var message = $(this).closest('.message-item');
     
                 //if message is open, then close it
                 if (message.hasClass('message-inline-open')) {
                     message.removeClass('message-inline-open').find('.message-content').remove();
                     return;
                 }
     
                 $('.message-container').append('<div class="message-loading-overlay"><i class="icon-spin icon-spinner orange2 bigger-160"></i></div>');
                 setTimeout(function () {
                     $('.message-container').find('.message-loading-overlay').remove();
                     message
                                 .addClass('message-inline-open')
                                 .append('<div class="message-content" />')
                     var content = message.find('.message-content:last').html($('#id-message-content').html());
     
                     content.find('.message-body').slimScroll({
                         height: 200,
                         railVisible: true
                     });
     
                 }, 500 + parseInt(Math.random() * 500));
     
             });
             */


            //back to message list
            $('.btn-back-message-list').on('click', function (e) {
                e.preventDefault();
                Inbox.show_list();
                $('#inbox-tabs a[data-target="inbox"]').tab('show');
            });



            //hide message list and display new message form
            /**
            $('.btn-new-mail').on('click', function(e){
            e.preventDefault();
            Inbox.show_form();
            });
            */


            var Inbox = {
                //displays a toolbar according to the number of selected messages
                display_bar: function (count) {
                    if (count == 0) {
                        $('#id-toggle-all').removeAttr('checked');
                        $('#id-message-list-navbar .message-toolbar').addClass('hide');
                        $('#id-message-list-navbar .message-infobar').removeClass('hide');
                    } else {
                        $('#id-message-list-navbar .message-infobar').addClass('hide');
                        $('#id-message-list-navbar .message-toolbar').removeClass('hide');
                    }
                },
                select_all: function () {
                    var count = 0;
                    $('.message-item input[type=checkbox]').each(function () {
                        this.checked = true;
                        $(this).closest('.message-item').addClass('selected');
                        count++;
                    });

                    $('#id-toggle-all').get(0).checked = true;

                    Inbox.display_bar(count);
                },
                select_none: function () {
                    $('.message-item input[type=checkbox]').removeAttr('checked').closest('.message-item').removeClass('selected');
                    $('#id-toggle-all').get(0).checked = false;

                    Inbox.display_bar(0);
                },
                select_read: function () {
                    $('.message-unread input[type=checkbox]').removeAttr('checked').closest('.message-item').removeClass('selected');

                    var count = 0;
                    $('.message-item:not(.message-unread) input[type=checkbox]').each(function () {
                        this.checked = true;
                        $(this).closest('.message-item').addClass('selected');
                        count++;
                    });
                    Inbox.display_bar(count);
                },
                select_unread: function () {
                    $('.message-item:not(.message-unread) input[type=checkbox]').removeAttr('checked').closest('.message-item').removeClass('selected');

                    var count = 0;
                    $('.message-unread input[type=checkbox]').each(function () {
                        this.checked = true;
                        $(this).closest('.message-item').addClass('selected');
                        count++;
                    });

                    Inbox.display_bar(count);
                }
            };

            //show message list (back from writing mail or reading a message)
            Inbox.show_list = function () {
                $('.message-navbar').addClass('hide');
                $('#id-message-list-navbar').removeClass('hide');

                $('.message-footer').addClass('hide');
                $('.message-footer:not(.message-footer-style2)').removeClass('hide');

                $('.message-list').removeClass('hide').next().addClass('hide');
                //hide the message item / new message window and go back to list
            };

            //show write mail form
            Inbox.show_form = function () {
                if ($('.message-form').is(':visible')) return;
                if (!form_initialized) {
                    initialize_form();
                }


                var message = $('.message-list');
                $('.message-container').append('<div class="message-loading-overlay"><i class="icon-spin icon-spinner orange2 bigger-160"></i></div>');

                setTimeout(function () {
                    message.next().addClass('hide');

                    $('.message-container').find('.message-loading-overlay').remove();

                    $('.message-list').addClass('hide');
                    $('.message-footer').addClass('hide');
                    $('.message-form').removeClass('hide').insertAfter('.message-list');

                    $('.message-navbar').addClass('hide');
                    $('#id-message-new-navbar').removeClass('hide');


                    //reset form??
                    $('.message-form .wysiwyg-editor').empty();

                    $('.message-form .ace-file-input').closest('.file-input-container:not(:first-child)').remove();
                    $('.message-form input[type=file]').ace_file_input('reset_input');

                    $('.message-form').get(0).reset();

                }, 300 + parseInt(Math.random() * 300));
            }




            var form_initialized = false;
            function initialize_form() {
                if (form_initialized) return;
                form_initialized = true;

                //intialize wysiwyg editor
                $('.message-form .wysiwyg-editor').ace_wysiwyg({
                    toolbar:
                    [
                        'bold',
                        'italic',
                        'strikethrough',
                        'underline',
                        null,
                        'justifyleft',
                        'justifycenter',
                        'justifyright',
                        null,
                        'createLink',
                        'unlink',
                        null,
                        'undo',
                        'redo'
                    ]
                }).prev().addClass('wysiwyg-style1');

                //file input
                $('.message-form input[type=file]').ace_file_input()
                    //and the wrap it inside .span7 for better display, perhaps
                    .closest('.ace-file-input').addClass('width-90 inline').wrap('<div class="row file-input-container"><div class="col-sm-7"></div></div>');

                //the button to add a new file input
                $('#id-add-attachment').on('click', function () {
                    var file = $('<input type="file" name="attachment[]" />').appendTo('#form-attachments');
                    file.ace_file_input();
                    file.closest('.ace-file-input').addClass('width-90 inline').wrap('<div class="row file-input-container"><div class="col-sm-7"></div></div>')
                        .parent(/*.span7*/).append('<div class="action-buttons pull-right col-xs-1">\
							<a href="#" data-action="delete" class="middle">\
								<i class="icon-trash red bigger-130 middle"></i>\
							</a>\
						</div>').find('a[data-action=delete]').on('click', function (e) {
						    //the button that removes the newly inserted file input
						    e.preventDefault();
						    $(this).closest('.row').hide(300, function () {
						        $(this).remove();
						    });
						});
                });
            } //initialize_form


            //turn the recipient field into a tag input field!
            /**	
            var tag_input = $('#form-field-recipient');
            if(! ( /msie\s*(8|7|6)/.test(navigator.userAgent.toLowerCase())) ) 
            tag_input.tag({placeholder:tag_input.attr('placeholder')});
                
                
            //and add form reset functionality
            $('.message-form button[type=reset]').on('click', function(){
            $('.message-form .message-body').empty();
                        
            $('.message-form .ace-file-input:not(:first-child)').remove();
            $('.message-form input[type=file]').ace_file_input('reset_input');
                        
                        
            var val = tag_input.data('value');
            tag_input.parent().find('.tag').remove();
            $(val.split(',')).each(function(k,v){
            tag_input.before('<span class="tag">'+v+'<button class="close" type="button">&times;</button></span>');
            });
            });
            */
            //$('span.ace').replaceWith('<input class="ace" type="checkbox">' + $('span.ace').innerHTML);
        });
    </script>

    <script type="text/javascript">


        function res(id) {




            $.ajax({
                type: "Post",
                contentType: "application/json; charset=utf-8",
                url: "Inbox.aspx/Directclick",
                dataType: "json",
                data: "{'mid':'" + id + "'}",

                success: function (data) {

                    var obj = data.d;

                    window.location.replace("MessageReader.aspx?id=" + obj);

                },
                error: function (result) {

                    alert("Error");
                }
            });
        };
        function res2(id) {




            $.ajax({
                type: "Post",
                contentType: "application/json; charset=utf-8",
                url: "Inbox.aspx/Directclick",
                dataType: "json",
                data: "{'mid':'" + id + "'}",

                success: function (data) {

                    var obj = data.d;

                    window.open("MessageReader.aspx?id=" + obj, '_blank');


                },
                error: function (result) {

                    alert("Error");
                }
            });
        };
    </script>
</asp:Content>
