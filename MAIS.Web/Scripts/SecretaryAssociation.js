$(document).ready(function () {   
    $('.btnSearch').click(function (e) {
        var spinner = new Spinner(opts).spin($('.divSpinner')[0]);
        $(".errMsg")[0].innerHTML = '';
        $(".errMsg").hide();
        validate(e)
    });

    $('.btnSave').click(function (e) {
        $(".errMsg")[0].innerHTML = '';
        $(".errMsg").hide();
        DataValidation(e)
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
function DataValidation(e) {
    
    if (($.trim($(".txt_Email").val()) == "")
        || ($.trim($(".txtComments").val()) == "")       
        || ($.trim($(".ddlRNsList").val()) == "-1")) {
        $(".errMsg")[0].innerHTML = "Please enter mandatory data RN,Email and Comments.";
        $(".errMsg").show();
        e.preventDefault();
        return false;
    }
    var cityValidMessage = "";
    var emailAddressRegx = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    if ($('.txt_Email').val() != '') {
        if (!emailAddressRegx.test($('.txt_Email').val())) {
            cityValidMessage = "Please enter valid Email address";
        }
    }
}
function validate(e) {
    if (($.trim($(".txtEmail").val()) == "")
        && ($.trim($(".txtFName").val()) == "")
        && ($.trim($(".txtLName").val()) == "")
        && ($.trim($(".ddlRNSearch").val()) == "-1")
        && ($.trim($(".ddlStatus").val()) == "0")) {
        $(".errMsg")[0].innerHTML = "Please enter either one of the below search criteria.";
        $(".errMsg").show();
        e.preventDefault();
        return false;
    }
    var cityValidMessage = "";
    var emailAddressRegx = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    if ($('.txtEmail').val() != '') {
        if (!emailAddressRegx.test($('.txtEmail').val())) {
            cityValidMessage = "Please enter valid Email address";            
        }
    }
    if ($('.txtFName').val() != '') {
        var alphabets = /^[A-Za-z'.\s]+$/;
        if (!alphabets.test($('.txtFName').val())) {
            cityValidMessage = "Please enter valid first name. ";
        }
    }
    if ($('.txtLName').val() != '') {
        var alphabets = /^[A-Za-z'.\s]+$/;
        if (!alphabets.test($('.txtLName').val())) {
            cityValidMessage = cityValidMessage + "Please enter valid last name.";
        }
    }
    if (cityValidMessage.length > 0) {
        $(".errMsg")[0].innerHTML = cityValidMessage;
        $(".errMsg").show();
        e.preventDefault();
        return false;
    }
}



