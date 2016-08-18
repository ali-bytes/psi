<%@ Page Title="<%$Resources:Tokens,BranchsDiscounts %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="BranchsDiscount.aspx.cs" Inherits="NewIspNL.Pages.BranchsDiscount" %>
    
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view">
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="v_index" runat="server">
                <div id="message">
                    <asp:Label runat="server" ID="l_message"></asp:Label></div>
                <asp:Panel runat="server" ID="p_index">
                    <div class="page-header"><h1><asp:Literal runat="server" Text="<%$Resources:Tokens,BranchPackagesDiscounts %>"></asp:Literal></h1></div>
                    <div>
                        <div class="well">
                            <div>
                                <label for="DdlBranch">
                                    <%= Tokens.Branch%></label>
                                <div>
                                    <asp:DropDownList runat="server" ID="DdlBranch">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="DdlBranch" ID="R1"
                                        runat="server" />
                                </div>
                            </div>
                            <div>
                                <label for="DdlProvider">
                                    <%= Tokens.Provider %></label>
                                <div>
                                    <asp:DropDownList runat="server" ID="DdlProvider">
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
                                    <asp:TemplateField HeaderText="#">
                                        <ItemTemplate>
                                            <div>
                                                <asp:Label ID="l_number" runat="server" />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Branch" HeaderText="<%$Resources:Tokens,Branch %>" />
                                    <asp:BoundField DataField="Provider" HeaderText="<%$Resources:Tokens,Provider %>" />
                                    <asp:BoundField DataField="Package" HeaderText="<%$Resources:Tokens,Package %>" />
                                    <asp:BoundField DataField="Discount" HeaderText="<%$Resources:Tokens,Discount %>" />
                                    <%--<asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes %>" />--%>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                         <%--   <span class="btn btn-primary btn-xs" data-select="edit" data-discount='<%#Eval("Discount")%>' data-rel="tooltip" title="<%=Tokens.Edit %>"
                                                data-prov='<%#Eval("ProviderId")%>' data-bra='<%#Eval("BranchId")%>' data-pack='<%#Eval("PackageId")%>'>
                                               </span>--%>
                                            
                                              <button type="button" class="btn btn-primary btn-sm" data-pack='<%#Eval("PackageId")%>' onclick="savehf(<%#Eval("Discount")%>,'<%#Eval("ProviderId")%>',<%#Eval("BranchId")%>,<%#Eval("PackageId")%>)" title="<%=Tokens.Edit %>">
              
                                            <%=Tokens.Edit %>
                                             <i class="icon-only icon-edit"></i>
                                          
                                            </button>
                                            
                                            
                                            <%--<button   class="btn btn-primary btn-xs" onclick="savehf(<%#Eval("Discount")%>,'<%#Eval("ProviderId")%>',<%#Eval("BranchId")%>,<%#Eval("PackageId")%>)" ><%=Tokens.Edit %> </button>--%>
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
                <asp:HiddenField runat="server" ID="HfBra" />
                <asp:HiddenField runat="server" ID="HfPack" />
                <div>
                    <label for="TbDiscount">
                        <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,Discount %>" runat="server" />
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
        <div class="modal-footer">
            <span data-dismiss="modal" class="btn btn-danger"><i class="icon-white icon-arrow-left">
            </i>&nbsp;<%= Tokens.Cancel %></span>
            <button id="Button1" type="button" runat="server"  onserverclick="SaveDiscount" validationgroup="ed"
                class="btn btn-primary">
                <i class="icon-white icon-ok"></i>&nbsp;<%= Tokens.Save %></button>
        </div>
        </div></div>
    </div>
    <script type="text/javascript">
        
        function savehf(amount, pid, bid, packid) {
            


          

            var $editmodal = $('#editdiscount');

            document.getElementById("<%=HfPack.ClientID%>").value = packid;
            document.getElementById("<%=TbDiscount.ClientID%>").value = amount;
            document.getElementById("<%=HfBra.ClientID%>").value = bid;
            document.getElementById("<%=HfProv.ClientID%>").value = pid;
            $editmodal.modal('show');



         
        }




        //var $editmodal = $('#editdiscount');
        //var $pack = $('#HfPack');
        //var $bra = $('#HfBra');
        //var $prov = $('#HfProv');
        //var $discount = $('#TbDiscount');

        //$('span[data-select="edit"]').on('click', function () {
        //    $pack.val($(this).attr('data-pack'));
        //    $bra.val($(this).attr('data-bra'));
        //    $prov.val($(this).attr('data-prov'));
        //    $discount.val($(this).attr('data-discount'));
        //    $editmodal.modal('show');
        //    $discount.focus();
        //});
    </script>
</asp:Content>
