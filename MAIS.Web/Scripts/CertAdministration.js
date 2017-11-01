function pageLoad() {
    //$('.txtStDate').change(function () {
    //    var startDate = new Date($('.txtStDate').val());
    //    //var EndDate = new Date("12/31/9999");
    //    var dd1 = new Date(startDate.getFullYear() + 1, startDate.getMonth(), startDate.getDate());
    //    var EndDate = new Date(dd1.getFullYear(), dd1.getMonth(), dd1.getDate() - 1);
    //    $('.txtEDate').val((EndDate.getMonth() + 1) + '/' + EndDate.getDate() + '/' + EndDate.getFullYear());
    //});

    //$("[id$=txtStDate]").change(function (e) {
    //    var tmpdate = IsDate($.trim($("[id$=txtStDate]").val()));
    //    if (tmpdate != "") {
    //    } else {
    //        var startDate = new Date($("[id$=txtStDate]").val());
    //        var EndDate = new Date("12/31/9999");
    //        if ($("[id$=hIsPersonDD]").val() == "1") {
    //            var dd1 = new Date(startDate.getFullYear() + 1, startDate.getMonth(), startDate.getDate());
    //            EndDate = new Date(dd1.getFullYear(), dd1.getMonth(), dd1.getDate() - 1);
    //            $("[id$=txtEDate]").val((EndDate.getMonth() + 1) + '/' + EndDate.getDate() + '/' + EndDate.getFullYear());
    //        }
    //    }
    //});
    $('[id$=txtDtOccurance]').datepick();
    //$('[id$=txtEDate]').datepick();
    //$('[id$=txtStDate]').datepick();
   // $("[id$=btnSave]").click(function (e) {

   // });

   
    //$(".rblSelect").click(function (e) {
    //     var chkflg = $('.rblSelect input[type=radio]:checked').val();
    //     if (chkflg == 1) {
    //         $(".txtStDate").prop("disabled", true);
    //         $('[id$=ddCertStatus]').prop("disabled", false);
    //     }
    //     else if (chkflg == 2) {
    //         $(".txtStDate").prop("disabled", false);
    //         $('[id$=ddCertStatus]').prop("disabled", true);
    //     }
    //});
}

$(document).ready(function () {
    var nowdate = new Date;
    var nowdatestr = (nowdate.getMonth() + 1) + '/' + nowdate.getDate() + '/' + (nowdate.getFullYear());

});