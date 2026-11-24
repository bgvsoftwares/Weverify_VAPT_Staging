function disableBackButton() {
    window.history.forward();
}
setTimeout("disableBackButton()", 0);

function killBackSpace(e) {
    e = e ? e : window.event;
    var t = e.target ? e.target : e.srcElement ? e.srcElement : null;
    if (t && t.tagName && (t.type && /(password)|(text)|(number)|(file)/.test(t.type.toLowerCase())) || t.tagName.toLowerCase() == 'textarea')
        return true;
    var k = e.keyCode ? e.keyCode : e.which ? e.which : null;
    if (k == 8) {
        if (e.preventDefault)
            e.preventDefault();
        //alert("BackButton Disabled");
        return false;
    };
    return true;
};
if (typeof document.addEventListener != 'undefined')
    document.addEventListener('keydown', killBackSpace, false);
else if (typeof document.attachEvent != 'undefined')
    document.attachEvent('onkeydown', killBackSpace);
else {
    if (document.onkeydown != null) {
        var oldOnkeydown = document.onkeydown;
        document.onkeydown = function (e) {
            oldOnkeydown(e);
            killBackSpace(e);
        };
    }
    else
        document.onkeydown = killBackSpace;
}

window.history.forward();
function noBack()
{
    window.history.forward();
}

document.attachEvent("onkeydown", my_onkeydown_handler)
function my_onkeydown_handler() {

    if (event.keyCode == 116) {
        event.keyCode = 0;
        event.returnValue = false;
    }
}