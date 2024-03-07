$(document).ready(() => {
    $('.password-eye').on('click', function (e){
        const input = $(e.target).siblings('input')
        if (input.attr("type") === "password"){
            input.attr('type', 'text')
            $(e.target).removeClass('fa-eye').addClass('fa-eye-slash')
        }else{
            input.attr('type', 'password')
            $(e.target).removeClass('fa-eye-slash').addClass('fa-eye')
        }
    })
    
    $('form').on('submit', function (e){
        const form = $(this)
        let validator = form.validate();
        validator.form();
        if (validator.valid()){
            form.find('button[type="submit"]').attr('disabled', 'true')
        }
    })
})