var salaryreport = angular.module('salaryreport', ['ui.date']).directive('wholeNumber', function () {
    return {
        link: function (scope, elem) {
            var num = (parseFloat(elem.val(), 10));
            decimalOnly(elem, num);
            elem.on("blur", function () {
                num = num > (scope.maxValue) || (isNaN(num)) ? 0 : num;
                return num;
            });
            elem.on("change", function () {
                num = num > (scope.maxValue) || (isNaN(num)) ? 0 : num;
                return num;
            });
        }
    };
});

var salaryreportCtrl = function ($scope, $http) {

    $scope.dateFormat = 'd/m/yy';
    $scope.Employees = [];
    $scope.Months = [];
    $scope.Years = [];
    $scope.startDate = new Date;
    $scope.total = 0;
    $scope.totalAbsence = 0;
    $scope.totalDebit = 0;
    $scope.Withdrawals = [];
    $scope.Daypound = false;
    $scope.Halfpound = false;
    $scope.fullrent = 0;
    $scope.totalrent = 0;
    $scope.Discounts = 0;
    $scope.Awards = 0;

    $scope.fetchEmployees = function () {

        $scope.Employees = [];
        $http.get('../Service/HrSvc.svc/Getemployees').success(function (data) {
            var emp = { Id: 0, Name: '', Rent: 0, Mobile: '', Email: '' };
            $scope.Employees.push(emp);
            for (var i = 0; i < data.length; i++) {
                $scope.Employees.push(data[i]);
            }
            $scope.selectedEmp = $scope.Employees[0];
        }).error(function (data) {
            $scope.Employees = data || [];
        });
    };


    $scope.search = function () {
        $scope.results = [];
        var salary = {};
        salary.EmployeeId = $scope.selectedEmp.Id;
        salary.Year = $scope.selectedYears.Id;
        salary.Month = $scope.selectedMonths.Id;
        $http.post('../Service/HrSvc.svc/GetAllSalaries', JSON.stringify(salary)).success(function (data) {
            $scope.results = data;

        }).error(function (data) {

            $scope.results = data || [];
        });
    };

    $scope.filterYear = function (id) {
        $scope.results = [];
        $http.get('../Service/HrSvc.svc/GetAllSalaries/' + id).success(function (data) {
            $scope.results = data;

        }).error(function (data) {

            $scope.results = data || [];
        });
    };

    $scope.updateEmployeeItemByCode = function () {
        var match = _.filter($scope.Employees, function (item) {
            return item.Id.toString() === $scope.EmployeeQuery;
        });
        if (match.length <= 0) {
            $scope.selectedEmp = $scope.Employees[0];

            return;
        }
        $scope.selectedEmp = match[0];
        //$scope.fetchTravel($scope.selectedEmp.Id);
        //$scope.fetchGetWithdrawals($scope.selectedEmp.Id);

        $scope.EmployeeQuery = '';

    };


    $scope.fetchmonth = function () {
        $scope.Months = [];
        var month = { Name: 'January', Id: 1 };
        $scope.Months.push(month);
        month = { Name: 'February', Id: 2 };
        $scope.Months.push(month);
        month = { Name: 'March', Id: 3 };
        $scope.Months.push(month);
        month = { Name: 'April', Id: 4 };
        $scope.Months.push(month);
        month = { Name: 'May', Id: 5 };
        $scope.Months.push(month);
        month = { Name: 'June', Id: 6 };
        $scope.Months.push(month);
        month = { Name: 'July', Id: 7 };
        $scope.Months.push(month);
        month = { Name: 'August', Id: 8 };
        $scope.Months.push(month);
        month = { Name: 'September', Id: 9 };
        $scope.Months.push(month);
        month = { Name: 'October', Id: 10 };
        $scope.Months.push(month);
        month = { Name: 'November', Id: 11 };
        $scope.Months.push(month);
        month = { Name: 'December', Id: 12 };
        $scope.Months.push(month);
        $scope.selectedMonths = $scope.Months[0];
    };

    $scope.fetchYears = function () {
        $scope.Years = [];
        var mydate = new Date();
        var year = mydate.getFullYear();
        for (var i = year - 3; i < year + 6; i++) {
            var ys = { Name: i, Id: i };
            $scope.Years.push(ys);
        }
        $scope.selectedYears = $scope.Years[0];
    };

    $scope.fetchmonth();

    $scope.fetchYears();
    $scope.fetchEmployees();

};