<%@ Page Title="<%$Resources:Tokens,Transfers%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="Transfers.aspx.cs" Inherits="NewIspNL.Pages.Transfers" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="view">
        <fieldset>
            <div class="page-header"><h1>
                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,Transfers %>" runat="server" /></h1></div>
                
                
                    <div class="row">
        <div class="col-sm-12">
            <div class="form-horizontal">
                
                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal4" runat="server" Text="<%$Resources:Tokens,TransferFrom %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div style="float: right;">
                            <asp:DropDownList runat="server" ID="fromdrop" ClientIDMode="Static" 
                                AutoPostBack="True" onselectedindexchanged="fromchange" Width="178px"/>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="fromdrop"
                                                        ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                        </div>
                        <div style="float: right;">
                            <asp:DropDownList runat="server" ID="ddlFromSave" AutoPostBack="True" OnTextChanged="FromValue" ClientIDMode="Static" 
                                Width="178px"/>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlFromSave"
                                                        ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                            
                            
                    </div> </div><asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                </div></div>
                <div class="space-4"></div>

<div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal5" runat="server" Text="<%$Resources:Tokens,To %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                     <div style="float: right;">
                            <asp:DropDownList runat="server" ID="todrop" ClientIDMode="Static" 
                                AutoPostBack="True" onselectedindexchanged="tochange" Width="178px"/>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="todrop"
                                                        ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                        </div>
                    <div style="float: right;">
                        <asp:DropDownList runat="server" ID="ddlToSave" ClientIDMode="Static" AutoPostBack="True" OnTextChanged="toValue"  Width="178px"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlToSave"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                   <div><asp:Label ID="Label2" runat="server" Text=""></asp:Label></div> 
                </div>
                </div></div>
                <div class="space-4"></div>

<div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal8" runat="server" Text="<%$Resources:Tokens,Amount %>"></asp:Literal></label>
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
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal9" runat="server" Text="<%$Resources:Tokens,Notes %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                        <div>
                            <asp:TextBox runat="server" ID="TbNotes" ClientIDMode="Static" TextMode="MultiLine"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TbNotes"
                                                        ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                
                
            
                </div></div>
                                        <div class="clearfix form-actions">
										<div class="col-md-offset-3 col-md-9">
											<%--<button class="btn btn-info" type="button" runat="server" ID="BtnSave" OnServerClick="BtnSave_Click">
												<i class="icon-ok bigger-110"></i>
												<asp:Literal runat="server" Text="<%$Resources:Tokens,TransferAmount %>" ID="lbltransfer"></asp:Literal>
											</button>--%>
                                            
                                            
                                            
                                             <i class="icon-ok bigger-110">    <asp:Button ID="save_b" runat="server" Text="<%$ Resources:Tokens,TransferAmount %>" CssClass="btn btn-info"
                            Width="97px"  OnClick="BtnSave_Click" UseSubmitBehavior="false" OnClientClick="plswait(this.id)" />
                    </i>
                                            
                                            

											&nbsp; &nbsp; &nbsp;
											<button class="btn" type="reset">
												<i class="icon-undo bigger-110"></i>
												<%=Tokens.reset %>
											</button> <br/>     <div id="messagediv">
                        <asp:Literal runat="server" ID="Message"></asp:Literal>
                    </div>
										</div>
									</div>

            </div></div></div>               

 </fieldset>
    </div>
    
      <script type="text/javascript">

          function plswait(id) {

              var type = document.getElementById('<%=ddlFromSave.ClientID%>').value;
              var amo = document.getElementById('<%=TbAmount.ClientID%>').value;

              var type2 = document.getElementById('<%=ddlToSave.ClientID%>').value;
              var com = document.getElementById('<%=TbNotes.ClientID%>').value;



              if (type == "" || amo == "" || type2 == "" || com == "") { return; }
              else {
                  var check2 = document.getElementById(id);
                  check2.disabled = 'true'; check2.value = 'Please wait...';
              }

          }     </script>
    

</asp:Content>



