var count = parseInt($('#m-items').val());
for (var i = 0; i < count; i++) {
    var parent = $('li[data-parent="parent-' + i + '"]');
    bindLis($(parent), i);
}

function bindLis(parent, selected) {
    $(parent).bind('click', function () {
        var parentChildren = $('li[data-child="child-' + selected + '"]');
        if ($(parent).attr("data-closed") === "true") {
            $(parent).attr("data-closed", "false");
            $(parentChildren).show("slow");
        } else {
            $(parent).attr("data-closed", "true");
            $(parentChildren).hide("slow");
        }
    });
}