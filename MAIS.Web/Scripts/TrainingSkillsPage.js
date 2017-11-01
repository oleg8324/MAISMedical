$(document).ready(function () {

    function validate(input) {
        var date = new Date(input);
        input = input.split('/');
        return date.getMonth() + 1 === +input[0] &&
               date.getDate() === +input[1] &&
               date.getFullYear() === +input[2];
    };

    $(".txtCourseNumberRnNew").mask("aaa-999-999-****");
    $(".txtRNNumber").mask("RN999999");

    //$(".txtRNNumber").focusout(function () {
    //    //alert("Button Hit");
    //    if ($(this).val() != "") {
    //        $(".bntSearchRN").trigger("click");
    //        }
    //    });
    //$(".txtRNName").focusout(function () {
    //   // alert("Button Hit");
    //    if ($(this).val() != "") {
    //        $(".bntSearchRNName").trigger("click");
    //    }
    //});

    var dateToday = new Date();
    dateToday.setDate(dateToday.getDate());

    //set up the Masking feilds
    $(".MaskOne, .MaskTwo").keydown(function (e) {
        var key = e.charCode || e.keyCode || 0;
        // allow backspace, tab, delete, arrows, ".", numbers and keypad numbers ONLY
        return (
            key == 8 ||
            key == 9 ||
            key == 46 ||
            key == 110 ||
            (key >= 37 && key <= 40) ||
            (key >= 48 && key <= 57) ||
            (key >= 96 && key <= 105));

    });

    //Test the if CEU date in Valid 
    $(".txtCEUDate").blur(function () {
        if (!($(".txtCEUDate").val() == "")) {
            if (validate($(".txtCEUDate").val()) == false) {

                alert("This is not a vaild date.");
                $(".txtCEUDate").val("");
                $(".txtCEUDate").focus();
            }
        }
    });

    $(".txtCEUDate").calendarsPicker('option', 'maxDate', (dateToday.getMonth() + 1) + '/' + dateToday.getDate() + '/' + dateToday.getFullYear());
    $(".txtCEUDate").calendarsPicker('option', 'minDate', $('#HFCertStartDate').val());
});