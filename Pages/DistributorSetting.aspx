<%@ Page Title="<%$Resources:Tokens,DistributorSetting %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="DistributorSetting.aspx.cs" Inherits="NewIspNL.Pages.DistributorSetting" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row">
        <div class="page-header">
            <h1>
                <%= Tokens.DistributorSetting %></h1>
        </div>
        <div runat="server" id="Msg"></div>
        <div class="form-horizontal">
            <div class="control-group">
                <div class="well">
                    <div class="checkbox">
                        <label>
                            <input type="checkbox" class="ace" runat="server" id="CbclientActivationSubtract" /><span class="lbl">&nbsp;<%= Tokens.SubtractInvoiceReseller %></span></label>
                    </div>
                    <div class="checkbox">
                        <label>
                            <input type="checkbox" class="ace" runat="server" id="CbSubtractResellerCommission" /><span class="lbl">&nbsp;<%= Tokens.SubtractResellerCommission %></span></label>
                    </div>
                    <hr />
                    <div class="col-md-4">
                        <label for="txtCollectionCommission">
                            <%= Tokens.CollectionCommission %>
                        </label>
                        <asp:TextBox runat="server" ID="txtCollectionCommission" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="col-md-4">
                        <label for="ddlbox">
                            <%= Tokens.DistributorBox %>
                        </label>
                        <asp:DropDownList runat="server" ID="ddlbox" ClientIDMode="Static" Width="178px">
                        </asp:DropDownList>
                    </div>
                     <br />
                    <hr />
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            
                        <input type="checkbox" class="ace" runat="server" id="CbProvidersForDistributor" clientidmode="Static" /><span class="lbl">&nbsp;<%= Tokens.DistributorProviders%></span>
                   
                        </div>
                        <div class="panel-body" style="height: 50px">
                           <div id="ProvidersForDistributorDiv" runat="server" clientidmode="Static" class="collapse">

                        <asp:CheckBoxList ClientIDMode="Static" runat="server" Width="100%" ID="CbProvidersForDistributorList"
                            RepeatDirection="Horizontal">
                        </asp:CheckBoxList>
                    </div>
                        </div>
                    </div>
                  <%--  <span class="col-md-12">
                        <input type="checkbox" class="ace" runat="server" id="CbProvidersForDistributor" clientidmode="Static" /><span class="lbl">&nbsp;<%= Tokens.DistributorProviders%></span>
                    </span>
                    <br />
                    <div id="ProvidersForDistributorDiv" runat="server" clientidmode="Static" class="collapse" style="min-height: 200px !important">

                        <asp:CheckBoxList ClientIDMode="Static" Width="100%" runat="server" ID="CbProvidersForDistributorList"
                            RepeatDirection="Horizontal">
                        </asp:CheckBoxList>
                    </div>--%>
                    <br />
                    <hr />
                    <button runat="server" id="BSave" class="btn btn-primary" onserverclick="Save">
                        <i class="icon-save icon-only"></i>&nbsp;<%= Tokens.Save %></button>
                </div>
            </div>
        </div>

    </div>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script src="../Content/ace-assest/js/jquery-ui-1.10.3.full.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#CbProvidersForDistributor').change(function () {
                if (this.checked) {
                    $('#ProvidersForDistributorDiv').fadeIn('slow');

                } else {
                    $('#ProvidersForDistributorDiv').fadeOut('slow');
                    $("INPUT[id^='CbProvidersForDistributorList_']").removeAttr('checked');
                }

            });
        });
    </script>
</asp:Content>
