$(document).ready(function () {

    $('input[data-val-length-max]').each(function () {
        var $this = $(this);
        var data = $this.data();
        $this.prop("maxlength", data.valLengthMax);
    });

    $('.searchEmployee').on('click', function () {
        var $rows = $('.Employees tbody tr');//.next();
        var val = $.trim($('.employeeKey').val()).replace(/ +/g, ' ').toLowerCase();

        $rows.show().filter(function () {
            var text = $(this).text().replace(/\s+/g, ' ').toLowerCase();
            return !~text.indexOf(val);
        }).hide();
    });

    $('.employeeKey').on('keyup', function (e) {

        if (e.keyCode == 13) {
            $('.searchEmployee').click();
            return;
        }
        if ($(this).val().length == 0) {
            $('.Employees tbody tr').show();
        }
        //var data = $.trim($('.employeeKey').val());
        //if (data == "") {
        //    var keyWord = data.replace(/ +/g, ' ').toLowerCase();
        //    var $rows = $('.Employees tr').next();

        //    $rows.show().filter(function () {
        //        var text = $(this).text().replace(/\s+/g, ' ').toLowerCase();
        //        return !~text.indexOf(keyWord);
        //    }).hide();
        //}
    });

    $(document).on('change', '#selectedgovidtype', function () {
        $('#govid2').val('');
        GovernmentId();
    });

    $('#phoneno,#mobileno').on('keypress keyup blur', AllowedInput);
    $('#firstname,#middlename,#lastname,#address,#nationality').on('keypress keyup blur', AllowedInputs);

    GovernmentId();
});

function GovernmentId() {
    var IDType = $('#selectedgovidtype :selected').val();
    var IDNo = $('#govid2');
    var valid;
    var pattern;
    var max;
    var placeholder = '';
    switch (IDType) {
        case "GSIS":
            placeholder = '12345678901';
            max = 11;
            valid = "^([0-9])$";
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
    IDNo.data('valid', valid);
    IDNo.on('keypress keyup blur', AllowedInput);
    IDNo.prop('maxlength', max);
}

function AllowedInput(e) {
    var _event = e || window.event;
    //var key = (e.which) ? _event.keyCode : _event.which;
    var key = typeof _event.which === "undefined" ? _event.keyCode : _event.which;
    var allowchar = new RegExp($(this).data('valid'));

    return allowchar.test(String.fromCharCode(key));
}

function AllowedInputs(e) {
    var _event = e || window.event;
    var key = (e.which) ? _event.keyCode : _event.which;
    //var allowchar = new RegExp($(this).data('val-regex-pattern'));
    var key = typeof _event.which === "undefined" ? _event.keyCode : _event.which;
    return allowchar.test(String.fromCharCode(key));
}

function onRequestBegin() {
    $(".process").modal("show");
    $('.btncreateemployee').prop('disabled', true);
}

function onRegisterEmployeeSuccess(result) {
    $(".process").modal("hide");
    var container = ".error-container-create";
    if (result.code == 1001) {
        var popup = $('.modal-Notification');
        popup.find('.modal-text').html('Add Employee');
        popup.find('.modal-body').html(result.Message);
        popup.modal('show');
        popup.on('hidden.bs.modal', function () {
            window.location = result.action;
        })
    }
    else {
        Message(container, result.Message);
    }
    //switch (result.code) {        
    //    case 1001:
            
    //        break;
    //    case 1000, 1002, 1003, 1004, 1005, 1006:
            
    //        break;
    //    case 2000:
    //        Message(container, result.Message.toString().split(',').join('<br>'));
    //        break;
    //    default:
    //        Message(container, result.Message);
    //        break;
    //}
}

function onRegisterFail() {
    $('.btncreateemployee').prop('disabled', true);
    $(".process").modal("hide");
    var popup = $('.modal-Notification');
    popup.find('.modal-text').html('Employee');
    popup.find('.modal-body').html('There was a problem upon Creating this Employee');
    popup.modal('show');
}

function onUpdateSuccess(result) {
    $(".process").modal("hide");
    var container = ".error-container-edit";
    if (result.code == 1001)
    {
        var popup = $('.modal-Notification');
        popup.find('.modal-text').html('Employee');
        popup.find('.modal-body').html(result.Message);
        popup.modal('show');
        popup.on('hidden.bs.modal', function () {
            window.location = result.action;
        })        
    }
    else {
        Message(container, result.Message);
    }
    //switch (parseInt(result.code)) {       
    //    case 1001:            
    //    case 1000, 1002, 1003, 1004, 1005, 1006:
            
    //        break;
    //    case 2000:
    //        Message(container, result.Message.toString().split(',').join('<br>'));
    //        break;
    //    default:
    //        Message(container, result.Message);
    //        break;
    //}
}

function onUpdateFail() {
    $(".process").modal("hide");
    var popup = $('.modal-Notification');
    popup.find('.modal-text').html('Employee');
    popup.find('.modal-body').html('There was a problem upon updating this Employee');
    popup.modal('show');
}

function Message(container, msg) {
    $(container).html('<div class="alert alert-danger alert-dismissable" role="alert">' +
        '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button><b>Error: <br>' +
        msg + '</b></div>');
}
