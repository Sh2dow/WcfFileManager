$(document).ready(function () {
    $('.edit-mode').hide();
});

$('.edit-button').on("click", function () {
    // allEditableRow : will have all rows which are editable (where save button is visible).
    var allEditableRow = $("[src='/Content/images/save.jpg']");
    
    // make row editable only if no other row is editable as of now by toggle
    // otherwise go to else and save data of editable row first or alert to save that"
    if (allEditableRow.length == 0) {
        var tr = $(this).parents('tr:first');
        tr.find('.edit-mode,.display-mode').toggle();
        var imageSource = $(this).attr('src');

        if (imageSource == '/Content/images/edit.jpg') {
            $(this).attr('src', '/Content/images/save.jpg');
            $(this).attr('title', 'Save Customer');
        }

    }
    else {
        // making sure that only one row is editable and save button of editable row is clicked
        if (allEditableRow.length == 1 && $(this).attr('src') == '/Content/images/save.jpg')
        {
                      
            var selectedId = $(this).parents('tr').find('td:nth-child(1)').text().trim();
            var selectedName = $(this).parents('tr').find('#CustomerName').val();
            var selectedAddless = $(this).parents('tr').find('#CustomerAddress').val();
            // create object with updated values
            var customerModel =
                {
                    "CustomerId": selectedId,
                    "CustomerName": selectedName,
                    "CustomerAddress": selectedAddless
                };
            $.ajax({
                url: '/Home/EditCustomer',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(customerModel)
            });

            alert("Your data is saved");
            window.location.reload(true);
            $(this).attr('src', '/Content/images/edit.jpg');

        }
        else {
            alert("Please finish the editing of current editable row. Save first");
        }
    }

});

$('a').click(function () {
    $(this).attr('data-swhglnk', 'false');
});

$('.delete-customer').click(function () {
    var selectedId = $(this).parents('tr').find('td:nth-child(1)').text().trim();
    var selectedName = $(this).parents('tr').find('#CustomerName').val();
    var message = "Please confirm delete for Customer ID " + selectedId + ", and Name: " + selectedName + " ? \nClick 'OK' to delete otherwise 'Cancel'.";
    if (confirm(message) == true) {
        selectedId = $(this).parents('tr:first').children('td:first').text();
        $.ajax({
            url: '/Home/DeleteCustomer',
            data: { id: selectedId },
            type: 'POST',
            success: function () {
                window.location.reload();
            },
            error: function (xhr) {
                alert("something seems wrong");
            }
        });
    }
    else
    {
        alert("Delete is Canceled!");
    }
});

$('.btnCustomerPV').click(function () {
    $('.btnCustomerPV').hide();

    $.ajax({
        // Call CreatePartialView action method
        url: '/Home/GetCustomerPV',
        type: 'Get',
        success: function (data) {
            $("#placeHolderCustomerPV").empty().append(data);

        },
        error: function () {
            alert("something seems wrong");
        }
    });
});
