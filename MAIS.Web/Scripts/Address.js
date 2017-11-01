$(document).ready(function () {
    $('.txtPhoneNumber').mask("999-999-9999");
    $('.txtZip').keyup(function () {
        if (this.value.match(/[^0-9]/g)) {
            this.value = this.value.replace(/[^0-9]/g, '');
        }
    });
    $('.txtZipPlus').keyup(function () {
        if (this.value.match(/[^0-9]/g)) {
            this.value = this.value.replace(/[^0-9]/g, '');
        }
    });
});
function AddressValidation() {
    var x = ''
    if ($('.txtAdressLine1').val() == '') {
        x = "Please enter Address Line1"
    }
    if ($('.txtCity').val() == '') {
        var cityMessage = "Please enter city information"
        if (x == '') {
            x = cityMessage;
        }
        else { x = x + "<br/>" + cityMessage; }
    }
    if ($('.txtCity').val() != '') {
        var alphabets = /^[A-Za-z'.\s]+$/;
        var cityValidMessage = "Please enter valid city"
        if (!alphabets.test($('.txtCity').val())) {
            if (x == '') {
                x = cityValidMessage;
            }
            else { x = x + "<br/>" + cityValidMessage; }
        }
    }
    if ($('.ddlState').val() == '') {
        var stateMessage = "Please enter state information"
        if (x == '') {
            x = stateMessage;
        }
        else { x = x + "<br/>" + stateMessage; }
    }
    if ($('.ddlCounty').val() == -1) {
        var countyValidMessage = "Please enter valid county"
        if (x == '') {
            x = countyValidMessage;
        }
        else { x = x + "<br/>" + countyValidMessage; }
    }
    if ($('.txtZip').val() == '') {
        var zipMessage = "Please enter zip code information"
        if (x == '') {
            x = zipMessage;
        }
        else { x = x + "<br/>" + zipMessage; }
    }
    if ($('.txtZip').val() != '') {
        var zipMessage = "Please enter valid zip code information"
        if ($('.txtZip').val().length < 5) {
            if (x == '') {
                x = zipMessage;
            }
            else { x = x + "<br/>" + zipMessage; }
        }
    }
    if ($('.txtZip').val() != '') {
        var numericReg = /^\d*[0-9](|.\d*[0-9]|,\d*[0-9])?$/;
        var zipValidMessage = "Please enter valid zip"
        if (!numericReg.test($('.txtZip').val())) {
            if (x == '') {
                x = zipValidMessage;
            }
            else { x = x + "<br/>" + zipValidMessage; }
        }
    }
    if ($('.txtZipPlus').val() != '') {
        var numericReg = /^\d*[0-9](|.\d*[0-9]|,\d*[0-9])?$/;
        var zipValidMessage = "Please enter valid zipPlus"
        if (!numericReg.test($('.txtZipPlus').val())) {
            if (x == '') {
                x = zipValidMessage;
            }
            else { x = x + "<br/>" + zipValidMessage; }
        }
    }
    if ($('.txtZipPlus').val() != '') {
        var zipPlusMessage = "Please enter valid zip plus code information"
        if ($('.txtZipPlus').val().length < 4) {
            if (x == '') {
                x = zipPlusMessage;
            }
            else { x = x + "<br/>" + zipPlusMessage; }
        }
    }
    return x
}
function AddressValidationForPersonalInformation() {
    var x = ''
    //if ($('.txtAdressLine1').val() == '') {
    //    x = "Please enter Address Line1"
    //}
    //if ($('.txtCity').val() == '') {
    //    var cityMessage = "Please enter city information"
    //    if (x == '') {
    //        x = cityMessage;
    //    }
    //    else { x = x + "<br/>" + cityMessage; }
    //}
    if ($('.txtCity').val() != '') {
        var alphabets = /^[A-Za-z'.\s]+$/;
        var cityValidMessage = "Please enter valid city"
        if (!alphabets.test($('.txtCity').val())) {
            if (x == '') {
                x = cityValidMessage;
            }
            else { x = x + "<br/>" + cityValidMessage; }
        }
    }
    //if ($('.ddlState').val() == '') {
    //    var stateMessage = "Please enter state information"
    //    if (x == '') {
    //        x = stateMessage;
    //    }
    //    else { x = x + "<br/>" + stateMessage; }
    //}
    //if ($('.ddlCounty').val() == -1) {
    //    var countyValidMessage = "Please enter valid county"
    //    if (x == '') {
    //        x = countyValidMessage;
    //    }
    //    else { x = x + "<br/>" + countyValidMessage; }
    //}
    //if ($('.txtZip').val() == '') {
    //    var zipMessage = "Please enter zip code information"
    //    if (x == '') {
    //        x = zipMessage;
    //    }
    //    else { x = x + "<br/>" + zipMessage; }
    //}
    if ($('.txtZip').val() != '') {
        var zipMessage = "Please enter valid zip code information"
        if ($('.txtZip').val().length < 5) {
            if (x == '') {
                x = zipMessage;
            }
            else { x = x + "<br/>" + zipMessage; }
        }
    }
    if ($('.txtZip').val() != '') {
        var numericReg = /^\d*[0-9](|.\d*[0-9]|,\d*[0-9])?$/;
        var zipValidMessage = "Please enter valid zip"
        if (!numericReg.test($('.txtZip').val())) {
            if (x == '') {
                x = zipValidMessage;
            }
            else { x = x + "<br/>" + zipValidMessage; }
        }
    }
    if ($('.txtZipPlus').val() != '') {
        var numericReg = /^\d*[0-9](|.\d*[0-9]|,\d*[0-9])?$/;
        var zipValidMessage = "Please enter valid zipPlus"
        if (!numericReg.test($('.txtZipPlus').val())) {
            if (x == '') {
                x = zipValidMessage;
            }
            else { x = x + "<br/>" + zipValidMessage; }
        }
    }
    if ($('.txtZipPlus').val() != '') {
        var zipPlusMessage = "Please enter valid zip plus code information"
        if ($('.txtZipPlus').val().length < 4) {
            if (x == '') {
                x = zipPlusMessage;
            }
            else { x = x + "<br/>" + zipPlusMessage; }
        }
    }
    return x
}

function GetTheDataFromAbovePanel() {
    $('.txtAdressLine1').val($('.txtAdressLine1').val());
    $('.txtAddressLine2').val($('.txtAdressLine2').val());
    $('.txtCity').val($('.txtCity').val());
    $('.ddlState').val($('.ddlState').val());
    $('.txtZip').val($('.txtZip').val());
    $('.txtZipPlus').val($('.txtZipPlus').val());
    $('.ddlCounty').val($('.ddlCounty').val());
    $('.txtPhoneNumber').val($('.txtPhoneNumber').val());
    $('.txtEmailAddress').val($('.txtEmailAddress').val());
}

function GetAddressFields() {
    var addresslIne2 = $('.txtAddressLine2').val()
    var zipPlus = $('.txtZipPlus').val()
    var county = $('.ddlCounty').val()
    var phoneNumber = $('.txtPhoneNumber').val()
    var email = $('.txtEmailAddress').val()

    if ($('.txtAddressLine2').val() == undefined) {
        addresslIne2 = '';
    }
    if ($('.txtZipPlus').val() == undefined) {
        zipPlus = '';
    }
    if ($('.ddlCounty').val() == undefined) {
        county = '';
    }
    else if ($('.ddlCounty').val() == -1) {
        county = '';
    }
    if ($('.txtPhoneNumber').val() == undefined) {
        phoneNumber = '';
    }
    if ($('.txtEmailAddress').val() == undefined) {
        email = '';
    }
    var address = $('.txtAdressLine1').val() + '*' + addresslIne2 + '*' + $('.txtCity').val() + '*'
    + $('.ddlState').val() + '*' + $('.txtZip').val() + '*' + zipPlus + '*' + county + '*' + phoneNumber + '*' + email;
    return address
}

function AssignTheDataInControl(addressLine1, addressLine2, city, State, zip, zipPlus, County, phone, email) {
    $('.txtAdressLine1').val(addressLine1);
    $('.txtAddressLine2').prop('value', addressLine2);
    $('.txtCity').prop('value', city);
    $('.ddlState').prop('value', State);
    $('.txtZip').prop('value', zip);
    $('.txtZipPlus').prop('value', zipPlus);
    if (County == "") {
        $('.ddlCounty').prop('value', -1);
    }
    else { $('.ddlCounty').prop('value', County); }
    $('.txtPhoneNumber').prop('value', phone);
    $('.txtEmailAddress').prop('value', email);
}

function ClearAddressFields() {
    $('.txtAdressLine1').prop('value', '');
    $('.txtAddressLine2').prop('value', '');
    $('.txtCity').prop('value', '');
    $('.ddlState').prop('value', 35);
    $('.txtZip').prop('value', '');
    $('.txtZipPlus').prop('value', '');
    $('.ddlCounty').prop('value', -1);
    $('.txtPhoneNumber').prop('value', '');
    $('.txtEmailAddress').prop('value', '');
}
function ValidateEmail() {
    var x = ''
    if ($('.txtEmailAddress').val() != '') {
        var emailAddressRegx = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (!emailAddressRegx.test($('.txtEmailAddress').val())) {
            var EmailAddress = "Please enter valid email address";
            if (x == '') {
                x = EmailAddress;
            }
            else { x = x + "<br/>" + EmailAddress; }
        }
    }
    return x
}
function ValidatePhone() {
    var x = ''
    if ($('.txtPhoneNumber').val() != '') {
        var phoneNumberPattern = /^\(?(\d{3})\)?[- ]?(\d{3})[- ]?(\d{4})$/;
        var phoneMessage = "Please enter valid phone number";
        if (!phoneNumberPattern.test($('.txtPhoneNumber').val())) {
            if (x == '') {
                x = phoneMessage;
            }
            else { x = x + "<br/>" + phoneMessage; }
        }
    }
    return x
}

function ValidateEmail1(email, flag) {
    var x = ''
    if (email != '') {
        var emailAddressRegx = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (!emailAddressRegx.test(email)) {
            var related = ""
            if (flag == 1) {
                related = "for work location address"
            }
            else { related = "for agency address" }
            var EmailAddress = "Please enter valid email address" + " " + related;
            if (x == '') {
                x = EmailAddress;
            }
            else { x = x + "<br/>" + EmailAddress; }
        }
    }
    return x
}
function ValidatePhone1(phoneNumber, flag) {
    var x = ''
    if (phoneNumber != '') {
        var phoneNumberPattern = /^\(?(\d{3})\)?[- ]?(\d{3})[- ]?(\d{4})$/;
        var phoneMessage = "Please enter valid phone number";
        if (!phoneNumberPattern.test(phoneNumber)) {
            var related = ""
            if (flag == 1) {
                related = "for work location address"
            }
            else { related = "for agency address" }
            if (x == '') {
                x = phoneMessage + " " + related;
            }
            else { x = x + "<br/>" + phoneMessage; }
        }
    }
    return x
}

function ManadatoryPhoneEmail(phoneNumber, email, flag) {
    var message = ''
    var related = ""
    if (flag == 1) {
        related = "for work location address"
    }
    else { related = "for agency address" }
    if (phoneNumber == '') {
        message = message + "<br/>" + "Please enter manadatory field Phone Number." + " " + related;
    }
    else {
        message = ValidatePhone1(phoneNumber, flag);
    }
    if (email == '') {
        message = message + "<br/>" + "Please enter manadatory field Email Address." + " " + related;
    }
    else {
        message = ValidateEmail1(email, flag);
    }
    return message;
}

function AddressValidationForEmployer(addressLine1, city, state, county, zip, zipPlus, flag) {
    var x = ''
    var related = ""
    if (flag == 1) {
        related = "for work location address"
    }
    else { related = "for agency address" }
    //if (addressLine1 == '') {
    //    x = "Please enter Address Line1" + " " + related
    //}
    //if (city == '') {
    //    var cityMessage = "Please enter city information" + " " + related
    //    if (x == '') {
    //        x = cityMessage;
    //    }
    //    else { x = x + "<br/>" + cityMessage; }
    //}
    if (city != '') {
        var alphabets = /^[A-Za-z'.\s]+$/;
        var cityValidMessage = "Please enter valid city" + " " + related
        if (!alphabets.test($('.txtCity').val())) {
            if (x == '') {
                x = cityValidMessage;
            }
            else { x = x + "<br/>" + cityValidMessage; }
        }
    }
    if (state == '') {
        var stateMessage = "Please enter state information" + " " + related
        if (x == '') {
            x = stateMessage;
        }
        else { x = x + "<br/>" + stateMessage; }
    }
    //if (county == -1) {
    //    var countyValidMessage = "Please enter valid county" + " " + related
    //    if (x == '') {
    //        x = countyValidMessage;
    //    }
    //    else { x = x + "<br/>" + countyValidMessage; }
    //}
    //if (zip == '') {
    //    var zipMessage = "Please enter zip code information" + " " + related
    //    if (x == '') {
    //        x = zipMessage;
    //    }
    //    else { x = x + "<br/>" + zipMessage; }
    //}
    if (zip != '') {
        var zipMessage = "Please enter valid zip code information" + " " + related
        if ($('.txtZip').val().length < 5) {
            if (x == '') {
                x = zipMessage;
            }
            else { x = x + "<br/>" + zipMessage; }
        }
    }
    if (zip != '') {
        var numericReg = /^\d*[0-9](|.\d*[0-9]|,\d*[0-9])?$/;
        var zipValidMessage = "Please enter valid zip" + " " + related
        if (!numericReg.test($('.txtZip').val())) {
            if (x == '') {
                x = zipValidMessage;
            }
            else { x = x + "<br/>" + zipValidMessage; }
        }
    }
    if (zipPlus != '') {
        var numericReg = /^\d*[0-9](|.\d*[0-9]|,\d*[0-9])?$/;
        var zipValidMessage = "Please enter valid zipPlus" + " " + related
        if (!numericReg.test($('.txtZipPlus').val())) {
            if (x == '') {
                x = zipValidMessage;
            }
            else { x = x + "<br/>" + zipValidMessage; }
        }
    }
    if (zipPlus != '') {
        var zipPlusMessage = "Please enter valid zip plus code information" + " " + related
        if ($('.txtZipPlus').val().length < 4) {
            if (x == '') {
                x = zipPlusMessage;
            }
            else { x = x + "<br/>" + zipPlusMessage; }
        }
    }
    return x
}