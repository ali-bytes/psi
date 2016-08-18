<%@ Page Title="<%$Resources:Tokens,SiteData%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="SiteData.aspx.cs" Inherits="NewIspNL.Pages.SiteData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <fieldset>
            <div class="page-header"><h1>
                <asp:Literal Text="<%$Resources:Tokens,SiteData %>" runat="server" /></h1></div>
            <div class="well">
                <div>
                    <asp:Label ID="Label2" Text="<%$Resources:Tokens,CompanyNameInput %>" runat="server" AssociatedControlID="TbCompanyName" />
                    <div>
                        <asp:TextBox runat="server" ID="TbCompanyName" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="<%$Resources:Tokens,Required %>" ControlToValidate="TbCompanyName"
                                                    runat="server" />
                    </div>
                </div>
                <div>
                    <asp:Label runat="server" ID="lbl" Text="<%$Resources:Tokens,Url %>" AssociatedControlID="TbCompanyLink"></asp:Label>
                    <div>
                        <asp:TextBox runat="server" ID="TbCompanyLink"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="<%$Resources:Tokens,Required %>" ControlToValidate="TbCompanyLink"
                                                    runat="server" />
                    </div>
                </div>
                <div>
                    <asp:Label ID="Label1" Text="<%$Resources:Tokens,Logo %>" runat="server" AssociatedControlID="FLogo" />
                    <div>
                        <asp:FileUpload runat="server" ID="FLogo" />
                         <asp:RegularExpressionValidator runat="server" ForeColor="red" ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|png|PNG|)$" ControlToValidate="FLogo"  ErrorMessage="<%$Resources:Tokens,uploadimgerror %> "></asp:RegularExpressionValidator>
                   
                  
                        <div>
                            <asp:Image runat="server" ID="ImgLogo" />
                        </div>
                    </div>
                </div>
            </div>
            <p>
                <button runat="server" onserverclick="BSave_OnServerClick" id="BSave" class="btn btn-success">
                    <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,Save %>" runat="server" /></button>
            </p>
            <p>
                <asp:Literal ID="LMsg" runat="server" />
            </p>
        </fieldset>
    </div>
</asp:Content>

