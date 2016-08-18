<%@ Page Title="<%$ Resources:Tokens,SearchOutdoorsPayment %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="SearchOutdoorsPayment.aspx.cs" Inherits="NewIspNL.Pages.SearchOutdoorsPayment" %>

<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view">
        <div id="tr_Search" runat="server">
            <fieldset>
                <div class="page-header">
                    <h1>
                        <asp:Literal ID="Label38" runat="server"
                            Text="<%$ Resources:Tokens,SearchOutdoorsPayment %>"></asp:Literal></h1>
                </div>
                <div class="well">
                    <div class="row">
                        <div class="col-md-3">
                            <div>
                                <label for="TbStartAt">
                                    <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,From %>" runat="server" />
                                </label>
                                <div>
                                    <asp:TextBox runat="server" ID="TbStartAt" data-select="dp" Width="155px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RTbFrom" ValidationGroup="MTS" ErrorMessage="<%$Resources:Tokens,Required %>"
                                        ControlToValidate="TbStartAt" runat="server" />
                                </div>
                            </div>
                            <div>
                                <label for="TbTo">
                                    <asp:Literal ID="Literal2" Text="<%$Resources:Tokens,To %>" runat="server" />
                                </label>
                                <div>
                                    <asp:TextBox runat="server" ID="TbTo" data-select="dp" Width="155px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RTbTo" ValidationGroup="MTS" ErrorMessage="<%$Resources:Tokens,Required %>"
                                        ControlToValidate="TbTo" runat="server" />
                                </div>
                            </div>
                        </div>
                         <div class="col-md-9">
                             <div>
                                <label for="tbCust">
                                    <asp:Literal ID="Literal3" Text="<%$Resources:Tokens,Customer %>" runat="server" />
                                </label>
                                <div>
                                    <asp:TextBox runat="server" ID="tbCust" Width="155px"></asp:TextBox>
                                </div>
                            </div>
                            <div>
                                <label for="ddl_Company">
                                    <asp:Literal ID="Literal4" Text="<%$Resources:Tokens,Company %>" runat="server" />
                                </label>
                                <div>
                                    <asp:DropDownList runat="server" ID="ddl_Company" Width="155px" DataTextField="CompanyName" DataValueField="Id"></asp:DropDownList>
                                </div>
                            </div>
                         </div>
                    </div>
                    <p>
                        <br />
                        <asp:Button ID="btn_search" class="btn btn-success" runat="server" Text="<%$Resources:Tokens ,Search%>" Width="100px" OnClick="btn_search_Click"
                            ValidationGroup="MTS" />
                    </p>

                </div>

            </fieldset>
        </div>
        <div>
            <div id="tb_SearchResult" runat="server" visible="false">
                <div>
                    <asp:GridView OnDataBound="grd_DataBound"
                        CssClass="table table-bordered table-condensed text-center" ID="grd_Transactions" runat="server"
                        AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="Time" HeaderText="<%$ Resources:Tokens,Date %>">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BoxName" HeaderText="<%$ Resources:Tokens,Boxname %>">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BoxAmount" HeaderText="<%$ Resources:Tokens,BoxAmount %>">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="InvoiceAmount" HeaderText="<%$ Resources:Tokens,invoiceAmount %>">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,User %>" />
                            <asp:BoundField DataField="CustomerName" HeaderText="<%$Resources:Tokens,Customer.Name %>" />
                            <asp:BoundField DataField="CustomerTelephone" HeaderText="<%$Resources:Tokens,Customer.Phone %>" />
                            <asp:BoundField DataField="CompanyName" HeaderText="<%$Resources:Tokens,Company %>" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <a href="<%#Eval("ID","RecieptofOutdoorPayment.aspx?Id={0}") %>" target="_blank" data-rel="tooltip"
                                        title="<%=Tokens.Reciept %>"><i class="icon-only icon-file-text bigger-130"></i></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate><%=Tokens.NoResults %></EmptyDataTemplate>
                    </asp:GridView>
                </div>

                <asp:Button ID="BtnExport" runat="server" Visible="False" Text="<%$ Resources:Tokens,Export %>"
                    Width="100px" OnClick="BtnExport_Click" CssClass="btn btn-primary" />

            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(function () {
            $('input[data-select="dp"]').datepicker({ dateFormat: 'dd/mm/yy' });
        });
    </script>
</asp:Content>

