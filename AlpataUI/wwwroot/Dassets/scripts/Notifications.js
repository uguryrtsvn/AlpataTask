 
const obj = {
    memberType: "",
    PhoneNotificationState: false,
    PhoneNumberConfirmed: false,
    PhoneNotificationChangeTime: null,
    EmailNotificationState: true,
    EmailNotificationChangeTime: null
};
//const notifications = {
//    init: (memberType, phoneNotificationState, phoneNumberConfirmed, phoneNotificationChangeTime, emailNotificationState, emailNotificationChangeTime) => {
//        return {
//            memberType: memberType,
//            PhoneNotificationState: phoneNotificationState,
//            PhoneNumberConfirmed: phoneNumberConfirmed,
//            PhoneNotificationChangeTime: phoneNotificationChangeTime,
//            EmailNotificationState: emailNotificationState,
//            EmailNotificationChangeTime: emailNotificationChangeTime
//        };
//    }
//};
let now = new Date();
 
$("#emailNotification").change(async () => {   
    $("#emailNotification").prop('disabled', true);
    if ((Math.abs(now - obj.EmailNotificationChangeTime)) < (24 * 60 * 60 * 1000)) {
        notyf.error("Bildirim ayarlarını 24 saat içinde sadece bir kez değiştirebilirsiniz.");
        $("#emailNotification").prop('checked', obj.EmailNotificationState);
        return;
    } 
    obj.EmailNotificationState = $("#emailNotification").prop("checked");
    obj.EmailNotificationChangeTime = new Date();
    await $.ajax({
        method: "post",
        url: "/Dashboard/ChangeEmailNotificationState",
        datatype: 'json',
        data: obj,
        contenttype: 'application/json; charset=utf-8',
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,
        success: function (response, statustext, jqxhr) {
            if (response.success) {  
                notyf.success(response.message);
                return;
            }
            obj.EmailNotificationState = !obj.EmailNotificationState
            $("#emailNotification").prop('checked', obj.EmailNotificationState);
            notyf.error(response.message);
        },
        error: function (response, statustext, jqxhr) {
            $("#changePhone").prop("disabled", false);
            notyf.error("Doğrulama kodu oluşturulurken hata oluştu tekrar deneyiniz.");
            $("#emailNotification").prop('disabled', false);
        }
    });
});
 

$("#PhoneNotification").change(async () => { 
    $("#PhoneNotification").prop('disabled', true);
    if (obj.memberType=="Default") {
        notyf.error("SMS bildirimlerinden yalnızca premium üyeler faydalanabilir.");
        $("#PhoneNotification").prop('checked', false);
        return;
    }
    if (obj.PhoneNumberConfirmed == false) {
        notyf.error("SMS bildirimlerini kullanmak için önce telefon numaranızı eklemelisiniz.");
        $("#PhoneNotification").prop('checked', false);
        return;
    }
    if ((Math.abs(now - obj.PhoneNotificationChangeTime)) < (24 * 60 * 60 * 1000)) {
        notyf.error("Bildirim ayarlarını 24 saat içinde sadece bir kez değiştirebilirsiniz.");
        $("#PhoneNotification").prop('checked', obj.PhoneNotificationState);
        return;
    }
    obj.PhoneNotificationState = $("#PhoneNotification").prop("checked");
    obj.PhoneNotificationChangeTime = new Date();
    await $.ajax({
        method: "post",
        url: "/Dashboard/ChangePhoneNotificationState",
        datatype: 'json',
        data: obj,
        contenttype: 'application/json; charset=utf-8',
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,
        success: function (response, statustext, jqxhr) {
            if (response.success) {
                notyf.success(response.message);
                $("#PhoneNotification").prop('disabled', true);
                return;
            }
            obj.EmailNotificationState = !obj.EmailNotificationState
            $("#PhoneNotification").prop('checked', obj.EmailNotificationState);
            notyf.error(response.message);
        },
        error: function (response, statustext, jqxhr) {
            obj.EmailNotificationState = !obj.EmailNotificationState
            $("#PhoneNotification").prop('checked', obj.EmailNotificationState);
            notyf.error("Değişiklik sırasında hata oluştu. Lütfen tekrar deneyiniz.");
            $("#PhoneNotification").prop('disabled', false);
        }
    });
});