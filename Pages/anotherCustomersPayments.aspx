<%@ Page Title="<%$Resources:Tokens,anotherCustomersPayments %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="anotherCustomersPayments.aspx.cs" Inherits="NewIspNL.Pages.anotherCustomersPayments" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.7.123, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div class="row">
        <fieldset>
            <div class="page-header">
                <h1><asp:Literal ID="Literal1" Text="<%$Resources:Tokens,anotherCustomersPayments %>" runat="server" /></h1></div>
            <div class="well">
                <div>
                    <label for="DdlReseller">
                        <asp:Literal ID="Literal2" Text="<%$Resources:Tokens,Box %>" runat="server" /></label>
                    <div>
                        <asp:DropDownList runat="server" ID="DdlBox" ClientIDMode="Static" AutoPostBack="True" Width="178px"
                                          OnSelectedIndexChanged="DdlBox_SelectedIndexChanged"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DdlBox"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                </div>
                                <div>
                    <label for="DdlVoiceCompany">
                        <asp:Literal ID="Literal5" Text="<%$Resources:Tokens,Company %>" runat="server" /></label>
                    <div>
                        <asp:DropDownList runat="server" ID="DdlVoiceCompany" ClientIDMode="Static" Width="178px"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="DdlVoiceCompany"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div>
                    <label for="ddlSaves">
                        <asp:Literal runat="server" Text="<%$Resources:Tokens,sav %>"></asp:Literal>
                    </label>
                    <div>
                        <asp:DropDownList runat="server" ID="ddlSaves" DataTextField="SaveName" DataValueField="Id" ClientIDMode="Static" Width="178px"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlSaves"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div>
                    <label for="TbClientName">
                        <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:Tokens,Customer.Name %>"></asp:Literal>
                    </label>
                    <div>
                        <asp:TextBox runat="server" ID="TbClientName" ClientIDMode="Static"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TbClientName"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div>
                    <label for="TbClientPhone">
                        <asp:Literal ID="Literal6" Text="<%$Resources:Tokens,Customer.Phone %>" runat="server" /></label>
                    <div>
                        <asp:TextBox runat="server" ID="TbClientPhone" ClientIDMode="Static"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="TbClientPhone_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="TbClientPhone">
                        </asp:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TbClientPhone"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
<%--                        <asp:FilteredTextBoxExtender ID="txt_CustomerPhone_FilteredTextBoxExtender" runat="server"
                                                     FilterType="Numbers" TargetControlID="TbClientPhone" Enabled="True">
                        </asp:FilteredTextBoxExtender>--%>
                    </div>
                </div>

                <div>
                    <label for="TbInvoiceAmount">
                        <asp:Literal ID="Literal3" Text="<%$Resources:Tokens,invoiceAmount %>" runat="server" />
                    </label>
                    <div>
                        <asp:TextBox runat="server" ID="TbInvoiceAmount" ClientIDMode="Static" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TbInvoiceAmount"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="TbInvoiceAmount"
                                            ErrorMessage="<%$Resources:Tokens,NumbersOnly %>" MaximumValue="99999999999"
                                            MinimumValue=".1" Type="Double"></asp:RangeValidator>
                    </div>
                </div>
                                <div>
                    <label for="TbBoxAmount">
                        <asp:Literal ID="Literal9" Text="<%$Resources:Tokens,BoxAmount %>" runat="server" />
                    </label>
                    <div>
                        <asp:TextBox runat="server" ID="TbBoxAmount" ClientIDMode="Static" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="TbBoxAmount"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="TbBoxAmount"
                                            ErrorMessage="<%$Resources:Tokens,NumbersOnly %>" MaximumValue="99999999999"
                                            MinimumValue=".1" Type="Double"></asp:RangeValidator>
                    </div>
                </div>
                <div>
                    <label for="TbNotes">
                        <asp:Literal runat="server" ID="Literal8" Text="<%$Resources:Tokens,Notes %>"></asp:Literal>
                    </label>
                    <div>
                        <asp:TextBox runat="server" ID="TbNotes" ClientIDMode="Static" TextMode="MultiLine"/>
                    </div>
                </div>
                
                <p>
                    <br/>
                  <%--  <asp:LinkButton runat="server" CssClass="btn btn-success" ID="BtnSave"
                                onclick="BtnSave_Click" ><i class="icon-save icon-only"></i>&nbsp;<%=Tokens.Save %></asp:LinkButton>
                    --%>
                    
                        <asp:Button ID="btn_Payment"  CssClass="btn btn-info" runat="server" Text="<%$Resources:Tokens,Save %>" Width="97px" 
                                     onclick="BtnSave_Click"    UseSubmitBehavior="false" OnClientClick="plswait(this.id) " />
                

                </p>
                <div id="message">
                    <asp:Literal runat="server" ID="Message"></asp:Literal>
                </div>
            </div>
        </fieldset>
    </div>
    <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <asp:Literal ID="Literal4" Text="<%$Resources:Tokens,CreditHistory %>" runat="server" /></h3>
            <div>
                <asp:GridView runat="server" ID="GvHistory" CssClass="table table-bordered table-condensed"
                    AutoGenerateColumns="False" OnDataBound="GvHistory_DataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="no" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ID" HeaderText="<%$Resources:Tokens,InvoiceNumber %>"/>
                        <asp:BoundField DataField="Type" HeaderText="<%$Resources:Tokens,Type %>" />
                        <asp:BoundField DataField="BoxAmount" HeaderText="<%$Resources:Tokens,BoxAmount %>" />
                        <asp:BoundField DataField="InvoiceAmount" HeaderText="<%$Resources:Tokens,invoiceAmount %>" />
                        <asp:BoundField DataField="Box" HeaderText="<%$Resources:Tokens,Box %>" />
                        <asp:BoundField DataField="CompanyName" HeaderText="<%$Resources:Tokens,Company%>"/>
                        <asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,Customer.Name %>" />
                        <asp:BoundField DataField="CustomerTelephone" HeaderText="<%$Resources:Tokens,Customer.Phone %>"/>
                        <asp:BoundField DataField="Date" HeaderText="<%$Resources:Tokens,Date %>" />
                        <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes %>" />
                       <asp:TemplateField>
                            <ItemTemplate>
                                <a class="btn btn-success" href='<%#Eval("RecieptUrl") %>' target="_blank">
                                    <%= Tokens.Reciept %></a>
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

            var type = document.getElementById('<%=DdlBox.ClientID%>').value;
            var amo = document.getElementById('<%=DdlVoiceCompany.ClientID%>').value;
            var com = document.getElementById('<%=TbClientName.ClientID%>').value;

            var type2 = document.getElementById('<%=TbClientPhone.ClientID%>').value;
            var amo2 = document.getElementById('<%=TbInvoiceAmount.ClientID%>').value;
            var com2 = document.getElementById('<%=TbBoxAmount.ClientID%>').value;
            var com3 = document.getElementById('<%=ddlSaves.ClientID%>').value;

            if (type == "" || amo == "" || com == "" || type2 == "" || amo2 == "" || com2 == "" || com3 == "") { return; }
            else {
                var check2 = document.getElementById(id);
                check2.disabled = 'true'; check2.value = 'Please wait...';
            }

        }    </script>
</asp:Content>

