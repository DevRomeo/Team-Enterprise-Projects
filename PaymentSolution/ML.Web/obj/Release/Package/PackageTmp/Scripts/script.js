$(document).ready(function () {
    $(".reportHistory").on('click', function () {

        $.ajax({
            url: $(this).data('url'),
            datatype: 'json',
            contentTpe: 'application/json',
            type: 'GET',
            cache: false,
            async: true,
            success: function (result) {
                $(".reports").html(result);
            },
            error: function (e, x, d) {
                console.log(e);
                console.log(x.status);
                console.log(d);
            }
        });
    });
});

$(document).ajaxError(function (event, request, options) {
    if (request.status == 401) {
        location = $('.btn-settings').data('url');
    }
});