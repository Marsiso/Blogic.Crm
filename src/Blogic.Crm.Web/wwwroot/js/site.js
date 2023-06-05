// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    var i=1;
    $("#dynamic-table-add-row").click(function () {
        b=i-1;
        $('#dynamic-table-row-'+i).html($('#dynamic-table-row-'+b).html()).find('td:first-child').html(i+1);
        $('#dynamic-table').append('<tr id="dynamic-table-row-'+(i+1)+'"></tr>');
        i++;
    });
    $("#dynamic-table-del-row").click(function () {
        if (i>1) {
            $("#dynamic-table-row-"+(i-1)).html('');
            i--;
        }
    });
});