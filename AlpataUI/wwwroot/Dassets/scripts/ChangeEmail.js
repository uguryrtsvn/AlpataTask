let timerState = false;
let timerMunite = 0;
let timerSecond = 0;
let nextConfirm = false;


const ChangeEmailVerify = async () => {
    let m = $("#activeMail").val();
    $("#changeMail").prop("disabled", true);
    if (m != "") {
        let obj = {
            email: m
        }
        $("#activeMail").attr("disable");
        await $.ajax({
            method: "post",
            url: "/Dashboard/ChangeEmailVerify", 
            datatype: 'json',
            data: obj,
            contenttype: 'application/json; charset=utf-8',
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response, statustext, jqxhr) {
                if (response.success) {
                    $("#mailWizard").steps("next");
                    $("#activeMail").text("");
                    if (!timerState) {
                        StartTimer(timerMunite, timerSecond);
                        timerState = true;
                    }
                    notyf.success(response.message);
                    return;
                }
                $("#changeMail").prop("disabled", false);
                notyf.error(response.message);
            },
            error: function (response, statustext, jqxhr) {
                $("#changeMail").prop("disabled", false);
                notyf.error("Doğrulama kodu oluşturulurken hata oluştu tekrar deneyiniz.");
            }
        });
    } else {
        $("#changeMail").prop("disabled", false);
        notyf.error("Aktif e-mail adresinizi giriniz.");
    }

}
const StartTimer = (minutes, seconds) => {
    let totalTime = minutes * 60 + seconds;
    let interval = setInterval(() => {
        var minutes = Math.floor(totalTime / 60);
        var seconds = totalTime % 60;

        var formattedTime = (minutes < 10 ? "0" : "") + minutes + ":" + (seconds < 10 ? "0" : "") + seconds;

        $("#timer").text(formattedTime);

        if (--totalTime < 0) {
            clearInterval(interval);
        }
    }, 1000);
}
const StartTimer2 = (minutes, seconds) => {
    let totalTime = minutes * 60 + seconds;
    let interval = setInterval(() => {
        var minutes = Math.floor(totalTime / 60);
        var seconds = totalTime % 60;

        var formattedTime = (minutes < 10 ? "0" : "") + minutes + ":" + (seconds < 10 ? "0" : "") + seconds;

        $("#timer2").text(formattedTime);

        if (--totalTime < 0) {
            clearInterval(interval);
        }
    }, 1000);
}
const CheckVerifyCodeState = async () => {
    let obj = {
        code: $("#code").val()
    }
    if (obj.code == "" || obj.code == null || obj.code.length != 6) {
        notifyError("Lütfen doğru kodu giriniz.");

        return;
    }
    $("#codeConfirm").prop("disabled", true);
    await $.ajax({
        method: "get",
        url: "/Dashboard/ConfirmVerifyCodeState",
        async: true,
        dataType: 'json',
        data: obj,
        contentType: 'application/json; charset=utf-8',
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,
        success: function (response) {
            if (response.success) {
                $("#mailWizard").steps("next");
                notyf.success(response.message);
                $("#code").text("");
                $("#codeConfirm").prop("disabled", false);
                return;
            }
            notifyError(response.message);
            $("#codeConfirm").prop("disabled", false);
        },
        error: function (response, statustext, jqxhr) {
            $("#codeConfirm").prop("disabled", false);
            notyf.error("Doğrulama kodu kontrolü sırasında hata oluştu tekrar deneyiniz.");
        }
    });
}

const ChangeNewEmailVerify = async () => {
    let m = $("#newEmail").val();
    let nm = $("#newEmailConfirm").val();
    if (m != "") {
        $("#checkNewMail").prop("disabled", true);
        let obj = {
            NewEmail: m,
            NewEmailConfirm: nm
        }
        await $.ajax({
            method: "post",
            url: "/Dashboard/ChangeNewEmailVerify",
            async: true,
            datatype: 'json',
            data: obj,
            contenttype: 'application/json; charset=utf-8',
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response, statustext, jqxhr) {
                if (response.success) {
                    $("#mailWizard").steps("next");
                    StartTimer2(5, 0);
                    notyf.success(response.message);
                    return;
                }
                $("#checkNewMail").prop("disabled", false);
                notyf.error(response.message);
            },
            error: function (response, statustext, jqxhr) {
                $("#checkNewMail").prop("disabled", false);
                notyf.error("Doğrulama kodu oluşturulurken hata oluştu tekrar deneyiniz.");
            }
        });
    } else {
        notyf.error("Yeni e-mail adresinizi giriniz.");
    }

}
const CheckNewMailVerifyCodeState = async () => {

    let m = $("#newEmail").val();
    let nm = $("#newEmailConfirm").val();

    let obj = {
        NewEmail: m,
        NewEmailConfirm: nm,
        UserVerification: {
            Token: $("#code2").val()
        }
       
    }
    if (obj.UserVerification.Token == "" || obj.UserVerification.Token == null || obj.UserVerification.Token.length != 6) {
        notifyError("Lütfen doğru kodu giriniz."); 
        return;
    }
    $("#newCodeConfirm").prop("disabled", true);
    await $.ajax({
        method: "post",
        url: "/Dashboard/UpdateEmail",
        async: true,
        dataType: 'json',
        data: JSON.stringify(obj),
        contentType: 'application/json; charset=utf-8',
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,
        success: function (response) {
            if (response.success) {
                notyf.success(response.message);
                $("#code2").text("");
                $("#newEmail").text("");
                $("#newEmailConfirm").text(""); 
                window.location.href = "/Dashboard/Index";
                return;
            }
            notifyError(response.message);
            $("#newCodeConfirm").prop("disabled", false);
        },
        error: function (response, statustext, jqxhr) {
            $("#newCodeConfirm").prop("disabled", false);
            notyf.error("Doğrulama kodu kontrolü sırasında hata oluştu tekrar deneyiniz.");
        }
    });
}

 
 