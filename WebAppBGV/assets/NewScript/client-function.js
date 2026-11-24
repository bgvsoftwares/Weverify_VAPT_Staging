//Client Dashboard
$(document).ready(function () {
    $('#ClientRowID').val($('#hdnClientRowID').val());
});
$(function () {
    $('#ClientRowID').change(function () {
        var url = "/ClientArea/Home/RefreshDashboardDetails?" + $.param({ objval: $("#ClientRowID").val() });
        //@Html.Raw(@Url.Action("RefreshDashboardDetails", "Home", new { ClientRowID = 2}));
        $("#hdnClientRowID").val($("#ClientRowID").val());
        var notifications = $("#dvPartialView");
        notifications.load(url);
        //notifications.dialog("open");

    });
});
//Manage Client User
$(document).ready(function () {
    // 1st replace first column header text with checkbox
    $("#grid th").each(function () {
        if ($.trim($(this).text().toString().toLowerCase()) === "{checkall}") {
            $(this).text('');
            $("<input/>", { type: "checkbox", id: "cbSelectAll", value: "" }).appendTo($(this));
            //$(this).append("<span>Select All</span>");
        }
    });
    //2nd click event for header checkbox for select /deselect all
    $("#cbSelectAll").click(function () {
        var ischecked = this.checked;
        $('#grid').find("input:checkbox").each(function () {
            this.checked = ischecked;
        });
    });
    //3rd click event for checkbox of each row
    $("input[name='ids']").click(function () {
        var totalRows = $("#grid td :checkbox").length;
        var checked = $("#grid td :checkbox:checked").length;
        if (checked == totalRows) {
            $("#grid").find("input:checkbox").each(function () {
                this.checked = true;
            });
        }
        else {
            $("#cbSelectAll").removeAttr("checked");
        }
    });

});
function adchange(ChkData) {
    var id = ChkData.id;
    var checkeds = ChkData.checked;
    var flag = "";
    if (ChkData.checked == true) {
        flag = confirm('Are you sure to Enable this Record ?');
    }
    if (ChkData.checked == false) {
        flag = confirm('Are you sure to Disable this Record ?');
    }
    if (id != "" && flag) {
        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: '@Url.Action("ActiveClientLogin", "Home")',
            data: { "id": id, "checkeds": checkeds },
            dataType: "json",
            beforeSend: function () { },
            success: function (data) {
                if (data.result == true) {
                    alert("Record Update Successfully!!");
                    setTimeout(function () { window.location.replace("ManageClientUsers"); }, 500);
                }
                else {
                    alert('There is some error');
                }
            }
        });
    }
    else {
        if (ChkData.checked == false) {
            ChkData.checked = true;
        }
        else {
            ChkData.checked = false;
        }
    }
}



function reportchange(ChkData) {
    var id = ChkData.id;
    var checkeds = ChkData.checked;
    var flag = "";
    if (ChkData.checked == true) {
        flag = confirm('Are you sure to visible the Report ?');
    }
    if (ChkData.checked == false) {
        flag = confirm('Are you sure to not visible the Report ?');
    }
    if (id != "" && flag) {
        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: '@Url.Action("updateReportVisible", "Home")',
            data: { "id": id, "checkeds": checkeds },
            dataType: "json",
            beforeSend: function () { },
            success: function (data) {
                if (data.result == true) {
                    alert("Record Update Successfully!!");
                    setTimeout(function () { window.location.replace("ManageClientUsers"); }, 500);
                }
                else {
                    alert('There is some error');
                }
            }
        });
    }
    else {
        if (ChkData.checked == false) {
            ChkData.checked = true;
        }
        else {
            ChkData.checked = false;
        }
    }
}

$(".sendmail").on("click", function () {
    var str = $(this).attr("id").split("_");
    id = str[1];

    var flag = confirm('Do you want to send client login details?');
    if (id != "" && flag) {
        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: '@Url.Action("SendMailToClient", "Client")',
            data: { "id": id },
            dataType: "json",
            beforeSend: function () { },
            success: function (data) {
                if (data.result == true) {
                    alert("Login Details Sent Successfully!!");
                    setTimeout(function () { window.location.replace("ManageClientUsers"); }, 200);
                }
                else {
                    alert('There is some error');
                }
            }
        });
    }
});


//Register case
//$(document).ready(function () {
//    $('#dvChksPcks').hide();
    
//});
//$('#PricingType').change(function () {
//    alert($("#ClientRowID").val());
//    var url1 = "/ClientArea/Home/GetChecksAndPackageByClientId?id=" + $("#ClientRowID").val() + "&pt=" + $("#PricingType").val();
//    $.ajax({
//        url: url1,
//        type: 'GET',
//        async: false,
//        dataType: 'json',
//        success: function (allData) {
//            if (allData != null && allData != "") {
//                $('#dvChksPcks').show();
//                var items = '';
//                jQuery.each(allData, function (index, allData) {
//                    if (allData.CheckFamilyRowID != 0) {
//                        items += "<div  class='col-md-6'><input type='checkbox' class='chkPkgChks' value='Check-" + allData.ClientCheckRowID + "' /> " + allData.CheckOrPackageName + "</div>";
//                    }
//                    else {
//                        items += "<div  class='col-md-6'><input type='checkbox' class='chkPkgChks' value='Package-" + allData.CheckOrPackageName + "' /> " + allData.CheckOrPackageName + "</div>";
//                    }
//                });
//                $('#dvChecksPackages').html(items);
//            }
//            else {
//                $('#dvChksPcks').hide();
//                alert("No Checks And Packages Selected");
//            }
//        },
//        error: function (XMLHttpRequest, textStatus, errorThrown) {
//            alert("some error");
//        }
//    });
//});

//$(document).on('change', 'input[class="chkPkgChks"]', function (e) {
//    var selectedIds = '';
//    $('input[class="chkPkgChks"]').each(function (index, value) {
//        if (this.checked) { selectedIds += this.value + ','; }
//    });
//    if (selectedIds.length > 0) {
//        selectedIds = selectedIds.substring(0, selectedIds.length - 1);
//    } else { selectedIds = ''; }

//    $('#CandidateCheckOrPackage').val(selectedIds.trim());
//});

//function validateForm() {
//    if ($('#PricingType').val() == 0) {
//        alert("Please Select Pricing Type");
//        $('#PricingType').focus();
//        return false;
//    }

//    var crid = $("#ClientRowID").val();
//    if (crid == 636 || crid == 644 || crid == 645) {

//        if ($('#ddlDepartment').val() == 0) {
//            alert("Please Select Department");
//            $('#ddlDepartment').focus();
//            return false;
//        }
//    }

//    if (crid == 599) {
//        if ($('#ddlBand').val() == 0) {
//            alert("Please Select Band");
//            $('#ddlBand').focus();
//            return false;
//        }

//        if ($('#ddlCampusCase').val() == 0) {
//            alert("Please Select Campus Type");
//            $('#ddlCampusCase').focus();
//            return false;
//        }
//    }

//    var chkCount = 0;
//    $('input.chkPkgChks:checkbox').each(function (index, value) {
//        if (this.checked) {
//            chkCount += 1;
//        }
//    });

//    if (chkCount == 0) {
//        alert("Please select atleast one Check or Package.");
//        return false;
//    }

//    if (chkCount > 1 && $('#PricingType').val() == 1) {

//        alert("Please select  one  Package.");
//        return false;
//    }

//    if ($("#UploadDoc").val() == "") {
//        alert("Upload Candidate ducument");
//        $('#UploadDoc').focus();
//        return false;
//    }
//    return true;
//}

//function ForcevalidateForm() {

//    var crid = $("#ClientRowID").val();
//    if (crid == 636 || crid == 644 || crid == 645) {

//        if ($('#ddlDepartment').val() == 0) {
//            alert("Please Select Department");
//            $('#ddlDepartment').focus();
//            return false;
//        }
//    }


//    return true;
//}
//$(function () {
//    var crid = $("#ClientRowID").val();

//    if (crid == 8) {
//        $("#MobileNo").focusout(function () {
//            $("#ClientRefID").val($(this).val());
//        });
//    }
//})


//$(document).ready(function () {
//    var ClientRowID = $('#ClientRowID').val();
//    if (ClientRowID == 249) {

//        $("#ClientRefID").attr('maxlength', '9');
//    }
//    else {

//        $("#ClientRefID").attr('maxlength', '50');
//    }
//    //$('#ClientRowID').val($('#hdnClientRowID').val());
//    $('input[type=datetime]').datepicker({
//        dateFormat: "yy-mm-dd",
//        changeMonth: true,
//        changeYear: true,
//        showButtonPanel: true,
//        yearRange: "-66:+10",
//        //onClose: function (dateText, inst) {
//        //    $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, inst.selectedDay));
//        //}
//    });
//    var dNow = new Date();
//    var localdate = (dNow.getFullYear() + '-' + (dNow.getMonth() + 1) + '-' + dNow.getDate());
//    $('#OrderDate').val(localdate);
//});

//$(function () {
//    $('#ddlClientRowID').change(function () {
//        $("#ClientRowID").val($("#ddlClientRowID").val());
//        //alert($("#ClientRowID").val());
//        $('#PricingType').val(0);
//        $('#dvChksPcks').hide();
//        //alert($("#ClientRowID").val());
//    });
//}); 