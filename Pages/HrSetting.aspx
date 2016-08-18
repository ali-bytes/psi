<%@ Page Title="<%$Resources:Tokens,empsettings%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="HrSetting.aspx.cs" Inherits="NewIspNL.Pages.HrSetting" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <script type="text/javascript">
      //$('#TbAbsDaySelect').change(function () {
      //    if ( $('#AbsDaySelectradio').is(":checked"))
      //    {
      //        $('#AbsDayvalue').val('1');
      //    }
      //});
      //$('#AbsDayPound').change( function () {
      //    if ($('#AbsDayPoundradio').is(":checked"))
      //    $('#AbsDayvalue').val('2');

      //});

      //$('#TbHalfDaySelect').change(function () {
      //    if ($('#TbHalfDaySelectRadio').is(":checked"))
      //    $('#HalfDayValue').val('1');

      //});

      //$('#TbHalfDaySelectRadio').change(function () {
      //    if ($("#TbHalfDaySelectRadio").is(":checked")) {
      //        $('#HalfDayValue').val('1');
      //    }
      //});

      //$('#HalfDayText').change( function () {
      //    if (  $('#HalfDayTextradio').is(":checked"))
      //    $('#HalfDayValue').val('2');

      //});
      //$('#TbHalfDaySelect').change( function () {
      //    if ( $('#TbHalfDaySelectRadio').is(":checked"))
      //    $('#HalfDayValue').val('1');

      //});

      //$('#TbAddHourText').change( function () {
      //    if ( $('#TbAddHourTextRadio').is(":checked"))
      //    $('#AddValue').val('1');

      //});
      //$('#TbAddPoundText').change( function () {
      //    if ($('#AddPoundTextradio').is(":checked"))
      //    $('#AddValue').val('2');

      //});

      //if (document.getElementById("AbsDaySelectradio").checked) {
      //    document.getElementById("AbsDayPoundradio").checked = false;
      //    document.getElementById("AbsDayPound").disabled = false;
      //}
      //if (document.getElementById("AbsDayPoundradio").checked) {
      //    document.getElementById("AbsDaySelectradio").checked = false;
      //    document.getElementById("TbAbsDaySelect").disabled = false;
      //}
    </script>
   


    
    
    
    <asp:TextBox ID="txtError" runat="server" CssClass="error_box" Width="534px"  Visible="False" ReadOnly="True" ></asp:TextBox>
           
      <div class="content">
        <div class="alert alert-success" style="display: <%=l_message.Text==string.Empty?"none;":"block;"%>">
            <asp:Label runat="server" ID="l_message" ClientIDMode="Static"></asp:Label>
        </div>
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="v_index" runat="server">
                <fieldset>
                    <legend>  <%= Tokens.empsettings %></legend>
                    <div>
                        <div>
                            <p>
                                <asp:Button ID="b_new" runat="server" Text="<%$Resources:Tokens,Add%>" OnClick="b_new_OnClick" CssClass="btn btn-primary" />
                            </p>
                            <asp:GridView style="text-align: center"  CssClass="table table-bordered table-hover" ID="GvItems" runat="server" AutoGenerateColumns="False" OnDataBound="GvItems_OnDataBound" ClientIDMode="Static"
                                GridLines="None">
                                <AlternatingRowStyle BorderStyle="None" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="l_number" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="<%$Resources:Tokens,Name%>" DataField="Name"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$Resources:Tokens,dayapp%>" DataField="EmployeeDayAbsValue"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$Resources:Tokens,late%>" DataField="EmployeeHalfAbdsValue"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$Resources:Tokens,over%>" DataField="EmployeeAddValue"></asp:BoundField>


                                    <asp:TemplateField HeaderText="<%$Resources:Tokens,Update%>" >
                                        <ItemTemplate>
                                            <asp:Button ID="gvb_edit" runat="server" CommandArgument='<%#Bind("Id") %>' OnClick="gvb_edit_OnClick" Text="<%$Resources:Tokens,update%>" CssClass="btn btn-primary" Width="60px"/>
                                            <%-- <asp:Button ID="gvb_delete" runat="server" CommandArgument='<%# Bind("Id") %>' Text="حذف"
                                                OnClick="gvb_delete_Click" CssClass="btn btn-danger" />--%>
                                            <%--Visible='<%# Bind("CanDelete") %>'--%>
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
                        <legend><%= Tokens.empsettings %></legend>
                        <asp:HiddenField ID="hf_id" runat="server" />
                        <asp:HiddenField ID="AbsDayvalue" runat="server" Value="1" />
                        <asp:HiddenField ID="HalfDayValue" runat="server" Value="1" />
                        <asp:HiddenField ID="AddValue" runat="server" Value="1" />
                        <div class="well">
                            <div>
                                <label for="TbEmployeeSelect">
                                    <asp:Literal ID="Literal3" Text=" <%$Resources:Tokens,Employee%>" runat="server" />
                                </label>
                                <div>
                                    <asp:DropDownList runat="server" ID="TbEmployeeSelect" />
                                    <asp:RequiredFieldValidator runat="server" ID="r_TbEmployeeSelect" ValidationGroup="box" ControlToValidate="TbEmployeeSelect" ErrorMessage="*" CssClass="validation"/>أختر الموظف
                                </div>
                            </div>
                            <fieldset>
                                <legend><%=Tokens.appc%></legend>
                                <div class="btn-group row-fluid" >
                                    <div>

                                    <%--    <label for="TbAbsDaySelect">
                                            <input type="radio" name="AbsDay" id="AbsDaySelectradio" checked="checked" value="1" />الغياب
                                        </label>--%>
                                        <asp:RadioButtonList ID="RadioButtonList1"  runat="server">
                                            
                                            <asp:ListItem Text="<%$Resources:Tokens,perday%>" Selected="True" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="<%$Resources:Tokens,perpound%>" Value="2"></asp:ListItem>
                                        </asp:RadioButtonList>

                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<div>
                                       <%--     <asp:DropDownList runat="server" ID="TbAbsDaySelect" Visible="False">
                                                <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                                <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                            </asp:DropDownList>--%>
                                        </div>
                                    </div>

                                    <div>
                                        <%--<label for=AbsDayPoundd">
                                            <input type="radio" name="AbsDay" id="AbsDayPoundradio" value="2">الخصم بالجنية
                                        </label>--%>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<div>
                                            <asp:TextBox runat="server" ID="AbsDayPound" ClientIDMode="Static"  type="number" />
                                       
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="AbsDayPound"
                                                ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="box"></asp:RequiredFieldValidator>

                                             </div>
                                    </div>

                                </div>
                            </fieldset>
                            <fieldset>
                                <div style="margin-top: 100px;"> <legend style="margin-left: 150px"><%=Tokens.late%></legend>
                                <div class="btn-group" >
                                   

                                       <%-- <label for="TbHalfDaySelect">
                                            <input type="radio" name="HalfDay" id="TbHalfDaySelectRadio" checked="checked" value="1"/>الساعة
                                    
                                        </label>--%>
                                       <asp:RadioButtonList ID="RadioButtonList2"  runat="server">
                                            
                                            <asp:ListItem Text="<%$Resources:Tokens,perhour%>" Selected="True" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="<%$Resources:Tokens,perpound%>" Value="2"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        &nbsp;&nbsp;&nbsp;<div>
                                            <asp:TextBox runat="server"  ID="TbHalfDaySelect" ClientIDMode="Static" type="number" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TbHalfDaySelect"
                                                ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="box"></asp:RequiredFieldValidator>

                                             </div>
                                    </div>


                                    <div>


                                 <%--       <label for="AbsDay">
                                            <input type="radio" name="HalfDay" id="HalfDayTextradio" value="2" />الجنية</label>--%>
                           <%--             <div>
                                            <asp:TextBox runat="server" Visible="False" ID="HalfDayText" ClientIDMode="Static" type="number" />

                                        </div>--%>
                                    </div>

                                </div>

                            </fieldset>

                            <fieldset>
                                <legend><%=Tokens.over%></legend>
                                <div class="btn-group" >
                                    <div>

                                        <%--<label for="TbAddHourText">
                                            <input type="radio" name="AddDay" id="TbAddHourTextRadio" checked="checked" value="1" />الساعة
                                    
                                        </label>--%>
                                         <asp:RadioButtonList ID="RadioButtonList3"  runat="server">
                                            
                                            <asp:ListItem Text="<%$Resources:Tokens,perhour%>" Selected="True" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="<%$Resources:Tokens,perpound%>" Value="2"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<div>
                                            <asp:TextBox runat="server"  ID="TbAddHourText" ClientIDMode="Static" type="number" />
                                       
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TbAddHourText"
                                                ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="box"></asp:RequiredFieldValidator>
                                            
                                             </div>
                                    </div>
                                    <br/>

                                    <div>


                                      <%--  <label for="TbAddPoundText">
                                            <input type="radio" name="AddDay" id="AddPoundTextradio" value="2" />الجنية</label>--%>
                                        <%--<div>
                                            <asp:TextBox runat="server" Visible="False" ID="TbAddPoundText" ClientIDMode="Static" type="number" />

                                        </div>--%>
                                    </div>

                                </div>

                            </fieldset>

                            <p>
                                <asp:Button runat="server" CssClass="btn btn-primary" ValidationGroup="box" ID="BSave" Text="<%$Resources:Tokens,save%>" OnClick="BSave_OnClick">
                                    <%--  <i class="icon-white icon-ok"></i>&nbsp;حفظ--%></asp:Button>
                                <span>| </span>
                                <asp:Button runat="server" ID="BBack" CssClass="btn btn-success" CausesValidation="False" OnClick="BBack_OnClick" Text="<%$Resources:Tokens,Cancel%>" />
                            </p>
                        </div>
                    </fieldset>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>

</asp:Content>


    

