<%@ Page Title="<%$Resources:Tokens,BranchPaymentCredit%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="BranchPaymentCredit.aspx.cs" Inherits="NewIspNL.Pages.BranchPaymentCredit" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
    <div class="view">
        <fieldset>
            <div class="page-header">
                <h1><%= Tokens.BranchPaymentCredit%></h1></div>
                
                
                   <div class="row">
        <div class="col-sm-12">
            <div class="form-horizontal">
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,Branch %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:DropDownList runat="server" ID="DdlReseller" ClientIDMode="Static" AutoPostBack="True"
                            OnSelectedIndexChanged="DdlReseller_SelectedIndexChanged" Width="177px"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DdlReseller"
                            ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </div>
                </div>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal Visible="False" ID="lblsave" runat="server" Text="<%$Resources:Tokens,sav %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:DropDownList Visible="False" runat="server" ID="ddlSaves" ClientIDMode="Static" DataTextField="SaveName" DataValueField="Id" Width="177px"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlSaves"
                            ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </div>
                </div>
                
                
            
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal Visible="False" ID="lblAmount" runat="server" Text="<%$Resources:Tokens,Amount %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:TextBox Visible="False" runat="server" ID="TbAmount" ClientIDMode="Static" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TbAmount"
                            ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="TbAmount"
                            ErrorMessage="RangeValidator" MaximumValue="99999999999" MinimumValue=".1" Type="Double">greater than or equal 0</asp:RangeValidator>
                    </div>
                </div>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="lblNotes" Visible="False" runat="server" Text="<%$Resources:Tokens,Notes %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:TextBox runat="server" Visible="False" ID="TbNotes" ClientIDMode="Static" TextMode="MultiLine" />
                    </div>
                </div>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal Visible="False" ID="lblEffect" runat="server" Text="<%$Resources:Tokens,Operation %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div class="radios">
                        <asp:RadioButtonList Visible="False" runat="server" ID="RblEffect" 
                        ClientIDMode="Static" RepeatDirection="Horizontal">
                            <asp:ListItem Selected="True" Text="<%$Resources:Tokens,Add %>"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Tokens,Subtract %>"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                </div></div>


            </div></div></div> 
                                        <div class="clearfix form-actions">
										<div class="col-md-offset-3 col-md-9">
										<%--	<button class="btn btn-info" type="button" runat="server" Visible="False" ID="BSave" OnServerClick="BSave_Click">
												<i class="icon-ok bigger-110"></i>
												<%=Tokens.Save %>
											</button>--%>
                                            
                                             <asp:Button ID="btn_Payment" Visible="False" CssClass="btn btn-info" runat="server" Text="<%$Resources:Tokens,Save %>" Width="97px" 
                                     onclick="BSave_Click"  UseSubmitBehavior="false" OnClientClick="plswait(this.id) " />
                
                                            
                                            

											&nbsp; &nbsp; &nbsp;
											<button class="btn" type="reset">
												<i class="icon-undo bigger-110"></i>
												<%=Tokens.reset %>
											</button> <br/>           <div id="messagel">
                <asp:Literal runat="server" ID="Message"></asp:Literal>
            </div>
										</div>
									</div>
            <%--<p>
                <asp:LinkButton runat="server" CssClass="btn btn-primary" Visible="False" ID="BSave" OnClick="BSave_Click"><i class="icon-save"></i>&nbsp;<%=Tokens.Save %></asp:LinkButton></p>--%>

        </fieldset>
    </div>
    <div  class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <%= Tokens.CreditHistory %></h3>
                 <table class="alert alert-info">
                    <tr>
                        <td><%=Tokens.Credit %></td>
                        <td>:&nbsp;</td>
                        <td><asp:Label runat="server" ID="lblLastCredit"></asp:Label></td>
                    </tr>
                </table>
            <div>
           <%--     <asp:GridView runat="server" ID="GvHistory" CssClass="table table-bordered table-condensed"
                              AutoGenerateColumns="False" 
                              OnDataBound="GvHistory_DataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="no" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Type" HeaderText="<%$Resources:Tokens,Type %>" />
                        <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Amount %>" />
                        <asp:BoundField DataField="Net" HeaderText="<%$Resources:Tokens,Net %>" />
                        <asp:BoundField DataField="Branch" HeaderText="<%$Resources:Tokens,Branch %>" />
                        <asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,User %>" />
                        <asp:BoundField DataField="Date" HeaderText="<%$Resources:Tokens,Date %>" />
                        <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes %>" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <a data-rel="tooltip" title="<%=Tokens.Reciept %>" href='<%#Eval("RecieptUrl") %>' target="_blank">
                                    <i class="icon-file-text bigger-120 green"></i></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div>
                            <%= Tokens.NoResults %></div>
                    </EmptyDataTemplate>
                </asp:GridView>--%>
                               
            </div>
        </fieldset>
    </div>
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
                $('tr td input[type=radio]').addClass("ace");
                $('tr td label').addClass("lbl").css({ 'padding': '7px' });
            })
        </script>
</asp:Content>
