$(document).ready(function () {
    
    $(".PhoneFormat").text(function (i, text) {
        return text.replace(/(\d{3})(\d{3})(\d{4})/, '$1-$2-$3');
    });
        
    $('.txtRnLicenseNumber').mask("RN999999");
});