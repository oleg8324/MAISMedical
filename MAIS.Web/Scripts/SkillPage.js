$(document).ready(function () {
    var currentTime = new Date()
    var month = currentTime.getMonth() + 1
    var day = currentTime.getDate()
    var year = currentTime.getFullYear()

    var CurrectDate = month + '/' + day + '/' + year

    function validate(input) {
        var date = new Date(input);
        input = input.split('/');
        return date.getMonth() + 1 === +input[0] &&
               date.getDate() === +input[1] &&
               date.getFullYear() === +input[2];
    };

    //$(".ckbSkillsSelectAll").click(function (e) {
       
    //    //if ($(".ckbSkillsSelectAll input[type=checkbox][checked]").is(":checked") == 0) {
    //    //if ($(".ckbSkillsSelectAll").prop("Checked") == 1) {
    //    $(".tblSkills").toggle();
    //    //}
       

    //});


    $(".txtSkillsDate").calendarsPicker('option', 'maxDate', CurrectDate);



    $(".txtSkillsDate").blur(function () {
        if (!($('.txtSkillsDate').val() == '')) {
            if (validate($(".txtSkillsDate").val()) == false) {
                alert("This is not a valid date.");
                $(".txtSkillsDate").val("");
                $(".txtSkillsDate").focus();
            }
        }

    });

    var GetCerificationMinStartDate = function () {
        var RnDataTest = {
            'DDPersonalCodeID': $("#hdfDDPeronalID").val()
        };


        executePageMethod('Skills.aspx', 'GetDDPersonalCertificationMinStartDate', "{'DDPersonalCodeID':" + JSON.stringify(RnDataTest) + "}", function (data) {

            if (!(data.d == '1/1/0001')) {


                $('.txtSkillsDate').calendarsPicker('option', 'minDate', data.d);

            } else {
                $('.txtSkillsDate').calendarsPicker('option', 'minDate', '');

            }


        }, function () {
            return false;
        });

    };

    
    GetCerificationMinStartDate();

});