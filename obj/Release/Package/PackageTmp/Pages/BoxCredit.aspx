<%@ Page Title="<%$Resources:Tokens,BoxCredit %>"  Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="BoxCredit.aspx.cs" Inherits="NewIspNL.Pages.BoxCredit" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <fieldset>
            <div class="page-header">
               <h1> <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,BoxCredit %>" runat="server" /></h1></div>
            <div class="well">
                <div>
                    <label for="DdlBox">
                        <asp:Literal ID="Literal2" Text="<%$Resources:Tokens,OneBox %>" runat="server" /></label>
                    <div>
                        <asp:DropDownList runat="server" ID="DdlBox" ClientIDMode="Static" AutoPostBack="True"
                            OnSelectedIndexChanged="DdlReseller_SelectedIndexChanged" Width="180px" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DdlBox"
                            ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div>
                    <label for="TbAmount">
                        <asp:Literal ID="Literal3" Text="<%$Resources:Tokens,Amount %>" runat="server" />
                    </label>
                    <div>
                        <asp:TextBox runat="server" ID="TbAmount" ClientIDMode="Static" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TbAmount"
                            ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="TbAmount"
                            ErrorMessage="<%$Resources:Tokens,NumbersOnly %>" MaximumValue="99999999999"
                            MinimumValue=".1" Type="Double"></asp:RangeValidator>
                    </div>
                </div>
                <div>
                    <label for="TbNotes">
                        <asp:Literal ID="Literal4" Text="<%$Resources:Tokens,Notes %>" runat="server" />
                    </label>
                    <div>
                        <asp:TextBox runat="server" ID="TbNotes" ClientIDMode="Static" TextMode="MultiLine" />
                    </div>
                </div>
                <div>
                    <label for="RblEffect">
                        <asp:Literal ID="Literal5" Text="<%$Resources:Tokens,Effect %>" runat="server" />
                    </label>
                    <div class="control-group">
                        <asp:RadioButtonList runat="server" ID="RblEffect" ClientIDMode="Static" RepeatDirection="Horizontal">
                            <asp:ListItem Selected="True" Text="<%$Resources:Tokens,Add %>" ></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Tokens,Subtract %>"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <p>
                    <br/>
                    <asp:Button runat="server"  ID="BSave" OnClick="BSave_Click" CssClass="btn btn-primary" Text=" <%$Resources:Tokens,save %>" UseSubmitBehavior="false" OnClientClick="plswait(this.id)"/>
                      
                
                </p>
                <div id="message">
                    <asp:Literal runat="server" ID="Message"></asp:Literal>
                </div>
            </div>
        </fieldset>
        <div id="amountmessage">
                    <div  runat="server" id="Msg" class="alertMsg"></div>
                </div>
        <%--<h3 class="header smaller lighter blue"><asp:Literal ID="Literal6" Text="<%$Resources:Tokens,CreditHistory %>" runat="server" /></h3>--%>
        <div>
                <asp:GridView runat="server" ID="GvHistory" CssClass="table table-bordered table-condensed"
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
                        <asp:BoundField DataField="Box" HeaderText="<%$Resources:Tokens,Box %>" />
                       <%--<asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,User %>" />--%>
                        <asp:BoundField DataField="Date" HeaderText="<%$Resources:Tokens,Date %>" />
                        <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes %>" />
                       <%-- <asp:TemplateField>
                            <ItemTemplate>
                                <a class="btn btn-success" href='<%#Eval("RecieptUrl") %>' target="_blank">
                                    <%= Tokens.Reciept %></a>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                    <EmptyDataTemplate>
                        <div>
                            <%= Tokens.NoResults %></div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
                        <%--<asp:LinkButton ID="BtnExport" runat="server" CausesValidation="False"--%> 
                        <%--Width="100px" onclick="BtnExport_Click" CssClass="btn btn-success"><i class="icon-file-text"></i>&nbsp;<asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:Tokens,Export %>"></asp:Literal></asp:LinkButton>--%>
    </div>   
    
    
    
    <script type="text/javascript">
    
     function plswait(id) {

            var type = document.getElementById('<%=DdlBox.ClientID%>').value;
              var amo = document.getElementById('<%=TbAmount.ClientID%>').value;




              if (type == "" || amo == "") { return; }
              else {
                  var check2 = document.getElementById(id);
                  check2.disabled = 'true'; check2.value = 'Please wait...';
              }

     }     </script>
</asp:Content>


