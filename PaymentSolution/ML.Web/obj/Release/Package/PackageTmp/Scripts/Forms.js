
$.validator.setDefaults({
    highlight: function (element, errorClass, validClass) {

        $(element).addClass(errorClass).removeClass(validClass);
        $(element).closest('.form-group').removeClass('has-success').addClass('has-error');

    },
    unhighlight: function (element, errorClass, validClass) {

        $(element).removeClass(errorClass).addClass(validClass);
        $(element).closest('.form-group').removeClass('has-error').addClass('has-success');

    }
});


$('span.field-validation-valid, span.field-validation-error').each(function () {
    $(this).addClass('help-block');
});
