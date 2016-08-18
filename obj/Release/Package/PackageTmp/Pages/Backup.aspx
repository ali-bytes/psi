<%@ Page Title="<%$Resources:Tokens,Backup%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="Backup.aspx.cs" Inherits="NewIspNL.Pages.Backup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="content noprint">
        <div>
            <p style="font-weight: bold;color: red;font-size: 14pt;">
                <asp:Literal runat="server" ID="LMessage"></asp:Literal>
            </p>
            <p>
                <asp:Button CssClass="btn btn-primary" runat="server" ID="BBackup" Text="حفظ نسخة" OnClick="BBackup_OnClick"/>
                            
            </p>
            <div>
                                <asp:GridView CssClass="table table-striped table-bordered table-hover" 
                                    ID="gv_index" runat="server" AutoGenerateColumns="False" ClientIDMode="Static"  
                                    GridLines="None" >
                                    <AlternatingRowStyle BorderStyle="None" />
                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="رقم الملف" />
                                        <asp:BoundField DataField="Time" HeaderText="الوقت" />
                                        <asp:BoundField DataField="Url" HeaderText="اسم الملف" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <%--<a class="btn btn-success" href='<%#Eval("Url") %>'><i class="icon-white icon-download-alt"></i> تحميل</a>--%> 
                                                <asp:Button runat="server" CssClass="btn btn-success" Text="save" ID="btnSave" 
                                                    CommandArgument='<%# Bind("Id") %>' onclick="btnSave_Click" />
                                                | <asp:Button runat="server" CssClass="btn btn-danger" Text="حذف" ID="delBtu" 
                                                    CommandArgument='<%# Bind("Id") %>' onclick="delBtu_Click" /> 
                                                | 
                                                <asp:Button ID="Button1" runat="server" CommandArgument='<%# Bind("Id") %>' 
                                                    CssClass="btn btn-primary" onclick="Button1_Click" Text="ارسال" />
                                                 | 
                                                <asp:Button ID="btnrestore" runat="server" CommandArgument='<%# Bind("Id") %>' 
                                                    CssClass="btn btn-primary" onclick="btnrestore_Click" Text="استرجاع" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                     <HeaderStyle BorderStyle="None" />
                                <PagerStyle BorderStyle="None" />
                                <RowStyle BorderStyle="None" />
                                </asp:GridView>
                            </div>
                        </div>

      
        <asp:Literal runat="server" ID="Message"></asp:Literal>
    </div>
</asp:Content>
