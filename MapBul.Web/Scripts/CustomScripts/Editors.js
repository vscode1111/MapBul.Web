function OnEditorsDocumentReady() {
    $("#NewEditorButton").click(OnNewEditorClick);
    $(".EditorDescriptionRow").click(OnEditorRowClick);
    $('#EditorsTable table').dataTable({
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


function EditEditorSuccess(data) {
    if (data.success) {
        $("#Modal").modal("hide");
        RefreshEditorsTable();
        ViewNotification("Changes saved", "success");
    } else {
        ViewNotification(data.errorReason, "error");
    }
}


function OnEditorRowClick() {
    var url = "Users/_EditorInformationPartial";
    var editorId = $(this).attr("data-id");
    $.ajax({
        url: url,
        type: "POST",
        data: {
            editorId: editorId
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

function OnNewEditorClick() {
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

function RefreshEditorsTable() {
    var url = "Users/_EditorsTablePartial";
    $.ajax({
        url: url,
        type: "POST",
        success: function (data) {
            $("#EditorsTableContainer").html(data);
            OnEditorsDocumentReady();
        },
        error: function () {
            ViewNotification('Error', 'error');
        }
    });
}

function AddNewEditorSuccess(data) {
    if (data.success) {
        $("#Modal").modal("hide");
        RefreshEditorsTable();
        ViewNotification("The editor added", "success");
    } else {
        ViewNotification(data.errorReason, "error");
    }
}



$(document).ready(OnEditorsDocumentReady);