$(function () {
    $('.date-pick').calendarsPicker({ yearRange: '-80:+5',
        onClose: function (dates) {
            $(this).trigger('change');
        }
    });
});

function isInteger(s) {
    var i;
    for (i = 0; i < s.length; i++) {
        var c = s.charAt(i);
        if (((c < "0") || (c > "9"))) return false;
    }
    return true;
}

function stripCharsInBag(s, bag) {
    var i;
    var returnString = "";
    for (i = 0; i < s.length; i++) {
        var c = s.charAt(i);
        if (bag.indexOf(c) == -1) returnString += c;
    }
    return returnString;
}

function daysInFebruary(year) {
    return (((year % 4 == 0) && ((!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28);
}
function DaysArray(monthsInYr, yr) {
    for (var i = 1; i <= monthsInYr; i++) {
        if (i == 4 || i == 6 || i == 9 || i == 11)
            this[i] = 30;
        else if (i == 2)
            this[i] = daysInFebruary(yr);
        else
            this[i] = 31;
    }
    return this;
}

function IsDate(dtStr) {
    if (dtStr) {
        var dtCh = "/";
        var minYear = 1800;
        var maxYear = 9999;

        var pos1 = dtStr.indexOf(dtCh);
        var pos2 = dtStr.indexOf(dtCh, pos1 + 1);
        var strMonth = dtStr.substring(0, pos1);
        var strDay = dtStr.substring(pos1 + 1, pos2);
        var strYear = dtStr.substring(pos2 + 1);

        strYr = strYear;

        if (strDay.charAt(0) == "0" && strDay.length > 1) strDay = strDay.substring(1);
        if (strMonth.charAt(0) == "0" && strMonth.length > 1) strMonth = strMonth.substring(1);

        for (var i = 1; i <= 3; i++) {
            if (strYr.charAt(0) == "0" && strYr.length > 1) strYr = strYr.substring(1);
        }

        month = parseInt(strMonth);
        day = parseInt(strDay);
        year = parseInt(strYr);

        var daysInMonth = DaysArray(12, year);

        if (pos1 == -1 || pos2 == -1) {
            return "The date format is : mm/dd/yyyy";
        }
        if (strMonth.length < 1 || month < 1 || month > 12) {
            return "Please enter a valid month";
        }
        if (strDay.length < 1 || day < 1 || day > 31 || (month == 2 && day > daysInFebruary(year)) || day > daysInMonth[month]) {
            return "Please enter a valid day";
        }
        if (strYear.length != 4 || year == 0 || year < minYear || year > maxYear) {
            return "Please enter a valid 4 digit year between " + minYear + " and " + maxYear;
        }
        if (dtStr.indexOf(dtCh, pos2 + 1) != -1 || isInteger(stripCharsInBag(dtStr, dtCh)) == false) {
            return "Please enter a valid date";
        }
    }
    return "";
}

var executePageMethod = function (pageName, methodName, dataString, callback, error) {
    if (typeof (callback) == 'function' && typeof (error) == 'function') {
        $.ajax({
            type: "POST",
            url: pageName + "/" + methodName,
            data: dataString,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: callback,
            error: error
        });
    }
};