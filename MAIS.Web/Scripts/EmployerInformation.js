var call_rowHtml = '<tr class="data"><td class = "provname"><a href="#" class="expandLink" style="display:block;width:100%">{0}</a></td><td class = "name">{1}</td><td class = "contractname">{2}</td></tr>';
var formatHtml = function () {
    for (var i = 1; i < arguments.length; i++) {
        var exp = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        arguments[0] = arguments[0].replace(exp, arguments[i]);
    }
    return arguments[0];
};
var historyData = "False"
var integerVariable = 1
var supervisorWorkLocationEndDates = 1
var isDurty = false;

$(document).ready(function () {
    $('.txtSuperVisorPhoneNumber').mask("999-999-9999");    
    $('.txtCertStartDate').datepick();
    $('.txtCertEndDate').datepick();    
    $('.txtEmploymentStartDate').datepick();
    $('.txtEmploymentEndDate').datepick();
    $('.txtSuperVisorStartDate').datepick();
    $('.txtSuperVisorEndDate').datepick();
    $('.txtWorkLocationStartDate').datepick();
    $('.txtWorkLocationEndDate').datepick();
    $('.txtDODDProvider').mask("9999999");
    searchClick();
    $('.lblFound').hide();
    radiobuttonClick();
    checkboxClick();
    checkboxClick1();
    integerVariable = 1
    supervisorWorkLocationEndDates = 1
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_pageLoaded(function () {
        $('.pnlEmployer').css('visibility', 'visible');
        $('.txtSuperVisorPhoneNumber').mask("999-999-9999");
        $('.txtEmploymentStartDate').datepick();
        $('.txtEmploymentEndDate').datepick();
        $('.txtSuperVisorStartDate').datepick();
        $('.txtSuperVisorEndDate').datepick();
        $('.txtWorkLocationStartDate').datepick();
        $('.txtWorkLocationEndDate').datepick();
        $('.pnlMainAddress .txtPhoneNumber').mask("999-999-9999");
        $('.txtDODDProvider').mask("9999999");
        if (integerVariable == 2) {
            $('.pnlDD').attr('disabled', 'disabled');
            $('.rblSelect').attr('disabled', 'disabled');
            $('.pnlEmployer').attr('disabled', 'disabled');
            $('.txtDODDProvider').attr('disabled', 'disabled');
            $('.btnSearch').attr('disabled', 'disabled');
        }
        else {
            $('.pnlDD').attr('disabled', false);
            $('.rblSelect').attr('disabled', false);
            $('.pnlEmployer').attr('disabled', false);
            $('.txtDODDProvider').attr('disabled', false);
            $('.btnSearch').attr('disabled', false);
            var chkflg = $('.rblSelect input[type=radio]:checked').val();
            if (chkflg == 1) {
                $('.pnlEmployer').hide();
            }
            if (chkflg == 2) {
                $('.pnlEmployer').hide();
            }
        }
        if (supervisorWorkLocationEndDates == 2) {
            if ($('.txtSuperVisorEndDate').val().length != 0) {
                $('.txtSuperVisorEndDate').attr('disabled', true);
            }
            else { $('.txtSuperVisorEndDate').attr('disabled', false); }
            if ($('.txtWorkLocationEndDate').val().length != 0) {
                $('.txtWorkLocationEndDate').attr('disabled', true);
            }
            else { $('.txtWorkLocationEndDate').attr('disabled', false); }
        }
    });

 

   
    //$('.btnSaveAdditional').click(function () {   
    //    if (isDurty == true) {
    //        var mesg = 'You have unsaved changes. Do you want to save first?';
    //        if (confirm(mesg) == true) {
    //            return false;
    //        }
    //        else {
    //            return true
    //            }
           
    //    }
    //    else {
    //        return;
    //    }
    //});
});

function pageLoad() {
    $(function ($) {
        $('.isDateChange').blur(function () {
            isDurty = true;
            ValidationInClientSide($(this));
        });
    });

    $(function ($) {
        $('.isTextChange').change(function () {
            isDurty = true;
            ValidationInClientSide($(this));
        });
    });

    //txtSupervisorLastName
    $('.txtSupervisorLastName, .txtSupervisorFirstName').keypress(function (event) {
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
};
//$('.txtEmploymentStartDate').click(function () {
//    alert('You click');
//});

function TestIfDataSaved(e) {
    if (isDurty == true) { 
        var mesg = 'You have unsaved changes. If you want to save click Yes. Then click the Save at the bottom of the form.';
                if (confirm(mesg) == true) {
                    //e.preventDefault();
                    return false;
                    
                }
                else {
                    isDurty = false;
                    return true;
                    } 
    } 
    else {
        return true;
    }
}

function searchClick() {
    $('.txtDODDProvider').keydown(function (event) {
        integerVariable = 1
        supervisorWorkLocationEndDates = 1
        if (event.keyCode == 13) {
            $('.btnSearch').trigger('click');
            event.preventDefault()
        }
    });

    $('.btnSearch').click(function (e) {
        //get the result and attach to the textbox in the form
        integerVariable = 1
        supervisorWorkLocationEndDates = 1
        $('.errMsg').hide();
        $('.lblFound').hide();
        var chkflg = $('.rblSelect input[type=radio]:checked').val();
        var flag = true;
        if (chkflg == undefined) {
            flag = false;
            $('.errMsg')[0].innerHTML = "Please select any one of the employer selection groupbox";
            $('.errMsg').show();
        }
        //if ($('.txtDODDProvider').val() == '') {
        //    $('.errMsg')[0].innerHTML = "Please enter the DD provider number";
        //    $('.errMsg').show();
        //    flag = false;
        //}

        if (flag == true) {
            //$('.txtCertStartDate').attr('disabled', 'disabled');
            //$('.txtCertEndDate').attr('disabled', 'disabled');
            //$('.txtCertStatus').attr('disabled', 'disabled');
            //$('.txtCEOLastName').attr('disabled', 'disabled');
            //$('.txtCEOFristName').attr('disabled', 'disabled');
            //$('.txtCEOMiddleName').attr('disabled', 'disabled');
            //$('.txtEmployerTaxID').attr('disabled', 'disabled');
            //$('.txtEmployerName').attr('disabled', 'disabled');
            if ($('.txtDODDProvider').val() != '') {
                GetAgencyInformation();
            }           
        }
    });

    $('tr.data').live("click", function (e) {
        integerVariable = 1
        supervisorWorkLocationEndDates = 1
        $('.errMsg').hide();
        var prov = $(this).find('td.contractname').text()
        GetEmployerInformation(prov);
    });
}

function checkboxClick() {
    $('.chkAgency').click(function (e) {
        integerVariable = 1
        supervisorWorkLocationEndDates = 1
        var html = $('.pnlMainAddress')[0].innerHTML;
        $('.lblFound').hide();
        if ($('.chkAgency').is(':unchecked')) {
            $('.pnlMainAddress .txtAdressLine1')[1].value = ""
            $('.pnlMainAddress .txtAddressLine2')[1].value = ""
            $('.pnlMainAddress .txtCity')[1].value = ""
            $('.pnlMainAddress .ddlCounty')[1].value = -1
            $('.pnlMainAddress .ddlState')[1].value = 35
            $('.pnlMainAddress .txtZip')[1].value = ""
            $('.pnlMainAddress .txtZipPlus')[1].value = ""
            $('.pnlMainAddress .txtPhoneNumber')[1].value = ""
            $('.pnlMainAddress .txtEmailAddress')[1].value = ""

        }
        else { $('.pnlMainAddress')[1].innerHTML = html }
        $('.pnlMainAddress .txtAdressLine1')[1].disabled = false;
        $('.pnlMainAddress .txtAddressLine2')[1].disabled = false;
        $('.pnlMainAddress .txtCity')[1].disabled = false;
        $('.pnlMainAddress .ddlCounty')[1].disabled = false;
        $('.pnlMainAddress .ddlState')[1].disabled = false;
        $('.pnlMainAddress .txtZip')[1].disabled = false;
        $('.pnlMainAddress .txtZipPlus')[1].disabled = false;
        $('.pnlMainAddress .txtPhoneNumber')[1].disabled = false;
        $('.pnlMainAddress .txtEmailAddress')[1].disabled = false;
    });
}

function checkboxClick1() {
    $('.chkAgencyAddress').click(function (e) {
        integerVariable = 1
        supervisorWorkLocationEndDates = 1
        var html = $('.pnlMainAddress')[0].innerHTML;
        $('.lblFound').hide();
        if ($('.chkAgencyAddress').is(':unchecked')) {
            $('.chkAgency').attr("checked", false);
            ClearAddressFields();
        }
        //else {
        //    $('.pnlMainAddress')[1].innerHTML = html
        //    $('.pnlMainAddress')[0].innerHTML = html
        //}
    });
}

function radiobuttonClick() {
    $('.rblSelect').click(function (e) {
        integerVariable = 1
        supervisorWorkLocationEndDates = 1
        var chkflg = $('.rblSelect input[type=radio]:checked').val();
        ClearData()
        $('.lblFound').hide();
        if (chkflg == 3) {
            $('.errMsg').hide();
            $('.pnlEmployer').show();
            $('.pnlEmployer').css('visibility', 'visible');
            $('.pnlEmployer').attr('disabled', false);
            $('.txtDODDProvider').attr('disabled', false);
            $('.btnSearch').attr('disabled', false);
            $('table.grdEmp').hide();
            $('.hdEmployerID').val('');
            //$('.txtEmployerAgencyInformation').val('');
            $('.txtDODDProvider').val('');
            $('.tdTaxID').html("Provider#:");
            $('.txtEmployerTaxID').unmask();
            $('.pnlProgressBar').attr('disabled', 'disabled');
            $('.pnlMainAddress .txtAdressLine1')[0].disabled = true;
            $('.pnlMainAddress .txtAddressLine2')[0].disabled = true;
            $('.pnlMainAddress .txtCity')[0].disabled = true;
            $('.pnlMainAddress .ddlCounty')[0].disabled = true;
            $('.pnlMainAddress .ddlState')[0].disabled = true;
            $('.pnlMainAddress .txtZip')[0].disabled = true;
            $('.pnlMainAddress .txtZipPlus')[0].disabled = true;
            $('.pnlMainAddress .txtPhoneNumber')[0].disabled = true;
            $('.pnlMainAddress .txtEmailAddress')[0].disabled = true;
            $('.txtCertStartDate').attr('disabled', 'disabled');
            $('.txtCertEndDate').attr('disabled', 'disabled');
            $('.txtCertStatus').attr('disabled', 'disabled');
            $('.txtCEOLastName').attr('disabled', 'disabled');
            $('.txtCEOFristName').attr('disabled', 'disabled');
            $('.txtCEOMiddleName').attr('disabled', 'disabled');
            $('.txtEmployerTaxID').attr('disabled', 'disabled');
            $('.txtEmployerName').attr('disabled', 'disabled');
        }
        if (chkflg == 4) {
            $('.errMsg').hide();
            $('.pnlEmployer').show();
            $('.pnlEmployer').css({ 'visibility': 'visible' });
            $('.hdEmployerID').val('');
            $('.pnlEmployer').attr('disabled', false);
            $('.txtDODDProvider').attr('disabled', false);
            $('.btnSearch').attr('disabled', false);
            $('table.grdEmp').hide();
            //$('.txtEmployerAgencyInformation').val('');
            $('.pnlProgressBar').attr('disabled', 'disabled');
            $('.pnlMainAddress .txtAdressLine1')[0].disabled = true;
            $('.pnlMainAddress .txtAddressLine2')[0].disabled = true;
            $('.pnlMainAddress .txtCity')[0].disabled = true;
            $('.pnlMainAddress .ddlCounty')[0].disabled = true;
            $('.pnlMainAddress .ddlState')[0].disabled = true;
            $('.pnlMainAddress .txtZip')[0].disabled = true;
            $('.pnlMainAddress .txtZipPlus')[0].disabled = true;
            $('.pnlMainAddress .txtPhoneNumber')[0].disabled = true;
            $('.pnlMainAddress .txtEmailAddress')[0].disabled = true;
            $('.txtDODDProvider').val('');
            $('.tdTaxID').html("Provider#:");
            $('.txtEmployerTaxID').unmask();
            $('.txtCertStartDate').attr('disabled', 'disabled');
            $('.txtCertEndDate').attr('disabled', 'disabled');
            $('.txtCertStatus').attr('disabled', 'disabled');
            $('.txtCEOLastName').attr('disabled', 'disabled');
            $('.txtCEOFristName').attr('disabled', 'disabled');
            $('.txtCEOMiddleName').attr('disabled', 'disabled');
            $('.txtEmployerTaxID').attr('disabled', 'disabled');
            $('.txtEmployerName').attr('disabled', 'disabled');
        }
        if (chkflg == 5) {
            $('.errMsg').hide();
            $('.pnlEmployer').show();
            $('.pnlEmployer').css({ 'visibility': 'visible' });
            $('.hdEmployerID').val('');
            $('.pnlEmployer').attr('disabled', false);
            $('.txtDODDProvider').attr('disabled', false);
            $('.btnSearch').attr('disabled', false);
            $('table.grdEmp').hide();
            //$('.txtEmployerAgencyInformation').val('');
            $('.pnlProgressBar').attr('disabled', false);
            $('.pnlMainAddress .txtAdressLine1')[0].disabled = false;
            $('.pnlMainAddress .txtAddressLine2')[0].disabled = false;
            $('.pnlMainAddress .txtCity')[0].disabled = false;
            $('.pnlMainAddress .ddlCounty')[0].disabled = false;
            $('.pnlMainAddress .ddlState')[0].disabled = false;
            $('.pnlMainAddress .txtZip')[0].disabled = false;
            $('.pnlMainAddress .txtZipPlus')[0].disabled = false;
            $('.pnlMainAddress .txtPhoneNumber')[0].disabled = false;
            $('.pnlMainAddress .txtEmailAddress')[0].disabled = false;
            $('.txtDODDProvider').val('');
            $('.tdTaxID').html("Provider#:");
            $('.txtEmployerTaxID').unmask();
            $('.txtCertStartDate').attr('disabled', false);
            $('.txtCertEndDate').attr('disabled', false);
            $('.txtCertStatus').attr('disabled', false);
            $('.txtCEOLastName').attr('disabled', false);
            $('.txtCEOFristName').attr('disabled', false);
            $('.txtCEOMiddleName').attr('disabled', false);
            $('.txtEmployerTaxID').attr('disabled', false);
            $('.txtEmployerName').attr('disabled', false);
            $('.chkAgencyAddress').attr('disabled', true);
        }
        if (chkflg == 1) {
            $('.errMsg').hide();
            $('.pnlEmployer').hide();
            $('.pnlEmployer').css('visibility', 'hidden');
            $('.hdEmployerID').val('');
            $('.tdTaxID').html("RN License#:");
            $('table.grdEmp').hide();
            $('.pnlEmployer').attr('disabled', true);
            $('.txtDODDProvider').attr('disabled', true);
            $('.txtEmployerTaxID').mask("RN999999");
           // $('.txtDODDProvider').mask("9999999");
            $('.btnSearch').attr('disabled', true);
            $('.pnlProgressBar').attr('disabled', false);
            $('.pnlMainAddress .txtAdressLine1').attr('disabled', false);
            $('.pnlMainAddress .txtAddressLine2').attr('disabled', false);
            $('.pnlMainAddress .txtCity').attr('disabled', false);
            $('.pnlMainAddress .ddlCounty').attr('disabled', false);
            $('.pnlMainAddress .ddlState').attr('disabled', false);
            $('.pnlMainAddress .txtZip').attr('disabled', false);
            $('.pnlMainAddress .txtZipPlus').attr('disabled', false);
            $('.pnlMainAddress .txtPhoneNumber').attr('disabled', false);
            $('.pnlMainAddress .txtEmailAddress').attr('disabled', false);
            $('.chkAgencyAddress').attr('disabled', false);
            $('.txtEmployerName').attr('disabled', 'disabled');
            $('.txtEmployerTaxID').attr('disabled', 'disabled');
            $('.txtCEOLastName').attr('disabled', 'disabled');
            $('.txtCEOFristName').attr('disabled', 'disabled');
            $('.txtCEOMiddleName').attr('disabled', 'disabled');
            $('.txtCertStartDate').attr('disabled', 'disabled');
            $('.txtCertEndDate').attr('disabled', 'disabled');
            $('.txtCertStatus').attr('disabled', 'disabled');
            GetRNInformation();
        }
        if (chkflg == 2) {
            $('.errMsg').hide();
            $('.pnlEmployer').hide();
            $('.pnlEmployer').css('visibility', 'hidden');
            $('.hdEmployerID').val('');
            $('.tdTaxID').html("RN License#:");
            $('.txtEmployerTaxID').mask("RN999999");
           // $('.txtDODDProvider').mask("RN999999");
            $('table.grdEmp').hide();
            $('.pnlEmployer').attr('disabled', true);
            $('.txtDODDProvider').attr('disabled', true);
            $('.btnSearch').attr('disabled', true);
            $('.pnlProgressBar').attr('disabled', false);
            $('.pnlMainAddress .txtAdressLine1').attr('disabled', false);
            $('.pnlMainAddress .txtAddressLine2').attr('disabled', false);
            $('.pnlMainAddress .txtCity').attr('disabled', false);
            $('.pnlMainAddress .ddlCounty').attr('disabled', false);
            $('.pnlMainAddress .ddlState').attr('disabled', false);
            $('.pnlMainAddress .txtZip').attr('disabled', false);
            $('.pnlMainAddress .txtZipPlus').attr('disabled', false);
            $('.pnlMainAddress .txtPhoneNumber').attr('disabled', false);
            $('.pnlMainAddress .txtEmailAddress').attr('disabled', false);
            $('.chkAgencyAddress').attr('disabled', false);
            $('.txtEmployerName').attr('disabled', false);
            $('.txtEmployerTaxID').attr('disabled', false);
            $('.txtCEOLastName').attr('disabled', false);
            $('.txtCEOFristName').attr('disabled', false);
            $('.txtCEOMiddleName').attr('disabled', false);
            $('.txtCertStartDate').attr('disabled', true);
            $('.txtCertEndDate').attr('disabled', true);
            $('.txtCertStatus').attr('disabled', true);
            $('.chkAgencyAddress').attr('disabled', true);
        }
    });
}

function gridClick() {
    $('.grdRecent').click(function (e) {
        integerVariable = 1
        supervisorWorkLocationEndDates = 1
        $('.lblFound').hide();
        $('.errMsg')[0].innerHTML = ""
        $('.errMsg').hide();
        $('.pnlEmployer').css({ 'visibility': 'visible' });
        $('.pnlEmployer').attr('disabled', false);
        $('.txtDODDProvider').attr('disabled', false);
        $('.btnSearch').attr('disabled', false);
        var chkflg = $('.rblSelect input[type=radio]:checked').val();
        if (chkflg == 3) {
            $('.errMsg').hide();
            $('.pnlEmployer').show();
            $('.pnlEmployer').css('visibility', 'visible');
            $('.pnlEmployer').attr('disabled', false);
            $('.txtDODDProvider').attr('disabled', false);
            $('.btnSearch').attr('disabled', false);
            $('table.grdEmp').hide();
            $('.hdEmployerID').val('');
            //$('.txtEmployerAgencyInformation').val('');
            $('.txtDODDProvider').val('');
            $('.tdTaxID').html("Provider#:");
            $('.pnlProgressBar').attr('disabled', 'disabled');
            $('.pnlMainAddress .txtAdressLine1')[0].disabled = true;
            $('.pnlMainAddress .txtAddressLine2')[0].disabled = true;
            $('.pnlMainAddress .txtCity')[0].disabled = true;
            $('.pnlMainAddress .ddlCounty')[0].disabled = true;
            $('.pnlMainAddress .ddlState')[0].disabled = true;
            $('.pnlMainAddress .txtZip')[0].disabled = true;
            $('.pnlMainAddress .txtZipPlus')[0].disabled = true;
            $('.pnlMainAddress .txtPhoneNumber')[0].disabled = true;
            $('.pnlMainAddress .txtEmailAddress')[0].disabled = true;
            $('.txtCertStartDate').attr('disabled', 'disabled');
            $('.txtCertEndDate').attr('disabled', 'disabled');
            $('.txtCertStatus').attr('disabled', 'disabled');
            $('.txtCEOLastName').attr('disabled', 'disabled');
            $('.txtCEOFristName').attr('disabled', 'disabled');
            $('.txtCEOMiddleName').attr('disabled', 'disabled');
            $('.txtEmployerTaxID').attr('disabled', 'disabled');
            $('.txtEmployerName').attr('disabled', 'disabled');
        }
        if (chkflg == 4) {
            $('.errMsg').hide();
            $('.pnlEmployer').show();
            $('.pnlEmployer').css({ 'visibility': 'visible' });
            $('.hdEmployerID').val('');
            $('.pnlEmployer').attr('disabled', false);
            $('.txtDODDProvider').attr('disabled', false);
            $('.btnSearch').attr('disabled', false);
            $('table.grdEmp').hide();
            //$('.txtEmployerAgencyInformation').val('');
            $('.pnlProgressBar').attr('disabled', 'disabled');
            $('.pnlMainAddress .txtAdressLine1')[0].disabled = true;
            $('.pnlMainAddress .txtAddressLine2')[0].disabled = true;
            $('.pnlMainAddress .txtCity')[0].disabled = true;
            $('.pnlMainAddress .ddlCounty')[0].disabled = true;
            $('.pnlMainAddress .ddlState')[0].disabled = true;
            $('.pnlMainAddress .txtZip')[0].disabled = true;
            $('.pnlMainAddress .txtZipPlus')[0].disabled = true;
            $('.pnlMainAddress .txtPhoneNumber')[0].disabled = true;
            $('.pnlMainAddress .txtEmailAddress')[0].disabled = true;
            $('.txtDODDProvider').val('');
            $('.tdTaxID').html("Provider#:");
            $('.txtCertStartDate').attr('disabled', 'disabled');
            $('.txtCertEndDate').attr('disabled', 'disabled');
            $('.txtCertStatus').attr('disabled', 'disabled');
            $('.txtCEOLastName').attr('disabled', 'disabled');
            $('.txtCEOFristName').attr('disabled', 'disabled');
            $('.txtCEOMiddleName').attr('disabled', 'disabled');
            $('.txtEmployerTaxID').attr('disabled', 'disabled');
            $('.txtEmployerName').attr('disabled', 'disabled');
        }
        if (chkflg == 5) {
            $('.errMsg').hide();
            $('.pnlEmployer').show();
            $('.pnlEmployer').css({ 'visibility': 'visible' });
            $('.hdEmployerID').val('');
            $('.pnlEmployer').attr('disabled', false);
            $('.txtDODDProvider').attr('disabled', false);
            $('.btnSearch').attr('disabled', false);
            $('table.grdEmp').hide();
            //$('.txtEmployerAgencyInformation').val('');
            $('.pnlProgressBar').attr('disabled', false);
            $('.pnlMainAddress .txtAdressLine1')[0].disabled = false;
            $('.pnlMainAddress .txtAddressLine2')[0].disabled = false;
            $('.pnlMainAddress .txtCity')[0].disabled = false;
            $('.pnlMainAddress .ddlCounty')[0].disabled = false;
            $('.pnlMainAddress .ddlState')[0].disabled = false;
            $('.pnlMainAddress .txtZip')[0].disabled = false;
            $('.pnlMainAddress .txtZipPlus')[0].disabled = false;
            $('.pnlMainAddress .txtPhoneNumber')[0].disabled = false;
            $('.pnlMainAddress .txtEmailAddress')[0].disabled = false;
            $('.txtDODDProvider').val('');
            $('.tdTaxID').html("Provider#:");
            $('.txtCertStartDate').attr('disabled', false);
            $('.txtCertEndDate').attr('disabled', false);
            $('.txtCertStatus').attr('disabled', false);
            $('.txtCEOLastName').attr('disabled', false);
            $('.txtCEOFristName').attr('disabled', false);
            $('.txtCEOMiddleName').attr('disabled', false);
            $('.txtEmployerTaxID').attr('disabled', false);
            $('.txtEmployerName').attr('disabled', false);
            $('.chkAgencyAddress').attr('disabled', true);
        }
        if (chkflg == 1) {
            $('.errMsg').hide();
            $('.pnlEmployer').hide();
            $('.pnlEmployer').css('visibility', 'hidden');
            $('.hdEmployerID').val('');
            $('.tdTaxID').html("RN License#:");
           // $('.txtDODDProvider').mask("RN999999");
            $('table.grdEmp').hide();
            $('.pnlEmployer').attr('disabled', true);
            $('.txtDODDProvider').attr('disabled', true);
            $('.btnSearch').attr('disabled', true);
            $('.pnlProgressBar').attr('disabled', false);
            $('.pnlMainAddress .txtAdressLine1').attr('disabled', false);
            $('.pnlMainAddress .txtAddressLine2').attr('disabled', false);
            $('.pnlMainAddress .txtCity').attr('disabled', false);
            $('.pnlMainAddress .ddlCounty').attr('disabled', false);
            $('.pnlMainAddress .ddlState').attr('disabled', false);
            $('.pnlMainAddress .txtZip').attr('disabled', false);
            $('.pnlMainAddress .txtZipPlus').attr('disabled', false);
            $('.pnlMainAddress .txtPhoneNumber').attr('disabled', false);
            $('.pnlMainAddress .txtEmailAddress').attr('disabled', false);
            $('.chkAgencyAddress').attr('disabled', false);
            $('.txtEmployerName').attr('disabled', 'disabled');
            $('.txtEmployerTaxID').attr('disabled', 'disabled');
            $('.txtCEOLastName').attr('disabled', 'disabled');
            $('.txtCEOFristName').attr('disabled', 'disabled');
            $('.txtCEOMiddleName').attr('disabled', 'disabled');
            $('.txtCertStartDate').attr('disabled', 'disabled');
            $('.txtCertEndDate').attr('disabled', 'disabled');
            $('.txtCertStatus').attr('disabled', 'disabled');
            GetRNInformation();
        }
        if (chkflg == 2) {
            $('.errMsg').hide();
            $('.pnlEmployer').hide();
            $('.pnlEmployer').css('visibility', 'hidden');
            $('.hdEmployerID').val('');
            $('.tdTaxID').html("RN License#:");
          //  $('.txtDODDProvider').mask("RN999999");
            $('table.grdEmp').hide();
            $('.pnlEmployer').attr('disabled', true);
            $('.txtDODDProvider').attr('disabled', true);
            $('.btnSearch').attr('disabled', true);
            $('.pnlProgressBar').attr('disabled', false);
            $('.pnlMainAddress .txtAdressLine1').attr('disabled', false);
            $('.pnlMainAddress .txtAddressLine2').attr('disabled', false);
            $('.pnlMainAddress .txtCity').attr('disabled', false);
            $('.pnlMainAddress .ddlCounty').attr('disabled', false);
            $('.pnlMainAddress .ddlState').attr('disabled', false);
            $('.pnlMainAddress .txtZip').attr('disabled', false);
            $('.pnlMainAddress .txtZipPlus').attr('disabled', false);
            $('.pnlMainAddress .txtPhoneNumber').attr('disabled', false);
            $('.pnlMainAddress .txtEmailAddress').attr('disabled', false);
            $('.chkAgencyAddress').attr('disabled', false);
            $('.txtEmployerName').attr('disabled', false);
            $('.txtEmployerTaxID').attr('disabled', false);
            $('.txtCEOLastName').attr('disabled', false);
            $('.txtCEOFristName').attr('disabled', false);
            $('.txtCEOMiddleName').attr('disabled', false);
            $('.txtCertStartDate').attr('disabled', true);
            $('.txtCertEndDate').attr('disabled', true);
            $('.txtCertStatus').attr('disabled', true);
            $('.chkAgencyAddress').attr('disabled', true);
        }        
    });
    $('.grdViewHistory').click(function (e) {
        integerVariable = 1
        supervisorWorkLocationEndDates = 1
        $('.lblFound').hide();
        $('.errMsg')[0].innerHTML = ""
        $('.errMsg').hide();
        historyData = "True"
        $('.pnlEmployer').css({ 'visibility': 'visible' });
        $('.pnlEmployer').attr('disabled', false);
        $('.txtDODDProvider').attr('disabled', false);
        $('.btnSearch').attr('disabled', false);
    });
}

function buttonClick() {
    var additional = false
    $('.lblFound').hide();
    $('.btnSave').click(function (e) {
        integerVariable = 1
        supervisorWorkLocationEndDates = 1
        //e.preventDefault();
        $('.errMsg').css("color", "red");
        $('.errMsg').css("border-color", "red");
      //  if (historyData == "False") {
            var flag = ValidationInClientSide(e);
            if (flag == true) {
                $('.errMsg').hide();
                SaveEmployerInformation(e, additional);
                isDurty = false;
            }
       // }
    });

    $('.btnSaveAdditional').click(function (e) {
        if (isDurty == false) {
            integerVariable = 1
            supervisorWorkLocationEndDates = 1
            $('.errMsg').hide();
            $('.errMsg').css("color", "red");
            $('.errMsg').css("border-color", "red");
            additional = true;
            ClearData();
            isDurty = false;
            window.location.reload();
        }
    });

    //$('.btnSaveAndContinue').click(function (e) {
    //    $('.errMsg').css("color", "red");
    //    $('.errMsg').css("border-color", "red");
    //    var flag = ValidationInClientSide(e);
    //    trueOrFalse = true;
    //    if (flag == true) {
    //        $('.errMsg').hide();
    //        if ($('.txtEmploymentEndDate').val().length == 0) {
    //            var date = '12/31/9999';
    //            $('.txtEmploymentEndDate').val(date);
    //        }
    //        SaveEmployerInformation(e, trueOrFalse, additional);
    //        //if ($('.errMsg')[0].innerHTML != "Data is successfully saved") {
    //        //    e.preventDefault()
    //        //}
    //    }
    //});
}

function SaveEmployerInformation(e, additional) {
    var employerId = 0
    var contractNumber = $('.txtDODDProvider').val()
    if ($('.hdEmployerID').val() != undefined && $('.hdEmployerID').val() != "") {
        employerId = $('.hdEmployerID').val();
    }
    if ($('.txtDODDProvider').val() == undefined) {
        contractNumber = ""
    }
    var date = '12/31/9999';
    var employmentEndDate = $('.txtEmploymentEndDate').val()
    var supervisorEndDate = $('.txtSuperVisorEndDate').val()
    var workLocationEndDate = $('.txtWorkLocationEndDate').val()
    var employmentStartDate = $('.txtEmploymentStartDate').val()
    var supervisorStartDate = $('.txtSuperVisorStartDate').val()
    var workLocationStartDate = $('.txtWorkLocationStartDate').val()
    if ($('.txtEmploymentEndDate').val().length == 0) {
        employmentEndDate = date;
    }
    if ($('.txtSuperVisorEndDate').val().length == 0) {
        supervisorEndDate = date;
    }
    if ($('.txtWorkLocationEndDate').val().length == 0) {
        workLocationEndDate = date;
    }
    if ($('.txtEmploymentStartDate').val().length == 0) {
        employmentStartDate = date;
    }
    if ($('.txtSuperVisorStartDate').val().length == 0) {
        supervisorStartDate = date;
    }
    if ($('.txtWorkLocationStartDate').val().length == 0) {
        workLocationStartDate = date;
    }
    var saveInfo = {
        'EmployerName': $('.txtEmployerName').val(),
        'IdentificationValue': $('.txtEmployerTaxID').val(),
        'IdentificationType': $('.tdTaxID').html(),
        'ContractNumber': contractNumber,
        'EmployerType': $('.rblSelect input[type=radio]:checked').val(),
        'EmployerStartDate': employmentStartDate,
        'EmployerEndDate': employmentEndDate,
        'CEOLastName': $('.txtCEOLastName').val(),
        'CEOFirstName': $('.txtCEOFristName').val(),
        'CEOMiddleName': $('.txtCEOMiddleName').val(),
        'SupervisorlastName': $('.txtSupervisorLastName').val(),
        'SupervisorFirstName': $('.txtSupervisorFirstName').val(),
        'SupervisorStartDate': supervisorStartDate,
        'SupervisorEndDate': supervisorEndDate,
        'WorkLocationStartDate': workLocationStartDate,
        'WorkLocationEndDate': workLocationEndDate,
        'PersonalCheckbox': $('.chkAgencyAddress').prop('checked'),
        'AgencyWorkSame': $('.chkAgency').prop('checked'),
        'SupervisorPhoneNumber': $('.txtSuperVisorPhoneNumber').val(),
        'SupervisorEmail': $('.txtSuperVisorEmail').val(),
        'AgencyAddressLine1': $('.pnlMainAddress .txtAdressLine1')[0].value,
        'AgencyAddressLine2': $('.pnlMainAddress .txtAddressLine2')[0].value,
        'AgencyCity': $('.pnlMainAddress .txtCity')[0].value,
        'AgencyState': $('.pnlMainAddress .ddlState')[0].value,
        'AgencyZip': $('.pnlMainAddress .txtZip')[0].value,
        'AgencyZipPlus': $('.pnlMainAddress .txtZipPlus')[0].value,
        'AgencyCounty': $('.pnlMainAddress .ddlCounty')[0].value,
        'AgencyPhone': $('.pnlMainAddress .txtPhoneNumber')[0].value,
        'AgencyEmail': $('.pnlMainAddress .txtEmailAddress')[0].value,
        'WorkAddressLine1': $('.pnlMainAddress .txtAdressLine1')[1].value,
        'WorkAddressLine2': $('.pnlMainAddress .txtAddressLine2')[1].value,
        'WorkCity': $('.pnlMainAddress .txtCity')[1].value,
        'WorkState': $('.pnlMainAddress .ddlState')[1].value,
        'WorkZip': $('.pnlMainAddress .txtZip')[1].value,
        'WorkZipPlus': $('.pnlMainAddress .txtZipPlus')[1].value,
        'WorkCounty': $('.pnlMainAddress .ddlCounty')[1].value,
        'WorkPhone': $('.pnlMainAddress .txtPhoneNumber')[1].value,
        'WorkEmail': $('.pnlMainAddress .txtEmailAddress')[1].value,
        'EmployerId': employerId
    }

    executePageMethod('EmployerInformation.aspx', 'SaveEmployerInformation', "{'saveInfo':" + JSON.stringify(saveInfo) + "}", function (data) {
        if (data.d == null || data.d.bolVal == undefined) {
            $('.errMsg').css("color", "green");
            $('.errMsg').css("border-color", "green");
            $('.errMsg')[0].innerHTML = "Data is successfully saved";
            ClearData();
            $('.errMsg').show();
            $('.btnSaveAndContinue').attr('disabled', false);
            $('.btnSaveAdditional').attr('disabled', false);
            
            integerVariable = 2
            supervisorWorkLocationEndDates = 2
            if (data.d.retVal != null) {
                $('.hdEmployerID').val(data.d.retVal)
            }
            if (additional) {
                ClearData();
                window.location.reload();
            }
        }
        else {
            $('.errMsg').css("color", "red");
            $('.errMsg').css("border-color", "red");
            $('.errMsg')[0].innerHTML = data.d.bolVal.Message;
            $('.errMsg').show();
            integerVariable = 2
            supervisorWorkLocationEndDates = 2
            $('.btnSaveAndContinue').attr('disabled', true);
            if (data.d.retVal != null) {
                $('.hdEmployerID').val(data.d.retVal)
            }
            e.preventDefault();
            return false;
        }
    }, function () {
        $('.errMsg').css("color", "red");
        $('.errMsg').css("border-color", "red");
        $('.errMsg')[0].innerHTML = "Error in saving employer Information";
        $('.errMsg').show();
        $('.btnSaveAndContinue').attr('disabled', true);
        e.preventDefault();
        return false;
    });
}

function GetRNInformation() {
    var county = ''
    var phone = ''
    var mail = ''
    executePageMethod('EmployerInformation.aspx', 'GetRNInformation', '{}', function (data) {
        if (data.d != null) {
            $('.txtEmployerName').val(data.d.EmployerName)
            $('.txtCEOFristName').val(data.d.CEOFirstName);
            $('.txtEmployerTaxID').val(data.d.RNLicense);
            $('.txtCEOLastName').val(data.d.CEOLastName);
            $('.txtCEOMiddleName').val(data.d.CEOMiddleName);
            $('.txtSupervisorLastName').val(data.d.CEOLastName);
            $('.txtSupervisorFirstName').val(data.d.CEOFirstName);
            $('.chkAgency').attr("checked", true);
            $('.chkAgencyAddress').attr("checked", true);
            var addressLine1 = data.d.AddressLine1;
            var addressLine2 = data.d.AddressLine2;
            var city = data.d.City;
            var State = data.d.State;
            var county = data.d.County;
            var zip = data.d.Zip;
            var zipPlus = data.d.ZipPlus;
            var phone = data.d.HomePhone;
            var mail = data.d.HomeEmail
            AssignTheDataInControl(addressLine1, addressLine2, city, State, zip, zipPlus, county, phone, mail);
        }
    }, function () {
        $('.errMsg').css("color", "red");
        $('.errMsg').css("border-color", "red");
        $('.errMsg')[0].innerHTML = "Error in getting employer Information";
        $('.errMsg').show();
    });
}

function GetEmployerInformation(prov) {
    var empInfo = {
        'ContractNumber': prov
    }
    executePageMethod('EmployerInformation.aspx', 'GetEmployerInformation', "{'empInfo':" + JSON.stringify(empInfo) + "}", function (data) {
        if (data.d != null && data.d.EntityName != null) {
            $('.lblFound').hide();
            $('.errMsg')[0].innerHTML = "";
            $('.errMsg').hide();
            $('.txtEmployerName').val(data.d.EntityName);
            var taxID = data.d.TaxID;
            if ($('.rblSelect input[type=radio]:checked').val() == 3) {
                taxID = "XXX-XX-" + data.d.TaxID.substr(data.d.TaxID.length - 4);
            }
            $('.txtEmployerTaxID').val(prov);
            $('.txtCertStartDate').val(data.d.CertificateStartDate);
            $('.txtCertEndDate').val(data.d.CertificationEndDate);
            $('.txtCertStatus').val(data.d.CertficationStatus);
            $('.txtCEOLastName').val(data.d.LastName);
            $('.txtCEOFristName').val(data.d.Firstname);
            $('.chkAgency').attr("checked", true);
            var addressLine1 = data.d.AddressLine1.replace("&#39;","'");
            var addressLine2 = data.d.AddressLine2.replace("&#39;","'");
            var city = data.d.City;
            var State = data.d.State;
            var zip = data.d.Zip;
            var zipPlus = data.d.ZipPlus;
            var county = data.d.County;
            var phone = data.d.PhoneNumber;
            var email = data.d.EmailAddress;

            AssignTheDataInControl(addressLine1, addressLine2, city, State, zip, zipPlus, county, phone, email);
            if (historyData == "True") {
                $(".txtEmploymentEndDate").attr('disabled', false);
                $(".txtEmploymentEndDate").datepick("enable");
                $(".txtSuperVisorEndDate").attr('disabled', false);
                $(".txtSuperVisorEndDate").datepick("enable");
                $(".txtWorkLocationEndDate").attr('disabled', false);
                $(".txtWorkLocationEndDate").datepick("enable");
            }
        }
        else {
            $('.errMsg').css("color", "red");
            $('.errMsg').css("border-color", "red");
            //$('.errMsg')[0].innerHTML = "No active employer found for this contract number, please check PCW for more information";
            $('.errMsg')[0].innerHTML = "This Provider Contract # is not in Active status. Please contact the Provider or DODD Provider Certification 1-800-617-6733"
            $('.errMsg').show();
            if (historyData == "True") {
                $(".txtEmploymentEndDate").attr('disabled', false);
                $(".txtEmploymentEndDate").datepick("enable");
                $(".txtSuperVisorEndDate").attr('disabled', false);
                $(".txtSuperVisorEndDate").datepick("enable");
                $(".txtWorkLocationEndDate").attr('disabled', false);
                $(".txtWorkLocationEndDate").datepick("enable");
            }
        }
    }, function () {
        $('.errMsg').css("color", "red");
        $('.errMsg').css("border-color", "red");
        $('.errMsg')[0].innerHTML = "Error in getting employer Information";
        $('.errMsg').show();
    });
}

function ValidationInClientSide(e) {
    $('.errMsg')[0].innerHTML = '';
    var chkflg = $('.rblSelect input[type=radio]:checked').val();
    var flag = true;
    if (chkflg == undefined) {
        flag = false;
        $('.errMsg')[0].innerHTML = "Please select any one of the employer selection groupbox";
        $('.errMsg').show();
        e.preventDefault();
    }
    var message = AddressValidationForEmployer($('.pnlMainAddress .txtAdressLine1')[0].value, $('.pnlMainAddress .txtCity')[0].value, $('.pnlMainAddress .ddlState')[0].value,
        $('.pnlMainAddress .ddlCounty')[0].value, $('.pnlMainAddress .txtZip')[0].value, $('.pnlMainAddress .txtZipPlus')[0].value,0)
    if (message != '') {
        flag = false
        if ($('.errMsg')[0].innerHTML == '') {
            $('.errMsg')[0].innerHTML = message
        }
        else {
            $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + message
        }
        $('.errMsg').show();
        e.preventDefault();
    }
    var message1 = AddressValidationForEmployer($('.pnlMainAddress .txtAdressLine1')[1].value, $('.pnlMainAddress .txtCity')[1].value, $('.pnlMainAddress .ddlState')[1].value,
        $('.pnlMainAddress .ddlCounty')[1].value, $('.pnlMainAddress .txtZip')[1].value, $('.pnlMainAddress .txtZipPlus')[1].value,1)
    if (message1 != '') {
        flag = false
        if ($('.errMsg')[0].innerHTML == '') {
            $('.errMsg')[0].innerHTML = message1
        }
        else {
            $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + message1
        }
        $('.errMsg').show();
        e.preventDefault();
    }
    

    //var phoneEmailmessage1 = ManadatoryPhoneEmail($('.pnlMainAddress .txtPhoneNumber')[1].value, $('.pnlMainAddress .txtEmailAddress')[1].value,1)
    //if (phoneEmailmessage1 != '') {
    //    flag = false
    //    if ($('.errMsg')[0].innerHTML == '') {
    //        $('.errMsg')[0].innerHTML = phoneEmailmessage1
    //    }
    //    else {
    //        $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + phoneEmailmessage1
    //    }
    //    $('.errMsg').show();
    //    e.preventDefault();
    //}
    //var CeoMessage = ValidationLastFirstName($('.txtCEOLastName').val(), $('.txtCEOFristName').val(), "CEO");
    //if (CeoMessage != '') {
    //    flag = false
    //    if ($('.errMsg')[0].innerHTML == '') {
    //        $('.errMsg')[0].innerHTML = CeoMessage
    //    }
    //    else {
    //        $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + CeoMessage
    //    }
    //    $('.errMsg').show();
    //    e.preventDefault();
    //}

    //var supervisorMessage = ValidationLastFirstName($('.txtSupervisorLastName').val(), $('.txtSupervisorFirstName').val(), "Supervisor");
    //if (supervisorMessage != '') {
    //    flag = false
    //    if ($('.errMsg')[0].innerHTML == '') {
    //        $('.errMsg')[0].innerHTML = supervisorMessage
    //    }
    //    else {
    //        $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + supervisorMessage
    //    }
    //    $('.errMsg').show();
    //    e.preventDefault();
    //}
    var validationEmployerSuperVisorWorkLocationEndDate = ValidationEmployerSuperVisorWorkLocationEndDate();
    if (validationEmployerSuperVisorWorkLocationEndDate != '') {
        flag = false
        if ($('.errMsg')[0].innerHTML == '') {
            $('.errMsg')[0].innerHTML = validationEmployerSuperVisorWorkLocationEndDate
        }
        else {
            $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + ValidationEmployerSuperVisorWorkLocationEndDate
        }
        $('.errMsg').show();
        e.preventDefault();
    }
    //var mandatorysupervisorphoneEmailAddress = MandatorySuperVisorPhoneAndEmailAddress();
    //if (mandatorysupervisorphoneEmailAddress != '') {
    //    flag = false
    //    if ($('.errMsg')[0].innerHTML == '') {
    //        $('.errMsg')[0].innerHTML = mandatorysupervisorphoneEmailAddress
    //    }
    //    else {
    //        $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + mandatorysupervisorphoneEmailAddress
    //    }
    //    $('.errMsg').show();
    //    e.preventDefault();
    //}
    var supervisorEmailAddress = ValidationForSupervisorEmail();
    if (supervisorEmailAddress != '') {
        flag = false
        if ($('.errMsg')[0].innerHTML == '') {
            $('.errMsg')[0].innerHTML = supervisorEmailAddress
        }
        else {
            $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + supervisorEmailAddress
        }
        $('.errMsg').show();
        e.preventDefault();
    }
    var phoneNumber = ValidationForPhoneNumber();
    if (phoneNumber != '') {
        flag = false
        if ($('.errMsg')[0].innerHTML == '') {
            $('.errMsg')[0].innerHTML = phoneNumber
        }
        else {
            $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + phoneNumber
        }
        $('.errMsg').show();
        e.preventDefault();
    }
    var dates = ValidationEmploymentDate();
    if (dates != '') {
        flag = false
        if ($('.errMsg')[0].innerHTML == '') {
            $('.errMsg')[0].innerHTML = dates
        }
        else {
            $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + dates
        }
        $('.errMsg').show();
        e.preventDefault();
    }
    var supervisorDates = ValidationSupervisor();
    if (supervisorDates != '') {
        flag = false
        if ($('.errMsg')[0].innerHTML == '') {
            $('.errMsg')[0].innerHTML = supervisorDates
        }
        else {
            $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + supervisorDates
        }
        $('.errMsg').show();
        e.preventDefault();
    }
    var workLocationDates = ValidationWorkLocation();
    if (workLocationDates != '') {
        flag = false
        if ($('.errMsg')[0].innerHTML == '') {
            $('.errMsg')[0].innerHTML = workLocationDates
        }
        else {
            $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + workLocationDates
        }
        $('.errMsg').show();
        e.preventDefault();
    }
    var workLocationSupervisorDates = ValidationSupervisorWorkLocation();
    if (workLocationSupervisorDates != '') {
        flag = false
        if ($('.errMsg')[0].innerHTML == '') {
            $('.errMsg')[0].innerHTML = workLocationSupervisorDates
        }
        else {
            $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + workLocationSupervisorDates
        }
        $('.errMsg').show();
        e.preventDefault();
    }
    //if ($('.txtEmployerName').val() == '' || $('.txtEmployerTaxID').val() == '') {
    //    flag = false
    //    var selfMessages = "If you are self employee, please enter your name and RN License Number. But if you are Other employee, please enter the employer name and Tax ID number";
    //    if ($('.errMsg')[0].innerHTML == '') {
    //        $('.errMsg')[0].innerHTML = selfMessages
    //    }
    //    else {
    //        $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + selfMessages
    //    }
    //    $('.errMsg').show();
    //    e.preventDefault();
    //}
    if ($('.pnlEmployer:disabled').length > 0 && $('.pnlEmployer:visible').length > 0) {
        var panelMessage = "Please enter DD provider number to search on the employer or agency";
        if ($('.txtDODDProvider').val() == '') {
            flag = false
            if ($('.errMsg')[0].innerHTML == '') {
                $('.errMsg')[0].innerHTML = panelMessage
            }
            else {
                $('.errMsg')[0].innerHTML = $('.errMsg')[0].innerHTML + "<br/>" + panelMessage
            }
            $('.errMsg').show();
            e.preventDefault();
        }
    }
    return flag;
}

//function MandatorySuperVisorPhoneAndEmailAddress() {
//    var message = ''
//    if ($('.txtSuperVisorPhoneNumber').val() == '' || $('.txtSuperVisorEmail').val() == '') {
//        message = message + "<br/>" + "Please enter manadatory fields supervisor Phone or Email Address";
//    }
//    return message;
//}

function ValidationEmployerSuperVisorWorkLocationEndDate() {
    var message = ''
    if ($('.txtEmploymentEndDate').val() != '' || $('.txtSuperVisorEndDate').val() != '' || $('.txtWorkLocationEndDate').val() != '') {
        var today = new Date();
        var EmploymentEnddatetime = new Date($(".txtEmploymentEndDate").val());
        var supervisorEnddatetime = new Date($(".txtSuperVisorEndDate").val());
        var worklocationEnddatetime = new Date($(".txtWorkLocationEndDate").val());
        if (EmploymentEnddatetime > today || supervisorEnddatetime > today || worklocationEnddatetime > today) {
            message = message + "<br/>" + "You cannot enter future dates for employment, supervisor and worklocation end dates, for these dates please leave the field blank";
        }
    }
    return message;
}

function ValidationEmploymentDate() {
    var message = '';
    //if ($('.txtEmploymentStartDate').val() == '') {
    //    message = "Please enter the employment start date";
    //}

    if ($('.txtEmploymentStartDate').val() != '') {
        var today = new Date();
        var Employmentdatetime = new Date($(".txtEmploymentStartDate").val());
        var tmp = IsDate($.trim($(".txtEmploymentStartDate").val()));
        if (tmp != "") {
            message = message + "<br/>" + "Please enter valid start date with format of MM/DD/YYYY, " + tmp;
        }
        if (Employmentdatetime > today) {
            message = message + "<br/>" + "The Employment Start date cannot be future date";
        }

        if ((Date.parse($('.txtEmploymentStartDate').val()) > Date.parse($('.txtCertEndDate').val()))) {
            var expiredmessage = "Please check the employment start date as it is greater than certification date of the provider. Please contact DODD Admin.";
            if (message == '') {
                message = expiredmessage;
            }
            else { message = message + "<br/>" + expiredmessage; }
        }
    }

    if ($('.txtEmploymentEndDate').val() != '') {
        var tmpdate = IsDate($.trim($(".txtEmploymentEndDate").val()));
        if (tmpdate != "") {
            message = message + "<br/>" + "Please enter valid end date with format of MM/DD/YYYY, " + tmpdate;
        }
    }

    if ($('.txtEmploymentStartDate').val() != '' && $('.txtEmploymentEndDate').val() != '') {
        if (Date.parse($('.txtEmploymentStartDate').val()) > Date.parse($('.txtEmploymentEndDate').val())) {
            var datemessage = "End date should always be greater than start date";
            if (message == '') {
                message = datemessage;
            }
            else { message = message + "<br/>" + datemessage; }
        }
    }
    return message;
}

function ValidationSupervisorWorkLocation() {
    var validatemessage = ''
    if ($('.txtSuperVisorEndDate').val() != "") {
        var employmentDate = "12/31/9999"
        if ($('.txtEmploymentEndDate').val() != "") {
            employmentDate = $('.txtEmploymentEndDate').val();
        }
        if (Date.parse($('.txtSuperVisorEndDate').val()) > Date.parse(employmentDate)) {
            var datemessage = "Supervisor End date should not be greater than employment end date";
            if (validatemessage == '') {
                validatemessage = datemessage;
            }
            else { validatemessage = validatemessage + "<br/>" + datemessage; }
        }
    }
    if ($('.txtWorkLocationEndDate').val() != "") {
        var employmentworkDate = "12/31/9999"
        if ($('.txtEmploymentEndDate').val() != "") {
            employmentworkDate = $('.txtEmploymentEndDate').val();
        }
        if (Date.parse($('.txtWorkLocationEndDate').val()) > Date.parse(employmentworkDate)) {
            var dateWorkmessage = "Work location End date should not be greater than employment end date";
            if (validatemessage == '') {
                validatemessage = dateWorkmessage;
            }
            else { validatemessage = validatemessage + "<br/>" + dateWorkmessage; }
        }
    }
    return validatemessage;
}

function ValidationSupervisor() {
    var message = '';
    //if ($('.txtSuperVisorStartDate').val() == '') {
    //    message = "Please enter the supervisor start date";
    //}

    if ($('.txtSuperVisorStartDate').val() != '') {
        var today = new Date();
        var Employmentdatetime = new Date($(".txtSuperVisorStartDate").val());
        var tmp = IsDate($.trim($(".txtSuperVisorStartDate").val()));
        if (tmp != "") {
            message = message + "<br/>" + "Please enter valid start date with format of MM/DD/YYYY, " + tmp;
        }
        if (Employmentdatetime > today) {
            message = message + "<br/>" + "The supervisor Start date cannot be future date";
        }
    }

    if ($('.txtSuperVisorEndDate').val() != '') {
        var tmpdate = IsDate($.trim($(".txtSuperVisorEndDate").val()));
        if (tmpdate != "") {
            message = message + "<br/>" + "Please enter valid end date with format of MM/DD/YYYY, " + tmpdate;
        }
    }

    if ($('.txtSuperVisorStartDate').val() != '' && $('.txtSuperVisorEndDate').val() != '') {
        if (Date.parse($('.txtSuperVisorStartDate').val()) > Date.parse($('.txtSuperVisorEndDate').val())) {
            var datemessage = "End date should always be greater than start date";
            if (message == '') {
                message = datemessage;
            }
            else { message = message + "<br/>" + datemessage; }
        }
    }
    return message;
}

function ValidationWorkLocation() {
    var message = '';
    //if ($('.txtWorkLocationStartDate').val() == '') {
    //    message = "Please enter the worklocation start date";
    //}

    if ($('.txtWorkLocationStartDate').val() != '') {
        var today = new Date();
        var Employmentdatetime = new Date($(".txtWorkLocationStartDate").val());
        var tmp = IsDate($.trim($(".txtWorkLocationStartDate").val()));
        if (tmp != "") {
            message = message + "<br/>" + "Please enter valid start date with format of MM/DD/YYYY, " + tmp;
        }
        if (Employmentdatetime > today) {
            message = message + "<br/>" + "The work location Start date cannot be future date";
        }
    }

    if ($('.txtWorkLocationEndDate').val() != '') {
        var tmpdate = IsDate($.trim($(".txtWorkLocationEndDate").val()));
        if (tmpdate != "") {
            message = message + "<br/>" + "Please enter valid end date with format of MM/DD/YYYY, " + tmpdate;
        }
    }

    if ($('.txtWorkLocationStartDate').val() != '' && $('.txtWorkLocationEndDate').val() != '') {
        if (Date.parse($('.txtWorkLocationStartDate').val()) > Date.parse($('.txtWorkLocationEndDate').val())) {
            var datemessage = "End date should always be greater than start date";
            if (message == '') {
                message = datemessage;
            }
            else { message = message + "<br/>" + datemessage; }
        }
    }
    return message;
}

//function ValidationLastFirstName(lastName, firstName, typeOfPerson) {
//    var message = '';
//    if (lastName == '' || firstName == '') {
//        message = "Please enter first and last name of the" + " " + typeOfPerson;
//    }
//    //if (lastName != '' || firstName != '') {
//    //    var alphabets = /^[A-Za-z'.\s]+$/;
//    //    if (!alphabets.test(lastName)) {
//    //        var lastName = "Please enter valid Last name of the" + " " + typeOfPerson;
//    //        if (message == '') {
//    //            message = lastName;
//    //        }
//    //        else { message = message + "<br/>" + lastName; }
//    //    }
//    //    if (!alphabets.test(firstName)) {
//    //        var firstName = "Please enter valid first name of the" + " " + typeOfPerson;
//    //        if (message == '') {
//    //            message = firstName;
//    //        }
//    //        else { message = message + "<br/>" + firstName; }
//    //    }
//    //}
//    return message;
//}

function ValidationForPhoneNumber() {
    var message = ''
    if ($('.txtSuperVisorPhoneNumber').val() != '') {
        var phoneNumberPattern = /^\(?(\d{3})\)?[- ]?(\d{3})[- ]?(\d{4})$/;
        var inputPhoneNumber = $('.txtSuperVisorPhoneNumber').val();
        if (inputPhoneNumber != '') {
            if (!phoneNumberPattern.test(inputPhoneNumber)) {
                var phoneMessage = "Please enter valid supervisor phone Number";
                if (message == '') {
                    message = phoneMessage;
                }
                else { message = message + "<br/>" + phoneMessage; }
            }
        }
    }
    return message;
}

function ValidationForSupervisorEmail() {
    var message = ''
    if ($('.txtSuperVisorEmail').val() != '') {
        var emailAddressRegx = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (!emailAddressRegx.test($('.txtSuperVisorEmail').val())) {
            var EmailAddress = "Please enter valid email address";
            if (message == '') {
                message = EmailAddress;
            }
            else { message = message + "<br/>" + EmailAddress; }
        }
    }
    return message;
}

var GetAgencyInformation = function () {
    var getAgencyInfo = {
        //'EmployerName': $('.txtEmployerAgencyInformation').val(),
        'ContractNumber': $('.txtDODDProvider').val(),
        'Independent': $('.rblSelect input[type=radio]:checked').val()
    };

    executePageMethod('EmployerInformation.aspx', 'GetAgencyInformation', "{'getAgencyInfo':" + JSON.stringify(getAgencyInfo) + "}", function (data) {
        if (data.d != null) {
            if (data.d.length > 0) {
                $('table.grdEmp').show();
                $('.lblFound').hide();
                $('table.grdEmp').html(' ');
                var tableToUpdate = $('table.grdEmp');
                var dataRow1 = "<tr><th align = center>Provider Name</th><th align = center>CEO Name</th><th align = center>Contract Number</th></tr>";
                tableToUpdate.append(dataRow1);
                $.each(data.d, function () {
                    var dataRow = formatHtml(call_rowHtml, this.ProviderName, this.Name, this.ContractNumber);
                    tableToUpdate.append(dataRow);
                });
            }
            else {
                $('table.grdEmp').hide();
                $('.lblFound').show();
                ClearData();
            }
        }
    }, function () {
        $('.errMsg').css("color", "red");
        $('.errMsg').css("border-color", "red");
        $('.errMsg')[0].innerHTML = "Error in getting agency Info";
        $('.errMsg').show();
        $('table.grdEmp').hide();
    });
};

function ClearData() {
    $('.txtEmployerName').val('');
    $('.txtEmployerTaxID').val('');
    $('.txtDODDProvider').val('');
    $('.txtEmploymentStartDate').val('');
    $('.txtEmploymentEndDate').val('');
    $('.txtCEOLastName').val('');
    $('.txtCEOFristName').val('');
    $('.txtCEOMiddleName').val('');
    $('.txtSupervisorLastName').val('');
    $('.txtSupervisorFirstName').val('');
    $('.txtSuperVisorStartDate').val('');
    $('.txtSuperVisorEndDate').val('');
    $('.txtWorkLocationStartDate').val('');
    $('.txtWorkLocationEndDate').val('');
    $('.txtCertStartDate').val('');
    $('.txtCertEndDate').val('');
    $('.txtCertStatus').val('');
    $('.chkAgencyAddress').val('');
    $('.chkAgency').val('');
    $('.txtSuperVisorPhoneNumber').val('');
    $('.txtSuperVisorEmail').val('');
    $('.chkAgency').attr("checked", false);
    $('.chkAgencyAddress').attr("checked", false);
    //$('.txtDODDProvider').attr('disabled', true);
    //$('.btnSearch').attr('disabled', true);
    ClearAddressFields();
}