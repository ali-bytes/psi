/* File Created: أبريل 19, 2015 */

//document.onreadystatechange = function () {
//    //ace.switch_direction(jQuery);
//    //to change language and direction
//    if (document.readyState === 'complete') {

//        var val = $('#hfRtl').val();
//        if (val == "1") {
//            //arabic
//            ace.switch_direction(jQuery);
//            $('#pTitle').css({ "direction": "ltr" });

//        } else {
//            //$('#ace-settings-rtl').prop('checked', false);
//            $('.nav-list>li>a').css({ 'font-size': '13px' });
//        }
//    }
//};
jQuery(function ($) {
    //Ahmed Saied Custome JQuery
    //ali comment
    $('#PhoneNumber').keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            $('#SearchCustomer').click();
            return false;
        } else {
            return true;
        }
    });
    $('[data-rel=tooltip]').tooltip();
    //$('[data-rel="tooltip"]').tooltip({ placement: tooltip_placement });
    //ali comment
    $('#pTitle').html($('title').html());

    $('input[multiple]').ace_file_input({
        style: 'well',
        btn_choose: 'Drop files here or click to choose',
        btn_change: null,
        no_icon: 'icon-cloud-upload',
        droppable: true,
        thumbnail: 'small'//large | fit
    });
    $('input[type="file"]').ace_file_input({
        no_file: 'No File ...',
        btn_choose: 'Choose',
        btn_change: 'Change',
        droppable: false,
        onchange: null,
        thumbnail: false //| true | large

    });

    $('input[type="file"]').bind('change', function () {


        var fileSize = this.files[0].size / 1024 / 1024;


        if (fileSize > 4) {//&& fileExt == ".png"

            alert('حجم الملف لا يجب ان يتجاوز ال 4 ميجا');
            $('a[class="remove"]').click();


        }
    });
});

$(document).ready(function () {
    //$('#btnAdd').hide();
    //ali comment
    //$("td[data-date]").click(function () {
    //    if ($(this).find(".fc-event").length > 0) {
    //        return true;
    //    }
    //    var date = $(this).attr("data-date");
    //    $('#hdnFrom').val(date);
    //    //$('#hdnTo').val(date);
    //    $('#addEvent').modal('show');
    //    return false;
    //});
    //$("#btnAdd").click(function () {
    //    var title = $('#txtTitle').val();
    //    if (title.length === 0) {
    //        return false;
    //    }
    //    $('#hdnEventName').val(title);
    //    return true;
    //});
    ////add event to exact day
    //$("div[data-event]").each(function () {
    //    var eventdate = $(this).attr('data-event');
    //    $(this).appendTo($('td[data-date="' + eventdate + '"] div div.fc-day-content div'));
    //});
    ////manage Event Delete OR Update
    //$("div[data-id]").click(function () {
    //    var id = $(this).attr('data-id');
    //    $("#hdnEventId").val(id);
    //    $("#txtUpdateEvent").val($(this).attr("data-name"));
    //    // $("#addEvent").css("display","none");
    //    $("#manageEvent").modal("show");
    //});

    ////add right border to the last cell
    //$("table tr td:last-child").addClass('fc-last'); ;

    ////hilight current Day
    //var daate = new Date();
    //var year = daate.getFullYear();
    //var month = daate.getMonth();
    //var day = daate.getDay();
    //$("td[data-date]").each(function () {
    //    var date = $(this).attr("data-date");
    //    var fakedat = year + "-" + month + "-" + day;
    //    if (date === fakedat) {
    //        $(this).addClass("fc-state-highlight");
    //    }
    //});
    //to change language and direction
    var val = $('#hfRtl').val();
    if (val !== "1") {
        $('div.infobox').css({ 'width': '243px' });

        $('#btn-7').css({ 'width': '115px' });

        //ali comment
        //resize any table
        if ($('#by-number').width() > 455) {
            $('#chart').css({ 'width': '55%' });
        }
    }
});

var adminScript = function() {
    function notificationsMethod( data ) {
        jQuery(function($) {
            $('.easy-pie-chart.percentage').each(function() {
                var $box = $(this).closest('.infobox');
                var barColor = $(this).data('color') || (!$box.hasClass('infobox-dark') ? $box.css('color') : 'rgba(255,255,255,0.95)');
                var trackColor = barColor == 'rgba(255,255,255,0.95)' ? 'rgba(255,255,255,0.25)' : '#E2E2E2';
                var size = parseInt($(this).data('size')) || 50;
                $(this).easyPieChart({
                    barColor: barColor,
                    trackColor: trackColor,
                    scaleColor: false,
                    lineCap: 'butt',
                    lineWidth: parseInt(size / 10),
                    animate: /msie\s*(8|7|6)/.test(navigator.userAgent.toLowerCase()) ? false : 1000,
                    size: size
                });
            });

            $('.sparkline').each(function() {
                var $box = $(this).closest('.infobox');
                var barColor = !$box.hasClass('infobox-dark') ? $box.css('color') : '#FFF';
                $(this).sparkline('html', { tagValuesAttribute: 'data-values', type: 'bar', barColor: barColor, chartRangeMin: $(this).data('min') || 0 });
            });


            var placeholder = $('#piechart-placeholder').css({ 'width': '90%', 'min-height': '150px' });
//            var data = [
//                { label: '<%=Active %>', data: '<%=ActiveDecimal %>', color: "#68BC31" }, //38.7
//                { label: '<%=New %>', data: '<%=NewDecimal %>', color: "#6fb3e0" }, //24.5
//                { label: '<%=MenuHold %>', data: '<%=MenuholdDecimal %>', color: "#2091CF" }, //5
//                { label: '<%=MenuSuspend %>', data: '<%=MenuSuspendDecimal %>', color: "#AF4E96" }, //8.2
//                { label: '<%=Cancel %>', data: '<%=CancelDecimal %>', color: "#DA5430" }, //18.6
//                { label: '<%=SystemProblem %>', data: '<%=SystemProblemDecimal %>', color: "#FEE074" } //5
//            ];
//            data = data;
            function drawPieChart(placeholder, data, position) {
                $.plot(placeholder, data, {
                    series: {
                        pie: {
                            show: true,
                            tilt: 0.8,
                            highlight: {
                                opacity: 0.25
                            },
                            stroke: {
                                color: '#fff',
                                width: 2
                            },
                            startAngle: 2
                        }
                    },
                    legend: {
                        show: true,
                        position: position || "ne",
                        labelBoxBorderColor: null,
                        margin: [-30, 15]
                    },
                    grid: {
                        hoverable: true,
                        clickable: true
                    }
                });
            }

            drawPieChart(placeholder, data);

          
            placeholder.data('chart', data);
            placeholder.data('draw', drawPieChart);


            var $tooltip = $("<div class='tooltip top in'><div class='tooltip-inner'></div></div>").hide().appendTo('body');
            var previousPoint = null;

            placeholder.on('plothover', function(event, pos, item) {
                if (item) {
                    if (previousPoint != item.seriesIndex) {
                        previousPoint = item.seriesIndex;
                        var tip = item.series['label'] + " : " + item.series['percent'] + '%';
                        $tooltip.show().children(0).text(tip);
                    }
                    $tooltip.css({ top: pos.pageY + 10, left: pos.pageX + 10 });
                } else {
                    $tooltip.hide();
                    previousPoint = null;
                }

            });


            var d1 = [];
            for (var i = 0; i < Math.PI * 2; i += 0.5) {
                d1.push([i, Math.sin(i)]);
            }

            var d2 = [];
            for (var i = 0; i < Math.PI * 2; i += 0.5) {
                d2.push([i, Math.cos(i)]);
            }

            var d3 = [];
            for (var i = 0; i < Math.PI * 2; i += 0.2) {
                d3.push([i, Math.tan(i)]);
            }


            $('#recent-box [data-rel="tooltip"]').tooltip({ placement: tooltip_placement });

            function tooltip_placement(context, source) {
                var $source = $(source);
                var $parent = $source.closest('.tab-content');
                var off1 = $parent.offset();
                var w1 = $parent.width();

                var off2 = $source.offset();
                var w2 = $source.width();

                if (parseInt(off2.left) < parseInt(off1.left) + parseInt(w1 / 2)) return 'right';
                return 'left';
            }

            $('.dialogs,.comments').slimScroll({
                height: '300px'
            });


            //Android's default browser somehow is confused when tapping on label which will lead to dragging the task
            //so disable dragging when clicking on label
            //ali comment
            var agent = navigator.userAgent.toLowerCase();
            if ("ontouchstart" in document && /applewebkit/.test(agent) && /android/.test(agent))
                $('#tasks').on('touchstart', function(e) {
                    var li = $(e.target).closest('#tasks li');
                    if (li.length == 0) return;
                    var label = li.find('label.inline').get(0);
                    if (label == e.target || $.contains(label, e.target)) e.stopImmediatePropagation();
                });
           
            $('#tasks').sortable({
                    opacity: 0.8,
                    revert: true,
                    forceHelperSize: true,
                    placeholder: 'draggable-placeholder',
                    forcePlaceholderSize: true,
                    tolerance: 'pointer',
                    stop: function(event, ui) { //just for Chrome!!!! so that dropdowns on items don't appear below other items after being moved
                        $(ui.item).css('z-index', 'auto');
                    }
                }
            );
            $('#tasks').disableSelection();
            $('#tasks input:checkbox').removeAttr('checked').on('click', function() {
                if (this.checked) $(this).closest('li').addClass('selected');
                else $(this).closest('li').removeClass('selected');
            });
            $("#sidebar-shortcuts-large>.btn").tooltip({
                hide: {
                    effect: "explode",
                    delay: 250
                }
            });


        });
    }

    function dialogNotificationsMethod() {
        
        //var count = $('#count').val();
        //if (count === "true") {
        //    $('#gv_reminders').css("border", "none");
        //    $('#pop-me-up').dialog({
        //        autoOpen: true,
        //        width: 435,
        //        resizable: false,
        //        draggable: false,
        //        title: 'Reminder',
        //        modal: true,
        //        position: 'center',
        //        buttons: {
        //            "Ok": function () {
        //                $(this).dialog("close");
        //            }
        //        }
        //    });
        //}
    }
return {
        NotificationsMethod: notificationsMethod,
        DialogNotifications : dialogNotificationsMethod
    }
} ();