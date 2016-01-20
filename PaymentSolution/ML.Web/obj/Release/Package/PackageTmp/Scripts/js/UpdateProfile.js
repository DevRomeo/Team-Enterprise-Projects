$(document).ready(function () {

    $('input[data-val-length-max]').each(function () {
        var $this = $(this);
        var data = $this.data();
        $this.prop("maxlength", data.valLengthMax);
    });

    $(document).on('change', '#selectedgovidtype', function () {
        $('#govid2').val('');
        GovernmentId();
    });

    $('#phoneno, #mobileno').on('keypress keyup blur', AllowedInput);
    $('#firstname, #middlename, #lastname, #address, #nationality,#username').on('keypress keyup blur', AllowedInputs);
    
    GovernmentId();
});

function GovernmentId() {
    var IDType = $('#selectedgovidtype :selected').val();
    var IDNo = $('#govid2');
    var valid;
    var max;   
    var placeholder = '';
    switch (IDType) {
        case "GSIS":
            placeholder = '12345678901';
            valid = "^([0-9])$";
            max = 11;
            break;
        case "SSS":
            placeholder = '12-1234567-1';
            valid = "^([0-9\-\/])$";
            max = 12;
            break;
        case "UMID":
            placeholder = '1234-1234567-1';
            valid = "^([0-9\-\/])$";
            max = 14;
            break;
        case "TAX IDENTIFICATION ID":
            placeholder = '123-456-789-000';
            valid = "^([0-9\-\/])$";
            max = 15;
            break;
    }
    //IDNo.val('');
    IDNo.removeAttr('keypress keyup blur');
    IDNo.attr('placeholder', placeholder);
    IDNo.on('keypress keyup blur', AllowedInput);
    IDNo.prop('maxlength', max);
    IDNo.data('valid', valid);
};

function AllowedInput(e) {
    var _event = e || window.event;
    //var key = (e.which) ? _event.keyCode : _event.which;
    var key = typeof _event.which === "undefined" ? _event.keyCode : _event.which;
    var allowchar = new RegExp($(this).data('valid'));
    //console.log($(this).val());
    return allowchar.test(String.fromCharCode(key));
}

function AllowedInputs(e) {
    var _event = e || window.event;
    //var key = (e.which) || (e.keyCode) ? _event.keyCode : _event.which;
    var key = typeof _event.which === "undefined" ? _event.keyCode : _event.which;
    var allowchar = new RegExp($(this).data('val-regex-pattern'));
    return allowchar.test(String.fromCharCode(key));
}

function onUpdateBegin() {
    $('.error-container-profile').find('.close').click();
    $(".process").modal("show");
}

function onUpdateSuccess(result) {
    setTimeout(function () {
        $(".process").modal("hide");
        switch (result.code) {
            case 1001:
                var popup = $('.modal-Notification');
                popup.find('.modal-text').html('Profile');
                popup.find('.modal-body').html(result.Message);
                popup.modal('show');
                return;
            case 1005, 1006, 1007, 1008:
                DisplayMessage(result.Message);
                break;
            case 2000:
                DisplayMessage(result.Message.toString().split(',').join('<br>'));
                break;
            default:
                DisplayMessage(result.Message);
                break;
        }
    }, 1000);
}

function onUpdateFail() {
    setTimeout(function () {
        $(".process").modal("hide");
        var popup = $('.modal-Notification');
        popup.find('.modal-text').html('Profile');
        popup.find('.modal-body').html('It appears that there was Problem during updating profile Please try again later');
        popup.modal('show');
    }, 1000);
}

function DisplayMessage(msg) {
    $(".error-container-profile").html('<div class="alert alert-danger alert-dismissable" role="alert">' +
        '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button><b>' + msg + '</b></div>');
}