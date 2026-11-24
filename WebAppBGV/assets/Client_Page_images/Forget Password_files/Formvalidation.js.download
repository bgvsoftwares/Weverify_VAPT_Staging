//$(document).ready(function () {
//    $('.form-control').bind('paste', function (e) {
//        e.preventDefault(); //disable paste
//        //alert('cut,copy & paste options are disabled !!');
//    });
//});

$(function () {
    $('.valid-textbox').keydown(function (e) {
        if (e.altKey) {
            e.preventDefault();
        } else {
            var key = e.keyCode;
            if (!((key == 8) || (key == 9) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90))) {
                e.preventDefault();
            }
        }
    });

});

 // Accept only alpha numerics, no special characters 
function allowAlphaNumericSpace(d) {
    var code = ('charCode' in d) ? d.charCode : d.keyCode;
    if (!(code == 32) && // space
        !(code > 47 && code < 58) && // numeric (0-9)
        !(code > 64 && code < 91) && // upper alpha (A-Z)
        !(code > 96 && code < 123)) { // lower alpha (a-z)
        d.preventDefault();
    }
};
/*no special characters allowed only text, comma, Dash and space will be allowed*/
function ValidateUsername(event) {
    // alert("changed");
    var username = event.target.value;
    //for (var i = 0; i < getusername.length; i++) {
    //    var username = $(getusername[i]).val();
    //}
    var code = (event.keyCode ? event.keyCode : event.which);
    //alert(username);
    var expr = /^[a-zA-Z0-9 ,-]*$/;

    if (!expr.test(username)) {
        alert("Only Alphabets, Numeric, whitespace, Dot and Dash allowed.");
        event.target.value = "";
        event.preventDefault();
        return false;
    }
    else if (code == 13) {
        alert("Enter press is not allowed!");
        event.preventDefault();
        return false;
    }
    else {
        return true;
    }
};
//numeric space and comma
function ValidateMobileNo(evt) {   
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    var keyCode = key;
    key = String.fromCharCode(key);
    if (key.length == 0) return;
    var regex = /^[0-9 ,\b -]+$/;
    if (keyCode == 188 || keyCode == 190) {
        return;
    } else {
        if (!regex.test(key)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) theEvent.preventDefault();
        }
    }
    //var username = event.target.value;    
    ////var code = (event.keyCode ? event.keyCode : event.which);
    ////alert(username);
    //var expr = /^[0-9 \.,]+$/;

    //if (!expr.test(username)) {
    //    alert("Only Numeric, whitespace, Dot, Dash and maximun 10 digits allowed to enter.");
    //    event.target.value = "";
    //    event.preventDefault();
    //    return false;
    //}
    //else {
    //    return true;
    //}
};

// Accept only  numerics, no special characters 
function allowNumericSpace(w) {
    var code = ('charCode' in w) ? w.charCode : w.keyCode;
    if (!(code > 47 && code < 58)  // space
        ) { // lower alpha (a-z)
        w.preventDefault();
    }
};


