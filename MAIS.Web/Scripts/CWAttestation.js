$(document).ready(function() {
    $(".rblAgree").click(function(e) {
        $(".btnSaveRequirement").attr("disabled", false);
    });
    $(".btnSaveRequirement").click(function(e) {
        $(".txtInitial").val($.trim($(".txtInitial").val()));
        if ($(".txtInitial").val().length < 2) {
            alert("You must enter your initials.");
            e.preventDefault();
        }
    });
});
