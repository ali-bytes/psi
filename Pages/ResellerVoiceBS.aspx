<%@ Page Title="<%$ Resources:Tokens,ResellerVoiceBS %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ResellerVoiceBS.aspx.cs" Inherits="NewIspNL.Pages.ResellerVoiceBS" %>


<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
<link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>

        <div class="view ">
        <div id="tr_Search" runat="server">
            <fieldset>
                <div class="page-header"><h1>
                    <asp:Label ID="Label38" runat="server" 
                               Text="<%$ Resources:Tokens,ResellerVoiceBS %>"></asp:Label></h1>
                </div>
                <div class="well">
                    <asp:Label AssociatedControlID="ddl_Reseller" ID="Label34" runat="server" Text="<%$ Resources:Tokens,Resellers %>"></asp:Label>
                    <div class="style1">
                        <asp:DropDownList ID="ddl_Reseller" CssClass="chosen-select" runat="server" Width="155px">
                        </asp:DropDownList>
                                             <asp:RequiredFieldValidator ControlToValidate="ddl_Reseller" ID="RequiredFieldValidator12"
                                                    runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>" 
                                                    ValidationGroup="SearchBox"></asp:RequiredFieldValidator>
                      
                    </div>
                    <div>
                        <label for="TbStartAt">
                            <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,From %>" runat="server" />
                        </label>
                        <div>
                            <asp:TextBox runat="server" ID="TbStartAt" data-select="dp" Width="155px"></asp:TextBox>
                               </div>
                    </div>
                    <div>
                        <label for="TbTo">
                            <asp:Literal ID="Literal2" Text="<%$Resources:Tokens,To %>" runat="server" />
                        </label>
                        <div>
                            <asp:TextBox runat="server" ID="TbTo" data-select="dp" Width="155px"></asp:TextBox>
                           </div>
                    </div>
                    <p>
                        <br/>
                        <asp:Button ID="btn_search" class="btn btn-primary" runat="server" Text="<%$Resources:Tokens ,Search%>" Width="100px" OnClick="btn_search_Click"
                                    ValidationGroup="SearchBox" />
                    </p>             
                    
                </div>

            </fieldset>
        </div>
        <div>
            <div  id="tb_SearchResult" runat="server" visible="false">
                <div>
                    <asp:GridView  OnDataBound="grd_DataBound"
                        CssClass="table table-bordered table-condensed" ID="grd_Transactions" runat="server" 
                        AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="Time" HeaderText="<%$ Resources:Tokens,Date %>">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ID" HeaderText="<%$Resources:Tokens,InvoiceNumber %>"/>
                            <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:Tokens,Reseller %>">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ClientName" HeaderText="<%$ Resources:Tokens,Customer.Name %>">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ClientTelephone" HeaderText="<%$ Resources:Tokens,Customer.Phone %>">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CompanyName" HeaderText="<%$ Resources:Tokens,Company %>">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Amount" HeaderText="<%$ Resources:Tokens,Amount %>">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes %>"/>
                             <asp:BoundField DataField="RejectReason" HeaderText="<%$Resources:Tokens,RejectReason %>"/>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,RequestStatus %>">
                            <ItemTemplate>
                              <asp:Label runat="server" ID="lblStatus" Text='<%#Eval("IsApproved") %>' ClientIDMode="Static"></asp:Label>  
                            </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate><%=Tokens.NoResults %></EmptyDataTemplate>
                    </asp:GridView>
                </div>
                                    <asp:Button ID="BtnExport" runat="server" Visible="False" Text="<%$ Resources:Tokens,Export %>"
                        Width="100px" onclick="BtnExport_Click" CssClass="btn btn-primary"/>
            </div>
        </div>
    </div>
 
        <script type="text/javascript">
            
            $(document).ready(function () {
                $(".chosen-select").chosen();
            });

            $(function () {
                $('input[data-select="dp"]').datepicker({ dateFormat: 'dd/mm/yy' });
            });
        </script>
</asp:Content>


