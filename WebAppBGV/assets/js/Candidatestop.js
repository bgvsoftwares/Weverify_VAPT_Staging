$('.Stop').click(function () {
    var str = $(this).attr("id").split("_");
    var id = str[1];
    swal({
        title: "Are you sure?",
        text: "You will not be able to recover this imaginary file!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Yes, delete it!",
        cancelButtonText: "No, cancel plx!",
        closeOnConfirm: false,
        closeOnCancel: false
    },
        function (isConfirm) {
            if (id != "") {
                $.ajax({
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    //url: '@Url.Action("ClientCaseStopById", "Home", new { area = "ClientArea" })',
                    url: "/ClientArea/Home/ClientCaseStopById/",
                    data: { "id": id },
                    dataType: "json",
                    //beforeSend: function () { },
                    success: function (data) {

                        if (data.result == true && isConfirm) {
                            swal("Deleted!", "Your imaginary file has been deleted.", "success");
                            setTimeout(function () { window.location.replace("ManagePendingWithCandidateCases"); }, 3000);
                        }
                        else {
                            swal("Cancelled", "Your imaginary file is safe :)", "error");
                        }
                    }
                });
            }
        });
});