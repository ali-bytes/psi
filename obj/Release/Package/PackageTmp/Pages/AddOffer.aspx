<%@ Page Title="<%$ Resources:Tokens,LastOffersEntry%>"  ValidateRequest="false" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="AddOffer.aspx.cs" Inherits="NewIspNL.Pages.AddOffer" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit.HTMLEditor" Assembly="AjaxControlToolkit, Version=4.1.7.123, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script>
        CKEDITOR.on('instanceReady', function () {
            $.each(CKEDITOR.instances, function (instance) {
                CKEDITOR.instances[instance].on("change",
function (e) {
    for (instance in CKEDITOR.instances
)
        CKEDITOR.instances[instance].updateElement();
});
            });
        });
    </script>    
   <%-- <script>
        //Replace the <textbox id="HTMLEditorBody"> with a CKEditor instance, using the default configuration. PCG. 05132014.
        CKEDITOR.replace('<% = Editor1.ClientID %>');
        CKEDITOR.config.height = 500; //This will force the height of the CKEditor window to match the height of the HTMLEditorBody asp Textbox. PCG. 07142017.
      </script>--%>
    <style type="text/css">
        img[data-select="offer-image"]
        {
            max-width: 80px;
        }
    </style>
    <div class="alert alert-danger" runat="server" ID="MsgError">
    </div>
    <div class="alert alert-success" runat="server" ID="MsgSuccess"></div>
    <div class="view" data-select="0">
        <fieldset>
            <div class="page-header">
                <h1><asp:Label runat="server" Text="<%$Resources:Tokens,OffersEntry %>"></asp:Label></h1>
            </div>
            <div>
                <div>
                    <button type="button" runat="server" ID="BAdd" CausesValidation="False" class="btn btn-primary">
                        <%= Tokens.Add %></button>
                </div>
                <asp:GridView runat="server" ID="GvOffers" AutoGenerateColumns="False" CssClass="table table-bordered  table-condensed">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LNo" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" HeaderText="<%$Resources:Tokens,Name %>" />
                        <asp:BoundField DataField="Brief" HeaderText="<%$Resources:Tokens,Brief %>" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Image data-select="offer-image" runat="server" ID="Img" ImageUrl='<%#Eval("ImageUrl") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div style="width: 150px">
                                    <button validationgroup='<%#Eval("Id") %>' runat="server" id="BEdit" class="btn btn-xs btn-primary"
                                            onserverclick="EditEvent" data-rel="tooltip" title="<%$Resources:Tokens,Edit %>"><i class="icon-edit bigger-120"></i></button>&nbsp;
                                        <asp:LinkButton runat="server" ValidationGroup='<%#Eval("Id") %>' ID="BDelete" class="btn btn-xs btn-danger"
                                         OnClick="DeleteEvent" ClientIDMode="Static" OnClientClick="return areyousure()" data-rel="tooltip" ToolTip="<%$Resources:Tokens,Delete %>"><i class="icon-trash bigger-120"></i></asp:LinkButton>
                                   <%-- <button  clientidmode="Static" runat="server"  ValidationGroup='<%#Eval("Id") %>' id="Bdelete" class="btn btn-xs btn-danger"
                                            onserverclick="DeleteEvent" onclick="return areyousure()" data-rel="tooltip" title="<%$Resources:Tokens,Delete %>"><i class="icon-trash bigger-120"></i></button>--%>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </fieldset>
    </div>
    <div class="view" data-select="1">
        <div>
            <fieldset>
                <%--<div class="page-header">
                   <h1> <%= Tokens.Edit %></h1></div>--%>
                <div class="row-fluid">
                    <div class="span6">
                    <div class="well">
                        <div>
                            <label for="TbName">
                                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,Name %>" runat="server" />
                            </label>
                            <div>
                                <asp:TextBox runat="server" ID="TbName"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RTbName" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                            ControlToValidate="TbName" runat="server" />
                            </div>
                        </div>
                        <div>

                            <label for="Editor1">
                                <asp:Literal ID="Literal2" Text="<%$Resources:Tokens,Details %>" runat="server" />
                            </label>
                            <div>
                                
                                   
                                <cc1:Editor  ID="Editor2" runat="server" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="Editor2"
                                                            Display="Dynamic" ErrorMessage="<%$Resources:Tokens,Required%>"></asp:RequiredFieldValidator>
                                                   
                                 <asp:Label ID="Label4" runat="server" ForeColor="red" Text="*" Visible="false"></asp:Label>
                            </div>
                        </div>
                        <div>
                            <label for="TbUpload">
                                <asp:Literal ID="Literal3" Text="<%$Resources:Tokens,Upload %>" runat="server" />
                            </label>
                            <div>
                                <asp:FileUpload runat="server" ID="FuImag"></asp:FileUpload>
                                 <asp:RegularExpressionValidator runat="server" ForeColor="red" ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|png|PNG|)$" ControlToValidate="FuImag"  ErrorMessage="<%$Resources:Tokens,uploadimgerror %>"></asp:RegularExpressionValidator>
                    
                            </div>
                        </div>
                    </div>
                   </div>
                   <div class="span6">
                        <div>
                            <asp:Image runat="server" ID="ImgOffer" style="max-width: 470px;"/>
                        </div>
                    </div>
                </div>
                <div>
                    <button class="btn btn-primary" runat="server" ID="BSave" ><i class="icon-white icon-search"></i>&nbsp;<%= Tokens.Save %></button> &nbsp; <button class="btn btn-danger" CausesValidation="False" runat="server" ID="bCancel"><i class="icon-reply icon-only">&nbsp;</i><%= Tokens.Cancel %></button>
                </div>
            </fieldset>
        </div>
    </div>
          <script type="text/javascript" src="/ckeditor/ckeditor.js"></script>
    <script type="text/javascript" src="/ckeditor/adapters/jquery.js"></script>

    <input type="hidden" id="flag" runat="server" ClientIDMode="Static"/>
    <input type="hidden" id="selected" runat="server" ClientIDMode="Static"/>
    <script type="text/javascript">
        $(document).ready(function () {
            //$("#Editor1").ckeditor();
        });
        var $preview = $('div[data-select="0"]');
        var $edit = $('div[data-select="1"]');
        var flag = $('#flag').val();
        if (flag === "0") {
            $preview.show();
            $edit.hide();
        } else {
            $preview.hide();
            $edit.show();
        }
        //        $('#BDelete').click(function() {
        //            <%-- confirm('<%= Tokens.AlertRUS %>');--%>
        //        });

        function areyousure() {
            return confirm('<%= Tokens.AlertRUS %>');
        }
    </script>
</asp:Content>


