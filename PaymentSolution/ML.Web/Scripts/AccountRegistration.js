$(document).ready(function () {

    $('input[data-val-length-max]').each(function () {
        var $this = $(this);
        var data = $this.data();
        $this.prop("maxlength", data.valLengthMax);
    });

    $('.WalletP , .Wallet, #mobileno,#phoneno').on('keyup keypress blur', AllowedInput);

    $('.AuthenticateAccount').on('click', function (e) {

        var popup = $('.modal-Notification');
        popup.find('.modal-text').html('Sign up');

        var progress = $('.process');

        var wallet = $('.Wallet');
        var walletP = $('.WalletP');
        if (wallet.val().length > 0, walletP.val().length > 0) {
            progress.modal("show");
            $.ajax({
                url: $(this).data('url'),
                datatype: 'json',
                contentTpe: 'application/json',
                type: 'POST',
                cache: false,
                async: true,
                data: { wallet: wallet.val(), walletP: walletP.val() },
                success: function (result) {
                    setTimeout(function () {
                        progress.modal("hide");
                        if (result.code == 1001) {
                            location = result.action;
                        }
                        else {
                            popup.find('.modal-body').html(result.Message);
                            popup.modal('show');
                        }
                    }, 2000);
                },
                error: function (e, i, j) {
                    progress.modal("hide");
                    popup.find('.modal-body').html('Whoos theres a problem with wallet information');
                    popup.modal('show');
                }
            });

        }
        else {
            popup.find('.modal-body').html('We may need your ML Wallet Credential for the registration ');
            popup.modal('show');
        }
    })

    $(document).on('change', '#selectedgovidtype', function () {
        var IDType = $('#selectedgovidtype :selected').val();
        var IDNo = $('#govid2');
        var valid;
        var max;
        IDNo.val('');
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
        IDNo.removeAttr('keypress keyup blur');
        IDNo.attr('placeholder', placeholder);
        IDNo.on('keypress keyup blur', AllowedInput);
        IDNo.prop('maxlength', max);
        IDNo.data('valid', valid);
    });

    $('#firstname, #middlename, #lastname, #address, #nationality,#username').on('keypress keyup blur', AllowedInputs);

    $('#Terms').on('click', function () {
        $('#btnregister').prop('disabled', !$(this).is(':checked'));
    });

    $(".btnlogin").click(function () {

        var user = $('#loginname').val();
        var pass = $('#loginpass').val();
        var redirect = $('#RedirectUrl').val();
        $.ajax({
            url: $(this).data('url'),
            datatype: 'json',
            contentTpe: 'application/json',
            type: 'post',
            cache: false,
            async: true,
            data: { username: user, password: pass, redirecturl: redirect },
            success: function (resp) {
                if (resp.code == 1001) {
                    location = resp.action;
                }
                else {
                    $(".wiggle").effect("shake");
                    $(".error").html(resp.Message).show();
                }
            },
            error: function (e, x, y) {
                var popup = $('.modal-Notification');
                popup.html('Sign In');
                popup.find('.modal-body').html('It appears that your having problem during sign in Please try again later.');
                popup.modal('show');
            }
        });
    });

});

function AllowedInput(e) {
    var _event = e || window.event;
    //var key = (e.which) ? _event.keyCode : _event.which;
    var key = typeof _event.which === "undefined" ? _event.keyCode : _event.which;
    var allowchar = new RegExp($(this).data('valid'));

    return allowchar.test(String.fromCharCode(key));
}

function AllowedInputs(e) {
    var _event = e || window.event;
    //var key = (e.which) ? _event.keyCode : _event.which;
    var key = typeof _event.which === "undefined" ? _event.keyCode : _event.which;
    var allowchar = new RegExp($(this).data('val-regex-pattern'));
    return allowchar.test(String.fromCharCode(key));
}

function onRegisterBegin() {
    $('.error-container-register').find('.close').click();
    $(".process").modal("show");
}

function onRegisterSuccess(result) {
    $(".process").modal("hide");
    switch (result.code) {
        case 1001:
            var popup = $('.modal-Notification');
            popup.find('.modal-text').html('Sign up');
            popup.find('.modal-body').html(result.Message);
            popup.modal('show');
            $('.modal-Notification').on('hidden.bs.modal', function () {
                location = result.action;
            });
            break;
        case 2000:
            DisplayMessage(result.Message.toString().split(',').join('<br>'));
            break;
        default:
            DisplayMessage(result.Message);
            break;
    }
}

function onFail() {
    $(".process").modal("hide");
    var popup = $('.modal-Notification');
    popup.find('.modal-text').html('Sign up');
    popup.find('.modal-body').html('It appears that there was Problem during Registration Please try again later');
    popup.modal('show');
}

function DisplayMessage(msg) {
    $(".error-container-register").html('<div class="alert alert-danger alert-dismissable" role="alert">' +
        '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button><b>Error: <br>' +
        msg + '</b></div>');
}