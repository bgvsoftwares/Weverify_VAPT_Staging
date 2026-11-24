
function PassMatch() {
    var $regexname = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})");

    if (!$("#Password").val().match($regexname)) {           
            $('.emsg').removeClass('hidden-pass');
            $('.emsg').show();            
            return false;
            
        }
        else {            
            $('.emsg').addClass('hidden-pass');          
            return true;
          
        }
    };