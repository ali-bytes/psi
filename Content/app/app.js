function startDialog(title,keyName) {
    $('#dialog').dialog({
        autoOpen: false,
        width: 600,
        height: 400,
        resizable: false,
        draggable: false,
        title: title,
        modal: true,
        position: 'center',
        buttons: {
            "Ok": function () {
                
                $(this).dialog("close");
            }
        }
    });



    $('#openDialog').click(function () {
        $('#dialog').dialog('open');
        $('.ui-button-text').html(keyName);
        return false;
    });
}

   
