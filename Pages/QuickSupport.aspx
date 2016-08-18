<%@ Page Title="<%$ Resources:Tokens,QuickSupport %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="QuickSupport.aspx.cs" ValidateRequest="false" Inherits="NewIspNL.Pages.QuickSupport" %>

<%@ Import Namespace="Resources" %>

<%--<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit.HTMLEditor" tagprefix="cc1" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="view">
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="v_index" runat="server">
                <div id="l_message" runat="server" Visible="False" class="alert alert-success">
                    <%=Tokens.Saved %>
                    </div>
                <asp:Panel runat="server" ID="p_index" >
                    <div class="page-header"><h1><asp:Literal runat="server" Text="<%$ Resources:Tokens,QuickSupport %>"></asp:Literal></h1></div>
                    <div style="padding: 5px;">
                        <p>
                            <asp:LinkButton ID="b_new" CssClass="btn btn-success" runat="server" 
                                OnClick="b_new_Click"><i class="icon-plus-sign"></i>&nbsp;<asp:Literal runat="server" Text="<%$Resources:Tokens,New%>"></asp:Literal></asp:LinkButton></p>
                        <div>
                            <asp:GridView  ID="gv_index" runat="server" CssClass="table table-bordered table-condensed text-center"
                                AutoGenerateColumns="False" OnDataBound="gv_index_DataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div>
                                                <asp:Label ID="l_number" runat="server" />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Url" HeaderText="<%$Resources:Tokens,Url %>" />
                                    <asp:BoundField DataField="Body" HeaderText="<%$Resources:Tokens,Notes %>" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div style="width: 200px;">
                                                <asp:LinkButton CommandArgument='<%#Eval("Id") %>' ID="BEdit" data-rel="tooltip"
                                                    runat="server" ToolTip="<%$Resources:Tokens,Edit %>" OnClick="gvb_edit_Click"><i class="icon-pencil icon-only bigger-130"></i></asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton CommandArgument='<%#Eval("Id") %>' ID="BDel" OnClientClick="return areyousure()" data-rel="tooltip"
                                                            runat="server" ToolTip="<%$Resources:Tokens,Delete %>" OnClick="gvb_delete_Click"><i class="icon-trash icon-only bigger-130 red"></i></asp:LinkButton>
                                                            
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate><%=Tokens.NoResults %></EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>
            </asp:View>
            <asp:View ID="v_AddEdit" runat="server">
                <asp:HiddenField ID="hf_id" runat="server" />
                <asp:Panel runat="server" ID="p_add">
                    <div class="page-header"><h1><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Tokens,QuickSupport %>"></asp:Literal></h1></div>
                    <div class="container-fluid">
                        <div class="well">
                            <asp:Literal runat="server" Text="<%$Resources:Tokens,Url %>"></asp:Literal>
                            <div>
                                <asp:TextBox runat="server" ID="txtUrl"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ControlToValidate="txtUrl" ValidationGroup="quik" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                            <asp:Literal runat="server" Text="<%$Resources:Tokens,Notes %>"></asp:Literal>
                            <div>
                                <%--<cc1:Editor ID="txtBody" runat="server" />--%>
                               <textarea id="txtBody" dir="rtl" ClientIDMode="Static" runat="server"></textarea>
                                  <asp:Label ID="Label4" runat="server" ForeColor="red" Text="*" Visible="false"></asp:Label>
                              <%--   <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                ControlToValidate="txtBody" ValidationGroup="quik" ErrorMessage="*"></asp:RequiredFieldValidator>
                                --%>
                            </div>
                        </div>
                    </div>
                    <hr />
                    <p>
                        <button runat="server" id="BSave" ValidationGroup="quik" class="btn btn-primary" onserverclick="b_save_Click">
                            <i class="icon-white icon-ok"></i>&nbsp;<asp:Literal ID="Literal7" Text="<%$Resources:Tokens,Save %>"
                                runat="server" />
                        </button>
                        &nbsp;|&nbsp;
                        <button runat="server" id="BBack" CausesValidation="False" class="btn btn-danger" onserverclick="ReturnToMainView">
                            <i class="icon-white icon-arrow-left"></i>&nbsp;<asp:Literal ID="Literal2" Text="<%$Resources:Tokens,Cancel %>"
                                runat="server" />
                        </button>
                    </p>
                </asp:Panel>
            </asp:View>
        </asp:MultiView>
    </div>
    
    
      <script type="text/javascript" src="/ckeditor/ckeditor.js"></script>
    <script type="text/javascript" src="/ckeditor/adapters/jquery.js"></script>
    <script type="text/javascript">
        function areyousure() {
            return confirm('<%= Tokens.AlertRUS %>');
        }

        $(document).ready(function() {

            $("#txtBody").ckeditor();
        });
    </script>
</asp:Content>
