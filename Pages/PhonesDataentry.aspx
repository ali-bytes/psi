<%@ Page Title="<%$Resources:Tokens,PhonesDataEntry%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="PhonesDataentry.aspx.cs" Inherits="NewIspNL.Pages.PhonesDataentry" %>


<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view">
        <div id="message" style="background-color: #dff0d8; color: #468847;border-color: #d6e9c6;
            margin: 5px;">
            <asp:Label runat="server" ID="l_message" ClientIDMode="Static"></asp:Label></div>
        <asp:Panel runat="server" ID="p_entery">
            <div class="page-header"><h1><%=Tokens.PhonesDataEntry %></h1></div>
            <div id="upload">
                <div class="well col-md-5">
                
                    <asp:Label runat="server" ID="Label2" Text="<%$Resources:Tokens,Employee%>"></asp:Label>
                <div>
                    <asp:DropDownList runat="server" ID="ddl_eployees" Width="150px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                        ControlToValidate="ddl_eployees"></asp:RequiredFieldValidator>
                </div>
                <div>
                    <asp:Label runat="server" ID="Label1" Text="<%$Resources:Tokens,Phones%>"></asp:Label>
                
                    <asp:FileUpload runat="server" ID="fu_sheet"></asp:FileUpload>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="<%$Resources:Tokens,Required%>"
                        ControlToValidate="fu_sheet"></asp:RequiredFieldValidator>
               
                     <asp:RegularExpressionValidator runat="server" ForeColor="red"   ValidationExpression="^.*\.(xls|XLS|xlsx|XLSX|)$" ControlToValidate="fu_sheet"  ErrorMessage="<%$Resources:Tokens,uploadfilexcelerror %>"></asp:RegularExpressionValidator>
            
                     </div>
                <p>
                    <asp:LinkButton runat="server" CssClass="btn btn-primary"  ID="b_save" OnClick="b_save_Click"
                        Width="160px"><i class="icon-save icon-only"></i>&nbsp;<asp:Literal runat="server" Text="<%$Resources:Tokens,Save%>"></asp:Literal></asp:LinkButton>&nbsp;
                        <a href="../ExcelTemplates/Phones.xls" class="btn btn-default">
                        <i class="icon-cloud-download bigger-120"></i>
                        <%=Tokens.Downloadsample %>
                    </a>
                </p>
                </div>
                <div class="col-md-12">
                    <asp:GridView runat="server" ID="gv_items" 
                    CssClass="table table-bordered table-responsive center"
                         ClientIDMode="Static" OnDataBound="gv_items_DataBound" AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="l_Number" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="<%$Resources:Tokens,Name%>" />
                            <asp:BoundField DataField="Phone1" HeaderText="<%$Resources:Tokens,Phone%>" />
                            <asp:BoundField DataField="Governate" HeaderText="<%$Resources:Tokens,Governrate%>" />
                            <asp:BoundField DataField="State" HeaderText="<%$Resources:Tokens,State%>" />
                            <asp:BoundField DataField="Offer1" HeaderText="<%$Resources:Tokens,Offer1%>" />
                            <asp:BoundField DataField="Offer2" HeaderText="<%$Resources:Tokens,Offer2%>" />
                            <asp:BoundField DataField="Employee" HeaderText="<%$Resources:Tokens,Employee%>" />
                            <asp:BoundField DataField="Central" HeaderText="<%$Resources:Tokens,Central%>" />
                            <asp:BoundField DataField="Mobile" HeaderText="<%$Resources:Tokens,Mobile%>" />
                            <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes%>" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('#l_message').html() === '') {
                $('#message').css("border", "none");
            } else {
                $('#message').css("border", "#d6e9c6 solid 1px").css("padding", "4px")
                    .css("-moz-box-shadow", "0 0 1px #888")
                    .css("-webkit-box-shadow", " 0 0 1px#888")
                    .css("box-shadow", "0 0 1px #888");
            }
            $("#gv_items tr").not(':first').hover(function () {
                $(this).css("background-color", "rgb(243, 255, 195)");
            }, function () {
                $(this).css("background-color", "");
            });
        });
    </script>
</asp:Content>

