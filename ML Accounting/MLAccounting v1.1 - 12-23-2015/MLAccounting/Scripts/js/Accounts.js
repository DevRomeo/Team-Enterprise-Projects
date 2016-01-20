$(document).ready(function () {

    initial();

    /*Select Corporate name*/
    $(document).on('change', '#CorporateName', function () {
        if (this.value != '') {
            var branch = $('#Branch');
            branch.prop('disabled', '');
            branch.data('source', populateDropDown($(this).data('source'), 'Corpname', this.value, branch, 'branchName'));                     
        }
        else {
            $('#Branch,#Month,#Year,.download-report').prop('disabled', 'disabled');
            $('#Branch,#Month,#Year').val('');
        }
    });

    /*Select Report Type*/
    $(document).on('change', '#Report', function () {
        $('#Branch,#Month,#Year,.download-report,.rdyear,.rdmonth').prop('disabled', 'disabled');
        $('#Branch,#Month,#Year,#CorporateName,#SelectedGL').val('');
        $(document).off('change', '#Branch');
        $(document).off('change', '#Year');
        if (this.value == 'CLOSING OF ACCOUNTS') {
            Closing();
        }
        if (this.value == 'JOURNAL') {
            Journal();
        }
        if (this.value == 'GENERAL LEDGER') {
            Ledger();
        }        
    });

});

/*Ajax that loads the components for Books of Accounts Report*/
function initial() {
    $.ajax({
        url: $('.reports').data('url'),
        datatype: 'json',
        contentTpe: 'application/json',
        type: 'GET',
        async: true,
        success: function (result) {
            $('.reports').html($('.loading'));
            setTimeout(function () {
                $('.reports').html(result.page);
                $.each(result.corporatenames, function (index, data) {//iterate json Object       
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

/*Function to populate target dropdown base on source array*/
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

/*Journal Event Handler*/
function Journal() {
    $('.rdmonth').prop('checked', true);

    /*Select Month*/
    $(document).on('change', '#Month', function () {
        var year = $('#Year')
        if (this.value != '') {
            year.prop('disabled', '');
        }
        else {
            year.prop('disabled', 'disabled');
            year.val('');
            year.trigger('change');
        }
    });

    /*Select Branch*/
    $(document).on('change', '#Branch', function (e) {
        if (this.value != '') {
            $('.rdmonth,#Month').prop('disabled', '');
            Branch($(this).val());
        }
        else {
            $('#Month,#Year').prop('disabled', 'disabled');
            $('#Month,#Year').val('');
            $('#Year').trigger('change');
        }
    });

    /*Select Year*/
    $(document).on('change', '#Year', function () {
        $('.download-report').prop('disabled', this.value != '' ? '' : 'disabled');
    });

    $('#CorporateName').val('').end().trigger('change', '');
}

/*General Ledger Event Handler*/
function Ledger() {
    $('.rdmonth').prop('checked', true);

    /*Show GLAccounts when gl-modal is hidden*/
    $(document).on('hidden.bs.modal', '.gl-modal', function () {
        $(this).find('input:text').val('');
        $('.search-ledger,.search-description').prop('disabled', '');
        $('.general-ledger tbody tr').show();
    })

    /*Select Month*/
    $(document).on('change', '#Month', function () {
        var year = $('#Year')
        if (this.value != '') {
            year.prop('disabled', '');
        }
        else {
            year.prop('disabled', 'disabled');
            year.val('');
        }
    });

    /*Select Branch*/
    $(document).on('change', '#Branch', function (e) {
        if (this.value != '') {
            $('.rdmonth,.rdyear,#Month').prop('disabled', '');
            Branch($(this).val());            
        }
        else {
            $('#Month,#Year').prop('disabled', 'disabled');
            $('#Month,#Year').val('');
        }
    });

    /*Report By Year*/
    $(document).on('change', '.rdyear', function () {
        var cbmonth = $('#Month');
        cbmonth.val('');
        cbmonth.prop('disabled', 'disabled');
        $('#Year').prop('disabled', '');
    });

    /*Report By Month*/
    $(document).on('change', '.rdmonth', function () {
        $('#Month').prop('disabled', '');
    });

    /*Show modal with GL Accounts*/
    $(document).on('click', '.search', function () {
        if ($('#Report').val() == 'GENERAL LEDGER')
            $('.gl-modal').modal('show');
    })

    /*Searching by GLNumber*/
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

    /*Searching by GLDescription*/
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

    /*If GL was selected*/
    $(document).on('change', '.select', function () {
        var modal = $('.gl-modal');
        modal.data('gl', $(this).closest('tr').find('td:eq(1)').text().trim());
        modal.data('desc', $(this).closest('tr').find('td:eq(2)').text().trim());
        $(this).prop('checked', false);
        modal.data('selected', true);
        $('#SelectedGL').val(modal.data('gl') + ' - ' + modal.data('desc'));
        $('#GLAccount').val(modal.data('gl'));
        $('#GLDescription').val(modal.data('desc'));
        $('.download-report').prop('disabled', '');
        modal.modal('hide');
    });

}

/*Closing of Account Event Handler*/
function Closing() {
    
    $('.rdyear').prop('checked', true);    

    /*Select Branch*/
    $(document).on('change', '#Branch', function (e) {
        if (this.value != '') {
            $('#Year').prop('disabled', '');            
            Branch($(this).val());
        }
        else {
            $('#Year').prop('disabled', 'disabled');
            $('#Year').val('');
        }
    });

    /*Select Year*/
    $(document).on('change', '#Year', function () {
        $('.download-report').prop('disabled', this.value != '' ? '' : 'disabled');
    });

}

/*Branch Information*/
function Branch(e) {
    var selectedBranch = e;
    $.each($('#Branch').data('source'), function (index, value) {
        if (this.branchName == selectedBranch) {
            $('#Address').val(this.address);
            $('#BranchCode').val(this.bcode);
            $('#Zone').val(this.zcode);
            return;
        }
    })
}