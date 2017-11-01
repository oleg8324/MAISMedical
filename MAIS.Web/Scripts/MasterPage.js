$(document).ready(function () {
    var today = new Date();
    var lastDateForOldMAIS = new Date("03/01/2014");
    if (today > lastDateForOldMAIS) {
        $("[id$=lnkOldMASystem]").css('visibility', 'hidden');
        $("[id$=lnkOldMASystem]").hide();
    }
    else {
        $("[id$=lnkOldMASystem]").css('visibility', 'visible');
        $("[id$=lnkOldMASystem]").show();
    }
});