$(document).ready(function () {

    $('#btnlogout').on('click', function (e) {
        $.ajax({
            url: $(this).data('url'),
            datatype: 'json',
            contentTpe: 'application/json',
            type: 'POST',
            cache: false,
            async: true,
            data: { },
            success: function (result) {                
                location = result.action;
            },
            error: function (e, x, y) {
                location = $(this).data('url');
            }
        });
    });

    $('.InvoiceKey').on('keyup', function (e) {
        if (e.keyCode == 13) {
            $('.SearchInvoice').click();
        }
    })

    $('.SearchInvoice').on('click', function () {
        var $rows = $('.Invoices tbody tr');

        var value = $.trim($('.InvoiceKey').val()).replace(/ +/g, ' ').toLowerCase();
        $rows.show().filter(function () {
            return !~$(this).children(':eq(0)').text().indexOf(value);
        }).hide();

        //$.ajax({
        //    url: '/',
        //    datatype: 'json',
        //    contentTpe: 'application/json',
        //    type: 'GET',
        //    cache: false,
        //    async: true,
        //    data: { searchString: $('.InvoiceKey').val() },
        //    success: function (result) {
        //        $(".indexPartial").html(result);                
        //    },
        //    error: function (e) {
        //        alert("error");
        //    }
        //});

    });

    $('.InvoiceKey').on('keypress keyup blur', function (e) {
        var txt = $(this);
        txt.val(txt.val().replace(/[^0-9]/g, ''));
        if (e.which < 48 || e.which > 57)
            e.preventDefault();
        var data = $.trim(txt.val());
        if (data == "") {
            $('.Invoices tbody tr').show();
        }
    });

    $('.payremit').on('click', function () {
        $.ajax({
            url: $(this).data('url'),
            datatype: 'json',
            contentTpe: 'application/json',
            type: 'GET',
            cache: false,
            async: true,
            success: function (result) {
                $(".payment").html(result);
                //$('.bal').data('balance', $('.bal').html());
            },
            error: function (e) {
                alert("error");
            }
        });
    });

    $(document).on('change', '#drpemp', function () {
        var group = $('#drpemp :selected').val();
        var data = $('.replacepartial');
        var CurrentBalance = $('.bal');
        var Pay = $('.Create');
        if (group.length > 0) {
            $.ajax({
                url: $(this).data('url'),
                datatype: 'json',
                contentTpe: 'application/json',
                type: 'GET',
                cache: false,
                async: true,
                data: { id: group },
                success: function (result) {
                    data.html(result);
                    //Pay.prop('disabled', group.length == 0)
                    //CurrentBalance.html(parseFloat(CurrentBalance.data('balance').replace(',', '')).toFixed(2)
                    //                            - parseFloat($('#grandtotal').val().replace(',', '')).toFixed(2));
                    //if (CurrentBalance.html().indexOf('.') == -1) {
                    //    CurrentBalance.html(Number(CurrentBalance.html()).toLocaleString('en') + '.00');
                    //}
                    //else {
                    //    CurrentBalance.html(Number(CurrentBalance.html()).toLocaleString('en'));
                    //}
                    //Pay.prop('disabled', CurrentBalance.html().replace(',', '') < 0);
                    Pay.prop('disabled', false);
                },
                error: function (e) {
                    alert("error");
                }
            });
        }
        else {
            //CurrentBalance.html(CurrentBalance.data('balance'));
            data.find('input').val('');
            Pay.prop('disabled', true);
        }
    })

    $('.Create').on('click', function () {
        var group = $('#drpemp :selected').val();
        var popup = $('.modal-Notification');
        popup.find('.modal-text').html('Payments');

        if (group.length > 5) {
            $.ajax({
                url: $(this).data('url'),
                datatype: 'json',
                contentTpe: 'application/json',
                type: 'POST',
                cache: false,
                async: true,
                data: { id: group },
                success: function (result) {
                    $('.modal-payment').modal('hide');
                    if (result.code == 1001) {
                        $(".reportviewer").html(result.data);
                        $(".modal-report").modal('show');
                    }
                    else {
                        popup.find('.modal-body').html(result.Message);
                        popup.modal('show');
                    }
                },
                error: function () {
                    popup.find('.modal-body').html("Sorry for the inconvenience but were having a problem with your payment");
                    popup.modal('show');
                }
            });
        }
        else {
            popup.find('.modal-body').html("Please select a group for the payment");
            popup.modal('show');
        }
    });

    $(document).on('click', '.ReprintPayment', function (e) {
        var invoice = $(this).data('invoice');
        $.ajax({
            url: $(this).data('url'),
            datatype: 'json',
            contentTpe: 'application/json',
            type: 'GET',
            cache: false,
            async: true,
            data: { invoiceno: invoice },
            success: function (result) {
                $(".reprintviewer").html(result);
            },
            error: function (e) {
                alert("error");
            }
        });
    });

    /*Commented Delete Payment*/

    //$('.DeletePayment').on('click', function () {
    //    var invoice = $('#del').text();
    //    $.ajax({
    //        url: $(this).data('url'),
    //        datatype: 'json',
    //        contentTpe: 'application/json',
    //        type: 'GET',
    //        cache: false,
    //        async: true,
    //        data: { payid: invoice },
    //        success: function (result) {
    //            $(".modal-deletepay").modal('hide');
    //            $(".indexPartial").html(result);
    //        },
    //        error: function () {
    //            alert("error");
    //        }
    //    });
    //});

    //$('.btndeletepay').on('click', function () {
    //    var invoiceno = $(this).data('invoice');
    //    $("#del").html(invoiceno);
    //});

    $('.btnupdatepass').on('click', function () {
        var curpass = $('#txtcurpass');
        var newpass = $('#txtnewpass');
        var conpass = $('#txtconfrmpass');
        var msg = $('.passInfo');
        msg.hide();
        if (newpass.val() != conpass.val()) {
            msg.html('Confirm password did not match.').show();
            return;
        }
        $('.loading').show();
        $.ajax({
            url: $(this).data('url'),
            datatype: 'json',
            contentTpe: 'application/json',
            type: 'POST',
            cache: false,
            async: true,
            data: { _currpass: curpass.val(), _password: newpass.val(), confirm: conpass.val() },
            success: function (result) {
                $(".loading").hide();
                if (result.code == 1003) {
                    msg.html(result.Message).show();
                }
                else {
                    msg.html(result.Message).show();
                }
            },
            error: function () {
                alert("Unable to connect to service");
            }
        });
    });

    $('.bs-example-modal-cp').on('hidden.bs.modal', function () {
        $(this).find('#txtcurpass , #txtnewpass , #txtconfrmpass').val('');
        $(this).find(".passInfo").html('').hide();
    });

    $('.createReports').on('hidden.bs.modal', function () {
        $(this).find('#historyList').val('')
        .end().find('#items').val('')
        .end().find('#month').val('JAN')
        .end().find('#selectedday').val('1')
        .end().find('#selectedyear').val('2015');
    })

    $('.reprintpreview').on('click', function () {
        $('.modal-reprint-invoice').modal('hide');
        printDiv('reprintablearea');
    });

    $('.btnprint').on('click', function () {
        $(".modal-report").modal('hide');
        printDiv('printablearea');
    });

    $(document).on('change', '#historyList', function () {

        var group = ['Group', 'CancelGroup'];
        if (group.indexOf($("#historyList option:selected").val()) > -1) {
            $('#items').removeAttr('disabled');
        }
        else {
            $('#items').attr('disabled', 'disabled');
        }
    });

    $(".btncreatereports").on('click', function () {
        var month = $("#month").val();
        var day = $("#selectedday").val();
        var year = $("#selectedyear").val();
        var date = year + '-' + month + '-' + day;
        var popup = $('.modal-Notification');
        var group = $('#items :selected');
        popup.find('.modal-text').html('Payments History');

        var history = $('#historyList :selected');
        switch (history.val()) {
            case "Group":

                $.ajax({
                    url: $(this).data('group'),
                    datatype: 'json',
                    contentTpe: 'application/json',
                    type: 'GET',
                    cache: false,
                    async: true,
                    data: {
                        name: group.text(),
                        historyname: history.text(),
                        dateselected: date
                    },
                    success: function (data) {
                        ShowReport(data);
                    },
                    error: ReportError
                });
                break;
            case "All":
                $.ajax({
                    url: $(this).data('all'),
                    datatype: 'json',
                    contentTpe: 'application/json',
                    type: 'GET',
                    cache: false,
                    async: true,
                    data: {
                        historyname: history.text(),
                        dateselected: date
                    },
                    success: function (data) {
                        ShowReport(data);
                    },
                    error: ReportError
                });
                break;
            case "AllCancel":
                $.ajax({
                    url: $(this).data('all-cancel'),
                    datatype: 'json',
                    contentTpe: 'application/json',
                    type: 'GET',
                    cache: false,
                    async: true,
                    data: { historyname: history.text(), date: date },
                    success: function (data) {
                        ShowReport(data)
                    },
                    error: ReportError
                });
                break;
            case "CancelGroup":
                //var group = $('#items :selected');
                $.ajax({
                    url: $(this).data('group-cancel'),
                    datatype: 'json',
                    contentTpe: 'application/json',
                    type: 'GET',
                    cache: false,
                    async: true,
                    data: { name: group.text(), historyname: history.text(), date: date },
                    success: function (data) {
                        ShowReport(data)
                    },
                    error: ReportError
                });
                break;

            default:
                popup.find('.modal-body').html("You need to select the History type of your report");
                popup.modal('show');
                break;

        }
    });
})

function printDiv(divname) {
    var printContents = document.getElementById(divname).innerHTML;
    var originalContents = document.body.innerHTML;

    document.body.innerHTML = printContents;

    window.print();

    document.body.innerHTML = originalContents;

    window.location.reload();
}

function ReportError(e, h, d) {
    var popup = $('.modal-Notification');
    popup.find('.modal-text').html('Payments History');
    popup.find('.modal-body').html("Something went wrong while generating the report");
    popup.modal('show');
}

function ShowReport(data) {
    $(".createReports").modal('hide');
    if (data.code == 1002) {
        var popup = $('.modal-Notification');
        popup.find('.modal-text').html('Payments History');
        popup.find('.modal-body').html(data.Message);
        popup.modal('show');
    }
    else {
        $(".modal-report").modal('show');
        $(".reportviewer").html(data);
    }
}