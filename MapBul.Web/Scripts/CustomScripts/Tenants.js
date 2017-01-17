function OnTenantsDocumentReady() {
    $(".TenantDescriptionRow").click(OnTenantRowClick);
    $('#TenantsTable table').dataTable({
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

function OnTenantRowClick() {
    var url = "Users/_TenantInformationPartial";
    var tenantId = $(this).attr("data-id");
    $.ajax({
        url: url,
        type: "POST",
        data: {
            tenantId: tenantId
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

function EditTenantSuccess(data) {
    if (data.success) {
        $("#Modal").modal("hide");
        RefreshTenantsTable();
        ViewNotification("Changes saved", "success");
    } else {
        ViewNotification(data.errorReason, "error");
    }
}

function RefreshTenantsTable() {
    var url = "Users/_TenantsTablePartial";
    $.ajax({
        url: url,
        type: "POST",
        success: function (data) {
            $("#GuidesTableContainer").html(data);
            OnTenantsDocumentReady();
        },
        error: function () {
            ViewNotification('Error', 'error');
        }
    });
}


$(document).ready(OnTenantsDocumentReady);
