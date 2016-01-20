$(document).ready(function () {
    //Retrieve GL from GLAccounts
    $('.gl-setting').on('show.bs.modal', function (e) {
        var $this = $(e.relatedTarget);
        $.ajax({
            url: $this.data('url'),
            datatype: 'json',
            contentTpe: 'application/json',
            type: 'GET',
            async: true,
            success: function (result) {
                setTimeout(function () {
                    $('.load-gl').html(result);
                }, 500);

                //if ($this.data('option') == 1) {
                //    $('.load-gl').html(result);
                //}
                //else if ($this.data('option') == 2) {

                //    $('.load-gl').html(result.page);                    
                //    $('.gl-categories').data('general-ledgers',result.categorygl);
                //}
            },
            error: function (e, x, y) {
                console.log(e);
                console.log(x);
            }
        });
    })



    $('.gl-setting').on('hide.bs.modal', function () {
        $('.load-gl').html('<img src="' + $(this).data('img') + '" /> Loading item . . .');
    })
    //Dropdown on removing current Category GLs on ML Accounting
    $(document).on('change', '.gl-categories', function () {
        var categoryItem = this.value;
        var $rows = $('.general-ledger-adding tbody tr');
        if (categoryItem != '') {
            $rows.show().filter(function () {
                //if ($(this).children('input[type="hidden"]:eq(3)').val() == categoryItem) {
                //    console.log($(this).children('input[type="hidden"]:eq(2)').val() + "   " + $(this).children('input[type="hidden"]:eq(3)').val());
                //    return false;
                //} else {
                //    return true;
                //}
                return $(this).children('input[type="hidden"]:eq(3)').val() != categoryItem;

                //return !~$(this).children('input[type="hidden"]:eq(3)').val().indexOf(categoryItem);
            }).hide();
            $('.select-update').prop('disabled', '');
        }
        else {
            $rows.show();
            $('.select-update,.update-category').prop('disabled', 'disabled');
            $('.select-update').prop('checked', false);
        }
    });

    $(document).on('change', '.select-update', function () {
        $('.update-category').prop('disabled', $('.select-update:checked').length <= 0);
    });
    //Dropdown on adding GL From synergy to ML Accounting System
    $(document).on('change', '.category', function () {

        if (this.value != '') {
            $('.search-ledger,.search-description,.select-add').prop('disabled', '');
        }
        else {
            $('.search-ledger,.search-description,.select-add,.submit-gl-entry').prop('disabled', 'disabled');
            $('.select-add').prop('checked', false);
            $('.search-ledger,.search-description').val('');
        }
    });

    //Search By GL code
    $(document).on('keyup keypress blur', '.search-ledger', function () {
        var $rows = $('.general-ledger-adding tbody tr');
        if ($(this).val().length > 4) {
            //var $rows = $('.general-ledger-adding tbody tr');
            var key = $.trim($(this).val()).replace(/ +/g, ' ').toLowerCase();
            $rows.show().filter(function () {
                return !~$(this).children(':eq(1)').text().toLowerCase().indexOf(key);
            }).hide();
        }
        if ($(this).val().length == 0) {
            $rows.show();
        }

    });
    //Search By GL Description
    $(document).on('keyup keypress blur', '.search-description', function () {
        var $rows = $('.general-ledger-adding tbody tr');
        if ($(this).val().length > 4) {

            var key = $.trim($(this).val()).replace(/ +/g, ' ').toLowerCase();

            $rows.show().filter(function () {
                return !~$(this).children(':eq(2)').text().toLowerCase().indexOf(key);
            }).hide();
        }
        if ($(this).val().length == 0) {
            $rows.show();
        }
    });

    //Enable submit button
    $(document).on('change', '.select-add', function () {
        $('.submit-gl-entry,.update-category').prop('disabled', $('.select-add:checked').length <= 0);
    });

    $(document).on('change', '.create-gl-category', function () {
        if (this.value != '')
            $('.create-gl').prop('disabled', '');
        else
            $('.create-gl').prop('disabled', 'disabled');
    });

    $(document).ajaxError(function (event, request, options) {
        if (request.status == 401) {
            location = $('.log-out').data('url');
        }
    });

    $(document).on('keypress keyup blur', '.entry-gl-description,.entry-gl', AllowedInput);
});

function AllowedInput(e) {
    var _event = e || window.event;
    //var key = (e.which) ? _event.keyCode : _event.which;
    var allowchar = new RegExp($(this).data('val-regex-pattern'));
    var key = typeof _event.which === "undefined" ? _event.keyCode : _event.which;
    return allowchar.test(String.fromCharCode(key));
}

function beginAddGl() {
    //alert('begin');
}
function addComplete(result) {
    var msg = $('.modal-notification');
    msg.find('.modal-body').text(result.responseJSON.msg);
    if (result.responseJSON.rcode == 1) {
        $('.gl-setting').modal('hide');
    }
    $('.modal-notification').modal('show');
}
function UpdateComplete(result) {
    var msg = $('.modal-notification');
    msg.find('.modal-body').text(result.responseJSON.msg);
    if (result.responseJSON.rcode == 1) {
        $('.gl-setting').modal('hide');
    }
    $('.modal-notification').modal('show');
}
function createGl(result) {
    var msg = $('.modal-notification');
    msg.find('.modal-body').text(result.responseJSON.msg);
    if (result.responseJSON.rcode == 1) {
        $('.gl-setting').modal('hide');
    }
    $('.modal-notification').modal('show');
}
