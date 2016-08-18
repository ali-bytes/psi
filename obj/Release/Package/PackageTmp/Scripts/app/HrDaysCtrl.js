var EmpHrApp = angular.module('EmpHrApp', ['ui.date'])

.controller('EmpHrCtrl', function ($scope, $http, $location) {
    $scope.dateFormat = 'd/m/yy';
    $scope.Employees = [];
    $scope.saves = [];
    $scope.States = [];
    $scope.startDate = new Date;

    $scope.fetchEmployees = function() {

        $scope.Employees = [];
        var req = {
            url: '../Service/HrSvc.svc/Getemployees',
            method: "GET",
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            }
    }
        $http.get('../Service/HrSvc.svc/Getemployees').success(function (data) {
            var emp = { Id: 0, Name: 'أخترالموظف' };
            $scope.Employees.push(emp);
            for (var i = 0; i < data.length; i++) {
                $scope.Employees.push(data[i]);
            }
            $scope.selectedEmp = $scope.Employees[0];
        }).error(function () {
            $scope.Employees = [];
        });
    };
  

    $scope.fetchEmpstates = function () {
        $scope.States = [];
        $http.get('../Service/HrSvc.svc/GetemployStates').success(function (data) {
            if (data.length < 0) {
                alert("أدخل البيانات");
                return;
            }
            $scope.States = data;
            $scope.SelectedState = data[0];

        }).error(function (data) {
            $scope.States = data || [];
        });
    };

    $scope.ValidateAddItem = function () {
        //
        //if ($scope.selectedEmp === undefined || $scope.selectedEmp.Id <= 0) {
        //    $scope.validToSave = false;
        //    alert("أختر الموظف ");
        //    $('#Employees').focus();
        //    return;
        //}

        if ($scope.SelectedState === undefined || $scope.SelectedState.Id <= 0) {
            $scope.validToSave = false;
            alert("أختر الحالة ");
            $('#States').focus();
            return;
        }
        if ($scope.time === '' || $scope.time === undefined || $scope.time === 0) {
            $scope.validToSave = false;
            alert("ادخل الوقت ");
            $('#States').focus();
            return;
        }

        if ($scope.startDate === '' || $scope.startDate === undefined) {
            alert("ادخل التاريخ");
            $('#start-date').focus();
            $scope.validToSave = false;
            return;
        }
        $scope.validToSave = true;
    };
    $scope.Save = function () {
        $scope.ValidateAddItem();

        var emp = {};
        //emp.EmployeeId = $scope.selectedEmp.Id;
        emp.EmployeeId = $('#tb_name').val();
        emp.date = GetDMY($scope.startDate);
        emp.Time = $scope.time;
        //emp.StateId = $scope.SelectedState.Id;
        emp.StateId = $('#dbStates').val();


        if ($scope.validToSave === true) {
            $http.post('../Service/HrSvc.svc/SaveDayes', JSON.stringify(emp)).success(function (data) {
                if (data.Id > 0) {
                    alert("تم الحفظ");
                    //$scope.Reset();
                    $scope.canSave = false;
                } else {
                    alert("عفوا يجب تسجيل االحضور اولا او تم تسجيل الحضور او الحضور والانصراف لهذا الموظف  ");
                }

            }).error(function (data) {
                alert("تعذر الحفظ ,اعد المحاولة او اتصل بالدعم الفنى");
                $scope.canPrint = false;
                console.log(data.Error);
                console.log(status);
            });
        }
    };

    $scope.fetchEmpstates();
    $scope.fetchEmployees();

});