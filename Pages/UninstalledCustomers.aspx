<%@ Page Title="<%$Resources:Tokens,UnInstalledCustomers%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="UninstalledCustomers.aspx.cs" Inherits="NewIspNL.Pages.UninstalledCustomers" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.7.123, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
<link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>

    
     <div runat="server" id="errorMsg" Visible="False"  class="alert alert-danger">
    </div>
    <div runat="server" id="successMsg" Visible="False" class="alert alert-success"></div>
   <%-- <div class="view" data-search="0">--%>
        <fieldset>
            <div class="page-header"><h1>
                <%= Tokens.Search %></h1></div>
            <div class="well">
                <div>
                    <label for="DdlReseller">
                        <%= Tokens.Reseller %></label>
                    <div>
                        <asp:DropDownList runat="server" CssClass="chosen-select" ID="DdlReseller" Width="178px">
                        </asp:DropDownList>
                      
                    </div>
                </div>
                <div>
                    <label for="DdlBranch">
                        <%= Tokens.Branch %></label>
                        <div>
                    <asp:DropDownList runat="server" CssClass="chosen-select" ID="DdlBranch" Width="178px">
                    </asp:DropDownList></div>
                 
                </div>
                <div>
                    <label for="TbActivation">
                        <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,Activation.Date %>" runat="server" />
                    </label>
                    <div>
                        <asp:TextBox runat="server" ID="TbActivation" data-select="dp" Width="178px"></asp:TextBox>
                       
                    </div>
                </div>
            </div>
            <p>
                <button runat="server" id="BSearch" class="btn btn-success">
                   <i class="icon-search"></i> <%= Tokens.Search %></button>
            </p>
        </fieldset>
  <%--  </div>--%>
    <input type="hidden" runat="server" id="IsInSearch" />
    <div class="view" data-search="1">
        <fieldset>
            <div class="page-header"><h1>
                <%= Tokens.UnInstalledCustomers%></h1></div>
            <div>
                <p>
                      <%-- <button  class="btn btn-primary" id="BResearch" runat="server">
                       <i class="icon-search"></i> <%= Tokens.Search %></button>
          
                    
                    <asp:Button ID="Button2" OnClick="SearchOrders" class="btn btn-primary"  runat="server" Text="<%$Resources:Tokens,Search%>" />--%>

                       </p>
                <asp:GridView runat="server" CssClass="table table-bordered table-condensed text-center"
                    ID="GvReport" AutoGenerateColumns="False" OnDataBound="GvReport_DataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="lNo" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustomerName" HeaderText="<%$Resources:Tokens,Customer%>" />
                        <asp:BoundField DataField="CustomerPhone" HeaderText="<%$Resources:Tokens,Phone%>" />
                        <asp:BoundField DataField="GovernorateName" HeaderText="<%$Resources:Tokens,Governorate%>" />
                        <asp:BoundField DataField="StatusName" HeaderText="<%$Resources:Tokens,Status%>" />
                        <asp:BoundField DataField="ServicePackageName" HeaderText="<%$Resources:Tokens,Package%>" />
                        <asp:BoundField DataField="SPName" HeaderText="<%$Resources:Tokens,Service.Provider %>"/>
                        <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Tokens,Branch%>" />
                        <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,Reseller%>" />
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,Installed%>">
                            <ItemTemplate>
                                <% if (CanProcess)
                                   { %>
                                <asp:CheckBox ID="CbInstall" runat="server" />
                                <asp:HiddenField ID="HfId" runat="server" Value='<%#Eval("ID") %>' />
                                <%   } %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <button  type="button" data-woid="<%#Eval("ID") %>" data-type="select" id="installonce" clientidmode="Static" data-rel="tooltip"
                                 class="btn btn-success btn-xs" title="<%=Tokens.Install %>" ><i class="icon-wrench icon-only bigger-125"></i></button>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <% if (CanProcess)
               { %>
            <div>
                <button type="button" clientidmode="Static" runat="server" class="btn btn-success"
                    id="BInstall"><%=Tokens.InstallSelected%></button><%--OnClick="BInstall_OnClick"--%>
            </div>
            <%   } %>
        </fieldset>
    </div>
   
                   <div id="NoteModal" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
                                      <div class="modal-dialog">
            <div class="modal-content">
        <div class="modal-header">
            <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                ×</button>
            <h4 id="myModalLabel">
                <%= Tokens.InstallSelected %></h4>
        </div>
        <div class="modal-body">
            <div class="bootbox-body">
           
                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Tokens,Notes %>"></asp:Label>
                <div>
                    <asp:TextBox runat="server" ID="txtNote" TextMode="MultiLine"></asp:TextBox>
                </div>
            
            </div>
        </div>
        <div class="modal-footer">
            <asp:HiddenField runat="server" ID="hfOnce" ClientIDMode="Static"/>
            <asp:Button runat="server" CssClass="btn btn-primary" ID="btnAdd" ClientIDMode="Static" Text="<%$Resources:Tokens,Save %>" OnClick="BInstall_OnClick"/>
            <asp:Button runat="server" CssClass="btn btn-primary" ID="btnAddOnce" ClientIDMode="Static" Text="<%$Resources:Tokens,Save %>" OnClick="Installorder"/>
            &nbsp;<button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="True"><%=Tokens.Cancel %></button>
        </div>
        </div></div>
        </div>
                        <asp:ModalPopupExtender ID="mpe_PrePaid" runat="server" BackgroundCssClass="modalBackground"
                    PopupControlID="NotPrepaidModal" TargetControlID="lblPrepaid" Drag="True" DynamicServicePath=""
                    Enabled="True" CancelControlID="btnCancel">
                </asp:ModalPopupExtender>

                                       <asp:Panel  runat="server" ID="NotPrepaidModal" class="modalPopup">
                                                          <div class="modal-dialog">
            <div class="modal-content">
                            <div class="modal-header">
                                <asp:Button runat="server" ID="Button1" Text="X" OnClick="Btncancel" data-dismiss="modal" aria-hidden="true" CssClass="close"/>
            <h3 id="H1">
                <asp:Label runat="server"  ID="lblModalTitle"></asp:Label>
                </h3>
        </div>
        <div class="modal-body">
                    <div class="well">
                        <div>
                        <asp:Label runat="server" ID="lblPrepaid" Text="<%$Resources:Tokens,Prepaid %>"></asp:Label>
                        <asp:TextBox runat="server" ID="txtPrePaid"></asp:TextBox></div>
                        <br/>
                        <div>
                        <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,sav %>"></asp:Literal>
                        <asp:DropDownList runat="server" ID="ddlSaves" DataTextField="SaveName"
                         DataValueField="Id" Width="140px"></asp:DropDownList></div>
                    </div>
                    
                    </div>
                    <div class="modal-footer">
                    <asp:Button runat="server" ID="btnprePaid" OnClick="BtnManagePrepaid" Text="<%$Resources:Tokens,Save %>" CssClass="btn btn-primary"/>
                    &nbsp;<asp:Button runat="server" ID="btnCancel" OnClick="Btncancel" Text="<%$Resources:Tokens,Cancel %>" CssClass="btn btn-danger"/>
                    </div>
                </div>
                </div>
                </asp:Panel>
              
    <script type="text/javascript">
        
        $(document).ready(function () {
            $(".chosen-select").chosen();
        });

        $(function () {
            $('input[data-select="dp"]').datepicker({ dateFormat: 'dd/mm/yy' });
            var $search = $('div[data-search="0"]');
            var $result = $('div[data-search="1"]');
            var $isInSearch = $('#IsInSearch').val() === "0";
            if ($isInSearch) {
                $search.show();
                $result.hide();
            } else {
                $search.hide();
                $result.show();
            }
            $('#BInstall').click(function () {
                $('#NoteModal').modal('show');
                $('#btnAdd').show();
                $('#btnAddOnce').hide();
            });
            $('button[data-type="select"]').click(function () {
                var woid = $(this).attr('data-woid');
                $('#hfOnce').val(woid);
                $('#NoteModal').modal('show');
                $('#btnAdd').hide();
                $('#btnAddOnce').show();
            });
        });
    </script>

</asp:Content>
