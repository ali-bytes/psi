<%@ Page Title="<%$Resources:Tokens,addemp%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="HrRegister.aspx.cs" Inherits="NewIspNL.Pages.HrRegister" %>
<%@ Import Namespace="Resources" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.7.123, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:TextBox ID="txtError" runat="server" CssClass="error_box" Width="534px" 
                        Visible="False" ReadOnly="True" ></asp:TextBox>
           
    <div class="content">
        <div class="alert alert-success" style="display: <%=l_message.Text==string.Empty?"none;":"block;"%>">
            <asp:Label runat="server" ID="l_message" ClientIDMode="Static"></asp:Label>
        </div>
        <asp:MultiView ID="MultiView1" runat="server" >
            <asp:View ID="v_index" runat="server" >
                <fieldset >
                    <legend><%= Tokens.Employees%></legend>
                    <div>
                        <div>
                            <p>
                                <asp:Button ID="b_new" runat="server" Text="<%$Resources:Tokens,Add%>" OnClick="b_new_Click" CssClass="btn btn-primary" />
                            </p>
                            
                            <asp:GridView  CssClass="table table-bordered table-condensed center" ID="GvItems" runat="server"
                                AutoGenerateColumns="False" OnDataBound="gv_index_DataBound"  ClientIDMode="Static"
                                GridLines="None" Width="100%"  >
                                <AlternatingRowStyle BorderStyle="None" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="l_number" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="<%$Resources:Tokens,Name%>" DataField="Name"></asp:BoundField>
                                    <asp:BoundField DataField="Code" HeaderText="<%$Resources:Tokens,Code%>" />
                                    <asp:BoundField HeaderText="<%$Resources:Tokens,Mobile%>" DataField="Mobile"></asp:BoundField>
                                    <asp:BoundField HeaderText=" <%$Resources:Tokens,Email%>" DataField="Email"></asp:BoundField>
                                    <asp:BoundField DataField="rent" HeaderText="<%$Resources:Tokens,sala%>" />
                                    <asp:BoundField DataField="Attendance" HeaderText="<%$Resources:Tokens,attend%>" />
                                    <asp:BoundField DataField="Leave" HeaderText="<%$Resources:Tokens,go%>" />
                                    <asp:BoundField DataField="LastDiscount" HeaderText="<%$Resources:Tokens,maxdis%>" />
                                   <asp:BoundField DataField="HiringDate" HeaderText="<%$Resources:Tokens,hiredate%>" DataFormatString="{0:dd-MM-yyyy}" />
                                  
                                    <asp:TemplateField HeaderText="<%$Resources:Tokens,Update%>" >
                                        <ItemTemplate>
                                            <asp:Button ID="gvb_edit" runat="server" CommandArgument='<%#Bind("Id") %>' OnClick="gvb_edit_Click"
                                                Text="<%$Resources:Tokens,Update%>" CssClass="btn btn-primary" Width="60px"/>
                                             </ItemTemplate>
                                    </asp:TemplateField>
                                             <asp:TemplateField HeaderText="<%$Resources:Tokens,Delete%>" >
                                        <ItemTemplate>
                                            <asp:Button ID="gvb_delete" runat="server" CommandArgument='<%# Bind("Id") %>' Text="<%$Resources:Tokens,Delete%>"
                                                OnClick="gvb_delete_Click" CssClass="btn btn-danger" Width="60px" />
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
                        <legend><%= Tokens.Employee%></legend>
                        <asp:HiddenField ID="hf_id" runat="server" />
                        <div class="well" style="text-align: right">
                            <div>
                                <label for="tb_name">
                                    <div class="pagination-right">
                                      <%= Tokens.Name%>
                                </div>
                                </label>
                                <div class="pagination-right">
                                    <asp:TextBox runat="server" ID="tb_name"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="r_offer" ControlToValidate="tb_name"
                                        ErrorMessage="*" CssClass="validation">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div>
                                <label for="tb_code">
                                    <div class="pagination-right">
                                       <%= Tokens.Code%></div>
                                </label>
                                &nbsp;<div class="pagination-right">
                                    <asp:TextBox runat="server" ID="tb_code" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="*" ControlToValidate="tb_code" runat="server" />
                                </div>
                            </div>
                            <div>
                                <label for="TbAddress">         
                                    <div class="pagination-right">
                                        <%= Tokens.Address%>
                                </div>
                                </label>
                                <div class="pagination-right">
                                    <asp:TextBox runat="server" ID="TbAddress" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="RfToAddress" ErrorMessage="*" ControlToValidate="TbAddress"
                                        runat="server" />
                                </div>
                            </div>
                            <div>
                                <label for="TbPhone">
                                    <div class="pagination-right">
                                        <%= Tokens.Phone%></div>
                                </label>
                                &nbsp;<div class="pagination-right">
                                    <asp:TextBox runat="server" ID="TbPhone" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="RfTbPhone" ErrorMessage="*" ControlToValidate="TbPhone"
                                        runat="server" />
                                </div>
                            </div>
                            <div>
                                <label for="TbMobile">
                                    <div class="pagination-right">
                                        <%= Tokens.Mobile%></div>
                                </label>
                                &nbsp;<div class="pagination-right">
                                    <asp:TextBox runat="server" ID="TbMobile" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="RTbMobile" ErrorMessage="*" ControlToValidate="TbMobile"
                                        runat="server" />
                                </div>
                            </div>


                            <div>
                                <label for="TbEmail">
                                    <div class="pagination-right">
                                        <%= Tokens.Email%>
                                </div>
                                </label>
                                <div class="pagination-right">
                                    <asp:TextBox runat="server" ID="TbEmail"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RTbEmail" ErrorMessage="*" ControlToValidate="TbEmail"
                                        runat="server" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" CssClass="validation"
                                        ControlToValidate="TbEmail" ErrorMessage="RegularExpressionValidator" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">ايميل خطأ</asp:RegularExpressionValidator>

                                </div>
                            </div>
                            <div>
                                <label for="TbGrade">
                                    <div class="pagination-right">
                                        <%= Tokens.quali%>
                                </div>
                                </label>
                                <div class="pagination-right">
                                    <asp:TextBox runat="server" ID="TbGrade"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RTbGrade" ErrorMessage="*" ControlToValidate="TbGrade"
                                        runat="server" />
                                </div>
                            </div>
                            <div>
                                <label for="TbSpacific">
                                    <div class="pagination-right">
                                <%= Tokens.spe%></div>
                                </label>
                                &nbsp;<div class="pagination-right">
                                    <asp:TextBox runat="server" ID="TbSpacific" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="RTbSpacific" ErrorMessage="*" ControlToValidate="TbSpacific"
                                        runat="server" />
                                </div>
                            </div>
                            <div>
                                <label for="GradeYear">
                                    <div class="pagination-right">
                                        <%= Tokens.qualidate%></div>
                                </label>
                                &nbsp;<div class="pagination-right">
                                    <asp:TextBox runat="server" ID="GradeYear" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="RGradeYear" ErrorMessage="*" ControlToValidate="GradeYear"
                                        runat="server" />
                                </div>
                            </div>
                            <div>
                                <label for="Tbrent">
                                    <div class="pagination-right">
                                        <%= Tokens.sala%></div>
                                </label>
                                &nbsp;<div class="pagination-centered">
                                    <div class="pagination-right">
                                        <asp:TextBox ID="Tbrent" runat="server" ClientIDMode="Static" />
                                        <asp:RequiredFieldValidator ID="RTbrent" runat="server" ControlToValidate="Tbrent" ErrorMessage="*" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="Tbrent" CssClass="validation" ErrorMessage="RegularExpressionValidator" ValidationExpression="[-+]?[0-9]*\.?[0-9]+"> ادخل الارقام </asp:RegularExpressionValidator>
                                        <br />
                                    </div>

                                    <div>
                                        <label for="startDate">
                                        <div class="pagination-right">
                                            <%= Tokens.hiredate%></div>
                                        </label>
                                        &nbsp;<div class="pagination-centered">
                                            <div class="pagination-right">
                                                <asp:TextBox ID="TextBox5" runat="server"  data-select="dt"></asp:TextBox>
                                               <%-- <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton2" TargetControlID="TextBox5">
                                                </cc1:CalendarExtender>--%>
                                                <%--<asp:ImageButton ID="ImageButton2" runat="server" Height="16px" ImageAlign="Bottom" ImageUrl="../images/calendar.png" Width="16px" />--%>
                                                &nbsp;<asp:RequiredFieldValidator ID="RGradeYear0" runat="server" ControlToValidate="TextBox5" ErrorMessage="*" />
                                            </div>
                                       
                                            <div class="pagination-right">
                                                
                                                <br />
                                                <%= Tokens.wrkdays%><br />
                                                <asp:TextBox ID="days" runat="server" ClientIDMode="Static" />
                                                <asp:RequiredFieldValidator ID="RGradeYear1" runat="server" ControlToValidate="days" ErrorMessage="*" />
                                                <br />
                                                <%= Tokens.ins%><br />
                                                <asp:TextBox ID="ins" runat="server" ClientIDMode="Static" OnTextChanged="ins_TextChanged" />
                                                <asp:RequiredFieldValidator ID="RGradeYear2" runat="server" ControlToValidate="ins" ErrorMessage="*" />
                                                <br />
                                            </div>
                                           <%= Tokens.Attachments%> : 
                                            <table style="width: 100%">
                                                <tr>
                                                    <td class="pagination-right" style="width: 541px; text-align: left">
                                                        <%= Tokens.birthcir%><asp:FileUpload ID="FileUpload1" runat="server" CssClass="textbox" />
                                                      <asp:RegularExpressionValidator runat="server" ForeColor="red" ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|png|PNG|)$" ControlToValidate="FileUpload1"  ErrorMessage="<%$Resources:Tokens,uploadimgerror %>"></asp:RegularExpressionValidator>
                   
                                                        <asp:LinkButton ID="LinkButton4" runat="server" OnClick="LinkButton4_Click"></asp:LinkButton>
                                                    </td>
                                                    <td class="pagination-right"> </td>
                                                </tr>
                                                <tr>
                                                    <td class="pagination-right" style="width: 541px; text-align: left">
                                                       <%= Tokens.militrycir%> <asp:FileUpload ID="FileUpload2" runat="server" CssClass="textbox" />
                                                      <asp:RegularExpressionValidator runat="server" ForeColor="red" ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|png|PNG|)$" ControlToValidate="FileUpload2"  ErrorMessage="<%$Resources:Tokens,uploadimgerror %>"></asp:RegularExpressionValidator>
                 
                                                          <asp:LinkButton ID="LinkButton5" runat="server" OnClick="LinkButton5_Click"></asp:LinkButton>
                                                    </td>
                                                    <td class="pagination-right">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td class="pagination-right" style="width: 541px; text-align: left">
                                                       <%= Tokens.qualicir%>    <asp:FileUpload ID="FileUpload3" runat="server" CssClass="textbox" />
                                                    <asp:RegularExpressionValidator runat="server" ForeColor="red" ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|png|PNG|)$" ControlToValidate="FileUpload3"  ErrorMessage="<%$Resources:Tokens,uploadimgerror %>"></asp:RegularExpressionValidator>
                 
                                                           <asp:LinkButton ID="LinkButton6" runat="server" OnClick="LinkButton6_Click"></asp:LinkButton>
                                                    </td>
                                                    <td class="pagination-right">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td class="pagination-right" style="width: 541px; text-align: left">
                                                       <%= Tokens.NationalId%>  <asp:FileUpload ID="FileUpload4" runat="server" CssClass="textbox" />
                                                      <asp:RegularExpressionValidator runat="server" ForeColor="red" ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|png|PNG|)$" ControlToValidate="FileUpload4"  ErrorMessage="<%$Resources:Tokens,uploadimgerror %>"></asp:RegularExpressionValidator>
                 
                                                         <asp:LinkButton ID="LinkButton7" runat="server" OnClick="LinkButton7_Click"></asp:LinkButton>
                                                        <br />
                                                        <br />
                                                    </td>
                                                    <td class="pagination-right">&nbsp;</td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <fieldset>
                                <legend>  <%= Tokens.attend%>   &  <%= Tokens.go%>  </legend>

                                <div>
                                    <label for="TbAttendance">
                                        <%= Tokens.attend%></label>
                                    <div>
                                        <asp:TextBox runat="server" ID="TbAttendance" ClientIDMode="Static" requierd />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="*" ControlToValidate="TbAttendance"
                                            runat="server" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" CssClass="validation"
                                            ControlToValidate="TbAttendance" ErrorMessage="RegularExpressionValidator" ValidationExpression="[-+]?[0-9]*\.?[0-9]+"> ادخل الارقام </asp:RegularExpressionValidator>

                                    </div>
                                </div>

                                <div>
                                    <label for="TbLeave">
                                         <%= Tokens.go%></label>
                                    <div>
                                        <asp:TextBox runat="server" ID="TbLeave" ClientIDMode="Static" requierd />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="*" ControlToValidate="TbLeave"
                                            runat="server" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" CssClass="validation"
                                            ControlToValidate="TbLeave" ErrorMessage="RegularExpressionValidator" ValidationExpression="[-+]?[0-9]*\.?[0-9]+"> ادخل الارقام </asp:RegularExpressionValidator>

                                    </div>
                                </div>
                                
                                <div>
                                    <label for="TbDiscount">
                                       <%= Tokens.maxdis%></label>
                                    <div>
                                        <asp:TextBox runat="server" ID="TbDiscount" ClientIDMode="Static" requierd />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ErrorMessage="*" ControlToValidate="TbDiscount"
                                            runat="server" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" CssClass="validation"
                                            ControlToValidate="TbDiscount" ErrorMessage="RegularExpressionValidator" ValidationExpression="[-+]?[0-9]*\.?[0-9]+"> ادخل الارقام </asp:RegularExpressionValidator>

                                    </div>
                                </div>


                              <%--  <div class="input-append bootstrap-timepicker">
                                    <input id="timepicker1" type="text" class="input-small">
                                    <span class="add-on"><i class="icon-time"></i></span>
                                </div>

                                <script type="text/javascript">
                                    $('#timepicker1').timepicker();
                                </script>--%>
                            </fieldset>
                            <p>
                                <asp:Button runat="server" CssClass="btn btn-primary" ID="BSave" Text="<%$Resources:Tokens,Save%>" OnClick="b_save_Click">
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


