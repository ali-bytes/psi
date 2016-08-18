<%@ Page Title="<%$Resources:Tokens,BranchesTransformation%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="BranchesTransformation.aspx.cs" Inherits="NewIspNL.Pages.BranchesTransformation" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
<link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>

        
    <div class="view">
        <fieldset>
            <div class="page-header"><h1>
                <asp:Literal runat="server" ID="lbltitle" ></asp:Literal></h1></div>
                <div id="message" runat="server"></div>
                    <div class="row">
        <div class="col-sm-12">
            <div class="form-horizontal">
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal3" runat="server" Text="<%$Resources:Tokens,Branches %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:DropDownList runat="server" CssClass="width-60 chosen-select" ID="ddlBranches" ClientIDMode="Static" Width="200px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlBranches"
                            Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,Required %>" ValidationGroup="trans"></asp:RequiredFieldValidator>
                    </div>
                </div>
                </div></div>
                <div class="space-4"></div>
               <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,TransferFrom %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:DropDownList runat="server" ID="ddlTransferFrom" ClientIDMode="Static" Width="200px"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlTransferFrom_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlTransferFrom"
                            Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,Required %>" ValidationGroup="trans"></asp:RequiredFieldValidator>
                    </div>
                </div>
                </div></div>
                <div class="space-4"></div>
                 <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal4" runat="server" Text="<%$Resources:Tokens,Credit %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:Literal runat="server" ID="lblcredit"></asp:Literal>
                    </div>
                </div>
                </div></div>
                <div class="space-4"></div>
                               <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,TransferTo %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:DropDownList runat="server" ID="ddlTransferTo" ClientIDMode="Static" Width="200px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlTransferTo"
                            Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,Required %>" ValidationGroup="trans"></asp:RequiredFieldValidator>
                    </div>
                </div>
                </div></div>
                <div class="space-4"></div>
                                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal5" runat="server" Text="<%$Resources:Tokens,TransferAmount %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:TextBox runat="server" ID="txtAmount" Width="200px" ClientIDMode="Static"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAmount"
                            Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,Required %>" ValidationGroup="trans"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtAmount"
                            Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,TransferAmountLimit %>"
                            MaximumValue="9999999999999999" MinimumValue="0" Type="Double"></asp:RangeValidator>
                    </div>
                </div>
                
                <asp:HiddenField runat="server" ID="hdnOldAmount"/>
            
                </div></div>
                <div class="clearfix form-actions">
										<div class="col-md-offset-3 col-md-9">
										<%--	<button class="btn btn-info" type="button" clientidmode="Static" ID="b_save" OnServerClick="Save_Click" runat="server" ValidationGroup="trans">
												<i class="icon-ok bigger-110"></i>
												<asp:Literal runat="server" Text="<%$ Resources:Tokens,Save %>" ID="lblsave"></asp:Literal>
											</button>--%>
                                            
                                            
                                                 <asp:Button ID="btn_Payment"  CssClass="btn btn-info" runat="server" Text="<%$Resources:Tokens,Save %>" Width="97px" 
                                     onclick="Save_Click" ValidationGroup="trans" UseSubmitBehavior="false" OnClientClick="plswait(this.id) " />

											&nbsp; &nbsp; &nbsp;
											<button class="btn" type="reset">
												<i class="icon-undo bigger-110"></i>
												<%=Tokens.reset %>
											</button>
										</div>
									</div>

                
            </div></div></div>
                
                

        </fieldset>
    </div>
    <script type="text/javascript">

        function plswait(id) {

            var type = document.getElementById('<%=ddlBranches.ClientID%>').value;
            var amo = document.getElementById('<%=ddlTransferFrom.ClientID%>').value;

            var type2 = document.getElementById('<%=ddlTransferTo.ClientID%>').value;
            var com = document.getElementById('<%=txtAmount.ClientID%>').value;



            if (type == "" || amo == "" || type2 == "" || com == "") { return; }
            else {
                var check2 = document.getElementById(id);
                check2.disabled = 'true'; check2.value = 'Please wait...';
            }

        }


        $(document).ready(function () {
            $(".chosen-select").chosen();
        });
    </script>

</asp:Content>
