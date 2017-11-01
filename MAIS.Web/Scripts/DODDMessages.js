$(document).ready(function () {

    if ($('.txtStartDate').val() == "") {
        $('.txtEndDate').attr('disabled', 'disable')
    };

    if ($('.txtSearchStartDate').val() == "") {
          $('.txtSearchEndDate').attr('disabled', 'disable')
    };

    if ($('.ckGroup  input[type=checkbox]:checked').val() == "on") {
        $('.trGroup').show();
    };

    if ($('.ckPerson input[type=checkbox]:checked').val() == "on") {
        $('.trPerson').show();
    };

    if ($('.ckbSearchMessage input[type=checkbox]:checked').val() == "on") {
        $('.dvMessageSearchDates').show();
    };

    function validate(input) {
        var date = new Date(input);
        input = input.split('/');
        return date.getMonth() + 1 === +input[0] &&
               date.getDate() === +input[1] &&
               date.getFullYear() === +input[2];
    };

    var now = new Date();
    var curDate = now.getMonth() + 1 + "/" + now.getDate() + "/" + now.getFullYear();

    $('.txtStartDate').calendarsPicker('option', 'minDate', curDate);
    
    $('.txtStartDate').calendarsPicker('option', 'onClose', function () {
        if (!($('.txtStartDate').val() == "")) {
            if (validate($('.txtStartDate').val()) == true) {
                $('.txtEndDate').removeAttr('disabled');
                $('.txtEndDate').calendarsPicker('option', 'minDate', $('.txtStartDate').val());
            }
            else {
                alert("This is not a valid date.");

            }
        }
        else {
            $('.txtEndDate').attr('disabled', 'disable')
        }
    });

    $('.txtSearchStartDate').calendarsPicker('option', 'onClose', function () {
        if (!($('.txtSearchStartDate').val() == "")) {
            if (validate($('.txtSearchStartDate').val()) == true) {
                $('.txtSearchEndDate').removeAttr('disabled');
                $('.txtSearchEndDate').calendarsPicker('option', 'minDate', $('.txtSearchStartDate').val());
            }
            else {
                alert("This is not a valid date.");

            }
        }
        else {
            $('.txtSearchEndDate').attr('disabled', 'disable')
        }
    });
    

    $('.txtStartDate').blur(function () {
        if (!($('.txtStartDate').val() == "")) {
            if (validate($('.txtStartDate').val())) {
                var inputDate = new Date($('.txtStartDate').val());
                var TestDate = new Date(curDate);

                if (inputDate < TestDate) {
                    alert("Date enter can not before today.");
                    $('.txtStartDate').val("");
                    $('.txtEndDate').attr('disabled', 'disable');
                } else {
                    $('.txtEndDate').removeAttr('disabled');
                }
            }
            else {
                alert("This is not a valid date.");
                $('.txtStartDate').val("");
            }
        }
       
    });

    $('.txtSearchStartDate').blur(function () {
        if (!($('.txtSearchStartDate').val() == "")) {
            if (validate($('.txtSearchStartDate').val())) {
               
                    $('.txtSearchEndDate').removeAttr('disabled');
                    $('.txtSearchEndDate').val("");
            }
            else {
                alert("This is not a valid date.");
                $('.txtSearchStartDate').val("");
            }
        }

    });

    $('.ckbSearchMessage').change(function (e) {
        if ($('.ckbSearchMessage input[type=checkbox]:checked').val() == "on") {
            $('.dvMessageSearchDates').show();
        } else {
            $('.dvMessageSearchDates').hide();
        }
    });


    $('.ckGroup').change(function (e) {
        if ($('.ckGroup  input[type=checkbox]:checked').val() == "on") {
            $('.trGroup').show();
        }
        else {
            $('.trGroup').hide();
        }

       
    });

    $('.ckPerson').change(function (e) {
        if ($('.ckPerson input[type=checkbox]:checked').val() == "on") {
            $('.trPerson').show();
        }
        else {
            $('.trPerson').hide();
        }
    });
});