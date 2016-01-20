$(document).ready(function () {

    var loading = $('.loading');

    $('.unprocess-entries').html(loading);

    $('.search').on('click', function () {        
        $('.unprocess-entries').html(loading);
        $.ajax({
            url: $(this).data('url'),
            datatype: 'json',
            contentTpe: 'application/json',
            type: 'GET',
            data: { dt: $('#search-unprocess').val() },
            async: true,
            success: function (result) {
                setTimeout(function () {
                    $('.unprocess-entries').html(result);
                }, 500);

            },
            error: function (e, x, y) {                
                console.log(e);
                console.log(x);
            }
        });
    });

    $('.search').trigger('click');

    $(document).on('change', '.Process', function () {
        $('.process-entry').prop('disabled', $('.Process:checked').length <= 0);
    });

    $(document).on('show.bs.modal', '.confirm', function (e) {
        var cancelbtn = $(e.relatedTarget);

        $(this).find('.modal-body p').text(cancelbtn.data('message'))
        .end().find('.modal-title').text(cancelbtn.data('title'))
        .end().find('.submit').text(cancelbtn.data('button'))
        .end().find('.modal-footer .submit').data('form', cancelbtn.closest('form'));

    });

    $(document).on('show.bs.modal', '.modal-delete', function (e) {
        var deleteEntry = $(e.relatedTarget);

        $(this).find('.modal-body p').text(deleteEntry.data('message'))
        .end().find('.modal-title').text(deleteEntry.data('title'))
        .end().find('.confirm-delete-entry').data('url', deleteEntry.data('url'))
        .end().find('.confirm-delete-entry').data('corpname', deleteEntry.data('corpname'))
        .end().find('.confirm-delete-entry').data('entry-number', deleteEntry.data('entry-number'))
    });

    $(document).on('click', '.confirm-delete-entry', function () {
        $.ajax({
            url: $(this).data('url'),
            type: 'get',
            data: { entryNo: $(this).data('entry-number'), corpName: $(this).data('corpname') },
            cache: false,
            async: true,
            success: function (e) {
                $('.modal-delete').modal('hide');
                var msg = $('.modal-notification');
                if (e.code == 1) {
                    $('.modal-notification').on('hidden.bs.modal', function () {
                        location = e.action;
                    });
                }
                msg.find('.modal-body').text(e.msg);
                msg.modal('show');
            },
            error: function (e, x, y) {
                console.log(e);
                console.log(x);
            }
        });
    });

    $(document).on('click', '.submit', function () {
        $(this).data('form').submit();
        $('.confirm').modal('hide');
    });

    $('.date').on('click', function () {
        $('#search-unprocess').datepicker('show');
    });
    $('#search-unprocess').datepicker({
        format: 'yyyy-mm-dd'
    });

});

function BeginProcess() {
    //alert('Begin Process');
}
function processComplete(e) {
    var msg = $('.modal-notification');
    if (e.responseJSON.code == 1) {        
        $('.modal-notification').on('hidden.bs.modal', function () {
            location = e.responseJSON.action;
        });        
    }
    msg.find('.modal-body').text(e.responseJSON.msg);
    msg.modal('show');
}