<%@ Page Title="<%$Resources:Tokens,EmailMessages %>"  validateRequest="false"  EnableEventValidation="false" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="EmailMessages.aspx.cs" Inherits="NewIspNL.Pages.EmailMessages" %>
<%@ Import Namespace="Resources" %>
 
 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style type="text/css">
        .delItem
        {
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
                .r-item
        {
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
        .item-div
        {
            height: auto;
        }
        .btn-toolbar {
  display: block;
  float: left;
  margin-top: -4px; /* Recover part of margin set in child Header nodes */
  margin-left: 10px;
  font-size: 20px;
  font-weight: 200;
  color: #777777;
  text-shadow: 0 1px 0 #ffffff;
}
    </style>
    <div class="page-header"><h1>
        <asp:Label runat="server" Text="<%$Resources:Tokens,EmailMessages %>"></asp:Label></h1>
    </div>
    <div id="lblMessgae" runat="server"></div>
<button class="btn-new-mail btn btn-purple" id="addNewMessage" type="button" ><i class=" icon-envelope bigger-130"></i><%=Tokens.MessageTo %></button>
                               <br/>
    <br />    
<asp:GridView ID="gvEmails" runat="server" OnRowDataBound="OnRowDataBound" DataKeyNames="MessageNumber"
    AutoGenerateColumns="false" allowpaging="true" PageSize="100" OnPageIndexChanging="grdData_PageIndexChanging" CssClass="table table-bordered table-condensed center">
   <Columns>
       <asp:BoundField HeaderText="#" DataField="MessageNumber"/>
        <asp:BoundField HeaderText="From" DataField="From" HtmlEncode="false" />
        <asp:TemplateField HeaderText="Subject">
            <ItemTemplate>
                <a ID="lnkView" runat="server" ClientIDMode="Static"><%# Eval("Subject") %></a>
                <span class="body" style="display: none">
                    <%# Eval("Body") %></span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="Date" DataField="DateSent" />
        <asp:TemplateField ItemStyle-CssClass="Attachments">
            <ItemTemplate>
                <asp:Repeater ID="rptAttachments" runat="server">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkAttachment" runat="server" OnClick="Download" Text='<%# Eval("FileName") %>' />
                    </ItemTemplate>
                    <SeparatorTemplate>
                        <br>
                    </SeparatorTemplate>
                </asp:Repeater>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<div id="dialog" style="display: none">
    <span id="body"></span>
    <br />
    <span id="attachments"></span>
</div>
<div id="modalAddNewMessage" class="bootbox modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
                        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                             <div class="modal-header">
            <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                ×</button>
            <h4 id="H7">
                <%= Tokens.NewMessage %></h4>
        </div>
                             <div class="modal-body">
                                 <div class="bootbox-body">
                                     <div class="col-xs-12">
                                        

                                       <%--  <div class="input-group input-group-sm">
                                              <span class="input-group-addon" id="basic-addon3"><%= Tokens.To%></span>
                                                   <asp:TextBox  ID="txtTo" CssClass="form-control" placeholder="Email@Example.com" aria-describedby="basic-addon3" runat="server" ClientIDMode="Static"/>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="أدخل الايميل بشكل صحيح"
                                        ControlToValidate="txtTo" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        Display="Dynamic"></asp:RegularExpressionValidator>
                                              </div>
                                         <a href="#addmore" id="pickr" class="btn btn-sm btn-primary" style="display: inline-block" data-toggle="collapse" data-rel="tooltip" title="<%=Tokens.Add%>">&nbsp;<i
                                                                        class="icon-user"></i></a>--%>
                                                     
                                            <div class="input-group">
                                              <span class="input-group-addon"><%= Tokens.To%></span>
                                             <asp:TextBox  ID="txtTo" CssClass="form-control" Height="48px" placeholder="Email@Example.com" runat="server" ClientIDMode="Static"/>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="أدخل الايميل بشكل صحيح"
                                        ControlToValidate="txtTo" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        Display="Dynamic"></asp:RegularExpressionValidator>
                                              <span class="input-group-addon">
                                                  <a href="#addmore" id="pickr" class="btn btn-sm btn-primary" data-toggle="collapse" data-rel="tooltip" title="<%=Tokens.Add%>">&nbsp;<i
                                                     class="icon-user"></i></a>
                                             </span>
                                            </div>
                                                         
                                         <br/>
                                        <div id="addmore" class="collapse col-xs-12">
                                             <label> <asp:Literal ID="Literal2" Text="<%$Resources:Tokens,RecipientsList%>" runat="server" /></label>
                                           <div class="input-group input-group-sm">
                                            
                                             <span class="input-group-addon" id="basic-addon1">CC</span>
                                             <asp:TextBox  ID="txtcc" CssClass="form-control" placeholder="Add Email" aria-describedby="basic-addon1"  runat="server" ClientIDMode="Static"/>
                                       
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="أدخل الايميل بشكل صحيح"
                                            ControlToValidate="txtcc" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"></asp:RegularExpressionValidator>
                                       
                                            
                                            <span class="input-group-addon" id="basic-addon2">CC2</span>
                                             <asp:TextBox  ID="txtcc2" class="form-control" placeholder="Add Email" aria-describedby="basic-addon2"  runat="server" ClientIDMode="Static"/>
                                        
                                             <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="أدخل الايميل بشكل صحيح"
                                        ControlToValidate="txtcc2" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        Display="Dynamic"></asp:RegularExpressionValidator>
                                         
                                               </div>
                                                               
                                                           <%--     <div class="col-xs-12"><asp:HiddenField runat="server" ID="ToContainer" ClientIDMode="Static" /></div>--%>
                                          </div>
                                         <br/>
                                          </div>
                                               <div class="col-xs-12 input-group input-group-sm">
                                                    <span class="input-group-addon" id="basic-addon4"><%= Tokens.Subject%></span>
                                                                                
                        <asp:TextBox runat="server" MaxLength="100" class="form-control" aria-describedby="basic-addon4" ID="txtSubject" placeholder="<%$Resources:Tokens,Subject %>"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSubject"
                            Display="Dynamic" ErrorMessage="<%$Resources:Tokens,Required%>" ValidationGroup="newms"></asp:RequiredFieldValidator>
                    
                    </div>
                                     <br />
                                     <span class="divider"></span>
                                     <hr>
                                                           <div class="col-xs-12">
                                                               <hr/>
                                                               <span class="label label-info"><%= Tokens.Message%></span>
                                                       <%--<asp:Label ID="Label3" runat="server" Text="<%$Resources:Tokens,Message%>"></asp:Label>:--%>
                                                   
                                                    <div class="col-xs-12">
                                                         <textarea id="tb_message" dir="rtl" ClientIDMode="Static" rows="10" cols="6" runat="server"></textarea>
                                                      
                                                    </div>
                                                    </div>
                                               
            </div>
        </div>  
                             <div class="modal-footer">
            <asp:Button runat="server" ID="Button2" ValidationGroup="newms"
                CssClass="btn btn-info" Text="<%$Resources:Tokens,Send %>" 
                OnClick="b_send_Click"/>
            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="True"><%=Tokens.Cancel %></button>
        </div>
        </div>
        </div>
                </div>

<div id="subjectModal" style="background-color: lightseagreen;" class="bootbox modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
                        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                             <div class="modal-header">
            <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                ×</button>
            <h4 id="MessageTitle"></h4>
        </div>
                             <div class="modal-body">
                                 <div class="bootbox-body">
                                     <div id="bodyId"></div>
            </div>
                                 <label id="replayto"></label>
                                 <a href="#replaydiv" class="btn btn-default" data-toggle="collapse"><%=Tokens.Reply %></a>
                                 <div id="replaydiv" class="collapse">
                                      <div class="col-xs-12">
                                        <textarea id="replay_message" dir="rtl" ClientIDMode="Static" rows="10" cols="6" runat="server"></textarea>
                                      </div>
                                    
                                     <div class="span7 text-center">
                                     <asp:Button ID="btnReplay" runat="server" Text="<%$Resources:Tokens,Send %>" CssClass="btn btn-info" OnClick="btnReplay_Click" />
                                     </div>
                                    </div>
                                 
        </div>  
                             <div class="modal-footer">
            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="True"><%=Tokens.Cancel %></button>
        </div>
        </div>
        </div>

                </div>
    <asp:HiddenField ID="mail_num" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="mail_subject" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="old_message" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="old_date" runat="server" ClientIDMode="Static" />
    

      <script type="text/javascript" src="/ckeditor/ckeditor.js"></script>
    <script type="text/javascript" src="/ckeditor/adapters/jquery.js"></script>

<script type="text/javascript">
    $(document).ready(function() {
        $("#tb_message").ckeditor();
        $("#replay_message").ckeditor();
    });

    //$(function () {
    //    $(".table > tbody > tr:not(:has(table, th))")
    //        .css("cursor", "pointer")


    //        .click(function (e) {
    //            $(".table td").removeClass("highlite");

    //            var $cell = $(e.target).closest("td");
    //            $cell.addClass('highlite');

    //            var $currentCellText = $cell.text();

    //            var $leftCellText = $cell.prev().text();
    //            var $rightCellText = $cell.next().text();
    //            var $bodyCellText = $cell.next().next().next().text();

    //            var $colIndex = $cell.parent().children().index($cell);
    //        alert("current :" + $currentCellText);
    //        alert("prev left :" + $leftCellText);
    //        alert("next body :" + $bodyCellText);
            //    var $colName = $cell.closest("table")
            //        .find('th:eq(' + $colIndex + ')').text();
            //$("#para").empty()
            //    .append("<b>Current Cell Text: </b>"
            //        + $currentCellText + "<br/>")
            //    .append("<b>Text to Left of Clicked Cell: </b>"
            //        + $leftCellText + "<br/>")
            //    .append("<b>Text to Right of Clicked Cell: </b>"
            //        + $rightCellText + "<br/>")
            //    .append("<b>Column Name of Clicked Cell: </b>"
    //        //        + $colName);
    //    });

    //});
    /*$("[id*=lnkView]").live("click", function () {
        var subject = $(this).text();
        var row = $(this).closest("tr");
        $("#body").html($(".body", row).html());
        $("#attachments").html($(".Attachments", row).html());
        $("#dialog").dialog({
            title: subject,
            buttons: {
                Ok: function () {
                    $(this).dialog('close');
                }
            }
        });
        return false;
    });*/
    //$("table.table").each(function () {
    //    var currentPage = 0;
    //    var numPerPage = 50;
    //    var $table = $(this);
    //    $table.bind('repaginate', function () {
    //        $table.find('tbody tr').hide().slice(currentPage * numPerPage, (currentPage + 1) * numPerPage).show();
    //    });
    //    $table.trigger('repaginate');
    //    var numRows = $table.find('tbody tr').length;
    //    var numPages = Math.ceil(numRows / numPerPage);
    //    var $pager = $("<ul class='pagination'></ul>");
    //    for (var page = 0; page < numPages; page++) {
    //        var num = page + 1;
    //        $("<li></li>").html("<a>" + num + "</a>").bind('click', {
    //                newPage: page
    //            }, function (event) {
    //                currentPage = event.data['newPage'];
    //                $table.trigger('repaginate');
    //                $(this).addClass('active').siblings().removeClass('active');
    //            }).appendTo($pager); //.addClass('clickable');
    //    }
    //    $pager.insertBefore($table).find('span.page-number:first').addClass('active');
    //});


    //var ClickCount = 0;
    //var picker = $("#pick");
    //$(picker).click(function () {
    //    var selection = $("#txtTo").text();
    //    if (selection === String.empty) {
    //        alert("أدخل المرسل اليه");
    //    } else {
    //        var name = $("#txtTo").val();
    //        /*var ms = IsEmail(name);
    //        if (ms===true) {
    //            alert("أدخل الايميل بشكل صحيح");
    //            return false;
    //        }*/
    //        ClickCount++;
    //        var id = $("#txtTo").val();
    //        var newItem = $("<li class=\"r-item\"></li>");
    //        var itemText = $('<div class="item-div"></div>').html(name);

    //        var delButton = $('<div>x</div>').addClass("delItem").attr("data-id", id).click(function () {
    //            var currentId = ($(this).attr("data-id"));
    //            var ids = $("#ToContainer").val();
    //            var splited = ids.split(',');
    //            var currentIndex = null;
    //            for (var j = 0; j < splited.length; j++) {
    //                if (parseInt(splited[j]) === parseInt(currentId)) {
    //                    currentIndex = j;
    //                }
    //            }
    //            var newIds = [];
    //            if (currentIndex !== null) {
    //                for (var k = 0; k < splited.length; k++) {
    //                    if (k === currentIndex) {
    //                        continue;
    //                    } else {
    //                        newIds.push(splited[k]);
    //                    }
    //                }
    //            }
    //            var concatenated = '';
    //            for (var m = 0; m < newIds.length; m++) {
    //                if (concatenated === '') {
    //                    concatenated = newIds[m];
    //                } else {
    //                    concatenated = concatenated + "," + newIds[m];
    //                }

    //            }

    //            $("#ToContainer").val(concatenated);
    //            $(this).parent().fadeOut('300', function () {
    //                $(this).remove();
    //            });
    //        });
    //        $(newItem).append($(delButton));
    //        newItem.append(itemText);
    //        var allIds = $("#ToContainer").val();
    //        var checker = allIds.split(',');
    //        for (var i = 0; i < checker.length; i++) {
    //            if (parseInt(checker[i]) === parseInt(id)) {
    //                alert(name + " " + "مضاف من قبل");
    //                return;
    //            }
    //        }
    //        if (allIds === "") {
    //            allIds = allIds + id;
    //            $("#ToContainer").val(allIds);

    //        } else {
    //            allIds = allIds + "," + id;
    //            $("#ToContainer").val(allIds);

    //        }
    //        $(newItem).appendTo($("#toSendTo"));
    //        $("#txtTo").val("");
    //    }
    //    if (ClickCount === 3) {
    //        $("#txtTo").hide();
    //    }
    //});
    $("#addNewMessage").click(function () {
        $("#modalAddNewMessage").modal("show");
    });
    $("[id*=lnkView]").click(function (event) {
        var subject = $(this).text();
        var row = $(this).closest("tr");
        $("#bodyId").html($(".body", row).html());
        $("#MessageTitle").html(subject);
        var cell = $(event.currentTarget);
        var row1 = cell.parent().parent();
        var mailNum = row1.children()[0];
        var  mailSubject = row1.children()[2];
        var  mailDate = row1.children()[3];
        $("#mail_num").val(mailNum.innerText);
        $("#mail_subject").val(mailSubject.innerText);
        $("#old_message").val($(".body", row).html());
        $("#old_date").val(mailDate.innerText);

        $("#subjectModal").modal("show");
    });
    //var ToCount = $("#toSendTo li").length();

    function IsEmail(email) {
        var regex = new RegExp("\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        if (!regex.test(email)) {
            return false;
        } else {
            return true;
        }
    }

</script>

</asp:Content>

