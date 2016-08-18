<%@ Page Title="<%$Resources:Tokens,ResellerPaymentCredit %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ResellerPaymenyCredit.aspx.cs" Inherits="NewIspNL.Pages.ResellerPaymenyCredit" %>



<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view">
        <fieldset>
            <div class="page-header"><h1>
                <asp:Literal Text="<%$Resources:Tokens,ResellerCredit %>" runat="server" /></h1></div>
                
                   <div class="row">
        <div class="col-sm-12">
            <div class="form-horizontal">
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,Reseller %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:DropDownList runat="server" ID="DdlReseller" CssClass="width-60 chosen-select" ClientIDMode="Static" AutoPostBack="True"
                            OnSelectedIndexChanged="DdlReseller_SelectedIndexChanged"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DdlReseller"
                            ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                </div>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal Visible="False" ID="Literal1" runat="server" Text="<%$Resources:Tokens,sav %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:DropDownList runat="server" ID="ddlSaves" DataTextField="SaveName" DataValueField="Id"  Width="178px" Visible="False"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlSaves" ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                </div>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="ltAmount" Visible="False" runat="server" Text="<%$Resources:Tokens,Amount %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:TextBox runat="server" Visible="False" ID="TbAmount" ClientIDMode="Static" />
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
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="ltNotes" Visible="False" runat="server" Text="<%$Resources:Tokens,Notes %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:TextBox runat="server" ID="TbNotes" Visible="False" ClientIDMode="Static" TextMode="MultiLine" />
                    </div>
                </div>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="ltEffect" Visible="False" runat="server" Text="<%$Resources:Tokens,Effect %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:RadioButtonList runat="server" Visible="False" ID="RblEffect" ClientIDMode="Static" RepeatDirection="Horizontal">
  </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="RblEffect"
                            ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>

                    </div>
                </div>
                </div></div>
                                        <div class="clearfix form-actions">
										<div class="col-md-offset-3 col-md-9">
											<%--<button class="btn btn-info" type="button" Visible="False" id="BSave" runat="server" OnServerClick="BSave_Click">
												<i class="icon-ok bigger-110"></i>
												<asp:Literal runat="server" Text="<%$Resources:Tokens,Save %>"></asp:Literal>
											</button>--%>
                                            
                                            
                                            <asp:Button ID="btn_Payment" Visible="False" CssClass="btn btn-info" runat="server" Text="<%$Resources:Tokens,Save %>" Width="97px" 
                                     onclick="BSave_Click"  UseSubmitBehavior="false" OnClientClick="plswait(this.id) " />
                

											&nbsp; &nbsp; &nbsp;
											<button class="btn" type="reset">
												<i class="icon-undo bigger-110"></i>
												<%=Tokens.reset %>
											</button>    <br/>            <div id="messagel">
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
                <asp:Literal Text="<%$Resources:Tokens,CreditHistory %>" runat="server" /></h3>
             <table class="alert alert-info">
                    <tr>
                        <td><%=Tokens.Credit %></td>
                        <td>:&nbsp;</td>
                        <td><asp:Label runat="server" ID="lblLastCredit"></asp:Label></td>
                    </tr>
                </table>
           
        </fieldset>
    </div>
    <!--start: Dropdownlist Files-->
       <link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>
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


        $(document).ready(function() {
            $(".chosen-select").chosen();
            $('tr td input[type=radio]').addClass("ace");
            $('tr td label').addClass("lbl").css({ 'padding': '7px' });
        });
    </script>
</asp:Content>

