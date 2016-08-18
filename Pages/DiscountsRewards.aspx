<%@ Page Title="<%$Resources:Tokens,empreward%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="DiscountsRewards.aspx.cs" Inherits="NewIspNL.Pages.DiscountsRewards" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
    <fieldset>
                    <legend><%=Tokens.empreward%><br />
                    <asp:TextBox ID="txtMessage" runat="server" CssClass="alert-success" Width="534px" 
                        Visible="False" ReadOnly="True" ></asp:TextBox>
                        <br />
                <asp:TextBox ID="txtError" runat="server" CssClass="alert-error" Width="534px" 
                        Visible="False" ReadOnly="True"  ></asp:TextBox>
                    </legend>
          <span style="font-size: large; text-align: center">
        <%=Tokens.Employee%>   <br />
                    <asp:DropDownList ID="DropDownList1" runat="server" >
                    </asp:DropDownList>
                  <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" 
                        ErrorMessage="&lt;img src=../images/user_logout.png style=height:12px;width:14px; /&gt;" 
                        ControlToValidate="DropDownList1" ValidationGroup="branch" 
                    InitialValue="-1" Display="Dynamic"></asp:RequiredFieldValidator>
                    <br />
                        <%=Tokens.Type%>   <br />
                    <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" 
                        ErrorMessage="*" ForeColor="red"
                        ControlToValidate="DropDownList2" ValidationGroup="branch" 
                    InitialValue="-1" Display="Dynamic"></asp:RequiredFieldValidator>
                    <br />
                        <%=Tokens.Value%>   <br />
                    <asp:TextBox ID="TextBox1" runat="server" Width="205px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                    ControlToValidate="TextBox1" 
                    ErrorMessage="*" ForeColor="red"
                    ValidationGroup="branch" Display="Dynamic"></asp:RequiredFieldValidator>
                    <br />
                        <%=Tokens.Notes%>   <br />
                    <asp:TextBox ID="TextBox2" runat="server" Height="79px" TextMode="MultiLine" Width="205px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" 
                    ControlToValidate="TextBox2" 
                    ErrorMessage="*" ForeColor="red"
                    ValidationGroup="branch" Display="Dynamic"></asp:RequiredFieldValidator>
                   
                    <br />
                    <br />  <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="<%$Resources:Tokens,Add%>" ValidationGroup="branch" CssClass="btn btn-primary" />
                    <br />
                   <br />
              
              
               <br />
                
                      <%=Tokens.SearchBy%>  <%=Tokens.Month%> <br />
                    <asp:DropDownList ID="DropDownList3" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList3_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" 
                        ErrorMessage="&lt;img src=../images/user_logout.png style=height:12px;width:14px; /&gt;" 
                        ControlToValidate="DropDownList3" ValidationGroup="branch" 
                    InitialValue="-1" Display="Dynamic"></asp:RequiredFieldValidator>
                       <br />
              
              
               <br />
              <asp:GridView ID="GridView1" CssClass="table table-bordered table-hover table-responsive"   runat="server" AutoGenerateColumns="False" Width="100%" >
                        <Columns>
                            <asp:BoundField HeaderText="<%$Resources:Tokens,Name%>" DataField="Name"/>
                            <asp:BoundField HeaderText="<%$Resources:Tokens,Value%>" DataField="value" />
                            <asp:BoundField HeaderText="<%$Resources:Tokens,Type%>" DataField="kind" />
                            <asp:BoundField HeaderText="<%$Resources:Tokens,Note%>" DataField="note"/>
                            <asp:BoundField HeaderText="<%$Resources:Tokens,Date%>" DataField="date" DataFormatString="{0:dd/MM/yyyy}"/>
                        </Columns>
                    </asp:GridView>
                    </span>
       </fieldset>
   
    
</asp:Content>
