<%@ Page Title="<%$ Resources:Tokens,Offers %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="Offers.aspx.cs" Inherits="NewIspNL.Pages.Offers" %>
 
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="view">
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="v_index" runat="server">
                <div id="message">
                    <asp:Label runat="server" ID="l_message"></asp:Label></div>
                <asp:Panel runat="server" ID="p_index">
                    <div class="page-header"><h1><asp:Literal runat="server" Text="<%$ Resources:Tokens,Offers %>"></asp:Literal></h1></div>
                    <div style="padding: 5px;">
                        <p>
                            <asp:LinkButton ID="b_new" CssClass="grid btn btn-success" runat="server" 
                                OnClick="b_new_Click"><i class="icon-plus-sign"></i>&nbsp;<asp:Literal runat="server" Text="<%$Resources:Tokens,New%>"></asp:Literal></asp:LinkButton></p>
                        <div>
                            <asp:GridView  ID="gv_index" runat="server" CssClass="table table-bordered table-condensed"
                                AutoGenerateColumns="False" OnDataBound="gv_index_DataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div>
                                                <asp:Label ID="l_number" runat="server" />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Title" HeaderText="<%$Resources:Tokens,Name %>" />
                                    <asp:BoundField DataField="Discount" HeaderText="<%$Resources:Tokens,Discount %>" />
                                    <asp:BoundField DataField="ByPercent" HeaderText="<%$Resources:Tokens,ByPercent %>" />
                                    <asp:BoundField DataField="RouterCost" HeaderText="<%$Resources:Tokens,RouterCost %>" />
                                    <asp:TemplateField Visible="False" HeaderText="<%$Resources:Tokens,Resellers %>">
                                        <ItemTemplate>
                                            <div>
                                                <asp:Literal runat="server" Text='<%#Eval("Resellers") %>' />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$Resources:Tokens,Branches %>">
                                        <ItemTemplate>
                                            <div>
                                                <asp:Literal ID="Literal5" runat="server" Text='<%#Eval("Branches") %>' />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                 
                                    <asp:BoundField DataField="LifeTime" HeaderText="<%$Resources:Tokens,OfferMonths %>" />
                                    <asp:BoundField DataField="FreeMonths" HeaderText="<%$Resources:Tokens,FreeMonthsCount %>" />
                                    <asp:BoundField DataField="FreeMonthsFirst" HeaderText="<%$Resources:Tokens,FreeMonthsFirst %>" />
                                    <asp:BoundField DataField="CanUpDown" HeaderText="<%$Resources:Tokens,canUpgrade/Downgrade %>" />
                                    <asp:BoundField DataField="CalculateOneBill" HeaderText="<%$Resources:Tokens,AggregatedBill %>" />
                                    <asp:TemplateField HeaderText="<%$Resources:Tokens,Providers %>">
                                        <ItemTemplate>
                                            <div>
                                                <asp:Literal runat="server" Text='<%#Eval("Providers") %>' />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CancelPenalty" HeaderText="<%$Resources:Tokens,CancelPenaltyOfFirstYear %>" />
                                    <asp:BoundField DataField="SuspendPenalty" HeaderText="<%$Resources:Tokens,CancelIfSuspensionExceeds %>" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div style="width: 100px;">
                                                <asp:LinkButton CommandArgument='<%#Eval("Id") %>' ID="BEdit" data-rel="tooltip"
                                                    runat="server" ToolTip="<%$Resources:Tokens,Edit %>" OnClick="gvb_edit_Click"><i class="icon-pencil icon-only bigger-130 blue"></i></asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton CommandArgument='<%#Eval("Id") %>' ID="BDel" data-rel="tooltip"
                                                            runat="server" ToolTip="<%$Resources:Tokens,Delete %>" OnClick="gvb_delete_Click"><i class="icon-trash icon-only red bigger-130"></i></asp:LinkButton>
                                                            &nbsp;&nbsp;&nbsp;
                                               <asp:HyperLink runat="server" NavigateUrl='<%#Eval("PackagesLink") %>' data-rel="tooltip" ToolTip="<%$Resources:Tokens,Packages %>" Target="_blank"><i class="icon-fire icon-only orange2 bigger-130" ></i></asp:HyperLink>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>
            </asp:View>
            <asp:View ID="v_AddEdit" runat="server">
                <asp:HiddenField ID="hf_id" runat="server" />
                <asp:Panel runat="server" ID="p_add">
                    <div class="page-header"><h1><asp:Literal runat="server" Text="<%$ Resources:Tokens,Offer%>"></asp:Literal></h1></div>
                    <asp:HiddenField ID="hd_packId" runat="server" Value='<%# Bind("PackId") %>' />
                    <div class="container-fluid">
                        <div class="row-fluid">
                            <div class="col-sm-4">
                                <div class="well">
                                    <div>
                                        <label for="TbCancel">
                                            <asp:Literal Text="<%$Resources:Tokens,CancelPenaltyOfFirstYear %>" runat="server" />
                                        </label>
                                        <div>
                                            <asp:TextBox runat="server" ID="TbCancel" ClientIDMode="Static"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RTbCancel" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                ControlToValidate="TbCancel" runat="server" />
                                            <asp:RangeValidator ID="RangeValidator3" runat="server" ErrorMessage="RangeValidator"
                                                ControlToValidate="TbCancel" Display="Dynamic" MaximumValue="999999999999999"
                                                MinimumValue="0" Type="Double" Text="<%$ Resources:Tokens,WrongAmount %>"></asp:RangeValidator>
                                        </div>
                                    </div>
                                    <div>
                                        <label for="TbCancel">
                                            <asp:Literal Text="<%$Resources:Tokens,CancelIfSuspensionExceeds %>" runat="server" />
                                        </label>
                                        <div>
                                            <asp:TextBox runat="server" ID="TbSuspend" ClientIDMode="Static"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RTbSuspend" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                ControlToValidate="TbSuspend" runat="server" />
                                            <asp:RangeValidator ID="RangeValidator4" runat="server" ErrorMessage="RangeValidator"
                                                ControlToValidate="TbSuspend" Display="Dynamic" MaximumValue="999999999999999"
                                                MinimumValue="0" Type="Double" Text="<%$ Resources:Tokens,WrongAmount %>"></asp:RangeValidator>
                                        </div>
                                    </div>
                                    <div>
                                         <asp:CheckBox runat="server" ID="checkUpDownGrade" Text="<%$Resources:Tokens,canUpgrade/Downgrade %>" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="well ">
                                    <div>
                                        <div>
                                            <label for="ddl_monthsCount">
                                                <asp:Literal ID="Label5" runat="server" Text="<%$ Resources:Tokens,OfferMonthsCount%>"></asp:Literal>
                                            </label>
                                            <div>
                                                <asp:DropDownList ID="ddl_monthsCount" runat="server" ClientIDMode="Static" Width="200px">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="ddl_monthsCount"
                                                    ErrorMessage="<%$ Resources:Tokens,Required %>" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div>
                                            <label>
                                                <asp:Literal Text="<%$Resources:Tokens,FreeMonthsCount %>" runat="server" />
                                            </label>
                                            <div>
                                                <asp:DropDownList runat="server" ID="DdlFreeMonths" />
                                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="ddl_monthsCount"
                                                    ControlToValidate="DdlFreeMonths" ErrorMessage="<%$Resources:Tokens,FreeMonthsValidation %>"
                                                    Operator="LessThanEqual" Type="Integer"></asp:CompareValidator>
                                            </div>
                                            <div>
                                                <asp:CheckBox runat="server" ID="FreeMonthsFirst" Text="<%$Resources:Tokens,FreeMonthsFirst %>" />
                                            </div>
                                            <div>
                                                <asp:CheckBox runat="server" ID="CbCalculateOneBill" Text="<%$Resources:Tokens,AggregatedBill %>" />
                                            </div>
                                        </div>
                                     
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="well">
                                    <div>
                                        <label for="tb_title">
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Tokens,Name%>"></asp:Label>
                                        </label>
                                        <div>
                                            <asp:TextBox ClientIDMode="Static" runat="server" ID="tb_title" Width="200px"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="r_offer" ControlToValidate="tb_title"
                                                ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div>
                                        <asp:CheckBox runat="server" ID="IsPercent" Text="<%$Resources:Tokens,ByPercent %>" />
                                    </div>
                                    <div>
                                        <label for="TbDiscount">
                                            <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,Discount %>" runat="server" />
                                        </label>
                                        <div>
                                            <span>
                                                <asp:TextBox ID="TbDiscount" runat="server" ClientIDMode="Static"></asp:TextBox>
                                            </span>
                                            <asp:RequiredFieldValidator ID="RDis" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                                        ControlToValidate="TbDiscount" runat="server" />
                                            <asp:RangeValidator ID="RangeValidator2" runat="server" ErrorMessage="RangeValidator"
                                                                ControlToValidate="TbDiscount" Display="Dynamic" MaximumValue="999999999999999"
                                                                MinimumValue="0" Type="Double" Text="<%$ Resources:Tokens,WrongAmount %>"></asp:RangeValidator>
                                        </div>
                                    </div>
                                    <div>
                                        <asp:CheckBox runat="server" ID="CheckRouter" 
                                            Text="<%$Resources:Tokens,Router %>" AutoPostBack="True" 
                                            oncheckedchanged="CheckRouter_CheckedChanged" />
                                    </div>

                                    <div>
                                        
                                        <label for="TbRouterCost">
                                            <asp:Literal ID="Literal4" Text="<%$Resources:Tokens,RouterCost %>" runat="server" />
                                        </label>
                                        <div>
                                            <asp:TextBox runat="server" ID="TbRouterCost" Enabled="False" ClientIDMode="Static" Text="0"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RTb" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                                        ControlToValidate="TbRouterCost" runat="server" />
                                            <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="RangeValidator"
                                                                ControlToValidate="TbRouterCost" Display="Dynamic" MaximumValue="999999999999999"
                                                                MinimumValue="0" Type="Double" Text="<%$ Resources:Tokens,WrongAmount %>"></asp:RangeValidator>
                                        </div>
                                    </div>
                                    <div>
                                        <label>
                                            <asp:CheckBox runat="server" ID="CheckAll" ClientIDMode="Static" />
                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Tokens,Resellers %>"></asp:Label>
                                        </label>
                                        <div style="padding: 0 17px;">
                                            <asp:CheckBoxList ClientIDMode="Static" runat="server" ID="cbl_resellers" Width="200px">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                     <div>
                                        <label>
                                            <asp:CheckBox runat="server" ID="AllBranchs" ClientIDMode="Static" />
                                            <asp:Label ID="Label77" runat="server" Text="<%$ Resources:Tokens,Branch %>"></asp:Label>
                                        </label>
                                        <div style="padding: 0 17px;">
                                            <asp:CheckBoxList ClientIDMode="Static" runat="server" ID="CblBranches" Width="200px">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <hr />
                    <p>
                        <button runat="server" id="BSave" class="btn btn-primary" onserverclick="b_save_Click">
                            <i class="icon-white icon-ok"></i>&nbsp;<asp:Literal ID="Literal3" Text="<%$Resources:Tokens,Save %>"
                                runat="server" />
                        </button>
                        &nbsp;|&nbsp;
                        <button runat="server" id="BBack" CausesValidation="False" class="btn btn-success" onserverclick="ReturnToMainView">
                            <i class="icon-white icon-arrow-left"></i>&nbsp;<asp:Literal ID="Literal2" Text="<%$Resources:Tokens,Cancel %>"
                                runat="server" />
                        </button>
                    </p>
                </asp:Panel>
            </asp:View>
        </asp:MultiView>
    </div>
    <span id="page-direction" data-culture="<%=Thread.CurrentThread.CurrentUICulture.ToString().Contains("ar")?"right":"left" %>">
    </span>
 
    <script type="text/javascript">
        $(function () {
            

            var allChecker = $('#CheckAll');
            var allChecks = $('#cbl_resellers input[type="checkbox"]');
            $(allChecker).bind('change', function () {
                if ($(allChecker).is(':checked')) {
                    $(allChecks).attr('checked', 'checked');
                } else {
                    $(allChecks).removeAttr('checked');
                }
            });
            var allBranch = $('#AllBranchs');
            var allCheckes = $('#CblBranches input[type="checkbox"]');
            $(allBranch).bind('click', function () {
                if ($(allBranch).is(':checked')) {
                    $(allCheckes).attr('checked', 'checked');
                } else {
                    $(allCheckes).removeAttr('checked');
                }
            });

            var allPChecker = $('#PChecker');
            var allPChecks = $('#CblProviders input[type="checkbox"]');
            $(allPChecker).bind('change', function () {
                if ($(allPChecker).is(':checked')) {
                    $(allPChecks).attr('checked', 'checked');
                } else {
                    $(allPChecks).removeAttr('checked');
                }
            });
            var direction = $('#page-direction').attr('data-culture');
            var vsDirection = direction === "right" ? "left" : "left";
            $('input[type="checkbox"]').css('float', direction).css('margin-' + vsDirection, '3px');
        });

    </script>

</asp:Content>

