function onRegisterBegin() {
    $(".process").modal("show");
}

function onUpdateSuccess(result) {
    $(".process").modal("hide");
    if (result.errCode == 1001) {
        var popup = $('.modal-Notification');
        popup.find('.modal-text').html('Employee');
        popup.find('.modal-body').html('Employee Information have been Updated');
        popup.modal('show');        
    } else if (result.errCode == 1002) {
        Message(result.errmessage);        
    } else if (result.errCode == 1003) {
        Message(result.errmessage);
    } else if (result.errCode == 1004) {
        Message(result.errmessage);
    } else if (result.errCode == 1005) {
        Message(result.errmessage);
    } else if (result.errCode == 1006) {
        Message(result.errmessage);
    } else if (result.errCode == 2000) {        
        Message(result.errmessage.toString().split(',').join('<br>'));
    }

}

function onUpdateFail() {
    $(".process").modal("hide");
    var popup = $('.modal-Notification');
    popup.find('.modal-text').html('Employee');
    popup.find('.modal-body').html('There was a problem upon updating this Employee');
    popup.modal('show');    
}

function Message(msg) {    
    $(".error-container-register").html('<div class="alert alert-danger alert-dismissable" role="alert">' +
        '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button><b>Error: <br>' +
        msg + '</b></div>');
}