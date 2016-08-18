function numberOnly(input,msgSpan,msgTxt) {
    $(input).keypress(function (e) {
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            $(msgSpan).html(msgTxt).show().fadeOut("slow");
            return false;
        }
        return true;
    });
}
function numberOnlydecimal(input, msgSpan, msgTxt) {
    $(input).keypress(function (e) {
        if (e.which != 8 && e.which != 0 && e.which != 46 && (e.which < 48 || e.which > 57)) {
            $(msgSpan).html(msgTxt).show().fadeOut("slow");
            return false;
        }
        return true;
    });
}

