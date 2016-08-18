<%@ Page Title="<%$Resources:Tokens,BranchVoiceCredit %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="BranchVoiceCredit.aspx.cs" Inherits="NewIspNL.Pages.BranchVoiceCredit" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
<link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>

    <div class="row">
                <div class="page-header"><h1>
                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,BranchVoiceCredit %>" runat="server" /></h1></div>
    
        <div class="col-xs-12">
            <div class="form-horizontal"> 
                <div class="form-group">
               <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal2" Text="<%$Resources:Tokens,Branch %>" runat="server" /></label>
               <div class="col-sm-9">
                                        <asp:DropDownList runat="server" ID="DdlBranch" CssClass="chosen-select" ClientIDMode="Static" AutoPostBack="True"
                            OnSelectedIndexChanged="DdlBranch_SelectedIndexChanged" Width="178px"/>
                        <asp:RequiredFieldValidator ValidationGroup="s" ID="RequiredFieldValidator1" runat="server" ControlToValidate="DdlBranch"
                            ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                </div></div>
                <div class="space-4"></div>
                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal7" runat="server" Text="<%$Resources:Tokens,sav %>"></asp:Literal></label>
                <div class="col-sm-9">
                                            <asp:DropDownList runat="server" ID="ddlSaves" ClientIDMode="Static" DataTextField="SaveName" DataValueField="Id" Width="178px"/>
                        <asp:RequiredFieldValidator ValidationGroup="s" ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlSaves"
                            ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                </div></div>
                                <div class="space-4"></div>
                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal3" Text="<%$Resources:Tokens,Amount %>" runat="server" /></label>
                <div class="col-sm-9">
                                            <asp:TextBox runat="server" ID="TbAmount" ClientIDMode="Static" />
                        <asp:RequiredFieldValidator ValidationGroup="s" ID="RequiredFieldValidator4" runat="server" ControlToValidate="TbAmount"
                            ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="TbAmount"
                            ErrorMessage="<%$Resources:Tokens,NumbersOnly %>" MaximumValue="99999999999"
                            MinimumValue=".1" Type="Double"></asp:RangeValidator>
                </div></div>
                                <div class="space-4"></div>
                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal10" runat="server" Text="<%$Resources:Tokens,Notes %>"></asp:Literal></label>
                <div class="col-sm-9">
                    <asp:TextBox runat="server" ID="TbNotes" ClientIDMode="Static" TextMode="MultiLine" />
                </div></div>
                                <div class="space-4"></div>
                
                
                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal9" runat="server" Text="<%$Resources:Tokens,Effect %>"></asp:Literal></label>
                <div class="col-sm-9">
                                            <asp:RadioButtonList runat="server" ID="RblEffect" ClientIDMode="Static" RepeatDirection="Horizontal">
                           
                        </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ValidationGroup="s" ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>" ControlToValidate="RblEffect"></asp:RequiredFieldValidator>
                </div></div>

                <div class="clearfix form-actions">
										<div class="col-md-offset-3 col-md-9">
										<%--	<button id="Button1" class="btn btn-info" type="button" runat="server" OnServerClick="BSave_Click">
												<i class="icon-ok bigger-110"></i>
												<%=Tokens.Save %>
											</button>--%>
                                            
                                                
                                                 <asp:Button ID="btn_Payment"  CssClass="btn btn-info" runat="server" Text="<%$Resources:Tokens,Save %>" Width="97px" 
                                     onclick="BSave_Click" ValidationGroup="s"   UseSubmitBehavior="false" OnClientClick="plswait(this.id) " />
                
                                            
                                            
                                            
                                            

											&nbsp; &nbsp; &nbsp;
											<button class="btn" type="reset">
												<i class="icon-undo bigger-110"></i>
												<%=Tokens.reset %>
											</button>
                                                            <div>
                    <asp:Literal runat="server" ID="Message"></asp:Literal>
                </div>
										</div>
									</div>
            </div></div>

        <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <asp:Literal ID="Literal6" Text="<%$Resources:Tokens,CreditHistory %>" runat="server" /></h3>
            <div>
                <asp:GridView runat="server" ID="GvHistory" CssClass="table table-bordered table-condensed"
                    AutoGenerateColumns="False" OnDataBound="GvHistory_DataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="no" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Type" HeaderText="<%$Resources:Tokens,Type %>" />
                        <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Amount %>" />
                        <asp:BoundField DataField="Net" HeaderText="<%$Resources:Tokens,Net %>" />
                        <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Tokens,Branch %>" />
                        <asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,User %>" />
                        <asp:BoundField DataField="Date" HeaderText="<%$Resources:Tokens,Date %>" />
                        <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes %>" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <a class="icon-file-text icon-only bigger-130" href='<%#Eval("RecieptUrl") %>' target="_blank" title="<%=Tokens.Reciept %>" data-rel="tooltip">
                                    </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div>
                            <%= Tokens.NoResults %></div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </fieldset>
    </div>
    </div>
            <script type="text/javascript">
                

                function plswait(id) {

                    var type = document.getElementById('<%=DdlBranch.ClientID%>').value;
                     var amo = document.getElementById('<%=ddlSaves.ClientID%>').value;

                     var type2 = document.getElementById('<%=TbAmount.ClientID%>').value;

                     var op = document.getElementById('<%=RblEffect.ClientID%>');
                     var cocheck = op.getElementsByTagName('input');
                     var checkedValues = [];

                     for (var i = 0; i < cocheck.length; i++) {
                         var checkBoxRef = cocheck[i];

                         if (checkBoxRef.checked == true) {
                             checkedValues.push("added");
                         }
                     }

                     if (type == "" || amo == "" || type2 == "" || checkedValues.length == 0) { return; }
                     else {
                         var check2 = document.getElementById(id);
                         check2.disabled = 'true'; check2.value = 'Please wait...';
                     }

                 }






                $(document).ready(function () {
                    $(".chosen-select").chosen();
                    $('tr td input[type=radio]').addClass("ace");
                    $('tr td label').addClass("lbl").css({ 'padding': '7px' });
                })
            </script>
</asp:Content>

