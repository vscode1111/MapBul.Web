function OnTenantsDocumentReady() {
    $(".TenantDescriptionRow").click(OnTenantRowClick);
    $('#TenantsTable table').dataTable({
        "pageLength": 30,
        "autoWidth": true,
        "language": {
            "lengthMenu": "Показать _MENU_",
            "zeroRecords": "Ничего не найдено",
            "info": "Страница _PAGE_ из _PAGES_",
            "infoEmpty": "Нет записей",
            "infoFiltered": "(Найдено из _MAX_ строк)",
            "search": "Поиск",
            "paginate": {
                "first": "Первая",
                "last": "Последняя",
                "next": "Следующая",
                "previous": "Предыдущая"
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
            ViewNotification('Ошибка', 'error');
        }
    });
}

function EditTenantSuccess(data) {
    if (data.success) {
        $("#Modal").modal("hide");
        RefreshTenantsTable();
        ViewNotification("Изменения сохранены", "success");
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
            ViewNotification('Ошибка', 'error');
        }
    });
}


$(document).ready(OnTenantsDocumentReady);
