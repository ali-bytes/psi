<%@ Page Title="<%$Resources:Tokens,Centrals%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="Centrals.aspx.cs" Inherits="NewIspNL.Pages.Centrals" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <div class="view">
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="v_index" runat="server">
                <div class="page-header"><h1><asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,Centrals %>"></asp:Literal></h1></div>
                <div id="message">
                    <asp:Label runat="server" ID="l_message"></asp:Label></div>
                <asp:Panel runat="server" ID="p_index">
                    <div style="padding: 5px;">
                        <p>
                            <asp:Button ID="b_new" runat="server" CssClass="btn btn-success" Text="<%$Resources:Tokens,New%>" Width="90px"
                                OnClick="b_new_Click" /></p>
                        <asp:GridView ID="gv_index" runat="server" CssClass="table table-bordered table-condensed text-center" 
                             GridLines="None" AutoGenerateColumns="False" OnDataBound="gv_index_DataBound"
                            Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="#">
                                    <ItemTemplate>
                                        <asp:Label ID="l_number" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="<%$Resources:Tokens,Name%>" DataField="Name">
                                    <ItemStyle Width="400px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="GovernorateName" HeaderText="<%$Resources:Tokens,Governrate%>" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                                    <asp:LinkButton ID="gvb_edit" runat="server" CommandArgument='<%# Bind("Id") %>' OnClick="gvb_edit_Click"
                                                        ToolTip="<%$Resources:Tokens,Edit%>" data-rel="tooltip"><i class="icon-pencil bigger-150 blue"></i></asp:LinkButton>
                                                        &nbsp;&nbsp;
                                                    <asp:LinkButton ID="gvb_delete" runat="server" CommandArgument='<%# Bind("Id") %>' ToolTip="<%$Resources:Tokens,Delete%>"
                                                        OnClick="gvb_delete_Click" data-rel="tooltip"><i class="icon-trash bigger-150 red"></i></asp:LinkButton>

                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </asp:View>
            <asp:View ID="v_AddEdit" runat="server">
                <asp:Panel runat="server" ID="p_add">
                    <div class="page-header"><h1><asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,Central %>"></asp:Literal></h1></div>
                    <asp:HiddenField ID="hf_id" runat="server" />
                    <div class="well">
                        <div>
                            <label>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Tokens,Central%>"></asp:Label>
                            </label>
                            <div>
                                <asp:TextBox runat="server" ID="tb_offer" Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="r_offer" ControlToValidate="tb_offer"
                                    ErrorMessage="<%$Resources:Tokens,Required%>" Text="<%$Resources:Tokens,Required%>"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <label for="ddl_governates">
                                <asp:Literal ID="Label2" runat="server" Text="<%$Resources:Tokens,Governrate%>"></asp:Literal>
                            </label>
                            <div>
                                <asp:DropDownList ClientIDMode="Static" runat="server" ID="ddl_governates" Width="200px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="tb_offer"
                                    ErrorMessage="<%$Resources:Tokens,Required%>" Text="<%$Resources:Tokens,Required%>"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <p>
                            <br/>
                            <asp:Button runat="server" ID="b_save" CssClass="btn btn-success" Text="<%$Resources:Tokens,Save%>" OnClick="b_save_Click" />
                        </p>
                    </div>
                </asp:Panel>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
