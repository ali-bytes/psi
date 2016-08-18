<%@ Page Title="<%$Resources:Tokens,UpdatedResellerBS%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="UpdatedResellerBS.aspx.cs" Inherits="NewIspNL.Pages.UpdatedResellerBS" %>
<%@Import namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%--     <div runat="server" id="l_message" class="alert" visible="False">
    </div>
    <div class="view" runat="server" id="searchPanel">
        <fieldset>
            <legend>
                <%= Tokens.UpdatedResellerBS%></legend>
        </fieldset>
        <div class="well">
            <div>
                <asp:Label AssociatedControlID="ddl_reseller" runat="server" Text="<%$Resources:Tokens,Reseller %>"
                           ID="labelReseller"></asp:Label></div>
            <div>
                <asp:DropDownList runat="server" ID="ddl_reseller">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddl_reseller"
                                            ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
            </div>
            <p>
                <asp:Button CssClass="btn btn-success" runat="server" ID="btnsearch" 
                            Text="<%$Resources:Tokens,Search %>" onclick="btnsearch_Click"/>
            </p>
        </div>

    </div>
    <div runat="server" id="data" Visible="False">
    <div class="view" >
        <fieldset>
            <legend>
                <%= Tokens.UpdatedResellerBS%></legend>
            <div>
                <asp:GridView runat="server" DataKeyNames="ID" ID="GV_BalanceSheet" AutoGenerateColumns="False" CssClass="table table-bordered table-responsive"
                              OnDataBound="GV_BalanceSheet_DataBound" OnRowCancelingEdit="GV_RowCancelingEdit" OnRowEditing="GV_RowEditing"
                              OnRowUpdating="GV_RowUpdating">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="gv_lNumber" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,Invoice %>">
                            <EditItemTemplate>
                                <asp:TextBox ID="TbInvoice" runat="server" Text='<%# Bind("Invoice") %>' Width="80px"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblInvoice" runat="server" Text='<%# Bind("Invoice") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,BeforReview %>">
                            <EditItemTemplate>
                                <asp:TextBox ID="TbBeforeReview" runat="server" Text='<%# Bind("InvoiceBeforeReview") %>' Width="80px"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBeforeReview" runat="server" Text='<%# Bind("InvoiceBeforeReview") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,AfterReview %>">
                            <EditItemTemplate>
                                <asp:TextBox ID="TbAfterReview" runat="server" Text='<%# Bind("InvoiceAfterReview") %>' Width="80px"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAfterReview" runat="server" Text='<%# Bind("InvoiceAfterReview") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,User %>">
                            <EditItemTemplate>
                                <asp:Label ID="lbleditUser" runat="server" Text='<%# Bind("User") %>' Width="80px"></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="LblUser" runat="server" Text='<%# Bind("User") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,Edit %>" ShowHeader="False" Visible="False">
                            <EditItemTemplate>
                                <asp:LinkButton ID="LinkUpdate" runat="server" CssClass="btn btn-success" CommandName="Update" Text="<%$Resources:Tokens,Update%>"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="LinkCancel" runat="server" CssClass="btn btn-danger" CausesValidation="False" CommandName="Cancel"
                                                      Text="<%$Resources:Tokens,Cancel%>"></asp:LinkButton>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="Linkedit" CssClass="btn btn-success" runat="server" CausesValidation="False" CommandName="Edit"
                                                Text="<%$Resources:Tokens,Edit%>"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate><%=Tokens.NoResults %></EmptyDataTemplate>
                </asp:GridView>
                <asp:HiddenField ID="hf_user" runat="server" />
                <table>
                    <tr>
                        <td style="width: 110px;">
                            <asp:Label runat="server" ID="Label2" Text="<%$Resources:Tokens,TotalBeforeReview %>"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td style="width: 150px;">
                            <asp:Label runat="server" ID="lblTotalBeforeReview" Text=""></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="Label1" Text="<%$Resources:Tokens,TotalAfterReview %>"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td style="width: 150px;">
                            <asp:Label runat="server" ID="lblTotalAfterReview" Text=""></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="Label4" Text="<%$Resources:Tokens,Credit %>"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblTotalCredit" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <hr />
                <div class="well" runat="server" id="divAmount" Visible="False">
                    <label for="txtmonthyear"><%=Tokens.MonthYearInvoice %></label>
                    <div>
                        <asp:TextBox runat="server" ID="txtmonthyear"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtmonthyear" ValidationGroup="balance" ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                    <label for="txtAmountBeforeReview"><%=Tokens.AmountBeforeReview %></label>
                    <div>
                        <asp:TextBox runat="server" ID="txtAmountBeforeReview"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="txtAmountBeforeReview" ValidationGroup="balance" ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                    <label for="txtAmountAfterReview"><%=Tokens.AmountAfterReview %></label>
                    <div>
                        <asp:TextBox runat="server" ID="txtAmountAfterReview"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="txtAmountAfterReview" ValidationGroup="balance" ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                    <p>
                        <asp:Button runat="server" ID="b_save" Text="<%$Resources:Tokens,Save %>" 
                                    ValidationGroup="balance" CssClass="btn btn-primary" onclick="b_save_Click"/>
                    </p>
                </div>

            </div>
        </fieldset>
    </div>
        <div class="view">
        <fieldset>
            <legend>
                <%= Tokens.ResellerPayments%></legend>
            <div>
                <asp:GridView runat="server" ID="gv_ResellerPayment" AutoGenerateColumns="False" CssClass="table table-bordered table-responsive"
                              OnDataBound="gv_ResellerPayment_DataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,Paid %>">
                            <ItemTemplate>
                                <asp:Label ID="lblTotal" runat="server" Text='<%# Bind("Total") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,Notes %>">
                            <ItemTemplate>
                                <asp:Label ID="lblBeforeReview" runat="server" Text='<%# Bind("Notes") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="<%$Resources:Tokens,Print %>" Visible="False">
                            <ItemTemplate>
                                <a href='<%#Eval("Id","UpdatedResellerReceipt.aspx?id={0}")%>' target="_blank" class="btn btn-success"><%=Tokens.Print %></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="BDel" CssClass="btn btn-danger" runat="server" OnClick="BDel_OnClick" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm ('تاكيد حذف المدفوع؟');" Text="<%$Resources:Tokens,Delete%>"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                    <EmptyDataTemplate><%=Tokens.NoResults %></EmptyDataTemplate>
                </asp:GridView>
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label3" Text="<%$Resources:Tokens,Paid %>"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblTotalPayment" Text=""></asp:Label>
                        </td>
                        <td>
                    </tr>
                </table>
                <hr />
                <div class="well" runat="server" id="divpay" Visible="False">
                    <label for="txtAmountPayment"><%=Tokens.Amount %></label>
                    <div>
                        <asp:TextBox runat="server" ID="txtAmountPayment"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="rq" ControlToValidate="txtAmountPayment" ValidationGroup="payment"
                         ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                    <label for="ddlSaves"><%=Tokens.Saves %></label>
                    <div>
                        <asp:DropDownList runat="server" ID="ddlSaves" DataTextField="SaveName"
                         DataValueField="Id" ValidationGroup="payment"></asp:DropDownList>
                         <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ControlToValidate="ddlSaves"
                          ValidationGroup="payment" ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                    <label for="txtNotes"><%=Tokens.Notes %></label>
                    <div>
                        <asp:TextBox runat="server" TextMode="MultiLine" ID="txtNotes"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtNotes" ValidationGroup="payment" ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                    <p>
                        <asp:Button runat="server" ID="btnPayment" Text="<%$Resources:Tokens,Payment %>" 
                                    ValidationGroup="payment" CssClass="btn btn-primary" onclick="btnPayment_Click"/>
                    </p>
                </div>

            </div>
            <p>
                <asp:Button runat="server" ID="btnExport" OnClick="btnExport_click" CssClass="btn btn-success" Text="<%$Resources:Tokens,Export %>"/>
            </p>
        </fieldset>
    </div>
</div>--%>



     <div runat="server" id="l_message" class="alert" visible="False">
    </div>
    <div class="view" runat="server" id="searchPanel">
        <fieldset>
            <div class="page-header"><h1>
                <%= Tokens.UpdatedResellerBS%></h1></div>
        </fieldset>
        <div class="well">
            <div>
                <asp:Label AssociatedControlID="ddl_reseller" runat="server" Text="<%$Resources:Tokens,Reseller %>"
                           ID="labelReseller"></asp:Label></div>
            <div>
                <asp:DropDownList runat="server" ID="ddl_reseller">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddl_reseller"
                                            ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
            </div>
            <p>
                <br/>
                <asp:Button CssClass="btn btn-success" runat="server" ID="btnsearch" 
                            Text="<%$Resources:Tokens,Search %>" onclick="btnsearch_Click"/>
            </p>
        </div>

    </div>
    <div runat="server" id="data" Visible="False">
    <div class="view" >
        <fieldset>
            <h3 class="header smaller lighter blue">
                <%= Tokens.UpdatedResellerBS%></h3>
            <div>
                <asp:GridView runat="server" DataKeyNames="ID" ID="GV_BalanceSheet" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed text-center"
                              OnDataBound="GV_BalanceSheet_DataBound" OnRowCancelingEdit="GV_RowCancelingEdit" OnRowEditing="GV_RowEditing"
                              OnRowUpdating="GV_RowUpdating">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="gv_lNumber" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,Invoice %>">
                            <EditItemTemplate>
                                <asp:TextBox ID="TbInvoice" runat="server" Text='<%# Bind("Invoice") %>' Width="80px"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblInvoice" runat="server" Text='<%# Bind("Invoice") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,BeforReview %>">
                            <EditItemTemplate>
                                <asp:TextBox ID="TbBeforeReview" runat="server" Text='<%# Bind("InvoiceBeforeReview") %>' Width="80px"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBeforeReview" runat="server" Text='<%# Bind("InvoiceBeforeReview") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,AfterReview %>">
                            <EditItemTemplate>
                                <asp:TextBox ID="TbAfterReview" runat="server" Text='<%# Bind("InvoiceAfterReview") %>' Width="80px"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAfterReview" runat="server" Text='<%# Bind("InvoiceAfterReview") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,User %>">
                            <EditItemTemplate>
                                <asp:Label ID="lbleditUser" runat="server" Text='<%# Bind("User") %>' Width="80px"></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="LblUser" runat="server" Text='<%# Bind("User") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,Edit %>" ShowHeader="False" Visible="False">
                            <EditItemTemplate>
                                <asp:LinkButton ID="LinkUpdate" runat="server" CssClass="btn btn-primary btn-sm" CommandName="Update" ToolTip="<%$Resources:Tokens,Update%>"
                                data-rel="tooltip"><i class="icon-ok icon-only bigger-125"></i></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="LinkCancel" runat="server" CssClass="btn btn-danger btn-sm" CausesValidation="False" CommandName="Cancel"
                                                    data-rel="tooltip" ToolTip="<%$Resources:Tokens,Cancel%>"><i class="icon-remove icon-only bigger-125"></i></asp:LinkButton>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="Linkedit" CssClass="btn btn-success btn-xs" runat="server" CausesValidation="False" CommandName="Edit"
                                          data-rel="tooltip" ToolTip="<%$Resources:Tokens,Edit%>"><i class="icon-edit icon-only bigger-125"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate><%=Tokens.NoResults %></EmptyDataTemplate>
                </asp:GridView>
                <asp:HiddenField ID="hf_user" runat="server" />
                <table>
                    <tr>
                        <td style="width: 110px;">
                            <asp:Label runat="server" ID="Label2" Text="<%$Resources:Tokens,TotalBeforeReview %>"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td style="width: 150px;">
                            <asp:Label runat="server" ID="lblTotalBeforeReview" Text=""></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="Label1" Text="<%$Resources:Tokens,TotalAfterReview %>"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td style="width: 150px;">
                            <asp:Label runat="server" ID="lblTotalAfterReview" Text=""></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="Label4" Text="<%$Resources:Tokens,Credit %>"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblTotalCredit" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <hr />
                <div class="well" runat="server" id="divAmount" Visible="False">
                    <label for="txtmonthyear"><%=Tokens.MonthYearInvoice %></label>
                    <div>
                        <asp:TextBox runat="server" ID="txtmonthyear"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtmonthyear" ValidationGroup="balance" ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                    <label for="txtAmountBeforeReview"><%=Tokens.AmountBeforeReview %></label>
                    <div>
                        <asp:TextBox runat="server" ID="txtAmountBeforeReview"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="txtAmountBeforeReview" ValidationGroup="balance" ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                    <label for="txtAmountAfterReview"><%=Tokens.AmountAfterReview %></label>
                    <div>
                        <asp:TextBox runat="server" ID="txtAmountAfterReview"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="txtAmountAfterReview" ValidationGroup="balance" ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                    <p><br/>
                        <asp:Button runat="server" ID="b_save" Text="<%$Resources:Tokens,Save %>" 
                                    ValidationGroup="balance" CssClass="btn btn-primary" onclick="b_save_Click"/>
                    </p>
                </div>

            </div>
        </fieldset>
    </div>
        <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <%= Tokens.ResellerPayments%></h3>
            <div>
                <asp:GridView runat="server" ID="gv_ResellerPayment" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed text-center"
                              OnDataBound="gv_ResellerPayment_DataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,Paid %>">
                            <ItemTemplate>
                                <asp:Label ID="lblTotal" runat="server" Text='<%# Bind("Total") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,Notes %>">
                            <ItemTemplate>
                                <asp:Label ID="lblBeforeReview" runat="server" Text='<%# Bind("Notes") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="<%$Resources:Tokens,Print %>" Visible="False">
                            <ItemTemplate>
                                <a href='<%#Eval("Id","UpdatedResellerReceipt.aspx?id={0}")%>' target="_blank" class="btn btn-primary btn-xs" title="<%=Tokens.Print %>"><i class="icon-print icon-only bigger-125"></i></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="BDel" CssClass="btn btn-danger btn-xs" runat="server" OnClick="BDel_OnClick" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm ('تاكيد حذف المدفوع؟');" 
                                ToolTip="<%$Resources:Tokens,Delete%>" data-rel="tooltip"><i class="icon-trash icon-only bigger-125"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                    <EmptyDataTemplate><%=Tokens.NoResults %></EmptyDataTemplate>
                </asp:GridView>
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label3" Text="<%$Resources:Tokens,Paid %>"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblTotalPayment" Text=""></asp:Label>
                        </td>
                        <td>
                    </tr>
                </table>
                <hr />
                <div class="well" runat="server" id="divpay" Visible="False">
                    <label for="txtAmountPayment"><%=Tokens.Amount %></label>
                    <div>
                        <asp:TextBox runat="server" ID="txtAmountPayment"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="rq" ControlToValidate="txtAmountPayment" ValidationGroup="payment"
                         ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                    <label for="ddlSaves"><%=Tokens.Saves %></label>
                    <div>
                        <asp:DropDownList runat="server" ID="ddlSaves" DataTextField="SaveName"
                         DataValueField="Id" ValidationGroup="payment"></asp:DropDownList>
                         <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ControlToValidate="ddlSaves"
                          ValidationGroup="payment" ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                    <label for="txtNotes"><%=Tokens.Notes %></label>
                    <div>
                        <asp:TextBox runat="server" TextMode="MultiLine" ID="txtNotes"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtNotes" ValidationGroup="payment" ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                    <p>
                        <br/>
                        <asp:Button runat="server" ID="btnPayment" Text="<%$Resources:Tokens,Payment %>" 
                                    ValidationGroup="payment" CssClass="btn btn-primary" onclick="btnPayment_Click"/>
                    </p>
                </div>

            </div>
            <p>
                <asp:Button runat="server" ID="btnExport" OnClick="btnExport_click" CssClass="btn btn-success" Text="<%$Resources:Tokens,Export %>"/>
            </p>
        </fieldset>
    </div>
</div>
</asp:Content>

