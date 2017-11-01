$(document).ready(function () {
    $('.errMsg')[0].innerHTML = '';
    $('.errMsg').hide();

    $('.rdbInitial').click(function (e) {
        $('.errMsg')[0].innerHTML = ''
        $('.errMsg').hide();
        GetCertificationDetails();
    });

    $('.rdbRenewal').click(function (e) {
        $('.errMsg')[0].innerHTML = ''
        $('.errMsg').hide();
        GetCertificationDetails();
    });

    $('.rdbAddOn').click(function (e) {
        $('.errMsg')[0].innerHTML = ''
        $('.errMsg').hide();
        GetCertificationDetails();
    });

    $('.btnSaveAndContinue').click(function (e) {
        var flag = ValidationStartPage();
        if (flag == true) {
            $('.errMsg').hide();
            SaveStartPage();
        }
    });
});

var SaveStartPage = function () {
    var saveInfo = {
        'ApplicationID': $('.hdApplicationID').val(),
        'currentRoleInitial': $('.rdbInitial input[type=radio]:checked').val(),
        'currentRoleAddOn': $('.rdbAddOn input[type=radio]:checked').val(),
        'currentRoleRenewal': $('.rdbRenewal input[type=radio]:checked').val()
    };

    executePageMethod('StartPage.aspx', 'SaveStartPageInformation', "{'saveInfo':" + JSON.stringify(saveInfo) + "}", function (data) {
        //alert("Data is successfully saved");
        $('.errMsg').hide();
        $('.errMsg').css("color", "green");
        $('.errMsg').css("border-color", "green");
        $('.errMsg')[0].innerHTML = "Data is successfully saved";
        $('.errMsg').show();
        window.location.assign("PersonalInformation.aspx")
    }, function () {
        //alert('An error occurred while attempting to save this application.');
        $('.errMsg')[0].innerHTML = "An error occurred while attempting to save this application.";
        $('.errMsg').show();
        return false;
    });
};

function ValidationStartPage() {
    var message = '';
    var flag = true;
    if ($('.pnlInitial:visible').length > 0 && $('.rdbInitial input[type=radio]:checked').is(':checked') == false) {
        flag = false;
        var initialMessage = "Please select one of the initial certification to continue further";
        if (message == '') {
            message = initialMessage
        }
        else {
            message = message + "<br/>" + initialMessage
        }
    }
    if ($('.pnlAddOn:visible').length > 0 && $('.rdbAddOn input[type=radio]:checked').is(':checked') == false) {
        flag = false;
        var addMessage = "Please select one of the addon certification to continue further";
        if (message == '') {
            message = addMessage
        }
        else {
            message = message + "<br/>" + addMessage
        }
    }
    if ($('.pnlRenewal:visible').length > 0 && $('.rdbRenewal input[type=radio]:checked').is(':checked') == false) {
        flag = false;
        var renewalMessage = "Please select one of the renewal certification to continue further";
        if (message == '') {
            message = renewalMessage
        }
        else {
            message = message + "<br/>" + renewalMessage
        }
    }
    if ($('.pnlUpdate:visible').length > 0 && $('.rdbUpdate input[type=radio]:checked').is(':checked') == false) {
        flag = false;
        var updateMessage = "Please select the update option";
        if (message == '') {
            message = updateMessage
        }
        else {
            message = message + "<br/>" + updateMessage
        }
    }
    $('.errMsg')[0].innerHTML = message;
    $('.errMsg').show();
    return flag;
}

var GetCertificationDetails = function () {
    var getCertInfo = {
    'role' : $('.hdRNorDDRole').val(),
    'currentRoleInitial': $('.rdbInitial input[type=radio]:checked').val(),
    'currentRoleAddOn': $('.rdbAddOn input[type=radio]:checked').val(),
    'currentRoleRenewal': $('.rdbRenewal input[type=radio]:checked').val()
    };

    executePageMethod('StartPage.aspx', 'GetCertificationDetails', "{'getCertInfo':" + JSON.stringify(getCertInfo) + "}", function (data) {
        if (data.d != null) {
            $('table.grdStart').html(' ');
            var tableToUpdate = $('table.grdStart');
            var dataRow1 = "<tr><th align = center>Certification Type</th><th align = center>Application Type</th><th align = center>Requirements</th></tr>";
            tableToUpdate.append(dataRow1);
            if (data.d.initialCertDescription != "") {
                var dataRow = "<tr><td>" + data.d.certificateType + "</td><td>" + data.d.applicationType + "</td><td>"
                    + data.d.initialCertDescription + "</td></tr>";
                tableToUpdate.append(dataRow);
            }
            if (data.d.renewalCertDescription != "") {
                var dataRow = "<tr><td>" + data.d.certificateType + "</td><td>" + data.d.applicationType + "</td><td>"
                    + data.d.renewalCertDescription + "</td></tr>";
                tableToUpdate.append(dataRow);
            }
            if (data.d.addonCertDescription != "") {
                var dataRow = "<tr><td>" + data.d.certificateType + "</td><td>" + data.d.applicationType + "</td><td>"
                    + data.d.addonCertDescription + "</td></tr>";
                tableToUpdate.append(dataRow);
            }
        }
    }, function () {
        $('.errMsg')[0].innerHTML = "Error in getting certification Details";
        $('.errMsg').show();
        return false;
    });
};