var donotSave = false;
$(document).ready(function () {
    $('.errMsg')[0].innerHTML = '';
    $('.errMsg').hide();
    var txt = $('.txtLicenseNoLast4SSN').val();
    var lbl = $('.lblLicenseNoLast4SSN').html();
    //var lbl1 = $('.lblDOBRNLicIssuance').html();
    //var txt1 = $('.txtDOBRNLicIssuance').val();
    $('.txtHomePhoneNumber').mask("999-999-9999");
    $('.txtWorkPhoneNumber').mask("999-999-9999");
    $('.txtCellPhoneNumber').mask("999-999-9999");

    $('.txtLicenseNoLast4SSN').live('blur', function (e) {
        $('.errMsg')[0].innerHTML = ''
        var validRN = "Please enter a valid RN License Number"
        var changeTxt = $('.txtLicenseNoLast4SSN').val();
        if (lbl == "RN Lic. No:") {
            $('.txtLicenseNoLast4SSN').mask("RN999999");
        }
        else {
            $('.txtLicenseNoLast4SSN').mask("9999");
        }
    });

    $('.txtDOBRNLicIssuance').live('blur', function (e) {
        donotSave = true;
    });

    $('.btnSave').click(function (e) {
        donotSave = true;
        var whichClick = false;
        var flag = ValidationPersonalInforamtionPage();
        if (flag) {
            $('.errMsg')[0].innerHTML = '';
            $('.errMsg').hide();
            SavePersonalInformation(whichClick);
        }
    });
    $('.btnSaveAndContinue').click(function (e) {
        donotSave = true;
        var whichClick = true;
        var flag = ValidationPersonalInforamtionPage();
        if (flag) {
            $('.errMsg')[0].innerHTML = '';
            $('.errMsg').hide();
            SavePersonalInformation(whichClick);
            //if ($('.errMsg')[0].innerHTML == '') {
            //    window.location.assign("EmployerInformation.aspx")
            //}
        }
    });

    $('.btnPrevious').click(function (e) {
        if (donotSave == false && $('.hdNew').val() == "New") {
            CallUnload()
        }
        if ($('.hdStartpage').val() == "True") {
            window.location.assign("StartPage.aspx?Create=New")
        } else { window.location.assign("StartPage.aspx") }
    });
    window.onbeforeunload = function () {
        if (donotSave == false && $('.hdNew').val() == "New") {
            CallUnload()
        }
    };

    $('.txtLastname, .txtFirstName').keypress(function (event) {
        var englishAlphabet = /^\'|-|[a-zA-Z]$/g;
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

    $('.txtMiddleName').keypress(function (event) {
        var englishAlphabet = /[a-zA-Z]$/g;
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

    //$('.txtLastname').keydown(function (event) {
    //    if (event.which == 56 || event.which == 106) {
    //        event.preventDefault();
    //    }
    //});

    //$('.txtFirstName').keydown(function (event) {
    //    if (event.which == 56 || event.which == 106) {
    //        event.preventDefault();
    //    }
    //});

});



function CallUnload() {
    executePageMethod('PersonalInformation.aspx', 'DeleteAppInfo', '{}', function (data) {
        if (data.d == true) {
            $('.errMsg').css("color", "green");
            $('.errMsg').css("border-color", "green");
            $('.errMsg')[0].innerHTML = "successfully deleted the application info";
            $('.errMsg').show();
        }
    }, function () {
        $('.errMsg')[0].innerHTML = "Error in deleting the application info";
        $('.errMsg').show();
        return false;
    });
}

function ValidationPersonalInforamtionPage() {
    var txt = $('.txtLicenseNoLast4SSN').val();
    var lbl = $('.lblLicenseNoLast4SSN').html();
    var lbl1 = $('.lblDOBRNLicIssuance').html();
    var txt1 = $('.txtDOBRNLicIssuance').val();
    $('.errMsg')[0].innerHTML = ''
    $('.errMsg').css("color", "red");
    $('.errMsg').css("border-color", "red");
    var flag = true;
    if (txt == '' || txt === undefined) {
        flag = false;
        var name = "Please enter the required data for" + " " + lbl
        $('.errMsg')[0].innerHTML = name
        $('.errMsg').show()
    }
    if (txt1 == '' || txt1 === undefined) {
        flag = false
        var name1 = "Please enter the required data for" + " " + lbl1
        if ($('.errMsg')[0].innerHTML == '') {
            $('.errMsg')[0].innerHTML = name1
        }
        else {
            $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + name1
        }
        $('.errMsg').show();
    }
    var chkflg = $('.rdbGender input[type=radio]:checked').val();
    if (chkflg == undefined) {
        flag = false
        var gender = "Please select the gender of the person"
        if ($('.errMsg')[0].innerHTML == '') {
            $('.errMsg')[0].innerHTML = gender
        }
        else {
            $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + gender
        }
        $('.errMsg').show();
    }
    var messagePhoneNumber = ValidationPhoneNumber();
    if (messagePhoneNumber != '') {
        flag = false
        if ($('.errMsg')[0].innerHTML == '') {
            $('.errMsg')[0].innerHTML = messagePhoneNumber
        }
        else {
            $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + messagePhoneNumber
        }
        $('.errMsg').show();
    }
    var lastFirstName = ValidationLastFirstName();
    if (lastFirstName != '') {
        flag = false
        if ($('.errMsg')[0].innerHTML == '') {
            $('.errMsg')[0].innerHTML = lastFirstName
        }
        else {
            $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + lastFirstName
        }
        $('.errMsg').show();
    }
    var emailAddress = ValidationOfEmailAddress();
    if (emailAddress != '') {
        flag = false
        if ($('.errMsg')[0].innerHTML == '') {
            $('.errMsg')[0].innerHTML = emailAddress
        }
        else {
            $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + emailAddress
        }
        $('.errMsg').show();
    }
    var validateDates = ValidateForFutureDates();
    if (validateDates != '') {
        flag = false
        if ($('.errMsg')[0].innerHTML == '') {
            $('.errMsg')[0].innerHTML = validateDates
        }
        else {
            $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + validateDates
        }
        $('.errMsg').show();
    }
    if (lbl1 == "Date of Birth:") {
        var dates = ValidationDOBDates(lbl1, txt1);
        if (dates != '') {
            flag = false
            if ($('.errMsg')[0].innerHTML == '') {
                $('.errMsg')[0].innerHTML = dates
            }
            else {
                $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + dates
            }
            $('.errMsg').show();
        }
    }
    else {
        var messageDate = ValidDate(txt1)
        if (messageDate != '') {
            flag = false
            if ($('.errMsg')[0].innerHTML == '') {
                $('.errMsg')[0].innerHTML = messageDate
            }
            else {
                $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + messageDate
            }
            $('.errMsg').show();
        }
    }
    var message = AddressValidationForPersonalInformation()
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
    return flag;
}

function ValidateForFutureDates() {
    var msg = ''
    if ($('.txtDOBRNLicIssuance').val() != '') {
        var today = new Date();
        var RNLicDOBDate = new Date($(".txtDOBRNLicIssuance").val());
        if (RNLicDOBDate > today) {
            msg = "The date of Birth of DD personnel or RN Licensce Issuance date cannot be future date";
        }
    }
    return msg;
}

function ValidationDOBDates(lbl1, txt1) {
    var message = '';
    var today = new Date();
    if (txt1 != '') {
        var tmpdate = IsDate($.trim(txt1));
        if (tmpdate != "") {
            message = message + "<br/>" + "Please enter valid date of birth with format of MM/DD/YYYY," + tmpdate;
        }
        else {
            if (lbl1 == "Date of Birth:") {
                today.setFullYear(today.getFullYear() - 18);
                var dt = new Date($.trim(txt1));
                if (dt > today) {
                    message = message + "<br/>" + "You must be at least 18 years old in order to apply for <br> certification. Applicants born after " + (today.getMonth() + 1) + "/" + today.getDate() + "/" + today.getFullYear() + " will be denied.";
                }
            }
        }
    }
    return message;
}

function ValidDate(txt1) {
    var msg = ''
    if (txt1 != '') {
        var tmp = IsDate($.trim(txt1));
        if (tmp != "") {
            msg = "Please enter valid date with format of MM/DD/YYYY, " + tmp;
        }
    }
    return msg
}

function ValidationLastFirstName() {
    var message = '';
    if ($('.txtLastname').val() == '' || $('.txtFirstName').val() == '') {
        message = "Please enter first and last name of the person"
    }
    //if ($('.txtLastname').val() != '' || $('.txtFirstName').val() != '') {
    //    var inputVal = $('.txtLastname').val();
    //    var inputFirstName = $('.txtFirstName').val();
    //    var alphabets = /^[A-Za-z'.\s]+$/;
    //    if (!alphabets.test(inputVal)) {
    //        var lastName = "Please enter valid Last name of the person";
    //        if (message == '') {
    //            message = lastName;
    //        }
    //        else { message = message + "<br/>" + lastName; }
    //    }
    //    if (!alphabets.test(inputFirstName)) {
    //        var firstName = "Please enter valid first name of the person";
    //        if (message == '') {
    //            message = firstName;
    //        }
    //       else { message = message + "<br/>" + firstName; }
    //    }
    //}
    return message;
}

function ValidationPhoneNumber() {
    var message = '';
    //if ($('.txtHomePhoneNumber').val() == '' || $('.txtWorkPhoneNumber').val() == '' ||
    //    $('.txtCellPhoneNumber').val() == '' || $('.txtHomeAddress').val() == '' || $('.txtWorkAddress').val() == '' || $('.txtCellAddress').val() == '') {
    //    message = "Please enter any one of phone information and any one of email address"
    //}
    if ($('.txtHomePhoneNumber').val() != '' || $('.txtWorkPhoneNumber').val() != '' ||
        $('.txtCellPhoneNumber').val() != '') {
        message = ''
        var phoneNumberPattern = /^\(?(\d{3})\)?[- ]?(\d{3})[- ]?(\d{4})$/;
        var inputHomePhoneNumber = $('.txtHomePhoneNumber').val();
        var inputWorkPhoneNumber = $('.txtWorkPhoneNumber').val();
        var inputCellPhoneNumber = $('.txtCellPhoneNumber').val();
        if (inputHomePhoneNumber != '') {
            if (!phoneNumberPattern.test(inputHomePhoneNumber)) {
                var phoneMessage = "Please enter valid home phone Number";
                if (message == '') {
                    message = phoneMessage;
                }
                else { message = message + "<br/>" + phoneMessage; }
            }
        }
        if (inputWorkPhoneNumber != '') {
            if (!phoneNumberPattern.test(inputWorkPhoneNumber)) {
                var workMessage = "Please enter valid work phone Number";
                if (message == '') {
                    message = workMessage;
                }
                else { message = message + "<br/>" + workMessage; }
            }
        }
        if (inputCellPhoneNumber != '') {
            if (!phoneNumberPattern.test(inputCellPhoneNumber)) {
                var cellMessage = "Please enter valid cell phone Number";
                if (message == '') {
                    message = cellMessage;
                }
                else { message = message + "<br/>" + cellMessage; }
            }
        }
    }
    return message;
}

function ValidationOfEmailAddress() {
    var message = '';
    //if ($('.txtHomeAddress').val() == '' && $('.txtWorkAddress').val() == '' && $('.txtCellAddress').val() == '') {
    //    message = "Please enter any one of your email addresses"
    //}
    if ($('.txtHomeAddress').val() != '' || $('.txtWorkAddress').val() != '' || $('.txtCellAddress').val() != '') {
        var emailAddressRegx = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if ($('.txtHomeAddress').val() != '') {
            if (!emailAddressRegx.test($('.txtHomeAddress').val())) {
                var homeEmailAddress = "Please enter valid home email address";
                if (message == '') {
                    message = homeEmailAddress;
                }
                else { message = message + "<br/>" + homeEmailAddress; }
            }
        }
        if ($('.txtWorkAddress').val() != '') {
            if (!emailAddressRegx.test($('.txtWorkAddress').val())) {
                var workEmailAddress = "Please enter valid work email address";
                if (message == '') {
                    message = workEmailAddress;
                }
                else { message = message + "<br/>" + workEmailAddress; }
            }
        }
        if ($('.txtCellAddress').val() != '') {
            if (!emailAddressRegx.test($('.txtCellAddress').val())) {
                var cellEmailAddress = "Please enter valid cell email address";
                if (message == '') {
                    message = cellEmailAddress;
                }
                else { message = message + "<br/>" + cellEmailAddress; }
            }
        }
    }
    return message;
}

var SavePersonalInformation = function (whichClick) {
    var userRole = $('.hdRNFlag').val();
    var flag = $('.hdRole').val();
    var message = GetAddressFields()
    var savePersonalInfo = {
        'LicenseLast4SSN': $('.txtLicenseNoLast4SSN').val(),
        'DOBRNLicIssuance': $('.txtDOBRNLicIssuance').val(),
        'LastName': $('.txtLastname').val(),
        'FirstName': $('.txtFirstName').val(),
        'MiddleName': $('.txtMiddleName').val(),
        'HomePhoneNumber': $('.txtHomePhoneNumber').val(),
        'WorkPhoneNumber': $('.txtWorkPhoneNumber').val(),
        'CellPhoneNumber': $('.txtCellPhoneNumber').val(),
        'HomeAddress': $('.txtHomeAddress').val(),
        'WorkAddress': $('.txtWorkAddress').val(),
        'CellAddress': $('.txtCellAddress').val(),
        'Gender': $('.rdbGender input[type=radio]:checked').val(),
        'UserControlAddress': message
    };

    executePageMethod('PersonalInformation.aspx', 'SavePersonalInformation', "{'savePersonalInfoVars':" + JSON.stringify(savePersonalInfo) + "}", function (data) {
        if (data.d == null) {
            $('.errMsg').hide();
            $('.errMsg').css("color", "green");
            $('.errMsg').css("border-color", "green");
            $('.errMsg')[0].innerHTML = "Data is successfully saved";
            $('.errMsg').show();
            $('.btnSaveAndContinue').attr('disabled', false);
            $('.hdNew').val("Old");
            if (whichClick) {
                if (flag == "False") {
                    window.location.assign("EmployerInformation.aspx")
                }
                else {
                    if (userRole == "5" || userRole == "6") {
                        window.location.assign("Summary.aspx")
                    }
                    else { window.location.assign("EmployerInformation.aspx") }
                }
            }
        }
        else {
            $('.errMsg').css("color", "red");
            $('.errMsg').css("border-color", "red");
            $('.errMsg')[0].innerHTML = data.d.bolVal.Message;
            $('.hdNew').val("New");
            donotSave = false;
            $('.errMsg').show();
            $('.btnSaveAndContinue').attr('disabled', true);
            return false;
        }
    }, function () {
    });
};




