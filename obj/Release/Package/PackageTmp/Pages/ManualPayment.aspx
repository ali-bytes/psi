<%@ Page Title="<%$ Resources:Tokens,ManualPayment %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ManualPayment.aspx.cs" Inherits="NewIspNL.Pages.ManualPayment" %>

<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <fieldset>

            <div class="col-xs-12 col-sm-7" style="margin: 0 21%;">
                <div class="widget-box">
                    <div class="widget-header">
                        <h4><%=Tokens.LoadSheet%></h4>
                        <span class="widget-toolbar">
                            <a href="#" data-action="collapse">
                                <i class="icon-chevron-up"></i>
                            </a>
                            <a href="#" data-action="reload">
                                <i class="icon-refresh"></i>
                            </a>
                            <a href="#" data-action="close">
                                <i class="icon-remove"></i>
                            </a>
                        </span>
                    </div>

                    <div class="widget-body">
                        <div class="widget-main no-padding">
                            <div>
                                <!-- <legend>Form</legend> -->

                                <fieldset>
                                    <asp:FileUpload runat="server" multiple="" ID="fu_sheet" />
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="pay2"
                                        ControlToValidate="fu_sheet" ErrorMessage="<%$ Resources:Tokens,Required %>" Display="Dynamic"></asp:RequiredFieldValidator>

                                    <asp:RegularExpressionValidator runat="server" ForeColor="red" ValidationGroup="pay2" ValidationExpression="^.*\.(xls|XLS|xlsx|XLSX|)$" ControlToValidate="fu_sheet" ErrorMessage="<%$Resources:Tokens,uploadfilexcelerror %>"></asp:RegularExpressionValidator>

                                </fieldset>
                                 <div class="form-actions center">
                                      <label for="ddlSaves">
                                    <asp:Literal runat="server" Text="<%$Resources:Tokens,sav %>"></asp:Literal>
                                </label>
                                <div>
                                    <asp:DropDownList runat="server" ID="ddlSaves" DataTextField="SaveName" DataValueField="Id" ClientIDMode="Static" Width="178px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlSaves" ValidationGroup="pay2"
                                        ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                                </div>
                                 </div>
                                <div class="form-actions center">

                                    <asp:LinkButton Style="width: 160px" runat="server" ValidationGroup="pay2" ID="bSave" CssClass="btn btn-primary" OnClick="bSave_Click"><i class="icon-save bigger-110"></i>&nbsp;<%=Tokens.Save %></asp:LinkButton>
                                   
                                </div>
                                 <div class="form-actions center">
                                      <a href="../ExcelTemplates/Manual Payment.xls" class="btn btn-default">
                                        <i class="icon-cloud-download bigger-120"></i>
                                        <%=Tokens.Downloadsample %>
                                    </a>
                                    <asp:Label runat="server" ID="l_message" Text=""></asp:Label>
                                 </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <hr />
            <div>
                <asp:GridView runat="server" ID="gv_errors" OnDataBound="gv_errors_DataBound"
                    AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE"
                    BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black"
                    GridLines="Vertical" Width="248px">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="gv_l_Number" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="error" HeaderText="<%$ Resources:Tokens,Error %>" />
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" />
                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <RowStyle BackColor="#F7F7DE" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#FBFBF2" />
                    <SortedAscendingHeaderStyle BackColor="#848384" />
                    <SortedDescendingCellStyle BackColor="#EAEAD3" />
                    <SortedDescendingHeaderStyle BackColor="#575357" />
                </asp:GridView>
            </div>
        </fieldset>
    </div>
  <%--  <div id="SavesModal" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <!-- -->
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                        ×</button>
                    <h4 id="myModalLabel" class="modal-title">
                        <%=Tokens.ChooseSave %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <div class="bootbox-form">
                            <div>
                                <label for="ddlSaves">
                                    <asp:Literal runat="server" Text="<%$Resources:Tokens,sav %>"></asp:Literal>
                                </label>
                                <div>
                                    <asp:DropDownList runat="server" ID="ddlSaves" DataTextField="SaveName" DataValueField="Id" ClientIDMode="Static" Width="178px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlSaves"
                                        ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton Style="width: 160px" runat="server" ValidationGroup="pay2" ID="LinkButton1" CssClass="btn btn-primary" OnClick="bSave_Click"><i class="icon-save bigger-110"></i>&nbsp;<%=Tokens.Save %></asp:LinkButton>
                    <button class="btn btn-default" data-dismiss="modal" aria-hidden="true">
                        <asp:Literal runat="server" Text="<%$Resources:Tokens,Cancel %>"></asp:Literal></button>
                </div>
                <asp:HiddenField runat="server" ID="hf_Woid" ClientIDMode="Static" />
            </div>
        </div>
    </div>
     <script type="text/javascript">--%>
       <%--  $(document).ready(function() {
             $('#btnChangeOffer').click(function() {

                 $('#SavesModal').modal('show');
             });
         });
     </script>--%>
</asp:Content>


