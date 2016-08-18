<%@ Page Title="<%$ Resources:Tokens,BoxBs %>" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="BoxBS.aspx.cs" Inherits="NewIspNL.Pages.BoxBS" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div id="tr_Search" runat="server">
            <fieldset>
                <div class="page-header">
                    <h1><asp:Label ID="Label38" runat="server" 
                               Text="<%$ Resources:Tokens,Search %>"></asp:Label></h1>
                </div>
                <div class="well">
                    <asp:Label AssociatedControlID="ddl_Box" ID="Label34" runat="server" Text="<%$ Resources:Tokens,OneBox %>"></asp:Label>
                    <div class="style1">
                        <asp:DropDownList ID="ddl_Box" runat="server" Width="157px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ControlToValidate="ddl_Box" ID="RequiredFieldValidator12"
                                                    runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>" 
                                                    ValidationGroup="SearchBox"></asp:RequiredFieldValidator>
                      
                    </div>
                    <div>
                        <label for="TbStartAt">
                            <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,From %>" runat="server" />
                        </label>
                        <div>
                            <asp:TextBox runat="server" ID="TbStartAt" data-select="dp" Width="155px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RTbFrom" ValidationGroup="SearchBox" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                        ControlToValidate="TbStartAt" runat="server" />
                        </div>
                    </div>
                    <div>
                        <label for="TbTo">
                            <asp:Literal ID="Literal2" Text="<%$Resources:Tokens,To %>" runat="server" />
                        </label>
                        <div>
                            <asp:TextBox runat="server" ID="TbTo" data-select="dp" Width="155px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RTbTo" ValidationGroup="SearchBox" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                        ControlToValidate="TbTo" runat="server" />
                        </div>
                    </div>
                    <p>
                        <br/>
                        <asp:LinkButton ID="btn_search" class="btn btn-primary" runat="server" Width="100px" OnClick="btn_search_Click"
                                    ValidationGroup="SearchBox" ><i class="icon-search icon-only">&nbsp; <%=Tokens.Search %></i></asp:LinkButton>
                    </p>             
                    
                </div>

            </fieldset>
        </div>
        <div>
            <div  id="tb_SearchResult" runat="server" visible="false">
                <div>
                    <asp:GridView  
                        CssClass="table table-bordered table-condensed center" ID="grd_Transactions" runat="server" 
                        AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="Time" HeaderText="<%$ Resources:Tokens,Date %>"/>
                            <asp:BoundField DataField="BoxName" HeaderText="<%$ Resources:Tokens,Box %>"/>
                            <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:Tokens,User %>"/>
                            <asp:BoundField DataField="Depositer" HeaderText="<%$Resources:Tokens,Depositor %>"/>
                            <asp:BoundField DataField="Amount" HeaderText="<%$ Resources:Tokens,Amount %>"/>
                            <asp:BoundField DataField="Total" HeaderText="<%$ Resources:Tokens,Credit %>"/>
                            <asp:TemplateField HeaderText="<%$Resources:Tokens,Notes %>">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("Notes")%>' ID="lblnote"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                            <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,ResellerandBranch %>"/>
                            <asp:TemplateField HeaderText="<%$Resources:Tokens,Attachments %>">
                            <ItemTemplate>
                                <a target="_blank" href="<%#Eval("link") %>"><%#Eval("link") %></a>
                            </ItemTemplate>
                        </asp:TemplateField>

                        </Columns>
                        <EmptyDataTemplate><%=Tokens.NoResults %></EmptyDataTemplate>
                    </asp:GridView>
                </div>
                                    <asp:LinkButton ID="BtnExport" runat="server" Visible="False" 
                        Width="100px" onclick="BtnExport_Click" CssClass="btn btn-success"><%= Tokens.Export %><i class="icon-file-text"></i>&nbsp;<asp:Literal runat="server" ></asp:Literal></asp:LinkButton>
            </div>
        </div>
    </div>
 
        <script type="text/javascript">
            $(function () {
                $('input[data-select="dp"]').datepicker({
                    showOtherMonths: true,
                    selectOtherMonths: false,
                    dateFormat: 'dd/mm/yy'
                });
            });
    </script>
</asp:Content>

