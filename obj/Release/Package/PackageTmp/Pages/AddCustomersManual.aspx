<%@ Page Title="<%$ Resources:Tokens,AddCustomersManual %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="AddCustomersManual.aspx.cs" Inherits="NewIspNL.Pages.AddCustomersManual" %>


<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--    <div runat="server" id="Msgsuccess" Visible="False" class="alert alert-success"><%=Tokens.Saved %></div>--%>
    <div class="col-md-12">
        <div id="tabs">
            <ul>
                <li><a href="#tabs-1">
                    <asp:Literal runat="server" ID="lbltitle" Text="<%$Resources:Tokens,AddCustomersManual %>"></asp:Literal>
                    </a>
                </li>
                <li><a href="#tabs-2">
                    <asp:Label runat="server" Text="<%$Resources:Tokens,UpdateCustomerManaul %>" ID="lblheader"></asp:Label></a>
                </li>
            </ul>
            <div id="tabs-1">
                <div class="col-xs-12 col-sm-7" style="margin: 0 22%;">
                    <div class="widget-box">
                        <div class="widget-header">
                            <h4>
                                <%=Tokens.AddCustomersManual%></h4>
                            <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up">
                            </i></a><a href="#" data-action="reload"><i class="icon-refresh"></i></a><a href="#"
                                data-action="close"><i class="icon-remove"></i></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-main no-padding">
                                <div>
                                    <!-- <legend>Form</legend> -->
                                    <fieldset>
                                        <asp:FileUpload runat="server" multiple="" ID="fu_sheet" />
                                        <asp:RequiredFieldValidator runat="server" ID="r_sheet" ValidationGroup="fi" ControlToValidate="fu_sheet"
                                            ErrorMessage="<%$ Resources:Tokens,Required %>" Display="Dynamic"></asp:RequiredFieldValidator>
                                       
                                        
                                          <asp:RegularExpressionValidator runat="server" ForeColor="red" ValidationGroup="fi"  ValidationExpression="^.*\.(xls|XLS|xlsx|XLSX|)$" ControlToValidate="fu_sheet"  ErrorMessage="<%$Resources:Tokens,uploadfilexcelerror %>"></asp:RegularExpressionValidator>
             
                                          </fieldset>
                                    <div class="form-actions center">
                                        <%--<asp:Button runat="server" ID="bSave" CssClass="btn btn-success"  OnClick="btnAdd_Click" Text="<%$Resources:Tokens,Save%>"/><i class="icon-ok bigger-110">&nbsp;</i>
                                                                 <button type="submit" runat="server" id="bSave" class="btn btn-primary" onserverclick="btnAdd_Click"><i class="icon-ok bigger-110"></i>&nbsp;<%=Tokens.Save %></button>--%>
                                        <asp:LinkButton runat="server" ID="bSave" class="btn btn-primary" ValidationGroup="fi"
                                            OnClick="btnAdd_Click"><i class="icon-ok bigger-110"></i>&nbsp;<%=Tokens.Save %></asp:LinkButton>
                                        <a href="../ExcelTemplates/Workorders.xls" class="btn btn-default"><i class="icon-cloud-download bigger-120">
                                        </i>
                                            <%=Tokens.Downloadsample %>
                                        </a>
                                        <asp:Label runat="server" ID="l_message" Text=""></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div>
                    <h3 class="header smaller lighter blue">
                        <asp:Literal runat="server" ID="lblnotcomplete" Text="<%$Resources:Tokens,CustomersNotComplete %>"></asp:Literal></h3>
                    <asp:GridView runat="server" ID="GvUnComplete" AutoGenerateColumns="False" OnDataBound="GvUnp_OnDataBound"
                        CssClass="table table-bordered table-condensed text-center">
                        <Columns>
                            <asp:TemplateField HeaderText="#">
                                <ItemTemplate>
                                    <asp:Label ID="LNo" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CustomerName" HeaderText="<%$Resources:Tokens,Customer%>" />
                            <asp:BoundField DataField="CustomerPhone" HeaderText="<%$Resources:Tokens,Phone%>" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div id="tabs-2">
                <div class="col-xs-12 col-sm-7" style="margin: 0 22%;">
                    <div class="widget-box">
                        <div class="widget-header">
                            <h4>
                                <%=Tokens.UpdateCustomerManaul%></h4>
                            <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up">
                            </i></a><a href="#" data-action="reload"><i class="icon-refresh"></i></a><a href="#"
                                data-action="close"><i class="icon-remove"></i></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-main no-padding">
                                <div>
                                    <!-- <legend>Form</legend> -->
                                    <fieldset>
                                        <asp:FileUpload runat="server" multiple="" ID="FileUpload1" />
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="fi2"
                                            ControlToValidate="FileUpload1" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                           <asp:RegularExpressionValidator runat="server" ForeColor="red" ValidationGroup="fi2"  ValidationExpression="^.*\.(xls|XLS|xlsx|XLSX|)$" ControlToValidate="FileUpload1"  ErrorMessage="<%$Resources:Tokens,uploadfilexcelerror %>"></asp:RegularExpressionValidator>
             
                                        
                                         </fieldset>
                                    <div class="form-actions center">
                                        <asp:LinkButton runat="server" ID="LinkButton1" class="btn btn-primary" ValidationGroup="fi2"
                                            OnClick="UpdateCustomer"><i class="icon-ok bigger-110"></i>&nbsp;<%=Tokens.Update %></asp:LinkButton>
                                        <a href="../ExcelTemplates/UpdateWorkorders.xls" class="btn btn-default"><i class="icon-cloud-download bigger-120">
                                        </i>
                                            <%=Tokens.Downloadsample %>
                                        </a>
                                        <asp:Label runat="server" ID="lblMsg" Text=""></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div>
                                        <h3 class="header smaller lighter blue">
                        <asp:Literal runat="server" ID="Literal1" Text="<%$Resources:Tokens,CustomersNotUpdated %>"></asp:Literal></h3>
                    <asp:GridView runat="server" ID="GvUpdate" AutoGenerateColumns="False" OnDataBound="GvUpdate_OnDataBound"
                        CssClass="table table-bordered table-condensed text-center">
                        <Columns>
                            <asp:TemplateField HeaderText="#">
                                <ItemTemplate>
                                    <asp:Label ID="LNo" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CustomerName" HeaderText="<%$Resources:Tokens,Customer%>" />
                            <asp:BoundField DataField="CustomerPhone" HeaderText="<%$Resources:Tokens,Phone%>" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
 
    <script type="text/javascript">
        $(document).ready(function () {
            $("#tabs").tabs();
        });
    </script>
</asp:Content>

