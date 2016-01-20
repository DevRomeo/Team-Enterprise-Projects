function onRegisterBegin() {
    $('.process').modal('show');
    //$(".my-modal").modal("show");
}
function onRegisterSuccess(result) {
    if (result.errCode == 1001) {
        var popup = $('.modal-Notification');
        popup.find('.modal-text').html('Add Employee');
        popup.find('.modal-body').html(result.Message);
        popup.modal('show');                        
        window.location = result.action;
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
    } else if (result.errCode == 1000) {
        Message(result.errmessage);
    } else if (result.errCode == 2000) {        
        Message(result.errmessage.toString().split(',').join('<br>'));
    }
}
function Message(msg) {
    $(".process").modal("hide");
    $(".error-container-register").html('<div class="alert alert-danger alert-dismissable" role="alert">' +
        '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button><b>Error: <br>' +
        msg + '</b></div>');
}