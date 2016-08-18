<%@ Page Title="<%$Resources:Tokens,salrep%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="SalaryReports.aspx.cs" Inherits="NewIspNL.Pages.SalaryReports" %>
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

    <div class="content noprint" data-ng-app="salaryreport" data-ng-controller="salaryreportCtrl" data-ng-cloak>
        <div data-ng-hide="loading">
            <fieldset>
                <legend><%=Tokens.salrep%></legend>
                <div class="row-fluid">
               
                    <div class="span6">
                        <div class="well">
                            <div data-ng-show="Employees.length>0">
                                <label for="Employees">
                                    <%=Tokens.Employee%></label>
                                <div class="input-prepend">
                                    <input class="input-mini" ng-model="EmployeeQuery" ng-change="updateEmployeeItemByCode()"
                                        type="text" placeholder="تصفية">
                                    <select id="Employees" class="input-xlarge" ng-model="selectedEmp" ng-options="i.Name for i in Employees|filter:{Name:cquery}">
                                    </select>
                                </div>
                            </div>
                            <div>


                                <div>
                                    <label for="from">
                                        <%=Tokens.Month%></label>
                                    <div>
                                        <select id="Months" class="input-xlarge" ng-model="selectedMonths" ng-options="i.Name for i in Months">
                                        </select>
                                    </div>
                                </div>
                                <div>
                                    <label for="to">
                                        <%=Tokens.Year%></label>
                                    <div>
                                        <select id="Years" class="input-xlarge" ng-model="selectedYears" ng-options="i.Name for i in Years">
                                        </select>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>  </div>
            </fieldset>
        </div>
        <div >
                 <div class="span6">
                        <div style="max-width: 300px; margin: 0 auto 10px;">
                            <button class="btn btn-success btn-large btn-block" type="button" data-ng-click="search()">
                                <i class="icon-white icon-search"></i>&nbsp;   <%=Tokens.Show%>
                            </button>
                        </div>
                    </div>
            <fieldset style="margin-top:150px;">
                <legend><%=Tokens.Results%></legend>
                <div>
                    <table style="text-align: center" class="table table-bordered table-condensed table-hover table-striped white" >
                        <thead>
                            <tr>
                                <th></th>
                                <th><%=Tokens.sala%> 
                                </th>
                                <th><%=Tokens.appc%>
                                </th>
                                <th><%=Tokens.Emptranv%>
                                </th>
                                <th><%=Tokens.empDebits%>
                                </th>
                                <th><%=Tokens.Total%>
                                </th>
                                <th>
                              <%=Tokens.emprew%>
                                 <th> <%=Tokens.empdis%>
                                 </th>
                                <th>الصافى
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr ng-repeat="i in results">
                                <td>{{$index+1}}
                                </td>
                                <td>{{i.Salary}}
                                </td>
                                <td>{{i.TotalAbsences}}
                                </td>
                                <td>{{i.TotalTravels}}
                                </td>
                                <td>{{i.TotalDebits}}
                                </td>
                                <td>{{i.TotalSalary}}
                                </td>
                                <td>{{i.Awards}}
                                </td>
                                <td>{{i.Discounts}}
                                </td>
                                <td>{{i.FullSalary}}
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </fieldset>
        </div>
        <div data-ng-show="loading">
            <div style="width: 30%;" class="progress progress-warning progress-striped active">
                <div class="bar" style="width: 100%;">
                </div>
            </div>
        </div>
    </div>
    <script src="/Scripts/app/salaryreportCtrl.js"></script>
</asp:Content>

