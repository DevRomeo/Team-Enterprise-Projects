$(document).ready(function () {

    $(document).on('click', '.UpdateGroupName', function () {
        var popup = $('.modal-Notification');
        popup.find('.modal-text').html('Group');

        var progress = $('.process');

        var group = $('#groupname').data('group');
        var groupname = $('#groupname').val();

        if (groupname.length > 0) {
            progress.modal('show');

            $.ajax({
                url: $(this).data('url'),
                datatype: 'json',
                contentTpe: 'application/json',
                type: 'post',
                cache: false,
                async: true,
                data: { groupid: group, newgroupname: groupname },
                success: function (result) {
                    setTimeout(function () {
                        progress.modal('hide');
                        if (result.code == 1001) {
                            popup.find('.modal-body').html(result.Message);
                        }
                        else {
                            popup.find('.modal-body').html(result.Message);
                            $('#groupname').val(result.GroupName);
                        }
                        popup.modal('show');
                    }, 500);
                },
                error: function () {
                    progress.modal('hide');
                    popup.find('.modal-body').html('Oops something wrong while Updating the Group name');
                    popup.modal('show');
                }
            });

        } else {
            popup.find('.modal-body').html('Group name should not be Empty');
            popup.modal('show');
        }
    });

    $(document).on('click', '.AddEmployeeToGroup', function () {
        var group = $('#groupname').data('group');
        var popup = $('.modal-Notification');
        popup.find('.modal-text').html('Add employee to group');

        $.ajax({
            url: $(this).data('url'),
            datatype: 'json',
            contentTpe: 'application/json',
            type: 'post',
            cache: false,
            async: true,
            data: { groups: group },
            success: function (result) {
                if (result.code == 1001) {
                    $('.myModalContent').html(result.data);
                }
                else {
                    popup.find('.modal-body').html(result.Message);
                }
            },
            error: function (e, r, d) {
                console.log(e);
                popup.find('.modal-body').html('We are having problem while processing your request. Please Try again late');
            }
        });
    });

    $(document).on('click', '.SaveEmployeeToGroup', function () {
        var groups = $('#groupname').data('group');
        var $parent = $(this).parent().parent();
        var emp = $parent.find("option:selected").val();
        var sal = $parent.find("input").val();
        var msg = $parent.find('.msg');
        var popup = $('.modal-Notification');
        popup.find('.modal-text').html('Adding of Groups');

        if (!isNaN(sal) && sal.trim().length > 0 && sal > 0) {
            msg.hide();
            $.ajax({
                url: $(this).data('url'),
                datatype: 'json',
                contentTpe: 'application/json',
                type: 'post',
                cache: false,
                async: true,
                data: { groupid: groups, empid: emp, salary: sal },
                success: function (result) {
                    $('.modal-add-group-employee').modal('hide');
                    if (result.code == 1001) {
                        $("#refreshdata").html(result.data);
                    } else {
                        popup.find('.modal-body').html(result.Message);
                        popup.modal('show');
                    }
                },
                error: function (e) {
                    msg.html('There was a problem while Adding Employee to the Group').show();
                }
            });
        }
        else {
            msg.html("Please Provide Employee Salary").show();
        }

    });

    $(document).on('click', '.RemoveEmployee', function () {
        var employee = $('.DeleteEmployee');
        employee.data('employee', '');
        employee.data('employee', $(this).data('mymember2'));
        employee.data('url', '');
        employee.data('url', $(this).data('url'));
    });

    $('.btndeletegroup').on('click', function () {
        var grpid = $('#group').data('group');
        $.ajax({
            url: $(this).data('url'),
            datatype: 'json',
            contentTpe: 'application/json',
            type: 'post',
            cache: false,
            async: false,
            data: { groupid: grpid },
            success: function (result) {
                if (result.errCode == 1) {
                    $(".modal-sm-group-delete").hide();
                    location = result.action
                }
            },
            error: function (e) {
                console.log(e);
                alert("error");
            }
        });
    });

    $(document).on('click', '.DeleteEmployee', function () {
        var group = $('#groupname').data('group');
        var employee = $(this).data('employee');
        $.ajax({
            url: $(this).data('url'),
            datatype: 'json',
            contentTpe: 'application/json',
            type: 'post',
            cache: false,
            async: true,
            data: { groupid: group, empid: employee },
            success: function (result) {
                $(".modal-sm-member-delete").modal('hide');
                $("#refreshdata").html(result);
            },
            error: function (e, s, d) {
                alert("error");
            }
        });
    });

    $(document).on('click', '.EditSalary', function () {
        var data = $(this).data('myempmember');
        $.ajax({
            url: $(this).data('url'),
            datatype: 'json',
            contentTpe: 'application/json',
            type: 'GET',
            cache: false,
            async: true,
            data: { empid: data },
            success: function (result) {
                $('.modalsalarycontent').html(result);
            },
            error: function (e, x, y) {
                alert("error in Edit group Member");
            }
        });
    });

    $(document).on('click', '.UpdateSalary', function () {
        var $parent = $(this).parent().parent();
        var emp = $parent.find("#kycId").val();
        var sal = $parent.find("#txtsalary").val();
        var grpid = $('#groupname').data('group');
        var msg = $parent.find('.msg');
        var popup = $('.modal-Notification');
        popup.find('.modal-text').html('Employee Salary');
        if (!isNaN(sal) && sal.trim().length > 0 && sal > 0) {
            $.ajax({
                url: $(this).data('url'),
                datatype: 'json',
                contentTpe: 'application/json',
                type: 'post',
                cache: false,
                async: true,
                data: { groupid: grpid, empid: emp, salary: sal },
                success: function (result) {
                    $(".modal-edit-salary").modal('hide');
                    if (result.code == 1001) {
                        $("#refreshdata").html(result.data);
                    }
                    else {
                        popup.find('.modal-body').html(result.Message);
                        popup.modal('show');
                    }
                },
                error: function () {
                    msg.html("There was a problem upon editing Employee Salary").show();
                }
            });
        }
        else {
            msg.html("Please Provide Employee Salary").show();
        }
    });

    $('.modal-add-group-employee , .modal-edit-salary').on('hidden.bs.modal', function () {
        $(this).find('.msg').html('');
    });

    $('.modal-add-group').on('hidden.bs.modal', function () {
        $(this).find('.txtgroupname').val('');
        $(this).find('.msg').html('');
    })

    var groups = create();

    $('.AddGroup').on('click', function () {
        var popup = $('.modal-Notification');
        popup.find('.modal-text').html('Adding of Groups');
        var GroupForm = $('.modal-add-group');
        var param = $("#txtgroupname");
        var GroupExist = false;

        $.each(groups, function (key, value) {
            if (param.val().trim().toLowerCase() == value.toLowerCase()) {
                GroupExist = true;
                return;
            }
        });

        if (param.val().length > 0) {
            GroupForm.find('.loading').show();
            if (!GroupExist) {
                $.ajax({
                    url: $(this).data('url'),
                    datatype: 'json',
                    contentTpe: 'application/json',
                    type: 'post',
                    cache: false,
                    async: true,
                    data: { groupname: param.val() },
                    success: function (result) {
                        GroupForm.find('.loading').hide();
                        GroupForm.modal('hide');
                        setTimeout(function () {
                            if (result.code == 1001) {
                                popup.find('.modal-body').html(result.Message);
                                popup.modal('show');
                                popup.on('hidden.bs.modal', function () {
                                    location = result.action;
                                })
                            } else {
                                popup.find('.modal-body').html(result.Message);
                                popup.modal("show");
                            }
                        }, 500);
                    },
                    error: function () {
                        popup.find('.modal-body').html("There was a problem upon Creating new Group");
                        popup.modal("show");
                    }
                });
            }
            else {
                GroupForm.find('.loading').hide();
                GroupForm.modal('hide');
                popup.find('.modal-body').html('You already have a group containing this name Please try another name of a group');
                popup.modal("show");
            }
            param.val("");
        }
        else {
            GroupForm.modal('hide');
            popup.find('.modal-body').html('Please provide a name for your new group');
            popup.modal("show");
        }
    });

    $('.searchGroup').on('click', function () {
        var $rows = $('.Groups tbody tr');//.next();
        var key = $.trim($('.GroupKey').val()).replace(/ +/g, ' ').toLowerCase();

        $rows.show().filter(function () {
            return !~$(this).children(':eq(0)').text().toLowerCase().indexOf(key);
        }).hide();

    });

    $(document).on('click', '.edit-group', function () {
        location = $(this).data('url');
    });

    $('.GroupKey').on('keyup', function (e) {

        if (e.keyCode == 13) {
            $('.searchGroup').click();
            return;
        }

        var data = $.trim($('.GroupKey').val());
        if (data == "") {
            $('.Groups tbody tr').show();
        }
    });

    $(document).on('keypress keyup blur', '#txtsalary', function (e) {
        var _event = e || window.event;
        var key = (e.which) ? _event.keyCode : _event.which;
        var allowchar = new RegExp(/^([0-9.])$/);

        return allowchar.test(String.fromCharCode(key))
    });



});

function create() {

    return $('.Groups tr:gt(0)').map(function (i, e) {
        return $(this).find('td').eq(0).text();
    }).get();
};