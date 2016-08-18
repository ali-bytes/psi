<%@ Page Title="<%$ Resources:Tokens,SMSCnfg %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="SMSCnfg.aspx.cs" Inherits="NewIspNL.Pages.SMSCnfg" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="v_index" runat="server">
                <div id="message">
                    <asp:Label runat="server" ID="l_message"></asp:Label>
                </div>
                                <div class="view">
                        <fieldset>
                <asp:Panel runat="server" ID="p_index">
                <div class="page-header"><h1><asp:Literal runat="server" ID="lblindex" Text="<%$ Resources:Tokens,Index %>"></asp:Literal></h1></div>
                    
                    <asp:GridView ID="gv_index" runat="server" GridLines="Horizontal"
                                  AutoGenerateColumns="False" OnDataBound="gv_index_DataBound"
                                  CssClass="table table-bordered table-condensed text-center">
                        <Columns>
                            <asp:TemplateField HeaderText="#">
                                <ItemTemplate>
                                    <asp:Label ID="l_number" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,UserName %>" />
                            <asp:BoundField DataField="Password" HeaderText="<%$Resources:Tokens,Password %>" />
                            <asp:BoundField DataField="Sender" HeaderText="<%$Resources:Tokens,SenderName %>" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                                <asp:LinkButton ID="gvb_edit" runat="server" CommandArgument='<%# Bind("Id") %>' OnClick="gvb_edit_Click"
                                                            ToolTip="<%$ Resources:Tokens,Edit %>" data-rel="tooltip"><i class="icon-pencil icon-only bigger-130"></i></asp:LinkButton>


                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                </asp:Panel>
                                        </fieldset>
                    </div>
            </asp:View>
            <asp:View ID="v_AddEdit" runat="server">
                <div class="view">
                    <fieldset>
                <asp:Panel runat="server" ID="p_add">
                <div class="page-header"><h1><asp:Literal runat="server" ID="Literal1" Text="<%$ Resources:Tokens,SMSCnfg %>"></asp:Literal></h1></div>
                    <div class="well">
                        <asp:HiddenField ID="hf_id" runat="server" />
                        <div></div><%--runat="server" ID="inputs"--%>
                            <div>
                                <asp:Label runat="server" ID="lblurl" AssociatedControlID="txtUrl" Text="<%$Resources:Tokens,Url %>"></asp:Label>
                                <div>
                                    <asp:TextBox runat="server" ID="txtUrl" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtUrl"
                                                                ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                                                                EX: http://www.resalty.net/api/sendSMS.php? only
                                </div>
                            </div>
                            <div>
                                <label>
                                    <asp:Label ID="Label2" AssociatedControlID="txtUserName" runat="server" Text="<%$ Resources:Tokens,UserName %>"></asp:Label>
                                </label>
                                <div>
                                    <asp:TextBox runat="server" ID="txtUserName" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="RFMailFrom" ControlToValidate="txtUserName"
                                                                ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div>
                                <label>
                                    <asp:Label ID="LSmtpClient" AssociatedControlID="txtPassword" runat="server" Text="<%$ Resources:Tokens,Password %>"></asp:Label>
                                </label>
                                <div>
                                    <asp:TextBox runat="server" ID="txtPassword" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="RTbSmtpClient" ControlToValidate="txtPassword"
                                                                ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div>
                                <label>
                                    <asp:Label ID="LUserName" AssociatedControlID="txtSender" runat="server" Text="<%$ Resources:Tokens,SenderName %>"></asp:Label>
                                </label>
                                <div>
                                    <asp:TextBox runat="server" ID="txtSender" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="RTbUserName" ControlToValidate="txtSender"
                                                                ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div>
                                <asp:CheckBox runat="server" ID="chkSend" Text="<%$Resources:Tokens,SendsmsMessage %>"/>
                            </div>


                        

                    </div>
                    <div class="row-fluid">
                        
                        
                    <div class="col-sm-6">
                    <div class="well">
                               <h3 class="header smaller lighter blue"><%=Tokens.NotificationOfCustomersAndInvoices %></h3>                    
                            <div>
                               <asp:CheckBoxList runat="server"  ClientIDMode="Static" RepeatDirection="Vertical" ID="CheckNotification">
                </asp:CheckBoxList>
                            </div>
                    </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="well" id="txtLis" runat="server" clientidmode="Static">
                            <h3 class="header smaller lighter blue"><%=Tokens.Message %></h3>
                            <div>
                            <asp:TextBox runat="server" ToolTip="0" ID="txt0CustomerInvoice"></asp:TextBox></div>
                            <div>
                            <asp:TextBox runat="server" ToolTip="1" ID="txt1ResellerInvoice"></asp:TextBox>
                            </div>
                            <div>
                            <asp:TextBox runat="server" ToolTip="2" ID="txt2BranchInvoice"></asp:TextBox></div>
                            <div>
                            <asp:TextBox runat="server" ToolTip="3" ID="txt3CustomerPaymentInvoice"></asp:TextBox></div>
                            <div>
                            <asp:TextBox runat="server" ToolTip="4" ID="txt4AddCreditToReseller"></asp:TextBox></div>
                            <div>
                            <asp:TextBox runat="server" ToolTip="5" ID="txt5AddCreditToBranch"></asp:TextBox></div>
                            <div>
                            <asp:TextBox runat="server" ToolTip="6" ID="txt6ResellerPaymentFromBs"></asp:TextBox></div>
                            <div>
                            <asp:TextBox runat="server" ToolTip="7" ID="txt7BranchPaymentFromBs"></asp:TextBox></div>
                            <div>
                            <asp:TextBox runat="server" ToolTip="8" ID="txt8ConfirmRechargeReseller"></asp:TextBox></div>
                            <div>
                            <asp:TextBox runat="server" ToolTip="9" ID="txt9ConfirmRechargeBranch"></asp:TextBox></div>
                            <div>
                            <asp:TextBox runat="server" ToolTip="10" ID="txt10RunLine"></asp:TextBox></div>
                            <div>
                            <asp:TextBox runat="server" ToolTip="11" ID="txt11Suspend"></asp:TextBox></div>
                            <div>
                            <asp:TextBox runat="server" ToolTip="12" ID="txt12Cancle"></asp:TextBox></div>
                            <div>
                            <asp:TextBox runat="server" ToolTip="13" ID="txt13ChangePackage"></asp:TextBox></div>
                            <div>
                            <asp:TextBox runat="server" ToolTip="14" ID="txt14AddIPPackage"></asp:TextBox></div>
                            <div>
                            <asp:TextBox runat="server" ToolTip="15" ID="txt15AddExtraGiga"></asp:TextBox></div>
                                                        <div>
                            <asp:TextBox runat="server" ToolTip="16" ID="txt16AddNewCustomer"></asp:TextBox></div>
                        </div>
                    </div>
                        
                        </div>
                                            <p>
                            <asp:Button runat="server" ID="b_save" CssClass="btn btn-primary" Text="<%$ Resources:Tokens,Save %>" OnClick="b_save_Click"/>
                        </p>
                </asp:Panel>
                </fieldset>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>

            <style type="text/css">
        #CheckNotification>tbody>tr>td>label {
            display: -webkit-inline-box;
            margin-right: 4px;
            margin-bottom: 17px;
        }
        #txtLis input[type="text"] {
            margin-bottom: 9px;
        }
    </style>
</asp:Content>


