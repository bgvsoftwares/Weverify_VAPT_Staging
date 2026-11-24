//onkeypress = "return isNumberKey(event,false)"
function isNumberKey(e, decimal) {
    var key;
    var keychar;

    if (window.event) {
        key = window.event.keyCode;
    }
    else if (e) {
        key = e.which;
    }
    else {
        return true;
    }
    keychar = String.fromCharCode(key);

    //if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 13) || (key == 27) || (key == 65) || (key == 97)) {
    if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 13) || (key == 27)) {
        return true;
    }
    else if ((("0123456789").indexOf(keychar) > -1)) {
        return true;
    }
    else if (decimal && (keychar == ".")) {
        return true;
    }
    else
        return false;
}

//Username & Password(a-z ! # @ $ & * ( ) . _ - / 0-9)
function BlockBlacklist(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode    
    if (unicode != 8 && unicode != 9 && unicode != 11 && unicode != 33 && unicode != 34 && unicode != 35 && unicode != 36 && unicode != 37 && unicode != 38 && unicode != 40 && unicode != 41 && unicode != 42 && unicode != 43 && unicode != 61 && unicode != 94 && unicode != 95 && unicode != 13) {
        if ((unicode < 45 || unicode > 58) && (unicode < 64 || unicode > 90) && (unicode < 97 || unicode > 122)) {
            return false;
        }
    }
}

//Password(a-z A-Z 0-9 !@#$%^&_)  It'll accept only these Chars... 
function Password(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    //if(unicode!=8 && unicode!=9 && unicode!=11 && unicode!=35 && unicode!=36 && unicode!=38 && unicode!=40 && unicode!=41 && unicode!=95 && unicode!=13)
    if (unicode != 8 && unicode != 9 && unicode != 11 && unicode != 13 && unicode != 33 && unicode != 35 && unicode != 36 && unicode != 37 && unicode != 38 && unicode != 94 && unicode != 95) {
        if ((unicode < 48 || unicode > 57) && (unicode < 64 || unicode > 90) && (unicode < 97 || unicode > 122)) {
            return false;
        }
    }
}

//function blockcopypaste1(e)
function noCopyMouse(e) {
    var isRight = (e.button) ? (e.button == 2) : (e.which == 3);
    if (isRight) {
        alert('You are prompted to type this twice for a reason!');
        return false;
    }
    return true;
}

//Disable Esc Key
function DisableEscape(evt) {
    evt = evt || window.event;
    if (evt && evt.keyCode == 27) {
        return false;
    }
}