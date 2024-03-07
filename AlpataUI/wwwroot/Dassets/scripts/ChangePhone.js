let timerState = false;
let timerMunite = 0;
let timerSecond = 0;
let nextConfirm = false;


//phoneInput.value.replace(/\D/g, '');


const CheckPhoneNumber = async () => {
    let p = $("#activePhone").val().replace(/\D/g, '');
    $("#changePhone").prop("disabled", true);
    if (p != "") {
        let obj = {
            phone: p
        }
        await $.ajax({
            method: "post",
            url: "/Dashboard/ChangePhoneVerify",
            datatype: 'json',
            data: obj,
            contenttype: 'application/json; charset=utf-8',
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response, statustext, jqxhr) {
                if (response.success) {
                    $("#phoneWizard").steps("next");
                    $("#activePhone").text("");
                    if (!timerState) {
                        StartTimer(timerMunite, timerSecond);
                        timerState = true;
                    }
                    notyf.success(response.message);
                    return;
                }
                $("#changePhone").prop("disabled", false);
                notyf.error(response.message);
            },
            error: function (response, statustext, jqxhr) {
                $("#changePhone").prop("disabled", false);
                notyf.error("Doğrulama kodu oluşturulurken hata oluştu tekrar deneyiniz.");
            }
        });
    } else {
        $("#changePhone").prop("disabled", false);
        notyf.error("Aktif telefon adresinizi giriniz.");
    }

}
const CheckPhoneVerifyCodeState = async () => {
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
        url: "/Dashboard/ConfirmPhoneVerifyCodeState",
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
                $("#phoneWizard").steps("next");
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
const ChangeNewPhoneVerify = async (s) => {
    let m = $("#NewPhone").val().replace(/\D/g, '');
    let nm = $("#NewPhoneConfirm").val().replace(/\D/g, '');
    if (m != "") {
        $("#checkNewPhone").prop("disabled", true);
        let obj = {
            Phone: null,
            NewPhone: m,
            NewPhoneConfirm: nm,
            UserVerification: null
        }
        await $.ajax({
            method: "post",
            url: "/Dashboard/ChangeNewPhoneVerify",
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
                    $("#phoneWizard").steps("next");
                    if (s == "n") {
                        StartTimer(timerMunite, timerSecond, s);
                    } else {

                        StartTimer2(5, 0);
                    }
                    notyf.success(response.message);
                    return;
                }
                $("#checkNewPhone").prop("disabled", false);
                notyf.error(response.message);
            },
            error: function (response, statustext, jqxhr) {
                $("#checkNewPhone").prop("disabled", false);
                notyf.error("Doğrulama kodu oluşturulurken hata oluştu tekrar deneyiniz.");
            }
        });
    } else {
        notyf.error("Yeni e-mail adresinizi giriniz.");
    }

}
const CheckNewPhoneVerifyCodeState = async () => {
    let m = $("#NewPhone").val().replace(/\D/g, '');
    let nm = $("#NewPhoneConfirm").val().replace(/\D/g, '');
    let obj = {
        NewPhone: m,
        NewPhoneConfirm: nm,
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
        url: "/Dashboard/UpdatePhone",
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
                $("#NewPhone").text("");
                $("#NewPhoneConfirm").text("");
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
const StartTimer = (minutes, seconds, s) => {
    let totalTime = minutes * 60 + seconds;
    let interval = setInterval(() => {
        var minutes = Math.floor(totalTime / 60);
        var seconds = totalTime % 60;

        var formattedTime = (minutes < 10 ? "0" : "") + minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
        if (s =="n") { 
            $("#timer2").text(formattedTime);
        } else {
            $("#timer").text(formattedTime);
        }

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

$(document).ready(function () {
    $('.phoneInput').on('input', function (e) {
        var x = e.target.value.replace(/\D/g, '').match(/(\d{0,3})(\d{0,3})(\d{0,4})/);
        e.target.value = !x[2] ? x[1] : '(' + x[1] + ') ' + x[2] + (x[3] ? '-' + x[3] : '');
    });
});