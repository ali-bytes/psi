<%@ Page Title="<%$Resources:Tokens,empDebits%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="EmployeDebitspage.aspx.cs" Inherits="NewIspNL.Pages.EmployeDebitspage" %>
<%@ Import Namespace="Resources" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.7.123, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="content">
         <asp:TextBox ID="txtError" runat="server" CssClass="error_box" Width="534px" 
                        Visible="False" ReadOnly="True" ></asp:TextBox>
          
        <div class="alert alert-success" style="display: <%=l_message.Text==string.Empty?"none;":"block;"%>">
            <asp:Label runat="server" ID="l_message" ClientIDMode="Static"></asp:Label>
        </div>
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="v_index" runat="server">
                <fieldset>
                    <legend><%=Tokens.empDebits%></legend>
                  
                      <div>
                        <div>
                            <p>
                                <asp:Button ID="b_new" runat="server" Text="<%$Resources:Tokens,Add%>" OnClick="b_new_Click" CssClass="btn btn-primary" />
                            </p>
                            <asp:GridView  style="text-align: center"  CssClass="table table-bordered table-hover" ID="GvItems" runat="server"
                                AutoGenerateColumns="False" OnDataBound="gv_index_DataBound" ClientIDMode="Static"
                                GridLines="None">
                                <AlternatingRowStyle BorderStyle="None" />
                                <Columns>
                                    <asp:TemplateField>
                                        
                                        <ItemTemplate>
                                            
                                            <asp:Label ID="l_number" runat="server"></asp:Label>
                                            
                                        </ItemTemplate>
                                        
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Name" HeaderText="<%$Resources:Tokens,Name%>"></asp:BoundField>
                                   <asp:BoundField HeaderText="<%$Resources:Tokens,Value%>" DataField="Increase"></asp:BoundField>
                                    <asp:BoundField DataField="Time" HeaderText="<%$Resources:Tokens,Date%>" />
                                     <asp:BoundField DataField="note" HeaderText="<%$Resources:Tokens,Note%>"></asp:BoundField>
                                    <asp:TemplateField HeaderText="<%$Resources:Tokens,Delete%>" >
                                        
                                        <ItemTemplate>
                                            
                                            <asp:Button ID="gvb_delete" runat="server" CommandArgument='<%# Bind("Id") %>' CssClass="btn btn-danger" OnClientClick="return confirm('هل تريد رد المسحوب ؟');" OnClick="gvb_delete_Click" Text="<%$Resources:Tokens,returnn%>" />
                                            
                                        </ItemTemplate>
                                        
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BorderStyle="None" />
                                <PagerStyle BorderStyle="None" />
                                <RowStyle BorderStyle="None" />
                            </asp:GridView>
                        </div>
                    </div>
                </fieldset>
            </asp:View>
            <asp:View ID="v_AddEdit" runat="server">
                <div>
                    <fieldset>
                        <legend><%=Tokens.empDebits%> </legend>
                        <asp:HiddenField ID="hf_id" runat="server" />
                        <div class="well">
                            <div>
                                <label for="tb_name">
                                    <%=Tokens.Name%>
                                </label>
                                <div>
                                    <asp:DropDownList runat="server" ID="tb_name" />
                                    <asp:RequiredFieldValidator runat="server" ID="Rtb_name" ControlToValidate="tb_name"
                                        ErrorMessage="*" CssClass="validation">*</asp:RequiredFieldValidator>
                                    <br />
                                    <label for="tb_name">
                                    <%=Tokens.UserSaves%>
                                    </label>
                                    <div>
                                        <asp:DropDownList ID="DropDownList1" runat="server" />
                                        <asp:RequiredFieldValidator ID="Rtb_name0" runat="server" ControlToValidate="DropDownList1" CssClass="validation" ErrorMessage="*">*</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                          
                            <div>
                                <label for="TbIncrease">
                                    <%=Tokens.Value%></label>
                                <div>
                                    <asp:TextBox runat="server" ID="TbIncrease" ClientIDMode="Static" required="required" type="number" />
                                    <asp:RequiredFieldValidator ID="RTbIncrease" ErrorMessage="*" ControlToValidate="TbIncrease"
                                        runat="server" />
                                    <br />
                                   <%=Tokens.Note%><br />
                                    <asp:TextBox runat="server" ID="TextBox1" ClientIDMode="Static" required="required" type="number" Height="108px" TextMode="MultiLine" Width="224px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="*" ControlToValidate="TextBox1"
                                        runat="server" />
                                    <br />
                                <%=Tokens.Date%><br />&nbsp; <%--  <asp:TextBox ID="TextBox5" runat="server" CssClass="textbox" ReadOnly="True"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton2" TargetControlID="TextBox5">
                                    </cc1:CalendarExtender>
                                    <asp:ImageButton ID="ImageButton2" runat="server" ImageAlign="Bottom" ImageUrl="../images/calendar.png" Width="16px" Height="16px"/>
                                  --%>
                                     <asp:TextBox ID="TextBox5" runat="server"  data-select="dt"></asp:TextBox>
                                      &nbsp;<asp:RequiredFieldValidator ID="RGradeYear0" runat="server" ControlToValidate="TextBox5" ErrorMessage="*" />
                                  
                                    <br />
                                    <br />
                                </div>
                            </div>


                            <p>
                                <asp:Button runat="server" CssClass="btn btn-primary" ID="BSave" Text="<%$Resources:Tokens,save%>" OnClick="b_save_Click">
                                    <%--  <i class="icon-white icon-ok"></i>&nbsp;حفظ--%></asp:Button>
                                <span>| </span>
                                <asp:Button runat="server" ID="BBack" CssClass="btn btn-success" CausesValidation="False"
                                    OnClick="BBack_OnClick" Text="<%$Resources:Tokens,Cancel%>" />
                            </p>
                        </div>
                    </fieldset>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>
        <script type="text/javascript">
            $('input[data-select="dt"]').datepicker({
                showOtherMonths: true,
                selectOtherMonths: false,
                dateFormat: 'yy/mm/dd'
            });</script>
</asp:Content>