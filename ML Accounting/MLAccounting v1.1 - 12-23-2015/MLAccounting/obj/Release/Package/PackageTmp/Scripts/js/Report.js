$(document).ready(function () {    

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
            }, 500);
        },
        error: function (e, x, y) {
            console.log(e);
            console.log(x);
        }
    });

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
    $(document).on('change', '#Branch', function () {
        if (this.value != '') {
            $('#Month').prop('disabled', '');
            var selectedBranch = $(this).val();            
            $.each($(this).data('source'), function (index, value) {
                if (this.branchName == selectedBranch) {                    
                    $('#BranchCode').val(this.bcode);
                    $('#Zone').val(this.zcode);
                    return;
                }
            })
        }
        else {
            $('#Month,#Year,.download-report').prop('disabled', 'disabled');
            $('#Month,#Year').val('');
        }                
    });
    $(document).on('change', '#Month', function () {
        if (this.value != '') {
            $('#Year').prop('disabled', '');
        }
        else {
            $('#Year,.download-report').prop('disabled', 'disabled');
            $('#Year').val('');
        }
    });
    $(document).on('change', '#Year', function () {
        if (this.value != '') {
            $('.download-report').prop('disabled', '');
        }
        else {
            $('.download-report').prop('disabled', 'disabled');
        }
    });


});

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