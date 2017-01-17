function OnGuidesDocumentReady() {
    $("#NewGuideButton").click(OnNewGuideClick);
    $(".GuideDescriptionRow").click(OnGuideRowClick);
    $('#GuidesTable table').dataTable({
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

function OnNewGuideClick() {
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

function AddNewGuideSuccess(data) {
    if (data.success) {
        $("#Modal").modal("hide");
        RefreshGuidesTable();
        ViewNotification("Guides added", "success");
    } else {
        ViewNotification(data.errorReason, "error");
    }
}

function OnGuideRowClick() {
    var url = "Users/_GuideInformationPartial";
    var guideId = $(this).attr("data-id");
    $.ajax({
        url: url,
        type: "POST",
        data: {
            guideId: guideId
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



function EditGuideSuccess(data) {
    if (data.success) {
        $("#Modal").modal("hide");
        RefreshGuidesTable();
        ViewNotification("Changes saved", "success");
    } else {
        ViewNotification(data.errorReason, "error");
    }
}

function RefreshGuidesTable() {
    var url = "Users/_GuidesTablePartial";
    $.ajax({
        url: url,
        type: "POST",
        success: function (data) {
            $("#GuidesTableContainer").html(data);
            OnGuidesDocumentReady();
        },
        error: function () {
            ViewNotification('Error', 'error');
        }
    });
}

$(document).ready(OnGuidesDocumentReady);
