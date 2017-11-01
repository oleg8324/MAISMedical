var tagOptionHtml = '<option value="{0}">{1}</option>';

//format the html
var formatHtml = function () {
    for (var i = 1; i < arguments.length; i++) {
        var exp = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        arguments[0] = arguments[0].replace(exp, arguments[i]);
    }
    return arguments[0];
};

$(document).ready(function () {

    $(".pnlSearch").hide();
    $(".fontSSN").text('');
    LoadApplicationStatusType();
   
    radioButtonClick()
    
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
function radioButtonClick() {
    $(".rblSelect").click(function (e) {
        $(".fontSSN").text('');
        var chkflg = $('.rblSelect input[type=radio]:checked').val();
        //var ssn = "<font style=" + "'color:" + "Red;'" + "class=" + "'fontSSN'" + ">*</font>"
        if (chkflg == 1) {
            $('.errMsg').hide();
            $(".pnlLabel").hide();
            $(".pnlSearch").show();
            $(".lblRNDDLicenseSSN").text('Last 4SSN:');
            $(".lblRNDateDDDOB").text('Date of Birth:');
            $(".txtRNDDLicSSN").mask("9999");
            $(".txtDDPersoonelCode").mask("DD99999999");
            //$(".tdMandatory").append(ssn);
            ClearData();
            $(".trDDCode").css('visibility', 'visible');
            $(".trDDCode").show();
        }
        else if (chkflg == 0) {
            $('.errMsg').hide();
            $(".pnlLabel").hide();
            $(".pnlSearch").show();
            $(".lblRNDDLicenseSSN").text('RN License No.:');
            $(".lblRNDateDDDOB").text('Original Date of License Issued:');
            $(".txtRNDDLicSSN").mask("RN999999");
            $(".fontSSN").text('');
            ClearData();
            $(".trDDCode").css('visibility', 'hidden');
            $(".trDDCode").hide();
        }
        else {
            $('.errMsg').hide();
            $(".pnlSearch").hide();
            $(".lblRNDDLicenseSSN").text('');
            $(".lblRNDateDDDOB").text('');

        }
    });
    //buttonClick();
    //$(".btnSearch").click(function (e) {
    //    $(".errMsg")[0].innerHTML = '';
    //    $(".errMsg").hide();
    //    validate(e)
    //});
    //$('.ddlStatus').click(function (e) {

    //});
}

function buttonClick() {
    $(".btnSearch").click(function (e) {
        $('.grdDDSearch').remove();
        $('.grdRNSearch').remove();
        $('.hdApplicationStatus').val($('.ddlStatus').val())
        $(".errMsg")[0].innerHTML = '';
        $(".errMsg").hide();
        var x = validate(e)
        if (x == true) {
            var spinner = new Spinner(opts).spin($('.divSpinner')[0]);
        }
    });
}

function ClearData() {
    $('.txtAppID').val('')
    $('.txtRNDDLicSSN').val('')
    $('.txtRNDateDDDOB').val('')
    $('.txtFirstName').val('')
    $('.txtLastName').val('')
    $('.txtEmployer').val('')
    $('.grdRNSearch').remove();
    $('.grdDDSearch').remove();
}

var LoadApplicationStatusType = function () {
    executePageMethod('Search.aspx', 'BindApplicationTypeDropDown', '{}', function (data) {
        if (data.d != null) {
            $('.errMsg').hide();
            $.each(data.d, function () {
                var optionItem = formatHtml(tagOptionHtml, this.ApplicationStatusID, this.ApplicationStatusType);
                $('.ddlStatus').append(optionItem);
            });
        }
    }, function () {
        $('.errMsg')[0].innerHTML = "Error in fetching the application status";
        $('.errMsg').show();
    });
};

function validate(e) {
    var x = true;
    if (($.trim($(".txtAppID").val()) == "")
        && ($.trim($(".txtRNDDLicSSN").val()) == "")
        && ($.trim($(".txtRNDateDDDOB").val()) == "")
        && ($.trim($(".txtFirstName").val()) == "")
        && ($(".txtLastName").val() == "")
        && ($(".txtEmployer").val() == "") && $('.ddlStatus').val() == "0" && $(".txtDDPersoonelCode").val() == "") {
        $(".errMsg")[0].innerHTML = "Please enter either one of the below search criteria.";
        $(".errMsg").show();
        x = false;
        e.preventDefault();
        return false;
    }
    if ($('.lblRNDDLicenseSSN').html() == "Last 4SSN:") {
        if ($.trim($(".txtRNDDLicSSN").val()) == "" && ($(".txtDDPersoonelCode").val() == "") && ($.trim($(".txtAppID").val()) == "") ) {
            if ($(".errMsg")[0].innerHTML == "") {
                $(".errMsg")[0].innerHTML = "Please enter last 4 digits of SSN or DD personnel Code for DD personnel or Application ID.";
                $(".errMsg").show();
            }
            else {
                $(".errMsg")[0].innerHTML = $(".errMsg")[0].innerHTML + "<br/>" + "Please enter last 4 digits of SSN or DD personnel Code for DD personnel or Application ID.";
                $(".errMsg").show();
            }
            x = false;
            e.preventDefault();
            return false;
        }
        else {
            if ($.trim($(".txtRNDDLicSSN").val()) != "") {
                var intRegex = /^\d+$/;
                var value = $.trim($(".txtRNDDLicSSN").val());
                if(!intRegex.test(value)){
                    if ($(".errMsg")[0].innerHTML == "") {
                        $(".errMsg")[0].innerHTML = "Please enter valid last 4 digits of SSN for DD personnel.";
                        $(".errMsg").show();
                    }
                    else {
                        $(".errMsg")[0].innerHTML = $(".errMsg")[0].innerHTML + "<br/>" + "Please enter valid last 4 SSN number for DD personnel.";
                        $(".errMsg").show();
                    }
                    x = false;
                    e.preventDefault();
                    return false;
                }
            }
        }
    }
    return x;
}
