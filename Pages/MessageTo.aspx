<%@ Page Title="<%$Resources:Tokens,MessageTo%>" Language="C#"  MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="MessageTo.aspx.cs" Inherits="NewIspNL.Pages.MessageTo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>


<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>
    <style>
        .delItem {
            position: absolute;
            top: 0;
            bottom: 0;
            right: 0;
            width: 18px;
            height: auto;
            line-height: 25px;
            text-align: center;
        }

            .delItem:hover {
                color: red;
                background-color: rgba(0,0,0,0.2);
                cursor: pointer;
            }

        .r-item {
            background-image: none;
            background-color: #91b8d0;
            color: #fff;
            display: inline-block;
            font-size: 13px;
            font-weight: normal;
            margin-bottom: 3px;
            margin-right: 3px;
            padding: 6px 22px 7px 9px;
            position: relative;
            text-shadow: 1px 1px 1px rgba(0,0,0,0.15);
            transition: all .2s ease 0s;
            vertical-align: baseline;
            white-space: nowrap;
            border: 0;
            -webkit-box-shadow: none;
            box-shadow: none;
            border-radius: 0;
        }

        .item-div {
            height: auto;
        }
    </style>
    <div class="page-header">
        <h1>
            <%=Tokens.MessageTo %></h1>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <!-- PAGE CONTENT BEGINS -->
            <div class="row">
                <div class="col-xs-12">
                    <div class="tabbable">
                        <ul id="inbox-tabs" class="inbox-tabs nav nav-tabs padding-16 tab-size-bigger tab-space-1">
                            <li class="li-new-mail pull-right active"><a href="MessageTo.aspx" data-target="write"
                                class="btn-new-mail"><span class="btn bt1n-small btn-purple no-border"><i class=" icon-envelope bigger-130"></i><span class="bigger-110">
                                    <%=Tokens.MessageTo %></span> </span></a></li>
                            <!-- ./li-new-mail -->
                            <li class=""><a href="Inbox.aspx" data-target="inbox"><i class="blue icon-inbox bigger-130"></i><span class="bigger-110">
                                <%=Tokens.Inbox %></span> </a></li>
                            <li><a href="SentMail.aspx" data-target="sent"><i class="orange icon-location-arrow bigger-130 "></i><span class="bigger-110">
                                <%=Tokens.SentMail %></span> </a></li>
                        </ul>
                        <div class="tab-content no-border no-padding">
                            <div class="tab-pane in active">
                                <div class="message-container">

                                    <div id="id-message-new-navbar" class="message-navbar align-center clearfix">
                                        <div class="message-bar">
                                            <div class="message-toolbar">
                                            </div>
                                        </div>
                                        <div class="message-item-bar">

                                            <div class="messagebar-item-right">
                                                <span class="inline btn-send-message"></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="message-list-container">
                                        <div class="message-list hide" id="message-list">
                                        </div>
                                        <div id="message" style="background-color: #dff0d8; color: #468847; border-color: #d6e9c6; width: 100%">
                                            <asp:Label runat="server" ID="l_message" ClientIDMode="Static"></asp:Label>
                                        </div>
                                        <div id="id-message-form" class="form-horizontal message-form  col-xs-12">
                                            <div class="">
                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label no-padding-left" for="form-field-recipient">
                                                        <asp:Literal ID="Label1" runat="server" Text="<%$Resources:Tokens,To%>"></asp:Literal>:</label>
                                                    <div class="col-sm-9">
                                                        <span class="input-icon">
                                                            <div>
                                                                <asp:DropDownList ID="ddl_users"   CssClass="chosen-select" runat="server" Width="200px" ClientIDMode="Static">
                                                                </asp:DropDownList>
                                                                <button type="button" id="pick" class="btn btn-sm btn-danger">
                                                                    <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,Add%>" runat="server" />&nbsp;<i
                                                                        class="icon-user"></i></button>
                                                            </div>
                                                            <div>
                                                                <label>
                                                                    <asp:Literal ID="Literal2" Text="<%$Resources:Tokens,RecipientsList%>" runat="server" /></label>
                                                                <div>
                                                                    <asp:HiddenField runat="server" ID="ToContainer" ClientIDMode="Static" />
                                                                    <ul id="toSendTo">
                                                                    </ul>
                                                                </div>
                                                            </div>

                                                        </span>
                                                    </div>
                                                </div>
                                                <div class="hr hr-18 dotted">
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label no-padding-left" for="form-field-subject">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$Resources:Tokens,Subject%>"></asp:Label>:</label>
                                                    <div class="col-sm-6 col-xs-12">
                                                        <div class="input-icon block col-xs-12 no-padding">
                                                            <div>
                                                                <asp:TextBox runat="server" MaxLength="100" ID="tb_subject" CssClass="col-xs-12" placeholder="<%$Resources:Tokens,Subject %>"></asp:TextBox>

                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tb_subject"
                                                                    Display="Dynamic" ErrorMessage="<%$Resources:Tokens,Required%>"></asp:RequiredFieldValidator>
                                                            </div>



                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="hr hr-18 dotted">
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label no-padding-left">
                                                        <span class="inline space-24 hidden-480"></span>
                                                        <asp:Label ID="Label3" runat="server" Text="<%$Resources:Tokens,Message%>"></asp:Label>:
                                                    </label>
                                                    <div class="col-sm-9">
                                                               
     <%--    <cc:Editor ID="tb_message" runat="server" />--%>
                                                      <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tb_subject"
                                                            Display="Dynamic" ErrorMessage="<%$Resources:Tokens,Required%>"></asp:RequiredFieldValidator>
                                              --%>      
                                                      <%-- <textarea id="tb_message" dir="rtl" ClientIDMode="Static" runat="server" style="width:600px; height: 100px;"></textarea>
                                                        --%>
                                                 
                                                        <cc1:Editor  ID="Editor1" runat="server" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="Editor1"
                                                            Display="Dynamic" ErrorMessage="<%$Resources:Tokens,Required%>"></asp:RequiredFieldValidator>
                                                   
                                                        <asp:Label ID="Label4" runat="server" ForeColor="red" Text="*" Visible="false"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="hr hr-18 dotted">
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
                                                <div class="align-left">
                                                    <button id="b_send" type="button" class="btn btn-sm btn-danger" runat="server" onserverclick="b_send_Click" clientidmode="Static" style="width: 120px">
                                                        <i class="icon-arrow-right bigger-110"></i>&nbsp;<%=Tokens.Send %>
                                                    </button>

                                                </div>
                                                <div class="space">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- /.message-list-container -->
                                    <div class="message-footer clearfix hide">
                                        <div class="pull-right">
                                            151 messages total
                                        </div>
                                        <div class="pull-left">
                                            <div class="inline middle">
                                                page 1 of 16
                                            </div>
                                            &nbsp; &nbsp;
                                            <ul class="pagination middle">
                                                <li class="disabled"><span><i class="icon-step-backward middle"></i></span></li>
                                                <li class="disabled"><span><i class="icon-caret-left bigger-140 middle"></i></span>
                                                </li>
                                                <li><span>
                                                    <input value="1" maxlength="3" type="text">
                                                </span></li>
                                                <li><a href="#"><i class="icon-caret-right bigger-140 middle"></i></a></li>
                                                <li><a href="#"><i class="icon-step-forward middle"></i></a></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="hide message-footer message-footer-style2 clearfix">
                                        <div class="pull-right">
                                            simpler footer
                                        </div>
                                        <div class="pull-left">
                                            <div class="inline middle">
                                                message 1 of 151
                                            </div>
                                            &nbsp; &nbsp;
                                            <ul class="pagination middle">
                                                <li class="disabled"><span><i class="icon-angle-left bigger-150"></i></span></li>
                                                <li><a href="#"><i class="icon-angle-right bigger-150"></i></a></li>
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
            <div class="hide message-content" id="id-message-content">
                <div class="message-header clearfix">
                    <div class="pull-right">
                        <span class="blue bigger-125">Clik to open this message </span>
                        <div class="space-4">
                        </div>
                        <i class="icon-star orange2 mark-star"></i>&nbsp;
                        <img class="middle" alt="John's Avatar" src="assets/avatars/avatar.png" width="32">
                        &nbsp; <a href="#" class="sender">John Doe</a> &nbsp; <i class="icon-time bigger-110 orange middle"></i><span class="time">Today, 7:15 pm</span>
                    </div>
                    <div class="action-buttons pull-left">
                        <a href="#"><i class="icon-reply green icon-only bigger-130"></i></a><a href="#"><i
                            class="icon-mail-forward blue icon-only bigger-130"></i></a><a href="#"><i class="icon-trash red icon-only bigger-130"></i></a>
                    </div>
                </div>
                <div class="hr hr-double">
                </div>
                <div class="message-body">
                    <p>
                        Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                        incididunt ut labore et dolore magna aliqua.
                    </p>
                    <p>
                        Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip
                        ex ea commodo consequat.
                    </p>
                    <p>
                        Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu
                        fugiat nulla pariatur.
                    </p>
                    <p>
                        Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt
                        mollit anim id est laborum.
                    </p>
                    <p>
                        Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu
                        fugiat nulla pariatur.
                    </p>
                    <p>
                        Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip
                        ex ea commodo consequat.
                    </p>
                </div>
                <div class="hr hr-double">
                </div>
                <div class="message-attachment clearfix">
                    <div class="attachment-title">
                        <span class="blue bolder bigger-110">Attachments</span> &nbsp; <span class="grey">(2
                            files, 4.5 MB)</span>
                        <div class="inline position-relative">
                            <a href="#" data-toggle="dropdown" class="dropdown-toggle">&nbsp; <i class="icon-caret-down bigger-125 middle"></i></a>
                            <ul class="dropdown-menu dropdown-lighter pull-right">
                                <li><a href="#">Download all as zip</a> </li>
                                <li><a href="#">Display in slideshow</a> </li>
                            </ul>
                        </div>
                    </div>
                    &nbsp;
                    <ul class="attachment-list list-unstyled pull-right">
                        <li><a href="#" class="attached-file inline"><i class="icon-file-alt bigger-110 middle"></i><span class="attached-name middle">Document1.pdf</span> </a>
                            <div class="action-buttons inline">
                                <a href="#"><i class="icon-download-alt bigger-125 blue"></i></a><a href="#"><i class="icon-trash bigger-125 red"></i></a>
                            </div>
                        </li>
                        <li><a href="#" class="attached-file inline"><i class="icon-film bigger-110 middle"></i><span class="attached-name middle">Sample.mp4</span> </a>
                            <div class="action-buttons inline">
                                <a href="#"><i class="icon-download-alt bigger-125 blue"></i></a><a href="#"><i class="icon-trash bigger-125 red"></i></a>
                            </div>
                        </li>
                    </ul>
                    <div class="attachment-images pull-left">
                        <div class="vspace-sm-4">
                        </div>
                        <div>
                            <img width="36" alt="image 4" src="assets/images/gallery/thumb-4.jpg">
                            <img width="36" alt="image 3" src="assets/images/gallery/thumb-3.jpg">
                            <img width="36" alt="image 2" src="assets/images/gallery/thumb-2.jpg">
                            <img width="36" alt="image 1" src="assets/images/gallery/thumb-1.jpg">
                        </div>
                    </div>
                </div>
            </div>
            <!-- /.message-content -->
            <!-- PAGE CONTENT ENDS -->
        </div>
        <!-- /.col -->
    </div>
  
    <script type="text/javascript" src="/ckeditor/ckeditor.js"></script>
    <script type="text/javascript" src="/ckeditor/adapters/jquery.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".chosen-select").chosen();
})
        ;
        $('#id-disable-check').on('click', function () {
            var inp = $('#form-input-readonly').get(0);
            if (inp.hasAttribute('disabled')) {
                inp.setAttribute('readonly', 'true');
                inp.removeAttribute('disabled');
                inp.value = "This text field is readonly!";
            }
            else {
                inp.setAttribute('disabled', 'disabled');
                inp.removeAttribute('readonly');
                inp.value = "This text field is disabled!";
            }
        });


        $(".chosen-select").chosen();
        $('#chosen-multiple-style').on('click', function (e) {
            var target = $(e.target).find('input[type=radio]');
            var which = parseInt(target.val());
            if (which == 2) $('#form-field-select-4').addClass('tag-input-style');
            else $('#form-field-select-4').removeClass('tag-input-style');
        });
    </script>
    <script>
        //$(document).ready(function() {


        //    $("#tb_message").ckeditor();


        //});
        $(document).ready(function () {

          
           
            var button = $('#b_send');
            $(button).click(function (e) {
            
                if ($("#ToContainer").val() === "") {
                    e.preventDefault();
                    alert("يرجي اضافة المرسل اليه");
                } else {
                    return true;
                }
                return false;
            });


            var picker = $('#pick');
            $(picker).click(function () {
                var selection = $('#ddl_users').find(":selected").text();
              


                if (selection === "--اختر--") {
                    alert("اختر المرسل اليه");
                    return;
                } else {
                    var name = $('#ddl_users').find(":selected").text();
                    var id = $('#ddl_users').find(":selected").val();
                    var newItem = $("<li class=\"r-item\"></li>");
                    var itemText = $('<div class="item-div"></div>').html(name);

                    var delButton = $('<div>x</div>').addClass("delItem").attr("data-id", id).click(function () {
                        var currentId = ($(this).attr("data-id"));
                        var ids = $("#ToContainer").val();
                        var splited = ids.split(',');
                        var currentIndex = null;
                        for (var j = 0; j < splited.length; j++) {
                            if (parseInt(splited[j]) === parseInt(currentId)) {
                                currentIndex = j;
                            }
                        }
                        var newIds = [];
                        if (currentIndex !== null) {
                            for (var k = 0; k < splited.length; k++) {
                                if (k === currentIndex) {
                                    continue;
                                } else {
                                    newIds.push(splited[k]);
                                }
                            }
                        }
                        var concatenated = '';
                        for (var m = 0; m < newIds.length; m++) {
                            if (concatenated === '') {
                                concatenated = newIds[m];
                            } else {
                                concatenated = concatenated + "," + newIds[m];
                            }

                        }

                        $("#ToContainer").val(concatenated);
                        $(this).parent().fadeOut('300', function () {
                            $(this).remove();
                        });
                    });
                    $(newItem).append($(delButton));
                    newItem.append(itemText);
                    var allIds = $("#ToContainer").val();
                    var checker = allIds.split(',');
                    for (var i = 0; i < checker.length; i++) {
                        if (parseInt(checker[i]) === parseInt(id)) {
                            alert(name + " " + "مضاف من قبل");
                            return;
                        }
                    }
                    if (allIds === "") {
                        allIds = allIds + id;
                        $("#ToContainer").val(allIds);

                    } else {
                        allIds = allIds + "," + id;
                        $("#ToContainer").val(allIds);

                    }
                    $(newItem).appendTo($("#toSendTo"));
                }
            });

            if ($('#l_message').html() === '') {
                $('#message').css("border", "none");
            } else {
                $('#message')

                    .css("padding", "15px")
                    .css("-moz-box-shadow", "0 0 1px #888")
                    .css("-webkit-box-shadow", " 0 0 1px#888")
                    .css("box-shadow", "0 0 1px #888");
            }
        });
    </script>
</asp:Content>

