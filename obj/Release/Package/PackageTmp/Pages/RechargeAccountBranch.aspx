<%@ Page Title="<%$Resources:Tokens,RechargeAccountBranch %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="RechargeAccountBranch.aspx.cs" Inherits="NewIspNL.Pages.RechargeAccountBranch" %>


<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="view">
        <fieldset>
            <div class="page-header"><h1>
                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,RechargeAccountBranch %>" runat="server" /></h1></div>
                    <div class="row">
        <div class="col-sm-12">
            <div class="form-horizontal">
                
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal4" runat="server" Text="<%$Resources:Tokens,Branch %>"></asp:Literal></label>
                <div class="col-sm-9">
<div>
                    <div>
                        <asp:DropDownList runat="server" ID="DdlBranch" ClientIDMode="Static" Width="178px"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DdlBranch"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="save"></asp:RequiredFieldValidator>
                    </div>
                </div>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal5" runat="server" Text="<%$Resources:Tokens,Box %>"></asp:Literal></label>
                <div class="col-sm-9">
<div>
                    <div>
                        <asp:DropDownList runat="server" ID="DdlBoxes" ClientIDMode="Static" Width="178px"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DdlBoxes"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="save"></asp:RequiredFieldValidator>
                    </div>
                </div>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal9" runat="server" Text="<%$Resources:Tokens,Depositor %>"></asp:Literal></label>
                <div class="col-sm-9">
<div>
                    <div>
                        <asp:TextBox runat="server" ID="TbDepositor" ClientIDMode="Static"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TbDepositor"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="save"></asp:RequiredFieldValidator>
                    </div>
                </div>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal10" runat="server" Text="<%$Resources:Tokens,Amount %>"></asp:Literal></label>
                <div class="col-sm-9">
<div>
                    <div>
                        <asp:TextBox runat="server" ID="TbAmount" ClientIDMode="Static" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TbAmount"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="save"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="TbAmount"
                                            ErrorMessage="<%$Resources:Tokens,NumbersOnly %>" MaximumValue="99999999999"
                                            MinimumValue=".1" Type="Double"></asp:RangeValidator>
                    </div>
                </div>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal11" runat="server" Text="<%$Resources:Tokens,DirectingBalanceTo %>"></asp:Literal></label>
                <div class="col-sm-9">
<div><!--/*DirectingBRblEffectalanceTo %>*/-->
                    <div>
                        <asp:RadioButtonList runat="server" CellSpacing="4" ID="RblEffect" ClientIDMode="Static" RepeatDirection="Vertical">
                            <asp:ListItem  Text="<%$Resources:Tokens,BranchVoiceCredit %> &nbsp;&nbsp;"></asp:ListItem>
                            <asp:ListItem Selected="True" Text="<%$Resources:Tokens,BranchPaymentCredit %>"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Tokens,AddToBranchBalanceSheet %>"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal12" runat="server" Text="<%$Resources:Tokens,DemandReciept %>"></asp:Literal></label>
                <div class="col-sm-9">
<div>
                    <div>
                        <asp:FileUpload runat="server" ID="FUReciept" ClientIDMode="Static"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="FUReciept"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="save"></asp:RequiredFieldValidator>
                     <asp:RegularExpressionValidator runat="server" ValidationGroup="save" ForeColor="red" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif)$" ControlToValidate="FUReciept"  ErrorMessage="<%$Resources:Tokens,uploadimgerror %>"></asp:RegularExpressionValidator>
                  
                    </div>
                </div>
                </div></div>
                                        <div class="clearfix form-actions">
										<div class="col-md-offset-3 col-md-9">
											<%--<button class="btn btn-info" type="button" runat="server" OnServerClick="BtnSave_Click"  UseSubmitBehavior="false"  onclick="save()"   >
												<i class="icon-ok bigger-110"></i>
												<asp:Literal runat="server" Text="<%$Resources:Tokens,Save %>"></asp:Literal>
											</button>--%>
                                             <asp:Button ID="Button2"  runat="server" Text="<%$Resources:Tokens,Save %>" CssClass="btn btn-info icon-ok bigger-110" OnClick="BtnSave_Click" runat="server" UseSubmitBehavior="false"  OnClientClick="save()"  ValidationGroup="save"></asp:Button>

											&nbsp; &nbsp; &nbsp;
											<button class="btn" type="reset">
												<i class="icon-undo bigger-110"></i>
												<%=Tokens.reset %>
											</button> <br/>
                                                           <div id="message">
                    <asp:Literal runat="server" ID="Message"></asp:Literal>
                </div>
										</div>
									</div>




            </div></div></div>
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
        
        function save() {
            var amount = document.getElementById('<%=TbAmount.ClientID%>').value;
             var res = document.getElementById('<%=DdlBranch.ClientID%>').value;
             var box = document.getElementById('<%=DdlBoxes.ClientID%>').value;
             var recep = document.getElementById('<%=TbDepositor.ClientID%>').value;
            var re = document.getElementById('<%=FUReciept.ClientID%>').value;
            if (amount == "" || res == "" || box == "" || recep == "" || re == "") {
                 return;
             } else {
                 var check2 = document.getElementById('<%=Button2.ClientID%>');
                check2.disabled = 'true'; check2.value = 'Please wait...';
            }
        }

        $('tr td input[type=radio]').addClass("ace");
        $('tr td label').addClass("lbl");
    </script>
</asp:Content>



