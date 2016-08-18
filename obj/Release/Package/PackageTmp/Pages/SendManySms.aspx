<%@ Page Title="<%$Resources:Tokens,SendManySms %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="SendManySms.aspx.cs" Inherits="NewIspNL.Pages.SendManySms" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.7.123, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <div class="col-md-12">
                <div id="tabs">
											<ul>
												<li>
													<a href="#tabs-1"><asp:Literal runat="server" ID="lbltitle" Text="<%$Resources:Tokens,SendManySms %>"></asp:Literal></a>
												</li>

												<li>
													<a href="#tabs-2"><asp:Label runat="server" Text="<%$Resources:Tokens,SendSmsbyDay %>" ID="lblheader"></asp:Label></a>
												</li>
											</ul>

											<div id="tabs-1">
												                            <div class="row-fluid">
                <div class="col-sm-4">
                    <div class="well">
                        <div>
                            <label for="DdlReseller">
                                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,Reseller %>" runat="server" />
                            </label>
                            <div>
                                <asp:DropDownList ID="DdlReseller" runat="server" EnableViewState="True" />
                            </div>
                        </div>
                                                <div>
                            <label for="DdlBranchs">
                                <asp:Literal ID="Literal3" Text="<%$Resources:Tokens,Branch %>" runat="server" />
                            </label>
                            <div>
                                <asp:DropDownList ID="DdlBranchs" runat="server" EnableViewState="True" />
                            </div>
                        </div>


                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="well">

                                                <div>
                            <label>
                                <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:Tokens,Provider %>"></asp:Literal>
                            </label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlProvider" EnableViewState="True"/>
                            </div>
                        </div>
                                                                        <div>
                            <label>
                                <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,Status %>"></asp:Literal>
                            </label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlStatus" EnableViewState="True"/>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="col-sm-4">
                  <div class="well">
                      <div>
                            <label>
                                <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:Tokens,PaymentType %>"></asp:Literal>
                            </label>
                            <div>
                                 
                                <asp:DropDownList runat="server" ID="DdlPaymentType" EnableViewState="True"/>
                            </div>
                                </div>
                  </div>
                </div>
                                                                                </div>
                                                <div class="col-sm-6">
                            <p>
                                <button runat="server" id="btnSearch" class="btn btn-success" OnServerClick="SearchCustomers"><i class="icon-search icon-only"></i>&nbsp;
                                <asp:Literal runat="server" Text="<%$Resources:Tokens,Search  %>" ID="lblbtnSearch"></asp:Literal></button>
            </p>
                                                    </div>
            
            <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <asp:Literal ID="Literal4" Text="<%$Resources:Tokens,Results %>" runat="server" /></h3>
            <div runat="server" id="Msg" Visible="False" class="alert alert-success">
                <asp:Literal runat="server" Text="<%$Resources:Tokens,MsgSent %>"></asp:Literal>
            </div>

            <div>
                <asp:GridView CssClass="table table-bordered table-condensed text-center"
                    runat="server" ID="GvResults" AutoGenerateColumns="False" OnDataBound="NumberGrid" DataKeyNames="Id">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LNo" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Customer %>" DataField="Customer" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Phone %>" DataField="Phone" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Status %>" DataField="Status" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Provider %>" DataField="Provider" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Reseller %>" DataField="Reseller" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Branch %>" DataField="Branch"/>
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Central %>" DataField="Central" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Governorate %>" DataField="Governorate" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,PaymentType %>" DataField="PaymentMethod" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Offer %>" DataField="Offer" />
                        
                       
                    </Columns>
                </asp:GridView>
            </div>
            <p id="Pb1">
                											<button id="btnSend" class="btn btn-info" type="button">
												<i class="icon-ok bigger-110"></i>
                                                &nbsp;<asp:Literal runat="server" ID="lbllterl" Text="<%$Resources:Tokens,Send %>"></asp:Literal>
											</button>
            </p>
        </fieldset>
    </div>
											</div>

											<div id="tabs-2">
											    
                                                    <div Visible="False" runat="server" class="alert alert-success" id="Msgsuc">
        <%=Tokens.MsgSent %>
    </div>
    <div runat="server" id="errorMsg" Visible="False" class="alert alert-danger">
        <%=Tokens.NoResults %>
    </div>
												<div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal"> 
                <div class="form-group">
               <label class="col-sm-3 control-label no-padding-right"> <asp:Literal ID="lblday" runat="server" Text="<%$Resources:Tokens,Days %>"></asp:Literal></label>
               <div class="col-sm-9">
                
                    <asp:TextBox runat="server" ID="txtDays" CssClass="required-input"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="txt_CustomerMobile_FilteredTextBoxExtender" runat="server"
                                                 FilterType="Numbers" TargetControlID="txtDays" Enabled="True">
                    </asp:FilteredTextBoxExtender>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtDays" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="smsday"></asp:RequiredFieldValidator>
                </div></div>
                <div class="space-4"></div>
                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal8" runat="server" Text="<%$Resources:Tokens,Message %>"></asp:Literal></label>
                <div class="col-sm-9">                  
                    <asp:TextBox runat="server" ID="txtMeaageText" ClientIDMode="Static"  TextMode="MultiLine" CssClass="required-input col-xs-10 col-sm-5" Width="200px"></asp:TextBox>
  <span class="help-inline col-xs-12 col-sm-3">
												<span class="middle"><asp:Literal ID="Literal9" runat="server" Text="<%$Resources:Tokens,littersLimit %>"></asp:Literal></span>
											</span>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtMeaageText" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="smsday"></asp:RequiredFieldValidator>
                </div></div>
                <div class="clearfix form-actions">
										<div class="col-md-offset-3 col-md-9">
											<button id="Button1" class="btn btn-info" type="button" runat="server" OnServerClick="btnSendSmsDay_Click" ValidationGroup="smsday">
												<i class="icon-ok bigger-110"></i>
												<%=Tokens.Send %>
											</button>

											&nbsp; &nbsp; &nbsp;
											<button class="btn" type="reset">
												<i class="icon-undo bigger-110"></i>
												<%=Tokens.reset %>
											</button>
										</div>
									</div>
                                   
  </div></div>
            <div id="concon">
            </div>
        </div>
											</div>

										</div></div>

    <div id="SmsModal" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
                        <div class="modal-dialog">
            <div class="modal-content">
        <div class="modal-header">
            <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                ×</button>
            <h4 id="myModalLabel">
                <%=Tokens.SendsmsMessage%></h4>
        </div>
        <div class="modal-body">
            <div class="bootbox-body">
            <div>
                <asp:Literal runat="server" Text="<%$Resources:Tokens,Message %>" ID="lblMessabe"></asp:Literal>
                <div>                  
                    <asp:TextBox runat="server" ID="txtMessageText" ClientIDMode="Static"  TextMode="MultiLine" CssClass="required-input col-xs-10 col-sm-5" Width="200px"></asp:TextBox>
  <span class="help-inline col-xs-12 col-sm-3">
												<span class="middle"><asp:Literal ID="Literal6" runat="server" Text="<%$Resources:Tokens,littersLimit %>"></asp:Literal></span>
											</span>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMessageText" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="sms"></asp:RequiredFieldValidator>
                </div>
            </div></div>
        </div>
        <div class="modal-footer">
                											<button id="Button2" class="btn btn-info" type="button" runat="server" OnServerClick="btnSendSms_Click" ValidationGroup="sms">
												<i class="icon-ok bigger-110"></i>
                                                &nbsp;<asp:Literal runat="server" ID="Literal7" Text="<%$Resources:Tokens,Send %>"></asp:Literal>
											</button>
            <button class="btn btn-default" data-dismiss="modal" aria-hidden="true">
                <i class="icon-white icon-arrow-left"></i>
                <%=Tokens.Cancel %></button>
        </div>
        </div>
        </div>

    </div>
    
 
	
    <script type="text/javascript">
        $(document).ready(function () {
            $("#tabs").tabs();
           
            $('#btnSend').click(function () {
                var totalRows = $("#<%=GvResults.ClientID %> tr").length;
                if (totalRows > 0) {
                    $('#SmsModal').modal('show');
                    return false;
                }
                alert('<%=Tokens.NoResults %>');
                return true;
            });
            var val = $('#hfRtl').val();
            if (val == "1") $("#txtMeaageText").attr('maxlength', '70');
            else {
                $("#txtMeaageText").attr('maxlength', '160');
            }

        });
    </script>
</asp:Content>


