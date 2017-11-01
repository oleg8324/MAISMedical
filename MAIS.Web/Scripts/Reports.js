$(document).ready(function () {
   // $(".txtRNDDLicDDCode").mask("RN999999");
    var chkflg = $('.rblSelect input[type=radio]:checked').val();

    if (chkflg == 1) { //RN                             
        $(".txtRNDDLicDDCode").mask("RN999999");
    }
    else if (chkflg == 2) { //DD          
        $(".txtRNDDLicDDCode").mask("DD99999999");
        $(".txt4SSN").mask("9999");
    }
    else {

    }
  
    $(".btnRun").click(function (e) {       
        var spinner = new Spinner(opts).spin($('.divSpinner')[0]);
        $(".errMsg")[0].innerHTML = '';
        $(".errMsg").hide();
        validate(e)
    });
    $(".btnExport").click(function (e) {
        var spinner = new Spinner(opts).spin($('.divSpinner')[0]);
        $(".errMsg")[0].innerHTML = '';
        $(".errMsg").hide();
       
    });
    $(".btnPrint").click(function (e) {
        var spinner = new Spinner(opts).spin($('.divSpinner')[0]);
        $(".errMsg")[0].innerHTML = '';
        $(".errMsg").hide();
        
    });

});
var opts = {
    lines: 12, // The number of lines to draw
    length: 9, // The length of each line
    width: 4, // The line thickness 
    radius: 9, // The radius of the inner circle
    color: 'navy', // #rgb or #rrggbb
    speed: 1.5, // Rounds per second
    trail: 60, // Afterglow percentage
    shadow: false, // Whether to render a shadow
    hwaccel: false, // Whether to use hardware acceleration
    className: 'spinner', // The CSS class to assign to the spinner
    zIndex: 2e9, // The z-index (defaults to 2000000000)
    top: 'auto', // Top position relative to parent in px
    left: 'auto' // Left position relative to parent in px
};

function validate(e) {
    if (($.trim($(".txtRNDDLicDDCode").val()) == "")
        && ($.trim($(".txtFName").val()) == "")
        && ($.trim($(".txtLName").val()) == "")
        && ($.trim($(".txt4SSN").val()) == "")
        && ($.trim($(".txtSupFirst").val()) == "")
        && ($.trim($(".txtSupLast").val()) == "")
        && ($.trim($(".txtEmpName").val()) == "")
        && ($.trim($(".txtCEOFirst").val()) == "")
        && ($.trim($(".txtCEOLast").val()) == "")
        && ($.trim($(".ddlCertTypes").val()) == "0")
        && ($.trim($(".ddlCertStatus").val()) == "0")
        && ($.trim($(".ddlCourses").val()) == "0")
        && ($.trim($(".txtDateFrom").val()) == "")
        && ($.trim($(".txtDateTo").val()) == "")
        && ($.trim($(".ddlSessions").val()) == "0")
        && ($.trim($(".ddlRNTrainer").val()) == "0")) {
        $(".errMsg")[0].innerHTML = "Please enter either one of the below search criteria.";
        $(".errMsg").show();
        e.preventDefault();
        return false;
    }
    var cityValidMessage = "";
    if ($('.txtFName').val() != '') {
        var alphabets = /^[A-Za-z'.\s]+$/;
        if (!alphabets.test($('.txtFName').val())) {
            cityValidMessage = "Please enter valid first name.";
        }
    }
    if ($('.txtLName').val() != '') {
        var alphabets = /^[A-Za-z'.\s]+$/;
        if (!alphabets.test($('.txtLName').val())) {
            cityValidMessage = cityValidMessage + "Please enter valid last name.";
        }
    }
    if ($('.txtSupFirst').val() != '') {
        var alphabets = /^[A-Za-z'.\s]+$/;
        if (!alphabets.test($('.txtFName').val())) {
            cityValidMessage = cityValidMessage + "Please enter valid supervisor first name.";
        }
    }
    if ($('.txtSupLast').val() != '') {
        var alphabets = /^[A-Za-z'.\s]+$/;
        if (!alphabets.test($('.txtLName').val())) {
            cityValidMessage = cityValidMessage + "Please enter valid supervisor last name.";
        }
    }
    if ($('.txtCEOFirst').val() != '') {
        var alphabets = /^[A-Za-z'.\s]+$/;
        if (!alphabets.test($('.txtFName').val())) {
            cityValidMessage = cityValidMessage + "Please enter valid CEO first name.";
        }
    }
    if ($('.txtCEOLast').val() != '') {
        var alphabets = /^[A-Za-z'.\s]+$/;
        if (!alphabets.test($('.txtLName').val())) {
            cityValidMessage = cityValidMessage + "Please enter valid CEO last name.";
        }
    }
    if (cityValidMessage.length > 0) {
        $(".errMsg")[0].innerHTML = cityValidMessage;
        $(".errMsg").show();
        e.preventDefault();
        return false;
    }
}
