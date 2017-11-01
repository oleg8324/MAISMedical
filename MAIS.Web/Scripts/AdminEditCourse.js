function OneReq(source, args) {
    if (($.trim($('[id$=txtRNLNO]').val()) == "") && ($.trim($('[id$=txtFName]').val()) == "")) {
        source.errormessage = "Please enter either one of the search criteria";
        args.IsValid = false;
        //e.preventDefault();
    }
    else {
        args.IsValid = true;
    }
}