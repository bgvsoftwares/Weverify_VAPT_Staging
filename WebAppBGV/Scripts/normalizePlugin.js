(function ($) {
    var sqlVal = [
        { name: 'SELECT' },
        { name: 'ADDCONSTRAINT' },
        { name: 'ALTER' },
        { name: 'ALTERCOLUMN' },
        { name: 'ALTERTABLE' },
        { name: 'ALL' },
        { name: 'AND' },
        { name: 'ANY' },
        { name: 'AS' },
        { name: 'ASC' },
        { name: 'BACKUPDATABASE' },
        { name: 'BETWEEN' },
        { name: 'CASE' },
        { name: 'CHECK' },
        { name: 'COLUMN' },
        { name: 'CONSTRAINT' },
        { name: 'CREATE' },
        { name: 'CREATEDATABASE' },
        { name: 'CREATEINDEX' },
        { name: 'CREATEOR REPLACE VIEW' },
        { name: 'CREATETABLE' },
        { name: 'CREATEPROCEDURE' },
        { name: 'CREATEUNIQUE INDEX' },
        { name: 'CREATEVIEW' },
        { name: 'DATABASE' },
        { name: 'DEFAULT' },
        { name: 'DELETE' },
        { name: 'DESC' },
        { name: 'DISTINCT' },
        { name: 'DROP' },
        { name: 'DROPCOLUMN' },
        { name: 'DROPCONSTRAINT' },
        { name: 'DROPDATABASE' },
        { name: 'DROPDEFAULT' },
        { name: 'DROPINDEX' },
        { name: 'DROPTABLE' },
        { name: 'DROPVIEW' },
        { name: 'EXEC' },
        { name: 'EXISTS' },
        { name: 'FOREIGNKEY' },
        { name: 'FROM' },
        { name: 'FULLOUTERJOIN' },
        { name: 'GROUPBY' },
        { name: 'HAVING' },
        { name: 'IN' },
        { name: 'INDEX' },
        { name: 'INNERJOIN' },
        { name: 'INSERTINTO' },
        { name: 'INSERTINTOSELECT' },
        { name: 'ISNULL' },
        { name: 'ISNOTNULL' },
        { name: 'JOIN' },
        { name: 'LEFTJOIN' },
        { name: 'LIKE' },
        { name: 'LIMIT' },
        { name: 'NOT' },
        { name: 'NOTNULL' },
        { name: 'OR' },
        { name: 'ORDERBY' },
        { name: 'OUTERJOIN' },
        { name: 'PRIMARYKEY' },
        { name: 'PROCEDURE' },
        { name: 'RIGHTJOIN' },
        { name: 'ROWNUM' },
        { name: 'WHERE' },
        { name: 'SELECTDISTINCT' },
        { name: 'SELECTINTO' },
        { name: 'SELECTTOP' },
        { name: 'SET' },
        { name: 'TABLE' },
        { name: 'TOP' },
        { name: 'TRUNCATETABLE' },
        { name: 'UNION' },
        { name: 'UNIONALL' },
        { name: 'UNIQUE' },
        { name: 'UPDATE' },
        { name: 'VALUES' },
        { name: 'VIEW' }
    ];

 
    $.fn.NormalizedString = function (options) {
        var $inputs = $(this);
        var cnt = 0;
        var keywrd = "";
        var msg = "";
        var result = true;
       $inputs.each(function () {
            keywrd = $(this).val();
            var filter = sqlVal.filter(c => c.name.toLowerCase().trim() == keywrd.toLowerCase().trim());
           if (filter.length > 0 && keywrd != "") {
               msg = msg + '<div class="errormsg" style="text-align:center;">Not Allow Keyword :<span style="color:red;">' + keywrd + '</span></div>';
               cnt += 1;
            }
        });
        if (cnt > 0) {
            result = false;
            Swal.fire({
                icon: 'info',
                title: 'Info',
                html: msg
            }); 
            return result;
        }

    };
    

    $.fn.AlphabetWithSpace = function (options) {
        var $inputs = $(this);
        $inputs.on("input", function () {
            var regexp = /[^a-zA-Z\s]/g;
            if ($(this).val().match(regexp)) {
                $(this).val($(this).val().replace(regexp, ''));
            }
        });
      
    };

    $.fn.AlphaNumericWithMinus = function (options) {
        var $inputs = $(this);
        $inputs.on("input", function () {
            var regexp = /[^a-zA-Z0-9-\s]/g;
            if ($(this).val().match(regexp)) {
                $(this).val($(this).val().replace(regexp, ''));
            }
        });

    };

    $.fn.NumberValidation = function (options) {
        var $inputs = $(this);
        $inputs.on("input", function () {
            var regexp = /[^0-9]/g;
            if ($(this).val().match(regexp)) {
                $(this).val($(this).val().replace(regexp, ''));
            }
        });
    };

    $.fn.EmailString = function (options) {
        var $inputs = $(this);
        $inputs.on("input", function () {
            var regexp = /[^a-zA-Z0-9-_.@]/g;
            if ($(this).val().match(regexp)) {
                $(this).val($(this).val().replace(regexp, ''));
            }
        });
    };

    $.fn.AddressString = function (options) {
        var $inputs = $(this);
        $inputs.on("input", function () {
            var regexp = /[^a-zA-Z0-9-\s,()]/g;
            if ($(this).val().match(regexp)) {
                $(this).val($(this).val().replace(regexp, ''));
            }
        });

    };

    $.fn.RemarksString = function (options) {
        var $inputs = $(this);
        $inputs.on("input", function () {
            var regexp = /[^a-zA-Z0-9-\s,()]/g;
            if ($(this).val().match(regexp)) {
                $(this).val($(this).val().replace(regexp, ''));
            }
        });

    };

    $.fn.SearchString = function (options) {
        var $inputs = $(this);
        $inputs.on("input", function () {
            var regexp = /[^a-zA-Z0-9-_.@\s,]/g;
            if ($(this).val().match(regexp)) {
                $(this).val($(this).val().replace(regexp, ''));
            }
        });
    };

    ////-------------How to Call----------------
    //// $("selectors").RemarksString();
    ////------------------------------------------

})(jQuery);