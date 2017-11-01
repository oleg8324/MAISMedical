$(document).ready(function () {
    //$(".pnlRnSelectionValue").hide();
    //$(".pnlDDSelectionValue").hide();
    $(".SearchRN").click(function (e) {
        var $Test = new Boolean
        $Test = true

        if ($('.txtRNNO').val() == "" & $('.txtLName').val()=="") {
            $Test = false;
        }
        if ($Test == true) {
            var spinner = new Spinner(opts).spin($('.divSpinner')[0]);
        }
    });

    $(".Search").click(function (e) {
        var $Test = new Boolean
        $Test = true

        if ($('.txtDODDSSN').val() == "" & $('.txtDODDID').val() == "") {
            $Test = false;
        }
        if ($Test == true) {
            var spinner = new Spinner(opts).spin($('.divSpinner')[0]);
        }
    });

    $('.txtRNNO').mask("RN999999");
    $('.txtDODDID').mask("DD99999999");

    if ($('.rblRnTypeSelect').is(':visible')) {
        if (($('.rblRnTypeSelect input[type=radio]:checked').val()) == 0) {
            $(".pnlRnSelectionValue").show();
            $(".pnlDDSelectionValue").hide();
            
        } else if (($('.rblRnTypeSelect input[type=radio]:checked').val()) == 1) {
            $(".pnlRnSelectionValue").hide();
            $(".pnlDDSelectionValue").show();
             
        } else {
            $(".pnlRnSelectionValue").hide();
            $(".pnlDDSelectionValue").hide();
        }

    };

    $(".numericOnly").keydown(function (e) {
        var key = e.charCode || e.keyCode || 0;
        // allow backspace, tab, delete, arrows, ".", numbers and keypad numbers ONLY
        return (
            key == 8 ||
            key == 9 ||
            key == 46 ||
            key == 110 ||
            (key >= 37 && key <= 40) ||
            (key >= 48 && key <= 57) ||
            (key >= 96 && key <= 105));

    })
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