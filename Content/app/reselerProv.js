var checks = $('span[data-select="cb"] > input');
var selectAll = $('#chec');
$(selectAll).bind('change', function () {
    console.log($(this).is(':checked'));
    if ($(this).is(':checked')) {
        $(checks).attr("checked", "checked");
    } else {
        $(checks).removeAttr("checked");
    }
});
        