$(document).ready(function () {

    

    //$(".rblAgree").click(function(e) {
    //    $(".btnSaveRequirement").attr("disabled", false);
    //});
    $(".btnSaveRequirement").click(function(e) {
        $(".txtInitials").val($.trim($(".txtInitials").val()));
        if ($(".txtInitials").val().length < 2) {
            $(".lblAgreed").val("");
            alert("You must enter at least two letters for your initials.");
            e.preventDefault();
        }
    });
});

