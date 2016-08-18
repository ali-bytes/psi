<%@ Page Title="<%$Resources:Tokens,ResellerPaymenyCreditReport %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ResellerPaymenyCreditReport.aspx.cs" Inherits="NewIspNL.Pages.ResellerPaymenyCreditReport" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="view">
        <fieldset>
            <div class="page-header"><h1>
                <%=Tokens.ResellerPaymenyCreditReport %></h1></div>
            <div class="row-fluid">
                <div class="col-sm-4">
                    <div class="well" style="height: 208px;">
                         <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,Reseller %>"></asp:Literal></label>
                <div class="col-sm-9">
                <div>
                    <div>
                        <asp:DropDownList runat="server" ID="DdlReseller" CssClass="width-70 chosen-select" ClientIDMode="Static" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DdlReseller"
                            ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                </div>
                </div></div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="well" style="height: 208px;">
                        <div>
                            <div>
                                <label for="TbFrom">
                                    <%= Tokens.From %></label>
                                <div>
                                    <asp:TextBox runat="server" ID="TbFrom" data-select="dp" />
                                </div>
                            </div>
                            <div>
                                <label for="TbTo">
                                    <%= Tokens.To %></label>
                                <div>
                                    <asp:TextBox runat="server" ID="TbTo" data-select="dp" />
                                    <asp:CompareValidator ID="CV" ErrorMessage="*" ControlToValidate="TbTo" runat="server"
                                        ControlToCompare="TbFrom" Operator="GreaterThanEqual" Type="Date" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <hr />
                <div class="col-md-12">
                    <button runat="server" onserverclick="Search" type="submit" class="btn btn-success">
                        <i class="icon-search"></i>&nbsp;<%= Tokens.Search %></button>
                </div>
            </div>
        </fieldset>
    </div>
      <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <asp:Literal Text="<%$Resources:Tokens,CreditHistory %>" runat="server" /></h3>
             <table class="alert alert-info">
                    <tr>
                        <td><%=Tokens.Credit %></td>
                        <td>:&nbsp;</td>
                        <td><asp:Label runat="server" ID="lblLastCredit"></asp:Label></td>
                    </tr>
                </table>
            <div>
                <asp:GridView runat="server" ID="GvHistory" CssClass="table table-bordered table-condensed text-center"
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
                        <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,Reseller %>" />
                        <asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,User %>" />
                        <asp:BoundField DataField="Date" HeaderText="<%$Resources:Tokens,Date %>" />
                        <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes %>" />
                        
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,Attachments %>">
                            <ItemTemplate>
                                <a  runat="server" title="<%$Resources:Tokens,Attachments %>" href='<%#Eval("link")%>' target="_blank" data-rel="tooltip" Visible='<%#Eval("ifAttchment")%>'> 
                                    <i class="icon-paper-clip icon-only bigger-130"></i>
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <a title="<%=Tokens.Reciept %>" data-rel="tooltip" href='<%#Eval("RecieptUrl") %>' target="_blank">
                                    <i class="icon-file-text icon-only bigger-130"></i></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div>
                            <%= Tokens.NoResults %></div>
                    </EmptyDataTemplate>
                </asp:GridView>
               <asp:Button CssClass="btn btn-success" CausesValidation="false" runat="server" ID="btnExport" Text="<%$Resources:Tokens,Export %>"
                        OnClick="Export_OnClick" />
            </div>
        </fieldset>
    </div>
      <link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>
    <script type="text/javascript">
        $(document).ready(function() {
            jQuery(".chosen-select").chosen();
        $('input[data-select="dp"]').datepicker({ dateFormat: 'dd/mm/yy' });
        if ($('#GvResults').width() > 1058) {
            $('#GvResults').css({
                "font-size": "81%"
            });
        }
        });
    </script>
</asp:Content>
