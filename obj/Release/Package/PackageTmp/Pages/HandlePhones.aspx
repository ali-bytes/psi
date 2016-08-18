<%@ Page Title="<%$Resources:Tokens,HandlePhones%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="HandlePhones.aspx.cs" Inherits="NewIspNL.Pages.HandlePhones" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        #field2
        {
            background-image: url("clock.png");
            background-position: right center;
            background-repeat: no-repeat;
            border: 1px solid #FFC030;
            color: #3090C0;
            font-weight: bold;
        }
        #AnyTime--field2
        {
            background-color: #EFEFEF;
            border: 1px solid #CCC;
        }
        #AnyTime--field2 *
        {
            font-weight: bold;
        }
        #AnyTime--field2 .AnyTime-btn
        {
            background-color: #F9F9FC;
            border: 1px solid #CCC;
            color: #3090C0;
        }
        #AnyTime--field2 .AnyTime-cur-btn
        {
            background-color: #FCF9F6;
            border: 1px solid #FFC030;
            color: #FFC030;
        }
        #AnyTime--field2 .AnyTime-focus-btn
        {
            border-style: dotted;
        }
        #AnyTime--field2 .AnyTime-lbl
        {
            color: black;
        }
        #AnyTime--field2 .AnyTime-hdr
        {
            background-color: #FFC030;
            color: white;
        }
    </style>
    <div style="line-height: 14px; padding: 10px;">
        <div id="message" style="background-color: #d6e9c6; color: #468847;
            margin: 5px;">
            <asp:Label runat="server" ID="l_message" ClientIDMode="Static"></asp:Label></div>
        <div class="view">
            <asp:Panel runat="server" ID="p_entery">
                <div class="page-header"><h1><asp:Literal runat="server" Text="<%$Resources:Tokens,Phones %>"></asp:Literal></h1></div>
                <div class="well">
                <div>
                    <asp:Label runat="server" ID="Label2" Text="<%$Resources:Tokens,Employee%>"></asp:Label></div>
                <div>
                    <asp:DropDownList runat="server" ID="ddl_eployees" Width="150px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Tokens,Required%>"
                                       ValidationGroup="search"         ControlToValidate="ddl_eployees"></asp:RequiredFieldValidator>
                </div>
                    <br/>
                    <div>
                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Tokens,Central %>"></asp:Label>
                                <div>
                                    <asp:DropDownList ID="ddl_centrals" CssClass="required-input" runat="server" >
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ControlToValidate="ddl_centrals" ID="RequiredFieldValidator20"
                                        runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>" ValidationGroup="search"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                <p>
                    <br/>
                    <asp:LinkButton runat="server" ID="b_search"  OnClick="b_search_Click" ValidationGroup="search"
                                 CssClass="btn btn-success"><i class="icon-search"></i>&nbsp;<asp:Literal runat="server" ID="lblb_search" Text="<%$Resources:Tokens,Search%>"  ></asp:Literal></asp:LinkButton>
                </p>
                </div>
                <div style="text-align: center">
                    <asp:GridView runat="server" ID="gv_items" CssClass="table table-bordered table-condensed"
                                  CellPadding="4" ClientIDMode="Static" 
                                  GridLines="Horizontal" OnDataBound="gv_items_DataBound" AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="l_Number" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="<%$Resources:Tokens,Name%>" />
                            <asp:BoundField DataField="Phone1" HeaderText="<%$Resources:Tokens,Phone%>" />
                            <asp:BoundField DataField="Governate" HeaderText="<%$Resources:Tokens,Governrate%>" />
                            <asp:BoundField DataField="Offer1" HeaderText="<%$Resources:Tokens,Offer1%>" />
                            <asp:BoundField DataField="Offer2" HeaderText="<%$Resources:Tokens,Offer2%>" />
                            <asp:BoundField DataField="State" HeaderText="<%$Resources:Tokens,State%>" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div>
                                        <asp:LinkButton ID="b_approve" runat="server" data-val="Approve" ToolTip="<%$Resources:Tokens,Approve%>"
                                                    ClientIDMode="Static" CausesValidation="False" CommandArgument='<%# Bind("Id") %>' data-rel="tooltip"
                                                    data-id='<%# Eval("Id") %>' CssClass="btn btn-success btn-sm"><i class="icon-ok icon-only bigger-120"></i></asp:LinkButton>
                                        <asp:LinkButton ID="b_pend" runat="server" data-val="Pend" ToolTip="<%$Resources:Tokens,Pend%>" data-rel="tooltip" CssClass="btn btn-primary btn-sm"
                                                    ClientIDMode="Static" CommandArgument='<%# Bind("Id") %>' data-id='<%# Eval("Id") %>'><i class="icon-wrench icon-only bigger-120"></i></asp:LinkButton>
                                        <asp:LinkButton ID="b_reject" runat="server" data-val="Reject" ToolTip="<%$Resources:Tokens,Reject%>" CssClass="btn btn-danger btn-sm" data-rel="tooltip"
                                                    ClientIDMode="Static" CommandArgument='<%# Bind("Id") %>' data-id='<%# Eval("Id") %>'><i class="icon-trash icon-only bigger-120"></i></asp:LinkButton>
                                        <asp:HiddenField ID="gv_hf_id" runat="server" Value='<%# Bind("Id") %>' />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <%= Tokens.NoResults %>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
                <asp:HiddenField ID="hf_rejectionId" runat="server" ClientIDMode="Static" />
                <asp:Panel runat="server" ID="p_behaviors">
                    <div id="register" style="padding: 15px;">
                        <fieldset>
                            <legend>
                                <asp:Label runat="server" Text="<%$Resources:Tokens,Approve%>" ID="ll"></asp:Label></legend>
                            <div>
                                <asp:Label runat="server" ID="Label3" Text="<%$Resources:Tokens,Mobile%>"></asp:Label></div>
                            <div>
                                <asp:TextBox runat="server" ID="tb_mobile" Width="209px"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="tb_mobile"
                                                            ErrorMessage="<%$Resources:Tokens,Required%>" ValidationGroup="app">*</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="tb_mobile"
                                                                Display="Dynamic" ErrorMessage="<%$Resources:Tokens,Least10 %>" ValidationExpression="^[0-9]{10,12}$"
                                                                ValidationGroup="app"></asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <asp:Label runat="server" ID="Label4" Text="<%$Resources:Tokens,Provider%>"></asp:Label></div>
                            <div>
                                <asp:DropDownList ID="ddl_provider" runat="server" Width="209px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddl_provider"
                                                            ErrorMessage="<%$Resources:Tokens,Required%>" ValidationGroup="app"></asp:RequiredFieldValidator>
                            </div>
                            <div>
                                <asp:Label runat="server" ID="Label5" Text="<%$Resources:Tokens,Ip.Package%>"></asp:Label></div>
                            <div>
                                <asp:DropDownList ID="ddl_ipPack" runat="server" Width="209px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="ddl_ipPack"
                                                            ErrorMessage="<%$Resources:Tokens,Required%>" ValidationGroup="app"></asp:RequiredFieldValidator>
                            </div>
                            <div>
                                <asp:Label runat="server" ID="Label6" Text="<%$Resources:Tokens,Governrate%>"></asp:Label></div>
                            <div>
                                <asp:DropDownList ID="ddl_city" runat="server" Width="209px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ControlToValidate="ddl_city"
                                                            ErrorMessage="*" ValidationGroup="app">*</asp:RequiredFieldValidator>
                            </div>
                                              <div>
                                <asp:Label runat="server" ID="Label15" Text="<%$Resources:Tokens,NationalId%>"></asp:Label></div>
                            <div>
                                <asp:TextBox ID="txtNationalId" runat="server" />

                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator11" ControlToValidate="txtNationalId"
                                                            ErrorMessage="*" ValidationGroup="app">*</asp:RequiredFieldValidator>
                            </div>
                                              <div>
                                <asp:Label runat="server" ID="Label16" Text="<%$Resources:Tokens,LineOwner%>"></asp:Label></div>
                            <div>
                                <asp:TextBox ID="txtLineOwner" runat="server" />

                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator13" ControlToValidate="txtLineOwner"
                                                            ErrorMessage="*" ValidationGroup="app">*</asp:RequiredFieldValidator>
                            </div>
                            <div>
                                <asp:Label runat="server" ID="Label7" Text="<%$Resources:Tokens,Address%>"></asp:Label></div>
                            <div>
                                <asp:TextBox runat="server" ID="tb_address" Width="209px"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ErrorMessage="<%$Resources:Tokens,Required%>"
                                                            ControlToValidate="tb_address" ValidationGroup="app"></asp:RequiredFieldValidator>
                            </div>
                            <div>
                                <asp:Label runat="server" ID="Label8" Text="<%$Resources:Tokens,Email%>"></asp:Label></div>
                            <div>
                                <asp:TextBox runat="server" ID="tb_email" Width="209px"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8" ControlToValidate="tb_email"
                                                            ErrorMessage="<%$Resources:Tokens,Required%>" ValidationGroup="app"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="tb_email"
                                                                Display="Dynamic" ErrorMessage="<%$Resources:Tokens,ValidMail%>" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                                ValidationGroup="app"></asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <asp:Label runat="server" ID="Label9" Text="<%$Resources:Tokens,Service.Package%>"></asp:Label></div>
                            <div>
                                <asp:DropDownList ID="ddk_servicePack" runat="server" Width="209px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator9" ControlToValidate="ddk_servicePack"
                                                            ErrorMessage="<%$Resources:Tokens,Required%>" ValidationGroup="app"></asp:RequiredFieldValidator>
                            </div>
                            <div>
                                <asp:Label runat="server" ID="Label10" Text="<%$Resources:Tokens,PaymentType%>"></asp:Label></div>
                            <div>
                                <asp:DropDownList ID="ddl_paymentType" runat="server" Width="209px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator10" ControlToValidate="ddl_paymentType"
                                                            ErrorMessage="*" ValidationGroup="app">*</asp:RequiredFieldValidator>
                            </div>
                            <div>
                                <asp:Label runat="server" ID="Label11" Text="<%$Resources:Tokens,Notes%>"></asp:Label></div>
                            <div>
                                <asp:TextBox runat="server" ID="tb_notes" Height="60px" TextMode="MultiLine" Width="400px"></asp:TextBox>
                                <p>
                                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="<%$Resources:Tokens,Save%>"
                                                ValidationGroup="app" Width="90px" CssClass="btn btn-primary"/></p>
                            </div>
                        </fieldset>
                    </div>
                    <div id="pending" style="padding: 15px;">
                        <fieldset>
                            <legend>
                                <asp:Label runat="server" Text="<%$Resources:Tokens,Pend%>" ID="Label1"></asp:Label></legend>
                            <div>
                                <div>
                                    <asp:Label runat="server" ID="Label12" Text="<%$Resources:Tokens,Reason%>"></asp:Label></div>
                                <div>
                                    <asp:DropDownList ID="ddl_resons" runat="server" Width="209px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="ddl_resons"
                                                                ErrorMessage="<%$Resources:Tokens,Required%>" ValidationGroup="Pend"></asp:RequiredFieldValidator>
                                </div>
                                <asp:Literal runat="server" Text="<%$Resources:Tokens,Date_Time %>"></asp:Literal>
                                <div style="margin-bottom: 10px;">

                                    <asp:TextBox runat="server" ID="tb_nextAppointment"  Width="200px"  ClientIDMode="Static"/>

                                </div>
                                <asp:Label runat="server" ID="lblcom" Text="<%$Resources:Tokens,Comment %>"></asp:Label>
                                <div>
                                    <asp:TextBox runat="server" ID="txtComment" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                </div>
                                <p>
                                    <asp:Button runat="server" Text="<%$Resources:Tokens,Save%>" ID="b_pendSave" OnClick="b_pendSave_Click"
                                                Width="90px" ValidationGroup="Pend" CssClass="btn btn-primary"/></p>
                            </div>
                        </fieldset>
                    </div>
                    <div id="reject" style="padding: 15px;">
                        <fieldset>
                            <legend>
                                <asp:Label runat="server" Text="<%$Resources:Tokens,Reject%>" ID="Label13"></asp:Label></legend>
                            <div>
                                <div>
                                    <asp:Label runat="server" ID="Label14" Text="<%$Resources:Tokens,Reason%>"></asp:Label></div>
                                <div>
                                    <asp:DropDownList ID="ddl_reject2" runat="server" Width="209px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator12" ControlToValidate="ddl_reject2"
                                                                ErrorMessage="<%$Resources:Tokens,Required%>" ValidationGroup="reject"></asp:RequiredFieldValidator>
                                </div>
                                                                <asp:Label runat="server" ID="Label17" Text="<%$Resources:Tokens,Comment %>"></asp:Label>
                                <div>
                                    <asp:TextBox runat="server" ID="txtRejectComment" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                </div>
                                <p>
                                    <asp:Button runat="server" Text="<%$Resources:Tokens,Save%>" ID="b_saveReject" OnClick="b_saveReject_Click"
                                                Width="90px" ValidationGroup="reject" CssClass="btn btn-primary"/></p>
                            </div>
                        </fieldset>
                    </div>
                </asp:Panel>
            </asp:Panel>
        </div>
    </div>
        <script src="../Content/ace-assest/js/jquery-ui-1.10.3.full.min.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $('#tb_nextAppointment').datepicker({ dateFormat: 'dd-mm-yy' });
            if ($('#l_message').html() === '') {
                $('#message').css("border", "none");
            } else {
                $('#message').css("border", "silver solid 1px").css("padding", "4px")
                    .css("-moz-box-shadow", "0 0 1px #888")
                    .css("-webkit-box-shadow", "0 0 1px #888")
                    .css("box-shadow", "0 0 1px #888");
            }
            $("#gv_items tr").not(':first').hover(function () {
                $(this).css("background-color", "rgb(243, 255, 195)");
            }, function () {
                $(this).css("background-color", "");
            });


            var dlg = $('#register').dialog({
                autoOpen: false,
                width: 600,
                height: "auto",
                resizable: false,
                draggable: false,
                title: 'Register',
                modal: true,
                position: 'center'
            });
            dlg.parent().appendTo(jQuery("form:first"));
            $('a[data-val="Approve"]').click(function () {
                var x = $(this).attr("data-id");
                $('#hf_rejectionId').val(x);
                $('#register').dialog('open');
                return false;
            });

            var pendingDialog = $('#pending').dialog({
                autoOpen: false,
                width: 600,
                height: "auto",
                resizable: false,
                draggable: false,
                title: 'Pending',
                modal: true,
                position: 'center'
            });
            pendingDialog.parent().appendTo(jQuery("form:first"));
            $('a[data-val="Pend"]').click(function () {
                var x = $(this).attr("data-id");
                $('#hf_rejectionId').val(x);
                $('#pending').dialog('open');
                return false;
            });


            var rejectDialog = $('#reject').dialog({
                autoOpen: false,
                width: 600,
                height: "auto",
                resizable: false,
                draggable: false,
                title: 'Rejection',
                modal: true,
                position: 'center'
            });
            rejectDialog.parent().appendTo(jQuery("form:first"));
            $('a[data-val="Reject"]').click(function () {
                var x = $(this).attr("data-id");
                $('#hf_rejectionId').val(x);
                $('#reject').dialog('open');
                return false;
            });
        });
    </script>
</asp:Content>
