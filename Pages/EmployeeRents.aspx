<%@ Page Title="<%$Resources:Tokens,sal%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="EmployeeRents.aspx.cs" Inherits="NewIspNL.Pages.EmployeeRents" %>
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

    <div ng-app="Rentsapp" ng-controller="RentsCtrl" data-ng-cloak>
        
         <div class="content noprint">
             
            <fieldset class="noprint">
                <legend><%=Tokens.saladata%></legend>
                <div class="accordion" id="accordion2">
                    <div class="accordion-group">
                        <div class="accordion-heading">
                            <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#collapseOne">
                                <h4><%=Tokens.empdata%> </h4>
                            </a>
                        </div>
                        <div id="collapseOne" class="accordion-body collapse in">
                            <div class="accordion-inner">
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="alert alert-danger" data-ng-show="selectedEmp.ShowMessage">
                                            {{selectedEmp.Msg}}
                                        </div>
                                        <div class="alert alert-success">
                                            <table>
                                                <tr>
                                                    <td><%=Tokens.Name%>
                                                    </td>
                                                    <td>:&nbsp;
                                                    </td>
                                                    <td>{{selectedEmp.Name}}
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><%=Tokens.sala%>
                                                    </td>
                                                    <td>:&nbsp;
                                                    </td>
                                                    <td>{{selectedEmp.Rent|number:2}}
                                                    </td>
                                                </tr>
                                                <%--   <tr>
                                                    <td>خصم غياب
                                                    </td>
                                                    <td>:&nbsp;
                                                    </td>
                                                    <td ng-show="!Daypound">
                                                    {{selectedEmp.EmployeeDayAbsValue}}<td ng-show="Daypound">{{selectedEmp.EmployeeDayAbsValue|number:2}}
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>خصم غياب نصف اليوم
                                                    </td>
                                                    <td>:&nbsp;
                                                    </td>
                                                    <td ng-show="!Halfpound">
                                                    {{selectedEmp.EmployeeHalfAbdsValue}}<td ng-show="Halfpound">{{selectedEmp.EmployeeHalfAbdsValue|number:2}}
                                                    </td>
                                                </tr>--%>
                                            </table>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="well">
                                            <div>
                                                <label for="startDate">
                                                    <%=Tokens.Date%></label>
                                                <div>
                                                    <input  type="text"
                                                        id="startDate" ng-model="startDate" ui-date='{ dateFormat: dateFormat }' ui-date ui-date-format="dd/mm/yy" />
                                                    <br />
                                                <label for="Employees">
                                                    <%=Tokens.Employee%></label></div>
                                            </div>
                                            <div data-ng-show="Employees.length>0">
                                                <div class="input-prepend">
                                                    <input class="input-mini" ng-model="EmployeeQuery" ng-change="updateEmployeeItemByCode()"
                                                        type="text" placeholder="تصفية">
                                                    <select id="Employees" class="input-xlarge" ng-model="selectedEmp" ng-options="i.Name for i in Employees|filter:{Name:cquery}" data-ng-change="updateEmployeeItem()" style="width:194px; top: 0px; left: 0px;">
                                                    </select>
                                                    <br />
                                                </div>
                                            </div>

                                            <div>
                                                <%=Tokens.UserSaves%><br />
                                            
                                                &nbsp;<select id="Employees0" class="input-xlarge" ng-model="selectedsaves" ng-options="i.savename for i in saves|filter:{Save_id:cquery}"  name="D1">
                                                    </select></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="accordion-group">
                        <div class="accordion-heading">
                            <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#collapseFive">
                                <h4><%=Tokens.appcdata%> </h4>
                            </a>
                        </div>
                        <div id="collapseFive" class="accordion-body collapse in">
                            <div class="accordion-inner">
                                <div>
                                    <div>

                                        <div>
                                            <hr />
                                            <table id="Absencess-table" class="table table-bordered table-hover table-condensed table-striped white">
                                                <thead>
                                                    <tr>
                                                        <th></th>
                                                        <th><%=Tokens.appc%>
                                                        </th>
                                                        <th><%=Tokens.Value%> 
                                                        </th>
                                                        <th><%=Tokens.Date%>
                                                        </th>

                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr ng-repeat="i in Absencess">
                                                        <td>{{$index+1}}
                                                        </td>
                                                        <td>{{i.AbsenceType}}
                                                        </td>
                                                        <td>{{i.AbsenceValue|number:2}}
                                                        </td>
                                                        <td>{{i.Time}}
                                                        </td>


                                                    </tr>
                                                </tbody>
                                            </table>
                                            <div id="sum" class="alert alert-success span3 pull-right">
                                                <strong><%=Tokens.Total%></strong> : <strong id="absencestotal">{{totalAbsence|number:2}}</strong>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="accordion-group">
                        <div class="accordion-heading">
                            <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#collapseTwo">
                                <h4><%=Tokens.travdata%> </h4>
                            </a>
                        </div>
                        <div id="collapseTwo" class="accordion-body collapse in">
                            <div class="accordion-inner">
                                <div>
                                    <div>

                                        <div>
                                            <hr />
                                            <table id="items-table" class="table table-bordered table-hover table-condensed table-striped white">
                                                <thead>
                                                    <tr>
                                                        <th></th>
                                                        <th><%=Tokens.Type%>
                                                        </th>
                                                        <th><%=Tokens.Value%> 
                                                        </th>
                                                        <th><%=Tokens.Date%>
                                                        </th>

                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr ng-repeat="i in travls">
                                                        <td>{{$index+1}}
                                                        </td>
                                                        <td>{{i.Type}}
                                                        </td>
                                                        <td>{{i.Increase|number:2}}
                                                        </td>
                                                        <td>{{i.Time}}
                                                        </td>


                                                    </tr>
                                                </tbody>
                                            </table>
                                            <div id="sum" class="alert alert-success span3 pull-right">
                                                <strong><%=Tokens.Total%></strong> : <strong id="total">{{total|number:2}}</strong>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="accordion-group">
                        <div class="accordion-heading">
                            <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#collapseThree">
                                <h4><%=Tokens.Debitsdata%> </h4>
                            </a>
                        </div>
                        <div id="collapseThree" class="accordion-body collapse in">
                            <div class="accordion-inner">
                                <div>
                                    <div>

                                        <div>
                                            <hr />
                                            <table id="Withdrawals-table" class="table table-bordered table-hover table-condensed table-striped white">
                                                <thead>
                                                    <tr>
                                                        <th></th>

                                                        <th><%=Tokens.Value%> 
                                                        </th>
                                                        <th><%=Tokens.Date%>
                                                        </th>

                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr ng-repeat="i in Withdrawals">
                                                        <td>{{$index+1}}
                                                        </td>

                                                        <td>{{i.Debit|number:2}}
                                                        </td>
                                                        <td>{{i.Time}}
                                                        </td>


                                                    </tr>
                                                </tbody>
                                            </table>
                                            <div id="sumWithdrawal" class="alert alert-success span3 pull-right">
                                                <strong><%=Tokens.Total%></strong> : <strong id="WithdrawalsTotal">{{totalDebit|number:2}}</strong>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                    <div class="accordion-group">
                        <div class="accordion-heading">
                            <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#collapseFour">
                                <h4><%=Tokens.saladata%></h4>
                            </a>
                        </div>
                        <div id="collapseFour" class="accordion-body collapse in">
                            <div class="accordion-inner">
                                <div id="tab-details4">
                                    <div class="row-fluid">
                                        <div class="span6">
                                                    <label for="paid">
                                                       <%=Tokens.ins%>&nbsp;
                                            <table>
                                                <tr>
                                                    <td>{{selectedEmp.insurance}}
                                                    </td>
                                                </tr>
                                            </table>
                                        &nbsp;</label></div>

                                        <div class="span6">
                                            <div class="alert alert-danger">
                                                <h3>
                                                    <span><%=Tokens.Total%> <%=Tokens.sala%></span>:&nbsp; <span id="totalrent">{{totalrent|number:2}} </span>
                                                </h3>

                                        <div class="span6">
                                        </div>

                                            </div>
                                        </div>
                                    </div>
                                    <div class="row-fluid">

                                        <div class="span6">
                                            <div class="well">
                                                <div class="row-fluid">

                                                    <div class="span10">
                                                        <label>
                                                            <%=Tokens.empdis%>&nbsp;<br />
                                                        <strong id="WithdrawalsTotal2">{{totaldis|number:2}}</strong></label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="span6">
                                            <div class="well">
                                                <div>
                                                    <label for="paid">
                                                         <%=Tokens.emprew%>&nbsp;
                                                        <br />
                                                    <strong id="WithdrawalsTotal1">{{totalrew|number:2}}</strong>
                                                    </label>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row-fluid">
                                        <div class="span6">
                                            <div class="alert alert-success">
                                                <h3>
                                                    <span> <%=Tokens.Net%></span>:&nbsp; <span id="total">{{fullrent|number:2}}</span></h3>
                                            </div>
                                        </div>

                                        <div class="span4">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div>
                        <div style="max-width: 300px; margin: 0 auto 10px;">
                            <button ng-click="Save()" id="save" type="button" class="btn btn-primary btn-large btn-block">
                                <i class="icon-ok icon-white"></i>&nbsp; <%=Tokens.Save%></button>
                        </div>
                        <p>
                            <button ng-click="ShowPrint()" type="button" class="btn btn-success">
                                <i class="icon-white icon-print"></i>&nbsp;<%=Tokens.Reciept%></button>
                        </p>
                    </div>
                    <input type="hidden" id="savedId" value="0" />
                </div>
            </fieldset>
        </div>



        <div id="A4recipt">
            <div>
                <div id="A4psec1">
                    <center>
                        <h4><%=Tokens.sala%></h4>
                    </center>
                    <div>
                        <div class="Aspan4">


                            <table>

                                <tr>

                                    <td style="width: 80px; padding: 5px 0;"><%=Tokens.Employeenum%> 
                                    </td>
                                    <td>:
                                    </td>
                                    <td class="Aspan1">{{printModel.Id}} 
                                    </td>

                                    <td style="width: 80px; padding: 5px 0;"><%=Tokens.Date%>
                                    </td>
                                    <td>:
                                    </td>
                                    <td class="Aspan1">{{printModel.Month}}
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 80px; padding: 5px 0;"><%=Tokens.Employee%>
                                    </td>
                                    <td>:
                                    </td>
                                    <td class="Aspan2">{{printModel.EmployeeName}}
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td style="width: 80px; padding: 5px 0;"><%=Tokens.sala%>
                                    </td>
                                    <td>:
                                    </td>
                                    <td class="Aspan2">{{selectedEmp.Rent|number:2}}
                                    </td>
                                </tr>
                            </table>

                        </div>

                        <h4><%=Tokens.appcdata%>  </h4>

                        <div class="Aspan4">
                            <div>

                                <table id="Table1">
                                    <thead>
                                        <tr>
                                            <td class="Aspan2"> <%=Tokens.appc%></td>
                                            <td class="Aspan1"><%=Tokens.Value%></td>
                                            <td class="Aspan1"><%=Tokens.Date%></td>

                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr ng-repeat="i in printModel.Absencess">
                                            <td class="Aspan2">{{i.AbsenceType}}
                                            </td>
                                            <td class="Aspan1">{{i.AbsenceValue|number:2}}
                                            </td>
                                            <td class="Aspan1">{{i.Time}}
                                            </td>


                                        </tr>
                                        <tr>
                                            <td class="Aspan2"><%=Tokens.Total%>
                                            </td>

                                            <td class="Aspan1"></td>
                                            <td class="Aspan1"></td>
                                            <td class="Aspan1"></td>
                                            <td class="Aspan1"></td>
                                            <td class="Aspan1"></td>
                                            <td class="Aspan2">{{printModel.TotalAbsences|number:2}}
                                            </td>
                                        </tr>
                                    </tbody>

                                </table>
                          
                            </div>
                        </div>

                        <h4><%=Tokens.travdata%></h4>

                        <div class="Aspan4">
                            <div>

                                <table id="Table4">
                                    <thead>
                                        <tr>
                                            <td class="Aspan2"><%=Tokens.Type%></td>
                                            <td class="Aspan1"><%=Tokens.Value%></td>
                                            <td class="Aspan1"><%=Tokens.Date%></td>

                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr ng-repeat="i in printModel.travls">
                                            <td class="Aspan2">{{i.Type}}
                                            </td>
                                            <td class="Aspan1">{{i.Increase|number:2}}
                                            </td>
                                            <td class="Aspan1">{{i.Time}}
                                            </td>


                                        </tr>
                                        <tr>
                                            <td class="Aspan2"><%=Tokens.Total%>
                                            </td>

                                            <td class="Aspan1"></td>
                                            <td class="Aspan1"></td>
                                            <td class="Aspan1"></td>
                                            <td class="Aspan1"></td>
                                            <td class="Aspan1"></td>
                                            <td class="Aspan2">{{printModel.TotalTravels|number:2}}
                                            </td>
                                        </tr>
                                    </tbody>

                                </table>
                               

                            </div>
                        </div>

                        <h4><%=Tokens.Debitsdata%></h4>

                        <div class="Aspan4">
                            <div>

                                <table id="Table5">
                                    <thead>
                                        <tr>

                                            <td class="Aspan1"><%=Tokens.Value%></td>
                                            <td class="Aspan1"><%=Tokens.Date%></td>

                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr ng-repeat="i in printModel.Withdrawals">

                                            <td class="Aspan1">{{i.Debit|number:2}}
                                            </td>
                                            <td class="Aspan1">{{i.Time}}
                                            </td>


                                        </tr>
                                        <tr>
                                            <td class="Aspan2"><%=Tokens.Total%>
                                            </td>

                                            <td class="Aspan1"></td>
                                            <td class="Aspan1"></td>
                                            <td class="Aspan1"></td>
                                            <td class="Aspan1"></td>
                                            <td class="Aspan1"></td>
                                            <td class="Aspan2">{{printModel.totalDebit|number:2}}
                                            </td>
                                        </tr>
                                    </tbody>

                                </table>
                                <table>

                                    <tr>
                                        
                                        <td style="width: 80px"><%=Tokens.ins%>
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="Aspan2">{{selectedEmp.insurance}}
                                        </td>
                                        

                                        <td style="width: 80px"><%=Tokens.empdis%>
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="Aspan2">{{totaldis|number:2}}
                                        </td>
                                        <td style="width: 80px"><%=Tokens.emprew%>
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="Aspan2">{{totalrew|number:2}}
                                        </td>
                                        <td style="width: 80px"><%=Tokens.Net%>
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="Aspan2">{{printModel.FullSalary|number:2}}
                                        </td>
                                    </tr>
                                    </table>

                            </div>
                        </div>

                    </div>
                </div>

            </div>

            <div class="noprint">
                <button type="button" ng-click="Print()" class="btn btn-success">
                    <i class="icon-white icon-print"></i>&nbsp;<%=Tokens.Print%></button>
            </div>
        </div>
 

        <script src="/Scripts/app/RentsCtrl.js"></script>

    </div>
</asp:Content>
