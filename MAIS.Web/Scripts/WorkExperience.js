$(document).ready(function () {

    $('.btnSaveExp').click(function (e) {
        var flag = ValidationWorkExperiencePage();
        if (!flag) {
            e.preventDefault();
        }
    });
    //txtEmpName
    $('.txtEmpName').keypress(function (event) {
        var englishAlphabet = /^\'|-|[a-zA-Z\s\.]$/g;
        //var englishAlphabetDigitsAndWhiteSpace = /[A-Za-z0-9 ]/g;

        var Key = String.fromCharCode(event.which);
        // For the keyCodes, look here: http://stackoverflow.com/a/3781360/114029
        // keyCode == 8  is backspace
        // keyCode == 37 is left arrow
        // keyCode == 39 is right arrow

        if (event.KeyCode == 8 || event.KeyCode == 37 || event.KeyCode == 39 || englishAlphabet.test(Key)) {
            return true;
        }
        return false;
    });
});


function ValidationWorkExperiencePage() {

    $('.errMsg')[0].innerHTML = ''
    var flag = true;

    var empName = ValidateName();
    if (empName != '') {
        flag = false
        if ($('.errMsg')[0].innerHTML == '') {
            $('.errMsg')[0].innerHTML = empName
        }
        else {
            $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + empName
        }
        $('.errMsg').show();
    }

    var chkboxMsg = validateChkBox();
    if (chkboxMsg != '') {
        flag = false
        if ($('.errMsg')[0].innerHTML == '') {
            $('.errMsg')[0].innerHTML = chkboxMsg
        }
        else {
            $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + chkboxMsg
        }
        $('.errMsg').show();
    }

    var dateMessage = ValidateDates();
    if (dateMessage != '') {
        flag = false
        if ($('.errMsg')[0].innerHTML == '') {
            $('.errMsg')[0].innerHTML = dateMessage
        }
        else {
            $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + dateMessage
        }
        $('.errMsg').show();
    }

    var msgTitle = ValidateDesignation();
    if (msgTitle != '') {
        flag = false
        if ($('.errMsg')[0].innerHTML == '') {
            $('.errMsg')[0].innerHTML = msgTitle
        }
        else {
            $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + msgTitle
        }
        $('.errMsg').show();
    }

    var message = AddressValidation()
    if (message != '') {
        flag = false
        if ($('.errMsg')[0].innerHTML == '') {
            $('.errMsg')[0].innerHTML = message
        }
        else {
            $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + message
        }
        $('.errMsg').show();
    }

    var messageEmail = ValidateEmail();
    var messagephone = ValidatePhone();

    if (messageEmail != '') {
        flag = false
        if ($('.errMsg')[0].innerHTML == '') {
            $('.errMsg')[0].innerHTML = messageEmail
        }
        else {
            $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + messageEmail
        }
        $('.errMsg').show();
    }
    if (messagephone != '') {
        flag = false
        if ($('.errMsg')[0].innerHTML == '') {
            $('.errMsg')[0].innerHTML = messagephone
        }
        else {
            $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + messagephone
        }
        $('.errMsg').show();
    }
    return flag;
}

function validateChkBox() {
    var m = '';
    var isRNChecked = $('.chkRNExp').is(':checked');
    var isDDChecked = $('.chkDDExp').is(':checked');

    if (isDDChecked || isRNChecked) { }
    else {
        m = "Please check any one of the RN or DD experience";
    }
    return m;
}

function ValidateDesignation() {

    var m = '';
    if ($('.txtDesignation').val() == '') {
        m = "Please enter Designation/Title"
    }
    if ($('.txtDesignation').val() != '') {
        var inputVal = $('.txtDesignation').val();
        var alphabets = /^[0-9a-zA-Z ]+$/;
        if (!alphabets.test(inputVal)) {
            var title = "Please enter valid Designation/Title";
            if (m == '') {
                m = title;
            }
            else { m = m + "<br/>" + title; }
        }
    }
    return m;
}

function ValidateDates() {
    var msg = '';
    var nowdate = new Date;

    if ($('.txtStartDate').val() == '') {
        msg = "Please enter StartDate"
    }
    var tmp = IsDate($.trim($(".txtStartDate").val()));
    if (tmp != "") {
        msg = "Please enter valid start date," + tmp;
    }
    if (($.trim($('.txtStartDate').val()) != '') && (Date.parse($.trim($('.txtStartDate').val())) > Date.parse(nowdate))) {
        msg = "Start date cannot be greater than today";
    }
    if (($.trim($('.hdRNDate').val() != ''))) {
        if (($.trim($('.txtStartDate').val() != '')) && (Date.parse($.trim($('.txtStartDate').val())) < Date.parse($('.hdRNDate').val()))) {
            msg = "Start date cannot be prior to RN License issuance date " + $.trim($('.hdRNDate').val());
        }
    }
    if ($('.txtEndDate').val() == '') {
        msg = "Please enter EndDate"
    }
    var tmp1 = IsDate($.trim($(".txtEndDate").val()));
    if (tmp1 != "") {
        msg = "Please enter valid end date," + tmp1;
    }
    if ($('.txtEndDate').val() != '') {
        var tmpdate = IsDate($.trim($(".txtEndDate").val()));
        if (tmpdate != "") {
            msg = msg + "<br/>" + "Please enter valid end date," + tmpdate;
        }
        if ((Date.parse($.trim($('.txtEndDate').val())) > Date.parse(nowdate))) {
            msg = "End date cannot be greater than today";
        }
        if ((Date.parse($(".txtStartDate").val())) >= (Date.parse($(".txtEndDate").val()))) {
            if (msg == '') {
                msg = "Please check start date can not be later than end date";
            }
            else {
                msg = msg + "<br/>" + "Please check start date can not be later than end date";
            }
        }
    }
    return msg;
}

function ValidateName() {
    var message = '';
    if ($('.txtEmpName').val() == '') {
        message = "Please enter Employer/Agency Name"
    }
    //if ($('.txtEmpName').val() != '') {
    //    var inputVal = $('.txtEmpName').val();        
    //    var alphabets = /^[A-Za-z]+$/;
    //    if (!alphabets.test(inputVal)) {
    //        var eName = "Please enter valid Employer/Agency name";
    //        if (message == '') {
    //            message = eName;
    //        }
    //        else { message = message + "<br/>" + eName; }
    //    }       
    //}
    return message;
}


