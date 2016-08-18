

var carierInputs = $('#model-1 > input[type="hidden"]');
var can1 = "c-1";
createChart(carierInputs, can1);

var carierInputs = $('#model-2 > input[type="hidden"]');
var can1 = "c-2";
createChart(carierInputs, can1);

var carierInputs = $('#model-3 > input[type="hidden"]');
var can1 = "c-3";
createChart(carierInputs, can1);


var carierInputs = $('#model-4 > input[type="hidden"]');
var can1 = "c-4";
createChart(carierInputs, can1);

var carierInputs = $('#model-7 > input[type="hidden"]');
var can1 = "c-7";
createChart(carierInputs, can1);

$('#model-1').show();
var modelGroup = [
        $('#model-1'), $('#model-2'), $('#model-3'), $('#model-4'), $('#model-7')
    ];
$('#btn-1').bind('click', function () {
    showByIndexHideRest(modelGroup, 0);
});
$('#btn-2').bind('click', function () {
    showByIndexHideRest(modelGroup, 1);
});
$('#btn-3').bind('click', function () {
    showByIndexHideRest(modelGroup, 2);
});

$('#btn-4').bind('click', function () {
    showByIndexHideRest(modelGroup, 3);
});
$('#btn-7').bind('click', function () {
    showByIndexHideRest(modelGroup, 4);
});


function showByIndexHideRest(modelGroup, index) {
    for (var i = 0; i < modelGroup.length; i++) {
        if (i === index)
            $(modelGroup[i]).slideDown();
        else
            $(modelGroup[i]).slideUp();
    }
}


function createChart(carrierInputs, canvasId) {
    var labelGroup = [];
    var valsGroup = [];
    for (var i = 0; i < carrierInputs.length; i++) {
        labelGroup.push($(carrierInputs[i]).attr("data-names"));
        valsGroup.push(parseInt($(carrierInputs[i]).attr("data-vals")));
    }
    drawChart(canvasId, labelGroup, valsGroup);
}

function drawChart(canvasId, labelItems, valItems) {

    var lineChartData;
    switch(canvasId) {
    case "c-1":
        lineChartData = {
            datasets: [
                {
                    fillColor: "#d9edf7",
                    strokeColor: "#3a87ad",
                    pointColor: "#3a87ad",
                    pointStrokeColor: "#fff",
                    scaleFontColo: "rgb(117, 1, 43)",
                    scaleOverride: true,
                    scaleStartValue: 1,
                    data: valItems
                }
            ]
        };
        break;
    case "c-2":
        lineChartData = {
            datasets: [
                {
                    fillColor: "#d6e9c6",
                    strokeColor: "#468847",
                    pointColor: "#468847",
                    pointStrokeColor: "#fff",
                    scaleFontColo: "rgb(117, 1, 43)",
                    scaleOverride: true,
                    scaleStartValue: 1,
                    data: valItems
                }
            ]
        };
            break;
        case "c-3":
            lineChartData = {
                datasets: [
                {
                    fillColor: "#fbeed5",
                    strokeColor: "#f89406",
                    pointColor: "#f89406",
                    pointStrokeColor: "#fff",
                    scaleFontColo: "rgb(117, 1, 43)",
                    scaleOverride: true,
                    scaleStartValue: 1,
                    data: valItems
                }
            ]
            };
                break;
            case "c-4":
                lineChartData = {
                    datasets: [
                {
                    fillColor: "#eed3d7",
                    strokeColor: "#b94a48",
                    pointColor: "#b94a48",
                    pointStrokeColor: "#fff",
                    scaleFontColo: "rgb(117, 1, 43)",
                    scaleOverride: true,
                    scaleStartValue: 1,
                    data: valItems
                }
            ]
                };
            break;
        case "c-7":
            lineChartData = {
                datasets: [
                {
                    fillColor: "#bce8f1",
                    strokeColor: "#428bca",
                    pointColor: "#428bca",
                    pointStrokeColor: "#fff",
                    scaleFontColo: "rgb(117, 1, 43)",
                    scaleOverride: true,
                    scaleStartValue: 1,
                    data: valItems
                }
            ]
            };
            break;
            //old colors
                    default :
                        lineChartData = {
                            datasets: [
                                {
                                    fillColor: "rgba(151,187,205,0.5)",
                                    strokeColor: "rgba(151,187,205,1)",
                                    pointColor: "rgba(220,220,220,1)",
                                    pointStrokeColor: "#fff",
                                    scaleFontColo: "rgb(117, 1, 43)",
                                    scaleOverride: true,
                                    scaleStartValue: 1,
                                    data: valItems
                                }
                            ]
                            };
                        break;
                     
            
    }
    lineChartData.labels = labelItems;
    var steps = 10;
    var maxLabel = _.max(valItems, function (x) { return x; });
    var myLine = new Chart(document.getElementById(canvasId).getContext("2d")).Line(lineChartData, {
        scaleOverride: true,
        scaleSteps: steps,
        scaleStepWidth: Math.ceil(maxLabel / steps),
        scaleStartValue: 0
    });
}