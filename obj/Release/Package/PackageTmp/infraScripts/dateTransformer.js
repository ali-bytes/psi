function GetDMY(date) {
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var year = date.getFullYear();
    return day + "/" + month + "/" + year;
}

function SetDMY(dateString) {
    var all = dateString.split('/');
    return new Date(all[2], parseInt(all[1]) - 1, all[0]);


}