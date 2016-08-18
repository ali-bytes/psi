<%@ Page Title="<%$Resources:Tokens,RechargeAccountReseller %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="RechargeAccountReseller.aspx.cs" Inherits="NewIspNL.Pages.RechargeAccountReseller" %>





<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <fieldset>
            <div class="page-header"><h1>
                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,RechargeAccountReseller %>" runat="server" /></h1></div>

            
            
            
            
                                <div class="row">
        <div class="col-sm-12">
            <div class="form-horizontal">
                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal4" runat="server" Text="<%$Resources:Tokens,Reseller %>"></asp:Literal></label>
<div class="col-sm-9"> 
                                    <div >
                        <asp:DropDownList runat="server" ID="DdlReseller" ClientIDMode="Static" CssClass="col-xs-10 col-sm-3" Width="178px"/>

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DdlReseller"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="save"></asp:RequiredFieldValidator> <span class="help-inline col-xs-12 col-sm-7">
												<span class="middle"><asp:Label runat="server" ID="lblTextOfRechargeAccount"></asp:Label></span>
											</span>                                     
                    </div>
                     
                </div></div>
                <div class="space-4"></div>
                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal5" runat="server" Text="<%$Resources:Tokens,Box %>"></asp:Literal></label>
                <div class="col-sm-9">
                                    <div>
                        <asp:DropDownList runat="server" ID="DdlBoxes" ClientIDMode="Static" Width="178px"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DdlBoxes"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="save"></asp:RequiredFieldValidator>
                    </div>
                </div></div>
                <div class="space-4"></div>
                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal9" runat="server" Text="<%$Resources:Tokens,Depositor %>"></asp:Literal></label>
                <div class="col-sm-9">
                                    <div>
                        <asp:TextBox runat="server" ID="TbDepositor" ClientIDMode="Static"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TbDepositor"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="save"></asp:RequiredFieldValidator>
                    </div>
                </div></div>
                <div class="space-4"></div>
                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal11" runat="server" Text="<%$Resources:Tokens,Amount %>"></asp:Literal></label>
                <div class="col-sm-9">
                                                        <div>
                        <asp:TextBox runat="server" ID="TbAmount" ClientIDMode="Static" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TbAmount"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="save"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="TbAmount"
                                            ErrorMessage="<%$Resources:Tokens,NumbersOnly %>" MaximumValue="99999999999"
                                            MinimumValue=".1" Type="Double"></asp:RangeValidator>
                    </div>
                </div></div>
                <div class="space-4"></div>
                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal12" runat="server" Text="<%$Resources:Tokens,DirectingBalanceTo %>"></asp:Literal></label>
                <div class="col-sm-9">
                                    <div>
                        <asp:RadioButtonList runat="server" CellSpacing="4" ID="RblEffect" ClientIDMode="Static" RepeatDirection="Vertical">
                            <asp:ListItem  Text="<%$Resources:Tokens,ResellerVoiceCredit %> &nbsp;&nbsp;"></asp:ListItem>
                            <asp:ListItem Selected="True" Text="<%$Resources:Tokens,ResellerPaymentCredit %>"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Tokens,AddToResellerBalanceSheet %>"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div></div>
                <div class="space-4"></div>
                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal13" runat="server" Text="<%$Resources:Tokens,DemandReciept %>"></asp:Literal></label>
                <div class="col-sm-9">
                                    <div>
                        <asp:FileUpload runat="server" ID="FUReciept" ClientIDMode="Static"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="FUReciept"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="save"></asp:RequiredFieldValidator>
                   <asp:RegularExpressionValidator runat="server" ValidationGroup="save" ForeColor="red" ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|png|PNG|)$" ControlToValidate="FUReciept"  ErrorMessage="<%$Resources:Tokens,uploadimgerror %>"></asp:RegularExpressionValidator>
                    
                                          </div>
                </div></div>
                
                <div class="clearfix form-actions">
										<div class="col-md-offset-3 col-md-9">
										<%--	<asp:LinkButton ID="Button1" CssClass="btn btn-info"  runat="server"  OnClientClick="this.disabled='true'; this.value='Please wait...';"  UseSubmitBehavior="true" OnClick="BtnSave_Click"  >
												
												<asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,Save %>"></asp:Literal>
											</asp:LinkButton>--%>
                                            
                                        <asp:Button ID="Button2"  runat="server" Text="<%$Resources:Tokens,Save %>" CssClass="btn btn-info icon-ok bigger-110" OnClick="BtnSave_Click" runat="server" UseSubmitBehavior="false"  OnClientClick="save()"  ValidationGroup="save"></asp:Button>

                                       
											&nbsp; &nbsp; &nbsp;
											<button class="btn" type="reset" >
												<i class="icon-undo bigger-110"></i>
												<%=Tokens.reset %>
											</button> <br/>
                <div id="messagel">
                    <asp:Literal runat="server" ID="Message"></asp:Literal>
                </div>
										</div>
									</div>

            </div>
            </div></div>
        </fieldset>
    </div>
        <style type="text/css">
        #RblEffect>tbody>tr>td>label {
            display: -webkit-inline-box;
            margin-right: 4px;
            margin-bottom: 15px;
        }
    </style>
    <script type="text/javascript">
        $('tr td input[type=radio]').addClass("ace");
        $('tr td label').addClass("lbl");        


        function save() {
            var amount = document.getElementById('<%=TbAmount.ClientID%>').value;
            var res = document.getElementById('<%=DdlReseller.ClientID%>').value;
            var box = document.getElementById('<%=DdlBoxes.ClientID%>').value;
            var recep = document.getElementById('<%=FUReciept.ClientID%>').value;
            var re = document.getElementById('<%=TbDepositor.ClientID%>').value;
            if (amount == "" || res == "" || box == "" || recep == "" || re == "") {
                return;
            } else {
               var check2 = document.getElementById('<%=Button2.ClientID%>');
               check2.disabled = 'true'; check2.value = 'Please wait...';
           }
        }
    </script>
</asp:Content>


