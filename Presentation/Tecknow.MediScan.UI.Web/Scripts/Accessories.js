
$(function() {
    $("#datepicker2").datepicker(
    {
        changeMonth: true,
        changeYear: true,

    });
    $("#datepicker2").datepicker("option", "dateFormat", "yy-mm-dd");
});

$(function() {
    $("#datepicker1").datepicker(
    {
        changeMonth: true,
        changeYear: true
    });
    $("#datepicker1").datepicker("option", "dateFormat", "yy-mm-dd");
});


$(document).ready(function() {

    $(window).load(function() {
        $(".se-pre-con").fadeOut("slow");
    });

    $(function() { $("[data-toggle='tooltip']").tooltip(); });

    $(document).ajaxStart(function() {
        $("#wait").css("display", "block");
    });
    $(document).ajaxComplete(function() {
        $("#wait").css("display", "none");
    });
});