$(document).ready(function () {
    var nowdate = new Date;
    var nowdatestr = (nowdate.getMonth() + 1) + '/' + nowdate.getDate() + '/' + (nowdate.getFullYear());
    //$("[id$=txtStartDate]").val(nowdatestr);
    //$('.pCH').hide();
    $(".btnSaveContinue").click(function (e) {
        var spinner = new Spinner(opts).spin($('.divSpinner')[0]);
    });
    $("[id$=txtStartDate]").change(function (e) {
        //var dateRegEx = "/^(0[1-9]|1[012]|[1-9])[- /.](0[1-9]|[12][0-9]|3[01]|[1-9])[- /.](19|20)\d\d$/"
        var tmpdate = IsDate($.trim($("[id$=txtStartDate]").val()));
        //if(IsDate($.trim($("[id$=txtStartDate]").val()))) { // true
        if (tmpdate != "") {
        } else {
            //var startDate = Date.parse($("[id$=txtStartDate]").val());
            var startDate = new Date($("[id$=txtStartDate]").val());
            var EndDate = new Date("12/31/9999");
            if (parseInt($("[id$=hCertTime]").val()) == 1) {
                var dd1 = new Date(startDate.getFullYear() + 1, startDate.getMonth(), startDate.getDate());
                EndDate = new Date(dd1.getFullYear(), dd1.getMonth(), dd1.getDate()-1);
            }
            else if ((parseInt($("[id$=hCertTime]").val()) == 11) && ($('.hCertEndDate').val() != '12/12/1999')) {
                var certEndDate = new Date($('.hCertEndDate').val());
                EndDate = new Date(certEndDate.getFullYear() + 1, certEndDate.getMonth(), certEndDate.getDate());
            }
            else if (parseInt($("[id$=hCertTime]").val()) == 2) {
                var DToday = new Date()
                var ed1 = new Date("8/31/" + startDate.getFullYear());
                var ed2 = new Date("8/31/" + (startDate.getFullYear() + 1));
                var ed3 = new Date("8/31/" + (startDate.getFullYear() + 2));
                if (ed1 > startDate && (ed1 > nowdate) && (ed1.getFullYear() % 2 != 0)) {
                    EndDate = ed1
                } else if (ed2.getFullYear() % 2 != 0) {
                    EndDate = ed2
                }
                else if (ed3.getFullYear() % 2 != 0) {
                    EndDate = ed3
                }

                //EndDate = new Date("8/31/" + (startDate.getFullYear() + 2));
            }
            else if (parseInt($("[id$=hCertTime]").val()) == 12) {
                var DToday = new Date()
                //var ed1 = new Date("8/31/" + startDate.getFullYear());
                var ed2 = new Date("8/31/" + (startDate.getFullYear() + 1));
                var ed3 = new Date("8/31/" + (startDate.getFullYear() + 2));
                if (ed2.getFullYear() % 2 != 0) {
                    EndDate = ed2
                }
                else if (ed3.getFullYear() % 2 != 0) {
                    EndDate = ed3
                }

                //EndDate = new Date("8/31/" + (startDate.getFullYear() + 2));
            }
            //var end_date =(startDate.getMonth()+1)+'/'+startDate.getDate()+'/'+(startDate.getFullYear()+ parseInt($("[id$=hCertTime]").val()));// new Date(startDate.getFullYear() + $("[id$=hCertTime]").val(), startDate.getmonth, startDate.getDay);
            $("[id$=txtEndDate]").val((EndDate.getMonth() + 1) + '/' + EndDate.getDate() + '/' + EndDate.getFullYear());
            if ($("[id$=txtEndDate]").val() == "12/31/9999") {
                $("[id$=txtEndDate]").val("");
            }
       }
    });
    $("[id$=btnToggleWE]").click(function (e) {
        if ($("[id$=btnToggleWE]").val() == 'View Work Experience') {
            $('.dWE').show();
            $("[id$=btnToggleWE]").prop('value', 'Hide Work Experience');
        }
        else {
            $('.dWE').hide();
            $("[id$=btnToggleWE]").prop('value', 'View Work Experience');
        }
    });
    $("[id$=btnToggleEmp]").click(function (e) {
        if ($("[id$=btnToggleEmp]").val() == 'View Employers') {
            $('.dEmployers').show();
            $("[id$=btnToggleEmp]").prop('value', 'Hide Employers');
        }
        else {
            $('.dEmployers').hide();
            $("[id$=btnToggleEmp]").prop('value', 'View Employers');
        }
    });
    $("[id$=btnSaveContinue]").click(function (e) {
        if (!confirm('Are you sure you want to submit the application with the selected status?')) {
            e.preventDefault();
            $('.divSpinner').hide();            
            return false;
        }
        var msge = ValidateDates();
        if (msge != '') {
            $(".ErrorSummary").show();
            $('.ErrorSummary')[0].innerHTML = msge;
            $(".ErrorSummary").focus();
            e.preventDefault();
        }
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
function ValidateDates() {
    var msg = '';
    var nowdate = new Date;
    
    if ($('.txtStartDate').val() == '') {
        msg = "Please enter StartDate"
    }
    var tmp = IsDate($.trim($(".txtStartDate").val()));
    if (tmp != "") {
        msg = "Please enter valid start date," + tmp;
    }
    if (msg!=''){
        return msg;
    }
    var startDate = new Date($("[id$=txtStartDate]").val());


    if ($('.hCertEndDate').val() == '12/12/1999') {
        if (($.trim($('.txtStartDate').val() != '')) && (Date.parse($.trim($('.txtStartDate').val())) > Date.parse(nowdate))) {
            msg = "Start date cannot be greater than today";
        }
    }
    if ($('.hCertEndDate').val() != '12/12/1999') {
        var certEndDate = new Date($('.hCertEndDate').val());
        var nextcertDate = new Date(certEndDate.getFullYear() + 1, certEndDate.getMonth(), certEndDate.getDate());
        if ((startDate>nextcertDate) && (startDate > Date.parse(nowdate))) {
            msg = "Start date cannot be greater than next certification span date";
        }
    }
    if (($.trim($('.hBiggerDate').val() != ''))) {
        if (($.trim($('.txtStartDate').val() != '')) && (Date.parse($.trim($('.txtStartDate').val())) < Date.parse($('.hBiggerDate').val()))) {
            msg = "Start date cannot be prior to " + $('.hBiggerName').val() + ":" + $.trim($('.hBiggerDate').val());
        }
    }
    //if ($('.txtEndDate').val() == '') {
    //    msg = "Please enter EndDate"
    //}
    //var tmp1 = IsDate($.trim($(".txtStartDate").val()));
    //if (tmp1 != "") {
    //    msg = "Please enter valid end date," + tmp1;
    //}
    //if ($('.txtEndDate').val() != '') {
    //    var tmpdate = IsDate($.trim($(".txtEndDate").val()));
    //    if (tmpdate != "") {
    //        msg = msg + "<br/>" + "Please enter valid end date," + tmpdate;
    //    }
    //    if ((Date.parse($(".txtStartDate").val())) >= (Date.parse($(".txtEndDate").val()))) {
    //        if (msg == '') {
    //            msg = "Please check start date can not be later than end date";
    //        }
    //        else {
    //            msg = msg + "<br/>" + "Please check start date can not be later than end date";
    //        }
    //    }
    //}
    //if (msg != '') {
       
    //}
    return msg;

}