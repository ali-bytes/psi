<%@ Page Title="<%$Resources:Tokens,Home %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="NewIspNL.Pages.home" %>

<%@ Register Src="~/WebUserControls/Notifications.ascx" TagPrefix="uc" TagName="Notifications" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script src="../Content/ace-assest/css/Nossair/Home.js" type="text/javascript"></script>
    <style type="text/css">
         .ncal {
             position: absolute;
             left: 10px;
         }

        .m:hover {
            background: white;
            border-color: cadetblue;
        }
        .center-icon {
            margin-right: 50%;
            margin-left: 50%
        }
    </style>
    <div class="row">
        <div class="alert alert-block alert-info m">
            <button type="button" class="close" data-dismiss="alert">
                <i class="icon-remove"></i>
            </button>

            <i class="icon-user"></i>
            <asp:Label ID="homee" runat="server"><%=Tokens.WelcomeMsg %></asp:Label>
            :
            <asp:Label ID="username" runat="server"></asp:Label>

        </div>
        <div class="col-sm-12 col-xs-12">
            <uc:Notifications runat="server" ID="Notifications1" Visible="False" />
        </div>
          <% if (ShowOffers)
           { %>
        <div class="col-md-6" style="height: 440px; width: 50%" runat="server" id="LastOffers" clientidmode="Static" Visible="False">
            <div class="col-sm-6">
                <div class="widget-box col-sm-12 col-xs-12 " style="width: 200%; height: 100%">
                    <div class="widget-header col-sm-12 col-xs-12" style="height: 100%">
                        <h4 class="lighter smaller">
                            <a href="LastOffers.aspx" target="_blank">
                                <i class="icon-comment blue"></i>
                                <%=Tokens.CurrentOffers %></a>
                        </h4>
                    </div>
                    <div class="widget-body col-sm-12 col-xs-12" style="height: 100%">
                        <div class="widget-main no-padding" style="height: 100%">

                            <div style="position: relative; overflow: hidden; width: auto; height: 405px;">
                                <div class="dialogs" style="height: 390px; overflow: hidden; width: auto;">
                                    <asp:DataList ID="DataList1" runat="server" ShowFooter="False" Width="100%"
                                        ShowHeader="False" CellPadding="4" ForeColor="#333333">
                                        <AlternatingItemStyle BackColor="White" />
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <ItemStyle BackColor="#EFF3FB" />
                                        <ItemTemplate>
                                            <div class="itemdiv dialogdiv" style="height: 100%">
                                                <div class="user" style="height: 100%">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# Bind("ImageUrl") %>' />
                                                </div>
                                                <div class="body">
                                                    <div class="name">
                                                        <asp:Label ID="offername" ForeColor="#2283c5" text-align="justify" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                    </div>
                                                    <br />
                                                    <div class="tools" style="">
                                                        <a href="#">
                                                            <asp:LinkButton CommandArgument='<%# Eval("Id") %>' OnClick="Redirect" ID="more" runat="server"> <%=Tokens.more%><i class="icon-only icon-share-alt"></i></asp:LinkButton>
                                                        </a>
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    </asp:DataList>
                                </div>
                                <div style="transform-origin: 0 100%; height: 3px; background: #e0e8eb;"></div>
                                <div class="tools" style="margin-right: 35%; margin-top: 10%">
                                    <h4><a runat="server" id="addoffer" href="AddOffer.aspx" target="_blank">
                                        <i class="icon-plus-sign"><%=Tokens.LastOffersEntry%></i>
                                    </a></h4>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
         <%  } %>
        <% if (ShowCal)
           { %>
        <div class="col-md-6 col-sm-12 col-xs-12" id="cal" runat="server" Visible="False">
            <div class="widget-box" style="width: 100%; height: 100%">
                <div class="widget-header" style="height: 100%">
                    <h4 class="lighter smaller">
                        <i class="icon-calendar blue"></i>
                        <%=Tokens.mycalendar %>
                    </h4>
                </div>
                <div id="fr" class="col-md-6 col-sm-12 col-xs-12" style="margin-right: 50%; margin-top: 3%">
                    <iframe class="ncal" frameborder="0" width="200%" height="450px" src="../Calendar.aspx"></iframe>
                </div>
            </div>
        </div>
       <%  } %>
    </div>
        <br />
        <hr />
     <% if (ShowNotes)
           { %>
     <div class="row col-lg-12 col-md-12 col-sm-12 col-xs-12" id="Div1" runat="server" clientidmode="Static" Visible="False">
            <div class="widget-box col-md-12 col-sm-12 col-xs-12" style="width: 100%; height: 100%">
                <div class="widget-header" style="height: 100%">
                    <h4 class="lighter smaller">
                        <i class="icon-tasks blue"></i>
                        <%=Tokens.CutomerNotes %>
                    </h4>
                </div>
                <div class="view" id="result">
                    <fieldset>
                        <div runat="server" id="Msg">
                        </div>
                        <div>
                            <asp:GridView ShowHeader="False" runat="server" ID="GvResults" AutoGenerateColumns="False" ClientIDMode="Static"
                                CssClass="table table-bordered table-condensed" OnRowDataBound="GvResults_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="LNo" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div class="success" >
                                                <div>
                                                    <b>
                                                        <%= Tokens.Name %></b>:&nbsp;<span><asp:Literal Text='<%#Eval("Name") %>' runat="server" /></span>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div class="pull">
                                                <div>
                                                    <b>
                                                        <%= Tokens.Phone %></b>:&nbsp;<span><asp:Literal Text='<%#Eval("Phone") %>' runat="server" /></span>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField>
                                        <ItemTemplate>
                                             <div>
                                                    <b>
                                                        <%= Tokens.State %></b>:&nbsp;<span><asp:Literal Text='<%#Eval("State") %>' runat="server" /></span>
                                                </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                             <div>
                                                    <b>
                                                        <%= Tokens.Branch %></b>:&nbsp;<span><asp:Literal Text='<%#Eval("Branch") %>' runat="server" /></span>
                                                </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                             <div>
                                                    <b>
                                                        <%= Tokens.User %></b>:&nbsp;<span><asp:Literal Text='<%#Eval("User") %>' runat="server" /></span>
                                                </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                             <div>
                                                    <b>
                                                        <%= Tokens.Done %></b>:&nbsp;<span><asp:Literal Text='<%#Eval("TProcessed") %>' runat="server" /></span>
                                                </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                              <div style="color: #000094;">
                                                    <b>
                                                        <%= Tokens.CustomerNote %></b>:&nbsp;<span><asp:Literal Text='<%#Eval("Note") %>'
                                                            runat="server" /></span>
                                                </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                              <% if (CanProcess)
                                                   {%>
                                                <button runat="server" type="button" class="btn btn-primary btn-sm" data-select="s" data-id='<%#Eval("NoteId") %>' title="<%$Resources:Tokens,Process %>" data-rel="tooltip" clientidmode="Static">
                                                    <i class="icon-fire icon-only"></i>
                                                </button>
                                                <%  } %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
     <%  } %>
    <asp:HiddenField runat="server" ID="hdnFrom" ClientIDMode="Static" />
    <div id="his-dialog" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div style="background-color: white; margin-right: 300px; margin-top: 200px; width: 500px; height: 200px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                        ×</button>
                    <div style="margin-right: 200px;">
                        <label style="color: cadetblue; font-family: GESSTwoMedium; font-size: 150%; font-weight: bold;"><%=Tokens.Caution %></label>
                    </div>
                    <br />
                    <h4 id="H5">
                        <div style="margin-right: 150px;">
                            <asp:Label ID="adstitle" runat="server" Text="Label"></asp:Label>
                        </div>
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <div class="well">
                            <asp:Label ID="adsdetails" runat="server" Text="Label"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
      <div id="note-modal" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="note-note-modal"
        aria-hidden="true">
                       <div class="modal-dialog">
            <div class="modal-content">
        <div class="modal-header">
            <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                &times;</button>
            <h3 id="note-note-modal">
                <%= Tokens.Done %>
            </h3>
        </div>
        <div class="modal-body">
            <div class="bootbox-body">
            <div class="well">
                <div>
                    <div>
                        <asp:TextBox runat="server" ID="TbComment" ValidationGroup="c"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RTbComment" ValidationGroup="c" ErrorMessage="<%$Resources:Tokens,Required %>"
                            ControlToValidate="TbComment" runat="server" />
                    </div>
                </div>
            </div>
            </div>
        </div>
        <div class="modal-footer">
            <button id="BProcessNote" runat="server" causesvalidation="True" validationgroup="c"
                class="btn btn-primary">
                <%= Tokens.Done %></button>
            <span id="cancelReactivate" class="btn" data-dismiss="modal">
                <%= Tokens.Cancel %></span>
        </div>
        </div>
        </div>
    </div>
     <input type="hidden" id="selected" runat="server" value="0" ClientIDMode="Static" />
    <script type="text/javascript">

        $(document).ready(function () {

            $("#his-dialog").modal('show');

            $('button[data-select="s"]').on('click', function () {
                var id = $(this).attr('data-id');
                $('#selected').val(id);
                console.log(id);
                $('#note-modal').modal('show');
            });

            if (window.matchMedia('(max-width: 800px)').matches) {
                $('#footerdiv').css({
                    position: 'fixed',
                    bottom: 0,
                    left: 0
                    //z-index : '2',
                    //bottom : '0',
                    //left : '0',
                    //width : '100%',
                    //height : '48px',
                    //padding : '0'
                });
                $('#LastOffers').after($('#Div1'));
               
                //$('#Div1').css('display', 'none');
                //$('#GvResults').css('display', 'none');
                $('#GvResults').css('width', 320).css('display', 'block').css('overflow-x', 'scroll');

            } else {
                //...
            }
            if (window.matchMedia('(min-device-width : 200px) and (max-device-width : 800px)').matches) {
                $('#fr').css('width', '50%');
            }
        });


    </script>



</asp:Content>
