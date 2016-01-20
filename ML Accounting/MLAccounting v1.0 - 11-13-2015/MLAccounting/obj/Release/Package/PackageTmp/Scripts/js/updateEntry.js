$(document).ready(function () {
    $(document).keypress(function (e) {
        if (e.which == 13) {
            return false;
        }
    });

    $('input[data-val-length-max]').each(function () {
        var $this = $(this);
        var data = $this.data();
        $this.prop("maxlength", data.valLengthMax);
    });
    $(document).on('change', '.Debit', function () {
        var debit = $(this);
        if (debit.val().trim() != 0) {
            $(this).closest('tr').find('td:eq(3) input:text').prop('disabled', 'disabled');
        } else {
            $(this).closest('tr').find('td:eq(3) input:text').prop('disabled', '');
            debit.val('0');
        }
        $('.total-debit').html(sumItUp('.Debit'));
    });
    $(document).on('change', '.Credit', function () {
        var credit = $(this);
        if (credit.val().trim() != 0) {
            $(this).closest('tr').find('td:eq(2) input:text').prop('disabled', 'disabled');
        } else {
            $(this).closest('tr').find('td:eq(2) input:text').prop('disabled', '');
            credit.val('0');
        }
        $('.total-credit').html(sumItUp('.Credit'));
    });

    $(document).on('click', '.balance-sheet,.income-statement,.trial-balance', function () {
        var form = $('#dataentryForm');
        var oldpath = form.prop('action');
        $('.pick').prop('disabled', '');
        form.prop('action', $(this).data('url'));
        form.attr('data-ajax', "false");
        form.submit();
        form.prop('action', oldpath);
        form.attr('data-ajax', "true");
        $('.pick').prop('disabled', 'disabled');
    });

    $(document).on('keypress keyup blur', '.Debit,.Credit', AllowedInput);

    $('.Credit, .Debit').trigger('change');
})
function updateComplete(e) {
    var msg = $('.modal-notification');
    msg.find(".header").text("Data Entry");
    if (e.responseJSON.code == 1) {
        //alert(e.responseJSON.msg);                
        //location = e.responseJSON.action;
        $('.modal-notification').on('hidden.bs.modal', function () {
            location = e.responseJSON.action;
        });
    }
    msg.find('.modal-body').text(e.responseJSON.msg);
    msg.modal('show');
}
function sumItUp(field) {
    var sum = 0;
    $(field).each(function () {
        if (!isNaN(this.value) && this.value.length != 0) {
            sum += parseFloat(this.value);
        }
    });
    return sum.toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
function AllowedInput(e) {
    var _event = e || window.event;
    var key = (e.which) ? _event.keyCode : _event.which;
    var allowchar = new RegExp($(this).data('val-regex'));

    return allowchar.test(String.fromCharCode(key));
}