$(function () {


    $("#Date").datepicker({
        format: 'mm-dd-yyyy'

    });

    $('#search').on('click', function (e) {
        e.preventDefault();

        $.ajax({
            url: $(this).data("search"),
            type: 'POST',
            async: true,
            data: { zone: $('#zone').val(), date: $('#date').val() },
            success: function (data) {
                // alert("success");
                $('#requestDiv').html(data);
            },


        });



    });
    function onBeginUpdateRequest() {
        //Show Loading
    };
    function onRequestSuccess(result) {
        if (result.responseCode == 1) {
            alert(result.message);
        } else {
            alert(result.message);
        }

    };
    function onRequestError() {

    };

    function triggerSearh() {
        $('#search').click();
        setInterval(
            function () {
                $('#search').click();
            }, 60000);
    };
    triggerSearh();

    $('#submitUpdateRequest').on('click', function (e) {
        e.preventDefault();
        var temp = $(this);
        temp.attr('disabled', 'disabled');
        $.ajax({
            url: $(this).data("request"),
            type: 'POST',
            async:true,
            data: $('#addupdaterequest').serialize(),
            success: function (data) {
                if (data.responseCode == 0) {
                     

                    $('#message').html(" <div class='alert alert-danger alert-dismissible text-center' style='height:40px;width:250px;padding:0px;' role='alert'>" +
                                        " <button type='button' class='close' data-dismiss='alert' aria-label='Close'  "+
                                        " <span aria-hidden='true'>&times;</span> "+
                                        " </button> "+
                                        " <strong>" + data.message + "</strong></div>");
                    temp.removeAttr('disabled', 'disabled');
                }
                else
                {
                      
                    $('#message').html(" <div class='alert alert-success alert-dismissible text-center' style='height:40px;width:250px;padding:5px;' role='alert'>" +
                                        " <button type='button' class='close' data-dismiss='alert' aria-label='Close'> " +
                                        " <span aria-hidden='true'>&times;</span> " +
                                        " </button> " +
                                        " <strong>" + data.message + "</strong></div>");
                    temp.removeAttr('disabled', 'disabled');
                }
                  
            },
            error: function (data)
            {
                alert("Something went wrong");
            }

        });
    });

    $("#DateFrom").datepicker({
        format: 'mm-dd-yyyy'

    });
    $("#DateTo").datepicker({
        format: 'mm-dd-yyyy'
    });
    $('.DateTo').click(function () {
        $("#DateTo").datepicker("show");
    });
    $('.DateFrom').click(function () {
        $("#DateFrom").datepicker("show");
    });
    function onBeginUpdateRequest() {
        //Show Loading
    }
    function onRequestSuccess(result) {
        alert(result.responseCode);
        if (result.responseCode == 1) {
            alert(result.message);
        } else {
            alert(result.message);
        }

    }
    function onRequestError() {

    }
function showalert(message, message1, alerttype) {

    if (message1 != "") {
        $('#alert_placeholder2').append('<div id="alertdiv2" class="alert alert-danger"><a class="close" data-dismiss="alert">×</a><span><strong>NO TRANSACTIONS</strong><br>' + message1 + '</span></div>')

        setTimeout(function () { // this will automatically close the alert and remove this if the users doesnt close it in 5 secs
            $("#alertdiv2").remove();
        }, 30000);
    }
    $('#alert_placeholder').append('<div id="alertdiv" class="alert ' + alerttype + '"><a class="close" data-dismiss="alert">×</a><span>' + message + '</span></div>')

    setTimeout(function () { // this will automatically close the alert and remove this if the users doesnt close it in 5 secs
        $("#alertdiv").remove();

    }, 5000);
}

});
