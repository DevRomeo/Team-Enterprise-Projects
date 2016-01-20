$(document).ready(function () {

    init();

    $(document).on('change', '#Branch', function () {
        if (this.value != '') {
            var selectedBranch = $(this).val();
            $.each($(this).data('source'), function (index, value) {
                if (this.branchName == selectedBranch) {
                    $('#Address').val(this.address);
                    $('#BranchCode').val(this.bcode);
                    $('#Zone').val(this.zcode);
                    return;
                }
            })
        }
    });

    $(document).on('change', '#CorporateName', function () {
        if (this.value != '') {
            var branch = $('#Branch');
            branch.prop('disabled', '');
            branch.data('source', populateDropDown($(this).data('source'), 'Corpname', this.value, branch, 'branchName'));
        }
        else {
            $('#Branch').prop('disabled', 'disabled');
            $('#Branch').val('');
        }
    });


    $(document).on('click', '.search', function () {
        if ($('#Branch').val() != '') {
            $('.gl-modal').modal('show');
        }
    })


    $(document).on('keypress keyup blur', '#Amount', AllowedInput);

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
    $(document).on('change', '.select', function () {
        var modal = $('.gl-modal');
        modal.data('gl', $(this).closest('tr').find('td:eq(1)').text().trim());
        modal.data('desc', $(this).closest('tr').find('td:eq(2)').text().trim());
        $(this).prop('checked', false);
        modal.data('selected', true);
        $('#GLDescription').prop('readonly', '');
        $('#GLDescription').trigger('blur');
        $('#GLDescription').val(modal.data('gl') + ' - ' + modal.data('desc'));
        $('#GLDescription').prop('readonly', true);
        $('#GLNumber').val(modal.data('gl'));
        console.log($('#GLNumber'))
        modal.modal('hide');
    });

    $(document).on('hidden.bs.modal', '.gl-modal', function () {
        $(this).find('input:text').val('');
        $('.search-ledger,.search-description').prop('disabled', '');
        $('.general-ledger tbody tr').show();
    })

});

function loading() {
    var loading = $('.modal-process');
    loading.modal('show');
}

function completeDataEntry(response) {
    var res = response.responseJSON;
    if (res.code == 1) {
        $('#CorporateName,#Branch,#Amount,#GLDescription').val('');
    }
    $('.modal-process').modal('hide');
    $('.modal-notification').find('.modal-body').text(res.message).end().modal('show');
}

function AllowedInput(e) {
    var _event = e || window.event;
    //var key = (e.which) ? _event.keyCode : _event.which;
    var key = typeof _event.which === "undefined" ? _event.keyCode : _event.which;
    var allowchar = new RegExp($(this).data('valid'));

    return allowchar.test(String.fromCharCode(key));
}

function init() {

    $('.beginning-balance').html($('.loading'));
    
    $('input[data-val-length-max]').each(function () {
        var $this = $(this);
        var data = $this.data();
        console.log(data.valLengthMax)
        $this.prop("maxlength", data.valLengthMax);
    });

    $.ajax({
        url: $('.beginning-balance').data('url'),
        datatype: 'json',
        contentTpe: 'application/json',
        type: 'GET',
        async: true,
        success: function (result) {
            setTimeout(function () {
                $('.beginning-balance').html(result.page);
                $.each(result.corporatenames, function (index, data) {//iterate json object       
                    $('<option>', {
                        value: data
                    }).html(data).appendTo('#CorporateName');
                });
                $('#CorporateName').data('source', result.component);
                $('.gl').html(result.account);
            }, 500);
        },
        error: function (e, x, y) {
            console.log(e);
            console.log(x);
        }
    });

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