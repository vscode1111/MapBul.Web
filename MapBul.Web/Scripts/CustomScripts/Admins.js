function OnAdminsDocumentReady() {
    $("#NewAdminButton").click(OnNewAdminClick);
    $('#AdminsTable table').dataTable({
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

function OnNewAdminClick() {
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

function RefreshAdminsTable() {
    var url = "Users/_AdminsTablePartial";
    $.ajax({
        url: url,
        type: "POST",
        success: function (data) {
            $("#AdminsTableContainer").html(data);
            OnAdminsDocumentReady();
        },
        error: function () {
            ViewNotification('Error', 'error');
        }
    });
}

function AddNewAdminSuccess(data) {
    if (data.success) {
        $("#Modal").modal("hide");
        RefreshAdminsTable();
        ViewNotification("Administrator added", "success");
    } else {
        ViewNotification(data.errorReason, "error");
    }
}

$(document).ready(OnAdminsDocumentReady);
