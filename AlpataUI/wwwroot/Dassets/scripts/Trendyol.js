

const OpenProductPart = () => {
    $("#productPart").addClass("show");
    $('html, body').animate({
        scrollTop: $("#productPart").offset().top
    }, 1000);
}
const OpenSellerPart = () => { 
    $("#sellerPart").css("display","block");
    $("#sellerPart").addClass("show");
    $('html, body').animate({
        scrollTop: $("#sellerPart").offset().top
    }, 1000);
}

let memberType = null;
 
let product = null;
$(document).ready(function () {
    $(".premium").click(() => {   
        if (memberType == 'Default') { 
            notyf.error("Bu özelliği sadece premium kullanıcılar kullanabilir.");
        }
    });
    $('#frequency option:disabled').on('click', function () {
        if (memberType == 'Default') { 
            notyf.error("Bu özelliği sadece premium kullanıcılar kullanabilir.");
        }
    });
    $("#tyButton").on("click", async () => { 
        $(this).prop("disabled", true); 
        $("#spinnerIcon").show();
        $("#buttonText").text("Yükleniyor...");

        let obj = {
            link: $("#tyUrl").val()
        }
        await $.ajax({
            method: "post",
            url: "/Markets/GetTyProduct",
            async: true,
            datatype: 'json',
            data: obj,
            contenttype: 'application/json; charset=utf-8',
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response, statustext, jqxhr) {
                $("#spinnerIcon").hide();
                $("#buttonText").text("Ürünü Getir");
                $("#tyButton").prop("disabled", false);
                if (response.success) { 
                    product = response.data;
                    $("#newProduct").html(`<div class="card">
                                <div class="card-body  text-center">
                                    <img src="${product.imageUrl}" alt="" class="rounded-circle d-block mx-auto mt-2" height="70">
                                    <h4 class="m-0 font-weight-semibold text-dark font-16 mt-3">${product.brandName} ${product.name}</h4>
                                    <p class="text-muted  mb-0 font-13"><span class="text-dark">Satıcı Adı: </span>${product.sellerName}</p>
                                    <div class="mt-3">
                                        <h5 class="font-24 m-0 font-weight-bold">${product.sellingPrice}₺</h5>
                                        <p class="mb-0 text-muted">${product.category}</p>
                                    </div> 
                                </div>
                            </div>`);
                    notyf.success("Ürün bulundu.");
                    return;
                }
                $("#checkNewPhone").prop("disabled", false);
                notyf.error(response.message);
            },
            error: function (response, statustext, jqxhr) {
                $("#spinnerIcon").hide();
                $("#buttonText").text("Ürünü Getir");
                $("#tyButton").prop("disabled", false);
                notyf.error("Ürün bulunurken bir hata oluştu. Lütfen tekrar deneyin.");
            }
        });

         
    });
    $("#acceptSeller").on("click", async () => {
        $("#acceptSeller").prop("disabled", true);
        let obj = {
            "ApiKey": null,
            "ApiSecret": null,
            "SellerId": product.sellerId,
            "SellerName": product.sellerName,
            "Token": null,
            "UserId": null,
            "Markets": 2
        }
        await $.ajax({
            method: "post",
            url: "/Markets/GetTyProduct",
            async: true,
            datatype: 'json',
            data: obj,
            contenttype: 'application/json; charset=utf-8',
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response, statustext, jqxhr) {
                $("#spinnerIcon").hide();
                $("#buttonText").text("Mağazamı Getir");
                $("#tySellerButton").prop("disabled", false);

                if (response.success) {
                    product = response.data; 
                    notyf.success(response.message);
                    $("#sellerPart").removeClass("show");
                    $("#sellerPart").css("display", "none"); 
                    return;
                }
                $("#checkNewPhone").prop("disabled", false);
                $("#tySellerButton").prop("disabled", false);
                notyf.error(response.message);
            },
            error: function (response, statustext, jqxhr) {
                $("#spinnerIcon").hide();
                $("#buttonText").text("Mağazamı Getir");
                $("#tySellerButton").prop("disabled", false);
                notyf.error("Mağaza bulunurken bir hata oluştu. Lütfen tekrar deneyin.");
            }
        });
    });
    $("#tySellerButton").on("click", async () => {
        $("#tySellerButton").prop("disabled", true);
        $("#spinnerIcon").show();
        $("#buttonText").text("Yükleniyor...");

        let obj = {
            link: $("#tySellerUrl").val()
        }
        await $.ajax({
            method: "post",
            url: "/Markets/GetTyProduct",
            async: true,
            datatype: 'json',
            data: obj,
            contenttype: 'application/json; charset=utf-8',
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response, statustext, jqxhr) {
                $("#spinnerIcon").hide();
                $("#buttonText").text("Mağazamı Getir");
                $("#tySellerButton").prop("disabled", false);
            
                if (response.success) {
                    product = response.data;
                    $("#newSeller").html(`<div class="card">
                                <div class="card-body  text-center">
                                    <img src="${product.imageUrl}" alt="" class="rounded-circle d-block mx-auto mt-2" height="70">
                                    <h4 class="m-0 font-weight-semibold text-dark font-16 mt-3">Satıcı Adı: ${product.sellerName}</h4>
                                    <p class="text-muted  mb-0 font-13"><span class="text-dark">Satıcı Id: </span>${product.sellerId}</p>
                                    <div class="mt-3">
                                        <h5 class="font-24 m-0 font-weight-bold">${product.sellingPrice}₺</h5>
                                        <p class="mb-0 text-muted">${product.brandName} ${product.name}</p>
                                    </div> 
                                    <button id="sellerLogic" onclick="NextStep()" class="btn btn-primary btn-block waves-effect waves-light mt-2" type="button"> Mağazayı Doğrula </button>
                                </div>
                            </div>`);
                    notyf.success("Mağaza bulundu.");
                    $("#tySellerButton").prop("disabled", false);
                    return;
                }
                $("#checkNewPhone").prop("disabled", false);
                $("#tySellerButton").prop("disabled", false);
                notyf.error(response.message);
            },
            error: function (response, statustext, jqxhr) {
                $("#spinnerIcon").hide();
                $("#buttonText").text("Mağazamı Getir");
                $("#tySellerButton").prop("disabled", false);
                notyf.error("Mağaza bulunurken bir hata oluştu. Lütfen tekrar deneyin.");
            }
        });


    });
});
const ConfirmSeller = async () => {
    let obj = {
        code: $("#code").val(),
        t:9//tokenType
    }
    if (obj.code == "" || obj.code == null || obj.code.length != 6) {
        notifyError("Lütfen doğru kodu giriniz.");

        return;
    }
    $("#codeConfirm").prop("disabled", true);
    await $.ajax({
        method: "get",
        url: "/Markets/ConfirmVerifyCodeState",
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
                $("#sellerForm").steps("next");
                notyf.success(response.message); 
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
};

const NextStep = async () => { 
    $("#buttonText").prop("disabled", true);
    await $.ajax({
        method: "get",
        url: "/Markets/CreateSellerVerfy",
        async: true,
        datatype: 'json', 
        contenttype: 'application/json; charset=utf-8',
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,
        success: function (response, statustext, jqxhr) {
 
            if (response.success) {
                $("#sellerForm").steps("next");
                StartTimer(5, 0);
                notyf.success(response.message);
                return;
            }
            $("#buttonText").prop("disabled", false);
            notyf.error(response.message);
        },
        error: function (response, statustext, jqxhr) {
            $("#buttonText").prop("disabled", false);
            notyf.error("Doğrulama kodu oluşturulurken hata oluştu tekrar deneyiniz.");
        }
    });
};
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
 