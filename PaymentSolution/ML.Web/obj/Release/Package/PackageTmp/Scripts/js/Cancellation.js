$(document).ready(function () {

    $('.cancellationkey').on('keypress keyup blur', AllowedInput);

    $('.forcancellation').on('click', function () {
        var popup = $('.modal-Notification');
        popup.find('.modal-title').html('Cancellation');
        var input = $('.cancellationkey');
        if (input.val().length > 4) {
            $(".process").modal("show");

            $.ajax({
                url: $(this).data('url'),
                datatype: 'json',
                contentTpe: 'application/json',
                type: 'get',
                cache: false,
                async: true,
                data: { data: input.val() },
                success: function (resp) {
                    setTimeout(function () {
                        $(".process").modal("hide");
                        if (resp.code == 1001) {
                            $('.cancellation').html(resp.data);
                            $('.tbl-cancel').footable();
                        }
                        else {
                            popup.find('.modal-body').html(resp.Message + ' for <b>' + resp.search + '</b>');
                            popup.modal('show');
                            input.val('');
                        }
                    }, 1000);
                },
                error: function (e, x, y) {
                    popup.find('.modal-body').html('It appears that your having problem while retrieving transaction information .Please try again later.');
                    popup.modal('show');
                }
            });
        }
    });

    $('.cancellationkey').on('keypress', function (e) {
        if (e.keyCode == 13) {
            $('.forcancellation').click();
        }
    });

    $(document).on('show.bs.modal', '#confirm', function (e) {
        var checked = $('.pick:checked').length;
        $('.tbl-cancel').find('tbody tr').each(function () {
            var row = $(this);
            if (row.find('.pick:checkbox').is(':checked')) {
                if ($.trim(row.find('input:text').val()) != '') {
                    checked--;
                }
            }
        });

        if (checked != 0)
            e.preventDefault();
        var cancelbtn = $(e.relatedTarget);

        $(this).find('.modal-body p').text(cancelbtn.data('message'))
        .end().find('.modal-title').text(cancelbtn.data('title'))
        .end().find('#submit').text(cancelbtn.data('button'))
        .end().find('.modal-footer #submit').data('form', cancelbtn.closest('form'));

    })

    $(document).on('submit', '#cancellationForm', function (e) {
        e.preventDefault();
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            cache: false,
            async: true,
            beforeSend: onCancelRequest,
            success: function (result) {
                onCancellationSuccess(result.Message);
                $('.cancellation').empty();
                $('.cancellationkey').val('');
            },
            error: onCancelFail
        });
    })

    $(document).on('click', '#submit', function () {
        $(this).data('form').submit();
        $('#confirm').modal('hide');
    })

    $(document).on('change', '.pick', function () {
        var txtbox = $(this).closest('tr').find('input:text');
        $('.cancel').prop('disabled', $('.pick:checked').length <= 0);
        if (this.checked) {
            txtbox.removeAttr('readonly');
            txtbox.focus();
        }
        else {
            txtbox.val('');
            txtbox.attr('readonly', 'readonly');
        }
    })
});

function AllowedInput(e) {
    var _event = e || window.event;
    //var key = (e.which) ? _event.keyCode : _event.which;
    var key = typeof _event.which === "undefined" ? _event.keyCode : _event.which;
    var allowchar = new RegExp($(this).data('valid'));
    $(this).val(($(this).val()).toUpperCase());
    return allowchar.test(String.fromCharCode(key));
}

function onCancellationSuccess(result) {
    $(".process").modal("hide");
    var popup = $('.modal-Notification');
    popup.find('.modal-title').html('Cancellation')
    .end().find('.modal-body').html(result)
    .end().modal('show');

}
function onCancelRequest() {
    $(".process").modal("show");
}
function onCancelFail(e, x, y) {
    $(".process").modal("hide");
    var popup = $('.modal-Notification');
    popup.find('.modal-title').html('Cancellation')
    .end().find('.modal-body').html('It appears that your having problem while proccessing cancellation of transactions. Please try again later.')
    .end().modal('show');
}

