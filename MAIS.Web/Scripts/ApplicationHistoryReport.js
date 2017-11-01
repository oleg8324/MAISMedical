var tagOptionHtml = '<option value="{0}">{1}</option>';

//format the html
var formatHtml = function () {
    for (var i = 1; i < arguments.length; i++) {
        var exp = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        arguments[0] = arguments[0].replace(exp, arguments[i]);
    }
    return arguments[0];
};

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

$(document).ready(function () {
    $('.grdApplicationDetail').remove();
    var chkflg = $('.rblSelect input[type=radio]:checked').val();
    if (chkflg == 0) {
        $('.errMsg').hide();
        $(".pnlSearch").show();
        $(".lblRNDDLicenseSSN").text('RN License No.:');
        $(".trDDCode").css('visibility', 'hidden');
        $(".txtRNDDLicSSN").mask("RN999999");
        $(".trDDCode").hide();
    }
    LoadApplicationStatusType();
    LoadRNName();
    LoadApplicationType();
    radioButtonClick();
    //buttonClick();
});

function radioButtonClick() {
    $(".rblSelect").click(function (e) {
        //$('.grdApplicationDetail').empty();
        //$('.grdApplicationDetail').remove();
        //$('.grdApplicationStatusDetail').remove();
        $('.ddlAppType').empty();
        $(".fontSSN").text('');
        var chkflg = $('.rblSelect input[type=radio]:checked').val();
        if (chkflg == 1) {
            $('.errMsg').hide();
            $(".pnlSearch").show();
            $(".lblRNDDLicenseSSN").text('Last 4SSN:');
            $(".trDDCode").css('visibility', 'visible');
            $(".trDDCode").show();
            $(".txtRNDDLicSSN").mask("9999");
            $(".txtDDPersoonelCode").mask("DD99999999");
            ClearData();
        }
        else if (chkflg == 0) {
            $('.errMsg').hide();
            $(".pnlSearch").show();
            $(".lblRNDDLicenseSSN").text('RN License No.:');
            $(".trDDCode").css('visibility', 'hidden');
            $(".trDDCode").hide();
            $(".txtRNDDLicSSN").mask("RN999999");
            ClearData();
        }
        else {
            $('.errMsg').hide();
            $(".pnlSearch").hide();
            $(".lblRNDDLicenseSSN").text('');
        }
        LoadApplicationType();
    });
}

function ClearData() {
    $('.txtRNDDLicSSN').val('');
    $('.txtStartDate').val('');
    $('.txtEndDate').val('');
    $('.ddlStatus').val(0);
    $('.ddlRNName').val(0);
    $('.ddlAppType').val(0);
    $('.lblmessage').css('visibility', 'hidden');
    $('.lblmessage').hide();
    $('.grdApplicationDetail').remove();
    $('.grdApplicationStatusDetail').remove();
}

var LoadApplicationStatusType = function () {
    executePageMethod('ApplicationHistoryReport.aspx', 'BindApplicationTypeDropDown', '{}', function (data) {
        if (data.d != null) {
            $('.errMsg').hide();
            $.each(data.d, function () {
                var optionItem = formatHtml(tagOptionHtml, this.ApplicationStatusID, this.ApplicationStatusType);
                $('.ddlStatus').append(optionItem);
            });
        }
    }, function () {
        $('.errMsg')[0].innerHTML = "Error in fetching the application status.";
        $('.errMsg').show();
    });
};

var LoadRNName = function () {
    executePageMethod('ApplicationHistoryReport.aspx', 'BindRNNameDropDown', '{}', function (data) {
        if (data.d != null) {
            $('.errMsg').hide();
            $.each(data.d, function () {
                var optionItem = formatHtml(tagOptionHtml, this.RNSID, this.LastFirstname);
                $('.ddlRNName').append(optionItem);
            });
        }
    }, function () {
        $('.errMsg')[0].innerHTML = "Error in fetching the RN Name.";
        $('.errMsg').show();
    });
};

var LoadApplicationType = function () {
    var chkflg = $('.rblSelect input[type=radio]:checked').val();
    executePageMethod('ApplicationHistoryReport.aspx', 'BindApplicationTypeUsingRoleLevelDropDown', "{'saveinfo':" + JSON.stringify(chkflg) + "}", function (data) {
        if (data.d != null) {
            $('.errMsg').hide();
            $.each(data.d, function () {
                var optionItem = formatHtml(tagOptionHtml, this.ApplicationTypeID, this.ApplicationTypeDescription);
                $('.ddlAppType').append(optionItem);
            });
        }
    }, function () {
        $('.errMsg')[0].innerHTML = "Error in fetching the Application type with Roles.";
        $('.errMsg').show();
    });
};

function buttonClick() {
    $(".btnSearch").click(function (e) {
        $('.grdApplicationDetail').remove();
        $('.lblmessage').css('visibility', 'hidden');
        $('.lblmessage').hide();
        $('.hdApplicationStatus').val($('.ddlStatus').val())
        $('.hdRNName').val($('.ddlRNName').val())
        $('.hdAppType').val($(".ddlAppType option:selected").text())
        $('.grdApplicationStatusDetail').remove();
        $(".errMsg")[0].innerHTML = '';
        $(".errMsg").hide();
        var x = validate(e)
        if (x == true) {
            var spinner = new Spinner(opts).spin($('.divSpinner')[0]);
        }
    });
}
function validate(e) {
    var x = true
    if (($.trim($(".txtRNDDLicSSN").val()) == "")
       && ($.trim($(".txtStartDate").val()) == "")
       && ($(".ddlAppType").val() == 0)
       && ($(".ddlStatus").val() == 0)
       && ($(".ddlRNName").val() == 0) && $(".txtDDPersoonelCode").val() == "") {
        $(".errMsg")[0].innerHTML = "Please enter either one of the below search criteria.";
        $(".errMsg").show();
        x = false
        e.preventDefault();        
        return false;
    }
    if ($(".txtEndDate").val() != "" && $(".txtStartDate").val() == "") {
        if ($(".errMsg")[0].innerHTML == "") {
            $(".errMsg")[0].innerHTML = "Cannot enter end date without entering the start date.";
            $(".errMsg").show();
        }
        else {
            $(".errMsg")[0].innerHTML = $(".errMsg")[0].innerHTML + "<br/>" + "Cannot enter end date without entering the start date.";
            $(".errMsg").show();
        }
        x= false
        e.preventDefault();
        return false;
    }
    return x;
}