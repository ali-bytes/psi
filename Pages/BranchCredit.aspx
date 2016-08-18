<%@ Page Title="<%$Resources:Tokens,BranchCredit %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="BranchCredit.aspx.cs" Inherits="NewIspNL.Pages.BranchCredit" %>

<%@ Import Namespace="Resources" %>

<%@ Reference VirtualPath="~/Pages/ResellerCredit.aspx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    
    <div class="row">
        <div class="view">
            <fieldset>
                <div class="page-header">
                   <h1> <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,BranchCredit %>" runat="server" /></h1>
                </div>
                <div class="well">
                    <div>
                        <label for="ddl_branchs">
                            <asp:Literal ID="Literal2" Text="<%$Resources:Tokens,Branch %>" runat="server" />
                        </label>
                        <div>
                            <asp:DropDownList runat="server" ID="ddl_branchs" Width="150px" ClientIDMode="Static"
                                              ValidationGroup="search">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddl_branchs"
                                                        ErrorMessage="<%$Resources:Tokens,Required %>" 
                                InitialValue="empty" ValidationGroup="Br">*</asp:RequiredFieldValidator>
                        </div>
                        <label><%=Tokens.CreditFrom %></label><br />
                    <div>
                    <asp:RadioButtonList runat="server" ID="rblFrom" RepeatDirection="Vertical" CssClass="radio">
                        <asp:ListItem Selected="True" Value="<%$Resources:Tokens,BranchPR %>"></asp:ListItem>
                        <asp:ListItem Value="<%$Resources:Tokens,BranchBalanceSheet %>"></asp:ListItem>
                        <asp:ListItem Value="<%$Resources:Tokens,BranchVoiceCredit %>"></asp:ListItem>
                    </asp:RadioButtonList></div>
                    </div>
                    <p>
                        <asp:Button runat="server" ID="BSearch" CssClass="btn btn-success" Text="<%$Resources:Tokens,ShowCredit %>"
                                    OnClick="BSearch_OnClick" ValidationGroup="Br" />
                                     <asp:Button runat="server" CssClass="btn btn-success" ID="Button1" Text="<%$ Resources:Tokens,ClearAllBranchcredits %>" 
                                OnClick="b_addRequest1_Click" Visible="False" />
                    </p>
                </div>
                <section>
                     <div runat="server" id="one" Visible="False">
                        <asp:GridView runat="server" ID="GCredits"
                                       CssClass="table table-bordered table-condensed text-center"
                                      GridLines="Horizontal" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField DataField="Credit" HeaderText="<%$Resources:Tokens,Credit %>" />
                                <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,Branch %>" />
                            </Columns>
                            
                        </asp:GridView>
                    </div>
                             <div runat="server" id="all" Visible="False">
                        <asp:GridView runat="server" ID="Allreseleer"  GridLines="Horizontal" 
                                      AutoGenerateColumns="False" CssClass="table table-bordered table-condensed text-center">
                            <Columns>
                                <asp:BoundField DataField="Reseller" HeaderText="<%$ Resources:Tokens,Reseller%>" />
                                
                                 <asp:BoundField DataField="Credit" HeaderText="<%$ Resources:Tokens,Credit %>" />
                            <asp:BoundField DataField="currentpill" HeaderText="<%$ Resources:Tokens,currentpill %>" />
                           
                                 <asp:BoundField DataField="MenuResellerCredit" HeaderText="<%$ Resources:Tokens,MenuResellerCredit %>" />
                           
                                
                                 </Columns>
                        </asp:GridView>
                  
                            <p>
                      
                    </p>
                          </div>
                    <div class="alert alert-info">
                        <asp:Literal  runat="server" ID="LTotal"></asp:Literal>
                    </div>
                </section>
            </fieldset>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            $('<option value="empty"></option>').prependTo('#ddl_branchs');
        });
    </script>
</asp:Content>
