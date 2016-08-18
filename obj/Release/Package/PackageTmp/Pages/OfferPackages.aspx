<%@ Page Title="<%$Resources:Tokens,OfferPackages%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="OfferPackages.aspx.cs" Inherits="NewIspNL.Pages.OfferPackages" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="v_index" runat="server">
                <asp:Panel runat="server" ID="p_index">
                    <div class="page-header"><h1><asp:Literal runat="server" Text="<%$Resources:Tokens,OfferPackages%>"></asp:Literal></h1></div>
                    <h3 runat="server" id="H3" class="header smaller lighter blue">
                    </h3>
                    <div>
                        <asp:GridView ID="gv_index" runat="server" CssClass="table table-bordered table-condensed"
                            AutoGenerateColumns="False" OnDataBound="gv_index_DataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="l_number" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Provider" HeaderText="<%$Resources:Tokens,Provider %>" />
                                <asp:BoundField DataField="Package" HeaderText="<%$Resources:Tokens,Package %>" />
                                <asp:TemplateField HeaderText="<%$Resources:Tokens,InOffer%>">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" Checked='<%#Eval("Checked") %>' ID="CbSelected" CssClass='<%#Eval("Concate") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <p>
                        <button runat="server" onserverclick="Save" type="submit" class="btn btn-success">
                            <i class="icon-white icon-ok"></i>&nbsp;<%= Tokens.Save %></button>&nbsp;<a class="btn btn-primary"
                                href="Offers.aspx"><i class="icon-white icon-arrow-left"></i>&nbsp;<%= Tokens.Offers %></a>
                    </p>
                    <div id="message">
                        <asp:Label runat="server" ID="l_message"></asp:Label></div>
                    <asp:HiddenField runat="server" ID="HfOfferId" />
                </asp:Panel>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
