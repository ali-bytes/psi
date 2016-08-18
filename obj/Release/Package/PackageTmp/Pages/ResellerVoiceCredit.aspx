<%@ Page Title="<%$Resources:Tokens,ResellerVoiceCredit %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ResellerVoiceCredit.aspx.cs" Inherits="NewIspNL.Pages.ResellerVoiceCredit" %>


<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
<link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>

        <div class="view">
        <fieldset>
            <div class="page-header"><h1>
                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,ResellerVoiceCredit %>" runat="server" /></h1></div>
                
                    <div class="row">
        <div class="col-sm-12">
            <div class="form-horizontal">
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal7" runat="server" Text="<%$Resources:Tokens,Reseller %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:DropDownList runat="server" ID="DdlReseller" CssClass="chosen-select" ClientIDMode="Static" AutoPostBack="True"
                            OnSelectedIndexChanged="DdlReseller_SelectedIndexChanged" Width="178px"/>
                        <asp:RequiredFieldValidator ValidationGroup="s"  ID="RequiredFieldValidator1" runat="server" ControlToValidate="DdlReseller"
                            ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                </div>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="lblsave" Visible="False" ClientIDMode="Static" runat="server" Text="<%$Resources:Tokens,sav %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:DropDownList runat="server" ID="ddlSaves" Visible="False" ClientIDMode="Static" DataTextField="SaveName" 
                        DataValueField="Id" Width="178px"/>
                        <asp:RequiredFieldValidator ValidationGroup="s"  ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlSaves"
                            ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </div>
                </div>

                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal  ID="Literal3" Visible="False" runat="server" Text="<%$Resources:Tokens,Amount %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:TextBox runat="server" Visible="False" ID="TbAmount" ClientIDMode="Static" />
                        <asp:RequiredFieldValidator ValidationGroup="s"  ID="RequiredFieldValidator4" runat="server" ControlToValidate="TbAmount"
                            ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="TbAmount"
                            ErrorMessage="<%$Resources:Tokens,NumbersOnly %>" MaximumValue="99999999999"
                            MinimumValue=".1" Type="Double"></asp:RangeValidator>
                    </div>
                </div>
                
                
            
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal4" Visible="False" runat="server" Text="<%$Resources:Tokens,Notes %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:TextBox runat="server" Visible="False" ID="TbNotes" ClientIDMode="Static" TextMode="MultiLine" />
                    </div>
                </div>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal5" Visible="False" runat="server" Text="<%$Resources:Tokens,Effect %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:RadioButtonList runat="server" Visible="False" ID="RblEffect"  ClientIDMode="Static" RepeatDirection="Horizontal">
                           </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ValidationGroup="s" ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>" ControlToValidate="RblEffect"></asp:RequiredFieldValidator>
                    </div>
                </div>
                </div></div>

                        <div class="clearfix form-actions">
										<div class="col-md-offset-3 col-md-9">
											<%--<button class="btn btn-info" type="button" runat="server" Visible="False" ID="BSave" OnServerClick="BSave_Click">
												<i class="icon-ok bigger-110"></i>
												<asp:Literal runat="server" Text="<%$Resources:Tokens,Save %>"></asp:Literal>
											</button>--%>
                                            
                                                 <asp:Button ID="btn_Payment" Visible="False" CssClass="btn btn-info" runat="server" Text="<%$Resources:Tokens,Save %>" Width="97px" 
                                     onclick="BSave_Click" ValidationGroup="s"   UseSubmitBehavior="false" OnClientClick="plswait(this.id) " />
                
                                            
                                            


											&nbsp; &nbsp; &nbsp;
											<button class="btn" type="reset">
												<i class="icon-undo bigger-110"></i>
												<%=Tokens.reset %>
											</button> <br/>               <div id="messagel">
                    <asp:Literal runat="server" ID="Message"></asp:Literal>
                </div>
										</div>
									</div>
            </div></div></div>
                
              
        </fieldset>
    </div>
    <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <asp:Literal ID="Literal6" Text="<%$Resources:Tokens,CreditHistory %>" runat="server" /></h3>
            <div>
                <asp:GridView runat="server" ID="GvHistory" CssClass="table table-bordered table-condensed text-center"
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
                        <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,Reseller %>" />
                        <asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,User %>" />
                        <asp:BoundField DataField="Date" HeaderText="<%$Resources:Tokens,Date %>" />
                        <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes %>" />
                                                <asp:TemplateField HeaderText="<%$Resources:Tokens,Attachments %>">
                            <ItemTemplate>
                                <a id="A1"  runat="server" title="<%$Resources:Tokens,Attachments %>" href='<%#Eval("link")%>' target="_blank" data-rel="tooltip" Visible='<%#Eval("ifAttchment")%>'> 
                                    <i class="icon-paper-clip icon-only bigger-130"></i>
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <a class="icon-file-text icon-only bigger-130" href='<%#Eval("RecieptUrl") %>' title="<%=Tokens.Reciept %>" data-rel="tooltip" target="_blank">
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
        <script type="text/javascript">
            
            function plswait(id) {

                var type = document.getElementById('<%=DdlReseller.ClientID%>').value;
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



