

var Rentsapp = angular.module('Rentsapp', ['ui.date']).directive('wholeNumber', function () {
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

var RentsCtrl = function ($scope, $http) {

    $scope.dateFormat = 'd/m/yy';
    $scope.Employees = [];
    $scope.startDate = new Date;
    $scope.total = 0;
    $scope.totalAbsence = 0;
    $scope.totalDebit = 0;
   
    $scope.Withdrawals = [];
    $scope.Absencess = [];
    $scope.Daypound = false;
    $scope.Halfpound = false;
    $scope.fullrent = 0;
    $scope.totalrent = 0;
    $scope.Discounts = 0;
    $scope.Awards = 0;
    $scope.saves = [];
    
    $scope.totaldis = 0;
    $scope.totalrew = 0;
    
    $scope.fetchsaves = function () {

        $scope.saves = [];
        $http.get('../Service/HrSvc.svc/Getsavess').success(function (data) {
            var emp = { Id: 0, Name: 'أخترخزينة', User_id: 0, Save_id: 0 };
            $scope.saves.push(emp);
            for (var i = 0; i < data.length; i++) {
                $scope.saves.push(data[i]);
            }
            $scope.selectedsaves = $scope.saves[0];
        }).error(function () {
            $scope.saves = [];
        });
    };

    $scope.GetTotalRent = function () {
        $scope.totalrent = $scope.selectedEmp.Rent  + $scope.total - $scope.selectedEmp.insurance - $scope.totalDebit - $scope.totalAbsence;
        $scope.fullrent = $scope.totalrent + parseFloat ($scope.totalrew) - parseFloat ($scope.totaldis);
    };

    $scope.fetchEmployees = function () {

        $scope.Employees = [];
        var req = {
            url: '../Service/HrSvc.svc/Getemployees',
            method: "GET",
            headers: {
                'content-type': 'application/json; charset=utf-8'
            }
        }
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

    $scope.updateEmployeeItem = function () {
        if ($scope.selectedEmp === undefined || $scope.selectedEmp.Id <= 0) {
            return;
        }
            $scope.fetchTravel($scope.selectedEmp.Id);

            $scope.fetchAbsences();
            $scope.fetchGetWithdrawals($scope.selectedEmp.Id);
            if ($scope.selectedEmp.EmployeeDayAbs === 2) {
                $scope.Daypound = true;
            }
            if ($scope.selectedEmp.EmployeeHalfAbds === 2) {
                $scope.Halfpound = true;
            }
         $scope.fetchGetreward();
          $scope.fetchGetdis();

          $scope.GetTotalRent();
        

         
    };
    $scope.updateEmployeeItemByCode = function () {
        var match = _.filter($scope.Employees, function (item) {
            return item.Id.toString() === $scope.EmployeeQuery;
        });
        if (match.length <= 0) {
            $scope.selectedEmp = $scope.Employees[0];
            $scope.Reset();
            return;
        }


        $scope.selectedEmp = match[0];
        $scope.totalrent = $scope.selectedEmp.Rent;
        $scope.fullrent = $scope.totalrent;

        $scope.fetchTravel($scope.selectedEmp.Id);

        $scope.fetchAbsences();
        $scope.fetchGetWithdrawals($scope.selectedEmp.Id);
        if ($scope.selectedEmp.EmployeeDayAbs === 2) {
            $scope.Daypound = true;
        }
        if ($scope.selectedEmp.EmployeeHalfAbds === 2) {
            $scope.Halfpound = true;
        }
        $scope.EmployeeQuery = '';
        $scope.totaldis = $scope.fetchGetreward();
        $scope.totalrew = $scope.fetchGetdis();
        $scope.GetTotalRent();
    };

    $scope.fetchAbsences = function () {
        $scope.Absencess = [];
        var bses = {};
        bses.EmployeeId = $scope.selectedEmp.Id;
        bses.Time = GetDMY($scope.startDate);
        $http.post('../Service/HrSvc.svc/GetEmployeeabsences', JSON.stringify(bses)).success(function (data) {
            $scope.Absencess = data;

            for (var i = 0; i < data.length; i++) {
                $scope.totalAbsence += data[i].AbsenceValue;
            }
            $scope.GetTotalRent();
        }).error(function (data) {
            $scope.Absencess = [];
        });
    };
    $scope.fetchTravel = function (id) {
        $scope.travls = [];
        var bses = {};
        bses.EmployeeId = $scope.selectedEmp.Id;
        bses.Time = GetDMY($scope.startDate);
        $http.post('../Service/HrSvc.svc/GetTravels', JSON.stringify(bses)).success(function (data) {
            $scope.travls = data;
            for (var i = 0; i < data.length; i++) {
                $scope.total += data[i].Increase;
            }
            $scope.GetTotalRent();
        }).error(function (data) {
            $scope.travls = data || [];
        });
    };
 


    $scope.fetchGetreward = function () {
       
        var bses = {};
        bses.EmployeeId = $scope.selectedEmp.Id;
        bses.Time = GetDMY($scope.startDate);
        $http.post('../Service/HrSvc.svc/Getrew', JSON.stringify(bses)).success(function (data) {
            $scope.totalrew = data;
            $scope.GetTotalRent();
        }).error(function (data) {
            
        });
    };


    $scope.fetchGetdis = function () {

        var bses = {};
        bses.EmployeeId = $scope.selectedEmp.Id;
        bses.Time = GetDMY($scope.startDate);
        $http.post('../Service/HrSvc.svc/Getdis', JSON.stringify(bses)).success(function (data) {
            $scope. totaldis= data;
            $scope.GetTotalRent();
        }).error(function (data) {

        });
    };

    $scope.fetchGetWithdrawals = function (id) {
        $scope.travls = [];
        var bses = {};
        bses.EmployeeId = $scope.selectedEmp.Id;
        bses.Time = GetDMY($scope.startDate);
        $http.post('../Service/HrSvc.svc/GetWithdrawals', JSON.stringify(bses)).success(function (data) {
            $scope.Withdrawals = data;
            for (var i = 0; i < data.length; i++) {
                $scope.totalDebit += data[i].Debit;
            }
            $scope.GetTotalRent();
        }).error(function (data) {
            $scope.Withdrawals = data || [];
        });
    };

    $scope.ValidateAddItem = function () {
        if ($scope.selectedEmp === undefined || $scope.selectedEmp.Id <= 0) {
            $scope.validToSave = false;
            alert("أختر موظف ");
            $('#Employees').focus();
            return;
        }
        if ($scope.selectedsaves == undefined || $scope.selectedsaves.Id <= 0) {
            $scope.validToSave = false;
            alert("أختر خزينة ");
            $('#save').focus();
            return;
        }
        if ($scope.totalAbsence === undefined) {
            $scope.validToSave = false;
            alert("الغياب ");
            $('#Employees').focus();
            return;
        }
        if ($scope.totalDebit === undefined) {
            $scope.validToSave = false;
            alert("المسحوبات ");
            $('#WithdrawalsTotal').focus();
            return;
        }
        if ($scope.totalDebit === undefined || $scope.totalDebit < 0) {
            $scope.validToSave = false;
            alert("التركيبات والسفريات ");
            $('#total').focus();
            return;
        }

        if ($scope.Discounts === undefined || $scope.Discounts < 0) {
            $scope.validToSave = false;
            alert("الخصومات ");
            $('#Discounts').focus();
            return;
        }
        if ($scope.Awards === undefined || $scope.Awards < 0) {
            $scope.validToSave = false;
            alert("مكافئات ");
            $('#Awards').focus();
            return;
        }
        $scope.validToSave = true;
    };

    $scope.Save = function () {


        $scope.ValidateAddItem();

        var salary = {};
        salary.EmployeeId = $scope.selectedEmp.Id;
        salary.TotalAbsences = $scope.totalAbsence;
        salary.TotalDebits = $scope.totalDebit;
        salary.TotalSalary = $scope.totalrent;
        salary.Salary = $scope.selectedEmp.Rent;
        salary.TotalTravels = $scope.total;
        salary.Awards = $scope.totalrew;
        salary.FullSalary = $scope.fullrent;
        salary.Discounts = $scope.totaldis;
        salary.Year = GetDMY($scope.startDate);
        salary.Month = GetDMY($scope.startDate);
        salary.saveid = $scope.selectedsaves.Save_id;
        
        var hiredate = new Date($scope.selectedEmp.HiringDate);
        var date = new Date(GetDMY($scope.startDate));

       
        $scope.printModel = {};
        $scope.printModel.Id = $scope.selectedEmp.Id;
        $scope.printModel.EmployeeName = $scope.selectedEmp.Name;
        $scope.printModel.Month = salary.Month;
        $scope.printModel.TotalAbsences = $scope.totalAbsence;
        $scope.printModel.Absencess = $scope.Absencess;
        $scope.printModel.totalDebit = $scope.totalDebit;
        $scope.printModel.Withdrawals = $scope.Withdrawals;
        $scope.printModel.travls = $scope.travls;
        $scope.printModel.TotalSalary = salary.totalrent;
        $scope.printModel.Salary = salary.Salary;
        $scope.printModel.Awards = parseFloat($scope.totalrew);
        $scope.printModel.FullSalary = salary.FullSalary;
        $scope.printModel.Discounts = parseFloat($scope.totaldis);
        $scope.printModel.TotalTravels = salary.TotalTravels;



        if (hiredate - date === 0 || hiredate - date > 0) {

            alert("عفوا لا يمكن اختيار المرتب لشهر قبل تاريخ التعيين ");


        }
        else {


            if ($scope.validToSave === true) {
                $http.post('../Service/HrSvc.svc/SaveSalaries', JSON.stringify(salary)).success(function (data) {
                    if (data.Id > 0) {
                        alert("تم الحفظ");
                        //$scope.Reset();
                    }
                    if (data.Id == 0) {
                        alert(data.Msg);
                    }

                }).error(function (data) {
                    alert("تعذر الحفظ ,اعد المحاولة او اتصل بالدعم الفنى");
                    $scope.canPrint = false;
                    //console.log(data.Error);
                    //console.log(status);
                });
            }
        }
    };




    $scope.ShowPrint = function () {
        if ($scope.printModel.Id > 0) {
            $('#A4recipt').show();
        } else {
            alert("لا يوجد امر شغل للطباعة");
        }
    };


    $scope.Print = function () {
        var sec1 = $('#A4psec1');


        $(sec1).addClass('noprint');




        $(sec1).removeClass('noprint');
        window.print();



    };

    //$scope.Reset = function () {

    //    $scope.total = 0;
    //    $scope.totalAbsence = 0;
    //    $scope.totalDebit = 0;
    //    $scope.Withdrawals = [];
    //    $scope.Absencess = [];
    //    $scope.Daypound = false;
    //    $scope.Halfpound = false;
    //    $scope.fullrent = 0;
    //    $scope.totalrent = 0;
    //    $scope.Discounts = 0;
    //    $scope.Awards = 0;
    //};


    $scope.fetchEmployees();
    $scope.fetchsaves();
    $scope.Reset = function () {
        $scope.selectedEmp = $scope.Employees[0];
        $scope.startDate = new Date;
        $scope.total = 0;
        $scope.totalAbsence = 0;
        $scope.totalDebit = 0;
        $scope.Withdrawals = [];
        $scope.Absencess = [];
        $scope.Daypound = false;
        $scope.Halfpound = false;
        $scope.fullrent = 0;
        $scope.totalrent = 0;
        $scope.Discounts = 0;
        $scope.Awards = 0;
        $scope.totaldis = 0;
        $scope.totalrew = 0;
    };
};