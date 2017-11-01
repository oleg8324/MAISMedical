function pageLoad() {
    $('[id$=txtDtOccurance]').datepick();
    $('[id$=txtStDate]').datepick();
   
    //$('#inlineDatepicker').datepick({ onSelect: showDate });

    //$(".date-pick").datepick();
    //var fileref = document.createElement('script');
    //fileref.setAttribute("type", "text/javascript");
    //fileref.setAttribute("src", "Scripts/jquery/jquery.1.6.2.min.js");

    //var fileref1 = document.createElement('script');
    //fileref1.setAttribute("type", "text/javascript");
    //fileref1.setAttribute("src", "Scripts/jquery/jquery.calendars.pack.js");

    //var fileref2 = document.createElement('script');
    //fileref2.setAttribute("type", "text/javascript");
    //fileref2.setAttribute("src", "Scripts/jquery/jquery.calendars.picker.pack.js");
    //$('.date-pick').datePicker({ clickInput: true });
}
function CheckSelected(source, args) {
    var valid = true;
    $('[id$=cklReason]').each(function (i, v) {
        valid = valid && ($(this).find(':checkbox:checked').size() >= 1);
    });
    args.IsValid = valid;
    //if (($.trim($('[id$=txtRNLNO]').val()) == "") && ($.trim($('[id$=txtFName]').val()) == "")) {
    //    source.errormessage = "Please enter either one of the search criteria";
    //    args.IsValid = false;
    //    //e.preventDefault();
    //}
    //else {
    //    args.IsValid = true;
    //}
}
$(document).ready(function () {
    $(".btnSaveNotation").click(function (e) {
        var spinner = new Spinner(opts).spin($('.divSpinner')[0]);
        if (Page_ClientValidate("a") == false) {
            //alert("Works!");
            $('.divSpinner').hide();
        } else {
            if (!confirm('Are you sure you want to save changes?')) {
                e.preventDefault();
                $('.divSpinner').hide();
                return false;
            }
        }
    });
    //if ($('[id$=hNotId]').val() == "0") {
    //    $(".PEnterNotation").hide();
    //}
   // if ($("[id$=hPb]").val() == "0") {
        //opt_DNMR = $();
        //opt_other = $();
        //var t = false;
        //$("[id$=lbHReasons] > option").each(function () {
        //    if ($(this).text() == "DODD") {
        //        t = true;
        //    }
        //    if (t) {
        //        //if ($(this).prop("selected") == true) {
        //        //alert("selected");
        //        //  $(this).removeAttr('selected');
        //        //  opt_other = opt_other.add($(this));
        //        //  $(this).prop("selected", true);
        //        // }
        //        // else {
        //        opt_other = opt_other.add($(this).clone());
        //        //}
        //    }
        //    else {
        //        opt_DNMR = opt_DNMR.add($(this).clone());
        //    }
        //});
        //opt_other.each(function () {
        //    if ($(this).prop("selected") == true) {
        //        $(this).removeAttr('selected');
        //    }


        //});
        //opt_DNMR.each(function () {
        //    if ($(this).prop("selected") == true) {
        //        $(this).removeAttr('selected');
        //    }
        //});
    //}
    $("[id$=btnShowAddNotation]").click(function () {
        $(".PEnterNotation").show();
        //$("[id$=hNotId]").val("0");
        //$("[id$=lblDt]")[0].innerHTML = GetTodayDate();//"12/12/2012";need to set as todays date
        //$("[id$=txtDtOccurance]").val("");
        //$('[id$=ddNotationType]').find("option:contains(Select)").prop('selected', true);
        ////$('[id$=ddNotationType]').change();
        //$('[id$=chbUnflag]').prop("checked", false);
        //$('[id$=chbUnflag]').prop("disabled", true);
        //$(".txtUFDate").val("");


    });
//    for (var i = 13; i > 0; i--) {
//        if (i > 5) {
//            opt_other = opt_other.add($('[id$=ddNotation]').find('option[value="' + i + '"]'));
//        }
//        else {
//            opt_DNMR = opt_DNMR.add($('[id$=ddNotation]').find('option[value="' + i + '"]'));
//        }
    //    }
    if ($('[id$=hNotId]').val() == "0") {
       // $('[id$=hhlbReason]').children('option').remove();
    }
    else {
        //$("[id$=lbReason] > option").each(function () {
            //    if ($(this).prop("selected") == false) {
            //        $(this).remove();
            //    }
            //});
            $('[id$=lbReason]').prop("disabled", true);
            //$('[id$=ddlStatus]').prop("disabled", true);
        //});
    //$('[id$=ddlStatus]').change(function () {
    //    $('.PEnterNotation').hide();
    //    var t = $(this).find("option:selected").text()
    //    if (t != "Approved") {
    //        $(".PEnterNotation").show();
    //        //Append the status to notation reason box
    //        $(".ErrorSummary").show();
    //        $(".ErrorSummary")[0].innerHTML = "You Must Enter an appropriate Notation if you selected this status."
    //        $('[id$=ddNotationType]').find("option:contains(" + t + ")").prop('selected', true);
    //        $("select[id$=ddNotationType]").change();
    //       // alert("You must enter Notation with this status!");

    //    }
    }
    //if ($('[id$=hStatus]').val() != "") {
    //    var hstat = $('[id$=hStatus]').val();
    //    var notstat = "";
    //    if (hstat == "Did Not Meet Requirements") {
    //        notstat = "Does not meet requirements"
    //    } else if (hstat == "Removed From Registry") {
    //        notstat = "Unregistered"
    //    } else if (hstat == "DODD Review") {
    //        notstat = "DODD Review"
    //    } else if (hstat == "Intent to Deny") {
    //        notstat = "Intent to Deny"
    //    } else if (hstat == "Denied") {
    //        notstat = "Deny"
    //    }

    //    $(".PEnterNotation").show();
    //    //Append the status to notation reason box
    //    $(".ErrorSummary").show();
    //    $(".ErrorSummary")[0].innerHTML = "You Must Enter an appropriate Notation if you selected status: " + hstat;
    //    $('[id$=ddNotationType]').find("option:contains(" + notstat + ")").prop('selected', true);

    //    if ($("select[id$=ddNotationType]").val() == "0") {
    //        //$('[id$=ddNotation]').removeAttr("disabled", true);
    //        $('[id$=hlbReason]').attr("disabled", true);

    //    }
    //        //$("select[id$=ddNotationType]").text()
    //    else if ($("[id$=ddNotationType]").find("option:selected").text() == "Does not meet requirements") {
    //        $('[id$=hlbReason]').removeAttr("disabled", true);
    //        $('[id$=hlbReason]').children('option').remove();
    //        $('[id$=hlbReason]').append(opt_DNMR);
    //    }
    //    else {
    //        $('[id$=hlbReason]').removeAttr("disabled", true);
    //        $('[id$=hlbReason]').children('option').remove();
    //        $('[id$=hlbReason]').append(opt_other);
    //    }

    //    //$("select[id$=ddNotationType]").change();
    //}

    $('[id$=chbUnflag]').change(function () {
        if ($('[id$=chbUnflag]').prop('checked')) {
            $(".txtUFDate").val(GetTodayDate());
        } else {
            $(".txtUFDate").val("");
        }
    });

    //$('[id$=ddNotationType]').change(function () {
    //    if ($("select[id$=ddNotationType]").val() == "0") {
    //        //$('[id$=ddNotation]').removeAttr("disabled", true);
    //        $('[id$=hlbReason]').attr("disabled", true);

    //    }
    //    //$("select[id$=ddNotationType]").text()
    //    else if ($(this).find("option:selected").text() == "Does not meet requirements") {
    //        $('[id$=hlbReason]').removeAttr("disabled", true);
    //        $('[id$=hlbReason]').children('option').remove();
    //        $('[id$=hlbReason]').append(opt_DNMR);
    //    }
    //    else {
    //        $('[id$=hlbReason]').removeAttr("disabled", true);
    //        $('[id$=hlbReason]').children('option').remove();
    //        $('[id$=hlbReason]').append(opt_other);
    //    }
    //});
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
function GetTodayDate() {
    var d = new Date();
    return ((d.getMonth() + 1) + '/' + d.getDate() + '/' + d.getFullYear());
}