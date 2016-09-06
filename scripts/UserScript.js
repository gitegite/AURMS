$(document).ready(function () {
    $("#pendingButton").click(function () {
        $.ajax({
            type: "POST",
            url: "/USERrecord/UserPending",
            data: { userID: sessionStorage.UserName },
            success: function (data) {

            }
        });
    });
});