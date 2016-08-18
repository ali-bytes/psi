<%@ Page Title="<%$ Resources:Tokens,ServicPackages %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ServicePackages.aspx.cs" Inherits="NewIspNL.Pages.ServicePackages" %>


<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="v_index" runat="server">
                <div id="message">
                    <asp:Label runat="server" ID="l_message"></asp:Label></div>
                <div class="view">
                    <asp:Panel runat="server" ID="p_index">
                        <div class="page-header"><h1><asp:Literal runat="server" ID="lblindex" Text="<%$ Resources:Tokens,Index %>"></asp:Literal></h1></div>
                        <div>
                            <p>
                                <asp:Button ID="b_new" runat="server" Text="<%$ Resources:Tokens,New %>" Width="90px"
                                    OnClick="b_new_Click" CssClass="btn btn-primary"/></p>
                            <asp:GridView ID="gv_index" runat="server" CellPadding="4" GridLines="Horizontal"
                                AutoGenerateColumns="False" OnDataBound="gv_index_DataBound" Width="100%" ClientIDMode="Static"
                                CssClass="table table-bordered table-condensed text-center">
                                <Columns>
                                    <asp:TemplateField HeaderText="#">
                                        <ItemTemplate>
                                            <asp:Label ID="l_number" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Name" HeaderText="<%$Resources:Tokens,Package %>" />
                                    <asp:BoundField DataField="Type" HeaderText="<%$Resources:Tokens,PackageType %>" />
                                    <asp:BoundField DataField="Provider" HeaderText="<%$Resources:Tokens,Provider %>" />
                                    <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes %>" />
                                    <asp:BoundField DataField="TPrice" HeaderText="<%$Resources:Tokens,Price %>" />
                                    <asp:BoundField DataField="Active" HeaderText="<%$Resources:Tokens,Active %>" />
                                    <asp:BoundField DataField="PurchasePrice" HeaderText="<%$Resources:Tokens,PurchasePrice %>" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                                        <asp:LinkButton ID="gvb_edit" runat="server" CommandArgument='<%# Bind("ID") %>' OnClick="gvb_edit_Click"
                                                            ToolTip="<%$ Resources:Tokens,Edit %>" data-rel="tooltip"><i class="icon-pencil icon-only bigger-130"></i></asp:LinkButton>
                                                            &nbsp;
                                                        <asp:LinkButton ID="gvb_delete" Visible='<%#Eval("CanDelete") %>' runat="server" CommandArgument='<%# Bind("ID") %>' data-rel="tooltip"
                                                            ToolTip="<%$ Resources:Tokens,Delete%>" OnClick="gvb_delete_Click"><i class="icon-trash icon-only bigger-130 red"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                </div>
            </asp:View>
            <asp:View ID="v_AddEdit" runat="server">
                <div class="view">
                    <asp:Panel runat="server" ID="p_add">
                        <div class="page-header"><h1><asp:Literal runat="server" ID="Literal1" Text="<%$ Resources:Tokens,Service.Package %>"></asp:Literal></h1></div>
                        <asp:HiddenField ID="hf_id" runat="server" />
                        <div class="well">
                            <div>
                                <label>
                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Tokens,Service.Package %>"></asp:Label>
                                </label>
                                <div>
                                    <asp:TextBox runat="server" ID="TbName"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="r_offer" ControlToValidate="TbName"
                                        ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div>
                                <label>
                                    <asp:Label ID="LPrice" runat="server" Text="<%$ Resources:Tokens,Price %>"></asp:Label>
                                </label>
                                <div>
                                    <asp:TextBox runat="server" ID="TbPrice" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="RfPrice" ControlToValidate="TbPrice"
                                        ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                                    <span id="priceMsg"></span>
                                </div>
                            </div>
                            <div>
                                <label>
                                    <asp:Label ID="LPurchasePrice" runat="server" Text="<%$ Resources:Tokens,PurchasePrice %>"></asp:Label>
                                </label>
                                <div>
                                    <asp:TextBox runat="server" ID="TbPurchasePrice" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="RfPurchasePrice" ControlToValidate="TbPurchasePrice"
                                        ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                                    <span id="priceMsg2"></span>
                                </div>
                            </div>
                            <div>
                                <label>
                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Tokens,Provider %>" AssociatedControlID="DdProviders"></asp:Label>
                                </label>
                                <div>
                                    <asp:DropDownList runat="server" ID="DdProviders" ClientIDMode="Static" Width="178px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="DdProviders"
                                        ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div>
                                <label>
                                    <asp:Label ID="LDdTypes" runat="server" Text="<%$ Resources:Tokens,PackageType %>"
                                        AssociatedControlID="DdTypes"></asp:Label>
                                </label>
                                <div>
                                    <asp:DropDownList runat="server" ID="DdTypes" ClientIDMode="Static" Width="178px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="RfDdTypes" ControlToValidate="DdTypes"
                                        ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div>
                                <label>
                                    <asp:Label ID="LNotes" AssociatedControlID="TbNotes" runat="server" Text="<%$ Resources:Tokens,Notes %>"></asp:Label>
                                </label>
                                <div>
                                    <asp:TextBox runat="server" ID="TbNotes" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div>
                                <asp:CheckBox Text="<%$Resources:Tokens,Active %>" runat="server" ID="CbActive" />
                            </div>
                            <p>
                                <asp:Button runat="server" ID="b_save" Text="<%$ Resources:Tokens,Save %>" OnClick="b_save_Click"
                                     CssClass="btn btn-success"/>
                            </p>
                        </div>
                    </asp:Panel>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>
 
          <%:System.Web.Optimization.Scripts.Render("~/bundles/numberOnly")%>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#gv_index tr").not(':first').hover(function () {
                $(this).css("background-color", "rgb(243, 255, 195)");
            }, function () {
                $(this).css("background-color", "");
            });

            numberOnly($("#TbPrice"), $("#priceMsg"), "<%=Tokens.NumbersOnly %>");
            numberOnlydecimal($("#TbPurchasePrice"), $("#priceMsg2"), "<%=Tokens.NumbersOnly %>");
        });


    </script>
</asp:Content>
