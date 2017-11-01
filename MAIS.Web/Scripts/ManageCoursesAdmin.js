var CertificationStartDate = new Date()

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

$(document).ready(function () {
    $('.rblNewRnTypeSelect').click(function (x) {
        var $Test = new Boolean;
        $Test = true;

        if ($Test == true) {
            var spinner = new Spinner(opts).spin($('.divSpinner')[0]);
        }
    });


    $(".bntSearch").click(function (e) {
        var spinner = new Spinner(opts).spin($('.divSpinner')[0]);
    });
    function validate(input) {
        var date = new Date(input);
        input = input.split('/');
        return date.getMonth() + 1 === +input[0] &&
               date.getDate() === +input[1] &&
               date.getFullYear() === +input[2];
    };

    //$('.rdbInitial input[type=radio]:checked').val()
    $('.txtRNNO').mask("RN999999")

    //set up the Masking feilds
    $(".MaskOne, .MaskTwo").keydown(function (e) {
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

    var validCity = /^[A-Z,a-z'.\s]*$/
    var validZip = /^[0-9]+$/
    $(".CityMask").keypress(function (e) {
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (validCity.test(str)) {
            return true;
        }
        e.preventDefault();
        return false;
    })

    $(".ZipMask").keypress(function (e) {
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (validZip.test(str)) {
            return true;
        }
        e.preventDefault();
        return false;
    })
    // end of set up the Mask feilds

    if (($('.rblNewRnTypeSelect input[type=radio]:checked').is(':checked')) == 0) {

        $('.dvNewCourseTable').hide();
        //$('.dvAddSessions').hide();
        //$('.dvSessionDetailAdd').hide();

    }

    //$(".bntSaveCourse").click(function () {

    //    $(".dvAddSessions").show('slow');
    //    //$(".bntSaveCourse").attr("disable", true);
    //    //$(".bntCanceCourse").attr("disable", true);

    //})

    $(".bntMoveToSessionDetails").click(function () {
        $(".dvSessionDetailAdd").show("Slow");
    })

    $('.rblNewRnTypeSelect').click(function () {

        $('.dvNewCourseTable').show('slow');
        //__doPostBack('.rblNewRnTypeSelect', '0');
        //if ($('.rblNewRnTypeSelect input[type=radio]:checked').val() == 0) {
        //    if ($("#hdfMAISRole").val() == "RN Instructor") {
        //        $('.ddlRnNames').val($("#hdfRNID").val());
        //        $('.ddlRnNames').attr("disabled", true);
        //        //$('.ddlRnNames option[value=" $("#hdfRNID").val() "]').attr("selected", "selected");
        //    }
        //    if ($("#hdfMAISRole").val() == "RN Trainer") {
        //        $('.ddlRnNames').val($("#hdfRNID").val());
        //        $('.ddlRnNames').attr("disabled", true);
        //        //$('.ddlRnNames option[value=" $("#hdfRNID").val() "]').attr("selected", "selected");
        //    }
        //    $('.txtCourseNumberRnNew').attr("title", "");
        //    $('.txtCourseNumberRnNew').attr("disabled", false) // make text box allow to edit
        //    $('.txtCourseNumberRnNew').mask("AAA-999-999-****");
        //    __doPostBack('.rblNewRnTypeSelect', '0');

        //}
        //if ($('.rblNewRnTypeSelect input[type=radio]:checked').val() == 1) {
        //    if ($("#hdfMAISRole").val() == "RN Instructor") {
        //        $('.ddlRnNames').val($("#hdfRNID").val());
        //        $('.ddlRnNames').attr("disabled", true);
        //        //$('.ddlRnNames option[value=" $("#hdfRNID").val() "]').attr("selected", "selected");
        //    }
        //    if ($("#hdfMAISRole").val() == "RN Trainer") {
        //        $('.ddlRnNames').val($("#hdfRNID").val());
        //        $('.ddlRnNames').attr("disabled", true);
        //        //$('.ddlRnNames option[value=" $("#hdfRNID").val() "]').attr("selected", "selected");
        //    }
        //    $('.txtCourseNumberRnNew').attr
        //    $('.txtCourseNumberRnNew').attr("title", "This will auto create the Course Number on save.");
        //    $('.txtCourseNumberRnNew').val("");
        //    $('.txtCourseNumberRnNew').attr("disabled", true) //Make the text box disabled
        //    //$('.txtCourseNumberRnNew').mask("DODD-999-999-****"); // when the text box is disabled do not need the mask. 
        //    __doPostBack('.rblNewRnTypeSelect', '0');
        //}

    })

    if ($('.rblNewRnTypeSelect').is(':visible')) {
        if ($('.rblNewRnTypeSelect input[type=radio]:checked').val() == 0) {
            if ($("#hdfMAISRole").val() == "RN Instructor") {
                $('.ddlRnNames').val($("#hdfRNID").val());
                $('.ddlRnNames').attr("disabled", true);
                $('.ddlRnNames option[value=" $("#hdfRNID").val() "]').attr("selected", "selected");
                GetCerificationMinStartDate();
                TestCerificationStartDateIsAllowed();
            }
            if ($("#hdfMAISRole").val() == "RN Trainer") {
                $('.ddlRnNames').val($("#hdfRNID").val()).change();
                $('.ddlRnNames').attr("disabled", true);
                $('.ddlRnNames option[value=" $("#hdfRNID").val() "]').attr("selected", "selected");
                //GetCerificationMinStartDate();
                //TestCerificationStartDateIsAllowed();
            }
            $('.txtCourseNumberRnNew').attr("title", "");

            $('.txtCourseNumberRnNew').mask("a**-999-99-***?a", { completed: function () { $(this).val($(this).val().toUpperCase()); } });
            if ($('.txtEffectiveEndDate').prop('disabled') == true) {
                $('.txtCourseNumberRnNew').attr("disabled", true) // make text box allow to edit
            } else {
                $('.txtCourseNumberRnNew').attr("disabled", false) // make text box allow to edit
            }

        }
        if ($('.rblNewRnTypeSelect input[type=radio]:checked').val() == 1) {
            if ($("#hdfMAISRole").val() == "RN Instructor") {
                $('.ddlRnNames').val($("#hdfRNID").val());
                $('.ddlRnNames').attr("disabled", true);
                $('.ddlRnNames option[value=" $("#hdfRNID").val() "]').attr("selected", "selected");
                //GetCerificationMinStartDate();
                //TestCerificationStartDateIsAllowed();
            }
            if ($("#hdfMAISRole").val() == "RN Trainer") {
                $('.ddlRnNames').val($("#hdfRNID").val());
                $('.ddlRnNames').attr("disabled", true);
                $('.ddlRnNames option[value=" $("#hdfRNID").val() "]').attr("selected", "selected");
                //GetCerificationMinStartDate();
                //TestCerificationStartDateIsAllowed();
            }
            $('.txtCourseNumberRnNew').unmask();
            $('.txtCourseNumberRnNew').attr("title", "The system will auto create the course number when data is saved");
            if ($('#htMode').val() == "AddCourse") {
                $('.txtCourseNumberRnNew').val("");
            }
            $('.txtCourseNumberRnNew').attr("disabled", true) //Make the text box disabled
            //$('.txtCourseNumberRnNew').mask("DODD-999-999-****"); // when the text box is disabled do not need the mask. 
        }
    }



    if ($('.txtSessionStartDateNew').is(':visible')) {
        var mDate = $('.txtEffectiveStartDate').val();
        var mxDate = $('.txtEffectiveEndDate').val();
        $('.txtSessionStartDateNew').calendarsPicker('option', 'minDate', mDate);
        $('.txtSessionStartDateNew').calendarsPicker('option', 'maxDate', mxDate);
        $('.txtSessionEndDateNew').calendarsPicker('option', 'minDate', mDate);
        $('.txtSessionEndDateNew').calendarsPicker('option', 'maxDate', mxDate);
        $('.txtSessionEndDateNew').calendarsPicker('option', 'onClose', function (dates) {
            if (!($('.txtSessionStartDateNew').val() == "")) {
                TestSessionOverLap();
            }
        });

        $('.txtSessionStartDateNew').calendarsPicker('option',
            'onClose', function (dates) {
                //alert(dates);
                SetMinDateForSessionEndDate();
                if (!($('.txtSessionEndDateNew').val() == "")) {
                    TestSessionOverLap();
                }
            });
    }

    var SetMinDateForSessionEndDate = function () {
        var mDate = $('.txtSessionStartDateNew').val();
        $('.txtSessionEndDateNew').calendarsPicker('option', 'minDate', mDate);
        $('.txtSessionEndDateNew').val("");

    }
    //$('.txtSessionStartDateNew').blur(function () {
    //    var mDate = $('.txtSessionStartDateNew').val();
    //    $('.txtSessionEndDateNew').calendarsPicker('option', 'minDate', mDate);
    //});


    if ($('.txtAddClassDate.date-pick').is(':visible')) {
        var mDate = $('.txtSessionStartDateNew').val();
        var mxDate = $('.txtSessionEndDateNew').val();
        $('.txtAddClassDate').calendarsPicker('option', 'minDate', mDate);
        $('.txtAddClassDate').calendarsPicker('option', 'maxDate', mxDate);
        $('.txtAddClassDate').calendarsPicker('option', 'onClose', function () {
            TestIfClassExits();
        });


    }



    $('.ddlRnNames').change(function () {
        //alert($('.ddlRnNames option:selected')[0].value())
        if (!($('.ddlRnNames').val() == '-1')) {
           // GetCerificationMinStartDate();
           // TestCerificationStartDateIsAllowed();
            $(".txtEffectiveStartDate").val("")
        }
    });

    $(".txtEffectiveStartDate").change(function () {
        //if ($(".txtEffectiveEndDate").val() == "") {
        if (!($('.ddlRnNames').val() == -1)) {

            if (!($('.txtEffectiveStartDate').val() == "")) {
                if (validate($('.txtEffectiveStartDate').val())) {
                    //TestCerificationStartDateIsAllowed();
                    //var EffStartDate = new Date($('.txtEffectiveStartDate').val())
                    //if (EffStartDate >= CertificationStartDate) {
                    //    GetCerificationDate();
                    //}
                    //else {
                    //    //Set Error Message. 
                    //    var strDate = CertificationStartDate.getMonth() + 1 + "/" + CertificationStartDate.getDate() + "/" + CertificationStartDate.getFullYear();
                    //    alert("The date entered is not allowed. Must enter a date on or after " + strDate + ".");
                    //    $('.txtEffectiveStartDate').val("");

                    //}
                } else {
                    alert("This is not a vaild date.");
                    $(".txtEffectiveStartDate").val("");
                    $(".txtEffectiveStartDate").focus();
                }
            }
        } else {
            alert("Must select a RN before entering the statrt date of the course");
            $(".txtEffectiveStartDate").val("");
        }
        //var $d = new Date($(".txtEffectiveStartDate").val());
        //var str = $d.getMonth() + 1 + '/';
        //str += $d.getDate() + '/';
        //str += $d.getYear() + 2

        // $(".txtEffectiveEndDate").val($d);


        // }    
    })

    $(".txtEffectiveStartDate").blur(function () {
        //if ($(".txtEffectiveEndDate").val() == "") {
        if (!($('.ddlRnNames').val() == -1)) {

            if (!($('.txtEffectiveStartDate').val() == "")) {
                ////TestCerificationStartDateIsAllowed();
                //var EffStartDate = new Date($('.txtEffectiveStartDate').val())
                //if (EffStartDate >= CertificationStartDate) {
                //    GetCerificationDate();
                //}
                //else {
                //    //Set Error Message. 
                //    var strDate = CertificationStartDate.getMonth() + 1 + "/" + CertificationStartDate.getDate() + "/" + CertificationStartDate.getFullYear();
                //    alert("The date entered is not allowed. Must enter a date on or after " + strDate + ".");
                //    $('.txtEffectiveStartDate').val("");

                //}
            }
        } else {
            alert("Must select a RN before entering the statrt date of the course");
            $(".txtEffectiveStartDate").val("");
        }
        //var $d = new Date($(".txtEffectiveStartDate").val());
        //var str = $d.getMonth() + 1 + '/';
        //str += $d.getDate() + '/';
        //str += $d.getYear() + 2

        // $(".txtEffectiveEndDate").val($d);


        // }    
    })

    $('.txtCourseNumberRnNew').blur(function () {
        if (!$('.txtCourseNumberRnNew').val() == "") {
            TestCourseNumber();
        };
    });

    $('.txtSessionStartDateNew').blur(function () {
        var AllowDate = TestSessionStartDateIsAllowed();
        if (AllowDate == true) {

            if (!$('.txtSessionEndDateNew').val() == "") {

                if (!$('.txtSessionStartDateNew').val() == "") {
                    TestSessionOverLap();
                }
            }

        }
        else {
            if (validate($('.txtSessionStartDateNew').val())) {
                alert("The session start date must be between the course Start Date and Effective End Date.");
            } else {
                alert("This is not a valid date.");
            }

            $('.txtSessionStartDateNew').val("");
            $('.txtSessionStartDateNew').focus();
        }

    });

    $('.txtSessionEndDateNew').blur(function () {
        var AllowDate = TestSessionEndDateIsAllowed();

        if (AllowDate == true) {
            if (!$('.txtSessionStartDateNew').val() == "") {
                if (!$('.txtSessionEndDateNew').val() == "") {
                    TestSessionOverLap();
                }
            }
        }
        else {

            if (validate($('.txtSessionEndDateNew').val())) {
                alert("The session end date must be between the course Start Date and Effective End Date.");
            } else {
                alert("This Date is not valid.");
            }

            $('.txtSessionEndDateNew').val("");
            $('.txtSessionEndDateNew').focus();
        }

    });

    $('.txtAddClassDate').blur(function () {
        var AllowDate = TestClassDateIsAllowed();

        if (AllowDate == false) {
            if (validate($('.txtAddClassDate').val())) {
                alert('The class date must between the Session Start Date and the Session End Date.');
            } else {
                alert("This Date is not valid.");
            }
            $('.txtAddClassDate').val("");
            $('.txtAddClassDate').focus();
        }
    });

    $('.txtSearchSessionStartDate').blur(function () {
        if (!$('.txtSearchSessionStartDate').val() == "") {
            if (validate($('.txtSearchSessionStartDate').val()) == false) {
                alert('The Date is not valid.');
                $('.txtSearchSessionStartDate').val("");
            }
        }
    });

    var TestSessionStartDateIsAllowed = function () {
        var mEffectiveStartDate = new Date($('.txtEffectiveStartDate').val());
        var mEffiectiveEndDate = new Date($('.txtEffectiveEndDate').val());
        if (!$('.txtSessionStartDateNew').val() == "") {

            var cDate = new Date($('.txtSessionStartDateNew').val());

            if (cDate <= mEffiectiveEndDate && cDate >= mEffectiveStartDate && validate($('.txtSessionStartDateNew').val())) {
                return true;
            }
            else {
                return false;
            }
        }

        else {
            return true;
        }
    };


    var AlertShowing = false;

    var GetCerificationDate = function () {
        var RnDataTest = {
            'RNsID': $(".ddlRnNames").val(),
            'StartDate': $(".txtEffectiveStartDate").val()
        };
        executePageMethod('ManageCourses.aspx', 'GetRNsCerificationDate', "{'RnData':" + JSON.stringify(RnDataTest) + "}", function (data) {

            if (!(data.d == '1/1/0001')) {
                $('.txtEffectiveEndDate').val(data.d);
            }
            else {
                if (AlertShowing == false) {
                    AlertShowing = true;
                    alert('This RN is not certified with a start date this far in the future. Please enter a new start date.')
                    $(".txtEffectiveStartDate").val('');
                    $(".txtEffectiveStartDate").focus();
                    AlertShowing = false;
                }
            }
        }, function () {
            return false;
        });


    };





    var TestSessionEndDateIsAllowed = function () {
        var mEffectiveStartDate = new Date($('.txtEffectiveStartDate').val());
        var mEffiectiveEndDate = new Date($('.txtEffectiveEndDate').val());
        if (!$('.txtSessionEndDateNew').val() == "") {
            var cDate = new Date($('.txtSessionEndDateNew').val());

            if (cDate <= mEffiectiveEndDate && cDate >= mEffectiveStartDate && validate($('.txtSessionEndDateNew').val())) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return true;
        }

    };

    var TestCourseNumber = function () {
        var CourseNumberToTest = {
            'CourseNumber': $('.txtCourseNumberRnNew').val()
        };

        executePageMethod('ManageCourses.aspx', 'DoesCourseNumberExist', "{'CourseNumber':" + JSON.stringify(CourseNumberToTest) + "}", function (data) {

            if (data.d == true) {
                alert("You cannot use this course number. \n It is already been assigned to a course. \n Please enter in a new course number.  ");
                $('.txtCourseNumberRnNew').val("");
            }
            else {
                //This course number in not in user. user may use input course number. 
            };

            //$('.errMsg').hide();
            //$('.errMsg').css("color", "green");
            //$('.errMsg').css("border-color", "green");
            //$('.errMsg')[0].innerHTML = "Data is successfully saved";
            //$('.errMsg').show();
        }, function () {
            //alert('Course Number Does not match. Go to .');
            //$('.errMsg')[0].innerHTML = "An error occurred while attempting to save this application.";
            //$('.errMsg').show();
            return false;
        });

    };

    var TestSessionOverLap = function () {
        var SessionDataToTest = {
            'CourseNumber': $('.txtCourseNumberRnNew').val(),
            'SessionStartDate': $('.txtSessionStartDateNew').val(),
            'SessionEndDate': $('.txtSessionEndDateNew').val()
        }

        if ((!($('.txtSessionStartDateNew').val() == "")) && (!($('.txtSessionEndDateNew').val() == ""))) {
            executePageMethod('ManageCourses.aspx', 'DoesSessionOverlap', "{'SessionData':" + JSON.stringify(SessionDataToTest) + "}", function (data) {

                if (data.d == true) {
                    alert("The dates for this session over lap other session in this course.  \n Please enter new Start and End dates for this session.  ");
                    $('.txtSessionStartDateNew').val("");
                    $('.txtSessionEndDateNew').val("");
                }
                else {
                    //This session does not over lap the sessions in the course. user may use the session dates. 
                };

                //$('.errMsg').hide();
                //$('.errMsg').css("color", "green");
                //$('.errMsg').css("border-color", "green");
                //$('.errMsg')[0].innerHTML = "Data is successfully saved";
                //$('.errMsg').show();
            }, function () {
                //alert('Course Number Does not match. Go to .');
                //$('.errMsg')[0].innerHTML = "An error occurred while attempting to save this application.";
                //$('.errMsg').show();
                return false;
            });
        };
    };

    var TestClassDateIsAllowed = function () {
        var mSessionStartDate = new Date($('.txtSessionStartDateNew').val());
        var mSessionEndDate = new Date($('.txtSessionEndDateNew').val());

        if (!($('.txtAddClassDate').val() == "")) {
            var cDate = new Date($('.txtAddClassDate').val());
            if (cDate <= mSessionEndDate && cDate >= mSessionStartDate && validate($('.txtAddClassDate').val())) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return true;
        }

    };

    var TestIfClassExits = function () {
        var found = false
        $('.gvAddSessionDetails').find("tr").each(function () {
            if (!($('.txtAddClassDate').val() == "")) {
                if ($(this).find("td").each(function () {
                         if (found == false) {
                           if ($('.txtAddClassDate').val() == $(this)[0].innerText) {
                              alert("This date is already added to the Session Deatils. \n If you need to change the CE data, please remove the date by click on the remove button. ");
                              $('.txtAddClassDate').val("");
                              found = true;
                }
                }

                }));
            }
        });
    };
})