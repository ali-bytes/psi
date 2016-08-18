<%@ Page Title="<%$Resources:Tokens,RechargeBranch %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="RechargeBranch.aspx.cs" Inherits="NewIspNL.Pages.RechargeBranch" %>



<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.7.123, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="view">
        <fieldset>
            <div class="page-header"><h1>
                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,RechargeBranch %>" runat="server" /></h1></div>

                    <div class="row">
        <div class="col-sm-12">
            <div class="form-horizontal">
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal11" runat="server" Text="<%$Resources:Tokens,Branch %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:DropDownList runat="server" ID="DdlBranch" ClientIDMode="Static" AutoPostBack="True"
                                          OnSelectedIndexChanged="DdlBranche_SelectedIndexChanged" Width="159px"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DdlBranch"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                </div>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal12" runat="server" Text="<%$Resources:Tokens,Company %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    
                    <div>
                        <asp:DropDownList runat="server" ID="DdlVoiceCompany" OnSelectedIndexChanged="DdlVoiceCompany_SelectedIndexChanged"
                         ClientIDMode="Static" AutoPostBack="True" Width="159px"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="DdlVoiceCompany"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                </div>
               
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal13" runat="server" Text="<%$Resources:Tokens,Customer.Name %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:TextBox runat="server" ID="TbClientName" ClientIDMode="Static"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TbClientName"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                </div>
                
                
            
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal14" runat="server" Text="<%$Resources:Tokens,Customer.Phone %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:TextBox runat="server" ID="TbClientPhone" ClientIDMode="Static"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="TbClientPhone_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="TbClientPhone">
                        </asp:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TbClientPhone"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                </div>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal15" runat="server" Text="<%$Resources:Tokens,Amount %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:TextBox runat="server" ID="TbAmount" ClientIDMode="Static" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TbAmount"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="TbAmount"
                                            ErrorMessage="<%$Resources:Tokens,NumbersOnly %>" MaximumValue="99999999999"
                                            MinimumValue=".1" Type="Double"></asp:RangeValidator>
                    </div>
                </div>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal16" runat="server" Text="<%$Resources:Tokens,ServiceFees %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:TextBox runat="server" ID="txtServiceFees" ClientIDMode="Static" Enabled="False"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtServiceFees"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>

                    </div>
                </div>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal17" runat="server" Text="<%$Resources:Tokens,Total %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:TextBox runat="server" ID="txtTotal" ClientIDMode="Static" Enabled="False"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtTotal"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtTotal"
                                            ErrorMessage="<%$Resources:Tokens,NumbersOnly %>" MaximumValue="99999999999"
                                            MinimumValue=".1" Type="Double"></asp:RangeValidator>
                </div>
                </div>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><label for="TbNotes">
                        <asp:Literal runat="server" ID="Literal8" Text="<%$Resources:Tokens,Notes %>"></asp:Literal>
                    </label><%--<asp:Literal ID="Literal18" runat="server" Text="<%$Resources:Tokens,Customer.Name %>"></asp:Literal>--%></label>
                <div class="col-sm-9">
                <div>
                    
                    <div>
                        <asp:TextBox runat="server" ID="TbNotes" ClientIDMode="Static" TextMode="MultiLine"/>
                    </div>
                </div>
                </div></div>

 <div class="clearfix form-actions">
										<div class="col-md-offset-3 col-md-9">
										<%--	<button class="btn btn-info" type="button" runat="server" OnServerClick="BtnSave_Click">
												<i class="icon-ok bigger-110"></i>
												<asp:Literal runat="server" Text="<%$Resources:Tokens,Save %>"></asp:Literal>
											</button>--%>
                                            
                                            
                                            
                                                        <asp:Button ID="btn_Payment"  CssClass="btn btn-info" runat="server" Text="<%$Resources:Tokens,Save %>" Width="97px" 
                                     onclick="BtnSave_Click"    UseSubmitBehavior="false" OnClientClick="plswait(this.id) " />
                
                                            
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
                <asp:Literal ID="Literal4" Text="<%$Resources:Tokens,CreditHistory %>" runat="server" /></h3>
            <div>
                <asp:GridView runat="server" ID="GvHistory" CssClass="table table-bordered table-condensed text-center"
                    AutoGenerateColumns="False" OnDataBound="GvHistory_DataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="no" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Id" HeaderText="<%$Resources:Tokens,InvoiceNumber %>"/>
                        <asp:BoundField DataField="Type" HeaderText="<%$Resources:Tokens,Type %>" />
                        <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Amount %>" />
                        <asp:BoundField DataField="Branche" HeaderText="<%$Resources:Tokens,Branch %>" />
                        <asp:BoundField DataField="CompanyName" HeaderText="<%$Resources:Tokens,Company%>"/>
                        <asp:BoundField DataField="ServiceFees" HeaderText="<%$Resources:Tokens,ServiceFees %>"/>
                        <asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,Customer.Name %>" />
                        <asp:BoundField DataField="ClientTelephone" HeaderText="<%$Resources:Tokens,Customer.Phone %>"/>
                        <asp:BoundField DataField="Date" HeaderText="<%$Resources:Tokens,Date %>" />
                        <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes %>" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <a  href='<%#Eval("RecieptUrl") %>' target="_blank" data-rel="tooltip" title="<%= Tokens.Reciept %>">
                                   <i class="icon-file-text icon-only bigger-130"></i> </a>
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

            var type = document.getElementById('<%=DdlBranch.ClientID%>').value;
            var amo = document.getElementById('<%=DdlVoiceCompany.ClientID%>').value;
            var com = document.getElementById('<%=TbClientName.ClientID%>').value;

            var type2 = document.getElementById('<%=TbClientPhone.ClientID%>').value;
            var amo2 = document.getElementById('<%=TbAmount.ClientID%>').value;
            var com2 = document.getElementById('<%=txtServiceFees.ClientID%>').value;
            var com3 = document.getElementById('<%=txtTotal.ClientID%>').value;

            if (type == "" || amo == "" || com == "" || type2 == "" || amo2 == "" || com2 == "" || com3 == "") { return; }
            else {
                var check2 = document.getElementById(id);
                check2.disabled = 'true'; check2.value = 'Please wait...';
            }

        }








        window.addEventListener('load', doit);
        amount = document.getElementById('TbAmount');
        lbl = document.getElementById('txtTotal');
        function doit() {
            amount.onblur = checkamount;
        }
        function checkamount() {

            var fees = document.getElementById('txtServiceFees');
            lbl.value = parseFloat(fees.value) + parseFloat(amount.value);
        }
    </script>
</asp:Content>


