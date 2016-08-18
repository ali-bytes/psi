<%@ Page Title="<%$Resources:Tokens,ResellerPackagesDiscounts%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ResellerPackagesDiscounts.aspx.cs" Inherits="NewIspNL.Pages.ResellerPackagesDiscounts" %>


<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view">
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="v_index" runat="server">
                <div id="message">
                    <asp:Label runat="server" ID="l_message"></asp:Label></div>
                <asp:Panel runat="server" ID="p_index">
                    <div class="page-header"><h1><asp:Literal runat="server" Text="<%$Resources:Tokens,ResellerPackagesDiscounts %>"></asp:Literal></h1></div>
                    <div>
                        <div class="well">
                            <div>
                                <label for="DdlReseller">
                                    <%= Tokens.Reseller %></label>
                                <div>
                                    <asp:DropDownList runat="server" CssClass="width-60 chosen" ID="DdlReseller">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="DdlReseller" ID="R1"
                                        runat="server" />
                                </div>
                            </div>
                            <div>
                                <label for="DdlProvider">
                                    <%= Tokens.Provider %></label>
                                <div>
                                    <asp:DropDownList runat="server" CssClass="width-60 chosen" ID="DdlProvider">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="DdlProvider" runat="server"
                                        ID="r2" />
                                </div>
                            </div>
                        </div>
                        <p>
                            <button id="b_new" class="btn btn-success" runat="server" causesvalidation="True"
                                onserverclick="ResellerPacksDiscounts">
                                <i class="icon-white icon-search"></i>&nbsp;<%= Tokens.Search %></button></p>
                        <div>
                            <asp:GridView ID="gv_index" runat="server" CssClass="table table-bordered table-condensed text-center"
                                AutoGenerateColumns="False" OnDataBound="gv_index_DataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div>
                                                <asp:Label ID="l_number" runat="server" />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,Reseller %>" />
                                    <asp:BoundField DataField="Provider" HeaderText="<%$Resources:Tokens,Provider %>" />
                                    <asp:BoundField DataField="Package" HeaderText="<%$Resources:Tokens,Package %>" />
                                    <asp:BoundField DataField="Discount" HeaderText="<%$Resources:Tokens,Discount %>" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%--<span class="btn btn-primary btn-sm" data-select="edit" data-discount='<%#Eval("Discount")%>' data-rel="tooltip" title="<%=Tokens.Edit %>"
                                                data-prov='<%#Eval("ProviderId")%>' data-res='<%#Eval("ResellerId")%>' data-pack='<%#Eval("PackageId")%>'>
                                                
                                                <i class="icon-only icon-edit"></i>
                                            </span>--%>
                                            
                                            <button type="button" class="btn btn-primary btn-sm" data-pack='<%#Eval("PackageId")%>' onclick="show(<%#Eval("PackageId")%>,<%#Eval("Discount")%>)" title="<%=Tokens.Edit %>">
              
                                            <%=Tokens.Edit %>
                                             <i class="icon-only icon-edit"></i>
                                          
                                            </button>
                                            

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>
            </asp:View>
        </asp:MultiView>
    </div>
    <div id="editdiscount" class="bootbox modal fade">
                        <div class="modal-dialog">
            <div class="modal-content">
        <div class="modal-header">
            <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                &times;</button>
            <h4>
                <%= Tokens.Edit %>&nbsp;<%= Tokens.Discount %></h4>
        </div>
        <div class="modal-body">
            <div class="bootbox-body">
            <div class="well">
                <asp:HiddenField runat="server" ID="HfProv" />
                <asp:HiddenField runat="server" ID="HfRes" />
                <asp:HiddenField runat="server" ID="HfPack" />
                <div>
                    <label for="TbDiscount">
                        <asp:Literal Text="<%$Resources:Tokens,Discount %>" runat="server" />
                    </label>
                    <div>
                        <asp:TextBox runat="server" ValidationGroup="ed" ID="TbDiscount" ClientIDMode="Static"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RTbDiscount" ValidationGroup="ed" ErrorMessage="<%$Resources:Tokens,Required %>"
                            ControlToValidate="TbDiscount" runat="server" />
                        <asp:CompareValidator ErrorMessage="*" ValidationGroup="ed" runat="server" ID="CV"
                            ControlToValidate="TbDiscount" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                        <asp:RangeValidator ErrorMessage="*" runat="server" ID="RV" ValidationGroup="ed"
                            ControlToValidate="TbDiscount" MinimumValue="0" MaximumValue="100" Type="Double"></asp:RangeValidator>
                    </div>
                </div>
            </div>
            </div>
        </div>
        <div class="modal-footer">            <button type="button" runat="server" onserverclick="SaveDiscount" validationgroup="ed"
                class="btn btn-primary">
                <i class="icon-white icon-ok"></i>&nbsp;<%= Tokens.Save %></button>&nbsp;
            <span data-dismiss="modal" class="btn btn-default"><i class="icon-white icon-arrow-left">
            </i>&nbsp;<%= Tokens.Cancel %></span>

        </div>
        </div>
        </div>
    </div>
         <link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>

    <script type="text/javascript">
        $(document).ready(function() {
            //$(".chosen-select").chosen();
        });
        //var $editmodal = $('#editdiscount');
        //var $pack = $('#HfPack');
        //var $res = $('#HfRes');
        //var $prov = $('#HfProv');
        //var $discount = $('#TbDiscount');
        //$('span[data-select="edit"]').on('click', function () {
        //    $pack.val($(this).attr('data-pack'));
        //    $res.val($(this).attr('data-res'));
        //    $prov.val($(this).attr('data-prov'));
        //    $discount.val($(this).attr('data-discount'));
        //    document.getElementById('#HfPack').val = $(this).attr('data-pack');
        //    $editmodal.modal('show');
        //});
        


        function show(id,amount) {
            var e = document.getElementById("<%=DdlReseller.ClientID%>");
            var res = e.options[e.selectedIndex].value;
            var e2 = document.getElementById("<%=DdlProvider.ClientID%>");
            var pro = e2.options[e2.selectedIndex].value;
         
            var $editmodal = $('#editdiscount');
          
            document.getElementById("<%=HfPack.ClientID%>").value = id;
            document.getElementById("<%=TbDiscount.ClientID%>").value = amount;
            document.getElementById("<%=HfRes.ClientID%>").value = res;
            document.getElementById("<%=HfProv.ClientID%>").value = pro;
            $editmodal.modal('show');
        
        } ;

        jQuery(document).ready(function () {
            jQuery(".chosen").chosen();
        });
    </script>
</asp:Content>

