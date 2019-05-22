//$(function () {
//    $('[data-toggle="tooltip"]').tooltip({
//        delay: { "show": 500, "hide": 100 }
//    })
//});

$(document).ready(function () {
    $('#dropdownApplications').mouseover(function () {        
        $('#dropdownApplications-menu').dropdown('show');
    })
    $('#dropdownApplications').mouseout(function () {
        $('#dropdownApplications-menu').dropdown('hide');
        $('#dropdownApplications-menu').mouseover(function () {
            $('#dropdownApplications-menu').dropdown('show');
        })        
    })

    $('#dropdownApplications-menu').mouseout(function () {
        $('#dropdownApplications-menu').dropdown('hide');
    })
});