
function decimalOnly(input,num) {
    var ticks = { };
    ticks.times = 0;
    ticks.minus = 0;
    $(input).keydown(function (event) {

        if (event.keyCode === 9)
            return;

        var all = $(input).val();
        var times = all.split('.');
        var mins = all.split('-');

        if (all.indexOf('.') === -1) {
            ticks.times = 0;
        }


        if (times.length > 2) {
            $(input).val("0");
            ticks.times = 0;
            num = 0;
            return;
        }
        if (event.keyCode === 189 || event.keyCode === 109) {

            if (mins.indexOf('-') === -1) {
                ticks.minus = 0;
            }

            if (mins.length > 2) {
                $(input).val("0");
                ticks.minus = 0;
                num = 0;
                return;
            }
        }


        if (event.shiftKey) {
            event.preventDefault();
            return;
        }
        if (event.keyCode == 110) {
            if (ticks.times === 0) {
                ticks.times++;

                return;
            } else {
                event.preventDefault();
                return;
            }
        }


        if (event.keyCode == 46 || event.keyCode == 8) {
        } else {
            if (event.keyCode < 95) {
                if (event.keyCode < 48 || event.keyCode > 57) {
                    event.preventDefault();
                    return;
                }
            } else {
                if (event.keyCode < 96 || event.keyCode > 105) {
                    event.preventDefault();
                    return;
                }
            }
        }


    });
}