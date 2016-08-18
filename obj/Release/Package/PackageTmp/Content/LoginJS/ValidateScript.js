var validateScript = function () {
    var validate = function validateMethod(divVlaidate) {

        var alerMessage = "";

        divVlaidate.find('*[data-validate="required"]').each(function () {


            //Required
            if ($(this).data("validate-type") == 'required') {
                if ($(this).val().length == 0) {
                    $(this).parent().parent().addClass("has-error");
                    var label = $(this).parent().parent().find("label");
                    var name;
                    if (label.text() != "")
                        name = label.text();
                    else 
                        name = $(this).prop("placeholder");
                    
                    if ($.cookie("_culture") == "ar-EG" )
                        alerMessage += "حقل " + name + " مطلوب </br>";
                    
                    else
                        alerMessage += name + " field " + " is required </br>";

                } else {
                    if ($(this).parent().parent().hasClass("has-error")) {
                        $(this).parent().parent().removeClass("has-error");
                    }
                }
            }
            //Required
            if ($(this).data("validate-type") == 'requiredform') {
                if ($(this).val().length == 0) {
                    $(this).parent().parent().addClass("has-error");
                    var label = $(this).parent().parent().find("label");
                    var name;
                    if (label.text() != "")
                        name = label.text();
                    else
                        name = $(this).prop("placeholder");
                        alerMessage += name + " field " + " is required </br>";

                } else {
                    if ($(this).parent().parent().hasClass("has-error")) {
                        $(this).parent().parent().removeClass("has-error");
                    }
                }
            }

            //email
            if ($(this).data("validate-type") == 'email') {
                var re = /[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}/igm;

                if ($(this).val().length == 0 || (!re.test($(this).val()))) {
                    $(this).parent().parent().addClass("has-error");
                    var emailLabel = $(this).parent().parent().find("label");
                    var emailname;
                    if (emailLabel.text() != "")
                        emailname = emailLabel.text();
                    else
                        emailname = $(this).prop("placeholder");

                    if ($.cookie("_culture") == "ar-EG")
                        alerMessage += "أدخل " + emailname + " بشكل صحيح </br>";
                    else
                        alerMessage += " Insert correct " + emailLabel.text() +"</br>";

                } else {
                    if ($(this).parent().parent().hasClass("has-error")) {
                        $(this).parent().parent().removeClass("has-error");
                    }
                }
            }

            //Phone
            if ($(this).data("validate-type") == 'Phone') {
                var re = /[0-9]\d$/igm;

                if ($(this).val().length == 0 || (!re.test($(this).val()))) {
                    $(this).parent().parent().addClass("has-error");
                    var PhoneLabel = $(this).parent().parent().find("label");
                    var Phonename;
                    if (PhoneLabel.text() != "")
                        Phonename = PhoneLabel.text();
                    else
                        Phonename = $(this).prop("placeholder");

                    if ($.cookie("_culture") == "ar-EG")
                        alerMessage += "أدخل " + Phonename + " بشكل صحيح    </br>";
                    else
                        alerMessage += " Insert correct " + PhoneLabel.text() + "</br>";

                } else {
                    if ($(this).parent().parent().hasClass("has-error")) {
                        $(this).parent().parent().removeClass("has-error");
                    }
                }
            }
            // ^((http:\/\/www\.)|(www\.)|(http:\/\/))[a-zA-Z0-9._-]+\.[a-zA-Z.]{2,5}$
            //Website
            if ($(this).data("validate-type") == 'Website') {
                var re = /^((http:\/\/www\.)|(www\.)|(http:\/\/))[a-zA-Z0-9._-]+\.[a-zA-Z.]{2,5}$/igm;

                if ($(this).val().length == 0 || (!re.test($(this).val()))) {
                    $(this).parent().parent().addClass("has-error");
                    var PhoneLabel = $(this).parent().parent().find("label");
                    var Phonename;
                    if (PhoneLabel.text() != "")
                        Phonename = PhoneLabel.text();
                    else
                        Phonename = $(this).prop("placeholder");

                    if ($.cookie("_culture") == "ar-EG")
                        alerMessage += "أدخل " + Phonename + " بشكل صحيح   </br>";
                    else
                        alerMessage += " Insert correct " + PhoneLabel.text() + "</br>";

                } else {
                    if ($(this).parent().parent().hasClass("has-error")) {
                        $(this).parent().parent().removeClass("has-error");
                    }
                }
            }
            //ddl
            if ($(this).data("validate-type") == 'ddl') {
                if ($(this).val() == -1) {
                    $(this).parent().parent().addClass("has-error");
                    var ddlLabel = $(this).parent().parent().find("label");
                    if ($.cookie("_culture") == "ar-EG")
                        alerMessage += "اختر قيمة فى حقل " + ddlLabel.text() + " </br>";
                    else
                        alerMessage += "Select value in " + ddlLabel.text() + " field " + "</br>";

                } else {
                    if ($(this).parent().parent().hasClass("has-error")) {
                        $(this).parent().parent().removeClass("has-error");
                    }
                }
            }


        });


        if (alerMessage.length == 0) {
            if (!$("#divWarning").hasClass("invisible")) {
                $("#divWarning").addClass("invisible");

            }
        } else {
            if ($("#divWarning").hasClass("invisible"))
                $("#divWarning").removeClass("invisible");
            $("#divWarning").html(alerMessage);

        }

        ;


    };
    return {
        validate: validate
    };
}();