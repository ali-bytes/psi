<%@ Page Title="<%$Resources:Tokens,attend%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="HrDayes.aspx.cs" Inherits="NewIspNL.Pages.HrDayes" %>

<%@ Import Namespace="Resources" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- HR styles & Scripts -->
    <script src="/Scripts/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.9.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.ui.datepicker-ar.js" type="text/javascript"></script>
    <script src="/Scripts/angular1.1.14.min.js" type="text/javascript"></script>
    <script src="/Scripts/lib/angular-bootstrap.min.js" type="text/javascript"></script>
    <script src="/Scripts/lib/angular-bootstrap-prettify.min.js" type="text/javascript"></script>
    <script src="/Scripts/lib/angular-resource.min.js" type="text/javascript"></script>
    <script src="/Scripts/angular-ui-ieshiv.min.js" type="text/javascript"></script>
    <script src="/Scripts/underscore.min.js" type="text/javascript"></script>
    <script src="/Scripts/app/helpers/validate2.js" type="text/javascript"></script>
    <script src="/Scripts/vendor/angular-ui-custom/angular-ui-ieshiv.min.js" type="text/javascript"></script>
    <script src="/Scripts/vendor/angular-ui-custom/angular-ui.min.js" type="text/javascript"></script>
    <script src="/infraScripts/dateTransformer.js" type="text/javascript"></script>
    <script src="/infraScripts/config.js" type="text/javascript"></script>
    <script src="/infraScripts/date.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/app/HrDaysCtrl.js"></script>
    <style type="text/css">
        .ui-datepicker {
            width: 25em !important;
            left: 60% !important;
        }
    </style>


    <asp:TextBox ID="txtError" runat="server" CssClass="error_box" Width="534px"
        Visible="False" ReadOnly="True"></asp:TextBox>

    <div ng-app="EmpHrApp" ng-controller="EmpHrCtrl" data-ng-cloak>
        <div class="content noprint">
            <fieldset class="noprint">
                <legend><%=Tokens.attend%>  </legend>

                <div class="row-fluid">
                    <div class="well">

                        <div class="input-append">
                            <label for="Employees"><%=Tokens.Employee%> </label>
                            <br />
                            <%--<select id="Employees" class="input-xlarge" ng-model="selectedEmp" ng-options="i.Name for i in  Employees" data-ng-change="updateEmployeeItem()"></select>--%>

                            <asp:DropDownList runat="server" ID="tb_name" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator runat="server" ID="Rtb_name" ControlToValidate="tb_name"
                                ErrorMessage="*" CssClass="validation">*</asp:RequiredFieldValidator>

                        </div>

                    </div>
                </div>


                <div class="input-append">
                    <label for="States"><%=Tokens.Chose%></label><br />
                    <%--<select id="States" class="input-xlarge" ng-model="SelectedState" ng-options="i.Name for i in States"></select>--%>
                     <asp:DropDownList runat="server" ID="dbStates" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="dbStates"
                                ErrorMessage="*" CssClass="validation">*</asp:RequiredFieldValidator>
                </div>



                <br />

                <div style="float: right">
                    <div class="span10">
                        <label>
                            <%=Tokens.Date%><br />
                            <input id="date" ng-model="startDate" ui-date='{ dateFormat: dateFormat }' /></label>
                    </div>

                </div>


                <div style="margin-right: 20%">
                    <label for="time">
                        <%=Tokens.Time%><br />
                        <input type="number" id="time" value="0" ng-model="time" data-ng-change="validateTime()" whole-number />
                    </label>
                </div>




                <div style="margin-top: 60px;">
                    <div style="max-width: 300px; margin: 0 auto 10px;">
                        <button ng-click="Save()" id="save" type="button" class="btn btn-primary btn-large btn-block">
                            <i class="icon-ok icon-white"></i>&nbsp;<%=Tokens.Save%></button>
                    </div>

                </div>
                <input type="hidden" id="savedId" value="0" />
            </fieldset>

            <fieldset>
                <legend><%=Tokens.Upload%> <%=Tokens.attend%></legend>
                <div>
                    <asp:FileUpload ID="FileUploadControl" runat="server" />
                    <asp:RegularExpressionValidator runat="server" ForeColor="red" ValidationExpression="^.*\.(xls|XLS|xlsx|XLSX|)$" ControlToValidate="FileUploadControl" ErrorMessage="<%$Resources:Tokens,uploadfilexcelerror %>"></asp:RegularExpressionValidator>

                </div>
                <%-- <asp:TextBox runat="server" ID="TextBox1" ></asp:TextBox><br/>--%>

                <p>
                    <asp:Button runat="server" CssClass="btn btn-primary" ID="BSave" Text="<%$Resources:Tokens,save%>" OnClick="BSave_Click"></asp:Button>
                    <a href="../ExcelTemplates/HrAttendanceTemplate.xls" class="btn btn-default">
                        <i class="icon-cloud-download bigger-120"></i>
                        <%=Tokens.Downloadsample %>
                    </a>
                </p>
            </fieldset>
        </div>
    </div>

    <%-- <script type="text/javascript">
         $('input[data-select="dt"]').datepicker({
             showOtherMonths: true,
             selectOtherMonths: false,
             dateFormat: 'yy/mm/dd'
         });</script>--%>
</asp:Content>

