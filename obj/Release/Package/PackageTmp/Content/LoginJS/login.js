$('#hdfCulture').val('');

$('#radioArabic').click(function () {
    $('#radioArabic').attr('checked', 'checked');
    $('#radioEnglish').removeAttr('checked');
    $('#hdfCulture').val(1);
});
$('#radioEnglish').click(function () {
    $('#radioEnglish').attr('checked', 'checked');
    $('#radioArabic').removeAttr('checked');
    $('#hdfCulture').val(2);
});
$('#btnRegister').click(function () {
    var error = Validation();
    if (error === 0) {
        return false;
    } else {
        return true;
    }
});

function Validation() {
    var errors = 0;
    var araName = $("#txtArabicName").val();
    var enName = $("#txtEnglishName").val();
    var companyType = $("#ddlCompanyType").val();
    var employeeNumber = $("#ddlEmployeeNumbers").val();
    var companyActivity = $("#txtCompanyActivity").val();
    var companyAddress = $("#txtComapnyAddress").val();
    var companyEmail = $("#txtCompanyEmail").val();
    var companyMobile = $("#txtCompanyMobile").val();
    var resellerName = $("#txtResellerName").val();
    var nationalNumber = $("#txtNationalNumber").val();
    var resellerMobile = $("#txtResellerMobile").val();
    var resellerEmail = $("#txtResellerEmail").val();
    var userName = $("#txtResellerUsername").val();
    var passWord = $("#txtResellerPassword").val();
    var cultur = $("#hdfCulture").val();
    if (araName.length <= 0) {
        $('#txtArabicName').parent().addClass('has-error');
        errors++;
    } if (enName.length <= 0) {
        $('#txtEnglishName').parent().addClass('has-error');
        errors++;
    }
    if (companyType==='نوع المنشأة') {
        $('#ddlCompanyType').parent().addClass('has-error');
        errors++;
    }
    if (employeeNumber === 'عدد الموظفين') {
        $('#ddlEmployeeNumbers').parent().addClass('has-error');
        errors++;
    }
    if (companyActivity.length <= 0) {
        $('#txtCompanyActivity').parent().addClass('has-error');
        errors++;
    }
    if (companyAddress.length <= 0) {
        $('#txtComapnyAddress').parent().addClass('has-error');
        errors++;
    }
   
    if (companyEmail.length <= 0) {
        $('#txtCompanyEmail').parent().addClass('has-error');
        errors++;
    }
    if (companyMobile.length <= 0) {
        $('#txtCompanyMobile').parent().addClass('has-error');
        errors++;
    }
    if (resellerName.length <= 0) {
        $('#txtResellerName').parent().addClass('has-error');
        errors++;
    }
    if (nationalNumber.length <= 0) {
        $('#txtNationalNumber').parent().addClass('has-error');
        errors++;
    }
    if (resellerMobile.length <= 0) {
        $('#txtResellerMobile').parent().addClass('has-error');
        errors++;
    }
    if (userName.length <= 0) {
        $('#txtResellerUsername').parent().addClass('has-error');
        errors++;
    }
    if (cultur.length !== 0&&cultur.length!==1) {
        //$('#radioArabic').parent().addClass('has-error');
        alert('اختر لغة السيستم');
        errors++;
    }
    if (passWord.length <= 0) {
        $('#txtResellerPassword').parent().addClass('has-error');
        errors++;
    }
    if (resellerEmail.length <= 0) {
        $('#txtResellerEmail').parent().addClass('has-error');
        errors++;
    }
    return errors;

}

function clear() {
    $("#txtArabicName").val('');
    $("#txtEnglishName").val(''); $("#ddlCompanyType").val('');
     $("#ddlEmployeeNumbers").val('');
    $("#txtCompanyActivity").val('');
    $("#txtComapnyAddress").val('');
    $("#txtCompanyTele").val('');
    $("#txtfax").val('');
    $("#txtCompanyEmail").val('');
    $("#txtCompanyMobile").val('');
   $("#txtResellerName").val('');
     $("#txtNationalNumber").val('');
     $("#txtResellerMobile").val('');
    $("#txtResellerEmail").val('');
     $("#txtResellerUsername").val('');
     $("#txtResellerPassword").val('');
 }


/* nossair Comment */
$(function () {
    $('#btnbehind').hide();
    $('#btnLogin').click(function () {
        var user = $('#txtUsername').val();
        var pass = $('#txtPassword').val();
        if (user === "" || pass === "") {
            //alert('Enter Full Data');
            if (user === "") $('#txtUsername').parent().addClass('has-error');
            if (pass === "") $('#txtPassword').parent().addClass('has-error');
            $('#btnbehind').hide();
            return false;
        }
        $('#txtUsername').parent().removeClass('has-error');
        $('#txtPassword').parent().removeClass('has-error');
        // $('#btnbehind').show();
        $('#btnbehind').click();
        return true;
    });
});
function show_box(id) {
    jQuery('#' + id).css("display", "block");
    jQuery('#' + id).addClass('visible');
}

function hide_box(id) {
    jQuery('#' + id).css("display", "none");
    jQuery('#' + id).removeClass('visible');
}

