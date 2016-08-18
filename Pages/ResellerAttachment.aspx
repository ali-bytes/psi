<%@ Page Title="<%$Resources:Tokens,ResellerAttachment%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ResellerAttachment.aspx.cs" Inherits="NewIspNL.Pages.ResellerAttachment" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div class="view" id="searchPanel">
        <fieldset>
            <div class="page-header"><h1>
                <%= (Tokens.Search + " " + Tokens.ResellerAttachment)%></h1></div>
            <div class="row-fluid" runat="server" ID="Forsearch">
                <div class="col-sm-6">
                    <div class="well">
                        <div>
                            <label for="DdlReseller">
                                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,Reseller %>" runat="server" />
                            </label>
                            <div>
                                <asp:DropDownList ID="DdlReseller" runat="server" EnableViewState="True" />
                                <asp:RequiredFieldValidator ID="rrrr" ErrorMessage="*" ValidationGroup="att" ControlToValidate="DdlReseller"
                                                            runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" Display="Dynamic" ErrorMessage="*" ValidationGroup="attsearch" ControlToValidate="DdlReseller"
                                                            runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" Display="Dynamic" ErrorMessage="*" ControlToValidate="DdlYear" ValidationGroup="attupdate"
                                                            runat="server" />

                            </div>
                        </div>
                    <%--</div>
                </div>
                </div>
            <div class="row-fluid">
                <div class="span6">
                    <div class="well">--%>
                        <div>
                            <label for="DdlYear">
                                <%=Tokens.Year %>
                            </label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlYear" Width="157px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="att" ErrorMessage="*" ControlToValidate="DdlYear"
                                                            runat="server"  />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" Display="Dynamic" ErrorMessage="*" ValidationGroup="attsearch" ControlToValidate="DdlYear"
                                                            runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ErrorMessage="*" Display="Dynamic" ControlToValidate="DdlYear" ValidationGroup="attupdate"
                                                            runat="server" />

                            </div>
                        </div>
                        <div>
                            <label for="DdlMonth">
                                <%=Tokens.Month %></label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlMonth" Width="157px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ValidationGroup="att" ID="RequiredFieldValidator2" ErrorMessage="*" ControlToValidate="DdlMonth"
                                                            runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" Display="Dynamic" ErrorMessage="*" ValidationGroup="attsearch" ControlToValidate="DdlMonth"
                                                            runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ErrorMessage="*" Display="Dynamic" ControlToValidate="DdlMonth" ValidationGroup="attupdate"
                                                            runat="server" />

                            </div>
                        </div>
                    </div>
                </div>

            </div>
            <div class="row-fluid">
                        <div class="col-sm-6">
                            <div class="well" style="height: 208px">
                                <div>
                                          <label for="TbUploadBeforRview">
                                              <%=Tokens.InvoiceBeforeReview %></label>
                                          <div>
                                              <asp:FileUpload runat="server" ID="TbUploadBeforRview" ClientIDMode="Static"></asp:FileUpload>
                                              <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="*" Display="Dynamic" ControlToValidate="TbUploadBeforRview" ValidationGroup="att"
                                                                          runat="server" />
                                              <asp:RegularExpressionValidator runat="server" ValidationGroup="att" ForeColor="red"  ValidationExpression="^.*\.(xls|XLS|xlsx|XLSX|)$" ControlToValidate="TbUploadBeforRview"  ErrorMessage="<%$Resources:Tokens,uploadfilexcelerror %>"></asp:RegularExpressionValidator>
            
                                              <a id="lnkfilebeforreview" Visible="False" runat="server" target="_blank">تحميل فاتورة قبل المراجعة</a>
                                          </div>
                                      </div>
                                <div>
                                    <label for="TbUploadAfterRview">
                                        <%=Tokens.InvoiceAfterReview %>
                                    </label>
                                    <div>
                                        <asp:FileUpload runat="server" ID="TbUploadAfterRview" ClientIDMode="Static"></asp:FileUpload>
                                                                                      <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Display="Dynamic" ErrorMessage="*" ControlToValidate="TbUploadAfterRview" ValidationGroup="attupdate"
                                                                          runat="server" />
                                        <asp:RegularExpressionValidator runat="server" ValidationGroup="att" ForeColor="red"  ValidationExpression="^.*\.(xls|XLS|xlsx|XLSX|)$" ControlToValidate="TbUploadAfterRview"  ErrorMessage="<%$Resources:Tokens,uploadfilexcelerror %>"></asp:RegularExpressionValidator>
            
                                         <a id="lnkfileafterReview" Visible="False" runat="server" target="_blank">تحميل فاتورة بعد المراجعة</a>
                                    </div>
                                </div>
                                
                            </div>
                        </div>

            </div>
            <p>
                <asp:Button ID="BSearch" Text="<%$Resources:Tokens,Search %>" ValidationGroup="attsearch" CssClass="btn btn-success" runat="server"
                            OnClick="SearchDemands" />&nbsp;
                <asp:Button class="btn btn-primary" runat="server" ID="btnAdd" ValidationGroup="att"
                    Text="<%$Resources:Tokens,Add %>"  onclick="btnAdd_Click"/>
                &nbsp;
                                <asp:Button class="btn btn-danger" Visible="False" ValidationGroup="attupdate" runat="server" ID="btnupdate" 
                    Text="<%$Resources:Tokens,Edit %>" onclick="btnUpdate_Click"/>
            <span runat="server" ID="Msg"></span>
            </p>
           
        </fieldset>
    </div>
    <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <asp:Literal ID="Literal2" Text="<%$Resources:Tokens,Results %>" runat="server" /></h3>
           
            <div>
                <asp:GridView runat="server" ID="GvResults" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed text-center">
                    <Columns>
                        <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,Reseller %>" />
                        <asp:BoundField DataField="FileNameBefor" HeaderText="<%$Resources:Tokens,InvoiceBeforeReview %>" />
                        <asp:BoundField DataField="FileNameAfter" HeaderText="<%$Resources:Tokens,InvoiceAfterReview %>" />
                        <%--<asp:TemplateField>
                            <ItemTemplate>
                                <a id="A1" runat="server" class="btn btn-success" href='<%#Eval("Url") %>' target="_blank">
                                    <%= Tokens.Download %></a>
                                &nbsp;
                                <asp:Button runat="server" ID="btnUpdate" OnClick="btnUpdate_Click"  CommandArgument="<%#Eval("ID") %>" CssClass="btn btn-primary" Text="<%$Resources:Tokens,Edit %>"/>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                    <EmptyDataTemplate><%=Tokens.NoResults%></EmptyDataTemplate>
                </asp:GridView>
            </div>
           <%-- <div>
                <div class="span3">
                    <asp:GridView ShowHeader="False" runat="server" ID="GvReport"  CssClass="table table-bordered table-hover table-condensed table-striped white"/>
                </div>
                <div class="span9">
                    <asp:Button Text="<%$Resources:Tokens,CreateInvoice %>" ID="btnCreatInvoice" CssClass="btn btn-primary" runat="server" OnClick="CreateInvoice"/>&nbsp;<button id="Button1" class="btn btn-success"  type="button" runat="server" OnServerClick="SearchAgain"><%=Tokens.BackToSearch %></button>
                </div>
            </div>--%>
            
        </fieldset>
    </div>
    <asp:HiddenField runat="server" ID="HfSerched" />

    <script type="text/javascript">
        $(document).ready(function () {
            //$(function () {
            var isSearched = $('#HfSerched').val();
            var searchBtn = $('#searchPanel');
            var resultsPanel = $('#resultPanel');
            if (isSearched === "1") {
                $(searchBtn).hide();
                $(resultsPanel).show();
            } else {
                $(searchBtn).show();
                $(resultsPanel).hide();
            }
        });
    </script>
</asp:Content>


