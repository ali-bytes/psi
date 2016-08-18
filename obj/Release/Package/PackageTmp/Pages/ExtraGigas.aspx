<%@ Page Title="<%$Resources:Tokens,ExtraGigas%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ExtraGigas.aspx.cs" Inherits="NewIspNL.Pages.ExtraGigas" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view">
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="v_index" runat="server">
                <div id="message" class="well well-small">
                    <asp:Label runat="server" ID="l_message" ClientIDMode="Static"></asp:Label></div>
                <asp:Panel runat="server" ID="p_index">
                    <div class="page-header"><h1><asp:Literal runat="server" Text="<%$Resources:Tokens,Index%>"></asp:Literal></h1></div>
                    <div>
                        <p>
                            <button type="button" ID="BNew" runat="server"  class="btn btn-primary"
                                    OnServerClick="BtnNew_Click" ><i class="icon-white icon-plus-sign"></i>&nbsp;<%=Tokens.New%>
                            
                            </button>
                        </p>
                        <div style="text-align: center">
                            <asp:GridView ID="GvItems" runat="server" CssClass="table table-bordered table-condensed"
                                AutoGenerateColumns="False" OnDataBound="gv_index_DataBound" 
                                ClientIDMode="Static">
                                <Columns>
                                    <asp:TemplateField HeaderText="#">
                                        <ItemTemplate>
                                            <asp:Label ID="l_number" runat="server"></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="<%$Resources:Tokens,Name%>" DataField="Name" />
                                    <asp:BoundField HeaderText="<%$Resources:Tokens,Price%>" DataField="Price" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div>
                                                <asp:LinkButton ID="gvb_edit" runat="server" CommandArgument='<%# Bind("Id") %>' OnClick="gvb_edit_Click"
                                                            ToolTip="<%$Resources:Tokens,Edit%>" data-rel="tooltip"><i class="icon-edit-sign bigger-130 icon-only"></i></asp:LinkButton>&nbsp;
                                                <asp:LinkButton ID="gvb_delete" runat="server" Visible='<%#Eval("CanDelete") %>' CommandArgument='<%# Bind("Id") %>' ToolTip="<%$Resources:Tokens,Delete%>"
                                                data-rel="tooltip" OnClick="gvb_delete_Click"><i class="icon-trash icon-only bigger-130 red"></i></asp:LinkButton>
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
                      <div class="page-header"><h1><asp:Literal runat="server" Text="<%$Resources:Tokens,Reason%>"></asp:Literal></h1></div>
                      <div class="well">
                          <label for="TbName">
                              <%= Tokens.Name %>
                          </label>
                          <div>
                              <asp:TextBox runat="server" ID="TbName" ></asp:TextBox>
                              <asp:RequiredFieldValidator runat="server" 
                                                          ID="RName" 
                                                          ControlToValidate="TbName"
                                                          ErrorMessage="<%$Resources:Tokens,Required%>">
                              </asp:RequiredFieldValidator>
                          </div>
                          <label for="">
                              <%= Tokens.Price %>
                          </label>
                          <div>
                              <asp:TextBox runat="server" ID="TbPrice"  ></asp:TextBox>
                              <asp:RequiredFieldValidator runat="server" 
                                                          ID="RPrice" 
                                                          ControlToValidate="TbPrice"
                                                          ErrorMessage="<%$Resources:Tokens,Required%>">
                              </asp:RequiredFieldValidator>

                          </div>
                       
                      </div>
                       <p>
                           <button runat="server" type="submit" ID="b_save"  OnServerClick="b_save_Click" class="btn btn-primary" ><i class="icon-white icon-ok"></i>&nbsp;<%=Tokens.Save%></button>
                           &nbsp;|&nbsp;
                           <button type="button" runat="server" ID="BCancel" OnServerClick="CancelProcess" class="btn btn-success" CausesValidation="False"><i class="icon-white icon-arrow-left"></i>&nbsp;<%=Tokens.Cancel %></button>
                        </p>
                </asp:Panel>
            </asp:View>
        </asp:MultiView>
    </div>
    <script src="../Content/ace-assest/js/jquery-ui-1.10.3.full.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('#l_message').html() === '') {
                $('#message').hide();
            } else {
                $('#message').slideDown("slow");
            }

        });
    </script>
</asp:Content>


