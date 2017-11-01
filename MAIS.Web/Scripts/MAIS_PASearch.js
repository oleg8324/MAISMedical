function validate(e) {
    if (($.trim($(".txtRNLicenseNo").val()) == "")
        && ($.trim($(".txtFname").val()) == "")
        && ($.trim($(".txtLname").val()) == "")
        && ($.trim($(".txtLast4ssn").val()) == "")
        && ($.trim($(".txtDODDIdNo").val()) == "")
        && ($.trim($(".txtDDLname").val()) == "")
        && ($.trim($(".txtDDFname").val()) == "")
        && ($.trim($(".txtEmployer").val()) == "")
        && ($.trim($(".txtStDate").val()) == "")
        && ($.trim($(".txtEndDate").val()) == "")
        && ($.trim($(".txtCounty").val()) == "")
        && ($.trim($(".txtRNLname").val()) == "")
        && ($.trim($(".txtRNFname").val()) == "")) {
        $(".ErrorSummary")[0].innerHTML = "Please enter either one of the below search criteria.";
        $(".errMsg").show();
        e.preventDefault();
        return false;
    }
    //var cityValidMessage = "";
    //if ($('.txtFName').val() != '') {
    //    var alphabets = /^[A-Za-z'.\s]+$/;
    //    if (!alphabets.test($('.txtFName').val())) {
    //        cityValidMessage = "Please enter valid first name.";
    //    }
    //}
    //if ($('.txtLName').val() != '') {
    //    var alphabets = /^[A-Za-z'.\s]+$/;
    //    if (!alphabets.test($('.txtLName').val())) {
    //        cityValidMessage = cityValidMessage + "Please enter valid last name.";
    //    }
    //}
    //if ($('.txtSupFirst').val() != '') {
    //    var alphabets = /^[A-Za-z'.\s]+$/;
    //    if (!alphabets.test($('.txtFName').val())) {
    //        cityValidMessage = cityValidMessage + "Please enter valid supervisor first name.";
    //    }
    //}
    //if ($('.txtSupLast').val() != '') {
    //    var alphabets = /^[A-Za-z'.\s]+$/;
    //    if (!alphabets.test($('.txtLName').val())) {
    //        cityValidMessage = cityValidMessage + "Please enter valid supervisor last name.";
    //    }
    //}
    //if ($('.txtCEOFirst').val() != '') {
    //    var alphabets = /^[A-Za-z'.\s]+$/;
    //    if (!alphabets.test($('.txtFName').val())) {
    //        cityValidMessage = cityValidMessage + "Please enter valid CEO first name.";
    //    }
    //}
    //if ($('.txtCEOLast').val() != '') {
    //    var alphabets = /^[A-Za-z'.\s]+$/;
    //    if (!alphabets.test($('.txtLName').val())) {
    //        cityValidMessage = cityValidMessage + "Please enter valid CEO last name.";
    //    }
    //}
    //if (cityValidMessage.length > 0) {
    //    $(".errMsg")[0].innerHTML = cityValidMessage;
    //    $(".errMsg").show();
    //    e.preventDefault();
    //    return false;
    //}
}
$(document).ready(function () {
    var chkflg = $('.rblSelect input[type=radio]:checked').val();

    if (chkflg == 1) { //RN                             
        $(".txtRNLicenseNo").mask("RN999999");
    }
    else if (chkflg == 2) { //DD          
        $(".txtDODDIdNo").mask("DD99999999");
        $(".txtLast4ssn").mask("9999");
    }
    $(".btnRun").click(function (e) {
        var spinner = new Spinner(opts).spin($('.divSpinner')[0]);
        //if (Page_ClientValidate("a") == false) {
        //    //alert("Works!");
        //    $('.divSpinner').hide();
        //} else {
        //    if (!confirm('Are you sure you want to save changes?')) {
        //        e.preventDefault();
        //        $('.divSpinner').hide();
        //        return false;
        //    }
        //}
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