//$(document).ajaxError(function (event, request, options) {
//    if (request.status == 401) {
//        //alert('authentication Expired');        
//        location = $('.log-out').data('url');
//    }
//});

$(document).ready(function () {
    $('input[data-val-length-max]').each(function () {
        var $this = $(this);
        var data = $this.data();
        $this.prop("maxlength", data.valLengthMax);
    });
    alert('test');
    $(document).on('keypress keyup blur', '.entry-gl-description,.entry-gl', AllowedInput);
});
function AllowedInput(e) {
    var _event = e || window.event;
    var key = (e.which) ? _event.keyCode : _event.which;
    var allowchar = new RegExp($(this).data('val-regex'));    
    return allowchar.test(String.fromCharCode(key));
}