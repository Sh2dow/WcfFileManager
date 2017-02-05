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
            $(this).attr('title', 'Save File');
        }

    }
    else {
        // making sure that only one row is editable and save button of editable row is clicked
        if (allEditableRow.length == 1 && $(this).attr('src') == '/Content/images/save.jpg')
        {
                      
            var selectedId = $(this).parents('tr').find('td:nth-child(1)').text().trim();
            var selectedName = $(this).parents('tr').find('#FileName').val();
            var selectedSize = $(this).parents('tr').find('#FileSize').val();
            // create object with updated values
            var FileModel =
                {
                    "FileId": selectedId,
                    "FileName": selectedName,
                    "FileSize": selectedSize
                };
            $.ajax({
                url: '/Home/EditFile',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(FileModel)
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

$('.delete-File').click(function () {
    var selectedId = $(this).parents('tr').find('td:nth-child(1)').text().trim();
    var selectedName = $(this).parents('tr').find('#FileName').val();
    var message = "Please confirm delete for File ID " + selectedId + ", and Name: " + selectedName + " ? \nClick 'OK' to delete otherwise 'Cancel'.";
    if (confirm(message) == true) {
        selectedId = $(this).parents('tr:first').children('td:first').text();
        $.ajax({
            url: '/Home/DeleteFile',
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

$('.btnFile').click(function () {
    $('.btnFile').hide();

    $.ajax({
        // Call CreatePartialView action method
        url: '/Home/GetFile',
        type: 'Get',
        success: function (data) {
            $("#placeHolderFile").empty().append(data);

        },
        error: function () {
            alert("something seems wrong");
        }
    });
});
