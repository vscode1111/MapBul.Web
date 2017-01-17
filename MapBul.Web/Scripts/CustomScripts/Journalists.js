function OnJournalistsDocumentReady() {
    $("#NewJornalistButton").click(OnNewJournalistClick);
    $(".JournalistDescriptionRow").click(OnJournalistRowClick);
    $('#JournalistsTable table').dataTable({
        "pageLength": 30,
        "autoWidth": true,
        "language": {
            "lengthMenu": "Show _MENU_",
            "zeroRecords": "Nothing found",
            "info": "Page _PAGE_ from _PAGES_",
            "infoEmpty": "No records",
            "infoFiltered": "(Found from _MAX_)",
            "search": "Search",
            "paginate": {
                "first": "First",
                "last": "Last",
                "next": "Next",
                "previous": "Previous"
            }
        }
    });
}

function EditJournalistSuccess(data) {
    if (data.success) {
        $("#Modal").modal("hide");
        RefreshJournalistsTable();
        ViewNotification("Changes saved", "success");
    } else {
        ViewNotification(data.errorReason, "error");
    }
}

function OnJournalistRowClick() {
    var url = "Users/_JournalistInformationPartial";
    var journalistId = $(this).attr("data-id");
    $.ajax({
        url: url,
        type: "POST",
        data: {
            journalistId: journalistId
        },
        success: function (data) {
            $("#ModalContent").html(data);
            $("#Modal").modal("show");
        },
        error: function () {
            ViewNotification('Error', 'error');
        }
    });
}

function OnNewJournalistClick() {
    var url = $(this).attr("data-actionurl");
    $.ajax({
        url: url,
        type: "POST",
        success: function (data) {
            $("#ModalContent").html(data);
            $("#Modal").modal("show");
        },
        error: function () {
            ViewNotification('Error', 'error');
        }
    });
}

function RefreshJournalistsTable() {
    var url = "Users/_JournalistsTablePartial";
    $.ajax({
        url: url,
        type: "POST",
        success: function (data) {
            $("#JournalistsTableContainer").html(data);
            OnJournalistsDocumentReady();
        },
        error: function () {
            ViewNotification('Error', 'error');
        }
    });
}

function AddNewJournalistSuccess(data) {
    if (data.success) {
        $("#Modal").modal("hide");
        RefreshJournalistsTable();
        ViewNotification("The journalist added", "success");
    } else {
        ViewNotification(data.errorReason, "error");
    }
}

$(document).ready(OnJournalistsDocumentReady);


