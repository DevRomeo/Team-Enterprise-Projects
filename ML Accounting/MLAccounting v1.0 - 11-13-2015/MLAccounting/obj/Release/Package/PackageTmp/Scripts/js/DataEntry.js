$(document).ready(function () {

    $(document).keypress(function (e) {
        if (e.which == 13) {
            return false;
        }
    })

    $('input[data-val-length-max]').each(function () {
        var $this = $(this);
        var data = $this.data();
        $this.prop("maxlength", data.valLengthMax);
    });
    /*Check checkbox to start data entry*/
    $('.data-entry-form').modal('show');
    $(document).on('change', '.pick', function () {
        var txtbox = $(this).closest('tr');
        var debit = $(this).closest('tr').find('td:eq(2) input:text');
        var credit = $(this).closest('tr').find('td:eq(3) input:text');
        var modal = $('.gl-modal');
        if (this.checked) {
            txtbox.find('td:not(:nth-child(1),:nth-child(2)) input:text').removeAttr('readonly');
            modal.data('id', '#' + $(this).prop('id'));
            modal.data('gl', '#' + $(this).closest('tr').find('td:eq(0) input:text').prop('id'));
            modal.data('desc', '#' + $(this).closest('tr').find('td:eq(1) input:text').prop('id'));
            modal.data('selected', false);
            modal.modal('show');
        }
        else {
            txtbox.find('input:text').val('');
            debit.val('0');
            credit.val('0');
            txtbox.find('input:text').attr('readonly', 'readonly');
            if ($('.pick:checked').length == 0) {
                $('.submit-entry,.income-statement,.balance-sheet,.trial-balance').prop('disabled', 'disabled');
            }
            //$('.Credit,.Debit').html('');
            $('.Credit, .Debit').trigger('change');
        }
    })

    $(document).on('shown.bs.modal', '.gl-modal', function () {
        $('.modal-body').scrollTop(0);
    });
    //uncheck checkbox if no gl selected
    $(document).on('hidden.bs.modal', '.gl-modal', function () {
        $(this).find('input:text').val('');
        $('.search-ledger,.search-description').prop('disabled', '');
        $('.general-ledger tbody tr').show();
        if (!$(this).data('selected')) {
            $($(this).data('id')).trigger('click');
        } else {
            $('.submit-entry,.income-statement,.balance-sheet,.trial-balance').prop('disabled', '');
        }
    })
    //Load GL and Description on page Load
    $.ajax({
        url: $('.data-entry').data('url'),
        datatype: 'json',
        contentTpe: 'application/json',
        type: 'GET',
        async: true,
        success: function (result) {
            if (result.code == 1) {
                $.each(result.corporatenames, function (index, data) {//iterate json Object       
                    $('<option>', {
                        value: data
                    }).html(data).appendTo('#corporate-names');
                });
                $('#corporate-names').data('source', $.parseJSON(result.component));
                $('#EntryNo,.EntryNo').val(result.entryno);
                $('.gl').html(result.EntryGL);
            }
        },
        error: function (e, x, y) {
            console.log(e);
            console.log(x);
        }

    });

    $(document).on('change', '.select', function () {
        var modal = $('.gl-modal');        
        $(modal.data('gl')).val($(this).closest('tr').find('td:eq(1)').text().trim());
        $(modal.data('desc')).val($(this).closest('tr').find('td:eq(2)').text().trim());
        $(this).prop('checked', false);
        modal.data('selected', true);
        modal.modal('hide');
    })

    $(document).on('click', '.balance-sheet,.income-statement,.trial-balance', function () {
        var form = $('#dataentryForm');
        var oldpath = form.prop('action');
        form.prop('action', $(this).data('url'));
        form.attr('data-ajax', "false");
        form.prop('target', '_blank');
        form.submit();
        form.prop('action', oldpath);
        form.attr('data-ajax', "true");
        //console.log(form.prop('target'));
    });

    $(document).on('keyup keypress blur', '.search-ledger', function () {

        if ($(this).val().length > 4) {
            $('.search-description').prop('disabled', 'disabled');
            var $rows = $('.general-ledger tbody tr');
            var key = $.trim($(this).val()).replace(/ +/g, ' ').toLowerCase();
            $rows.show().filter(function () {
                return !~$(this).children(':eq(1)').text().toLowerCase().indexOf(key);
            }).hide();
        }
        if ($(this).val().length == 0) {
            $('.general-ledger tbody tr').show();
            $('.search-description').prop('disabled', '');
        }
    });

    $(document).on('keyup keypress blur', '.search-description', function () {
        if ($(this).val().length > 4) {
            $('.search-ledger').prop('disabled', 'disabled');
            var $rows = $('.general-ledger tbody tr');//.next();
            //if ($(this).data('search-result') == "") {
            //    $(this).data('serch-result', $rows);
            //}

            var key = $.trim($(this).val()).replace(/ +/g, ' ').toLowerCase();

            $rows.show().filter(function () {
                return !~$(this).children(':eq(2)').text().toLowerCase().indexOf(key);
            }).hide();
        }
        if ($(this).val().length == 0) {
            $('.general-ledger tbody tr').show();
            $('.search-ledger').prop('disabled', '');
        }
    });

    $(document).on('change', '.Debit', function () {
        var debit = $(this);
        if (debit.val().trim() != 0) {
            $(this).closest('tr').find('td:eq(3) input:text').prop('disabled', 'disabled');
            //debit.val(parseFloat(debit.val()).toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",").toString())
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
            //credit.val(parseFloat(credit.val()).toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",").toString())
        } else {
            $(this).closest('tr').find('td:eq(2) input:text').prop('disabled', '');
            credit.val('0');
        }
        $('.total-credit').html(sumItUp('.Credit'));
    });
    $('#data-entry-date').datepicker({
        format: 'mm-dd-yyyy'
    });

    $('.date').on('click', function () {
        $('#data-entry-date').datepicker('show');
    });
    $('#data-entry-date').on('changeDate', function () {
        $(this).datepicker('hide');
    })
    $('#corporate-names').on('change', function () {
        var branch = $('#branches');
        if ($(this).val() != "") {
            branch.prop('disabled', '');
            branch.data('source', populateDropDown($(this).data('source'), 'Corpname', this.value, branch, 'branchName'));
        }
        else {
            branch.prop('disabled', 'disabled');
            branch.val('');
            $('.proceed-data-entry').prop('disabled', 'disabled');
        }
    })
    $('#branches').on('change', function () {
        var btnProceed = $('.data-entry-form .proceed-data-entry')
        var selectedBranch = $(this).val();
        if ($(this).val() != "") {
            btnProceed.prop('disabled', '');
            $.each($(this).data('source'), function (index, value) {
                if (this.branchName == selectedBranch) {
                    $('#BranchAddress').val(this.address);
                    $('#Zone').val(this.zcode);
                    $('#BranchCode').val(this.bcode);
                    return;
                }
            })
        }
        else {
            btnProceed.prop('disabled', 'disabled');
        }
    });

    $('.data-entry-form').on('hidden.bs.modal', function () {
        var entryForm = $('#dataentryForm');
        entryForm.find('#CorporateName').val($(this).find('#corporate-names').val());
        entryForm.find('#Branch').val($(this).find('#branches').val());
        entryForm.find('#Date').val($(this).find('#data-entry-date').val());

    });

    $(document).on('keypress keyup blur', '.Debit,.Credit,.search-ledger,.search-description', AllowedInput);
});

function sumItUp(field) {
    var sum = 0;
    $(field).each(function () {
        if (!isNaN(this.value) && this.value.length != 0) {
            sum += parseFloat(this.value);
        }
    });
    return sum.toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
function beginDataEntry() {
    alert('begin data entry');
}
function completeDataEntry(e) {
    var msg = $('.modal-notification');
    msg.find(".header").text("JOURNAL ENTRY");
    if (e.responseJSON.code == 1) {
        $('.modal-notification').on('hidden.bs.modal', function () {
            location = e.responseJSON.action;
        });
    }
    msg.find('.modal-body').text(e.responseJSON.msg);
    msg.modal('show');
}
function AllowedInput(e) {
    var _event = e || window.event;
    //var key = (e.which) ? _event.keyCode : _event.which;
    var key = typeof _event.which === "undefined" ? _event.keyCode : _event.which;
    var allowchar = new RegExp($(this).data('val-regex'));    

    return allowchar.test(String.fromCharCode(key));
}

function populateDropDown(Datasource, SearchPropertyField, SearchPropertyValue, Targetdropdown, TargetDropDownData) {
    Targetdropdown.empty()
    $('<option>', {
        value: ''
    }).html('').appendTo(Targetdropdown);
    var items = [];
    var newSource = [];
    $.each(Datasource, function (index, data) {//iterate json Object        
        $.each(data, function (propertyName, propertyValue) {//iterate Object Propety Name    
            if (propertyName.toLowerCase() == SearchPropertyField.toLowerCase() && propertyValue.toLowerCase() == SearchPropertyValue.toLowerCase()) {
                newSource.push(Datasource[index]);
                $.each(Datasource[index], function (property, value) {
                    if (property == TargetDropDownData) {
                        if ($.inArray(value, items) === -1) {
                            items.push(value);
                            $('<option>', {
                                value: value
                            }).html(value).appendTo(Targetdropdown);
                        }
                    }
                })
            }
        });
    });
    return newSource;
}