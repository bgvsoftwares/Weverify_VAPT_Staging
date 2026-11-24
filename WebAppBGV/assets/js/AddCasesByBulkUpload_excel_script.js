function file_XLS_Validation(fileInput) {
    var filePath = fileInput.value;
    var allowedExtensions = /(\.xls|\.xlsx|\.XLS|\.XLSX)$/i;
    if (!allowedExtensions.exec(filePath)) {
        swal('Please upload file having extensions .xls/.xlsx only.');
        fileInput.value = '';
        return false;
    } else {
        return true;
    }
}

function file_Doc_Validation(fileInput) {
    var filePath = fileInput.value;
    var allowedExtensions = /(\.gif|\.GIF\.doc|\.docx|\.xls|\.xlsx|\.pdf|\.rtf|\.txt|\.jpeg|\.jpg|\.png|\.eml|\.email|\.msg|\.DOC|\.DOCX|\.XLS|\.XLSX|\.PDF|\.RTF|\.TXT|\.JPEG|\.JPG|\.PNG|\.EML|\.EMAIL|\.MSG|\.zip|\.ZIP|\.rar|\.RAR)$/i;
    if (!allowedExtensions.exec(filePath)) {
        swal('Please upload file having extensions .doc/.gif/.GIF/.docx/.xls/.xlsx/.pdf/.rtf/.txt/.jpeg/.jpg/.png/.eml/.email/.msg/.DOC/.DOCX/.XLS/.XLSX/.PDF/.RTF/.TXT/.JPEG/.JPG/.PNG/.EML/.EMAIL/.MSG/.ZIP.zip only.');
        fileInput.value = '';
        return false;
    } else {
        return true;
    }
}