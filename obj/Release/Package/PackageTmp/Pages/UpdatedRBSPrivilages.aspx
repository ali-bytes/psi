<%@ Page Title="<%$Resources:Tokens,UpdatedRBSPrivilages %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="UpdatedRBSPrivilages.aspx.cs" Inherits="NewIspNL.Pages.UpdatedRBSPrivilages" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">    
        <div class="view">
        <fieldset>
            <div class="page-header"><h1><%=Tokens.UpdatedRBSPrivilages%></h1></div>
            <div class="view">
                <div class="well">
                <label><%=Tokens.Groups %></label>
                <div>
                    <asp:DropDownList runat="server" ID="ddlGroups" DataTextField="GroupName"
                                      DataValueField="ID" AutoPostBack="True"
                                      onselectedindexchanged="ddlGroups_SelectedIndexChanged"/>
                </div>
                <br/>
                <label><%=Tokens.GroupPrivileges %></label>
                <asp:CheckBoxList runat="server" ClientIDMode="Static" RepeatDirection="Vertical" ID="CheckPrivilages">
                    <asp:ListItem  Value="0" Text='<%$Resources:Tokens,Edit %>'></asp:ListItem>
                    <asp:ListItem Value="1" Text='<%$Resources:Tokens,Delete %>'></asp:ListItem>
                    <asp:ListItem Value="2" Text='<%$Resources:Tokens,Print %>'></asp:ListItem>
                    <asp:ListItem Value="3" Text='<%$Resources:Tokens,AddResellerBS %>'></asp:ListItem>
                    <asp:ListItem Value="4" Text='<%$Resources:Tokens,AddResellerPayments %>'></asp:ListItem>
                </asp:CheckBoxList>
                <p>
                    <asp:Button runat="server" ID="btnSave" CssClass="btn btn-success" 
                                onclick="btnSave_Click" Text="<%$Resources:Tokens,Save %>"/>
                </p>
                    <asp:Label runat="server" ID="lblMsg" ForeColor="Green"></asp:Label>
                    </div>
            </div>
        </fieldset>
    </div>
    <style type="text/css">
        #CheckPrivilages>tbody>tr>td>label {
            display: -webkit-inline-box;
            margin-right: 4px;
            margin-bottom: 15px;
        }
    </style>
    <script type="text/javascript">
        $('tr td input[type=checkbox]').addClass("ace");
        //$('tr td input[type=radio]').addClass("ace");
        $('tr td label').addClass("lbl");
    </script>
</asp:Content>
